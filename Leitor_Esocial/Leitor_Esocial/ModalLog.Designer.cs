namespace Leitor_Esocial
{
    partial class ModalLog
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
            this.btn_atualiza = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // btn_atualiza
            // 
            this.btn_atualiza.Location = new System.Drawing.Point(4, 3);
            this.btn_atualiza.Name = "btn_atualiza";
            this.btn_atualiza.Size = new System.Drawing.Size(357, 23);
            this.btn_atualiza.TabIndex = 1;
            this.btn_atualiza.Text = "Atualizar";
            this.btn_atualiza.UseVisualStyleBackColor = true;
            this.btn_atualiza.Click += new System.EventHandler(this.btn_atualiza_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.Color.White;
            this.richTextBox1.Location = new System.Drawing.Point(4, 27);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(357, 358);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = "";
            // 
            // ModalLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(366, 388);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.btn_atualiza);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ModalLog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ModalLog";
            this.Shown += new System.EventHandler(this.ModalLog_Shown);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btn_atualiza;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}