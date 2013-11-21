/*
' DotNetNuke� - http://www.dotnetnuke.com
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

namespace Affine.Dnn.Modules.ATI_Intro
{
    /// -----------------------------------------------------------------------------
    ///<summary>
    /// The Controller class for the ATI_Intro
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <history>
    /// </history>
    /// -----------------------------------------------------------------------------
    public class ATI_IntroController : ISearchable, IPortable
    {

    #region Constructors

        public ATI_IntroController()
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
        /// <param name="objATI_Intro">The ATI_IntroInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void AddATI_Intro(ATI_IntroInfo objATI_Intro)
        {
            if (objATI_Intro.Content.Trim() != "")
            {
                DataProvider.Instance().AddATI_Intro(objATI_Intro.ModuleId, objATI_Intro.Content, objATI_Intro.CreatedByUser);
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
        public void DeleteATI_Intro(int ModuleId, int ItemId) 
        {
            DataProvider.Instance().DeleteATI_Intro(ModuleId,ItemId);
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
        public ATI_IntroInfo GetATI_Intro(int ModuleId, int ItemId)
        {
            return CBO.FillObject < ATI_IntroInfo >(DataProvider.Instance().GetATI_Intro(ModuleId, ItemId));
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
        public List<ATI_IntroInfo> GetATI_Intros(int ModuleId)
        {
            return CBO.FillCollection< ATI_IntroInfo >(DataProvider.Instance().GetATI_Intros(ModuleId));
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// saves an object to the database
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="objATI_Intro">The ATI_IntroInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void UpdateATI_Intro(ATI_IntroInfo objATI_Intro)
        {
            if (objATI_Intro.Content.Trim() != "")
            {
                DataProvider.Instance().UpdateATI_Intro(objATI_Intro.ModuleId, objATI_Intro.ItemId, objATI_Intro.Content, objATI_Intro.CreatedByUser);
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
            List<ATI_IntroInfo> colATI_Intros  = GetATI_Intros(ModInfo.ModuleID);

            foreach (ATI_IntroInfo objATI_Intro in colATI_Intros)
            {
                if(objATI_Intro != null)
                {
                    SearchItemInfo SearchItem = new SearchItemInfo(ModInfo.ModuleTitle, objATI_Intro.Content, objATI_Intro.CreatedByUser, objATI_Intro.CreatedDate, ModInfo.ModuleID, objATI_Intro.ItemId.ToString(), objATI_Intro.Content, "ItemId=" + objATI_Intro.ItemId.ToString());
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
            List<ATI_IntroInfo> colATI_Intros  = GetATI_Intros(ModuleID);

            if (colATI_Intros.Count != 0)
            {
                strXML += "<ATI_Intros>";
                foreach (ATI_IntroInfo objATI_Intro in colATI_Intros)
                {
                    strXML += "<ATI_Intro>";
                    strXML += "<content>" + XmlUtils.XMLEncode(objATI_Intro.Content) + "</content>";
                    strXML += "</ATI_Intro>";
                }
                strXML += "</ATI_Intros>";
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
            XmlNode xmlATI_Intros = Globals.GetContent(Content, "ATI_Intros");

            foreach (XmlNode xmlATI_Intro in xmlATI_Intros.SelectNodes("ATI_Intro"))
            {
                ATI_IntroInfo objATI_Intro = new ATI_IntroInfo();

                objATI_Intro.ModuleId = ModuleID;
                objATI_Intro.Content = xmlATI_Intro.SelectSingleNode("content").InnerText;
                objATI_Intro.CreatedByUser = UserId;
                AddATI_Intro(objATI_Intro);
            }

}

    #endregion

    }
}

