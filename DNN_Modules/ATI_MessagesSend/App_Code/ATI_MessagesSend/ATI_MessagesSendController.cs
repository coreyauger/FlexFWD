/*
' DotNetNukeŽ - http://www.dotnetnuke.com
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

namespace Affine.Dnn.Modules.ATI_MessagesSend
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
    public class ATI_MessagesSendController : ISearchable, IPortable
    {

    #region Constructors

        public ATI_MessagesSendController()
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
        /// <param name="objATI_MessagesSend">The ATI_MessagesSendInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void AddATI_MessagesSend(ATI_MessagesSendInfo objATI_MessagesSend)
        {
            if (objATI_MessagesSend.Content.Trim() != "")
            {
                DataProvider.Instance().AddATI_MessagesSend(objATI_MessagesSend.ModuleId, objATI_MessagesSend.Content, objATI_MessagesSend.CreatedByUser);
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
        public void DeleteATI_MessagesSend(int ModuleId, int ItemId) 
        {
            DataProvider.Instance().DeleteATI_MessagesSend(ModuleId,ItemId);
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
        public ATI_MessagesSendInfo GetATI_MessagesSend(int ModuleId, int ItemId)
        {
            return CBO.FillObject < ATI_MessagesSendInfo >(DataProvider.Instance().GetATI_MessagesSend(ModuleId, ItemId));
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
        public List<ATI_MessagesSendInfo> GetATI_MessagesSends(int ModuleId)
        {
            return CBO.FillCollection< ATI_MessagesSendInfo >(DataProvider.Instance().GetATI_MessagesSends(ModuleId));
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// saves an object to the database
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="objATI_MessagesSend">The ATI_MessagesSendInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void UpdateATI_MessagesSend(ATI_MessagesSendInfo objATI_MessagesSend)
        {
            if (objATI_MessagesSend.Content.Trim() != "")
            {
                DataProvider.Instance().UpdateATI_MessagesSend(objATI_MessagesSend.ModuleId, objATI_MessagesSend.ItemId, objATI_MessagesSend.Content, objATI_MessagesSend.CreatedByUser);
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
            List<ATI_MessagesSendInfo> colATI_MessagesSends  = GetATI_MessagesSends(ModInfo.ModuleID);

            foreach (ATI_MessagesSendInfo objATI_MessagesSend in colATI_MessagesSends)
            {
                if(objATI_MessagesSend != null)
                {
                    SearchItemInfo SearchItem = new SearchItemInfo(ModInfo.ModuleTitle, objATI_MessagesSend.Content, objATI_MessagesSend.CreatedByUser, objATI_MessagesSend.CreatedDate, ModInfo.ModuleID, objATI_MessagesSend.ItemId.ToString(), objATI_MessagesSend.Content, "ItemId=" + objATI_MessagesSend.ItemId.ToString());
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
            List<ATI_MessagesSendInfo> colATI_MessagesSends  = GetATI_MessagesSends(ModuleID);

            if (colATI_MessagesSends.Count != 0)
            {
                strXML += "<ATI_MessagesSends>";
                foreach (ATI_MessagesSendInfo objATI_MessagesSend in colATI_MessagesSends)
                {
                    strXML += "<ATI_MessagesSend>";
                    strXML += "<content>" + XmlUtils.XMLEncode(objATI_MessagesSend.Content) + "</content>";
                    strXML += "</ATI_MessagesSend>";
                }
                strXML += "</ATI_MessagesSends>";
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
            XmlNode xmlATI_MessagesSends = Globals.GetContent(Content, "ATI_MessagesSends");

            foreach (XmlNode xmlATI_MessagesSend in xmlATI_MessagesSends.SelectNodes("ATI_MessagesSend"))
            {
                ATI_MessagesSendInfo objATI_MessagesSend = new ATI_MessagesSendInfo();

                objATI_MessagesSend.ModuleId = ModuleID;
                objATI_MessagesSend.Content = xmlATI_MessagesSend.SelectSingleNode("content").InnerText;
                objATI_MessagesSend.CreatedByUser = UserId;
                AddATI_MessagesSend(objATI_MessagesSend);
            }

}

    #endregion

    }
}

