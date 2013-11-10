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

namespace Affine.Dnn.Modules.ATI_CompRegister
{
    /// -----------------------------------------------------------------------------
    ///<summary>
    /// The Controller class for the ATI_CompRegister
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <history>
    /// </history>
    /// -----------------------------------------------------------------------------
    public class ATI_CompRegisterController : ISearchable, IPortable
    {

    #region Constructors

        public ATI_CompRegisterController()
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
        /// <param name="objATI_CompRegister">The ATI_CompRegisterInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void AddATI_CompRegister(ATI_CompRegisterInfo objATI_CompRegister)
        {
            if (objATI_CompRegister.Content.Trim() != "")
            {
                DataProvider.Instance().AddATI_CompRegister(objATI_CompRegister.ModuleId, objATI_CompRegister.Content, objATI_CompRegister.CreatedByUser);
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
        public void DeleteATI_CompRegister(int ModuleId, int ItemId) 
        {
            DataProvider.Instance().DeleteATI_CompRegister(ModuleId,ItemId);
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
        public ATI_CompRegisterInfo GetATI_CompRegister(int ModuleId, int ItemId)
        {
            return CBO.FillObject < ATI_CompRegisterInfo >(DataProvider.Instance().GetATI_CompRegister(ModuleId, ItemId));
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
        public List<ATI_CompRegisterInfo> GetATI_CompRegisters(int ModuleId)
        {
            return CBO.FillCollection< ATI_CompRegisterInfo >(DataProvider.Instance().GetATI_CompRegisters(ModuleId));
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// saves an object to the database
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="objATI_CompRegister">The ATI_CompRegisterInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void UpdateATI_CompRegister(ATI_CompRegisterInfo objATI_CompRegister)
        {
            if (objATI_CompRegister.Content.Trim() != "")
            {
                DataProvider.Instance().UpdateATI_CompRegister(objATI_CompRegister.ModuleId, objATI_CompRegister.ItemId, objATI_CompRegister.Content, objATI_CompRegister.CreatedByUser);
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
            List<ATI_CompRegisterInfo> colATI_CompRegisters  = GetATI_CompRegisters(ModInfo.ModuleID);

            foreach (ATI_CompRegisterInfo objATI_CompRegister in colATI_CompRegisters)
            {
                if(objATI_CompRegister != null)
                {
                    SearchItemInfo SearchItem = new SearchItemInfo(ModInfo.ModuleTitle, objATI_CompRegister.Content, objATI_CompRegister.CreatedByUser, objATI_CompRegister.CreatedDate, ModInfo.ModuleID, objATI_CompRegister.ItemId.ToString(), objATI_CompRegister.Content, "ItemId=" + objATI_CompRegister.ItemId.ToString());
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
            List<ATI_CompRegisterInfo> colATI_CompRegisters  = GetATI_CompRegisters(ModuleID);

            if (colATI_CompRegisters.Count != 0)
            {
                strXML += "<ATI_CompRegisters>";
                foreach (ATI_CompRegisterInfo objATI_CompRegister in colATI_CompRegisters)
                {
                    strXML += "<ATI_CompRegister>";
                    strXML += "<content>" + XmlUtils.XMLEncode(objATI_CompRegister.Content) + "</content>";
                    strXML += "</ATI_CompRegister>";
                }
                strXML += "</ATI_CompRegisters>";
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
            XmlNode xmlATI_CompRegisters = Globals.GetContent(Content, "ATI_CompRegisters");

            foreach (XmlNode xmlATI_CompRegister in xmlATI_CompRegisters.SelectNodes("ATI_CompRegister"))
            {
                ATI_CompRegisterInfo objATI_CompRegister = new ATI_CompRegisterInfo();

                objATI_CompRegister.ModuleId = ModuleID;
                objATI_CompRegister.Content = xmlATI_CompRegister.SelectSingleNode("content").InnerText;
                objATI_CompRegister.CreatedByUser = UserId;
                AddATI_CompRegister(objATI_CompRegister);
            }

}

    #endregion

    }
}

