using System;
using System.Collections.Generic;
using System.Text;
using unoidl.com.sun.star.table;
using unoidl.com.sun.star.beans;
using unoidl.com.sun.star.sheet;
using unoidl.com.sun.star.text;
using unoidl.com.sun.star.util;

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

//An OO range wrapper class. Used to manipulate data and it's appearance within a page

namespace SA.TblProc.OO
{
    public class OORange : TableRange
    {
        private XCellRange _range;
        private OOSheet _page;
        private uno.Any[][] _data;

        public XCellRange XCellRange
        {
            get { return _range; }
        }

        public OOSheet Page
        {
            get { return _page; }
        }

        public OORange(CoordRange coordRange, XCellRange range, OOSheet page)
            : base(coordRange)
        {
            this._range = range;
            this._page = page;
        }
        public override void Merge()
        {
            XMergeable xm = _range as XMergeable;
            if (xm != null) xm.merge(true);
        }
        //supports zero-based indexes
        public override object Value
        {
            get
            {
                XCellRangeData crd = (XCellRangeData)_range;
                uno.Any[][] arr = crd.getDataArray();
                if (arr == null) return null;
                if (arr[0] == null) return null;
                int width = arr[0].Length;

                object[,] ret = new object[arr.Length, width];
                for (int y = 0; y < arr.Length; y++)
                {
                    for (int x = 0; x < width && x < arr[y].Length; x++)
                    {
                        ret[y, x] = arr[y][x].Value;
                    }
                }
                return ret;
            }
            set
            {
                Array val;
                if (!(value is Array))
                {
                    object[] newArr = new object[1];
                    newArr[0] = value;
                    val = newArr;
                }
                else
                {
                    val = value as Array;
                }
                if (val.Rank > 3) throw new TooManyDimensionsException("too many dimansions in OORange.value");
                int cols = 1, rows = 1;
                bool singleString = (val.Rank == 1);
                if (singleString)
                {
                    cols = val.Length;
                }
                else
                {
                    rows = val.GetLength(0);
                    cols = val.GetLength(1);
                }
                if (rows > (_coords.EndRow - _coords.StartRow + 1))
                    rows = _coords.EndRow - _coords.StartRow + 1;
                if (cols > (_coords.EndCol - _coords.StartCol + 1))
                    cols = _coords.EndCol - _coords.StartCol + 1;
                uno.Any[][] arr = new uno.Any[_coords.EndRow - _coords.StartRow + 1][];

                for (int y = 0; y < _coords.EndRow - _coords.StartRow + 1; y++)
                    arr[y] = new uno.Any[_coords.EndCol - _coords.StartCol + 1];

                for (int y = 0; y < rows; y++)
                {
                    for (int x = 0; x < cols; x++)
                    {
                        object oVal;
                        if (singleString)
                            oVal = val.GetValue(x);
                        else
                            oVal = val.GetValue(y, x);
                        arr[y][x] = Conv(oVal);
                    }
                }
                XCellRangeData crd = (XCellRangeData)_range;
                crd.setDataArray(arr);
            }
        }

        private uno.Any Conv(object val)
        {
            if (val == null) val = "";
            System.Type type = val.GetType();
            if(val.GetType()==typeof(decimal))
            {
                return new uno.Any(typeof(double), Convert.ToDouble((decimal)val));
            }
            if(val.GetType()==typeof(System.DateTime))
            {
                System.DateTime csDate = (System.DateTime)val;
                string date = csDate.ToShortDateString();
                return new uno.Any(typeof(string), date);
            }
            if(val.GetType()==typeof(bool))
            {
                return new uno.Any(typeof(string), ((bool)val==true)?"TRUE":"FALSE");
            }
            return new uno.Any(type, val);
        }

        private XPropertySet Props
        {
            get
            {
                return (XPropertySet)_range;
            }
        }

        public override bool FontBold
        {
            set
            {
                if (value) Props.setPropertyValue("CharWeight", new uno.Any((int)unoidl.com.sun.star.awt.FontWeight.BOLD));
                else Props.setPropertyValue("CharWeight", new uno.Any((int)unoidl.com.sun.star.awt.FontWeight.NORMAL));
            }
        }


        public override bool FontItalic
        {
            set
            {
                if (value) Props.setPropertyValue("CharPosture", new uno.Any((int)unoidl.com.sun.star.awt.FontSlant.ITALIC));
                else Props.setPropertyValue("CharPosture", new uno.Any((int)unoidl.com.sun.star.awt.FontSlant.NONE));
            }
        }


