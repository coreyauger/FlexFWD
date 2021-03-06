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

namespace Affine.Dnn.Modules.ATI_Register
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
    public class ATI_RegisterController : ISearchable, IPortable
    {

    #region Constructors

        public ATI_RegisterController()
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
        /// <param name="objATI_Register">The ATI_RegisterInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void AddATI_Register(ATI_RegisterInfo objATI_Register)
        {
            if (objATI_Register.Content.Trim() != "")
            {
                DataProvider.Instance().AddATI_Register(objATI_Register.ModuleId, objATI_Register.Content, objATI_Register.CreatedByUser);
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
        public void DeleteATI_Register(int ModuleId, int ItemId) 
        {
            DataProvider.Instance().DeleteATI_Register(ModuleId,ItemId);
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
        public ATI_RegisterInfo GetATI_Register(int ModuleId, int ItemId)
        {
            return CBO.FillObject < ATI_RegisterInfo >(DataProvider.Instance().GetATI_Register(ModuleId, ItemId));
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
        public List<ATI_RegisterInfo> GetATI_Registers(int ModuleId)
        {
            return CBO.FillCollection< ATI_RegisterInfo >(DataProvider.Instance().GetATI_Registers(ModuleId));
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// saves an object to the database
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="objATI_Register">The ATI_RegisterInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void UpdateATI_Register(ATI_RegisterInfo objATI_Register)
        {
            if (objATI_Register.Content.Trim() != "")
            {
                DataProvider.Instance().UpdateATI_Register(objATI_Register.ModuleId, objATI_Register.ItemId, objATI_Register.Content, objATI_Register.CreatedByUser);
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
            List<ATI_RegisterInfo> colATI_Registers  = GetATI_Registers(ModInfo.ModuleID);

            foreach (ATI_RegisterInfo objATI_Register in colATI_Registers)
            {
                if(objATI_Register != null)
                {
                    SearchItemInfo SearchItem = new SearchItemInfo(ModInfo.ModuleTitle, objATI_Register.Content, objATI_Register.CreatedByUser, objATI_Register.CreatedDate, ModInfo.ModuleID, objATI_Register.ItemId.ToString(), objATI_Register.Content, "ItemId=" + objATI_Register.ItemId.ToString());
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
            List<ATI_RegisterInfo> colATI_Registers  = GetATI_Registers(ModuleID);

            if (colATI_Registers.Count != 0)
            {
                strXML += "<ATI_Registers>";
                foreach (ATI_RegisterInfo objATI_Register in colATI_Registers)
                {
                    strXML += "<ATI_Register>";
                    strXML += "<content>" + XmlUtils.XMLEncode(objATI_Register.Content) + "</content>";
                    strXML += "</ATI_Register>";
                }
                strXML += "</ATI_Registers>";
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
            XmlNode xmlATI_Registers = Globals.GetContent(Content, "ATI_Registers");

            foreach (XmlNode xmlATI_Register in xmlATI_Registers.SelectNodes("ATI_Register"))
            {
                ATI_RegisterInfo objATI_Register = new ATI_RegisterInfo();

                objATI_Register.ModuleId = ModuleID;
                objATI_Register.Content = xmlATI_Register.SelectSingleNode("content").InnerText;
                objATI_Register.CreatedByUser = UserId;
                AddATI_Register(objATI_Register);
            }

}

    #endregion

    }
}

