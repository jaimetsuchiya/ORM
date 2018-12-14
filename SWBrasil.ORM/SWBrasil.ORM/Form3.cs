using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SWBrasil.ORM
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void btnAddSource_Click(object sender, EventArgs e)
        {
            if (txtDataSourceField.Text != "")
            {
                chkSources.Items.Add(txtDataSourceField.Text);
                txtDataSourceField.Text = "";
                txtDataSourceField.Focus();
            }
        }

        private void btnRemoveSource_Click(object sender, EventArgs e)
        {
            if (chkSources.CheckedItems.Count > 0)
            {
                for (var i = chkSources.CheckedItems.Count - 1; i >= 0; i--)
                    chkSources.Items.Remove(chkSources.CheckedItems[i]);
            }
        }

        private void btnAddTitle_Click(object sender, EventArgs e)
        {
            if (txtTitleField.Text != "")
            {
                chkTitles.Items.Add(txtTitleField.Text);
                txtTitleField.Text = "";
                txtTitleField.Focus();
            }
        }

        private void btnRemoveTitle_Click(object sender, EventArgs e)
        {
            if (chkTitles.CheckedItems.Count > 0)
            {
                for (var i = chkTitles.CheckedItems.Count - 1; i >= 0; i--)
                    chkTitles.Items.Remove(chkTitles.CheckedItems[i]);
            }
        }

        private void btnGerar_Click(object sender, EventArgs e)
        {
            #region Valida Parametros de Geração

            if (txtName.Text == "")
            {
                MessageBox.Show("Favor informar o nome do Formulário");
                txtName.Focus();
                return;
            }
            
            if (chkSources.Items.Count == 0)
            {
                MessageBox.Show("Favor informar as Propriedades de Origem");
                txtDataSourceField.Focus();
                return;
            }

            if (chkTitles.Items.Count == 0)
            {
                MessageBox.Show("Favor informar os Nomes das Colunas");
                txtTitleField.Focus();
                return;
            }

            if (chkSources.Items.Count != chkTitles.Items.Count)
            {
                MessageBox.Show("O Número de Propriedades e Títulos do Grid está diferente!");
                return;
            }

            if (txtOutputPath.Text == "")
            {
                MessageBox.Show("Favor informar o Path de Saída");
                txtOutputPath.Focus();
                return;
            }
            else
            {
                if (Directory.Exists(txtOutputPath.Text) == false)
                {
                    MessageBox.Show("Path de Saída inválido!");
                    txtOutputPath.Focus();
                    return;
                }
            }

            if (chkLink.Checked && txtLink.Text == "")
            {
                MessageBox.Show("Favor informar a URL do Link");
                txtLink.Focus();
                return;
            }

            #endregion Valida Parametros de Geração

            StringBuilder ret = new StringBuilder();


        }
    }
}
