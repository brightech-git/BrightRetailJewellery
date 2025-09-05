using System;
using System.Collections.Generic;
using System.Text;
using unoidl.com.sun.star.sheet;
using unoidl.com.sun.star.container;
using unoidl.com.sun.star.table;
using unoidl.com.sun.star.style;
using unoidl.com.sun.star.beans;
using unoidl.com.sun.star.chart;
using unoidl.com.sun.star.document;

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

//An OO sheet wrapper class. Used to manage document's pages

namespace SA.TblProc.OO
{
    public class OOSheet : TableSheet
    {
        private XSpreadsheet _sheet;
        private OOApp _app;

        public OOApp Application
        {
            get { return _app; }
        }
        public XSpreadsheet XSpreadsheet
        {
            get { return _sheet; }
        }

        public OOSheet(XSpreadsheet sheet, OOApp app)
            : this(sheet, 1, app)
        {
        }
        public OOSheet(XSpreadsheet sheet, int startRow, OOApp app)
            : base(startRow)
        {
            this._sheet = sheet;
            this._app = app;
        }
        public static OOSheet CreateOOSheet(string name)
        {
            return CreateOOSheet(name, 1);
        }
        public static OOSheet CreateOOSheet(string name, int startRow)
        {
            OOApp app = new OOApp(name, startRow);
            return app.FirstOOPage;
        }
        public override string Name
        {
            get
            {
                XNamed xn = _sheet as XNamed;
                if (xn == null) return null;
                return xn.getName();
            }
            set
            {
                XNamed xn = _sheet as XNamed;
                if (xn == null) return;
                xn.setName(value);
            }
        }
        public override TableRange Range(CoordRange range)
        {
            return OORange(range);
        }
        //using 1-based numeration
        public OORange OORange(CoordRange range)
        {
            XCellRange rng = _sheet.getCellRangeByPosition(range.StartCol - 1, range.StartRow - 1,
                range.EndCol - 1, range.EndRow - 1);
            return new OORange(range, rng, this);
        }

        public override TableRange BorderAround(CoordRange range, LineStyle lineStyle, BorderWeight borderWeight,
            object color)
        {
            OORange r = OORange(range);
            r.BorderAround(lineStyle, borderWeight, color);
            return r;
        }
        public override bool Visible
        {
            set { _app.Visible = value; }
        }

        public override PageOrientation Orientation
        {
            set
            {
                XStyleFamiliesSupplier xSupplier = (XStyleFamiliesSupplier)_app.XSpreadsheetDocument;
                XNameContainer xPageStyleCollection = (XNameContainer)xSupplier.getStyleFamilies().getByName("PageStyles").Value;
                XPropertySet xPropertySet = (XPropertySet)xPageStyleCollection.getByName("Default").Value;

                bool isLandscape = (bool)xPropertySet.getPropertyValue("IsLandscape").Value;
                if (isLandscape == (value == PageOrientation.Landscape)) return;
                //переворот
                unoidl.com.sun.star.awt.Size size = (unoidl.com.sun.star.awt.Size)xPropertySet.getPropertyValue("Size").Value;
                int height = size.Height;
                size.Height = size.Width;
                size.Width = height;
                xPropertySet.setPropertyValue("Size", new uno.Any(size.GetType(), size));


                xPropertySet.setPropertyValue("IsLandscape", new uno.Any(value == PageOrientation.Landscape));
            }
        }

        public override void PrintRowsOnEachPage(int startRow, int endRow)
        {
            XPrintAreas areas = (XPrintAreas)_sheet;
            areas.setPrintTitleRows(true);
            areas.setTitleRows(new CellRangeAddress(0, 0, startRow - 1, 0, endRow - 1));
        }

