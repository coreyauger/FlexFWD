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

namespace Affine.Dnn.Modules.ATI_WorkoutBuilder
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
    public class ATI_WorkoutBuilderController : ISearchable, IPortable
    {

    #region Constructors

        public ATI_WorkoutBuilderController()
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
        /// <param name="objATI_WorkoutBuilder">The ATI_WorkoutBuilderInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void AddATI_WorkoutBuilder(ATI_WorkoutBuilderInfo objATI_WorkoutBuilder)
        {
            if (objATI_WorkoutBuilder.Content.Trim() != "")
            {
                DataProvider.Instance().AddATI_WorkoutBuilder(objATI_WorkoutBuilder.ModuleId, objATI_WorkoutBuilder.Content, objATI_WorkoutBuilder.CreatedByUser);
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
        public void DeleteATI_WorkoutBuilder(int ModuleId, int ItemId) 
        {
            DataProvider.Instance().DeleteATI_WorkoutBuilder(ModuleId,ItemId);
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
        public ATI_WorkoutBuilderInfo GetATI_WorkoutBuilder(int ModuleId, int ItemId)
        {
            return CBO.FillObject < ATI_WorkoutBuilderInfo >(DataProvider.Instance().GetATI_WorkoutBuilder(ModuleId, ItemId));
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
        public List<ATI_WorkoutBuilderInfo> GetATI_WorkoutBuilders(int ModuleId)
        {
            return CBO.FillCollection< ATI_WorkoutBuilderInfo >(DataProvider.Instance().GetATI_WorkoutBuilders(ModuleId));
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// saves an object to the database
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="objATI_WorkoutBuilder">The ATI_WorkoutBuilderInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void UpdateATI_WorkoutBuilder(ATI_WorkoutBuilderInfo objATI_WorkoutBuilder)
        {
            if (objATI_WorkoutBuilder.Content.Trim() != "")
            {
                DataProvider.Instance().UpdateATI_WorkoutBuilder(objATI_WorkoutBuilder.ModuleId, objATI_WorkoutBuilder.ItemId, objATI_WorkoutBuilder.Content, objATI_WorkoutBuilder.CreatedByUser);
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
            List<ATI_WorkoutBuilderInfo> colATI_WorkoutBuilders  = GetATI_WorkoutBuilders(ModInfo.ModuleID);

            foreach (ATI_WorkoutBuilderInfo objATI_WorkoutBuilder in colATI_WorkoutBuilders)
            {
                if(objATI_WorkoutBuilder != null)
                {
                    SearchItemInfo SearchItem = new SearchItemInfo(ModInfo.ModuleTitle, objATI_WorkoutBuilder.Content, objATI_WorkoutBuilder.CreatedByUser, objATI_WorkoutBuilder.CreatedDate, ModInfo.ModuleID, objATI_WorkoutBuilder.ItemId.ToString(), objATI_WorkoutBuilder.Content, "ItemId=" + objATI_WorkoutBuilder.ItemId.ToString());
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
            List<ATI_WorkoutBuilderInfo> colATI_WorkoutBuilders  = GetATI_WorkoutBuilders(ModuleID);

            if (colATI_WorkoutBuilders.Count != 0)
            {
                strXML += "<ATI_WorkoutBuilders>";
                foreach (ATI_WorkoutBuilderInfo objATI_WorkoutBuilder in colATI_WorkoutBuilders)
                {
                    strXML += "<ATI_WorkoutBuilder>";
                    strXML += "<content>" + XmlUtils.XMLEncode(objATI_WorkoutBuilder.Content) + "</content>";
                    strXML += "</ATI_WorkoutBuilder>";
                }
                strXML += "</ATI_WorkoutBuilders>";
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
            XmlNode xmlATI_WorkoutBuilders = Globals.GetContent(Content, "ATI_WorkoutBuilders");

            foreach (XmlNode xmlATI_WorkoutBuilder in xmlATI_WorkoutBuilders.SelectNodes("ATI_WorkoutBuilder"))
            {
                ATI_WorkoutBuilderInfo objATI_WorkoutBuilder = new ATI_WorkoutBuilderInfo();

                objATI_WorkoutBuilder.ModuleId = ModuleID;
                objATI_WorkoutBuilder.Content = xmlATI_WorkoutBuilder.SelectSingleNode("content").InnerText;
                objATI_WorkoutBuilder.CreatedByUser = UserId;
                AddATI_WorkoutBuilder(objATI_WorkoutBuilder);
            }

}

    #endregion

    }
}

