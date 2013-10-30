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

namespace Affine.Dnn.Modules.ATI_Compare
{
    /// -----------------------------------------------------------------------------
    ///<summary>
    /// The Controller class for the ATI_Compare
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <history>
    /// </history>
    /// -----------------------------------------------------------------------------
    public class ATI_CompareController : ISearchable, IPortable
    {

    #region Constructors

        public ATI_CompareController()
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
        /// <param name="objATI_Compare">The ATI_CompareInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void AddATI_Compare(ATI_CompareInfo objATI_Compare)
        {
            if (objATI_Compare.Content.Trim() != "")
            {
                DataProvider.Instance().AddATI_Compare(objATI_Compare.ModuleId, objATI_Compare.Content, objATI_Compare.CreatedByUser);
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
        public void DeleteATI_Compare(int ModuleId, int ItemId) 
        {
            DataProvider.Instance().DeleteATI_Compare(ModuleId,ItemId);
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
        public ATI_CompareInfo GetATI_Compare(int ModuleId, int ItemId)
        {
            return CBO.FillObject < ATI_CompareInfo >(DataProvider.Instance().GetATI_Compare(ModuleId, ItemId));
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
        public List<ATI_CompareInfo> GetATI_Compares(int ModuleId)
        {
            return CBO.FillCollection< ATI_CompareInfo >(DataProvider.Instance().GetATI_Compares(ModuleId));
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// saves an object to the database
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="objATI_Compare">The ATI_CompareInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void UpdateATI_Compare(ATI_CompareInfo objATI_Compare)
        {
            if (objATI_Compare.Content.Trim() != "")
            {
                DataProvider.Instance().UpdateATI_Compare(objATI_Compare.ModuleId, objATI_Compare.ItemId, objATI_Compare.Content, objATI_Compare.CreatedByUser);
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
            List<ATI_CompareInfo> colATI_Compares  = GetATI_Compares(ModInfo.ModuleID);

            foreach (ATI_CompareInfo objATI_Compare in colATI_Compares)
            {
                if(objATI_Compare != null)
                {
                    SearchItemInfo SearchItem = new SearchItemInfo(ModInfo.ModuleTitle, objATI_Compare.Content, objATI_Compare.CreatedByUser, objATI_Compare.CreatedDate, ModInfo.ModuleID, objATI_Compare.ItemId.ToString(), objATI_Compare.Content, "ItemId=" + objATI_Compare.ItemId.ToString());
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
            List<ATI_CompareInfo> colATI_Compares  = GetATI_Compares(ModuleID);

            if (colATI_Compares.Count != 0)
            {
                strXML += "<ATI_Compares>";
                foreach (ATI_CompareInfo objATI_Compare in colATI_Compares)
                {
                    strXML += "<ATI_Compare>";
                    strXML += "<content>" + XmlUtils.XMLEncode(objATI_Compare.Content) + "</content>";
                    strXML += "</ATI_Compare>";
                }
                strXML += "</ATI_Compares>";
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
            XmlNode xmlATI_Compares = Globals.GetContent(Content, "ATI_Compares");

            foreach (XmlNode xmlATI_Compare in xmlATI_Compares.SelectNodes("ATI_Compare"))
            {
                ATI_CompareInfo objATI_Compare = new ATI_CompareInfo();

                objATI_Compare.ModuleId = ModuleID;
                objATI_Compare.Content = xmlATI_Compare.SelectSingleNode("content").InnerText;
                objATI_Compare.CreatedByUser = UserId;
                AddATI_Compare(objATI_Compare);
            }

}

    #endregion

    }
}

