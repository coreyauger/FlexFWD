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

namespace Affine.Dnn.Modules.ATI_Friends
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
    public class ATI_FriendsController : ISearchable, IPortable
    {

    #region Constructors

        public ATI_FriendsController()
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
        /// <param name="objATI_Friends">The ATI_FriendsInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void AddATI_Friends(ATI_FriendsInfo objATI_Friends)
        {
            if (objATI_Friends.Content.Trim() != "")
            {
                DataProvider.Instance().AddATI_Friends(objATI_Friends.ModuleId, objATI_Friends.Content, objATI_Friends.CreatedByUser);
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
        public void DeleteATI_Friends(int ModuleId, int ItemId) 
        {
            DataProvider.Instance().DeleteATI_Friends(ModuleId,ItemId);
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
        public ATI_FriendsInfo GetATI_Friends(int ModuleId, int ItemId)
        {
            return CBO.FillObject < ATI_FriendsInfo >(DataProvider.Instance().GetATI_Friends(ModuleId, ItemId));
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
        public List<ATI_FriendsInfo> GetATI_Friendss(int ModuleId)
        {
            return CBO.FillCollection< ATI_FriendsInfo >(DataProvider.Instance().GetATI_Friendss(ModuleId));
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// saves an object to the database
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="objATI_Friends">The ATI_FriendsInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void UpdateATI_Friends(ATI_FriendsInfo objATI_Friends)
        {
            if (objATI_Friends.Content.Trim() != "")
            {
                DataProvider.Instance().UpdateATI_Friends(objATI_Friends.ModuleId, objATI_Friends.ItemId, objATI_Friends.Content, objATI_Friends.CreatedByUser);
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
            List<ATI_FriendsInfo> colATI_Friendss  = GetATI_Friendss(ModInfo.ModuleID);

            foreach (ATI_FriendsInfo objATI_Friends in colATI_Friendss)
            {
                if(objATI_Friends != null)
                {
                    SearchItemInfo SearchItem = new SearchItemInfo(ModInfo.ModuleTitle, objATI_Friends.Content, objATI_Friends.CreatedByUser, objATI_Friends.CreatedDate, ModInfo.ModuleID, objATI_Friends.ItemId.ToString(), objATI_Friends.Content, "ItemId=" + objATI_Friends.ItemId.ToString());
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
            List<ATI_FriendsInfo> colATI_Friendss  = GetATI_Friendss(ModuleID);

            if (colATI_Friendss.Count != 0)
            {
                strXML += "<ATI_Friendss>";
                foreach (ATI_FriendsInfo objATI_Friends in colATI_Friendss)
                {
                    strXML += "<ATI_Friends>";
                    strXML += "<content>" + XmlUtils.XMLEncode(objATI_Friends.Content) + "</content>";
                    strXML += "</ATI_Friends>";
                }
                strXML += "</ATI_Friendss>";
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
            XmlNode xmlATI_Friendss = Globals.GetContent(Content, "ATI_Friendss");

            foreach (XmlNode xmlATI_Friends in xmlATI_Friendss.SelectNodes("ATI_Friends"))
            {
                ATI_FriendsInfo objATI_Friends = new ATI_FriendsInfo();

                objATI_Friends.ModuleId = ModuleID;
                objATI_Friends.Content = xmlATI_Friends.SelectSingleNode("content").InnerText;
                objATI_Friends.CreatedByUser = UserId;
                AddATI_Friends(objATI_Friends);
            }

}

    #endregion

    }
}

