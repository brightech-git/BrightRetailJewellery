using System;
using System.Collections.Generic;
using System.Text;
using SA.TblProc;
using System.Windows.Forms;

namespace Test
{
    class TestCreator
    {
        private bool? useExcel;
        TableProcessor proc;

        public TestCreator(bool? useExcel)
        {
            this.useExcel = useExcel;
        }

        public void Create()
        {
            try
            {
                //create table processor of the desired type.
                //The processor will have ` page with name "sample"
                if (useExcel == null) proc = TableProcessor.CreateAvailable("sample");
                else
                {
                    if (useExcel == true)
                    {
                        //here I can check TableProcessor.ExcelAvailable to determine if I should continue
                        //but I won't check it ;-)                       
                        proc = SA.TblProc.Excel.ExcelApp.CreateExcel("sample", 1);
                    }
                    else
                    {
                        //here I can check TableProcessor.OOAvailable to determine if I should continue
                        //but I also won't check it, I'm too lazy and the goal of abstrtact base classes 
                        //is to avoid using CreateExcel and CreateOO methods of corresponding classes
                        
                        proc = SA.TblProc.OO.OOApp.CreateOO("sample", 1);
                    }
                }
                try
                {
                    //create 2 extra pages 
                    proc.CreateNextPage("sample 1");
                    proc.CreateNextPage("sample 2");

                    //set upper-left cell values to strings
                    proc[0].Cell(1, 1).Value = "First sheet";
                    proc[1].Cell(1, 1).Value = "Second sheet";
                    proc[2].Cell(1, 1).Value = "Third sheet";

                    //working with first page
                    TableSheet sheet = proc.FirstPage;

                    //rows and cols numbering starts as in Excel - from 1
                    //this is the row to start adding arrays to.
                    //Adding an array increases the CurrentRow property according to number of rows in the array
                    sheet.CurrentRow = 2;
                    sheet.AddArray(new object[,] { { "1", 2, 3, 111 }, { 10, "11", true, DateTime.Now } });

                    //working with second sheet
                    sheet = proc[1];
                    //numbering of cols and rows starts from 1
                    TableRange range = sheet.Range(3, 3, 5, 7);
                    //Notice values 6, 16, 26 wouldn't show - they are out of range
                    //OO hates wrong array sizes, but I fix it in wrapper (See OORange.Value)
                    range.Value = new object[,] { { 1, 2, 3, 4, 5, 6 }, { 11, 12, 13, 14, 15, 16 }, { 21, 22, 23, 24, 25, 26 } };
                    range.BackgroundColorIndex = 4;
                    range.FontBold = true;
                    range.FontColor = 0xff0000;
                    range.BorderAround(LineStyle.Double, BorderWeight.Thick, 0);
                    //auto fitting range only
                    range.AutoFitColumns();

                    //last page.
                    sheet = proc.LastPage;
                    range = sheet.Range(2, 1, 4, 10);
                    //Initializing inner array.
                    //It helps avoid converting array type for OO as in case with range.Value or AddArray
                    range.CreateArray();
                    range[0, 0] = 0;
                    range[1, 0] = "Ya krevedko ;-)";
                    range[2, 0] = .11 / .3;
                    for (int i = 1; i < 10; i++)
                    {
                        range[0, i] = i;
                        range[1, i] = i + 10;
                        range[2, i] = i + 20;
                    }
                    //writing inner array data to table processor
                    range.FlushArray();
                    //colorizing it
                    range.FontItalic = true;
                    range.FontSize = 16;
                    range.HAlign = HAlign.Right;
                    range.VAlign = VAlign.Top;

                    //auto fitting all page
                    sheet.AutoFitColumns();

                    //Creating a diagram
                    TableDiagram td = sheet.AddDiagram(sheet.Range(2, 1, 4, 10), RowsCols.Columns, DiagramType.Bar, sheet.Range(5, 2, 14, 20));
                    td.XAxisName = "X_really_x";
                    td.YAxisName = "definitely_Y";
                    td.Name = "ThisIsName";
                    //applying a range to get X axis names from. In OO it will just copy strings from specified 
                    //range, and set them as names for X axis items
                    td.XAxisNamesRange=sheet.Range(2, 1, 4, 1);
                    //moving a diagram to the range's coordinates
                    td.MoveToRectOf(sheet.Range(5, 1, 14, 20));

                    //creating diagrams of all supported types
                    for (int i = 0; i < 8; i++)
                    {
                        sheet.AddDiagram(sheet.Range(2, 1, 4, 10), RowsCols.Rows, (DiagramType)i, sheet.Range(15+i*15, 2, 24+i*15, 15));
                        sheet.Cell(15 + i * 15, 1).Value = Enum.GetName(typeof(DiagramType),(DiagramType)i);
                    }
                }
                finally
                {
                    //showing the result
                    proc.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Colorizing went wrong: " + ex.Message);
            }
        }
    }
}
