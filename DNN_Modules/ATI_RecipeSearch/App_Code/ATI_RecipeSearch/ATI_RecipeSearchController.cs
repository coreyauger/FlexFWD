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

namespace Affine.Dnn.Modules.ATI_RecipeSearch
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
    public class ATI_RecipeSearchController : ISearchable, IPortable
    {

    #region Constructors

        public ATI_RecipeSearchController()
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
        /// <param name="objATI_RecipeSearch">The ATI_RecipeSearchInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void AddATI_RecipeSearch(ATI_RecipeSearchInfo objATI_RecipeSearch)
        {
            if (objATI_RecipeSearch.Content.Trim() != "")
            {
                DataProvider.Instance().AddATI_RecipeSearch(objATI_RecipeSearch.ModuleId, objATI_RecipeSearch.Content, objATI_RecipeSearch.CreatedByUser);
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
        public void DeleteATI_RecipeSearch(int ModuleId, int ItemId) 
        {
            DataProvider.Instance().DeleteATI_RecipeSearch(ModuleId,ItemId);
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
        public ATI_RecipeSearchInfo GetATI_RecipeSearch(int ModuleId, int ItemId)
        {
            return CBO.FillObject < ATI_RecipeSearchInfo >(DataProvider.Instance().GetATI_RecipeSearch(ModuleId, ItemId));
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
        public List<ATI_RecipeSearchInfo> GetATI_RecipeSearchs(int ModuleId)
        {
            return CBO.FillCollection< ATI_RecipeSearchInfo >(DataProvider.Instance().GetATI_RecipeSearchs(ModuleId));
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// saves an object to the database
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="objATI_RecipeSearch">The ATI_RecipeSearchInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void UpdateATI_RecipeSearch(ATI_RecipeSearchInfo objATI_RecipeSearch)
        {
            if (objATI_RecipeSearch.Content.Trim() != "")
            {
                DataProvider.Instance().UpdateATI_RecipeSearch(objATI_RecipeSearch.ModuleId, objATI_RecipeSearch.ItemId, objATI_RecipeSearch.Content, objATI_RecipeSearch.CreatedByUser);
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
            List<ATI_RecipeSearchInfo> colATI_RecipeSearchs  = GetATI_RecipeSearchs(ModInfo.ModuleID);

            foreach (ATI_RecipeSearchInfo objATI_RecipeSearch in colATI_RecipeSearchs)
            {
                if(objATI_RecipeSearch != null)
                {
                    SearchItemInfo SearchItem = new SearchItemInfo(ModInfo.ModuleTitle, objATI_RecipeSearch.Content, objATI_RecipeSearch.CreatedByUser, objATI_RecipeSearch.CreatedDate, ModInfo.ModuleID, objATI_RecipeSearch.ItemId.ToString(), objATI_RecipeSearch.Content, "ItemId=" + objATI_RecipeSearch.ItemId.ToString());
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
            List<ATI_RecipeSearchInfo> colATI_RecipeSearchs  = GetATI_RecipeSearchs(ModuleID);

            if (colATI_RecipeSearchs.Count != 0)
            {
                strXML += "<ATI_RecipeSearchs>";
                foreach (ATI_RecipeSearchInfo objATI_RecipeSearch in colATI_RecipeSearchs)
                {
                    strXML += "<ATI_RecipeSearch>";
                    strXML += "<content>" + XmlUtils.XMLEncode(objATI_RecipeSearch.Content) + "</content>";
                    strXML += "</ATI_RecipeSearch>";
                }
                strXML += "</ATI_RecipeSearchs>";
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
            XmlNode xmlATI_RecipeSearchs = Globals.GetContent(Content, "ATI_RecipeSearchs");

            foreach (XmlNode xmlATI_RecipeSearch in xmlATI_RecipeSearchs.SelectNodes("ATI_RecipeSearch"))
            {
                ATI_RecipeSearchInfo objATI_RecipeSearch = new ATI_RecipeSearchInfo();

                objATI_RecipeSearch.ModuleId = ModuleID;
                objATI_RecipeSearch.Content = xmlATI_RecipeSearch.SelectSingleNode("content").InnerText;
                objATI_RecipeSearch.CreatedByUser = UserId;
                AddATI_RecipeSearch(objATI_RecipeSearch);
            }

}

    #endregion

    }
}

