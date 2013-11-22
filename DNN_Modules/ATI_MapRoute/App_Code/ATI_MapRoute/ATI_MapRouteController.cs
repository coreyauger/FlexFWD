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

namespace Affine.Dnn.Modules.ATI_MapRoute
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
    public class ATI_MapRouteController : ISearchable, IPortable
    {

    #region Constructors

        public ATI_MapRouteController()
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
        /// <param name="objATI_MapRoute">The ATI_MapRouteInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void AddATI_MapRoute(ATI_MapRouteInfo objATI_MapRoute)
        {
            if (objATI_MapRoute.Content.Trim() != "")
            {
                DataProvider.Instance().AddATI_MapRoute(objATI_MapRoute.ModuleId, objATI_MapRoute.Content, objATI_MapRoute.CreatedByUser);
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
        public void DeleteATI_MapRoute(int ModuleId, int ItemId) 
        {
            DataProvider.Instance().DeleteATI_MapRoute(ModuleId,ItemId);
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
        public ATI_MapRouteInfo GetATI_MapRoute(int ModuleId, int ItemId)
        {
            return CBO.FillObject < ATI_MapRouteInfo >(DataProvider.Instance().GetATI_MapRoute(ModuleId, ItemId));
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
        public List<ATI_MapRouteInfo> GetATI_MapRoutes(int ModuleId)
        {
            return CBO.FillCollection< ATI_MapRouteInfo >(DataProvider.Instance().GetATI_MapRoutes(ModuleId));
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// saves an object to the database
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="objATI_MapRoute">The ATI_MapRouteInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void UpdateATI_MapRoute(ATI_MapRouteInfo objATI_MapRoute)
        {
            if (objATI_MapRoute.Content.Trim() != "")
            {
                DataProvider.Instance().UpdateATI_MapRoute(objATI_MapRoute.ModuleId, objATI_MapRoute.ItemId, objATI_MapRoute.Content, objATI_MapRoute.CreatedByUser);
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
            List<ATI_MapRouteInfo> colATI_MapRoutes  = GetATI_MapRoutes(ModInfo.ModuleID);

            foreach (ATI_MapRouteInfo objATI_MapRoute in colATI_MapRoutes)
            {
                if(objATI_MapRoute != null)
                {
                    SearchItemInfo SearchItem = new SearchItemInfo(ModInfo.ModuleTitle, objATI_MapRoute.Content, objATI_MapRoute.CreatedByUser, objATI_MapRoute.CreatedDate, ModInfo.ModuleID, objATI_MapRoute.ItemId.ToString(), objATI_MapRoute.Content, "ItemId=" + objATI_MapRoute.ItemId.ToString());
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
            List<ATI_MapRouteInfo> colATI_MapRoutes  = GetATI_MapRoutes(ModuleID);

            if (colATI_MapRoutes.Count != 0)
            {
                strXML += "<ATI_MapRoutes>";
                foreach (ATI_MapRouteInfo objATI_MapRoute in colATI_MapRoutes)
                {
                    strXML += "<ATI_MapRoute>";
                    strXML += "<content>" + XmlUtils.XMLEncode(objATI_MapRoute.Content) + "</content>";
                    strXML += "</ATI_MapRoute>";
                }
                strXML += "</ATI_MapRoutes>";
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
            XmlNode xmlATI_MapRoutes = Globals.GetContent(Content, "ATI_MapRoutes");

            foreach (XmlNode xmlATI_MapRoute in xmlATI_MapRoutes.SelectNodes("ATI_MapRoute"))
            {
                ATI_MapRouteInfo objATI_MapRoute = new ATI_MapRouteInfo();

                objATI_MapRoute.ModuleId = ModuleID;
                objATI_MapRoute.Content = xmlATI_MapRoute.SelectSingleNode("content").InnerText;
                objATI_MapRoute.CreatedByUser = UserId;
                AddATI_MapRoute(objATI_MapRoute);
            }

}

    #endregion

    }
}