        public override int BackgroundColor
        {
            set
            {
                Props.setPropertyValue("CellBackColor", new uno.Any(value));
            }
        }

        public override int FontColor
        {
            get
            {
                return (int)Props.getPropertyValue("CharColor").Value;
            }
            set
            {
                Props.setPropertyValue("CharColor", new uno.Any(value));
            }
        }

        public override int FontColorIndex
        {
            get
            {
                return ConstConvert.FindClosestColorIndex(FontColor);
            }
            set
            {
                FontColor = ConstConvert.ColorIndexToRGB(value);
            }
        }


        public override int FontSize
        {
            get
            {
                return Convert.ToInt32(Props.getPropertyValue("CharHeight").Value);
            }
            set
            {
                Props.setPropertyValue("CharHeight", new uno.Any((int)value));
            }
        }

        public override HAlign HAlign
        {
            set
            {
                Props.setPropertyValue("HoriJustify", new uno.Any(ConstConvert.ToOO(value)));
            }
        }

        public override VAlign VAlign
        {
            set
            {
                Props.setPropertyValue("VertJustify", new uno.Any(ConstConvert.ToOO(value)));
            }
        }

        public override string FontName
        {
            get
            {
                object name = Props.getPropertyValue("CharFontName").Value;
                if (name == null) return null;
                return name.ToString();
            }
            set
            {
                Props.setPropertyValue("CharFontName", new uno.Any(value));
            }
        }
        public override bool WrapText
        {
            set
            {
                Props.setPropertyValue("IsTextWrapped", new uno.Any(value));
            }
        }

        public override string NumberFormat
        {
            set
            {
                int nFmt = _page.Application.GetNumberFormatCode(value);
                Props.setPropertyValue("NumberFormat", new uno.Any((Int32)nFmt));

            }
        }




        public override int Orientation
        {
            set
            {
                Props.setPropertyValue("RotateAngle", new uno.Any(value * 100));
            }
        }

        public override void BorderAround(LineStyle lineStyle, BorderWeight borderWeight, object color)
        {
            BorderLine line = new BorderLine();
            try
            {
                int col = (int)color;
                line.Color = col;
            }
            catch (System.Exception) { }

            line.OuterLineWidth = ConstConvert.ToOO(borderWeight);//типа 70, 100 и т.п.
            TableBorder bord = new TableBorder();
            bord.TopLine = bord.BottomLine = bord.LeftLine = bord.RightLine = line;
            bord.IsVerticalLineValid = bord.IsHorizontalLineValid = false;
            bord.IsBottomLineValid = bord.IsTopLineValid = true;
            bord.IsLeftLineValid = bord.IsRightLineValid = true;
            Props.setPropertyValue("TableBorder", new uno.Any(typeof(TableBorder), bord));
        }

        public override void SetBorderWeight(BordersIndex index, BorderWeight weight)
        {
            BorderLine line = new BorderLine();
            line.OuterLineWidth = ConstConvert.ToOO(weight);
            TableBorder bord = new TableBorder();
            switch (index)
            {
                case BordersIndex.EdgeLeft:
                    bord.LeftLine = line;
                    bord.IsLeftLineValid = true;
                    break;
                case BordersIndex.EdgeTop:
                    bord.TopLine = line;
                    bord.IsTopLineValid = true;
                    break;
                case BordersIndex.EdgeBottom:
                    bord.BottomLine = line;
                    bord.IsBottomLineValid = true;
                    break;
                case BordersIndex.EdgeRight:
                    bord.RightLine = line;
                    bord.IsRightLineValid = true;
                    break;
                case BordersIndex.InsideVertical:
                    bord.VerticalLine = line;
                    bord.IsVerticalLineValid = true;
                    break;
                case BordersIndex.InsideHorizontal:
                    bord.HorizontalLine = line;
                    bord.IsHorizontalLineValid = true;
                    break;
            }
            Props.setPropertyValue("TableBorder", new uno.Any(typeof(TableBorder), bord));
        }

        public override void AutoFitColumns()
        {
            XColumnRowRange rCol = (XColumnRowRange)_range;
            XTableColumns tCol = rCol.getColumns();
            int colNum = tCol.getCount();
            for (int i = 0; i < colNum; i++)
            {
                uno.Any col = tCol.getByIndex(i);
                XPropertySet xPropSet = (XPropertySet)col.Value;
                xPropSet.setPropertyValue("OptimalWidth", new uno.Any((bool)true));
            }
        }

