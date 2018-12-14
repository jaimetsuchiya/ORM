using SWBrasil.ORM.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ORM
{
    public partial class Form1 : Form
    {
        ProjectModel projectModel = new ProjectModel();
        BaseORM orm = null;
        List<ICommand> _commands = null;
        List<IProject> _projects = null;
        string _fileName = "";

        public Form1()
        {
            InitializeComponent();
            orm = new SqlServerORM();
            cboViewType.SelectedIndex = 0;
            _commands = orm.AvailableTableTemplates(Environment.CurrentDirectory).OrderBy(cmd=>cmd.CommandID).ToList();

            cboTemplate.Items.Clear();
            for (int i = 0; i < _commands.Count; i++)
                cboTemplate.Items.Add(_commands[i].CommandID);

            _projects = orm.AvailableProjectTemplates(Environment.CurrentDirectory).OrderBy(cmd => cmd.CommandID).ToList();
            cboTemplateProjeto.Items.Clear();
            for (int i = 0; i < _projects.Count; i++)
                cboTemplateProjeto.Items.Add(_projects[i].CommandID);
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Application.DoEvents();

            if (orm.Connect(txtDataSource.Text))
            {
                lstObjects.Items.Clear();
                foreach (TableModel tabela in orm.Tables)
                {
                    bool tableChecked = projectModel.Tables.Where(t => t.Name == tabela.Name).Count() > 0;
                    lstObjects.Items.Add(tabela.Name);
                }
                txtFiltro_TextChanged(null, null);
            }
            else
                MessageBox.Show("Não foi possível conectar ao Banco de Dados!");

            Cursor.Current = Cursors.AppStarting;
        }

        private bool isTable()
        {
            return cboViewType.SelectedItem.ToString() == "Tables";
        }

        private void cboViewType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (orm == null || orm.Tables == null || orm.Procedures == null)
                return;

            if(isTable())
            {
                lstObjects.Items.Clear();
                foreach (TableModel tabela in orm.Tables)
                {
                    lstObjects.Items.Add(tabela.Name);
                }
            }
            else
            {
                lstObjects.Items.Clear();
                foreach (ProcModel proc in orm.Procedures)
                {
                    lstObjects.Items.Add(proc.Name);
                }
            }
        }

        private void lstObjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            if( isTable() )
            {
                string tableName = lstObjects.SelectedItem.ToString();
                tableGroup.Visible = true;

                var pjtable = projectModel.Tables.Where(t => t.Name == tableName).SingleOrDefault();
                var dbTable = orm.Tables.Where(t => t.Name == tableName).SingleOrDefault();

                if (pjtable == null)
                    pjtable = dbTable;
                else
                {
                    for( var i=pjtable.Columns.Count -1; i>= 0; i--)
                    {
                        var colDTO = pjtable.Columns[i];
                        var colDB  = dbTable.Columns.Where(c => c.ColumnName == colDTO.ColumnName).FirstOrDefault();
                        if (colDB == null)
                            pjtable.Columns.RemoveAt(i);
                        else
                        {
                            pjtable.Columns[i].DataType = colDB.DataType;
                            pjtable.Columns[i].DbType = colDB.DbType;
                            pjtable.Columns[i].DefaultValue = colDB.DefaultValue;
                            pjtable.Columns[i].ExtendedProperty = colDB.ExtendedProperty;
                            pjtable.Columns[i].IsIdentity= colDB.IsIdentity;
                            pjtable.Columns[i].IsPK = colDB.IsPK;
                            pjtable.Columns[i].IsUniqueKey = colDB.IsUniqueKey;
                            pjtable.Columns[i].Precision = colDB.Precision;
                            pjtable.Columns[i].RelatedTable = colDB.RelatedTable;
                            pjtable.Columns[i].Required = colDB.Required;
                            pjtable.Columns[i].Size = colDB.Size;
                            
                            colDB.DTOName = pjtable.Columns[i].DTOName;
                            colDB.IgnoreOnDTO = pjtable.Columns[i].IgnoreOnDTO;
                            colDB.SelectionType = pjtable.Columns[i].SelectionType;
                            colDB.ShowOnResultGrid = pjtable.Columns[i].ShowOnResultGrid;
                            colDB.UseAsLabelOnComboBox = pjtable.Columns[i].UseAsLabelOnComboBox;
                            colDB.UseAsRelatedObject = pjtable.Columns[i].UseAsRelatedObject;
                            colDB.UseAsSearchParameter = pjtable.Columns[i].UseAsSearchParameter;
                        }
                    }

                    for( var i=0; i < dbTable.Columns.Count; i++)
                    {
                        var colDB = dbTable.Columns[i];
                        var colDTO = pjtable.Columns.Where(c => c.ColumnName == colDB.ColumnName).SingleOrDefault();
                        if (colDTO == null)
                            pjtable.Columns.Add(colDB);
                    }
                }

                if(pjtable == null )
                {
                    MessageBox.Show(string.Format("Tabela [{0}] não encontrada!", tableName));
                    return;
                }
                else
                {
                    txtModelName.Text = string.IsNullOrEmpty(pjtable.ModelName) ? pjtable.Name.Replace("tb_", "") : pjtable.ModelName;
                    txtDTOName.Text = pjtable.Alias;
                    txtGroupName.Text = pjtable.Group;
                    cboTableType.SelectedItem = pjtable.Type.ToString();
                    chkMainDTO.Checked = pjtable.MainDTO;
                    chkIgnoreDTO.Checked = pjtable.IgnoreDTO;
                    txtLabel.Text = pjtable.Label;
                    dataGridView1.DataSource = pjtable.Columns;
                    dataGridView1.Refresh();
                }
            }
  
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string tableName = lstObjects.SelectedItem.ToString();
            if ( chkIgnoreDTO.Checked == false )
            {
                if (string.IsNullOrEmpty(txtDTOName.Text))
                {
                    MessageBox.Show("Favor informar o Nome do DTO");
                    txtDTOName.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(txtGroupName.Text))
                {
                    MessageBox.Show("Favor informar o Nome do Grupo");
                    txtGroupName.Focus();
                    return;
                }

                if( cboTableType.SelectedItem == null || cboTableType.SelectedItem.ToString() == "")
                {
                    MessageBox.Show("Favor informar o Tipo de Tabela");
                    cboTableType.Focus();
                    return;
                }
                
                var table = orm.Tables.Where(t => t.Name == tableName).Single();
                foreach( ColumnModel col in table.Columns)
                {
                    if( col.IgnoreOnDTO == false && string.IsNullOrEmpty(col.DTOName))
                    {
                        MessageBox.Show(string.Format("Favor informar o nome da propriedade no DTO para a coluna [{0}]", col.ColumnName));
                        return;
                    }

                    if (col.IsPK == false && string.IsNullOrEmpty(col.DTOName))
                    {
                        MessageBox.Show(string.Format("Favor informar o nome da propriedade no DTO para a coluna [{0}]", col.ColumnName));
                        return;
                    }
                }
            }

            if(chkMainDTO.Checked)
            {
                if( orm.Tables.Where(t => t.Name == tableName).Single().Columns.Where(c=>c.IsPK).Count() > 1 )
                {
                    MessageBox.Show("Tabelas Marcadas como Main, nao devem possuir mais de uma coluna como PK!");
                    return;
                }
                if (orm.Tables.Where(t => t.Name == tableName).Single().Columns.Where(c => c.IsPK).Count() == 0)
                {
                    MessageBox.Show("Tabelas Marcadas como Main, devem possuir uma coluna como PK!");
                    return;
                }
            }

            orm.Tables.Where(t => t.Name == tableName).Single().ModelName = txtModelName.Text;
            orm.Tables.Where(t => t.Name == tableName).Single().IgnoreDTO = chkIgnoreDTO.Checked;
            orm.Tables.Where(t => t.Name == tableName).Single().Alias = txtDTOName.Text;
            orm.Tables.Where(t => t.Name == tableName).Single().Group = txtGroupName.Text;
            orm.Tables.Where(t => t.Name == tableName).Single().Type = GetTableType();
            orm.Tables.Where(t => t.Name == tableName).Single().MainDTO = chkMainDTO.Checked;
            orm.Tables.Where(t => t.Name == tableName).Single().Columns = (List<ColumnModel>)dataGridView1.DataSource;
            orm.Tables.Where(t => t.Name == tableName).Single().Label = txtLabel.Text;

            if (projectModel.Tables.Where(t => t.Name == tableName).Count() == 0)
                projectModel.Tables.Add(orm.Tables.Where(t => t.Name == tableName).Single());
            else
            {
                projectModel.Tables.Where(t => t.Name == tableName).Single().ModelName = txtModelName.Text;
                projectModel.Tables.Where(t => t.Name == tableName).Single().IgnoreDTO = chkIgnoreDTO.Checked;
                projectModel.Tables.Where(t => t.Name == tableName).Single().Alias = txtDTOName.Text;
                projectModel.Tables.Where(t => t.Name == tableName).Single().Group = txtGroupName.Text;
                projectModel.Tables.Where(t => t.Name == tableName).Single().Type = GetTableType();
                projectModel.Tables.Where(t => t.Name == tableName).Single().MainDTO = chkMainDTO.Checked;
                projectModel.Tables.Where(t => t.Name == tableName).Single().Label = txtLabel.Text;
                projectModel.Tables.Where(t => t.Name == tableName).Single().Columns = (List<ColumnModel>)dataGridView1.DataSource;
            }

            if( string.IsNullOrEmpty(_fileName) == false )
                projectModel.Save(_fileName);
        }

        private enumTableType GetTableType()
        {
            enumTableType ret = enumTableType.Basic;

            switch (cboTableType.SelectedItem.ToString())
            {
                case "Relation_1_To_N":
                    ret = enumTableType.Relation_1_To_N;
                    break;

                case "Relation_N_To_N":
                    ret = enumTableType.Relation_N_To_N;
                    break;

                case "Dictionary":
                    ret = enumTableType.Dictionary;
                    break;
            }

            return ret;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Project Files (*.swprj)|*.swprj";
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK && openFileDialog1.FileName != "")
            {
                projectModel.Load(openFileDialog1.FileName);
                loadProject();
                _fileName = openFileDialog1.FileName;
                btnConnect_Click(null, null);
            }
        }

        private void loadProject()
        {
            txtProjectName.Text = projectModel.name;
            txtNameSpace.Text = projectModel.nameSpace;
            txtDataSource.Text = projectModel.connectionString;
            txtOutputPath.Text = projectModel.outputFolder;
            cboTemplateProjeto.SelectedItem = projectModel.projectTemplate;
            txtFiltro.Text = projectModel.tableFilter;
            txtConnectionStringID.Text = projectModel.connectionStringID;
        }

        private void saveProject()
        {
            projectModel.tableFilter = txtFiltro.Text;
            projectModel.name = txtProjectName.Text;
            projectModel.nameSpace = txtNameSpace.Text;
            projectModel.connectionString = txtDataSource.Text;
            projectModel.outputFolder = txtOutputPath.Text;
            if(cboTemplateProjeto.SelectedItem != null )
                projectModel.projectTemplate = cboTemplateProjeto.SelectedItem.ToString();
            projectModel.connectionStringID = txtConnectionStringID.Text;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = _fileName;
            saveFileDialog1.Filter = "Project Files (*.swprj)|*.swprj";

            DialogResult result = saveFileDialog1.ShowDialog();
            if( result == DialogResult.OK && saveFileDialog1.FileName != "")
            {
                saveProject();
                projectModel.Save(saveFileDialog1.FileName);
            }
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            if (cboTemplate.SelectedItem == null || cboTemplate.SelectedItem.ToString() == "")
                return;

            if (lstObjects.SelectedItem == null || lstObjects.SelectedItem.ToString() == "")
                return;

            try
            {
                ITableTransformation cmd = (ITableTransformation)orm.AvailableTableTemplates(Environment.CurrentDirectory).Where(t => t.CommandID == cboTemplate.SelectedItem.ToString()).Single();
                cmd.NameSpace = txtNameSpace.Text;
                var table = projectModel.Tables.Where(t => t.Name == lstObjects.SelectedItem.ToString()).FirstOrDefault();
                if (table == null)
                    table = orm.Tables.Where(t=>t.Name == lstObjects.SelectedItem.ToString()).FirstOrDefault();
                if (table == null)
                    txtConsole.Text = string.Format("Tabela [{0}] não encontrada!", lstObjects.SelectedItem.ToString());
                else
                {
                    Form2 frm = new Form2();
                    frm.PreviewCode = cmd.ApplyTemplate(table, projectModel.Tables);
                    txtConsole.Text = FormatarConsole(cmd.Mensagens);
                    frm.Show();
                }
            }
            catch(Exception err)
            {
                txtConsole.Text = string.Format("Erro no processamento!Msg: {0}", err.Message);
            }
        }

        private string FormatarConsole(List<ProjectConsoleMessages> messages)
        {
            string ret = "";
            foreach(var msg in messages)
            {
                ret += msg.data.ToString("HH:mm") + " - " + (msg.erro ? "ERRO" : "") + " " + msg.mensagem;
                ret += Environment.NewLine;
            }
            return ret;
        }

        private void btnOutputPath_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
                txtOutputPath.Text = folderBrowserDialog1.SelectedPath;
        }

        private void btnBuild_Click(object sender, EventArgs e)
        {
            IProject cmd = (IProject)_projects.Where(t => t.CommandID == cboTemplateProjeto.SelectedItem.ToString()).Single();
            cmd.Load(projectModel);

            if ( txtOutputPath.Text == "" )
            {
                MessageBox.Show("Favor informar o path de saida");
                txtOutputPath.Focus();
                return;
            }
            try
            {
                cmd.Build(txtOutputPath.Text);
                txtConsole.Text = FormatarConsole(cmd.Mensagens);
            }
            catch(Exception err)
            {
                txtConsole.Text = FormatarConsole(cmd.Mensagens);
                txtConsole.Text+= Environment.NewLine + string.Format("Erro no processamento!Msg: {0}", err.Message);
            }
        }

        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            if (txtFiltro.Text == "")
                txtFiltro.Text = "Filtrar por ...";

            bool check = txtFiltro.Text != "Filtrar por ...";

            lstObjects.Items.Clear();
            if(orm.Tables != null)
            {
                foreach (TableModel tabela in orm.Tables)
                {
                    if (tabela.Name.Contains(txtFiltro.Text) || check == false)
                        lstObjects.Items.Add(tabela.Name);
                }

            }
        }
    }
}
