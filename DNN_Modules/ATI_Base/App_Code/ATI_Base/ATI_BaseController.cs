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

namespace Affine.Dnn.Modules.ATI_Base
{
    /// -----------------------------------------------------------------------------
    ///<summary>
    /// The Controller class for the ATI_Base
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <history>
    /// </history>
    /// -----------------------------------------------------------------------------
    public class ATI_BaseController : ISearchable, IPortable
    {

    #region Constructors

        public ATI_BaseController()
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
        /// <param name="objATI_Base">The ATI_BaseInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void AddATI_Base(ATI_BaseInfo objATI_Base)
        {
            if (objATI_Base.Content.Trim() != "")
            {
                DataProvider.Instance().AddATI_Base(objATI_Base.ModuleId, objATI_Base.Content, objATI_Base.CreatedByUser);
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
        public void DeleteATI_Base(int ModuleId, int ItemId) 
        {
            DataProvider.Instance().DeleteATI_Base(ModuleId,ItemId);
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
        public ATI_BaseInfo GetATI_Base(int ModuleId, int ItemId)
        {
            return CBO.FillObject < ATI_BaseInfo >(DataProvider.Instance().GetATI_Base(ModuleId, ItemId));
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
        public List<ATI_BaseInfo> GetATI_Bases(int ModuleId)
        {
            return CBO.FillCollection< ATI_BaseInfo >(DataProvider.Instance().GetATI_Bases(ModuleId));
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// saves an object to the database
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="objATI_Base">The ATI_BaseInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void UpdateATI_Base(ATI_BaseInfo objATI_Base)
        {
            if (objATI_Base.Content.Trim() != "")
            {
                DataProvider.Instance().UpdateATI_Base(objATI_Base.ModuleId, objATI_Base.ItemId, objATI_Base.Content, objATI_Base.CreatedByUser);
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
            List<ATI_BaseInfo> colATI_Bases  = GetATI_Bases(ModInfo.ModuleID);

            foreach (ATI_BaseInfo objATI_Base in colATI_Bases)
            {
                if(objATI_Base != null)
                {
                    SearchItemInfo SearchItem = new SearchItemInfo(ModInfo.ModuleTitle, objATI_Base.Content, objATI_Base.CreatedByUser, objATI_Base.CreatedDate, ModInfo.ModuleID, objATI_Base.ItemId.ToString(), objATI_Base.Content, "ItemId=" + objATI_Base.ItemId.ToString());
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
            List<ATI_BaseInfo> colATI_Bases  = GetATI_Bases(ModuleID);

            if (colATI_Bases.Count != 0)
            {
                strXML += "<ATI_Bases>";
                foreach (ATI_BaseInfo objATI_Base in colATI_Bases)
                {
                    strXML += "<ATI_Base>";
                    strXML += "<content>" + XmlUtils.XMLEncode(objATI_Base.Content) + "</content>";
                    strXML += "</ATI_Base>";
                }
                strXML += "</ATI_Bases>";
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
            XmlNode xmlATI_Bases = Globals.GetContent(Content, "ATI_Bases");

            foreach (XmlNode xmlATI_Base in xmlATI_Bases.SelectNodes("ATI_Base"))
            {
                ATI_BaseInfo objATI_Base = new ATI_BaseInfo();

                objATI_Base.ModuleId = ModuleID;
                objATI_Base.Content = xmlATI_Base.SelectSingleNode("content").InnerText;
                objATI_Base.CreatedByUser = UserId;
                AddATI_Base(objATI_Base);
            }

}

    #endregion

    }
}

