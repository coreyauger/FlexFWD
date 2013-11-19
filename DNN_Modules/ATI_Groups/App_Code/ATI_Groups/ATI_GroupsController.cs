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

namespace Affine.Dnn.Modules.ATI_Groups
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
    public class ATI_GroupsController : ISearchable, IPortable
    {

    #region Constructors

        public ATI_GroupsController()
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
        /// <param name="objATI_Groups">The ATI_GroupsInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void AddATI_Groups(ATI_GroupsInfo objATI_Groups)
        {
            if (objATI_Groups.Content.Trim() != "")
            {
                DataProvider.Instance().AddATI_Groups(objATI_Groups.ModuleId, objATI_Groups.Content, objATI_Groups.CreatedByUser);
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
        public void DeleteATI_Groups(int ModuleId, int ItemId) 
        {
            DataProvider.Instance().DeleteATI_Groups(ModuleId,ItemId);
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
        public ATI_GroupsInfo GetATI_Groups(int ModuleId, int ItemId)
        {
            return CBO.FillObject < ATI_GroupsInfo >(DataProvider.Instance().GetATI_Groups(ModuleId, ItemId));
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
        public List<ATI_GroupsInfo> GetATI_Groupss(int ModuleId)
        {
            return CBO.FillCollection< ATI_GroupsInfo >(DataProvider.Instance().GetATI_Groupss(ModuleId));
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// saves an object to the database
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="objATI_Groups">The ATI_GroupsInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void UpdateATI_Groups(ATI_GroupsInfo objATI_Groups)
        {
            if (objATI_Groups.Content.Trim() != "")
            {
                DataProvider.Instance().UpdateATI_Groups(objATI_Groups.ModuleId, objATI_Groups.ItemId, objATI_Groups.Content, objATI_Groups.CreatedByUser);
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
            List<ATI_GroupsInfo> colATI_Groupss  = GetATI_Groupss(ModInfo.ModuleID);

            foreach (ATI_GroupsInfo objATI_Groups in colATI_Groupss)
            {
                if(objATI_Groups != null)
                {
                    SearchItemInfo SearchItem = new SearchItemInfo(ModInfo.ModuleTitle, objATI_Groups.Content, objATI_Groups.CreatedByUser, objATI_Groups.CreatedDate, ModInfo.ModuleID, objATI_Groups.ItemId.ToString(), objATI_Groups.Content, "ItemId=" + objATI_Groups.ItemId.ToString());
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
            List<ATI_GroupsInfo> colATI_Groupss  = GetATI_Groupss(ModuleID);

            if (colATI_Groupss.Count != 0)
            {
                strXML += "<ATI_Groupss>";
                foreach (ATI_GroupsInfo objATI_Groups in colATI_Groupss)
                {
                    strXML += "<ATI_Groups>";
                    strXML += "<content>" + XmlUtils.XMLEncode(objATI_Groups.Content) + "</content>";
                    strXML += "</ATI_Groups>";
                }
                strXML += "</ATI_Groupss>";
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
            XmlNode xmlATI_Groupss = Globals.GetContent(Content, "ATI_Groupss");

            foreach (XmlNode xmlATI_Groups in xmlATI_Groupss.SelectNodes("ATI_Groups"))
            {
                ATI_GroupsInfo objATI_Groups = new ATI_GroupsInfo();

                objATI_Groups.ModuleId = ModuleID;
                objATI_Groups.Content = xmlATI_Groups.SelectSingleNode("content").InnerText;
                objATI_Groups.CreatedByUser = UserId;
                AddATI_Groups(objATI_Groups);
            }

}

    #endregion

    }
}

