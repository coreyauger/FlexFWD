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

namespace Affine.Dnn.Modules.ATI_Workouts
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
    public class ATI_WorkoutsController : ISearchable, IPortable
    {

    #region Constructors

        public ATI_WorkoutsController()
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
        /// <param name="objATI_Workouts">The ATI_WorkoutsInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void AddATI_Workouts(ATI_WorkoutsInfo objATI_Workouts)
        {
            if (objATI_Workouts.Content.Trim() != "")
            {
                DataProvider.Instance().AddATI_Workouts(objATI_Workouts.ModuleId, objATI_Workouts.Content, objATI_Workouts.CreatedByUser);
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
        public void DeleteATI_Workouts(int ModuleId, int ItemId) 
        {
            DataProvider.Instance().DeleteATI_Workouts(ModuleId,ItemId);
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
        public ATI_WorkoutsInfo GetATI_Workouts(int ModuleId, int ItemId)
        {
            return CBO.FillObject < ATI_WorkoutsInfo >(DataProvider.Instance().GetATI_Workouts(ModuleId, ItemId));
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
        public List<ATI_WorkoutsInfo> GetATI_Workoutss(int ModuleId)
        {
            return CBO.FillCollection< ATI_WorkoutsInfo >(DataProvider.Instance().GetATI_Workoutss(ModuleId));
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// saves an object to the database
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="objATI_Workouts">The ATI_WorkoutsInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void UpdateATI_Workouts(ATI_WorkoutsInfo objATI_Workouts)
        {
            if (objATI_Workouts.Content.Trim() != "")
            {
                DataProvider.Instance().UpdateATI_Workouts(objATI_Workouts.ModuleId, objATI_Workouts.ItemId, objATI_Workouts.Content, objATI_Workouts.CreatedByUser);
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
            List<ATI_WorkoutsInfo> colATI_Workoutss  = GetATI_Workoutss(ModInfo.ModuleID);

            foreach (ATI_WorkoutsInfo objATI_Workouts in colATI_Workoutss)
            {
                if(objATI_Workouts != null)
                {
                    SearchItemInfo SearchItem = new SearchItemInfo(ModInfo.ModuleTitle, objATI_Workouts.Content, objATI_Workouts.CreatedByUser, objATI_Workouts.CreatedDate, ModInfo.ModuleID, objATI_Workouts.ItemId.ToString(), objATI_Workouts.Content, "ItemId=" + objATI_Workouts.ItemId.ToString());
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
            List<ATI_WorkoutsInfo> colATI_Workoutss  = GetATI_Workoutss(ModuleID);

            if (colATI_Workoutss.Count != 0)
            {
                strXML += "<ATI_Workoutss>";
                foreach (ATI_WorkoutsInfo objATI_Workouts in colATI_Workoutss)
                {
                    strXML += "<ATI_Workouts>";
                    strXML += "<content>" + XmlUtils.XMLEncode(objATI_Workouts.Content) + "</content>";
                    strXML += "</ATI_Workouts>";
                }
                strXML += "</ATI_Workoutss>";
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
            XmlNode xmlATI_Workoutss = Globals.GetContent(Content, "ATI_Workoutss");

            foreach (XmlNode xmlATI_Workouts in xmlATI_Workoutss.SelectNodes("ATI_Workouts"))
            {
                ATI_WorkoutsInfo objATI_Workouts = new ATI_WorkoutsInfo();

                objATI_Workouts.ModuleId = ModuleID;
                objATI_Workouts.Content = xmlATI_Workouts.SelectSingleNode("content").InnerText;
                objATI_Workouts.CreatedByUser = UserId;
                AddATI_Workouts(objATI_Workouts);
            }

}

    #endregion

    }
}

