using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Test
{
    public partial class ProgressFrm : Form
    {
        public ProgressFrm(int iMax, string strText)
        {
            InitializeComponent();
            _progress.Minimum = 0;
            _progress.Maximum = iMax;
            _label.Text = strText;
        }

        public bool Step()
        {
            try
            {
                _progress.PerformStep();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}