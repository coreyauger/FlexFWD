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

namespace Affine.Dnn.Modules.ATI_NikePlus
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
    public class ATI_NikePlusController : ISearchable, IPortable
    {

    #region Constructors

        public ATI_NikePlusController()
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
        /// <param name="objATI_NikePlus">The ATI_NikePlusInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void AddATI_NikePlus(ATI_NikePlusInfo objATI_NikePlus)
        {
            if (objATI_NikePlus.Content.Trim() != "")
            {
                DataProvider.Instance().AddATI_NikePlus(objATI_NikePlus.ModuleId, objATI_NikePlus.Content, objATI_NikePlus.CreatedByUser);
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
        public void DeleteATI_NikePlus(int ModuleId, int ItemId) 
        {
            DataProvider.Instance().DeleteATI_NikePlus(ModuleId,ItemId);
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
        public ATI_NikePlusInfo GetATI_NikePlus(int ModuleId, int ItemId)
        {
            return CBO.FillObject < ATI_NikePlusInfo >(DataProvider.Instance().GetATI_NikePlus(ModuleId, ItemId));
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
        public List<ATI_NikePlusInfo> GetATI_NikePluss(int ModuleId)
        {
            return CBO.FillCollection< ATI_NikePlusInfo >(DataProvider.Instance().GetATI_NikePluss(ModuleId));
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// saves an object to the database
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="objATI_NikePlus">The ATI_NikePlusInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void UpdateATI_NikePlus(ATI_NikePlusInfo objATI_NikePlus)
        {
            if (objATI_NikePlus.Content.Trim() != "")
            {
                DataProvider.Instance().UpdateATI_NikePlus(objATI_NikePlus.ModuleId, objATI_NikePlus.ItemId, objATI_NikePlus.Content, objATI_NikePlus.CreatedByUser);
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
            List<ATI_NikePlusInfo> colATI_NikePluss  = GetATI_NikePluss(ModInfo.ModuleID);

            foreach (ATI_NikePlusInfo objATI_NikePlus in colATI_NikePluss)
            {
                if(objATI_NikePlus != null)
                {
                    SearchItemInfo SearchItem = new SearchItemInfo(ModInfo.ModuleTitle, objATI_NikePlus.Content, objATI_NikePlus.CreatedByUser, objATI_NikePlus.CreatedDate, ModInfo.ModuleID, objATI_NikePlus.ItemId.ToString(), objATI_NikePlus.Content, "ItemId=" + objATI_NikePlus.ItemId.ToString());
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
            List<ATI_NikePlusInfo> colATI_NikePluss  = GetATI_NikePluss(ModuleID);

            if (colATI_NikePluss.Count != 0)
            {
                strXML += "<ATI_NikePluss>";
                foreach (ATI_NikePlusInfo objATI_NikePlus in colATI_NikePluss)
                {
                    strXML += "<ATI_NikePlus>";
                    strXML += "<content>" + XmlUtils.XMLEncode(objATI_NikePlus.Content) + "</content>";
                    strXML += "</ATI_NikePlus>";
                }
                strXML += "</ATI_NikePluss>";
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
            XmlNode xmlATI_NikePluss = Globals.GetContent(Content, "ATI_NikePluss");

            foreach (XmlNode xmlATI_NikePlus in xmlATI_NikePluss.SelectNodes("ATI_NikePlus"))
            {
                ATI_NikePlusInfo objATI_NikePlus = new ATI_NikePlusInfo();

                objATI_NikePlus.ModuleId = ModuleID;
                objATI_NikePlus.Content = xmlATI_NikePlus.SelectSingleNode("content").InnerText;
                objATI_NikePlus.CreatedByUser = UserId;
                AddATI_NikePlus(objATI_NikePlus);
            }

}

    #endregion

    }
}

