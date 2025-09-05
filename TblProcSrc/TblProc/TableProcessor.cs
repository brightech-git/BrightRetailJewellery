using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;

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

//Abstract table processor interface and sum logic. Use this to avoid concrete 
//table processor dependencies.

namespace SA.TblProc
{
    public abstract class TableProcessor
    {
        public abstract TableSheet FirstPage { get; }
        public abstract TableSheet LastPage { get; }
        public abstract TableSheet CreateNextPage(string name, int startRow);
        public virtual TableSheet CreateNextPage(string name)
        {
            return CreateNextPage(name, 1);
        }
        public abstract TableSheet this[int idx] { get; }

        public static TableProcessor CreateAvailable()
        {
            return CreateAvailable(null, 1);
        }
        public static TableProcessor CreateAvailable(string name)
        {
            return CreateAvailable(name, 1);
        }
        public static bool OOAvailable
        {
            get
            {
                using (RegistryKey rk = Registry.ClassesRoot)
                {
                    using (RegistryKey sub = rk.OpenSubKey("com.sun.star.ServiceManager"))
                    {
                        return (sub != null);   
                    }
                }
            }
        }
        public static bool ExcelAvailable
        {
            get
            {
                using (RegistryKey rk = Registry.ClassesRoot)
                {
                    using (RegistryKey sub = rk.OpenSubKey("Excel.Application"))
                    {
                        return (sub != null);
                    }
                }
            }
        }
        public static TableProcessor CreateAvailable(string name, int startRow)
        {
            if (ExcelAvailable) return CreateExcel(name, startRow);
            if (OOAvailable) return CreateOO(name, startRow);
            throw new NoTableProcessorsAvailableException("Не найден Microsoft Excel или Open Office Calc");
        }
        public static Excel.ExcelApp CreateExcel(string name, int startRow)
        {
            return new Excel.ExcelApp(name, startRow);
        }
        public static OO.OOApp CreateOO(string name, int startRow)
        {
            return new OO.OOApp(name, startRow);
        }

        public abstract bool Visible { set; }

    }
    

    public class NoTableProcessorsAvailableException : ApplicationException
    {
        public NoTableProcessorsAvailableException(string msg) : base(msg) { }
    }
}

namespace SA.TblProc.Null
{
    public class TableProcessorNull : TableProcessor, INull
    {
        private static TableProcessorNull nullObj = new TableProcessorNull();
        private TableProcessorNull() { }
        public static TableProcessorNull Get() { return nullObj; }

        public override TableSheet FirstPage
        { get { return TableSheetNull.Get(); } }
        public override TableSheet LastPage
        { get { return TableSheetNull.Get(); } }
        public override TableSheet CreateNextPage(string name, int startRow)
        { return TableSheetNull.Get(); }
        public override TableSheet this[int idx]
        { get { return TableSheetNull.Get(); } }

        public override bool Visible { set { } }
    }
}
