using System;
using System.Collections.Generic;
using System.Text;
using unoidl.com.sun.star.sheet;
using unoidl.com.sun.star.lang;
using unoidl.com.sun.star.frame;
using unoidl.com.sun.star.container;
using System.Collections;
using unoidl.com.sun.star.util;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.IO;

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

//An OpenOffice calc table processor wrapper class. Represents calc's document

namespace SA.TblProc.OO
{
    public class OOApp : TableProcessor
    {
        private XSpreadsheetDocument _doc = null;
        private List<OOSheet> _pages = new List<OOSheet>();
        private Dictionary<string, int> _numberFormats = new Dictionary<string, int>();
        public XSpreadsheetDocument XSpreadsheetDocument
        {
            get { return _doc; }
        }


        public OOApp() : this(null, 1) { }
        public OOApp(string firstPageName) : this(firstPageName, 1) { }

        private static XSpreadsheetDocument RunOO2x()
        {
            unoidl.com.sun.star.uno.XComponentContext localContext =
                uno.util.Bootstrap.bootstrap();
            XMultiServiceFactory multiServiceFactory =
                            (XMultiServiceFactory)
                            localContext.getServiceManager();
            XComponentLoader componentLoader =
                            (XComponentLoader)multiServiceFactory.createInstance(
                                "com.sun.star.frame.Desktop");
            XComponent xComponent = componentLoader.loadComponentFromURL(
                //"private:factory/swriter",	//пустой документ writer
                            "private:factory/scalc",	//пустой документ calc
                            "_blank", 0,		//в пустом фрейме без searchflag
                //без дополнительных аргументов.
                            new unoidl.com.sun.star.beans.PropertyValue[0]
                            );
            return xComponent as unoidl.com.sun.star.sheet.XSpreadsheetDocument;
        }

        private static void ConfigureOO3x()
        {
            string baseKey;  
            // OpenOffice being a 32 bit app, its registry location is different in a 64 bit OS  
            if (Marshal.SizeOf(typeof(IntPtr)) == 8)  
                baseKey = @"SOFTWARE\Wow6432Node\OpenOffice.org\";      
            else  
                baseKey = @"SOFTWARE\OpenOffice.org\";    
            // Get the URE directory  
            string key = baseKey + @"Layers_\URE\1";  
            RegistryKey reg = Registry.CurrentUser.OpenSubKey(key);  
            if (reg==null) reg = Registry.LocalMachine.OpenSubKey(key);
            string urePath = reg.GetValue("UREINSTALLLOCATION") as string;  
            reg.Close();  
            urePath = Path.Combine(urePath, "bin");  

            // Get the UNO Path  
            key = baseKey + @"UNO\InstallPath";  
            reg = Registry.CurrentUser.OpenSubKey(key);  
            if (reg==null) reg = Registry.LocalMachine.OpenSubKey(key);  
            string unoPath = reg.GetValue(null) as string;  
            reg.Close();  

            string path;  
            path = string.Format ("{0};{1}", System.Environment.GetEnvironmentVariable("PATH"), urePath);  
            System.Environment.SetEnvironmentVariable("PATH", path);  
            System.Environment.SetEnvironmentVariable("UNO_PATH", unoPath); 
        }

        public OOApp(string firstPageName, int firstPageStartRow)
        {
            //Comment next line if working with OO 2.x
            ConfigureOO3x();
            _doc = RunOO2x();
            Visible = false;
            XSpreadsheets sheets = _doc.getSheets();
            XIndexAccess xia = sheets as XIndexAccess;
            while (SheetsCount > 1)
            {
                XSpreadsheet sh = xia.getByIndex(1).Value as XSpreadsheet;
                XNamed named = sh as XNamed;
                sheets.removeByName(named.getName());
            }
            if (SheetsCount != 0)
            {
                XSpreadsheet sheet = xia.getByIndex(0).Value as XSpreadsheet;
                if (firstPageName != null)
                {
                    XNamed named = sheet as XNamed;
                    named.setName(firstPageName);
                }
                OOSheet page = new OOSheet(sheet, firstPageStartRow, this);
                _pages.Add(page);
            }
            else
            {
                CreateNextPage(firstPageName ?? "", firstPageStartRow);
            }

        }

        public OOApp(XSpreadsheetDocument doc)
        {
            this._doc = doc;
            XSpreadsheets sheets = doc.getSheets();
            XIndexAccess xia = sheets as XIndexAccess;
            for (int i = 0; i < SheetsCount; i++)
            {
                OOSheet page = new OOSheet(xia.getByIndex(i).Value as XSpreadsheet, 1, this);
                _pages.Add(page);
            }

        }
        public override TableSheet FirstPage
        {
            get
            {
                return FirstOOPage;
            }
        }
        public OOSheet FirstOOPage
        {
            get
            {
                return _pages[0];
            }
        }
        public override TableSheet LastPage
        {
            get
            {
                return LastOOPage;
            }
        }
        public OOSheet LastOOPage
        {
            get
            {
                return _pages[_pages.Count - 1];
            }
        }
        public override TableSheet CreateNextPage(string name, int startRow)
        {
            return CreateNextOOPage(name, startRow);
        }
        public int SheetsCount
        {
            get
            {
                XSpreadsheets sheets = _doc.getSheets();
                return sheets.getElementNames().Length;
            }
        }
        public OOSheet CreateNextOOPage(string name, int startRow)
        {
            XSpreadsheets sheets = _doc.getSheets();
            sheets.insertNewByName(name, (short)SheetsCount);
            OOSheet ret = new OOSheet((sheets.getByName(name).Value as XSpreadsheet), startRow, this);
            _pages.Add(ret);
            return ret;
        }
        public override TableSheet this[int idx]
        {
            get
            {
                return _pages[idx];
            }
        }
        public override bool Visible
        {
            set
            {
                XModel model = _doc as XModel;
                if (model != null)
                {
                    model.getCurrentController().getFrame().getContainerWindow().setVisible(value);
                }
            }
        }

        public int GetNumberFormatCode(string fmt)
        {
            if (_numberFormats.ContainsKey(fmt)) return _numberFormats[fmt];

            XNumberFormatsSupplier xFormatsSupplier = (XNumberFormatsSupplier)_doc;
            XNumberFormats xFormats = xFormatsSupplier.getNumberFormats();

            unoidl.com.sun.star.lang.Locale loc = new unoidl.com.sun.star.lang.Locale("ru", "RU", "WIN");
            int nFmt = xFormats.addNew(fmt, new unoidl.com.sun.star.lang.Locale());
            _numberFormats.Add(fmt, nFmt);
            return nFmt;

        }
    }
}
