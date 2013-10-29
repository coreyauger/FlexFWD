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

namespace Affine.Dnn.Modules.ATI_CompAdmin
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
    public class ATI_CompAdminController : ISearchable, IPortable
    {

    #region Constructors

        public ATI_CompAdminController()
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
        /// <param name="objATI_CompAdmin">The ATI_CompAdminInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void AddATI_CompAdmin(ATI_CompAdminInfo objATI_CompAdmin)
        {
            if (objATI_CompAdmin.Content.Trim() != "")
            {
                DataProvider.Instance().AddATI_CompAdmin(objATI_CompAdmin.ModuleId, objATI_CompAdmin.Content, objATI_CompAdmin.CreatedByUser);
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
        public void DeleteATI_CompAdmin(int ModuleId, int ItemId) 
        {
            DataProvider.Instance().DeleteATI_CompAdmin(ModuleId,ItemId);
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
        public ATI_CompAdminInfo GetATI_CompAdmin(int ModuleId, int ItemId)
        {
            return CBO.FillObject < ATI_CompAdminInfo >(DataProvider.Instance().GetATI_CompAdmin(ModuleId, ItemId));
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
        public List<ATI_CompAdminInfo> GetATI_CompAdmins(int ModuleId)
        {
            return CBO.FillCollection< ATI_CompAdminInfo >(DataProvider.Instance().GetATI_CompAdmins(ModuleId));
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// saves an object to the database
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="objATI_CompAdmin">The ATI_CompAdminInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void UpdateATI_CompAdmin(ATI_CompAdminInfo objATI_CompAdmin)
        {
            if (objATI_CompAdmin.Content.Trim() != "")
            {
                DataProvider.Instance().UpdateATI_CompAdmin(objATI_CompAdmin.ModuleId, objATI_CompAdmin.ItemId, objATI_CompAdmin.Content, objATI_CompAdmin.CreatedByUser);
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
            List<ATI_CompAdminInfo> colATI_CompAdmins  = GetATI_CompAdmins(ModInfo.ModuleID);

            foreach (ATI_CompAdminInfo objATI_CompAdmin in colATI_CompAdmins)
            {
                if(objATI_CompAdmin != null)
                {
                    SearchItemInfo SearchItem = new SearchItemInfo(ModInfo.ModuleTitle, objATI_CompAdmin.Content, objATI_CompAdmin.CreatedByUser, objATI_CompAdmin.CreatedDate, ModInfo.ModuleID, objATI_CompAdmin.ItemId.ToString(), objATI_CompAdmin.Content, "ItemId=" + objATI_CompAdmin.ItemId.ToString());
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
            List<ATI_CompAdminInfo> colATI_CompAdmins  = GetATI_CompAdmins(ModuleID);

            if (colATI_CompAdmins.Count != 0)
            {
                strXML += "<ATI_CompAdmins>";
                foreach (ATI_CompAdminInfo objATI_CompAdmin in colATI_CompAdmins)
                {
                    strXML += "<ATI_CompAdmin>";
                    strXML += "<content>" + XmlUtils.XMLEncode(objATI_CompAdmin.Content) + "</content>";
                    strXML += "</ATI_CompAdmin>";
                }
                strXML += "</ATI_CompAdmins>";
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
            XmlNode xmlATI_CompAdmins = Globals.GetContent(Content, "ATI_CompAdmins");

            foreach (XmlNode xmlATI_CompAdmin in xmlATI_CompAdmins.SelectNodes("ATI_CompAdmin"))
            {
                ATI_CompAdminInfo objATI_CompAdmin = new ATI_CompAdminInfo();

                objATI_CompAdmin.ModuleId = ModuleID;
                objATI_CompAdmin.Content = xmlATI_CompAdmin.SelectSingleNode("content").InnerText;
                objATI_CompAdmin.CreatedByUser = UserId;
                AddATI_CompAdmin(objATI_CompAdmin);
            }

}

    #endregion

    }
}

