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

namespace Affine.Dnn.Modules.ATI_CompLeaderBoard
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
    public class ATI_CompLeaderBoardController : ISearchable, IPortable
    {

    #region Constructors

        public ATI_CompLeaderBoardController()
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
        /// <param name="objATI_CompLeaderBoard">The ATI_CompLeaderBoardInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void AddATI_CompLeaderBoard(ATI_CompLeaderBoardInfo objATI_CompLeaderBoard)
        {
            if (objATI_CompLeaderBoard.Content.Trim() != "")
            {
                DataProvider.Instance().AddATI_CompLeaderBoard(objATI_CompLeaderBoard.ModuleId, objATI_CompLeaderBoard.Content, objATI_CompLeaderBoard.CreatedByUser);
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
        public void DeleteATI_CompLeaderBoard(int ModuleId, int ItemId) 
        {
            DataProvider.Instance().DeleteATI_CompLeaderBoard(ModuleId,ItemId);
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
        public ATI_CompLeaderBoardInfo GetATI_CompLeaderBoard(int ModuleId, int ItemId)
        {
            return CBO.FillObject < ATI_CompLeaderBoardInfo >(DataProvider.Instance().GetATI_CompLeaderBoard(ModuleId, ItemId));
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
        public List<ATI_CompLeaderBoardInfo> GetATI_CompLeaderBoards(int ModuleId)
        {
            return CBO.FillCollection< ATI_CompLeaderBoardInfo >(DataProvider.Instance().GetATI_CompLeaderBoards(ModuleId));
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// saves an object to the database
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="objATI_CompLeaderBoard">The ATI_CompLeaderBoardInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void UpdateATI_CompLeaderBoard(ATI_CompLeaderBoardInfo objATI_CompLeaderBoard)
        {
            if (objATI_CompLeaderBoard.Content.Trim() != "")
            {
                DataProvider.Instance().UpdateATI_CompLeaderBoard(objATI_CompLeaderBoard.ModuleId, objATI_CompLeaderBoard.ItemId, objATI_CompLeaderBoard.Content, objATI_CompLeaderBoard.CreatedByUser);
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
            List<ATI_CompLeaderBoardInfo> colATI_CompLeaderBoards  = GetATI_CompLeaderBoards(ModInfo.ModuleID);

            foreach (ATI_CompLeaderBoardInfo objATI_CompLeaderBoard in colATI_CompLeaderBoards)
            {
                if(objATI_CompLeaderBoard != null)
                {
                    SearchItemInfo SearchItem = new SearchItemInfo(ModInfo.ModuleTitle, objATI_CompLeaderBoard.Content, objATI_CompLeaderBoard.CreatedByUser, objATI_CompLeaderBoard.CreatedDate, ModInfo.ModuleID, objATI_CompLeaderBoard.ItemId.ToString(), objATI_CompLeaderBoard.Content, "ItemId=" + objATI_CompLeaderBoard.ItemId.ToString());
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
            List<ATI_CompLeaderBoardInfo> colATI_CompLeaderBoards  = GetATI_CompLeaderBoards(ModuleID);

            if (colATI_CompLeaderBoards.Count != 0)
            {
                strXML += "<ATI_CompLeaderBoards>";
                foreach (ATI_CompLeaderBoardInfo objATI_CompLeaderBoard in colATI_CompLeaderBoards)
                {
                    strXML += "<ATI_CompLeaderBoard>";
                    strXML += "<content>" + XmlUtils.XMLEncode(objATI_CompLeaderBoard.Content) + "</content>";
                    strXML += "</ATI_CompLeaderBoard>";
                }
                strXML += "</ATI_CompLeaderBoards>";
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
            XmlNode xmlATI_CompLeaderBoards = Globals.GetContent(Content, "ATI_CompLeaderBoards");

            foreach (XmlNode xmlATI_CompLeaderBoard in xmlATI_CompLeaderBoards.SelectNodes("ATI_CompLeaderBoard"))
            {
                ATI_CompLeaderBoardInfo objATI_CompLeaderBoard = new ATI_CompLeaderBoardInfo();

                objATI_CompLeaderBoard.ModuleId = ModuleID;
                objATI_CompLeaderBoard.Content = xmlATI_CompLeaderBoard.SelectSingleNode("content").InnerText;
                objATI_CompLeaderBoard.CreatedByUser = UserId;
                AddATI_CompLeaderBoard(objATI_CompLeaderBoard);
            }

}

    #endregion

    }
}

