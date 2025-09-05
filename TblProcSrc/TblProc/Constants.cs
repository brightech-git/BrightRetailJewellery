using System;
using System.Collections.Generic;
using System.Text;
using MSExcel = Excel;

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

//Enum definitions and converter to avoid table processor dependency


namespace SA.TblProc
{
    public enum HAlign
    {
        Right,
        Left,
        //Justify,
        //Distributed,
        Center,
        General,
        //Fill,
        CenterAcrossSelection,
    };
    public enum VAlign
    {
        Top,
        //Justify,
        //Distributed,
        Center,
        Bottom,
    }

    public enum LineStyle
    {
        None,
        Double,
        Dot,
        Dash,
        Continuous,
        DashDot,
        DashDotDot,
        SlantDashDot,
    }

    public enum BorderWeight
    {
        Medium,
        Hairline,
        Thin,
        Thick,
    }

    public enum PageOrientation
    {
        Portrait,
        Landscape,
    }
    public enum BordersIndex
    {
        //DiagonalDown,
        //DiagonalUp,
        EdgeLeft,
        EdgeTop,
        EdgeBottom,
        EdgeRight,
        InsideVertical,
        InsideHorizontal,
    }

    public enum RowsCols
    {
        Columns,
        Rows
    }

    public enum DiagramType
    {
        Bar,
        Area,
        Line,
        Pie,
        Donut,
        Net,
        XY,
        Stock
    }
    public class ConstConvert
    {
        public static MSExcel.XlRowCol ToExcel(RowsCols rowsCols)
        {
            switch (rowsCols)
            {
                case RowsCols.Columns:
                    return MSExcel.XlRowCol.xlColumns;
            }
            return MSExcel.XlRowCol.xlRows;
        }
        public static unoidl.com.sun.star.chart.ChartDataRowSource ToOO(RowsCols rowsCols)
        {
            if(rowsCols==RowsCols.Columns)
                return unoidl.com.sun.star.chart.ChartDataRowSource.COLUMNS;
            return unoidl.com.sun.star.chart.ChartDataRowSource.ROWS;
        }
        public static MSExcel.XlChartType ToExcel(DiagramType dt)
        {
            
            switch(dt)
            {
                case DiagramType.Bar:
                    return MSExcel.XlChartType.xlColumnClustered;
                case DiagramType.Area:
                    return MSExcel.XlChartType.xlAreaStacked;
                case DiagramType.Line:
                    return MSExcel.XlChartType.xlLine;
                case DiagramType.Pie:
                    return MSExcel.XlChartType.xlPie;
                case DiagramType.Donut:
                    return MSExcel.XlChartType.xlDoughnut;
                case DiagramType.Net:
                    return MSExcel.XlChartType.xlRadarMarkers;
                case DiagramType.XY:
                    return MSExcel.XlChartType.xlXYScatterLines;
                case DiagramType.Stock:
                    return MSExcel.XlChartType.xlStockHLC;
            }
            throw new InvalidParameterException("Unknown DiagramType value");
        }
        public static MSExcel.XlHAlign ToExcel(HAlign align)
        {
            switch (align)
            {
                case HAlign.Right:
                    return MSExcel.XlHAlign.xlHAlignRight;
                case HAlign.Left:
                    return MSExcel.XlHAlign.xlHAlignLeft;
                //case HAlign.Justify:
                //    return Excel.XlHAlign.xlHAlignJustify;
                //case HAlign.Distributed:
                //    return Excel.XlHAlign.xlHAlignDistributed;
                case HAlign.Center:
                    return MSExcel.XlHAlign.xlHAlignCenter;
                case HAlign.General:
                    return MSExcel.XlHAlign.xlHAlignGeneral;
                //case HAlign.Fill:
                //    return Excel.XlHAlign.xlHAlignFill;
                case HAlign.CenterAcrossSelection:
                    return MSExcel.XlHAlign.xlHAlignCenterAcrossSelection;
            }
            throw new InvalidParameterException("Unknown HAlign value");
        }
        public static MSExcel.XlVAlign ToExcel(VAlign align)
        {
            switch (align)
            {
                case VAlign.Top:
                    return MSExcel.XlVAlign.xlVAlignTop;
                //case VAlign.Justify:
                //    return Excel.XlVAlign.xlVAlignJustify;
                //case VAlign.Distributed:
                //    return Excel.XlVAlign.xlVAlignDistributed;
                case VAlign.Center:
                    return MSExcel.XlVAlign.xlVAlignCenter;
                case VAlign.Bottom:
                    return MSExcel.XlVAlign.xlVAlignBottom;
            }
            throw new InvalidParameterException("Unknown VAlign value");
        }
        public static MSExcel.XlLineStyle ToExcel(LineStyle style)
        {
            switch (style)
            {
                case LineStyle.None:
                    return MSExcel.XlLineStyle.xlLineStyleNone;
                case LineStyle.Double:
                    return MSExcel.XlLineStyle.xlDouble;
                case LineStyle.Dot:
                    return MSExcel.XlLineStyle.xlDot;
                case LineStyle.Dash:
                    return MSExcel.XlLineStyle.xlDash;
                case LineStyle.Continuous:
                    return MSExcel.XlLineStyle.xlContinuous;
                case LineStyle.DashDot:
                    return MSExcel.XlLineStyle.xlDashDot;
                case LineStyle.DashDotDot:
                    return MSExcel.XlLineStyle.xlDashDotDot;
                case LineStyle.SlantDashDot:
                    return MSExcel.XlLineStyle.xlSlantDashDot;
            }
            throw new InvalidParameterException("Unknown LineStyle value");
        }
        public static MSExcel.XlBorderWeight ToExcel(BorderWeight weight)
        {
            switch (weight)
            {
                case BorderWeight.Medium:
                    return MSExcel.XlBorderWeight.xlMedium;
                case BorderWeight.Hairline:
                    return MSExcel.XlBorderWeight.xlHairline;
                case BorderWeight.Thin:
                    return MSExcel.XlBorderWeight.xlThin;
                case BorderWeight.Thick:
                    return MSExcel.XlBorderWeight.xlThick;
            }
            throw new InvalidParameterException("Unknown BorderWeight value");
        }
        public static MSExcel.XlPageOrientation ToExcel(PageOrientation po)
        {
            switch (po)
            {
                case PageOrientation.Portrait:
                    return MSExcel.XlPageOrientation.xlPortrait;
                case PageOrientation.Landscape:
                    return MSExcel.XlPageOrientation.xlLandscape;
            }
            throw new InvalidParameterException("Unknown PageOrientation value");
        }
        public static MSExcel.XlBordersIndex ToExcel(BordersIndex bi)
        {
            switch (bi)
            {
                //case BordersIndex.DiagonalDown:
                //    return Excel.XlBordersIndex.xlDiagonalDown;
                //case BordersIndex.DiagonalUp:
                //    return Excel.XlBordersIndex.xlDiagonalUp;
                case BordersIndex.EdgeLeft:
                    return MSExcel.XlBordersIndex.xlEdgeLeft;
                case BordersIndex.EdgeTop:
                    return MSExcel.XlBordersIndex.xlEdgeTop;
                case BordersIndex.EdgeBottom:
                    return MSExcel.XlBordersIndex.xlEdgeBottom;
                case BordersIndex.EdgeRight:
                    return MSExcel.XlBordersIndex.xlEdgeRight;
                case BordersIndex.InsideVertical:
                    return MSExcel.XlBordersIndex.xlInsideVertical;
                case BordersIndex.InsideHorizontal:
                    return MSExcel.XlBordersIndex.xlInsideHorizontal;

            }
            throw new InvalidParameterException("Unknown BordersIndex value");
        }

