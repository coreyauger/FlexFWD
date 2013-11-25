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

namespace Affine.Dnn.Modules.ATI_Modal
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
    public class ATI_ModalController : ISearchable, IPortable
    {

    #region Constructors

        public ATI_ModalController()
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
        /// <param name="objATI_Modal">The ATI_ModalInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void AddATI_Modal(ATI_ModalInfo objATI_Modal)
        {
            if (objATI_Modal.Content.Trim() != "")
            {
                DataProvider.Instance().AddATI_Modal(objATI_Modal.ModuleId, objATI_Modal.Content, objATI_Modal.CreatedByUser);
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
        public void DeleteATI_Modal(int ModuleId, int ItemId) 
        {
            DataProvider.Instance().DeleteATI_Modal(ModuleId,ItemId);
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
        public ATI_ModalInfo GetATI_Modal(int ModuleId, int ItemId)
        {
            return CBO.FillObject < ATI_ModalInfo >(DataProvider.Instance().GetATI_Modal(ModuleId, ItemId));
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
        public List<ATI_ModalInfo> GetATI_Modals(int ModuleId)
        {
            return CBO.FillCollection< ATI_ModalInfo >(DataProvider.Instance().GetATI_Modals(ModuleId));
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// saves an object to the database
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="objATI_Modal">The ATI_ModalInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void UpdateATI_Modal(ATI_ModalInfo objATI_Modal)
        {
            if (objATI_Modal.Content.Trim() != "")
            {
                DataProvider.Instance().UpdateATI_Modal(objATI_Modal.ModuleId, objATI_Modal.ItemId, objATI_Modal.Content, objATI_Modal.CreatedByUser);
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
            List<ATI_ModalInfo> colATI_Modals  = GetATI_Modals(ModInfo.ModuleID);

            foreach (ATI_ModalInfo objATI_Modal in colATI_Modals)
            {
                if(objATI_Modal != null)
                {
                    SearchItemInfo SearchItem = new SearchItemInfo(ModInfo.ModuleTitle, objATI_Modal.Content, objATI_Modal.CreatedByUser, objATI_Modal.CreatedDate, ModInfo.ModuleID, objATI_Modal.ItemId.ToString(), objATI_Modal.Content, "ItemId=" + objATI_Modal.ItemId.ToString());
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
            List<ATI_ModalInfo> colATI_Modals  = GetATI_Modals(ModuleID);

            if (colATI_Modals.Count != 0)
            {
                strXML += "<ATI_Modals>";
                foreach (ATI_ModalInfo objATI_Modal in colATI_Modals)
                {
                    strXML += "<ATI_Modal>";
                    strXML += "<content>" + XmlUtils.XMLEncode(objATI_Modal.Content) + "</content>";
                    strXML += "</ATI_Modal>";
                }
                strXML += "</ATI_Modals>";
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
            XmlNode xmlATI_Modals = Globals.GetContent(Content, "ATI_Modals");

            foreach (XmlNode xmlATI_Modal in xmlATI_Modals.SelectNodes("ATI_Modal"))
            {
                ATI_ModalInfo objATI_Modal = new ATI_ModalInfo();

                objATI_Modal.ModuleId = ModuleID;
                objATI_Modal.Content = xmlATI_Modal.SelectSingleNode("content").InnerText;
                objATI_Modal.CreatedByUser = UserId;
                AddATI_Modal(objATI_Modal);
            }

}

    #endregion

    }
}

