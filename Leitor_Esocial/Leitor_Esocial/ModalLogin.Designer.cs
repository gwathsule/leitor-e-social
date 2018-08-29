namespace Leitor_Esocial
{
    partial class ModalLogin
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
            this.btn_logar = new System.Windows.Forms.Button();
            this.txt_email = new System.Windows.Forms.TextBox();
            this.lbl_email = new System.Windows.Forms.Label();
            this.lbl_documento = new System.Windows.Forms.Label();
            this.txt_documento = new System.Windows.Forms.TextBox();
            this.lbl_senha = new System.Windows.Forms.Label();
            this.txt_senha = new System.Windows.Forms.TextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btn_logar
            // 
            this.btn_logar.Location = new System.Drawing.Point(114, 152);
            this.btn_logar.Name = "btn_logar";
            this.btn_logar.Size = new System.Drawing.Size(163, 31);
            this.btn_logar.TabIndex = 0;
            this.btn_logar.Text = "Logar";
            this.btn_logar.UseVisualStyleBackColor = true;
            this.btn_logar.Click += new System.EventHandler(this.btn_logar_Click);
            // 
            // txt_email
            // 
            this.txt_email.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_email.Location = new System.Drawing.Point(96, 24);
            this.txt_email.Name = "txt_email";
            this.txt_email.Size = new System.Drawing.Size(289, 29);
            this.txt_email.TabIndex = 1;
            this.txt_email.Text = "teste@apicontando.com.br";
            // 
            // lbl_email
            // 
            this.lbl_email.AutoSize = true;
            this.lbl_email.Font = new System.Drawing.Font("Arial", 14.25F);
            this.lbl_email.Location = new System.Drawing.Point(12, 24);
            this.lbl_email.Name = "lbl_email";
            this.lbl_email.Size = new System.Drawing.Size(78, 22);
            this.lbl_email.TabIndex = 2;
            this.lbl_email.Text = "EMAIL: ";
            // 
            // lbl_documento
            // 
            this.lbl_documento.AutoSize = true;
            this.lbl_documento.Font = new System.Drawing.Font("Arial", 14.25F);
            this.lbl_documento.Location = new System.Drawing.Point(12, 62);
            this.lbl_documento.Name = "lbl_documento";
            this.lbl_documento.Size = new System.Drawing.Size(139, 22);
            this.lbl_documento.TabIndex = 4;
            this.lbl_documento.Text = "DOCUMENTO:";
            // 
            // txt_documento
            // 
            this.txt_documento.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_documento.Location = new System.Drawing.Point(157, 59);
            this.txt_documento.Name = "txt_documento";
            this.txt_documento.Size = new System.Drawing.Size(228, 29);
            this.txt_documento.TabIndex = 3;
            this.txt_documento.Text = "10335370000133";
            // 
            // lbl_senha
            // 
            this.lbl_senha.AutoSize = true;
            this.lbl_senha.Font = new System.Drawing.Font("Arial", 14.25F);
            this.lbl_senha.Location = new System.Drawing.Point(12, 97);
            this.lbl_senha.Name = "lbl_senha";
            this.lbl_senha.Size = new System.Drawing.Size(80, 22);
            this.lbl_senha.TabIndex = 6;
            this.lbl_senha.Text = "SENHA:";
            // 
            // txt_senha
            // 
            this.txt_senha.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_senha.Location = new System.Drawing.Point(96, 94);
            this.txt_senha.Name = "txt_senha";
            this.txt_senha.PasswordChar = '*';
            this.txt_senha.Size = new System.Drawing.Size(289, 29);
            this.txt_senha.TabIndex = 5;
            this.txt_senha.Text = "12345678";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(157, 129);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(79, 17);
            this.checkBox1.TabIndex = 7;
            this.checkBox1.Text = "EDUCONT";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // ModalLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(418, 193);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.lbl_senha);
            this.Controls.Add(this.txt_senha);
            this.Controls.Add(this.lbl_documento);
            this.Controls.Add(this.txt_documento);
            this.Controls.Add(this.lbl_email);
            this.Controls.Add(this.txt_email);
            this.Controls.Add(this.btn_logar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ModalLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Login";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_logar;
        private System.Windows.Forms.TextBox txt_email;
        private System.Windows.Forms.Label lbl_email;
        private System.Windows.Forms.Label lbl_documento;
        private System.Windows.Forms.TextBox txt_documento;
        private System.Windows.Forms.Label lbl_senha;
        private System.Windows.Forms.TextBox txt_senha;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}