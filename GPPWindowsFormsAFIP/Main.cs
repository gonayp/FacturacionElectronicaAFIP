using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using Npgsql;
using System.Xml.Linq;
using System.Threading;
using GPPWindowsFormsAFIP.AFIP.TEST.WSFE;

namespace GPPWindowsFormsAFIP
{
    public partial class Main : Form
    {


        private DataSet ds = new DataSet();
        private DataTable dt = new DataTable();
        public static NpgsqlConnection connection;

        //CONFIG BD
        public static String ApplicationName = "";
        public static String Host = "";
        public static int Port = 1;
        public static String Username = "";
        public static String Password = "";
        public static String Database = "";
        public static bool Pooling = false;

        //CONFIG AFIP
        public static String CLAVE = "";
        public static String SERVICIO = "";
        public static String CERTIFICADO = "";
        public static String URL_WSAA_test = "";
        public static String URL_WSAA = "";
        public static String URL_WSFE = "";
        public static String URL_WSFE_test = "";
        public static X509Certificate2 certificado = new X509Certificate2();

        //Otras configuraciones
        public static long CUIT = 0;
        public static string LogUrl = "";
        public static int Testing = 1;


        //Respuesta login Afip
        public static String TOKEN = null;
        public static String SING = null;
        public static DateTime EXPIRATION;

        //Datos de AFIP
        public FEPtoVentaResponse puntosventa;
        public CbteTipoResponse TiposComprobantes;
        public ConceptoTipoResponse TipoConceptos;
        public DocTipoResponse TipoDoc;
        public MonedaResponse Monedas;
        public IvaTipoResponse TiposIVA;
        public OpcionalTipoResponse opcionales;
        public static FEAuthRequest authRequest;

        Thread thread;

        

        public Main()
        {
            InitializeComponent();

            MetodosGenerales.leerXML();//Inicializa config de BD y afip

            String a = "";
            if (Testing == 1) a = "TESTING";

            LogHelper.Log(LogTarget.File, DateTime.Now.ToString() + " - Inicio de programa. " +a);

            MetodosGenerales.crearCertificado();

            connection = MetodosBD.ConnectRemote(ApplicationName, Host, Port, Username, Password, Database, Pooling);//Conexion con BD

            MetodosGenerales.leerLoginAnteriores();//Leer si existe algun login anteiror
            if(EXPIRATION <= DateTime.Now)
                cargarDatosAfip();

            //Linea en segundo plano para hacer facturacion y login si fuese necesario
            thread = new Thread(backgroundWork);
            thread.Start();


        }

       


        private void backgroundWork()
        {
            DateTime ahora;

            while (true)
            {
                SetText(EXPIRATION.ToString());//cambia fecha de expiracion que se muestra en el form
                

                ahora = DateTime.Now;
                while (EXPIRATION >= ahora)
                {

                    mostrarComprobantes();//Muestra comprobantes pendientes en el form
                    buscar_facturas();//Busca facturas pendientes y las manda para afip


                    ahora = DateTime.Now;
                }

                MetodosGenerales.hacer_login();
                //cargarDatosAfip();
            }
        }
        

        private void cargarDatosAfip()
        {
            try
            {
                authRequest = new FEAuthRequest();
                authRequest.Cuit = CUIT;
                authRequest.Sign = SING;
                authRequest.Token = TOKEN;

                AFIP.TEST.WSFE.Service service = new AFIP.TEST.WSFE.Service();
                if (Main.Testing == 1)
                    service.Url = "https://wswhomo.afip.gov.ar/wsfev1/service.asmx?WSDL";// URL;
                else //Produccion
                    service.Url = "https://servicios1.afip.gov.ar/wsfev1/service.asmx?WSDL";
                service.ClientCertificates.Add(certificado);

                ptos_venta_cm.DisplayMember = "Nro";
                TiposComprobantesCMB.DisplayMember = "Desc";
                TipoConcepto.DisplayMember = "Desc";
                TipoDocCMB.DisplayMember = "Desc";
                MonedaCMB.DisplayMember = "Desc";
                TipoIVACmb.DisplayMember = "Desc";


                puntosventa = service.FEParamGetPtosVenta(authRequest);
                ptos_venta_cm.DataSource = puntosventa.ResultGet;

                TiposComprobantes = service.FEParamGetTiposCbte(authRequest);
                TiposComprobantesCMB.DataSource = TiposComprobantes.ResultGet;

                TipoConceptos = service.FEParamGetTiposConcepto(authRequest);
                TipoConcepto.DataSource = TipoConceptos.ResultGet;

                TipoDoc = service.FEParamGetTiposDoc(authRequest);
                TipoDocCMB.DataSource = TipoDoc.ResultGet;

                Monedas = service.FEParamGetTiposMonedas(authRequest);
                MonedaCMB.DataSource = Monedas.ResultGet;

                TiposIVA = service.FEParamGetTiposIva(authRequest);
                TipoIVACmb.DataSource = TiposIVA.ResultGet;

                //ultimo numero
                //var lastCbteObj = service.FECompUltimoAutorizado(authRequest, 4, TiposComprobantes.ResultGet[0].Id);
                //NroCbteTX.Text = lastCbteObj.CbteNro + 1;

                opcionales = service.FEParamGetTiposOpcional(authRequest);

            }
            catch(Exception e)
            {
                LogHelper.Log(LogTarget.File, DateTime.Now.ToString() + " - ERROR Datos AFIP: " + e.Message + " - " + e.StackTrace);
                //MessageBox.Show(e.Message);
            }
        }


        private void buscar_facturas()
        {
            //Dormimos 2 segundos el proceso para no hacer muchas llamadas
            int milliseconds = 2000;
            Thread.Sleep(milliseconds);

            MetodosGenerales.leerComprobantes();
        }



        protected override void OnClosed(EventArgs e)
        {
            thread.Abort();
            LogHelper.Log(LogTarget.File, DateTime.Now.ToString() + " - Cierre de programa.");
            base.OnClosed(e);
        }





        // Usando delegados y funciones "callback"
        //
        // Este delegado define un método void
        // que recibe un parámetro de tipo string
        delegate void SetTextCallback(String text);

        private void SetText(String text)
        {
            if (lb_expiration.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                try
                {
                    lb_expiration.Text = EXPIRATION.ToString();
                }
                catch
                {
                    
                }
            }
        }



        delegate void mostrarComprobantesCallback();

        private void mostrarComprobantes()
        {


            string sql = "SELECT comp_fecha, comp_pv, comp_codigo, comp_total, comp_doc, comp_estado FROM afip_comprobante order by comp_fecha desc";
            // data adapter making request from our connection
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, connection);
            //Mostramos la tabla en el formulario
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];

            if (dataGridView1.InvokeRequired)
                dataGridView1.Invoke(new EventHandler(delegate
                {
                    dataGridView1.DataSource = dt;
                }));
            else dataGridView1.DataSource = dt;



            
        }


        private void Btn_read_Click(object sender, EventArgs e)
        {
            cargarDatosAfip();
        }
    }


}
