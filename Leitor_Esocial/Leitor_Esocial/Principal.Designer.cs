﻿namespace Leitor_Esocial
{
    partial class Principal
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Principal));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnAjuda = new System.Windows.Forms.PictureBox();
            this.btnFechar = new System.Windows.Forms.PictureBox();
            this.pnNFeEntrada = new System.Windows.Forms.Panel();
            this.btn_cnf_certificado = new System.Windows.Forms.Button();
            this.eSocialDataGrid = new System.Windows.Forms.DataGridView();
            this.btn_folder_esocial = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.icon_principal = new System.Windows.Forms.NotifyIcon(this.components);
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.panel9 = new System.Windows.Forms.Panel();
            this.panel10 = new System.Windows.Forms.Panel();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnAjuda)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnFechar)).BeginInit();
            this.pnNFeEntrada.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.eSocialDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(108)))), ((int)(((byte)(185)))));
            this.panel1.Controls.Add(this.btnAjuda);
            this.panel1.Controls.Add(this.btnFechar);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(821, 33);
            this.panel1.TabIndex = 1;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseMove);
            this.panel1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseUp);
            // 
            // btnAjuda
            // 
            this.btnAjuda.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAjuda.Image = ((System.Drawing.Image)(resources.GetObject("btnAjuda.Image")));
            this.btnAjuda.Location = new System.Drawing.Point(765, 8);
            this.btnAjuda.Name = "btnAjuda";
            this.btnAjuda.Size = new System.Drawing.Size(19, 22);
            this.btnAjuda.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnAjuda.TabIndex = 3;
            this.btnAjuda.TabStop = false;
            // 
            // btnFechar
            // 
            this.btnFechar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFechar.Image = ((System.Drawing.Image)(resources.GetObject("btnFechar.Image")));
            this.btnFechar.Location = new System.Drawing.Point(790, 8);
            this.btnFechar.Name = "btnFechar";
            this.btnFechar.Size = new System.Drawing.Size(19, 22);
            this.btnFechar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnFechar.TabIndex = 0;
            this.btnFechar.TabStop = false;
            this.btnFechar.Click += new System.EventHandler(this.btnFechar_Click);
            // 
            // pnNFeEntrada
            // 
            this.pnNFeEntrada.Controls.Add(this.btn_cnf_certificado);
            this.pnNFeEntrada.Controls.Add(this.eSocialDataGrid);
            this.pnNFeEntrada.Controls.Add(this.btn_folder_esocial);
            this.pnNFeEntrada.Controls.Add(this.label1);
            this.pnNFeEntrada.Location = new System.Drawing.Point(8, 39);
            this.pnNFeEntrada.Name = "pnNFeEntrada";
            this.pnNFeEntrada.Size = new System.Drawing.Size(810, 428);
            this.pnNFeEntrada.TabIndex = 3;
            // 
            // btn_cnf_certificado
            // 
            this.btn_cnf_certificado.Location = new System.Drawing.Point(515, 7);
            this.btn_cnf_certificado.Name = "btn_cnf_certificado";
            this.btn_cnf_certificado.Size = new System.Drawing.Size(137, 23);
            this.btn_cnf_certificado.TabIndex = 4;
            this.btn_cnf_certificado.Text = "Configurar Certificado";
            this.btn_cnf_certificado.UseVisualStyleBackColor = true;
            this.btn_cnf_certificado.Click += new System.EventHandler(this.btn_cnf_certificado_Click);
            // 
            // eSocialDataGrid
            // 
            this.eSocialDataGrid.AllowUserToDeleteRows = false;
            this.eSocialDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.eSocialDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.status});
            this.eSocialDataGrid.Location = new System.Drawing.Point(0, 35);
            this.eSocialDataGrid.Name = "eSocialDataGrid";
            this.eSocialDataGrid.ReadOnly = true;
            this.eSocialDataGrid.Size = new System.Drawing.Size(805, 388);
            this.eSocialDataGrid.TabIndex = 3;
            // 
            // btn_folder_esocial
            // 
            this.btn_folder_esocial.Location = new System.Drawing.Point(658, 7);
            this.btn_folder_esocial.Name = "btn_folder_esocial";
            this.btn_folder_esocial.Size = new System.Drawing.Size(143, 23);
            this.btn_folder_esocial.TabIndex = 2;
            this.btn_folder_esocial.Text = "Iniciar Sincronizador";
            this.btn_folder_esocial.UseVisualStyleBackColor = true;
            this.btn_folder_esocial.Click += new System.EventHandler(this.btn_folder_esocial_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Ultimos Eventos Adquiridos";
            // 
            // icon_principal
            // 
            this.icon_principal.Icon = ((System.Drawing.Icon)(resources.GetObject("icon_principal.Icon")));
            this.icon_principal.Text = "OnContabil";
            this.icon_principal.Visible = true;
            this.icon_principal.DoubleClick += new System.EventHandler(this.icon_principal_DoubleClick);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(2, 470);
            this.panel4.TabIndex = 44;
            // 
            // panel8
            // 
            this.panel8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel8.Location = new System.Drawing.Point(0, 468);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(821, 2);
            this.panel8.TabIndex = 44;
            // 
            // panel9
            // 
            this.panel9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel9.Location = new System.Drawing.Point(0, 0);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(821, 2);
            this.panel9.TabIndex = 45;
            // 
            // panel10
            // 
            this.panel10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel10.Location = new System.Drawing.Point(819, 0);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(2, 470);
            this.panel10.TabIndex = 46;
            // 
            // id
            // 
            this.id.HeaderText = "saida";
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.Width = 660;
            // 
            // status
            // 
            this.status.HeaderText = "status";
            this.status.Name = "status";
            this.status.ReadOnly = true;
            // 
            // Principal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(821, 470);
            this.Controls.Add(this.panel10);
            this.Controls.Add(this.panel8);
            this.Controls.Add(this.panel9);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.pnNFeEntrada);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Principal";
            this.Text = "OnContabil";
            this.Load += new System.EventHandler(this.Principal_Load);
            this.Shown += new System.EventHandler(this.Principal_Shown);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnAjuda)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnFechar)).EndInit();
            this.pnNFeEntrada.ResumeLayout(false);
            this.pnNFeEntrada.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.eSocialDataGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel pnNFeEntrada;
        private System.Windows.Forms.NotifyIcon icon_principal;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.PictureBox btnAjuda;
        private System.Windows.Forms.PictureBox btnFechar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_folder_esocial;
        private System.Windows.Forms.DataGridView eSocialDataGrid;
        private System.Windows.Forms.Button btn_cnf_certificado;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn status;
    }
}

