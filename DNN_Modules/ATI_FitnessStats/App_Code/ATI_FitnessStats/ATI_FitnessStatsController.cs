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

namespace Affine.Dnn.Modules.ATI_FitnessStats
{
    /// -----------------------------------------------------------------------------
    ///<summary>
    /// The Controller class for the ATI_FitnessStats
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <history>
    /// </history>
    /// -----------------------------------------------------------------------------
    public class ATI_FitnessStatsController : ISearchable, IPortable
    {

    #region Constructors

        public ATI_FitnessStatsController()
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
        /// <param name="objATI_FitnessStats">The ATI_FitnessStatsInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void AddATI_FitnessStats(ATI_FitnessStatsInfo objATI_FitnessStats)
        {
            if (objATI_FitnessStats.Content.Trim() != "")
            {
                DataProvider.Instance().AddATI_FitnessStats(objATI_FitnessStats.ModuleId, objATI_FitnessStats.Content, objATI_FitnessStats.CreatedByUser);
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
        public void DeleteATI_FitnessStats(int ModuleId, int ItemId) 
        {
            DataProvider.Instance().DeleteATI_FitnessStats(ModuleId,ItemId);
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
        public ATI_FitnessStatsInfo GetATI_FitnessStats(int ModuleId, int ItemId)
        {
            return CBO.FillObject < ATI_FitnessStatsInfo >(DataProvider.Instance().GetATI_FitnessStats(ModuleId, ItemId));
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
        public List<ATI_FitnessStatsInfo> GetATI_FitnessStatss(int ModuleId)
        {
            return CBO.FillCollection< ATI_FitnessStatsInfo >(DataProvider.Instance().GetATI_FitnessStatss(ModuleId));
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// saves an object to the database
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="objATI_FitnessStats">The ATI_FitnessStatsInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void UpdateATI_FitnessStats(ATI_FitnessStatsInfo objATI_FitnessStats)
        {
            if (objATI_FitnessStats.Content.Trim() != "")
            {
                DataProvider.Instance().UpdateATI_FitnessStats(objATI_FitnessStats.ModuleId, objATI_FitnessStats.ItemId, objATI_FitnessStats.Content, objATI_FitnessStats.CreatedByUser);
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
            List<ATI_FitnessStatsInfo> colATI_FitnessStatss  = GetATI_FitnessStatss(ModInfo.ModuleID);

            foreach (ATI_FitnessStatsInfo objATI_FitnessStats in colATI_FitnessStatss)
            {
                if(objATI_FitnessStats != null)
                {
                    SearchItemInfo SearchItem = new SearchItemInfo(ModInfo.ModuleTitle, objATI_FitnessStats.Content, objATI_FitnessStats.CreatedByUser, objATI_FitnessStats.CreatedDate, ModInfo.ModuleID, objATI_FitnessStats.ItemId.ToString(), objATI_FitnessStats.Content, "ItemId=" + objATI_FitnessStats.ItemId.ToString());
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
            List<ATI_FitnessStatsInfo> colATI_FitnessStatss  = GetATI_FitnessStatss(ModuleID);

            if (colATI_FitnessStatss.Count != 0)
            {
                strXML += "<ATI_FitnessStatss>";
                foreach (ATI_FitnessStatsInfo objATI_FitnessStats in colATI_FitnessStatss)
                {
                    strXML += "<ATI_FitnessStats>";
                    strXML += "<content>" + XmlUtils.XMLEncode(objATI_FitnessStats.Content) + "</content>";
                    strXML += "</ATI_FitnessStats>";
                }
                strXML += "</ATI_FitnessStatss>";
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
            XmlNode xmlATI_FitnessStatss = Globals.GetContent(Content, "ATI_FitnessStatss");

            foreach (XmlNode xmlATI_FitnessStats in xmlATI_FitnessStatss.SelectNodes("ATI_FitnessStats"))
            {
                ATI_FitnessStatsInfo objATI_FitnessStats = new ATI_FitnessStatsInfo();

                objATI_FitnessStats.ModuleId = ModuleID;
                objATI_FitnessStats.Content = xmlATI_FitnessStats.SelectSingleNode("content").InnerText;
                objATI_FitnessStats.CreatedByUser = UserId;
                AddATI_FitnessStats(objATI_FitnessStats);
            }

}

    #endregion

    }
}

