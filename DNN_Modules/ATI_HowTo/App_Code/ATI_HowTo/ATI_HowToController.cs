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

namespace Affine.Dnn.Modules.ATI_HowTo
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
    public class ATI_HowToController : ISearchable, IPortable
    {

    #region Constructors

        public ATI_HowToController()
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
        /// <param name="objATI_HowTo">The ATI_HowToInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void AddATI_HowTo(ATI_HowToInfo objATI_HowTo)
        {
            if (objATI_HowTo.Content.Trim() != "")
            {
                DataProvider.Instance().AddATI_HowTo(objATI_HowTo.ModuleId, objATI_HowTo.Content, objATI_HowTo.CreatedByUser);
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
        public void DeleteATI_HowTo(int ModuleId, int ItemId) 
        {
            DataProvider.Instance().DeleteATI_HowTo(ModuleId,ItemId);
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
        public ATI_HowToInfo GetATI_HowTo(int ModuleId, int ItemId)
        {
            return CBO.FillObject < ATI_HowToInfo >(DataProvider.Instance().GetATI_HowTo(ModuleId, ItemId));
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
        public List<ATI_HowToInfo> GetATI_HowTos(int ModuleId)
        {
            return CBO.FillCollection< ATI_HowToInfo >(DataProvider.Instance().GetATI_HowTos(ModuleId));
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// saves an object to the database
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="objATI_HowTo">The ATI_HowToInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void UpdateATI_HowTo(ATI_HowToInfo objATI_HowTo)
        {
            if (objATI_HowTo.Content.Trim() != "")
            {
                DataProvider.Instance().UpdateATI_HowTo(objATI_HowTo.ModuleId, objATI_HowTo.ItemId, objATI_HowTo.Content, objATI_HowTo.CreatedByUser);
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
            List<ATI_HowToInfo> colATI_HowTos  = GetATI_HowTos(ModInfo.ModuleID);

            foreach (ATI_HowToInfo objATI_HowTo in colATI_HowTos)
            {
                if(objATI_HowTo != null)
                {
                    SearchItemInfo SearchItem = new SearchItemInfo(ModInfo.ModuleTitle, objATI_HowTo.Content, objATI_HowTo.CreatedByUser, objATI_HowTo.CreatedDate, ModInfo.ModuleID, objATI_HowTo.ItemId.ToString(), objATI_HowTo.Content, "ItemId=" + objATI_HowTo.ItemId.ToString());
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
            List<ATI_HowToInfo> colATI_HowTos  = GetATI_HowTos(ModuleID);

            if (colATI_HowTos.Count != 0)
            {
                strXML += "<ATI_HowTos>";
                foreach (ATI_HowToInfo objATI_HowTo in colATI_HowTos)
                {
                    strXML += "<ATI_HowTo>";
                    strXML += "<content>" + XmlUtils.XMLEncode(objATI_HowTo.Content) + "</content>";
                    strXML += "</ATI_HowTo>";
                }
                strXML += "</ATI_HowTos>";
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
            XmlNode xmlATI_HowTos = Globals.GetContent(Content, "ATI_HowTos");

            foreach (XmlNode xmlATI_HowTo in xmlATI_HowTos.SelectNodes("ATI_HowTo"))
            {
                ATI_HowToInfo objATI_HowTo = new ATI_HowToInfo();

                objATI_HowTo.ModuleId = ModuleID;
                objATI_HowTo.Content = xmlATI_HowTo.SelectSingleNode("content").InnerText;
                objATI_HowTo.CreatedByUser = UserId;
                AddATI_HowTo(objATI_HowTo);
            }

}

    #endregion

    }
}

