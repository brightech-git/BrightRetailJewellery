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

//Abstract cell range interface and some logic. Use to avoid
//concrete table processor dependency

namespace SA.TblProc
{
    public abstract class TableRange
    {
        protected CoordRange _coords;
        public int StartRow { get { return _coords.StartRow; } }
        public int StartCol { get { return _coords.StartCol; } }
        public int EndRow { get { return _coords.EndRow; } }
        public int EndCol { get { return _coords.EndCol; } }

        public TableRange(CoordRange range)
        {
            _coords = range;
        }

        public CoordRange Coords
        {
            get { return _coords; }
        }

        public abstract void Merge();
        public abstract void AutoFitColumns();
        public abstract object Value { get; set; }
        public abstract bool FontBold { set; }
        public abstract bool FontItalic { set; }
        public abstract int BackgroundColor { set; }
        public abstract HAlign HAlign { set; }
        public abstract VAlign VAlign { set; }
        public abstract int FontSize { get; set; }
        public abstract string FontName { get; set; }
        public abstract int FontColor { get; set; }
        public abstract int FontColorIndex { get; set; }
        public abstract string NumberFormat { set; }
        public abstract double ColumnWidth { get; set; }
        public abstract bool WrapText { set; }
        public abstract void BorderAround(LineStyle lineStyle, BorderWeight borderWeight, object color);
        public abstract int Orientation { set; }//in degrees
        public abstract int BackgroundColorIndex { set; }
        public abstract void SetBorderLineStyle(BordersIndex index, LineStyle style);
        public abstract void SetBorderWeight(BordersIndex index, BorderWeight weight);
        public abstract void SetCharFontBold(int start, int end, bool set);
        public abstract void CreateArray();
        public abstract object this[int row, int col] { get; set; }
        public abstract void FlushArray();
        public abstract DrawRect DrawRect { get; }
    }
}

namespace SA.TblProc.Null
{
    public class TableRangeNull : TableRange, INull
    {
        private static TableRangeNull nullObj = new TableRangeNull();
        private TableRangeNull() : base(new CoordRange(1, 1, 1, 1)) { }
        public static TableRangeNull Get() { return nullObj; }
        public override void Merge() { }
        public override void AutoFitColumns() { }
        public override object Value { get { return null; } set { } }
        public override bool FontBold { set { } }
        public override bool FontItalic { set { } }
        public override int BackgroundColor { set { } }
        public override HAlign HAlign { set { } }
        public override VAlign VAlign { set { } }
        public override int FontSize { get { return 12; } set { } }
        public override string FontName { get { return ""; } set { } }
        public override int FontColor { get { return 0; } set { } }
        public override int FontColorIndex { get { return 0; } set { } }
        public override string NumberFormat { set { } }
        public override double ColumnWidth { get { return 1; } set { } }
        public override bool WrapText { set { } }
        //public override void SetValue(Array data) { }
        public override void BorderAround(LineStyle lineStyle, BorderWeight borderWeight, object color) { }
        public override int Orientation { set { } }
        public override int BackgroundColorIndex { set { } }
        public override void SetBorderLineStyle(BordersIndex index, LineStyle style) { }
        public override void SetBorderWeight(BordersIndex index, BorderWeight weight) { }
        public override void SetCharFontBold(int start, int end, bool set) { }
        public override void CreateArray() { }
        public override object this[int row, int col] { get { return null; } set { } }
        public override void FlushArray() { }
        public override DrawRect DrawRect { get { return null; } }

    }
}
