namespace Leitor_Esocial
{
    partial class ModalConfig
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
            this.btn_certificado_windows = new System.Windows.Forms.Button();
            this.btn_certificado_arquivo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_certificado_windows
            // 
            this.btn_certificado_windows.Location = new System.Drawing.Point(39, 19);
            this.btn_certificado_windows.Name = "btn_certificado_windows";
            this.btn_certificado_windows.Size = new System.Drawing.Size(363, 27);
            this.btn_certificado_windows.TabIndex = 0;
            this.btn_certificado_windows.Text = "Usar certificado instalado no Windows";
            this.btn_certificado_windows.UseVisualStyleBackColor = true;
            this.btn_certificado_windows.Click += new System.EventHandler(this.btn_certificado_windows_Click);
            // 
            // btn_certificado_arquivo
            // 
            this.btn_certificado_arquivo.Location = new System.Drawing.Point(39, 64);
            this.btn_certificado_arquivo.Name = "btn_certificado_arquivo";
            this.btn_certificado_arquivo.Size = new System.Drawing.Size(363, 28);
            this.btn_certificado_arquivo.TabIndex = 1;
            this.btn_certificado_arquivo.Text = "Usar certificado em arquivo";
            this.btn_certificado_arquivo.UseVisualStyleBackColor = true;
            this.btn_certificado_arquivo.Click += new System.EventHandler(this.btn_certificado_arquivo_Click);
            // 
            // ModalConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(432, 105);
            this.Controls.Add(this.btn_certificado_arquivo);
            this.Controls.Add(this.btn_certificado_windows);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ModalConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ModalConfig";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_certificado_windows;
        private System.Windows.Forms.Button btn_certificado_arquivo;
    }
}