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

namespace Affine.Dnn.Modules.ATI_Viral
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
    public class ATI_ViralController : ISearchable, IPortable
    {

    #region Constructors

        public ATI_ViralController()
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
        /// <param name="objATI_Viral">The ATI_ViralInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void AddATI_Viral(ATI_ViralInfo objATI_Viral)
        {
            if (objATI_Viral.Content.Trim() != "")
            {
                DataProvider.Instance().AddATI_Viral(objATI_Viral.ModuleId, objATI_Viral.Content, objATI_Viral.CreatedByUser);
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
        public void DeleteATI_Viral(int ModuleId, int ItemId) 
        {
            DataProvider.Instance().DeleteATI_Viral(ModuleId,ItemId);
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
        public ATI_ViralInfo GetATI_Viral(int ModuleId, int ItemId)
        {
            return CBO.FillObject < ATI_ViralInfo >(DataProvider.Instance().GetATI_Viral(ModuleId, ItemId));
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
        public List<ATI_ViralInfo> GetATI_Virals(int ModuleId)
        {
            return CBO.FillCollection< ATI_ViralInfo >(DataProvider.Instance().GetATI_Virals(ModuleId));
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// saves an object to the database
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="objATI_Viral">The ATI_ViralInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void UpdateATI_Viral(ATI_ViralInfo objATI_Viral)
        {
            if (objATI_Viral.Content.Trim() != "")
            {
                DataProvider.Instance().UpdateATI_Viral(objATI_Viral.ModuleId, objATI_Viral.ItemId, objATI_Viral.Content, objATI_Viral.CreatedByUser);
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
            List<ATI_ViralInfo> colATI_Virals  = GetATI_Virals(ModInfo.ModuleID);

            foreach (ATI_ViralInfo objATI_Viral in colATI_Virals)
            {
                if(objATI_Viral != null)
                {
                    SearchItemInfo SearchItem = new SearchItemInfo(ModInfo.ModuleTitle, objATI_Viral.Content, objATI_Viral.CreatedByUser, objATI_Viral.CreatedDate, ModInfo.ModuleID, objATI_Viral.ItemId.ToString(), objATI_Viral.Content, "ItemId=" + objATI_Viral.ItemId.ToString());
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
            List<ATI_ViralInfo> colATI_Virals  = GetATI_Virals(ModuleID);

            if (colATI_Virals.Count != 0)
            {
                strXML += "<ATI_Virals>";
                foreach (ATI_ViralInfo objATI_Viral in colATI_Virals)
                {
                    strXML += "<ATI_Viral>";
                    strXML += "<content>" + XmlUtils.XMLEncode(objATI_Viral.Content) + "</content>";
                    strXML += "</ATI_Viral>";
                }
                strXML += "</ATI_Virals>";
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
            XmlNode xmlATI_Virals = Globals.GetContent(Content, "ATI_Virals");

            foreach (XmlNode xmlATI_Viral in xmlATI_Virals.SelectNodes("ATI_Viral"))
            {
                ATI_ViralInfo objATI_Viral = new ATI_ViralInfo();

                objATI_Viral.ModuleId = ModuleID;
                objATI_Viral.Content = xmlATI_Viral.SelectSingleNode("content").InnerText;
                objATI_Viral.CreatedByUser = UserId;
                AddATI_Viral(objATI_Viral);
            }

}

    #endregion

    }
}

