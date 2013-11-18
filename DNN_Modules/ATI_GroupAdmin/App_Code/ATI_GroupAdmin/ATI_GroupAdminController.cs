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

namespace Affine.Dnn.Modules.ATI_GroupAdmin
{
    /// -----------------------------------------------------------------------------
    ///<summary>
    /// The Controller class for the ATI_GroupAdmin
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <history>
    /// </history>
    /// -----------------------------------------------------------------------------
    public class ATI_GroupAdminController : ISearchable, IPortable
    {

    #region Constructors

        public ATI_GroupAdminController()
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
        /// <param name="objATI_GroupAdmin">The ATI_GroupAdminInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void AddATI_GroupAdmin(ATI_GroupAdminInfo objATI_GroupAdmin)
        {
            if (objATI_GroupAdmin.Content.Trim() != "")
            {
                DataProvider.Instance().AddATI_GroupAdmin(objATI_GroupAdmin.ModuleId, objATI_GroupAdmin.Content, objATI_GroupAdmin.CreatedByUser);
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
        public void DeleteATI_GroupAdmin(int ModuleId, int ItemId) 
        {
            DataProvider.Instance().DeleteATI_GroupAdmin(ModuleId,ItemId);
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
        public ATI_GroupAdminInfo GetATI_GroupAdmin(int ModuleId, int ItemId)
        {
            return CBO.FillObject < ATI_GroupAdminInfo >(DataProvider.Instance().GetATI_GroupAdmin(ModuleId, ItemId));
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
        public List<ATI_GroupAdminInfo> GetATI_GroupAdmins(int ModuleId)
        {
            return CBO.FillCollection< ATI_GroupAdminInfo >(DataProvider.Instance().GetATI_GroupAdmins(ModuleId));
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// saves an object to the database
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="objATI_GroupAdmin">The ATI_GroupAdminInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void UpdateATI_GroupAdmin(ATI_GroupAdminInfo objATI_GroupAdmin)
        {
            if (objATI_GroupAdmin.Content.Trim() != "")
            {
                DataProvider.Instance().UpdateATI_GroupAdmin(objATI_GroupAdmin.ModuleId, objATI_GroupAdmin.ItemId, objATI_GroupAdmin.Content, objATI_GroupAdmin.CreatedByUser);
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
            List<ATI_GroupAdminInfo> colATI_GroupAdmins  = GetATI_GroupAdmins(ModInfo.ModuleID);

            foreach (ATI_GroupAdminInfo objATI_GroupAdmin in colATI_GroupAdmins)
            {
                if(objATI_GroupAdmin != null)
                {
                    SearchItemInfo SearchItem = new SearchItemInfo(ModInfo.ModuleTitle, objATI_GroupAdmin.Content, objATI_GroupAdmin.CreatedByUser, objATI_GroupAdmin.CreatedDate, ModInfo.ModuleID, objATI_GroupAdmin.ItemId.ToString(), objATI_GroupAdmin.Content, "ItemId=" + objATI_GroupAdmin.ItemId.ToString());
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
            List<ATI_GroupAdminInfo> colATI_GroupAdmins  = GetATI_GroupAdmins(ModuleID);

            if (colATI_GroupAdmins.Count != 0)
            {
                strXML += "<ATI_GroupAdmins>";
                foreach (ATI_GroupAdminInfo objATI_GroupAdmin in colATI_GroupAdmins)
                {
                    strXML += "<ATI_GroupAdmin>";
                    strXML += "<content>" + XmlUtils.XMLEncode(objATI_GroupAdmin.Content) + "</content>";
                    strXML += "</ATI_GroupAdmin>";
                }
                strXML += "</ATI_GroupAdmins>";
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
            XmlNode xmlATI_GroupAdmins = Globals.GetContent(Content, "ATI_GroupAdmins");

            foreach (XmlNode xmlATI_GroupAdmin in xmlATI_GroupAdmins.SelectNodes("ATI_GroupAdmin"))
            {
                ATI_GroupAdminInfo objATI_GroupAdmin = new ATI_GroupAdminInfo();

                objATI_GroupAdmin.ModuleId = ModuleID;
                objATI_GroupAdmin.Content = xmlATI_GroupAdmin.SelectSingleNode("content").InnerText;
                objATI_GroupAdmin.CreatedByUser = UserId;
                AddATI_GroupAdmin(objATI_GroupAdmin);
            }

}

    #endregion

    }
}

