namespace ProcesadorNominaas
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnUbicacion = new System.Windows.Forms.Button();
            this.btnProcesar = new System.Windows.Forms.Button();
            this.lblUbi = new System.Windows.Forms.Label();
            this.txtRuta = new System.Windows.Forms.TextBox();
            this.pbxSuccess = new System.Windows.Forms.PictureBox();
            this.pbxLoading = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbxSuccess)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxLoading)).BeginInit();
            this.SuspendLayout();
            // 
            // btnUbicacion
            // 
            this.btnUbicacion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnUbicacion.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUbicacion.ForeColor = System.Drawing.Color.White;
            this.btnUbicacion.Location = new System.Drawing.Point(252, 88);
            this.btnUbicacion.Name = "btnUbicacion";
            this.btnUbicacion.Size = new System.Drawing.Size(664, 112);
            this.btnUbicacion.TabIndex = 0;
            this.btnUbicacion.Text = "ESTABLECE LA UBICACION DE LOS ARCHIVOS";
            this.btnUbicacion.UseVisualStyleBackColor = false;
            this.btnUbicacion.Click += new System.EventHandler(this.btnUbicacion_Click);
            // 
            // btnProcesar
            // 
            this.btnProcesar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnProcesar.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProcesar.ForeColor = System.Drawing.Color.White;
            this.btnProcesar.Location = new System.Drawing.Point(428, 341);
            this.btnProcesar.Name = "btnProcesar";
            this.btnProcesar.Size = new System.Drawing.Size(308, 79);
            this.btnProcesar.TabIndex = 1;
            this.btnProcesar.Text = "Procesa archivos.";
            this.btnProcesar.UseVisualStyleBackColor = false;
            this.btnProcesar.Click += new System.EventHandler(this.btnProcesar_Click);
            // 
            // lblUbi
            // 
            this.lblUbi.AutoSize = true;
            this.lblUbi.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUbi.Location = new System.Drawing.Point(40, 244);
            this.lblUbi.Name = "lblUbi";
            this.lblUbi.Size = new System.Drawing.Size(212, 29);
            this.lblUbi.TabIndex = 2;
            this.lblUbi.Text = "Ubicación actual:";
            // 
            // txtRuta
            // 
            this.txtRuta.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRuta.Location = new System.Drawing.Point(258, 244);
            this.txtRuta.Name = "txtRuta";
            this.txtRuta.Size = new System.Drawing.Size(874, 28);
            this.txtRuta.TabIndex = 3;
            // 
            // pbxSuccess
            // 
            this.pbxSuccess.Image = global::ProcesadorNominaas.Properties.Resources._6_2_success_png_image;
            this.pbxSuccess.Location = new System.Drawing.Point(828, 341);
            this.pbxSuccess.Name = "pbxSuccess";
            this.pbxSuccess.Size = new System.Drawing.Size(139, 79);
            this.pbxSuccess.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbxSuccess.TabIndex = 5;
            this.pbxSuccess.TabStop = false;
            this.pbxSuccess.Visible = false;
            // 
            // pbxLoading
            // 
            this.pbxLoading.Image = global::ProcesadorNominaas.Properties.Resources.Loading_icon;
            this.pbxLoading.Location = new System.Drawing.Point(828, 341);
            this.pbxLoading.Name = "pbxLoading";
            this.pbxLoading.Size = new System.Drawing.Size(139, 79);
            this.pbxLoading.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbxLoading.TabIndex = 4;
            this.pbxLoading.TabStop = false;
            this.pbxLoading.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1178, 530);
            this.Controls.Add(this.pbxSuccess);
            this.Controls.Add(this.pbxLoading);
            this.Controls.Add(this.txtRuta);
            this.Controls.Add(this.lblUbi);
            this.Controls.Add(this.btnProcesar);
            this.Controls.Add(this.btnUbicacion);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbxSuccess)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxLoading)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnUbicacion;
        private System.Windows.Forms.Button btnProcesar;
        private System.Windows.Forms.Label lblUbi;
        private System.Windows.Forms.TextBox txtRuta;
        private System.Windows.Forms.PictureBox pbxLoading;
        private System.Windows.Forms.PictureBox pbxSuccess;
    }
}

