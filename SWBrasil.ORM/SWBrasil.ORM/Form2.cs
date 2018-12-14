using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SWBrasil.ORM
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DirectoryInfo di = new DirectoryInfo(@"C:\Jaime\Temp\Dados");
            FileInfo[] files = di.GetFiles("*.sql");
            listBox1.DataSource = files;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlCommand sqlCmd;
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["WIS_HML_SQL"].ConnectionString))
            {
                try
                {
                    cnx.Open();

                    for (int i = 0; i < listBox1.Items.Count; i++)
                    {
                        listBox1.SelectedIndex = i;
                        string[] commands = File.ReadAllLines(@"C:\Jaime\Temp\Dados\" + listBox1.Items[i].ToString());
                        foreach (string command in commands)
                        {
                            if (string.IsNullOrEmpty(command) == false )
                            {
                                try
                                {
                                    sqlCmd = cnx.CreateCommand();
                                    sqlCmd.CommandText = command;
                                    sqlCmd.CommandType = CommandType.Text;
                                    int rows = sqlCmd.ExecuteNonQuery();

                                    textBox1.Text = command + Environment.NewLine;
                                    textBox1.Text += rows.ToString() + " row(s) affected" + Environment.NewLine;
                                    textBox1.Text += Environment.NewLine;
                                    textBox1.Text += Environment.NewLine;
                                    textBox1.SelectionStart = textBox1.Text.Length - 2;
                                    textBox1.ScrollToCaret();
                                }
                                catch (Exception err)
                                {
                                    //if (MessageBox.Show(err.Message, "Erro", MessageBoxButtons.RetryCancel) == System.Windows.Forms.DialogResult.Cancel)
                                    //    return;
                                }
                            }
                            Application.DoEvents();
                        }
                    }
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }

                if (cnx.State != ConnectionState.Closed && cnx.State != ConnectionState.Broken)
                    cnx.Close();
            }

        }


    }
}
