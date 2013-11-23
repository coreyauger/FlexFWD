/*
' DotNetNuke� - http://www.dotnetnuke.com
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

namespace Affine.Dnn.Modules.ATI_Messages
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
    public class ATI_MessagesController : ISearchable, IPortable
    {

    #region Constructors

        public ATI_MessagesController()
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
        /// <param name="objATI_Messages">The ATI_MessagesInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void AddATI_Messages(ATI_MessagesInfo objATI_Messages)
        {
            if (objATI_Messages.Content.Trim() != "")
            {
                DataProvider.Instance().AddATI_Messages(objATI_Messages.ModuleId, objATI_Messages.Content, objATI_Messages.CreatedByUser);
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
        public void DeleteATI_Messages(int ModuleId, int ItemId) 
        {
            DataProvider.Instance().DeleteATI_Messages(ModuleId,ItemId);
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
        public ATI_MessagesInfo GetATI_Messages(int ModuleId, int ItemId)
        {
            return CBO.FillObject < ATI_MessagesInfo >(DataProvider.Instance().GetATI_Messages(ModuleId, ItemId));
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
        public List<ATI_MessagesInfo> GetATI_Messagess(int ModuleId)
        {
            return CBO.FillCollection< ATI_MessagesInfo >(DataProvider.Instance().GetATI_Messagess(ModuleId));
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// saves an object to the database
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="objATI_Messages">The ATI_MessagesInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void UpdateATI_Messages(ATI_MessagesInfo objATI_Messages)
        {
            if (objATI_Messages.Content.Trim() != "")
            {
                DataProvider.Instance().UpdateATI_Messages(objATI_Messages.ModuleId, objATI_Messages.ItemId, objATI_Messages.Content, objATI_Messages.CreatedByUser);
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
            List<ATI_MessagesInfo> colATI_Messagess  = GetATI_Messagess(ModInfo.ModuleID);

            foreach (ATI_MessagesInfo objATI_Messages in colATI_Messagess)
            {
                if(objATI_Messages != null)
                {
                    SearchItemInfo SearchItem = new SearchItemInfo(ModInfo.ModuleTitle, objATI_Messages.Content, objATI_Messages.CreatedByUser, objATI_Messages.CreatedDate, ModInfo.ModuleID, objATI_Messages.ItemId.ToString(), objATI_Messages.Content, "ItemId=" + objATI_Messages.ItemId.ToString());
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
            List<ATI_MessagesInfo> colATI_Messagess  = GetATI_Messagess(ModuleID);

            if (colATI_Messagess.Count != 0)
            {
                strXML += "<ATI_Messagess>";
                foreach (ATI_MessagesInfo objATI_Messages in colATI_Messagess)
                {
                    strXML += "<ATI_Messages>";
                    strXML += "<content>" + XmlUtils.XMLEncode(objATI_Messages.Content) + "</content>";
                    strXML += "</ATI_Messages>";
                }
                strXML += "</ATI_Messagess>";
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
            XmlNode xmlATI_Messagess = Globals.GetContent(Content, "ATI_Messagess");

            foreach (XmlNode xmlATI_Messages in xmlATI_Messagess.SelectNodes("ATI_Messages"))
            {
                ATI_MessagesInfo objATI_Messages = new ATI_MessagesInfo();

                objATI_Messages.ModuleId = ModuleID;
                objATI_Messages.Content = xmlATI_Messages.SelectSingleNode("content").InnerText;
                objATI_Messages.CreatedByUser = UserId;
                AddATI_Messages(objATI_Messages);
            }

}

    #endregion

    }
}