        public override double ColumnWidth
        {
            get
            {
                XColumnRowRange rCol = (XColumnRowRange)_range;
                XTableColumns tCol = rCol.getColumns();
                int colNum = tCol.getCount();
                int? width = null;
                for (int i = 0; i < colNum; i++)
                {
                    uno.Any col = tCol.getByIndex(i);
                    XPropertySet xPropSet = (XPropertySet)col.Value;
                    object val = xPropSet.getPropertyValue("Width").Value;
                    if (val != null)
                    {
                        try
                        {
                            int iVal = (int)val;
                            if (width == null) width = iVal;
                            if (iVal != (int)width) return 0;
                        }
                        catch (System.Exception) { return 0; }
                    }
                }
                if (width == null) return 0;
                return ((double)width) / 1000;
            }
            set
            {
                XColumnRowRange rCol = (XColumnRowRange)_range;
                XTableColumns tCol = rCol.getColumns();
                int colNum = tCol.getCount();
                for (int i = 0; i < colNum; i++)
                {
                    uno.Any col = tCol.getByIndex(i);
                    XPropertySet xPropSet = (XPropertySet)col.Value;
                    xPropSet.setPropertyValue("Width", new uno.Any((int)value * 1000));
                }
            }
        }

        public override int BackgroundColorIndex
        {
            set
            {
                BackgroundColor = ConstConvert.ColorIndexToRGB(value);
            }
        }

        public override void SetCharFontBold(int start, int length, bool set)
        {
            for (int x = _coords.StartCol - 1; x < _coords.EndCol; x++)
            {
                for (int y = _coords.StartRow - 1; y < _coords.EndRow; y++)
                {
                    XCell cell = _range.getCellByPosition(x, y);
                    XText text = (XText)cell;
                    XTextCursor curs = text.createTextCursor();
                    curs.gotoStart(false);
                    try
                    {
                        if (!curs.goRight((short)start, false)) continue;
                        if (!curs.goRight((short)length, true)) continue;
                        XPropertySet xCursorProps = (XPropertySet)curs;
                        if (set) xCursorProps.setPropertyValue("CharWeight", new uno.Any((int)unoidl.com.sun.star.awt.FontWeight.BOLD));
                        else xCursorProps.setPropertyValue("CharWeight", new uno.Any((int)unoidl.com.sun.star.awt.FontWeight.NORMAL));

                    }
                    catch (System.Exception) { }
                }
            }
        }

        public override void CreateArray()
        {
            _data = new uno.Any[_coords.Height][];
            for (int i = 0; i < _coords.Height; i++) _data[i] = new uno.Any[_coords.Width];
        }
        public override object this[int row, int col]
        {
            get
            {
                return _data[row][col].Value;
            }
            set
            {
                _data[row][col] = Conv(value);
            }
        }
        public override void FlushArray()
        {
            XCellRangeData crd = (XCellRangeData)_range;
            crd.setDataArray(_data);
        }

        public override DrawRect DrawRect 
        { 
            get 
            {
                if (_coords.Width == 0 || _coords.Height == 0)
                {
                    return new DrawRect(0, 0, 0, 0);
                }
                XCell cell = _range.getCellByPosition(0, 0);
                XPropertySet cellProps = cell as XPropertySet;
                unoidl.com.sun.star.awt.Point pos = cellProps.getPropertyValue("Position").Value as unoidl.com.sun.star.awt.Point;

                XColumnRowRange rColRow = (XColumnRowRange)_range;
                XTableColumns cols = rColRow.getColumns();

                double width=0;
                for(int i=0;i<cols.getCount();i++)
                {
                    XPropertySet xps= cols.getByIndex(i).Value as XPropertySet;
                    width+=Convert.ToDouble(xps.getPropertyValue("Width").Value);
                }
                XTableRows rows = rColRow.getRows();
                double height = 0;
                for (int i = 0; i < rows.getCount(); i++)
                {
                    XPropertySet xps = rows.getByIndex(i).Value as XPropertySet;
                    height += Convert.ToDouble(xps.getPropertyValue("Height").Value);
                }
                
                return new DrawRect(pos.X, pos.Y, pos.X+width, pos.Y+height);
            } 
        }

        //UNIMPLEMENTED
        public override void SetBorderLineStyle(BordersIndex index, LineStyle style)
        {
            //no main line styles in OO
        }

        


    }
}
