/*
' DotNetNuke® - http://www.dotnetnuke.com
' Copyright (c) 2002-2006
' by Perpetual Motion Interactive Systems Inc. ( http://www.perpetualmotion.ca )
'
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
' documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
' the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
' to permit persons to whom the Software is furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in all copies or substantial portions 
' of the Software.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
' DEALINGS IN THE SOFTWARE.
 */

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Xml;
using System.Web;
using DotNetNuke;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Search;

namespace Affine.Dnn.Modules.ATI_Comps
{
    /// -----------------------------------------------------------------------------
    ///<summary>
    /// The Controller class for the ATI_Register
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <history>
    /// </history>
    /// -----------------------------------------------------------------------------
    public class ATI_CompsController : ISearchable, IPortable
    {

    #region Constructors

        public ATI_CompsController()
        {
        }

    #endregion

    #region Public Methods

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// adds an object to the database
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="objATI_Comps">The ATI_CompsInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void AddATI_Comps(ATI_CompsInfo objATI_Comps)
        {
            if (objATI_Comps.Content.Trim() != "")
            {
                DataProvider.Instance().AddATI_Comps(objATI_Comps.ModuleId, objATI_Comps.Content, objATI_Comps.CreatedByUser);
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// deletes an object from the database
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="ModuleId">The Id of the module</param>
        /// <param name="ItemId">The Id of the item</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void DeleteATI_Comps(int ModuleId, int ItemId) 
        {
            DataProvider.Instance().DeleteATI_Comps(ModuleId,ItemId);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// gets an object from the database
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="moduleId">The Id of the module</param>
        /// <param name="ItemId">The Id of the item</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public ATI_CompsInfo GetATI_Comps(int ModuleId, int ItemId)
        {
            return CBO.FillObject < ATI_CompsInfo >(DataProvider.Instance().GetATI_Comps(ModuleId, ItemId));
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// gets an object from the database
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="moduleId">The Id of the module</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public List<ATI_CompsInfo> GetATI_Compss(int ModuleId)
        {
            return CBO.FillCollection< ATI_CompsInfo >(DataProvider.Instance().GetATI_Compss(ModuleId));
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// saves an object to the database
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="objATI_Comps">The ATI_CompsInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void UpdateATI_Comps(ATI_CompsInfo objATI_Comps)
        {
            if (objATI_Comps.Content.Trim() != "")
            {
                DataProvider.Instance().UpdateATI_Comps(objATI_Comps.ModuleId, objATI_Comps.ItemId, objATI_Comps.Content, objATI_Comps.CreatedByUser);
            }
        }

    #endregion

    #region Optional Interfaces

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// GetSearchItems implements the ISearchable Interface
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="ModInfo">The ModuleInfo for the module to be Indexed</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public SearchItemInfoCollection GetSearchItems(ModuleInfo ModInfo)
        {
            SearchItemInfoCollection SearchItemCollection = new SearchItemInfoCollection();
            List<ATI_CompsInfo> colATI_Compss  = GetATI_Compss(ModInfo.ModuleID);

            foreach (ATI_CompsInfo objATI_Comps in colATI_Compss)
            {
                if(objATI_Comps != null)
                {
                    SearchItemInfo SearchItem = new SearchItemInfo(ModInfo.ModuleTitle, objATI_Comps.Content, objATI_Comps.CreatedByUser, objATI_Comps.CreatedDate, ModInfo.ModuleID, objATI_Comps.ItemId.ToString(), objATI_Comps.Content, "ItemId=" + objATI_Comps.ItemId.ToString());
                    SearchItemCollection.Add(SearchItem);
                }
            }

            return SearchItemCollection;
        }


        /// -----------------------------------------------------------------------------
        /// <summary>
        /// ExportModule implements the IPortable ExportModule Interface
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="ModuleID">The Id of the module to be exported</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public string ExportModule(int ModuleID)
        {
            string strXML = "";
            List<ATI_CompsInfo> colATI_Compss  = GetATI_Compss(ModuleID);

            if (colATI_Compss.Count != 0)
            {
                strXML += "<ATI_Compss>";
                foreach (ATI_CompsInfo objATI_Comps in colATI_Compss)
                {
                    strXML += "<ATI_Comps>";
                    strXML += "<content>" + XmlUtils.XMLEncode(objATI_Comps.Content) + "</content>";
                    strXML += "</ATI_Comps>";
                }
                strXML += "</ATI_Compss>";
            }

            return strXML;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// ImportModule implements the IPortable ImportModule Interface
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="ModuleID">The Id of the module to be imported</param>
        /// <param name="Content">The content to be imported</param>
        /// <param name="Version">The version of the module to be imported</param>
        /// <param name="UserId">The Id of the user performing the import</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void ImportModule(int ModuleID, string Content, string Version, int UserId)
        {
            XmlNode xmlATI_Compss = Globals.GetContent(Content, "ATI_Compss");

            foreach (XmlNode xmlATI_Comps in xmlATI_Compss.SelectNodes("ATI_Comps"))
            {
                ATI_CompsInfo objATI_Comps = new ATI_CompsInfo();

                objATI_Comps.ModuleId = ModuleID;
                objATI_Comps.Content = xmlATI_Comps.SelectSingleNode("content").InnerText;
                objATI_Comps.CreatedByUser = UserId;
                AddATI_Comps(objATI_Comps);
            }

}

    #endregion

    }
}

