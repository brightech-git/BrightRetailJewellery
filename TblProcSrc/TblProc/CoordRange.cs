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

//Simple coord range class

namespace SA.TblProc
{
    public class CoordRange
    {
        int _startRow;
        int _startCol;
        int _endRow;
        int _endCol;

        public int StartRow { get { return _startRow; } }
        public int StartCol { get { return _startCol; } }
        public int EndRow { get { return _endRow; } }
        public int EndCol { get { return _endCol; } }

        public CoordRange(int startRow, int startCol, int endRow, int endCol)
        {
            this._startRow = startRow;
            this._startCol = startCol;
            this._endRow = endRow;
            this._endCol = endCol;
        }

        public CoordRange(CoordRange range) : this(range._startRow, range._startCol, range._endRow, range._endRow) { }

        public int Width
        {
            get { return _endCol - _startCol + 1; }
        }
        public int Height
        {
            get { return _endRow - _startRow + 1; }
        }

    }

    public class DrawRect
    {
        double _left;
        double _top;
        double _right;
        double _bottom;

        public double Left { get { return _left; } }
        public double Top { get { return _top; } }
        public double Right { get { return _right; } }
        public double Bottom { get { return _bottom; } }

        public DrawRect(double left, double top, double right, double bottom)
        {
            this._left = left;
            this._top = top;
            this._right = right;
            this._bottom = bottom;
        }

        public DrawRect(DrawRect range) : this(range._left, range._top, range._right, range._bottom) { }

        public double Width
        {
            get { return _right - _left; }
        }
        public double Height
        {
            get { return _bottom - _top; }
        }

    }
}
