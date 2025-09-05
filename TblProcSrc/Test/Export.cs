using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using SA.TblProc;

/********************************************************************************
* Copyright : Alexander Sazonov 2007                                           //
*                                                                              //
* Email : sazon666@mail.ru                                                     //
*         sazon@freemail.ru                                                    // 
*                                                                              //
* This code may be used in any way you desire. This                            //
* file may be redistributed by any means PROVIDING it is                       //
* not sold for profit without the authors written consent, and                 //
* providing that this notice and the authors name is included.                 //
*                                                                              //
* This file is provided 'as is' with no expressed or implied warranty.         //
* The author accepts no liability if it causes any damage to your computer.    //
*                                                                              //
* Expect Bugs.                                                                 //
* Please let me know of any bugs/mods/improvements.                            //
* and I will try to fix/incorporate them into this file.                       //
* thx Amar Chaudhary for disclaimer text ;-)                                   //
* Enjoy!                                                                       //
*                                                                              //
*/
/////////////////////////////////////////////////////////////////////////////////

//A simple code to export data from DataGridView to table processor.
//Uses TblProc library

namespace Test
{
    public class Export
    {
        private int nVisibleCols = 0;
        private string name = null;
        private DataGridView grid = null;
        private bool? useExcel;

        public Export(DataGridView grid, string name) : this(grid, name, null) { }

        public Export(DataGridView grid, string name, bool? useExcel)
        {
            this.grid = grid;
            this.name = name;
            this.useExcel = useExcel;
            nVisibleCols = 0;
            for (int i = 0; i < grid.Columns.Count; i++)
            {
                if (grid.Columns[i].Visible) nVisibleCols++;
            }
        }

        private string[] Header
        {
            get
            {
                string[] aNames = new string[nVisibleCols];
                for (int i = 0, j = 0; j < nVisibleCols; i++)
                {
                    DataGridViewColumn col = grid.Columns[i];
                    if (col.Visible)
                    {
                        aNames[j] = col.HeaderText;
                        j++;
                    }
                }
                return aNames;
            }

        }

        public bool Create()
        {
            ProgressFrm progDlg = null;
            try
            {
                int[] aColumnIndexes = new int[nVisibleCols];
                for (int i = 0, j = 0; j < nVisibleCols; i++)
                {
                    DataGridViewColumn col = grid.Columns[i];
                    if (col.Visible)
                    {
                        aColumnIndexes[j] = i;
                        j++;
                    }
                }
                int nStrings = grid.Rows.Count;
                if (grid.NewRowIndex >= 0) nStrings--;
                if (nStrings < 0) return false;
                progDlg = new ProgressFrm(nStrings, "Please, wait while the we're creating the report...");
                progDlg.Show();
                progDlg.Refresh();

                TableSheet page=null;
                if (useExcel == null)
                {
                    page = TableSheet.CreateAvailable(name);
                }
                else
                {
                    if (useExcel == true) page = SA.TblProc.Excel.ExcelSheet.CreateExcelSheet(name);
                    else page = SA.TblProc.OO.OOSheet.CreateOOSheet(name);
                }
                page.AddArray(Header);
                TableRange data = page.Range(page.CurrentRow, 1, page.CurrentRow + nStrings - 1, nVisibleCols);
                data.CreateArray();
                for (int i = 0; i < nStrings; i++)
                {
                    DataGridViewRow row = grid.Rows[i];
                    if (row == null) continue;
                    for (int j = 0; j < nVisibleCols; j++)
                    {
                        object o = row.Cells[aColumnIndexes[j]].FormattedValue;
                        if (o != null)
                        {
                            Type t = o.GetType();
                            if (t == typeof(bool))
                            {
                                data[i, j] = (Convert.ToBoolean(o) ? "Yes" : "No");
                                continue;
                            }
                        }
                        data[i, j] = o;
                    }
                    progDlg.Step();
                }
                data.FlushArray();

                for (int i = 1; i <= nVisibleCols; i++)
                {
                    page.BorderAround(2, i, nStrings + 1, i, LineStyle.Dot, BorderWeight.Thin);
                }
                //data border
                page.BorderAround(2, 1, nStrings + 1, nVisibleCols, LineStyle.Continuous, BorderWeight.Thick);

                //header border
                for (int i = 1; i <= nVisibleCols; i++)
                {
                    page.BorderAround(1, i, 1, i, LineStyle.Continuous, BorderWeight.Thick);
                }
                page.AutoFitColumns();
                page.Visible = true;
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error while creating the report: " + ex.Message);
                return false;
            }
            finally
            {
                if (progDlg != null)
                {
                    progDlg.Close();
                    progDlg.Dispose();
                }
            }
            return true;
        }
    }
}
