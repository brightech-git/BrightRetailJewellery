using System;
using System.Collections.Generic;
using System.Text;

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

//Abstract document sheet (page) interface and logic. Use to avoid
//concrete table processor dependency

namespace SA.TblProc
{
    public abstract class TableSheet
    {
        protected int _curRow = 1;
        public int CurrentRow
        {
            get { return _curRow; }
            set { _curRow = value; }
        }



        public TableSheet() { }
        public TableSheet(int startRow)
        {
            _curRow = startRow;
        }

        public static TableSheet CreateAvailable(string name)
        {
            return CreateAvailable(name, 1);
        }
        public static TableSheet CreateAvailable(string name, int startRow)
        {
            return TableProcessor.CreateAvailable(name, startRow).FirstPage;
        }


        public abstract string Name { get; set;}

        public virtual TableRange Cell(int row, int col)
        {
            return Range(new CoordRange(row, col, row, col));
        }
        public virtual TableRange Range(int startRow, int startCol, int endRow, int endCol)
        {
            return Range(new CoordRange(startRow, startCol, endRow, endCol));
        }
        public abstract TableRange Range(CoordRange range);

        public virtual void AddArray(Array data)
        {
            AddArray(data, false);
        }
        public virtual TableRange AddArray(Array data, bool bold)
        {
            if (data.Rank > 2)
                throw new TooManyDimensionsException("Too much dimensions in the array: " + data.Rank);
            int colCount = 1;
            int rowCount = 1;
            if (data.Rank == 2)
            {
                colCount = data.GetLength(1);
                rowCount = data.GetLength(0);
            }
            else
            {
                colCount = data.GetLength(0);
            }
            TableRange rng = Range(_curRow, 1, _curRow + rowCount - 1, colCount);
            rng.Value = data;
            if (bold) rng.FontBold = true;
            _curRow += rowCount;
            return rng;
        }
        public virtual TableRange BorderAround(int firstRow, int firstCol, int endRow, int endCol, LineStyle lineStyle, BorderWeight borderWeight)
        {
            return BorderAround(new CoordRange(firstRow, firstCol, endRow, endCol), lineStyle, borderWeight);
        }
        public virtual TableRange BorderAround(CoordRange range, LineStyle lineStyle, BorderWeight borderWeight)
        {
            return BorderAround(range, lineStyle, borderWeight, 0);
        }
        public virtual TableRange BorderAround(int firstRow, int firstCol, int endRow, int endCol, LineStyle lineStyle, BorderWeight borderWeight, object color)
        {
            return BorderAround(new CoordRange(firstRow, firstCol, endRow, endCol), lineStyle, borderWeight, color);
        }
        public abstract TableRange BorderAround(CoordRange range, LineStyle lineStyle, BorderWeight borderWeight,
            object color);

        public abstract void AutoFitColumns();
        public abstract bool Visible { set; }
        public TableRange MergeSetValue(CoordRange range, object value, HAlign align)
        {
            TableRange cr = Range(range);
            cr.Merge();
            cr.Value = value;
            if (align != HAlign.General) cr.HAlign = align;
            return cr;
        }
        public TableRange AddArray(CoordRange range, object[] aVal, int? colorIndex, HAlign align, bool bold, bool italic)
        {
            TableRange cr = Range(range);
            cr.Value = aVal;
            if (HAlign.General != align) cr.HAlign = align;
            if (colorIndex != null) cr.BackgroundColorIndex = (int)colorIndex;
            if (bold) cr.FontBold = true;
            if (italic) cr.FontItalic = true;
            return cr;
        }
        public abstract void AddPageNumbering();
        public abstract PageOrientation Orientation { set; }
        public abstract void PrintRowsOnEachPage(int startRow, int endRow);
        public abstract int FitToPagesWide { set; }
        public abstract int FitToPagesTall { set; }

        public abstract TableDiagram AddDiagram(TableRange dataRange, RowsCols rowsCols, DiagramType type, DrawRect rect);

        public TableDiagram AddDiagram(TableRange dataRange, RowsCols rowsCols, DiagramType type)
        {
            return AddDiagram(dataRange, rowsCols, type, null as DrawRect);
        }
        public TableDiagram AddDiagram(TableRange dataRange, RowsCols rowsCols, DiagramType type, TableRange rangeOfRect)
        {
            return AddDiagram(dataRange, rowsCols, type, rangeOfRect.DrawRect);
        }

    }

    public class TooManyDimensionsException : ApplicationException
    {
        public TooManyDimensionsException(string msg)
            : base(msg)
        { }
    }
}

namespace SA.TblProc.Null
{
    public class TableSheetNull : TableSheet, INull
    {
        private static TableSheetNull nullObj = new TableSheetNull();
        private TableSheetNull() { }
        public static TableSheetNull Get() { return nullObj; }
        public override string Name
        { get { return ""; } set { } }

        public override TableRange Range(CoordRange range)
        { return TableRangeNull.Get(); }

        public override TableRange BorderAround(CoordRange range, LineStyle lineStyle, BorderWeight borderWeight,
            object color) { return TableRangeNull.Get(); }

        public override void AutoFitColumns() { }
        public override bool Visible { set { } }
        public override void AddPageNumbering() { }
        public override PageOrientation Orientation { set { } }
        public override void PrintRowsOnEachPage(int startRow, int endRow) { }
        public override int FitToPagesWide { set { } }
        public override int FitToPagesTall { set { } }
        public override TableDiagram AddDiagram(TableRange dataRange, RowsCols rowsCols, DiagramType type, DrawRect rect)
        { return TableDiagramNull.Get(); }
    }
}
