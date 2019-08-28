using Npgsql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using GPPWindowsFormsAFIP.AFIP.TEST.WSFE;

namespace GPPWindowsFormsAFIP
{
    public static class MetodosGenerales
    {

        public static string mensaje_afip = "";
        public static string mensaje_obs = "";
        public static string mensaje_error = "";

        public static long numero_comprobante;
        public static string cae_comprobante;
        public static string cae_vencimiento;
        public static string resultado;



       

        public static void leerXML()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("c:\\gpp\\config.xml");
            XmlNodeList BDconfig = xmlDoc.DocumentElement.SelectNodes("/Table/BD");

            foreach (XmlNode node in BDconfig)
            {
                Main.ApplicationName = node.SelectSingleNode("ApplicationName").InnerText;
                Main.Host = node.SelectSingleNode("Host").InnerText;
                Main.Port = int.Parse(node.SelectSingleNode("Port").InnerText);
                Main.Username = node.SelectSingleNode("Username").InnerText;
                Main.Password = node.SelectSingleNode("Password").InnerText;
                Main.Database = node.SelectSingleNode("Database").InnerText;
                //Pooling = int.Parse(node.SelectSingleNode("Pooling").InnerText);

            }

            XmlNodeList AFIPconfig = xmlDoc.DocumentElement.SelectNodes("/Table/AFIP");

            foreach (XmlNode node in AFIPconfig)
            {
                Main.CLAVE = node.SelectSingleNode("Clave").InnerText;
                Main.SERVICIO = node.SelectSingleNode("Servicio").InnerText;
                Main.CERTIFICADO = node.SelectSingleNode("Certificado").InnerText;
                Main.URL_WSAA_test = node.SelectSingleNode("Url_wsaa_test").InnerText;
                Main.URL_WSAA = node.SelectSingleNode("Url_wsaa").InnerText;
                Main.URL_WSFE_test = node.SelectSingleNode("Url_wsfe_test").InnerText;
                Main.URL_WSFE = node.SelectSingleNode("Url_wsfe").InnerText;

            }

            XmlNodeList OtrasConfig = xmlDoc.DocumentElement.SelectNodes("/Table/OTROS");

            foreach (XmlNode node in OtrasConfig)
            {
                Main.CUIT = long.Parse( node.SelectSingleNode("Cuit").InnerText);
                Main.LogUrl = node.SelectSingleNode("Log").InnerText;
                Main.Testing = int.Parse(node.SelectSingleNode("Testing").InnerText);
                //Main.URL = node.SelectSingleNode("Url").InnerText;

            }

        }

