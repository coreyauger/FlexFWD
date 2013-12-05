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

namespace Affine.Dnn.Modules.ATI_Redirect
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
    public class ATI_RedirectController : ISearchable, IPortable
    {

    #region Constructors

        public ATI_RedirectController()
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
        /// <param name="objATI_Redirect">The ATI_RedirectInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void AddATI_Redirect(ATI_RedirectInfo objATI_Redirect)
        {
            if (objATI_Redirect.Content.Trim() != "")
            {
                DataProvider.Instance().AddATI_Redirect(objATI_Redirect.ModuleId, objATI_Redirect.Content, objATI_Redirect.CreatedByUser);
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
        public void DeleteATI_Redirect(int ModuleId, int ItemId) 
        {
            DataProvider.Instance().DeleteATI_Redirect(ModuleId,ItemId);
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
        public ATI_RedirectInfo GetATI_Redirect(int ModuleId, int ItemId)
        {
            return CBO.FillObject < ATI_RedirectInfo >(DataProvider.Instance().GetATI_Redirect(ModuleId, ItemId));
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
        public List<ATI_RedirectInfo> GetATI_Redirects(int ModuleId)
        {
            return CBO.FillCollection< ATI_RedirectInfo >(DataProvider.Instance().GetATI_Redirects(ModuleId));
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// saves an object to the database
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="objATI_Redirect">The ATI_RedirectInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void UpdateATI_Redirect(ATI_RedirectInfo objATI_Redirect)
        {
            if (objATI_Redirect.Content.Trim() != "")
            {
                DataProvider.Instance().UpdateATI_Redirect(objATI_Redirect.ModuleId, objATI_Redirect.ItemId, objATI_Redirect.Content, objATI_Redirect.CreatedByUser);
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
            List<ATI_RedirectInfo> colATI_Redirects  = GetATI_Redirects(ModInfo.ModuleID);

            foreach (ATI_RedirectInfo objATI_Redirect in colATI_Redirects)
            {
                if(objATI_Redirect != null)
                {
                    SearchItemInfo SearchItem = new SearchItemInfo(ModInfo.ModuleTitle, objATI_Redirect.Content, objATI_Redirect.CreatedByUser, objATI_Redirect.CreatedDate, ModInfo.ModuleID, objATI_Redirect.ItemId.ToString(), objATI_Redirect.Content, "ItemId=" + objATI_Redirect.ItemId.ToString());
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
            List<ATI_RedirectInfo> colATI_Redirects  = GetATI_Redirects(ModuleID);

            if (colATI_Redirects.Count != 0)
            {
                strXML += "<ATI_Redirects>";
                foreach (ATI_RedirectInfo objATI_Redirect in colATI_Redirects)
                {
                    strXML += "<ATI_Redirect>";
                    strXML += "<content>" + XmlUtils.XMLEncode(objATI_Redirect.Content) + "</content>";
                    strXML += "</ATI_Redirect>";
                }
                strXML += "</ATI_Redirects>";
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
            XmlNode xmlATI_Redirects = Globals.GetContent(Content, "ATI_Redirects");

            foreach (XmlNode xmlATI_Redirect in xmlATI_Redirects.SelectNodes("ATI_Redirect"))
            {
                ATI_RedirectInfo objATI_Redirect = new ATI_RedirectInfo();

                objATI_Redirect.ModuleId = ModuleID;
                objATI_Redirect.Content = xmlATI_Redirect.SelectSingleNode("content").InnerText;
                objATI_Redirect.CreatedByUser = UserId;
                AddATI_Redirect(objATI_Redirect);
            }

}

    #endregion

    }
}

