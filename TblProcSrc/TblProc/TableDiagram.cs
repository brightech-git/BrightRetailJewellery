
/********************************************************************************
* Copyright : Alexander Sazonov 2009                                           //
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

using System;
namespace SA.TblProc
{
    public abstract class TableDiagram
    {
        //SATODO: add common Delete/AddLegend methods
        public abstract string XAxisName { get; set; }
        public abstract string YAxisName { get; set; }
        public abstract string Name { get; set; }
        public abstract DrawRect DrawRect { get; set; }

        public abstract TableRange XAxisNamesRange {set;}
        public abstract void SetMainAxisNamesRange(TableRange range, TableSheet rangeSheet);
        public abstract string[] MainAxisNames { set;}
        //public abstract bool ShowLegend { set; }
       

        protected int GetOneDimensionRangeSize(TableRange range, int max)
        {
            bool vert = (range.Coords.Width == 1);
            int rangeSize = 0;
            if (vert)
                rangeSize = range.Coords.Height;
            else
                rangeSize = range.Coords.Width;

            return (max < rangeSize) ? max : rangeSize;
        }

        protected string[] GetOneDimensionRangeValues(TableRange range, int max, TableSheet sheet)
        {
            if (range.Coords.Height != 1 && range.Coords.Width != 1)
                throw new InvalidParameterException("Range, specifying data names, should have one dimension's length=1");
            bool vert = (range.Coords.Width == 1);
            int count = GetOneDimensionRangeSize(range, max);

            string[] ret = new string[count];

            TableRange tr = null;

            if (vert)
                tr = sheet.Range(range.StartRow, range.StartCol, range.StartRow + count - 1, range.StartCol);
            else
                tr = sheet.Range(range.StartRow, range.StartCol, range.StartRow, range.StartCol + count - 1);

            int addIdx = (this is Excel.ExcelDiagram) ? 1 : 0;
            Array vals = null;
            if (tr.Coords.Width != 1 || tr.Coords.Height != 1)
                vals = tr.Value as Array;
            else
            {
                //hand-made
                vals = new object[,] { { tr.Value } };
                addIdx = 0;
            }
            if (vals == null || vals.Length < count) return null;
            
            for (int i = 0; i < count; i++)
            {
                object o = null;
                if (vert) o = vals.GetValue(i + addIdx, addIdx);
                else o = vals.GetValue(addIdx, i + addIdx);
                if (o != null && o.ToString().Length != 0) ret[i] = o.ToString();
            }
            return ret;
        }

        public void MoveToRectOf(TableRange range)
        {
            DrawRect = range.DrawRect;
        }
    }

    public class AxisNotFoundException : ApplicationException
    {
        public AxisNotFoundException(string msg) : base(msg) { }
    }

    public class InvalidTableProcessorException : ApplicationException
    {
        public InvalidTableProcessorException(string msg) : base(msg) { }
    }

    public class ChartDataNotFoundException : ApplicationException
    {
        public ChartDataNotFoundException(string msg) : base(msg) { }
    }

    public class DiagramException : ApplicationException
    {
        public DiagramException(string msg) : base(msg) { }
    }

    public class SheetNotFoundException : ApplicationException
    {
        public SheetNotFoundException(string msg) : base(msg) { }
    }

    public class IAmSoSorryException : ApplicationException
    {
        public IAmSoSorryException(string msg) : base(msg) { }
    }

 
}

namespace SA.TblProc.Null
{
    public class TableDiagramNull: TableDiagram, INull
    {
        private static TableDiagramNull nullObj = new TableDiagramNull();
        private TableDiagramNull() { }
        public static TableDiagramNull Get() { return nullObj; }

        public override string XAxisName 
        {
            get { return ""; }
            set { }
        }
        public override string YAxisName
        {
            get { return ""; }
            set { }
        }
        public override string Name
        {
            get { return ""; }
            set { }
        }
        public override DrawRect DrawRect 
        {
            get { return null; }
            set { }
        }
        public override TableRange XAxisNamesRange { set { } }
        public override void SetMainAxisNamesRange(TableRange range, TableSheet rangeSheet) { }
        public override string[] MainAxisNames { set { } }
    }
}
