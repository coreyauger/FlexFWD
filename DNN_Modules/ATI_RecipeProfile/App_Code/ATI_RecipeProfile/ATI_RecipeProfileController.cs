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

namespace Affine.Dnn.Modules.ATI_RecipeProfile
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
    public class ATI_RecipeProfileController : ISearchable, IPortable
    {

    #region Constructors

        public ATI_RecipeProfileController()
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
        /// <param name="objATI_RecipeProfile">The ATI_RecipeProfileInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void AddATI_RecipeProfile(ATI_RecipeProfileInfo objATI_RecipeProfile)
        {
            if (objATI_RecipeProfile.Content.Trim() != "")
            {
                DataProvider.Instance().AddATI_RecipeProfile(objATI_RecipeProfile.ModuleId, objATI_RecipeProfile.Content, objATI_RecipeProfile.CreatedByUser);
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
        public void DeleteATI_RecipeProfile(int ModuleId, int ItemId) 
        {
            DataProvider.Instance().DeleteATI_RecipeProfile(ModuleId,ItemId);
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
        public ATI_RecipeProfileInfo GetATI_RecipeProfile(int ModuleId, int ItemId)
        {
            return CBO.FillObject < ATI_RecipeProfileInfo >(DataProvider.Instance().GetATI_RecipeProfile(ModuleId, ItemId));
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
        public List<ATI_RecipeProfileInfo> GetATI_RecipeProfiles(int ModuleId)
        {
            return CBO.FillCollection< ATI_RecipeProfileInfo >(DataProvider.Instance().GetATI_RecipeProfiles(ModuleId));
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// saves an object to the database
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="objATI_RecipeProfile">The ATI_RecipeProfileInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void UpdateATI_RecipeProfile(ATI_RecipeProfileInfo objATI_RecipeProfile)
        {
            if (objATI_RecipeProfile.Content.Trim() != "")
            {
                DataProvider.Instance().UpdateATI_RecipeProfile(objATI_RecipeProfile.ModuleId, objATI_RecipeProfile.ItemId, objATI_RecipeProfile.Content, objATI_RecipeProfile.CreatedByUser);
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
            List<ATI_RecipeProfileInfo> colATI_RecipeProfiles  = GetATI_RecipeProfiles(ModInfo.ModuleID);

            foreach (ATI_RecipeProfileInfo objATI_RecipeProfile in colATI_RecipeProfiles)
            {
                if(objATI_RecipeProfile != null)
                {
                    SearchItemInfo SearchItem = new SearchItemInfo(ModInfo.ModuleTitle, objATI_RecipeProfile.Content, objATI_RecipeProfile.CreatedByUser, objATI_RecipeProfile.CreatedDate, ModInfo.ModuleID, objATI_RecipeProfile.ItemId.ToString(), objATI_RecipeProfile.Content, "ItemId=" + objATI_RecipeProfile.ItemId.ToString());
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
            List<ATI_RecipeProfileInfo> colATI_RecipeProfiles  = GetATI_RecipeProfiles(ModuleID);

            if (colATI_RecipeProfiles.Count != 0)
            {
                strXML += "<ATI_RecipeProfiles>";
                foreach (ATI_RecipeProfileInfo objATI_RecipeProfile in colATI_RecipeProfiles)
                {
                    strXML += "<ATI_RecipeProfile>";
                    strXML += "<content>" + XmlUtils.XMLEncode(objATI_RecipeProfile.Content) + "</content>";
                    strXML += "</ATI_RecipeProfile>";
                }
                strXML += "</ATI_RecipeProfiles>";
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
            XmlNode xmlATI_RecipeProfiles = Globals.GetContent(Content, "ATI_RecipeProfiles");

            foreach (XmlNode xmlATI_RecipeProfile in xmlATI_RecipeProfiles.SelectNodes("ATI_RecipeProfile"))
            {
                ATI_RecipeProfileInfo objATI_RecipeProfile = new ATI_RecipeProfileInfo();

                objATI_RecipeProfile.ModuleId = ModuleID;
                objATI_RecipeProfile.Content = xmlATI_RecipeProfile.SelectSingleNode("content").InnerText;
                objATI_RecipeProfile.CreatedByUser = UserId;
                AddATI_RecipeProfile(objATI_RecipeProfile);
            }

}

    #endregion

    }
}

