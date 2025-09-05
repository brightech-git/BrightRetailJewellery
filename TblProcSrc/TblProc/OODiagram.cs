using System;
using System.Collections.Generic;
using System.Text;
using unoidl.com.sun.star.chart;
using unoidl.com.sun.star.beans;
using unoidl.com.sun.star.drawing;

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

//An OpenOffice.org chart wrapper class.

namespace SA.TblProc.OO
{
    public class OODiagram: TableDiagram
    {
        private XChartDocument _chart;
        private OOSheet _sheet;


        public XChartDocument XChartDocument
        {
            get { return _chart; }
        }

        public OODiagram(XChartDocument doc, OO.OOSheet sheet)
        {
            this._chart = doc;
            this._sheet = sheet;
        }

        private XAxisXSupplier XAxisSupplier
        {
            get
            {
                XAxisXSupplier aXAxisSupplier = _chart.getDiagram() as XAxisXSupplier;
                if (aXAxisSupplier == null)
                    throw new AxisNotFoundException("X axis not found");
                return aXAxisSupplier;
            }
        }

        private XAxisYSupplier YAxisSupplier
        {
            get
            {
                XAxisYSupplier aYAxisSupplier = _chart.getDiagram() as XAxisYSupplier;
                if (aYAxisSupplier == null)
                    throw new AxisNotFoundException("Y axis not found");
                return aYAxisSupplier;
            }
        }

        public override string XAxisName 
        {
            get
            {
                XAxisXSupplier aXAxisSupplier = XAxisSupplier;
                XPropertySet axisProp = aXAxisSupplier as XPropertySet;
                object hasName = axisProp.getPropertyValue("HasXAxisTitle").Value;
                if (hasName == null) return null;
                try
                {
                    bool bHas = Convert.ToBoolean(hasName);
                    if (!bHas) return null;
                }
                catch (Exception)
                {
                    return null;
                }
                XShape shape = aXAxisSupplier.getXAxisTitle();
                XPropertySet shapeProp = shape as XPropertySet;
                return shapeProp.getPropertyValue("String").Value.ToString();
            }
            set
            {
                XAxisXSupplier aXAxisSupplier = XAxisSupplier;
                XPropertySet axisProp = aXAxisSupplier as XPropertySet;
                if (value == null)
                {
                    axisProp.setPropertyValue("HasXAxisTitle", new uno.Any(false));
                    return;
                }
                axisProp.setPropertyValue("HasXAxisTitle", new uno.Any(true));
                XShape shape = aXAxisSupplier.getXAxisTitle();
                XPropertySet shapeProp = shape as XPropertySet;
                shapeProp.setPropertyValue("String", new uno.Any(value));
            } 
        }

        public override string YAxisName 
        { 
            get 
            {
                XAxisYSupplier aYAxisSupplier = YAxisSupplier;
                XPropertySet axisProp = aYAxisSupplier as XPropertySet;
                object hasName = axisProp.getPropertyValue("HasYAxisTitle").Value;
                if (hasName == null) return null;
                try
                {
                    bool bHas = Convert.ToBoolean(hasName);
                    if (!bHas) return null;
                }
                catch (Exception)
                {
                    return null;
                }
                XShape shape=aYAxisSupplier.getYAxisTitle();
                XPropertySet shapeProp = shape as XPropertySet;
                return shapeProp.getPropertyValue("String").Value.ToString();
            } 
            set 
            {
                XAxisYSupplier aYAxisSupplier = YAxisSupplier;
                XPropertySet axisProp = aYAxisSupplier as XPropertySet;
                if (value == null)
                {
                    axisProp.setPropertyValue("HasYAxisTitle", new uno.Any(false));
                    return;
                }
                axisProp.setPropertyValue("HasYAxisTitle", new uno.Any(true));
                XShape shape = aYAxisSupplier.getYAxisTitle();
                XPropertySet shapeProp = shape as XPropertySet;
                shapeProp.setPropertyValue("String", new uno.Any(value));
            } 
        }

        

        public override string Name 
        { 
            get 
            {
                XPropertySet xProp = _chart as XPropertySet;
                if (!Convert.ToBoolean(xProp.getPropertyValue("HasMainTitle").Value)) return null;
                xProp = (XPropertySet)_chart.getTitle() as XPropertySet;
                return xProp.getPropertyValue("String").Value.ToString();
            } 
            set 
            {
                if (value == null)
                {
                    XPropertySet xProp = _chart as XPropertySet;
                    xProp.setPropertyValue("HasMainTitle", new uno.Any(false));
                    return;
                }
                XPropertySet xPropertySet = _chart as XPropertySet;
                xPropertySet.setPropertyValue("HasMainTitle", new uno.Any(true));
                xPropertySet = _chart.getTitle() as XPropertySet;
                xPropertySet.setPropertyValue("String", new uno.Any(value));
            } 
        }

