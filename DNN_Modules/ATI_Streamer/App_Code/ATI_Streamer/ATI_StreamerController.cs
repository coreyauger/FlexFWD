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

namespace Affine.Dnn.Modules.ATI_Streamer
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
    public class ATI_StreamerController : ISearchable, IPortable
    {

    #region Constructors

        public ATI_StreamerController()
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
        /// <param name="objATI_Streamer">The ATI_StreamerInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void AddATI_Streamer(ATI_StreamerInfo objATI_Streamer)
        {
            if (objATI_Streamer.Content.Trim() != "")
            {
                DataProvider.Instance().AddATI_Streamer(objATI_Streamer.ModuleId, objATI_Streamer.Content, objATI_Streamer.CreatedByUser);
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
        public void DeleteATI_Streamer(int ModuleId, int ItemId) 
        {
            DataProvider.Instance().DeleteATI_Streamer(ModuleId,ItemId);
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
        public ATI_StreamerInfo GetATI_Streamer(int ModuleId, int ItemId)
        {
            return CBO.FillObject < ATI_StreamerInfo >(DataProvider.Instance().GetATI_Streamer(ModuleId, ItemId));
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
        public List<ATI_StreamerInfo> GetATI_Streamers(int ModuleId)
        {
            return CBO.FillCollection< ATI_StreamerInfo >(DataProvider.Instance().GetATI_Streamers(ModuleId));
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// saves an object to the database
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="objATI_Streamer">The ATI_StreamerInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void UpdateATI_Streamer(ATI_StreamerInfo objATI_Streamer)
        {
            if (objATI_Streamer.Content.Trim() != "")
            {
                DataProvider.Instance().UpdateATI_Streamer(objATI_Streamer.ModuleId, objATI_Streamer.ItemId, objATI_Streamer.Content, objATI_Streamer.CreatedByUser);
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
            List<ATI_StreamerInfo> colATI_Streamers  = GetATI_Streamers(ModInfo.ModuleID);

            foreach (ATI_StreamerInfo objATI_Streamer in colATI_Streamers)
            {
                if(objATI_Streamer != null)
                {
                    SearchItemInfo SearchItem = new SearchItemInfo(ModInfo.ModuleTitle, objATI_Streamer.Content, objATI_Streamer.CreatedByUser, objATI_Streamer.CreatedDate, ModInfo.ModuleID, objATI_Streamer.ItemId.ToString(), objATI_Streamer.Content, "ItemId=" + objATI_Streamer.ItemId.ToString());
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
            List<ATI_StreamerInfo> colATI_Streamers  = GetATI_Streamers(ModuleID);

            if (colATI_Streamers.Count != 0)
            {
                strXML += "<ATI_Streamers>";
                foreach (ATI_StreamerInfo objATI_Streamer in colATI_Streamers)
                {
                    strXML += "<ATI_Streamer>";
                    strXML += "<content>" + XmlUtils.XMLEncode(objATI_Streamer.Content) + "</content>";
                    strXML += "</ATI_Streamer>";
                }
                strXML += "</ATI_Streamers>";
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
            XmlNode xmlATI_Streamers = Globals.GetContent(Content, "ATI_Streamers");

            foreach (XmlNode xmlATI_Streamer in xmlATI_Streamers.SelectNodes("ATI_Streamer"))
            {
                ATI_StreamerInfo objATI_Streamer = new ATI_StreamerInfo();

                objATI_Streamer.ModuleId = ModuleID;
                objATI_Streamer.Content = xmlATI_Streamer.SelectSingleNode("content").InnerText;
                objATI_Streamer.CreatedByUser = UserId;
                AddATI_Streamer(objATI_Streamer);
            }

}

    #endregion

    }
}