        public static int ColorIndexToRGB(int index)
        {
            if (index < 1 || index > colors.Length) throw new InvalidParameterException("Unknown color index value");
            return colors[index - 1];
        }

        public static int ToOO(HAlign align)
        {
            switch (align)
            {
                case HAlign.Right:
                    return (Int32)unoidl.com.sun.star.table.CellHoriJustify.RIGHT;
                case HAlign.Left:
                    return (Int32)unoidl.com.sun.star.table.CellHoriJustify.LEFT;
                case HAlign.Center:
                    return (Int32)unoidl.com.sun.star.table.CellHoriJustify.CENTER;
                case HAlign.General:
                    return (Int32)unoidl.com.sun.star.table.CellHoriJustify.STANDARD;
            }
            throw new InvalidParameterException("Unknown HAlign value");
        }

        public static int ToOO(VAlign align)
        {
            switch (align)
            {
                case VAlign.Top:
                    return (Int32)unoidl.com.sun.star.table.CellVertJustify.TOP;
                case VAlign.Center:
                    return (Int32)unoidl.com.sun.star.table.CellVertJustify.CENTER;
                case VAlign.Bottom:
                    return (Int32)unoidl.com.sun.star.table.CellVertJustify.BOTTOM;
            }
            throw new InvalidParameterException("Unknown VAlign value");
        }