        public int FirstDataIndex
        {
            get
            {
                if (_chart.getDiagram().getDiagramType() == "com.sun.star.chart.XYDiagram") return 1;
                return 0;
            }
        }

        public override TableRange XAxisNamesRange
        {
            set
            {
                XChartDataArray xcda = _chart.getData() as XChartDataArray;
                if (xcda == null) return;
                int dataLen = xcda.getRowDescriptions().Length;
                if (dataLen == 0) return;

                OOSheet rangeSheet = _sheet;
                if (value is OORange)
                    rangeSheet = (value as OORange).Page;
                string[] rangeNames = GetOneDimensionRangeValues(value, dataLen, rangeSheet);
                if (rangeNames == null) return;
                if (rangeNames.Length < dataLen)
                {
                    string[] newNames = new string[dataLen];
                    for (int i = 0; i < rangeNames.Length; i++)
                        newNames[i] = rangeNames[i];
                    for (int i = rangeNames.Length; i < dataLen; i++)
                        newNames[i] = "";
                    rangeNames = newNames;
                }

                xcda.setRowDescriptions(rangeNames);
            }

        }
        public override void SetMainAxisNamesRange(TableRange range, TableSheet rangeSheet)
        {
            XChartDataArray xcda = _chart.getData() as XChartDataArray;
            if (xcda == null) return;
            int dataLen = xcda.getColumnDescriptions().Length;
            if (dataLen == 0) return;

            string[] rangeNames = GetOneDimensionRangeValues(range, dataLen, rangeSheet);
            if (rangeNames == null) return;
            MainAxisNames = rangeNames;
        }

        public override string[] MainAxisNames
        {
            set
            {
                if (value == null) return;
                XChartDataArray xcda = _chart.getData() as XChartDataArray;
                if (xcda == null) return;
                int dataLen = xcda.getColumnDescriptions().Length;
                if (dataLen == 0) return;


                string[] names = value;
                if (names.Length < dataLen)
                {
                    string[] newNames = new string[dataLen];
                    for (int i = 0; i < names.Length; i++)
                        newNames[i] = names[i];
                    for (int i = names.Length; i < dataLen; i++)
                        newNames[i] = "";
                    names = newNames;
                }
                xcda.setColumnDescriptions(names);
            }
        }

        private XShape XShape
        {
            get
            {
                unoidl.com.sun.star.sheet.XSpreadsheetDocument doc = _sheet.Application.XSpreadsheetDocument;

                XDrawPagesSupplier supp = doc as XDrawPagesSupplier;
                XDrawPages pages = supp.getDrawPages();
                if (pages.getCount() <= _sheet.PageNumber)
                    throw new SheetNotFoundException("Can't get drawing sheet reference");
                XDrawPage pg = pages.getByIndex(_sheet.PageNumber).Value as XDrawPage;
                //I can't get how to discover if this shape is of our chart or no
                if (pg.getCount() != 1)
                    throw new IAmSoSorryException("Can't get chart's shape if there's more than one graphics on a sheet for OO");
                return pg.getByIndex(0).Value as XShape;
            }
        }

        public override DrawRect DrawRect
        {
            get 
            {
                XShape shape = XShape;
                unoidl.com.sun.star.awt.Point pt = shape.getPosition();
                unoidl.com.sun.star.awt.Size sz = shape.getSize();
                return new DrawRect(pt.X,pt.Y,pt.X+sz.Width,pt.Y+sz.Height); 
            }
            set 
            {
                XShape shape = XShape;
                shape.setPosition(new unoidl.com.sun.star.awt.Point((int)value.Left, (int)value.Top));
                shape.setSize(new unoidl.com.sun.star.awt.Size((int)value.Width, (int)value.Height));

            }
        }

        public DiagramType Type
        {
            get 
            {
                string type = _chart.getDiagram().getDiagramType();
                int idx = type.LastIndexOf('.');
                type = type.Substring(idx + 1);
                if (type.EndsWith("Diagram")) type = type.Substring(0, type.Length - "Diagram".Length);
                if(!Enum.IsDefined(typeof(DiagramType),type))
                    throw new InvalidDiagramTypeException("Unknown diagram type "+type);
                return (DiagramType)Enum.Parse(typeof(DiagramType),type,true);
            }
        }
    }
}
