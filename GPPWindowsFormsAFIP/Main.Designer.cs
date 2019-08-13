namespace GPPWindowsFormsAFIP
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_read = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.lb_test = new System.Windows.Forms.Label();
            this.lb_expiration = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.TipoIVACmb = new System.Windows.Forms.ComboBox();
            this.MonedaCMB = new System.Windows.Forms.ComboBox();
            this.TipoDocCMB = new System.Windows.Forms.ComboBox();
            this.Label7 = new System.Windows.Forms.Label();
            this.TipoConcepto = new System.Windows.Forms.ComboBox();
            this.Label6 = new System.Windows.Forms.Label();
            this.ptos_venta_cm = new System.Windows.Forms.ComboBox();
            this.Label19 = new System.Windows.Forms.Label();
            this.Label18 = new System.Windows.Forms.Label();
            this.TiposComprobantesCMB = new System.Windows.Forms.ComboBox();
            this.Label16 = new System.Windows.Forms.Label();
            this.Label15 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_read
            // 
            this.btn_read.Location = new System.Drawing.Point(1094, 5);
            this.btn_read.Name = "btn_read";
            this.btn_read.Size = new System.Drawing.Size(75, 23);
            this.btn_read.TabIndex = 0;
            this.btn_read.Text = "leer";
            this.btn_read.UseVisualStyleBackColor = true;
            this.btn_read.Click += new System.EventHandler(this.Btn_read_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(40, 37);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(829, 329);
            this.dataGridView1.TabIndex = 1;
            // 
            // lb_test
            // 
            this.lb_test.AutoSize = true;
            this.lb_test.Location = new System.Drawing.Point(885, 9);
            this.lb_test.Name = "lb_test";
            this.lb_test.Size = new System.Drawing.Size(36, 13);
            this.lb_test.TabIndex = 3;
            this.lb_test.Text = "Login:";
            // 
            // lb_expiration
            // 
            this.lb_expiration.AutoSize = true;
            this.lb_expiration.Location = new System.Drawing.Point(927, 9);
            this.lb_expiration.Name = "lb_expiration";
            this.lb_expiration.Size = new System.Drawing.Size(0, 13);
            this.lb_expiration.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(37, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Comprobantes pendientes:";
            // 
            // TipoIVACmb
            // 
            this.TipoIVACmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TipoIVACmb.FormattingEnabled = true;
            this.TipoIVACmb.Location = new System.Drawing.Point(945, 171);
            this.TipoIVACmb.Name = "TipoIVACmb";
            this.TipoIVACmb.Size = new System.Drawing.Size(224, 21);
            this.TipoIVACmb.TabIndex = 12;
            // 
            // MonedaCMB
            // 
            this.MonedaCMB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MonedaCMB.FormattingEnabled = true;
            this.MonedaCMB.Location = new System.Drawing.Point(945, 144);
            this.MonedaCMB.Name = "MonedaCMB";
            this.MonedaCMB.Size = new System.Drawing.Size(224, 21);
            this.MonedaCMB.TabIndex = 13;
            // 
            // TipoDocCMB
            // 
            this.TipoDocCMB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TipoDocCMB.FormattingEnabled = true;
            this.TipoDocCMB.Location = new System.Drawing.Point(945, 115);
            this.TipoDocCMB.Name = "TipoDocCMB";
            this.TipoDocCMB.Size = new System.Drawing.Size(224, 21);
            this.TipoDocCMB.TabIndex = 14;
            // 
            // Label7
            // 
            this.Label7.AutoSize = true;
            this.Label7.Location = new System.Drawing.Point(915, 174);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(24, 13);
            this.Label7.TabIndex = 6;
            this.Label7.Text = "IVA";
            // 
            // TipoConcepto
            // 
            this.TipoConcepto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TipoConcepto.FormattingEnabled = true;
            this.TipoConcepto.Location = new System.Drawing.Point(945, 88);
            this.TipoConcepto.Name = "TipoConcepto";
            this.TipoConcepto.Size = new System.Drawing.Size(224, 21);
            this.TipoConcepto.TabIndex = 15;
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.Location = new System.Drawing.Point(885, 147);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(46, 13);
            this.Label6.TabIndex = 7;
            this.Label6.Text = "Moneda";
            // 
            // ptos_venta_cm
            // 
            this.ptos_venta_cm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ptos_venta_cm.FormattingEnabled = true;
            this.ptos_venta_cm.Location = new System.Drawing.Point(945, 34);
            this.ptos_venta_cm.Name = "ptos_venta_cm";
            this.ptos_venta_cm.Size = new System.Drawing.Size(224, 21);
            this.ptos_venta_cm.TabIndex = 16;
            // 
            // Label19
            // 
            this.Label19.AutoSize = true;
            this.Label19.Location = new System.Drawing.Point(885, 118);
            this.Label19.Name = "Label19";
            this.Label19.Size = new System.Drawing.Size(51, 13);
            this.Label19.TabIndex = 8;
            this.Label19.Text = "Tipo Doc";
            // 
            // Label18
            // 
            this.Label18.AutoSize = true;
            this.Label18.Location = new System.Drawing.Point(885, 91);
            this.Label18.Name = "Label18";
            this.Label18.Size = new System.Drawing.Size(53, 13);
            this.Label18.TabIndex = 9;
            this.Label18.Text = "Concepto";
            // 
            // TiposComprobantesCMB
            // 
            this.TiposComprobantesCMB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TiposComprobantesCMB.FormattingEnabled = true;
            this.TiposComprobantesCMB.Location = new System.Drawing.Point(945, 60);
            this.TiposComprobantesCMB.Name = "TiposComprobantesCMB";
            this.TiposComprobantesCMB.Size = new System.Drawing.Size(224, 21);
            this.TiposComprobantesCMB.TabIndex = 17;
            // 
            // Label16
            // 
            this.Label16.AutoSize = true;
            this.Label16.Location = new System.Drawing.Point(885, 63);
            this.Label16.Name = "Label16";
            this.Label16.Size = new System.Drawing.Size(28, 13);
            this.Label16.TabIndex = 10;
            this.Label16.Text = "Tipo";
            // 
            // Label15
            // 
            this.Label15.AutoSize = true;
            this.Label15.Location = new System.Drawing.Point(885, 37);
            this.Label15.Name = "Label15";
            this.Label15.Size = new System.Drawing.Size(54, 13);
            this.Label15.TabIndex = 11;
            this.Label15.Text = "Pto Venta";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1181, 450);
            this.Controls.Add(this.TipoIVACmb);
            this.Controls.Add(this.MonedaCMB);
            this.Controls.Add(this.TipoDocCMB);
            this.Controls.Add(this.Label7);
            this.Controls.Add(this.TipoConcepto);
            this.Controls.Add(this.Label6);
            this.Controls.Add(this.ptos_venta_cm);
            this.Controls.Add(this.Label19);
            this.Controls.Add(this.Label18);
            this.Controls.Add(this.TiposComprobantesCMB);
            this.Controls.Add(this.Label16);
            this.Controls.Add(this.Label15);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lb_expiration);
            this.Controls.Add(this.lb_test);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btn_read);
            this.Name = "Main";
            this.Text = "Main";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_read;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label lb_test;
        private System.Windows.Forms.Label lb_expiration;
        private System.Windows.Forms.Label label1;
        internal System.Windows.Forms.ComboBox TipoIVACmb;
        internal System.Windows.Forms.ComboBox MonedaCMB;
        internal System.Windows.Forms.ComboBox TipoDocCMB;
        internal System.Windows.Forms.Label Label7;
        internal System.Windows.Forms.ComboBox TipoConcepto;
        internal System.Windows.Forms.Label Label6;
        internal System.Windows.Forms.ComboBox ptos_venta_cm;
        internal System.Windows.Forms.Label Label19;
        internal System.Windows.Forms.Label Label18;
        internal System.Windows.Forms.ComboBox TiposComprobantesCMB;
        internal System.Windows.Forms.Label Label16;
        internal System.Windows.Forms.Label Label15;
    }
}