        public static short ToOO(BorderWeight weight)
        {
            switch (weight)
            {
                case BorderWeight.Hairline:
                    return 10;
                case BorderWeight.Thin:
                    return 40;
                case BorderWeight.Medium:
                    return 70;
                case BorderWeight.Thick:
                    return 100;
            }
            throw new InvalidParameterException("Unknown BorderWeight value");
        }

        private static int ColorDistance(System.Drawing.Color col1, System.Drawing.Color col2)
        {
            return Math.Abs(col1.R - col2.R) + Math.Abs(col1.G - col2.G) + Math.Abs(col1.B - col2.B);
        }

        public static int FindClosestColorIndex(int color)
        {
            int minDistance = 0x1000000;
            int ret = 0;
            System.Drawing.Color requested = System.Drawing.Color.FromArgb(color);
            for (int i = 0; i < colors.Length; i++)
            {
                System.Drawing.Color col = System.Drawing.Color.FromArgb(colors[i]);
                int dist = ColorDistance(requested, col);
                if (dist < minDistance)
                {
                    minDistance = dist;
                    ret = i;
                }
            }
            return ret + 1;
        }

        private static int[] colors =
            {
                0x000000,
                0xFFFFFF,
                0xFF0000,
                0x00FF00,
                0x0000FF,
                0xFFFF00,
                0xFF00FF,
                0x00FFFF,
                0x800000,
                0x008000,
                0x000080,
                0x808000,
                0x800080,
                0x008080,
                0xC0C0C0,
                0x808080,
                0x9999FF,
                0x993366,
                0xFFFFCC,
                0xCCFFFF,
                0x660066,
                0xFF8080,
                0x0066CC,
                0xCCCCFF,
                0x000080,
                0xFF00FF,
                0xFFFF00,
                0x00FFFF,
                0x800080,
                0x800000,
                0x008080,
                0x0000FF,
                0x00CCFF,
                0xCCFFFF,
                0xCCFFCC,
                0xFFFF99,
                0x99CCFF,
                0xFF99CC,
                0xCC99FF,
                0xFFCC99,
                0x3366FF,
                0x33CCCC,
                0x99CC00,
                0xFFCC00,
                0xFF9900,
                0xFF6600,
                0x666699,
                0x969696,
                0x003366,
                0x339966,
                0x003300,
                0x333300,
                0x993300,
                0x993366,
                0x333399,
                0x333333
            };


    }

    public class InvalidParameterException : ApplicationException
    {
        public InvalidParameterException(string msg) : base(msg) { }
    }

    public class InvalidDiagramTypeException : ApplicationException
    {
        public InvalidDiagramTypeException(string msg) : base(msg) { }
    }
}
