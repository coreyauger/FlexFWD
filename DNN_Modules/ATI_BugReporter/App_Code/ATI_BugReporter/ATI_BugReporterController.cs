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

namespace Affine.Dnn.Modules.ATI_BugReporter
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
    public class ATI_BugReporterController : ISearchable, IPortable
    {

    #region Constructors

        public ATI_BugReporterController()
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
        /// <param name="objATI_BugReporter">The ATI_BugReporterInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void AddATI_BugReporter(ATI_BugReporterInfo objATI_BugReporter)
        {
            if (objATI_BugReporter.Content.Trim() != "")
            {
                DataProvider.Instance().AddATI_BugReporter(objATI_BugReporter.ModuleId, objATI_BugReporter.Content, objATI_BugReporter.CreatedByUser);
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
        public void DeleteATI_BugReporter(int ModuleId, int ItemId) 
        {
            DataProvider.Instance().DeleteATI_BugReporter(ModuleId,ItemId);
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
        public ATI_BugReporterInfo GetATI_BugReporter(int ModuleId, int ItemId)
        {
            return CBO.FillObject < ATI_BugReporterInfo >(DataProvider.Instance().GetATI_BugReporter(ModuleId, ItemId));
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
        public List<ATI_BugReporterInfo> GetATI_BugReporters(int ModuleId)
        {
            return CBO.FillCollection< ATI_BugReporterInfo >(DataProvider.Instance().GetATI_BugReporters(ModuleId));
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// saves an object to the database
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="objATI_BugReporter">The ATI_BugReporterInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void UpdateATI_BugReporter(ATI_BugReporterInfo objATI_BugReporter)
        {
            if (objATI_BugReporter.Content.Trim() != "")
            {
                DataProvider.Instance().UpdateATI_BugReporter(objATI_BugReporter.ModuleId, objATI_BugReporter.ItemId, objATI_BugReporter.Content, objATI_BugReporter.CreatedByUser);
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
            List<ATI_BugReporterInfo> colATI_BugReporters  = GetATI_BugReporters(ModInfo.ModuleID);

            foreach (ATI_BugReporterInfo objATI_BugReporter in colATI_BugReporters)
            {
                if(objATI_BugReporter != null)
                {
                    SearchItemInfo SearchItem = new SearchItemInfo(ModInfo.ModuleTitle, objATI_BugReporter.Content, objATI_BugReporter.CreatedByUser, objATI_BugReporter.CreatedDate, ModInfo.ModuleID, objATI_BugReporter.ItemId.ToString(), objATI_BugReporter.Content, "ItemId=" + objATI_BugReporter.ItemId.ToString());
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
            List<ATI_BugReporterInfo> colATI_BugReporters  = GetATI_BugReporters(ModuleID);

            if (colATI_BugReporters.Count != 0)
            {
                strXML += "<ATI_BugReporters>";
                foreach (ATI_BugReporterInfo objATI_BugReporter in colATI_BugReporters)
                {
                    strXML += "<ATI_BugReporter>";
                    strXML += "<content>" + XmlUtils.XMLEncode(objATI_BugReporter.Content) + "</content>";
                    strXML += "</ATI_BugReporter>";
                }
                strXML += "</ATI_BugReporters>";
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
            XmlNode xmlATI_BugReporters = Globals.GetContent(Content, "ATI_BugReporters");

            foreach (XmlNode xmlATI_BugReporter in xmlATI_BugReporters.SelectNodes("ATI_BugReporter"))
            {
                ATI_BugReporterInfo objATI_BugReporter = new ATI_BugReporterInfo();

                objATI_BugReporter.ModuleId = ModuleID;
                objATI_BugReporter.Content = xmlATI_BugReporter.SelectSingleNode("content").InnerText;
                objATI_BugReporter.CreatedByUser = UserId;
                AddATI_BugReporter(objATI_BugReporter);
            }

}

    #endregion

    }
}

