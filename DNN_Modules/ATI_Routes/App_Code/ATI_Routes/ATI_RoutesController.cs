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

namespace Affine.Dnn.Modules.ATI_Routes
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
    public class ATI_RoutesController : ISearchable, IPortable
    {

    #region Constructors

        public ATI_RoutesController()
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
        /// <param name="objATI_Routes">The ATI_RoutesInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void AddATI_Routes(ATI_RoutesInfo objATI_Routes)
        {
            if (objATI_Routes.Content.Trim() != "")
            {
                DataProvider.Instance().AddATI_Routes(objATI_Routes.ModuleId, objATI_Routes.Content, objATI_Routes.CreatedByUser);
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
        public void DeleteATI_Routes(int ModuleId, int ItemId) 
        {
            DataProvider.Instance().DeleteATI_Routes(ModuleId,ItemId);
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
        public ATI_RoutesInfo GetATI_Routes(int ModuleId, int ItemId)
        {
            return CBO.FillObject < ATI_RoutesInfo >(DataProvider.Instance().GetATI_Routes(ModuleId, ItemId));
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
        public List<ATI_RoutesInfo> GetATI_Routess(int ModuleId)
        {
            return CBO.FillCollection< ATI_RoutesInfo >(DataProvider.Instance().GetATI_Routess(ModuleId));
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// saves an object to the database
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="objATI_Routes">The ATI_RoutesInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void UpdateATI_Routes(ATI_RoutesInfo objATI_Routes)
        {
            if (objATI_Routes.Content.Trim() != "")
            {
                DataProvider.Instance().UpdateATI_Routes(objATI_Routes.ModuleId, objATI_Routes.ItemId, objATI_Routes.Content, objATI_Routes.CreatedByUser);
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
            List<ATI_RoutesInfo> colATI_Routess  = GetATI_Routess(ModInfo.ModuleID);

            foreach (ATI_RoutesInfo objATI_Routes in colATI_Routess)
            {
                if(objATI_Routes != null)
                {
                    SearchItemInfo SearchItem = new SearchItemInfo(ModInfo.ModuleTitle, objATI_Routes.Content, objATI_Routes.CreatedByUser, objATI_Routes.CreatedDate, ModInfo.ModuleID, objATI_Routes.ItemId.ToString(), objATI_Routes.Content, "ItemId=" + objATI_Routes.ItemId.ToString());
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
            List<ATI_RoutesInfo> colATI_Routess  = GetATI_Routess(ModuleID);

            if (colATI_Routess.Count != 0)
            {
                strXML += "<ATI_Routess>";
                foreach (ATI_RoutesInfo objATI_Routes in colATI_Routess)
                {
                    strXML += "<ATI_Routes>";
                    strXML += "<content>" + XmlUtils.XMLEncode(objATI_Routes.Content) + "</content>";
                    strXML += "</ATI_Routes>";
                }
                strXML += "</ATI_Routess>";
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
            XmlNode xmlATI_Routess = Globals.GetContent(Content, "ATI_Routess");

            foreach (XmlNode xmlATI_Routes in xmlATI_Routess.SelectNodes("ATI_Routes"))
            {
                ATI_RoutesInfo objATI_Routes = new ATI_RoutesInfo();

                objATI_Routes.ModuleId = ModuleID;
                objATI_Routes.Content = xmlATI_Routes.SelectSingleNode("content").InnerText;
                objATI_Routes.CreatedByUser = UserId;
                AddATI_Routes(objATI_Routes);
            }

}

    #endregion

    }
}

