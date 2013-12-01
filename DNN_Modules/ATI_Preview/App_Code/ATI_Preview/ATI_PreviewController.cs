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

namespace Affine.Dnn.Modules.ATI_Preview
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
    public class ATI_PreviewController : ISearchable, IPortable
    {

    #region Constructors

        public ATI_PreviewController()
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
        /// <param name="objATI_Preview">The ATI_PreviewInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void AddATI_Preview(ATI_PreviewInfo objATI_Preview)
        {
            if (objATI_Preview.Content.Trim() != "")
            {
                DataProvider.Instance().AddATI_Preview(objATI_Preview.ModuleId, objATI_Preview.Content, objATI_Preview.CreatedByUser);
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
        public void DeleteATI_Preview(int ModuleId, int ItemId) 
        {
            DataProvider.Instance().DeleteATI_Preview(ModuleId,ItemId);
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
        public ATI_PreviewInfo GetATI_Preview(int ModuleId, int ItemId)
        {
            return CBO.FillObject < ATI_PreviewInfo >(DataProvider.Instance().GetATI_Preview(ModuleId, ItemId));
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
        public List<ATI_PreviewInfo> GetATI_Previews(int ModuleId)
        {
            return CBO.FillCollection< ATI_PreviewInfo >(DataProvider.Instance().GetATI_Previews(ModuleId));
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// saves an object to the database
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="objATI_Preview">The ATI_PreviewInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void UpdateATI_Preview(ATI_PreviewInfo objATI_Preview)
        {
            if (objATI_Preview.Content.Trim() != "")
            {
                DataProvider.Instance().UpdateATI_Preview(objATI_Preview.ModuleId, objATI_Preview.ItemId, objATI_Preview.Content, objATI_Preview.CreatedByUser);
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
            List<ATI_PreviewInfo> colATI_Previews  = GetATI_Previews(ModInfo.ModuleID);

            foreach (ATI_PreviewInfo objATI_Preview in colATI_Previews)
            {
                if(objATI_Preview != null)
                {
                    SearchItemInfo SearchItem = new SearchItemInfo(ModInfo.ModuleTitle, objATI_Preview.Content, objATI_Preview.CreatedByUser, objATI_Preview.CreatedDate, ModInfo.ModuleID, objATI_Preview.ItemId.ToString(), objATI_Preview.Content, "ItemId=" + objATI_Preview.ItemId.ToString());
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
            List<ATI_PreviewInfo> colATI_Previews  = GetATI_Previews(ModuleID);

            if (colATI_Previews.Count != 0)
            {
                strXML += "<ATI_Previews>";
                foreach (ATI_PreviewInfo objATI_Preview in colATI_Previews)
                {
                    strXML += "<ATI_Preview>";
                    strXML += "<content>" + XmlUtils.XMLEncode(objATI_Preview.Content) + "</content>";
                    strXML += "</ATI_Preview>";
                }
                strXML += "</ATI_Previews>";
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
            XmlNode xmlATI_Previews = Globals.GetContent(Content, "ATI_Previews");

            foreach (XmlNode xmlATI_Preview in xmlATI_Previews.SelectNodes("ATI_Preview"))
            {
                ATI_PreviewInfo objATI_Preview = new ATI_PreviewInfo();

                objATI_Preview.ModuleId = ModuleID;
                objATI_Preview.Content = xmlATI_Preview.SelectSingleNode("content").InnerText;
                objATI_Preview.CreatedByUser = UserId;
                AddATI_Preview(objATI_Preview);
            }

}

    #endregion

    }
}

