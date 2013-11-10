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

namespace Affine.Dnn.Modules.ATI_CompLiveScore
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
    public class ATI_CompLiveScoreController : ISearchable, IPortable
    {

    #region Constructors

        public ATI_CompLiveScoreController()
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
        /// <param name="objATI_CompLiveScore">The ATI_CompLiveScoreInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void AddATI_CompLiveScore(ATI_CompLiveScoreInfo objATI_CompLiveScore)
        {
            if (objATI_CompLiveScore.Content.Trim() != "")
            {
                DataProvider.Instance().AddATI_CompLiveScore(objATI_CompLiveScore.ModuleId, objATI_CompLiveScore.Content, objATI_CompLiveScore.CreatedByUser);
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
        public void DeleteATI_CompLiveScore(int ModuleId, int ItemId) 
        {
            DataProvider.Instance().DeleteATI_CompLiveScore(ModuleId,ItemId);
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
        public ATI_CompLiveScoreInfo GetATI_CompLiveScore(int ModuleId, int ItemId)
        {
            return CBO.FillObject < ATI_CompLiveScoreInfo >(DataProvider.Instance().GetATI_CompLiveScore(ModuleId, ItemId));
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
        public List<ATI_CompLiveScoreInfo> GetATI_CompLiveScores(int ModuleId)
        {
            return CBO.FillCollection< ATI_CompLiveScoreInfo >(DataProvider.Instance().GetATI_CompLiveScores(ModuleId));
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// saves an object to the database
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="objATI_CompLiveScore">The ATI_CompLiveScoreInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void UpdateATI_CompLiveScore(ATI_CompLiveScoreInfo objATI_CompLiveScore)
        {
            if (objATI_CompLiveScore.Content.Trim() != "")
            {
                DataProvider.Instance().UpdateATI_CompLiveScore(objATI_CompLiveScore.ModuleId, objATI_CompLiveScore.ItemId, objATI_CompLiveScore.Content, objATI_CompLiveScore.CreatedByUser);
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
            List<ATI_CompLiveScoreInfo> colATI_CompLiveScores  = GetATI_CompLiveScores(ModInfo.ModuleID);

            foreach (ATI_CompLiveScoreInfo objATI_CompLiveScore in colATI_CompLiveScores)
            {
                if(objATI_CompLiveScore != null)
                {
                    SearchItemInfo SearchItem = new SearchItemInfo(ModInfo.ModuleTitle, objATI_CompLiveScore.Content, objATI_CompLiveScore.CreatedByUser, objATI_CompLiveScore.CreatedDate, ModInfo.ModuleID, objATI_CompLiveScore.ItemId.ToString(), objATI_CompLiveScore.Content, "ItemId=" + objATI_CompLiveScore.ItemId.ToString());
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
            List<ATI_CompLiveScoreInfo> colATI_CompLiveScores  = GetATI_CompLiveScores(ModuleID);

            if (colATI_CompLiveScores.Count != 0)
            {
                strXML += "<ATI_CompLiveScores>";
                foreach (ATI_CompLiveScoreInfo objATI_CompLiveScore in colATI_CompLiveScores)
                {
                    strXML += "<ATI_CompLiveScore>";
                    strXML += "<content>" + XmlUtils.XMLEncode(objATI_CompLiveScore.Content) + "</content>";
                    strXML += "</ATI_CompLiveScore>";
                }
                strXML += "</ATI_CompLiveScores>";
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
            XmlNode xmlATI_CompLiveScores = Globals.GetContent(Content, "ATI_CompLiveScores");

            foreach (XmlNode xmlATI_CompLiveScore in xmlATI_CompLiveScores.SelectNodes("ATI_CompLiveScore"))
            {
                ATI_CompLiveScoreInfo objATI_CompLiveScore = new ATI_CompLiveScoreInfo();

                objATI_CompLiveScore.ModuleId = ModuleID;
                objATI_CompLiveScore.Content = xmlATI_CompLiveScore.SelectSingleNode("content").InnerText;
                objATI_CompLiveScore.CreatedByUser = UserId;
                AddATI_CompLiveScore(objATI_CompLiveScore);
            }

}

    #endregion

    }
}

