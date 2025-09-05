using System;
using System.Collections.Generic;
using System.Text;
using unoidl.com.sun.star.beans;

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

//Helper class to view XPropertySet contents while debugging :-)

namespace SA.TblProc
{
    //OO enum props
    internal class OOEP
    {
#if DEBUG
        public static object Obj
        {
            set
            {
                XPropertySet xps = value as XPropertySet;
                if (xps == null) return;
                XPropertySetInfo xpsi = xps.getPropertySetInfo();
                if (xpsi == null) return;
                Property[] props = xpsi.getProperties();
                for (int i = 0; i < props.Length; i++)
                {
                    string name = "<error getting value>";
                    object val = null;
                    try
                    {
                        name = props[i].Name;
                        val = xps.getPropertyValue(name).Value;
                    }
                    catch (Exception ex) {}
                }//place break here to view names and values
            }

        }
#else
        public static object Obj {set{}}
#endif
    }
}