        public override void AutoFitColumns()
        {
            XSheetCellCursor cur = _sheet.createCursor();
            XUsedAreaCursor usedCur = (XUsedAreaCursor)cur;
            usedCur.gotoStartOfUsedArea(false);
            usedCur.gotoEndOfUsedArea(true);
            XSheetCellRange range = (XSheetCellRange)usedCur;
            XCellRangeAddressable addr = (XCellRangeAddressable)range;
            CellRangeAddress cra = addr.getRangeAddress();
            Range(cra.StartRow + 1, cra.StartColumn + 1, cra.EndRow + 1, cra.EndColumn + 1).AutoFitColumns();
        }

        //NOT IMPLEMENTED
        public override int FitToPagesWide
        {
            set
            {
                //sheet.PageSetup.FitToPagesWide = value;
            }
        }
        public override int FitToPagesTall
        {
            set
            {
                //sheet.PageSetup.FitToPagesTall = value;
            }
        }
        public override void AddPageNumbering()
        {
            //by default it has page numbering

            /*if (sheet.Application.LanguageSettings.get_LanguageID(Microsoft.Office.Core.MsoAppLanguageID.msoLanguageIDInstall) == 1049)
                sheet.PageSetup.CenterFooter = "Страница &С из &К";
            else
                sheet.PageSetup.CenterFooter = "Страница &P из &N";*/
        }

        public short PageNumber
        {
            get
            {
                XSpreadsheets sheets = _app.XSpreadsheetDocument.getSheets();
                XIndexAccess xia = sheets as XIndexAccess;
                if(xia==null) return 0;
                for (short i = 0; i < xia.getCount(); i++)
                {
                    XNamed sheetName=xia.getByIndex(i).Value as XNamed;
                    if (sheetName == null) continue;
                    if (sheetName.getName() == Name) return i;
                }
                return 0;
            }
        }

        public override TableDiagram AddDiagram(TableRange dataRange, RowsCols plotBy, DiagramType type, DrawRect rect)
        {
            XTableChartsSupplier tcs = _sheet as XTableChartsSupplier;
            XTableCharts charts = tcs.getCharts();

            if (charts == null)
                throw new DiagramException("Internal error (Unable to get sharts collection)");
            int postfix = 0;
            for(;;postfix++)
            {
                if (!charts.hasByName("chart" + postfix)) break;
            }
            unoidl.com.sun.star.awt.Rectangle rectangle = null;
            if (rect == null) rectangle = new unoidl.com.sun.star.awt.Rectangle(10000, 10000, 20000, 15000);
            else rectangle = new unoidl.com.sun.star.awt.Rectangle((int)rect.Left, (int)rect.Top, (int)rect.Width, (int)rect.Height);
            CellRangeAddress[] addr=new CellRangeAddress[1];
            short pgNum = PageNumber;
            if (dataRange is OORange)
                pgNum = (dataRange as OORange).Page.PageNumber;
            addr[0] = new CellRangeAddress(pgNum, dataRange.StartCol - 1, dataRange.StartRow - 1, dataRange.EndCol - 1, dataRange.EndRow - 1);
            charts.addNewByName("chart" + postfix, rectangle, addr, false, false);
            XTableChart chart = charts.getByName("chart" + postfix).Value as XTableChart;
            XEmbeddedObjectSupplier eos=chart as XEmbeddedObjectSupplier;
            if (eos == null)
                throw new DiagramException("Internal error (Unable to get shart object)");
            XChartDocument doc = eos.getEmbeddedObject() as XChartDocument;
            if (doc == null)
                throw new DiagramException("Internal error (Unable to get shart document object)");
            unoidl.com.sun.star.lang.XMultiServiceFactory xmsf = doc as unoidl.com.sun.star.lang.XMultiServiceFactory;
            XDiagram xd = xmsf.createInstance("com.sun.star.chart." + type.ToString() + "Diagram") as XDiagram;
            doc.setDiagram(xd);
            xd = doc.getDiagram();
            XPropertySet xdProps = xd as XPropertySet;
            xdProps.setPropertyValue("DataRowSource", new uno.Any(typeof(unoidl.com.sun.star.chart.ChartDataRowSource), ConstConvert.ToOO(plotBy)));
            return new OODiagram(doc,this);
        }
    }
}
