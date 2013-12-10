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

namespace Affine.Dnn.Modules.ATI_Requests
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
    public class ATI_RequestsController : ISearchable, IPortable
    {

    #region Constructors

        public ATI_RequestsController()
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
        /// <param name="objATI_Requests">The ATI_RequestsInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void AddATI_Requests(ATI_RequestsInfo objATI_Requests)
        {
            if (objATI_Requests.Content.Trim() != "")
            {
                DataProvider.Instance().AddATI_Requests(objATI_Requests.ModuleId, objATI_Requests.Content, objATI_Requests.CreatedByUser);
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
        public void DeleteATI_Requests(int ModuleId, int ItemId) 
        {
            DataProvider.Instance().DeleteATI_Requests(ModuleId,ItemId);
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
        public ATI_RequestsInfo GetATI_Requests(int ModuleId, int ItemId)
        {
            return CBO.FillObject < ATI_RequestsInfo >(DataProvider.Instance().GetATI_Requests(ModuleId, ItemId));
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
        public List<ATI_RequestsInfo> GetATI_Requestss(int ModuleId)
        {
            return CBO.FillCollection< ATI_RequestsInfo >(DataProvider.Instance().GetATI_Requestss(ModuleId));
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// saves an object to the database
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="objATI_Requests">The ATI_RequestsInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void UpdateATI_Requests(ATI_RequestsInfo objATI_Requests)
        {
            if (objATI_Requests.Content.Trim() != "")
            {
                DataProvider.Instance().UpdateATI_Requests(objATI_Requests.ModuleId, objATI_Requests.ItemId, objATI_Requests.Content, objATI_Requests.CreatedByUser);
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
            List<ATI_RequestsInfo> colATI_Requestss  = GetATI_Requestss(ModInfo.ModuleID);

            foreach (ATI_RequestsInfo objATI_Requests in colATI_Requestss)
            {
                if(objATI_Requests != null)
                {
                    SearchItemInfo SearchItem = new SearchItemInfo(ModInfo.ModuleTitle, objATI_Requests.Content, objATI_Requests.CreatedByUser, objATI_Requests.CreatedDate, ModInfo.ModuleID, objATI_Requests.ItemId.ToString(), objATI_Requests.Content, "ItemId=" + objATI_Requests.ItemId.ToString());
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
            List<ATI_RequestsInfo> colATI_Requestss  = GetATI_Requestss(ModuleID);

            if (colATI_Requestss.Count != 0)
            {
                strXML += "<ATI_Requestss>";
                foreach (ATI_RequestsInfo objATI_Requests in colATI_Requestss)
                {
                    strXML += "<ATI_Requests>";
                    strXML += "<content>" + XmlUtils.XMLEncode(objATI_Requests.Content) + "</content>";
                    strXML += "</ATI_Requests>";
                }
                strXML += "</ATI_Requestss>";
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
            XmlNode xmlATI_Requestss = Globals.GetContent(Content, "ATI_Requestss");

            foreach (XmlNode xmlATI_Requests in xmlATI_Requestss.SelectNodes("ATI_Requests"))
            {
                ATI_RequestsInfo objATI_Requests = new ATI_RequestsInfo();

                objATI_Requests.ModuleId = ModuleID;
                objATI_Requests.Content = xmlATI_Requests.SelectSingleNode("content").InnerText;
                objATI_Requests.CreatedByUser = UserId;
                AddATI_Requests(objATI_Requests);
            }

}

    #endregion

    }
}

