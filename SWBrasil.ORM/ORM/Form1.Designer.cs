namespace ORM
{
    partial class Form1
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.txtDataSource = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtNameSpace = new System.Windows.Forms.TextBox();
            this.txtProjectName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtFiltro = new System.Windows.Forms.TextBox();
            this.cboViewType = new System.Windows.Forms.ComboBox();
            this.lstObjects = new System.Windows.Forms.ListBox();
            this.tableGroup = new System.Windows.Forms.GroupBox();
            this.txtModelName = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtLabel = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.chkMainDTO = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cboTemplate = new System.Windows.Forms.ComboBox();
            this.btnPreview = new System.Windows.Forms.Button();
            this.cboTableType = new System.Windows.Forms.ComboBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ColumnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IgnoreOnDTO = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Alias = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UseAsLabelOnComboBox = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.UseAsSearchParameter = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ShowOnResultGrid = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.UseAsRelatedObject = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.btnOK = new System.Windows.Forms.Button();
            this.chkIgnoreDTO = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtGroupName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtDTOName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cboTemplateProjeto = new System.Windows.Forms.ComboBox();
            this.btnOutputPath = new System.Windows.Forms.Button();
            this.btnBuild = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.txtOutputPath = new System.Windows.Forms.TextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.txtConsole = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtConnectionStringID = new System.Windows.Forms.TextBox();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableGroup.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.txtConnectionStringID);
            this.groupBox2.Controls.Add(this.btnSave);
            this.groupBox2.Controls.Add(this.btnLoad);
            this.groupBox2.Controls.Add(this.btnConnect);
            this.groupBox2.Controls.Add(this.txtDataSource);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.txtNameSpace);
            this.groupBox2.Controls.Add(this.txtProjectName);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(995, 82);
            this.groupBox2.TabIndex = 23;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Setup";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(780, 45);
            this.btnSave.Margin = new System.Windows.Forms.Padding(2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(99, 33);
            this.btnSave.TabIndex = 25;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(677, 45);
            this.btnLoad.Margin = new System.Windows.Forms.Padding(2);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(99, 33);
            this.btnLoad.TabIndex = 24;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(883, 45);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(2);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(99, 33);
            this.btnConnect.TabIndex = 23;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // txtDataSource
            // 
            this.txtDataSource.Location = new System.Drawing.Point(97, 18);
            this.txtDataSource.Margin = new System.Windows.Forms.Padding(2);
            this.txtDataSource.Name = "txtDataSource";
            this.txtDataSource.Size = new System.Drawing.Size(885, 20);
            this.txtDataSource.TabIndex = 23;
            this.txtDataSource.Text = "Persist Security Info=False; Data Source=[DATASOURCE]; User ID=[USER]; Password=[" +
    "PASSWORD]; Initial Catalog=[CATALOG]; MultipleActiveResultSets=True;";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 18);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 22;
            this.label1.Text = "Connection String";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(233, 55);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(66, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "NameSpace";
            // 
            // txtNameSpace
            // 
            this.txtNameSpace.Location = new System.Drawing.Point(308, 52);
            this.txtNameSpace.Margin = new System.Windows.Forms.Padding(2);
            this.txtNameSpace.Name = "txtNameSpace";
            this.txtNameSpace.Size = new System.Drawing.Size(99, 20);
            this.txtNameSpace.TabIndex = 19;
            // 
            // txtProjectName
            // 
            this.txtProjectName.Location = new System.Drawing.Point(96, 52);
            this.txtProjectName.Margin = new System.Windows.Forms.Padding(2);
            this.txtProjectName.Name = "txtProjectName";
            this.txtProjectName.Size = new System.Drawing.Size(99, 20);
            this.txtProjectName.TabIndex = 17;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(21, 55);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "Project Name";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtFiltro);
            this.groupBox1.Controls.Add(this.cboViewType);
            this.groupBox1.Controls.Add(this.lstObjects);
            this.groupBox1.Location = new System.Drawing.Point(12, 100);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(179, 449);
            this.groupBox1.TabIndex = 24;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "DataBase Objects";
            // 
            // txtFiltro
            // 
            this.txtFiltro.Location = new System.Drawing.Point(6, 416);
            this.txtFiltro.Name = "txtFiltro";
            this.txtFiltro.Size = new System.Drawing.Size(167, 20);
            this.txtFiltro.TabIndex = 30;
            this.txtFiltro.Text = "Filtrar por...";
            this.txtFiltro.TextChanged += new System.EventHandler(this.txtFiltro_TextChanged);
            // 
            // cboViewType
            // 
            this.cboViewType.FormattingEnabled = true;
            this.cboViewType.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cboViewType.Items.AddRange(new object[] {
            "Tables",
            "Procedures"});
            this.cboViewType.Location = new System.Drawing.Point(6, 31);
            this.cboViewType.Name = "cboViewType";
            this.cboViewType.Size = new System.Drawing.Size(167, 21);
            this.cboViewType.TabIndex = 29;
            this.cboViewType.SelectedIndexChanged += new System.EventHandler(this.cboViewType_SelectedIndexChanged);
            // 
            // lstObjects
            // 
            this.lstObjects.FormattingEnabled = true;
            this.lstObjects.HorizontalScrollbar = true;
            this.lstObjects.Location = new System.Drawing.Point(6, 54);
            this.lstObjects.Name = "lstObjects";
            this.lstObjects.Size = new System.Drawing.Size(167, 355);
            this.lstObjects.TabIndex = 0;
            this.lstObjects.SelectedIndexChanged += new System.EventHandler(this.lstObjects_SelectedIndexChanged);
            // 
            // tableGroup
            // 
            this.tableGroup.Controls.Add(this.txtModelName);
            this.tableGroup.Controls.Add(this.label11);
            this.tableGroup.Controls.Add(this.txtLabel);
            this.tableGroup.Controls.Add(this.label10);
            this.tableGroup.Controls.Add(this.chkMainDTO);
            this.tableGroup.Controls.Add(this.groupBox4);
            this.tableGroup.Controls.Add(this.cboTableType);
            this.tableGroup.Controls.Add(this.dataGridView1);
            this.tableGroup.Controls.Add(this.btnOK);
            this.tableGroup.Controls.Add(this.chkIgnoreDTO);
            this.tableGroup.Controls.Add(this.label4);
            this.tableGroup.Controls.Add(this.txtGroupName);
            this.tableGroup.Controls.Add(this.label3);
            this.tableGroup.Controls.Add(this.txtDTOName);
            this.tableGroup.Controls.Add(this.label2);
            this.tableGroup.Location = new System.Drawing.Point(197, 100);
            this.tableGroup.Name = "tableGroup";
            this.tableGroup.Size = new System.Drawing.Size(810, 449);
            this.tableGroup.TabIndex = 25;
            this.tableGroup.TabStop = false;
            this.tableGroup.Text = "Properties";
            // 
            // txtModelName
            // 
            this.txtModelName.Location = new System.Drawing.Point(104, 22);
            this.txtModelName.Margin = new System.Windows.Forms.Padding(2);
            this.txtModelName.Name = "txtModelName";
            this.txtModelName.Size = new System.Drawing.Size(176, 20);
            this.txtModelName.TabIndex = 33;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(29, 25);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(67, 13);
            this.label11.TabIndex = 34;
            this.label11.Text = "Model Name";
            // 
            // txtLabel
            // 
            this.txtLabel.Location = new System.Drawing.Point(104, 120);
            this.txtLabel.Margin = new System.Windows.Forms.Padding(2);
            this.txtLabel.Name = "txtLabel";
            this.txtLabel.Size = new System.Drawing.Size(176, 20);
            this.txtLabel.TabIndex = 31;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(29, 123);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(33, 13);
            this.label10.TabIndex = 32;
            this.label10.Text = "Label";
            // 
            // chkMainDTO
            // 
            this.chkMainDTO.AutoSize = true;
            this.chkMainDTO.Location = new System.Drawing.Point(296, 73);
            this.chkMainDTO.Name = "chkMainDTO";
            this.chkMainDTO.Size = new System.Drawing.Size(75, 17);
            this.chkMainDTO.TabIndex = 30;
            this.chkMainDTO.Text = "Main DTO";
            this.chkMainDTO.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.cboTemplate);
            this.groupBox4.Controls.Add(this.btnPreview);
            this.groupBox4.Location = new System.Drawing.Point(424, 19);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(373, 83);
            this.groupBox4.TabIndex = 29;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Preview";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 27);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 13);
            this.label5.TabIndex = 30;
            this.label5.Text = "Template";
            // 
            // cboTemplate
            // 
            this.cboTemplate.FormattingEnabled = true;
            this.cboTemplate.Location = new System.Drawing.Point(6, 47);
            this.cboTemplate.Name = "cboTemplate";
            this.cboTemplate.Size = new System.Drawing.Size(248, 21);
            this.cboTemplate.TabIndex = 29;
            // 
            // btnPreview
            // 
            this.btnPreview.Location = new System.Drawing.Point(269, 35);
            this.btnPreview.Margin = new System.Windows.Forms.Padding(2);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(99, 33);
            this.btnPreview.TabIndex = 27;
            this.btnPreview.Text = "Preview";
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // cboTableType
            // 
            this.cboTableType.FormattingEnabled = true;
            this.cboTableType.Items.AddRange(new object[] {
            "Basic",
            "Dictionary",
            "Relation_1_To_N",
            "Relation_N_To_N"});
            this.cboTableType.Location = new System.Drawing.Point(104, 94);
            this.cboTableType.Name = "cboTableType";
            this.cboTableType.Size = new System.Drawing.Size(176, 21);
            this.cboTableType.TabIndex = 28;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnName,
            this.IgnoreOnDTO,
            this.Alias,
            this.UseAsLabelOnComboBox,
            this.UseAsSearchParameter,
            this.ShowOnResultGrid,
            this.UseAsRelatedObject});
            this.dataGridView1.Location = new System.Drawing.Point(27, 159);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(765, 277);
            this.dataGridView1.TabIndex = 27;
            // 
            // ColumnName
            // 
            this.ColumnName.DataPropertyName = "ColumnName";
            this.ColumnName.Frozen = true;
            this.ColumnName.HeaderText = "ColumnName";
            this.ColumnName.Name = "ColumnName";
            this.ColumnName.ReadOnly = true;
            // 
            // IgnoreOnDTO
            // 
            this.IgnoreOnDTO.DataPropertyName = "IgnoreOnDTO";
            this.IgnoreOnDTO.HeaderText = "IgnoreOnDTO";
            this.IgnoreOnDTO.Name = "IgnoreOnDTO";
            // 
            // Alias
            // 
            this.Alias.DataPropertyName = "DTOName";
            this.Alias.HeaderText = "DTOName";
            this.Alias.Name = "Alias";
            // 
            // UseAsLabelOnComboBox
            // 
            this.UseAsLabelOnComboBox.DataPropertyName = "UseAsLabelOnComboBox";
            this.UseAsLabelOnComboBox.HeaderText = "UseAsLabelOnComboBox";
            this.UseAsLabelOnComboBox.Name = "UseAsLabelOnComboBox";
            // 
            // UseAsSearchParameter
            // 
            this.UseAsSearchParameter.DataPropertyName = "UseAsSearchParameter";
            this.UseAsSearchParameter.HeaderText = "UseAsSearchParameter";
            this.UseAsSearchParameter.Name = "UseAsSearchParameter";
            // 
            // ShowOnResultGrid
            // 
            this.ShowOnResultGrid.DataPropertyName = "ShowOnResultGrid";
            this.ShowOnResultGrid.HeaderText = "ShowOnResultGrid";
            this.ShowOnResultGrid.Name = "ShowOnResultGrid";
            // 
            // UseAsRelatedObject
            // 
            this.UseAsRelatedObject.DataPropertyName = "UseAsRelatedObject";
            this.UseAsRelatedObject.HeaderText = "UseAsRelatedObject";
            this.UseAsRelatedObject.Name = "UseAsRelatedObject";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(296, 107);
            this.btnOK.Margin = new System.Windows.Forms.Padding(2);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(99, 33);
            this.btnOK.TabIndex = 26;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // chkIgnoreDTO
            // 
            this.chkIgnoreDTO.AutoSize = true;
            this.chkIgnoreDTO.Location = new System.Drawing.Point(296, 48);
            this.chkIgnoreDTO.Name = "chkIgnoreDTO";
            this.chkIgnoreDTO.Size = new System.Drawing.Size(82, 17);
            this.chkIgnoreDTO.TabIndex = 25;
            this.chkIgnoreDTO.Text = "Ignore DTO";
            this.chkIgnoreDTO.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(29, 97);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 13);
            this.label4.TabIndex = 24;
            this.label4.Text = "Object Type";
            // 
            // txtGroupName
            // 
            this.txtGroupName.Location = new System.Drawing.Point(104, 70);
            this.txtGroupName.Margin = new System.Windows.Forms.Padding(2);
            this.txtGroupName.Name = "txtGroupName";
            this.txtGroupName.Size = new System.Drawing.Size(176, 20);
            this.txtGroupName.TabIndex = 21;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(29, 73);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 22;
            this.label3.Text = "Group Name";
            // 
            // txtDTOName
            // 
            this.txtDTOName.Location = new System.Drawing.Point(104, 46);
            this.txtDTOName.Margin = new System.Windows.Forms.Padding(2);
            this.txtDTOName.Name = "txtDTOName";
            this.txtDTOName.Size = new System.Drawing.Size(176, 20);
            this.txtDTOName.TabIndex = 19;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 49);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 20;
            this.label2.Text = "DTO Name";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.cboTemplateProjeto);
            this.groupBox3.Controls.Add(this.btnOutputPath);
            this.groupBox3.Controls.Add(this.btnBuild);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.txtOutputPath);
            this.groupBox3.Location = new System.Drawing.Point(12, 556);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(465, 99);
            this.groupBox3.TabIndex = 26;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Build";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(7, 58);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(87, 13);
            this.label9.TabIndex = 31;
            this.label9.Text = "Template Projeto";
            // 
            // cboTemplateProjeto
            // 
            this.cboTemplateProjeto.FormattingEnabled = true;
            this.cboTemplateProjeto.Location = new System.Drawing.Point(97, 55);
            this.cboTemplateProjeto.Name = "cboTemplateProjeto";
            this.cboTemplateProjeto.Size = new System.Drawing.Size(249, 21);
            this.cboTemplateProjeto.TabIndex = 30;
            // 
            // btnOutputPath
            // 
            this.btnOutputPath.Location = new System.Drawing.Point(308, 28);
            this.btnOutputPath.Name = "btnOutputPath";
            this.btnOutputPath.Size = new System.Drawing.Size(38, 23);
            this.btnOutputPath.TabIndex = 28;
            this.btnOutputPath.Text = "...";
            this.btnOutputPath.UseVisualStyleBackColor = true;
            this.btnOutputPath.Click += new System.EventHandler(this.btnOutputPath_Click);
            // 
            // btnBuild
            // 
            this.btnBuild.Location = new System.Drawing.Point(361, 28);
            this.btnBuild.Margin = new System.Windows.Forms.Padding(2);
            this.btnBuild.Name = "btnBuild";
            this.btnBuild.Size = new System.Drawing.Size(89, 52);
            this.btnBuild.TabIndex = 28;
            this.btnBuild.Text = "Gerar";
            this.btnBuild.UseVisualStyleBackColor = true;
            this.btnBuild.Click += new System.EventHandler(this.btnBuild_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(29, 33);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(64, 13);
            this.label7.TabIndex = 22;
            this.label7.Text = "Output Path";
            // 
            // txtOutputPath
            // 
            this.txtOutputPath.Location = new System.Drawing.Point(97, 30);
            this.txtOutputPath.Margin = new System.Windows.Forms.Padding(2);
            this.txtOutputPath.Name = "txtOutputPath";
            this.txtOutputPath.Size = new System.Drawing.Size(206, 20);
            this.txtOutputPath.TabIndex = 21;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.txtConsole);
            this.groupBox5.Location = new System.Drawing.Point(488, 556);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(519, 99);
            this.groupBox5.TabIndex = 29;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Console";
            // 
            // txtConsole
            // 
            this.txtConsole.BackColor = System.Drawing.SystemColors.Window;
            this.txtConsole.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtConsole.Location = new System.Drawing.Point(7, 20);
            this.txtConsole.Multiline = true;
            this.txtConsole.Name = "txtConsole";
            this.txtConsole.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtConsole.Size = new System.Drawing.Size(506, 73);
            this.txtConsole.TabIndex = 0;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(442, 55);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(99, 13);
            this.label12.TabIndex = 27;
            this.label12.Text = "ConnectionStringID";
            // 
            // txtConnectionStringID
            // 
            this.txtConnectionStringID.Location = new System.Drawing.Point(545, 52);
            this.txtConnectionStringID.Margin = new System.Windows.Forms.Padding(2);
            this.txtConnectionStringID.Name = "txtConnectionStringID";
            this.txtConnectionStringID.Size = new System.Drawing.Size(99, 20);
            this.txtConnectionStringID.TabIndex = 26;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1018, 681);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.tableGroup);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ORM";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tableGroup.ResumeLayout(false);
            this.tableGroup.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox txtDataSource;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtNameSpace;
        private System.Windows.Forms.TextBox txtProjectName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox lstObjects;
        private System.Windows.Forms.GroupBox tableGroup;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtGroupName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtDTOName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.CheckBox chkIgnoreDTO;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ComboBox cboViewType;
        private System.Windows.Forms.ComboBox cboTableType;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cboTemplate;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.Button btnOutputPath;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtOutputPath;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button btnBuild;
        private System.Windows.Forms.CheckBox chkMainDTO;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IgnoreOnDTO;
        private System.Windows.Forms.DataGridViewTextBoxColumn Alias;
        private System.Windows.Forms.DataGridViewCheckBoxColumn UseAsLabelOnComboBox;
        private System.Windows.Forms.DataGridViewCheckBoxColumn UseAsSearchParameter;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ShowOnResultGrid;
        private System.Windows.Forms.DataGridViewCheckBoxColumn UseAsRelatedObject;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cboTemplateProjeto;
        private System.Windows.Forms.TextBox txtLabel;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtConsole;
        private System.Windows.Forms.TextBox txtModelName;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtFiltro;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtConnectionStringID;
    }
}

