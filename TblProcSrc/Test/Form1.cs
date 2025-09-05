using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Test
{
    public partial class Form1 : Form
    {
        private List<DataClass> data = new List<DataClass>();
        private bool? useExcel = null;

        public Form1()
        {
            data.Add(new DataClass(0, "Bill", "you-know-yourselves", "IT-gangster ;-)", true));
            data.Add(new DataClass(1, "Alexander", "Pushkin", "Famous poet", false));
            data.Add(new DataClass(2, "Sid", "Vicious", "punk", false));
            data.Add(new DataClass(3, "Jello", "Biafra", "punk too", true));
            data.Add(new DataClass(4, "George W.", "Bush", "ex-US president", false));
            InitializeComponent();
            dataClassBindingSource.DataSource = data;
        }

        private void exportBtn_Click(object sender, EventArgs e)
        {
            try
            {
                Export exp = new Export(grid, "General export", useExcel);
                exp.Create();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception caught: " + ex.Message);
            }
        }

        private void createTest_Click(object sender, EventArgs e)
        {
            TestCreator tc = new TestCreator(useExcel);
            tc.Create();
        }

        private void SetTableProcessor(bool? excel)
        {
            useAutomaticToolStripMenuItem.Checked = (excel == null);
            useExcelToolStripMenuItem.Checked = (excel==true);
            useOpenOfficeorgToolStripMenuItem.Checked = (excel == false);
            useExcel = excel;
        }

        private void useExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetTableProcessor(true);
        }

        private void useOpenOfficeorgToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetTableProcessor(false);
        }

        private void useAutomaticToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetTableProcessor(null);
        }

    }
}