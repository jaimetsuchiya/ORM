using SWBrasil.ORM.CommandTemplate;
using SWBrasil.ORM.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace SWBrasil.ORM
{
    public partial class Form1 : Form
    {
        ProjectModel projectModel = new ProjectModel();
        string config = "";
        Common.BaseORM orm = null;

        #region Load and Save Project

        private void Form1_Load(object sender, EventArgs e)
        {
            config = Application.ExecutablePath.ToLower().Replace(".exe", ".log");
            if (File.Exists(config))
            {
                string[] lines = File.ReadAllLines(config);
                foreach (string line in lines)
                {
                    if (line.StartsWith("ProjectDefinition"))
                    {
                        try
                        {
                            projectModel.Load(line.Replace("ProjectDefinition=", ""));
                            loadProjectData();
                        }
                        catch (Exception err)
                        {
                            MessageBox.Show("Erro na leitura da Última Definição do Projeto:" + err.Message);
                        }
                    }
                }
            }
        }

        private void loadProjectData()
        {
            
        }

        //private void loadProjectData()
        //{
        //    txtDataSource.Text = projectModel.connectionString;
        //    txtNameSpace.Text = projectModel.nameSpace;
        //    txtProjectName.Text = projectModel.name;

        //    chkTables.Items.Clear();
        //    for( var i=0; i < projectModel.Tables.Count; i++)
        //    {
        //        chkTables.Items.Add( new DataBaseObject() { })
        //    }

        //}

        private void writeLastExecution(string fileName)
        {
            StringBuilder sb = new StringBuilder();

            if( string.IsNullOrEmpty(fileName) == false)
            {
                sb.AppendLine("ProjectDefinition=" + fileName);
                File.WriteAllText(config, sb.ToString());
            }
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (projectModel == null)
                    projectModel = new ProjectModel();

                projectModel.name = txtProjectName.Text;
                projectModel.nameSpace = txtNameSpace.Text;
                projectModel.connectionString = txtDataSource.Text;

                saveFileDialog1.Filter = "Project Files (*.swprj)|*.swprj";
                if (string.IsNullOrEmpty(saveFileDialog1.FileName))
                {
                    DialogResult result = saveFileDialog1.ShowDialog();
                }
                projectModel.Save(saveFileDialog1.FileName);
                writeLastExecution(saveFileDialog1.FileName);
            }
            catch (Exception err)
            {
                MessageBox.Show("Erro ao Salvar o Arquivo:" + err.Message);
            }
        }

        #endregion Load and Save Project
        
        #region Event Handler

        public Form1()
        {
            InitializeComponent();
        }

        //private void btnReadDataBase_Click(object sender, EventArgs e)
        //{
        //    chkTables.Items.Clear();
        //}

        //private void chkAll_CheckedChanged(object sender, EventArgs e)
        //{
        //    chkAll.Text = (chkAll.Checked ? "UnCheck All" : "Check All");
        //    for (int i = 0; i < chkTables.Items.Count; i++)
        //        chkTables.SetItemChecked(i, chkAll.Checked);
        //}

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    DialogResult result = folderBrowserDialog1.ShowDialog();
        //    if (result == System.Windows.Forms.DialogResult.OK)
        //        txtOutputPath.Text = folderBrowserDialog1.SelectedPath;
        //}
        
        //private void button2_Click(object sender, EventArgs e)
        //{
        //    DialogResult result = folderBrowserDialog1.ShowDialog();
        //    if (result == System.Windows.Forms.DialogResult.OK)
        //        txtTemplateProject.Text = folderBrowserDialog1.SelectedPath;
        //}
        
        #endregion Event Handler
        
        //private void button8_Click(object sender, EventArgs e)
        //{
        //    openFileDialog1.Filter = "Project Files (*.swprj)|*.swprj";
        //    DialogResult result = openFileDialog1.ShowDialog();
        //    if( result == DialogResult.OK && openFileDialog1.FileName != "")
        //    {
        //        projectModel.Load(openFileDialog1.FileName);
        //        writeLastExecution(saveFileDialog1.FileName);
        //        loadTab1FromProject();
        //    }
        //}

        private void btnConnect_Click_1(object sender, EventArgs e)
        {
            orm = new Common.SqlServerORM();
            if (orm.Connect(txtDataSource.Text))
            {
                lstObjects.Items.Clear();
                foreach (Common.TableModel tabela in orm.Tables)
                {
                    bool tableChecked = projectModel.Tables.Where(t => t.Name == tabela.Name).Count() > 0;
                    lstObjects.Items.Add(new DataBaseObject() { name = tabela.Name, type = "Table" });
                }
                foreach (Common.ProcModel proc in orm.Procedures)
                {
                    bool procChecked = projectModel.Procedures.Where(p => p.Name == proc.Name).Count() > 0;
                    lstObjects.Items.Add(new DataBaseObject() { name = proc.Name, type = "Procedure" });
                }
            }
            else
                MessageBox.Show("Não foi possível conectar ao Banco de Dados!");
        }
    }

}
