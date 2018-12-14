namespace SWBrasil.ORM
{
    partial class Form3
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
            this.chkSources = new System.Windows.Forms.CheckedListBox();
            this.chkTitles = new System.Windows.Forms.CheckedListBox();
            this.chkLink = new System.Windows.Forms.CheckBox();
            this.txtTitleField = new System.Windows.Forms.TextBox();
            this.txtDataSourceField = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnRemoveSource = new System.Windows.Forms.Button();
            this.btnAddSource = new System.Windows.Forms.Button();
            this.btnAddTitle = new System.Windows.Forms.Button();
            this.btnRemoveTitle = new System.Windows.Forms.Button();
            this.txtLink = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtServiceURI = new System.Windows.Forms.TextBox();
            this.btnGerar = new System.Windows.Forms.Button();
            this.directoryEntry1 = new System.DirectoryServices.DirectoryEntry();
            this.label5 = new System.Windows.Forms.Label();
            this.txtOutputPath = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // chkSources
            // 
            this.chkSources.FormattingEnabled = true;
            this.chkSources.Location = new System.Drawing.Point(27, 97);
            this.chkSources.Name = "chkSources";
            this.chkSources.Size = new System.Drawing.Size(154, 139);
            this.chkSources.TabIndex = 1;
            // 
            // chkTitles
            // 
            this.chkTitles.FormattingEnabled = true;
            this.chkTitles.Location = new System.Drawing.Point(273, 97);
            this.chkTitles.Name = "chkTitles";
            this.chkTitles.Size = new System.Drawing.Size(154, 139);
            this.chkTitles.TabIndex = 2;
            // 
            // chkLink
            // 
            this.chkLink.AutoSize = true;
            this.chkLink.Location = new System.Drawing.Point(82, 289);
            this.chkLink.Name = "chkLink";
            this.chkLink.Size = new System.Drawing.Size(87, 17);
            this.chkLink.TabIndex = 3;
            this.chkLink.Text = "Habilitar Link";
            this.chkLink.UseVisualStyleBackColor = true;
            // 
            // txtTitleField
            // 
            this.txtTitleField.Location = new System.Drawing.Point(273, 71);
            this.txtTitleField.Name = "txtTitleField";
            this.txtTitleField.Size = new System.Drawing.Size(154, 20);
            this.txtTitleField.TabIndex = 4;
            // 
            // txtDataSourceField
            // 
            this.txtDataSourceField.Location = new System.Drawing.Point(27, 71);
            this.txtDataSourceField.Name = "txtDataSourceField";
            this.txtDataSourceField.Size = new System.Drawing.Size(154, 20);
            this.txtDataSourceField.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "DataSource Field";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(270, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Field Title";
            // 
            // btnRemoveSource
            // 
            this.btnRemoveSource.Location = new System.Drawing.Point(187, 97);
            this.btnRemoveSource.Name = "btnRemoveSource";
            this.btnRemoveSource.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveSource.TabIndex = 8;
            this.btnRemoveSource.Text = "-";
            this.btnRemoveSource.UseVisualStyleBackColor = true;
            this.btnRemoveSource.Click += new System.EventHandler(this.btnRemoveSource_Click);
            // 
            // btnAddSource
            // 
            this.btnAddSource.Location = new System.Drawing.Point(187, 71);
            this.btnAddSource.Name = "btnAddSource";
            this.btnAddSource.Size = new System.Drawing.Size(75, 23);
            this.btnAddSource.TabIndex = 9;
            this.btnAddSource.Text = "+";
            this.btnAddSource.UseVisualStyleBackColor = true;
            this.btnAddSource.Click += new System.EventHandler(this.btnAddSource_Click);
            // 
            // btnAddTitle
            // 
            this.btnAddTitle.Location = new System.Drawing.Point(433, 71);
            this.btnAddTitle.Name = "btnAddTitle";
            this.btnAddTitle.Size = new System.Drawing.Size(75, 23);
            this.btnAddTitle.TabIndex = 10;
            this.btnAddTitle.Text = "+";
            this.btnAddTitle.UseVisualStyleBackColor = true;
            this.btnAddTitle.Click += new System.EventHandler(this.btnAddTitle_Click);
            // 
            // btnRemoveTitle
            // 
            this.btnRemoveTitle.Location = new System.Drawing.Point(433, 100);
            this.btnRemoveTitle.Name = "btnRemoveTitle";
            this.btnRemoveTitle.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveTitle.TabIndex = 11;
            this.btnRemoveTitle.Text = "-";
            this.btnRemoveTitle.UseVisualStyleBackColor = true;
            this.btnRemoveTitle.Click += new System.EventHandler(this.btnRemoveTitle_Click);
            // 
            // txtLink
            // 
            this.txtLink.Location = new System.Drawing.Point(27, 306);
            this.txtLink.Name = "txtLink";
            this.txtLink.Size = new System.Drawing.Size(400, 20);
            this.txtLink.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 290);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "URL Link";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 336);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Service URI";
            // 
            // txtServiceURI
            // 
            this.txtServiceURI.Location = new System.Drawing.Point(27, 352);
            this.txtServiceURI.Name = "txtServiceURI";
            this.txtServiceURI.Size = new System.Drawing.Size(400, 20);
            this.txtServiceURI.TabIndex = 14;
            // 
            // btnGerar
            // 
            this.btnGerar.Location = new System.Drawing.Point(433, 306);
            this.btnGerar.Name = "btnGerar";
            this.btnGerar.Size = new System.Drawing.Size(75, 66);
            this.btnGerar.TabIndex = 16;
            this.btnGerar.Text = "Gerar";
            this.btnGerar.UseVisualStyleBackColor = true;
            this.btnGerar.Click += new System.EventHandler(this.btnGerar_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(24, 247);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "Output Path";
            // 
            // txtOutputPath
            // 
            this.txtOutputPath.Location = new System.Drawing.Point(27, 263);
            this.txtOutputPath.Name = "txtOutputPath";
            this.txtOutputPath.Size = new System.Drawing.Size(400, 20);
            this.txtOutputPath.TabIndex = 17;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(25, 15);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(61, 13);
            this.label6.TabIndex = 20;
            this.label6.Text = "Form Name";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(27, 31);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(400, 20);
            this.txtName.TabIndex = 19;
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(535, 420);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtOutputPath);
            this.Controls.Add(this.btnGerar);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtServiceURI);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtLink);
            this.Controls.Add(this.btnRemoveTitle);
            this.Controls.Add(this.btnAddTitle);
            this.Controls.Add(this.btnAddSource);
            this.Controls.Add(this.btnRemoveSource);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtDataSourceField);
            this.Controls.Add(this.txtTitleField);
            this.Controls.Add(this.chkLink);
            this.Controls.Add(this.chkTitles);
            this.Controls.Add(this.chkSources);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form3";
            this.Text = "Gerador Grid";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox chkSources;
        private System.Windows.Forms.CheckedListBox chkTitles;
        private System.Windows.Forms.CheckBox chkLink;
        private System.Windows.Forms.TextBox txtTitleField;
        private System.Windows.Forms.TextBox txtDataSourceField;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnRemoveSource;
        private System.Windows.Forms.Button btnAddSource;
        private System.Windows.Forms.Button btnAddTitle;
        private System.Windows.Forms.Button btnRemoveTitle;
        private System.Windows.Forms.TextBox txtLink;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtServiceURI;
        private System.Windows.Forms.Button btnGerar;
        private System.DirectoryServices.DirectoryEntry directoryEntry1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtOutputPath;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtName;
    }
}