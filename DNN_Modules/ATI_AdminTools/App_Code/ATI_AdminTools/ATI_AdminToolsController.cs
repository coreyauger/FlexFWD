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

namespace Affine.Dnn.Modules.ATI_AdminTools
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
    public class ATI_AdminToolsController : ISearchable, IPortable
    {

    #region Constructors

        public ATI_AdminToolsController()
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
        /// <param name="objATI_AdminTools">The ATI_AdminToolsInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void AddATI_AdminTools(ATI_AdminToolsInfo objATI_AdminTools)
        {
            if (objATI_AdminTools.Content.Trim() != "")
            {
                DataProvider.Instance().AddATI_AdminTools(objATI_AdminTools.ModuleId, objATI_AdminTools.Content, objATI_AdminTools.CreatedByUser);
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
        public void DeleteATI_AdminTools(int ModuleId, int ItemId) 
        {
            DataProvider.Instance().DeleteATI_AdminTools(ModuleId,ItemId);
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
        public ATI_AdminToolsInfo GetATI_AdminTools(int ModuleId, int ItemId)
        {
            return CBO.FillObject < ATI_AdminToolsInfo >(DataProvider.Instance().GetATI_AdminTools(ModuleId, ItemId));
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
        public List<ATI_AdminToolsInfo> GetATI_AdminToolss(int ModuleId)
        {
            return CBO.FillCollection< ATI_AdminToolsInfo >(DataProvider.Instance().GetATI_AdminToolss(ModuleId));
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// saves an object to the database
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="objATI_AdminTools">The ATI_AdminToolsInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void UpdateATI_AdminTools(ATI_AdminToolsInfo objATI_AdminTools)
        {
            if (objATI_AdminTools.Content.Trim() != "")
            {
                DataProvider.Instance().UpdateATI_AdminTools(objATI_AdminTools.ModuleId, objATI_AdminTools.ItemId, objATI_AdminTools.Content, objATI_AdminTools.CreatedByUser);
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
            List<ATI_AdminToolsInfo> colATI_AdminToolss  = GetATI_AdminToolss(ModInfo.ModuleID);

            foreach (ATI_AdminToolsInfo objATI_AdminTools in colATI_AdminToolss)
            {
                if(objATI_AdminTools != null)
                {
                    SearchItemInfo SearchItem = new SearchItemInfo(ModInfo.ModuleTitle, objATI_AdminTools.Content, objATI_AdminTools.CreatedByUser, objATI_AdminTools.CreatedDate, ModInfo.ModuleID, objATI_AdminTools.ItemId.ToString(), objATI_AdminTools.Content, "ItemId=" + objATI_AdminTools.ItemId.ToString());
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
            List<ATI_AdminToolsInfo> colATI_AdminToolss  = GetATI_AdminToolss(ModuleID);

            if (colATI_AdminToolss.Count != 0)
            {
                strXML += "<ATI_AdminToolss>";
                foreach (ATI_AdminToolsInfo objATI_AdminTools in colATI_AdminToolss)
                {
                    strXML += "<ATI_AdminTools>";
                    strXML += "<content>" + XmlUtils.XMLEncode(objATI_AdminTools.Content) + "</content>";
                    strXML += "</ATI_AdminTools>";
                }
                strXML += "</ATI_AdminToolss>";
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
            XmlNode xmlATI_AdminToolss = Globals.GetContent(Content, "ATI_AdminToolss");

            foreach (XmlNode xmlATI_AdminTools in xmlATI_AdminToolss.SelectNodes("ATI_AdminTools"))
            {
                ATI_AdminToolsInfo objATI_AdminTools = new ATI_AdminToolsInfo();

                objATI_AdminTools.ModuleId = ModuleID;
                objATI_AdminTools.Content = xmlATI_AdminTools.SelectSingleNode("content").InnerText;
                objATI_AdminTools.CreatedByUser = UserId;
                AddATI_AdminTools(objATI_AdminTools);
            }

}

    #endregion

    }
}