        public static void leerLoginAnteriores(/*object sender, LinkLabelLinkClickedEventArgs e*/)
        {
            try
            {
                NpgsqlConnection connection = Main.connection;

                string sql = "SELECT * FROM afip_login order by afip_expiration desc";
                // data adapter making request from our connection
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, connection);
                NpgsqlCommand cmd = new NpgsqlCommand(sql, connection);
                
                using (var reader = cmd.ExecuteReader())
                {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            Main.TOKEN = reader.GetString(reader.GetOrdinal("afip_token"));
                            Main.SING = reader.GetString(reader.GetOrdinal("afip_sing"));
                            Main.EXPIRATION = reader.GetDateTime(reader.GetOrdinal("afip_expiration"));
                        }
                    else
                    {
                        Main.EXPIRATION = Convert.ToDateTime("01/01/1900");
                    }
                    //var i = 0;
                    //while (reader.Read())
                    //{
                    //int id = reader.GetInt32(reader.GetOrdinal("banco_id"));
                    //string banco_nombre = reader.GetString(reader.GetOrdinal("banco_nombre"));

                    //MessageBox.Show(expiration + ": " + token);
                    //}
                }


            }
            catch (Exception msg)
            {
                // something went wrong, and you wanna know why
                LogHelper.Log(LogTarget.File, DateTime.Now.ToString() + " - ERROR: " + msg.Message + " - " + msg.StackTrace);
                //MessageBox.Show(msg.ToString());
                Main.EXPIRATION = Convert.ToDateTime("01/01/1900");
                throw;
            }
        }


        public static void leerComprobantes()
        {

            int id = 0;
            int pv = 0;
            int codigo = 0 ;
            int estado;
            double importe = 0;
            double iva = 0;
            double total = 0;
            long doc = 0;
            int concepto = 0;
            string moneda = "";
            int tipo_iva = 0;
            int tipo_doc = 0;

            DateTime fecha = new DateTime();

            try
            {
                NpgsqlConnection connection = Main.connection;

                string sql = "SELECT * FROM afip_comprobante where comp_estado = 1 order by comp_fecha asc";
                // data adapter making request from our connection
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, connection);
                NpgsqlCommand cmd = new NpgsqlCommand(sql, connection);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        pv = reader.GetInt32(reader.GetOrdinal("comp_pv"));
                        codigo = reader.GetInt32(reader.GetOrdinal("comp_codigo"));
                        estado = reader.GetInt32(reader.GetOrdinal("comp_estado"));
                        id = reader.GetInt32(reader.GetOrdinal("id"));
                        importe = reader.GetDouble(reader.GetOrdinal("comp_importe"));
                        iva = reader.GetDouble(reader.GetOrdinal("comp_iva"));
                        total = reader.GetDouble(reader.GetOrdinal("comp_total"));
                        doc = reader.GetInt64(reader.GetOrdinal("comp_doc"));
                        fecha = reader.GetDateTime(reader.GetOrdinal("comp_fecha"));
                        moneda = reader.GetString(reader.GetOrdinal("comp_moneda"));
                        tipo_doc = reader.GetInt32(reader.GetOrdinal("tipo_doc"));
                        tipo_iva = reader.GetInt32(reader.GetOrdinal("tipo_iva"));
                        concepto = reader.GetInt32(reader.GetOrdinal("concepto"));

                    }

                    //MessageBox.Show(pv + "-"+ codigo+": " + importe);

                   
                }


            }
            catch (Exception msg)
            {
                // something went wrong, and you wanna know why
                LogHelper.Log(LogTarget.File, DateTime.Now.ToString() + " - ERROR: " + msg.Message + " - " + msg.StackTrace);
                //MessageBox.Show(msg.ToString());
                throw;
            }


            if (id != 0)
            {


                //consultar Afip
                string respuesta = consultarAfip(pv,codigo,importe,iva,total,fecha,doc,concepto,tipo_doc,tipo_iva,moneda);
                /*
                for (int i = 0; i <= 10000; i++)
                {
                    for (int j = 0; j <= 1000; j++)
                    {
                        for (int k = 0; k <= 1000; k++)
                        {
                            k++;
                        }
                    }
                }*/

                //string respuesta = "respuesta";

                if (respuesta != "ERROR")
                {
                    //Escribir registro BD
                    if (resultado != "R")
                    {
                        MetodosBD.modificarComprobante(id, 2, respuesta);
                    }
                    else
                    {
                        MetodosBD.modificarComprobante(id, 3, respuesta);
                    }
                }

            }

        }

        private static string consultarAfip(int p_pv, int p_codigo, double p_importe, double p_iva, double p_total, DateTime p_fecha, long doc, int concepto,int tipo_doc,int tipo_iva, string moneda)
        {
            numero_comprobante = 0;
            cae_comprobante = "";
            cae_vencimiento ="";
            mensaje_afip = "";
            mensaje_obs = "";
            mensaje_error = "";
            string m = "";
            resultado = "R";

            try
            {
                if (Main.authRequest == null)
                {
                    Main.authRequest = new FEAuthRequest();
                    Main.authRequest.Cuit = Main.CUIT;
                    Main.authRequest.Sign = Main.SING;
                    Main.authRequest.Token = Main.TOKEN;
                }

                AFIP.TEST.WSFE.Service service = getServicio();
                service.ClientCertificates.Add(Main.certificado);

                /*
                if (Main.Testing == 1)
                {
                    AFIP.TEST.WSFE.Service service = getServicioTest();
                    service.ClientCertificates.Add(Main.certificado);
                }
                else
                {
                    AFIP.WSFE.Service service = getServicioProducion();
                    service.ClientCertificates.Add(Main.certificado);
                }*/
                //PtoVenta pvObj = ptos_venta_cm.SelectedItem;
                //int pv = p_pv;//pvObj.Nro;
                //CbteTipo cm = p_codigo;//TiposComprobantesCMB.SelectedItem;

                FECAERequest req = new FECAERequest();
                FECAECabRequest cab = new FECAECabRequest();
                FECAEDetRequest det = new FECAEDetRequest();

                cab.CantReg = 1;
                cab.PtoVta = p_pv;
                cab.CbteTipo = p_codigo;//cm.Id;
                req.FeCabReq = cab;

                FECAEDetRequest bar = det;
                //ConceptoTipo concepto = TipoConcepto.SelectedItem;
                bar.Concepto = concepto;//concepto.ID;
                //DocTipo doctipo = TipoDocCMB.SelectedItem;
                bar.DocTipo = tipo_doc;//doctipo.Id
                bar.DocNro = doc;//long.Parse(DocTX.Text)

                //Buscar el ultimo numero
                try
                {
                    FERecuperaLastCbteResponse lastRes = service.FECompUltimoAutorizado(Main.authRequest, p_pv, p_codigo);
                    int last = lastRes.CbteNro;

                    bar.CbteDesde = last + 1;
                    bar.CbteHasta = last + 1;
                }
                catch
                {
                    bar.CbteDesde = p_codigo;
                    bar.CbteHasta = p_codigo;
                }

                bar.CbteFch = p_fecha.ToString("yyyyMMdd");//FechaDTP.Value.ToString("yyyyMMdd");

                bar.ImpNeto = Double.Parse(p_importe.ToString("0.00"));//NetoTX.Text;
                bar.ImpIVA = Double.Parse(p_iva.ToString("0.00"));// ImpIvaTx.Text;
                bar.ImpTotal = Double.Parse(p_total.ToString("0.00"));// TotalTx.Text;

                bar.ImpTotConc = 0;
                bar.ImpOpEx = 0;
                bar.ImpTrib = 0;

                //Moneda mon = MonedaCMB.SelectedItem;
                bar.MonId = moneda;// mon.Id
                bar.MonCotiz = 1;

                AlicIva alicuota = new AlicIva();
                //IvaTipo ivat = TipoIVACmb.SelectedItem;
                alicuota.Id = tipo_iva;// ivat.Id
                alicuota.BaseImp = p_importe;// NetoTX.Text
                alicuota.Importe = Double.Parse(p_iva.ToString("0.00"));// ImpIvaTx.Text

                bar.Iva = new AlicIva[] { alicuota};
                //bar.Iva.Append(alicuota);

                req.FeDetReq = new FECAEDetRequest[] { bar };
                //req.FeDetReq.Append(bar);

                
                try
                {
                    

                    FECAEResponse r = service.FECAESolicitar(Main.authRequest, req);
                    string vbCrLf = "\n";

                    resultado = r.FeDetResp[0].Resultado;

                    m = "Estado Cabecera: " + r.FeCabResp.Resultado + vbCrLf;
                    m += "Estado Detalle: " + r.FeDetResp[0].Resultado;
                    m += vbCrLf;
                    m += "CAE: " + r.FeDetResp[0].CAE;
                    m += vbCrLf;
                    m += "Vto: " + r.FeDetResp[0].CAEFchVto;
                    m += vbCrLf;
                    m += "Numero de comprobante: " + r.FeDetResp[0].CbteDesde ;
                    m += vbCrLf;

                    numero_comprobante = r.FeDetResp[0].CbteDesde;
                    cae_comprobante = r.FeDetResp[0].CAE;
                    cae_vencimiento = r.FeDetResp[0].CAEFchVto;

                    if (r.FeDetResp[0].Observaciones != null)
                    {
                        foreach (var o in r.FeDetResp[0].Observaciones)
                        {
                            mensaje_obs += string.Format("Obs: {0} ({1})", o.Msg, o.Code) + vbCrLf;
                            m += mensaje_obs;
                        }
                    }
                    if (r.Errors != null)
                    {
                        foreach (var er in r.Errors)
                        {
                            mensaje_error += string.Format("Er: {0}: {1}", er.Code, er.Msg) + vbCrLf;
                            m += mensaje_error;
                        }
                    }
                    if (r.Events != null)
                    {
                        foreach (var ev in r.Events)
                        {
                            mensaje_afip += string.Format("Ev: {0}: {1}", ev.Code, ev.Msg) + vbCrLf;
                            m += mensaje_afip;
                        }
                    }
                }
                catch(Exception e)
                {
                    LogHelper.Log(LogTarget.File, DateTime.Now.ToString() + " - ERROR COMPROBANTE AFIP: " + e.Message + " - " + e.StackTrace);
                    m = "ERROR";
                    return m;
                }


                

                //Resultado.Text = m
                LogHelper.Log(LogTarget.File, DateTime.Now.ToString() + " - Consultando en AFIP: " + resultado);


            }
            catch(Exception e)
            {
                LogHelper.Log(LogTarget.File, DateTime.Now.ToString() + " - ERROR COMPROBANTE AFIP: " + e.Message + " - " + e.StackTrace);
                //MessageBox.Show("ERROR: "+e.Message+"\n"+e.StackTrace);
            }

            return m;
        }

        private static AFIP.WSFE.Service getServicioProducion()
        {
            AFIP.WSFE.Service s = new AFIP.WSFE.Service();
            s.Url = "https://servicios1.afip.gov.ar/wsfev1/service.asmx?WSDL";// Main.URL;
            
            return s;
        }

        private static Service getServicioTest()
        {
            Service s = new Service();
            s.Url = "https://wswhomo.afip.gov.ar/wsfev1/service.asmx?WSDL";// Main.URL;
            
            return s;
        }

        private static Service getServicio()
        {
            Service s = new Service();
            if(Main.Testing == 1)
                s.Url = "https://wswhomo.afip.gov.ar/wsfev1/service.asmx?WSDL";
            else
                s.Url = "https://servicios1.afip.gov.ar/wsfev1/service.asmx?WSDL";

            return s;
        }

        public static  void hacer_login()
        {

            string cmsFirmadoBase64;
            string loginTicketResponse;

            XmlNode uniqueIdNode;
            XmlNode generationTimeNode;
            XmlNode ExpirationTimeNode;
            XmlNode ServiceNode;


            //String clave_o = Main.CLAVE;
            //String cert_path = Main.CERTIFICADO;
            String servicio_o = Main.SERVICIO;
            String url_testing = Main.URL_WSAA_test;
            String url_produccion = Main.URL_WSAA;

            try
            {
                //this._globalId += 1;

                // Preparo el XML Request
                XmlDocument XmlLoginTicketRequest = new XmlDocument();
                XmlLoginTicketRequest.Load("LoginTemplate");

                DateTime ahora = DateTime.Now;

                uniqueIdNode = XmlLoginTicketRequest.SelectSingleNode("//uniqueId");
                generationTimeNode = XmlLoginTicketRequest.SelectSingleNode("//generationTime");
                ExpirationTimeNode = XmlLoginTicketRequest.SelectSingleNode("//expirationTime");
                ServiceNode = XmlLoginTicketRequest.SelectSingleNode("//service");
                generationTimeNode.InnerText = ahora.AddMinutes(-10).ToString("s");
                ExpirationTimeNode.InnerText = ahora.AddMinutes(+10).ToString("s");
                uniqueIdNode.InnerText = System.Convert.ToString(1);
                ServiceNode.InnerText = servicio_o;


                DateTime fecha_expiracion = ahora.AddMinutes(+10);
                /*
                SecureString clave = new SecureString();
                foreach (char character in clave_o)
                    clave.AppendChar(character);
                clave.MakeReadOnly();

                
                // Obtenemos el Cert
                //X509Certificate2 certificado = new X509Certificate2();
                if (clave.IsReadOnly())
                    Main.certificado.Import(File.ReadAllBytes(cert_path), clave, X509KeyStorageFlags.PersistKeySet);
                else
                    Main.certificado.Import(File.ReadAllBytes(cert_path));
                    */
                byte[] msgBytes = Encoding.UTF8.GetBytes(XmlLoginTicketRequest.OuterXml);

                // Firmamos
                ContentInfo infoContenido = new ContentInfo(msgBytes);
                SignedCms cmsFirmado = new SignedCms(infoContenido);

                CmsSigner cmsFirmante = new CmsSigner(Main.certificado);
                cmsFirmante.IncludeOption = X509IncludeOption.EndCertOnly;

                cmsFirmado.ComputeSignature(cmsFirmante);

                cmsFirmadoBase64 = Convert.ToBase64String(cmsFirmado.Encode());

                // Hago el login
                if (Main.Testing == 1)
                {
                    AFIP.TEST.WSAA.LoginCMSService servicio = new AFIP.TEST.WSAA.LoginCMSService();
                    servicio.Url = url_testing;

                    loginTicketResponse = servicio.loginCms(cmsFirmadoBase64);
                }
                else
                {
                    AFIP.WSAA.LoginCMSService servicio = new AFIP.WSAA.LoginCMSService();
                    servicio.Url = url_produccion;

                    loginTicketResponse = servicio.loginCms(cmsFirmadoBase64);
                }

                // Analizamos la respuesta
                XmlDocument XmlLoginTicketResponse = new XmlDocument();
                XmlLoginTicketResponse.LoadXml(loginTicketResponse);

                String _Token = XmlLoginTicketResponse.SelectSingleNode("//token").InnerText;
                String _Sign = XmlLoginTicketResponse.SelectSingleNode("//sign").InnerText;
                //My.Settings.def_token = _Token;
                //My.Settings.def_sing = _Sign;

                var exStr = XmlLoginTicketResponse.SelectSingleNode("//expirationTime").InnerText;
                var genStr = XmlLoginTicketResponse.SelectSingleNode("//generationTime").InnerText;
                DateTime ExpirationTime = DateTime.Parse(exStr);
                DateTime GenerationTime = DateTime.Parse(genStr);
                // My.Settings.def_expiration = ExpirationTime;
                // My.Settings.Save();

                MetodosBD.insertarLogin(_Token, _Sign, GenerationTime, fecha_expiracion/*ExpirationTime*/);

                //XDocRequest = XDocument.Parse(XmlLoginTicketRequest.OuterXml);
                //XDocResponse = XDocument.Parse(XmlLoginTicketResponse.OuterXml);

                //MessageBox.Show(ExpirationTime.ToString());
                LogHelper.Log(LogTarget.File, DateTime.Now.ToString() + " Nuevo login. Exiration: " + ExpirationTime.ToString());
            }

            // MsgBox("Exito"

            catch (Exception ex)
            {
                //Interaction.MsgBox(ex.Message);
                //MessageBox.Show(ex.Message);
                LogHelper.Log(LogTarget.File, DateTime.Now.ToString() + " - ERROR LOGIN AFIP: " + ex.Message);
            }


        }


        public static void crearCertificado()
        {
            String cert_path = Main.CERTIFICADO;
            String clave_o = Main.CLAVE;

            SecureString clave = new SecureString();
            foreach (char character in clave_o)
                clave.AppendChar(character);
            clave.MakeReadOnly();


            // Obtenemos el Cert
            //X509Certificate2 certificado = new X509Certificate2();
            if (clave.IsReadOnly())
                Main.certificado.Import(File.ReadAllBytes(cert_path), clave, X509KeyStorageFlags.PersistKeySet);
            else
                Main.certificado.Import(File.ReadAllBytes(cert_path));
        }

        

        


    }
}
