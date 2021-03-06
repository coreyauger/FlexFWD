/*
' DotNetNukeŽ - http://www.dotnetnuke.com
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

namespace Affine.Dnn.Modules.ATI_Feedback
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
    public class ATI_FeedbackController : ISearchable, IPortable
    {

    #region Constructors

        public ATI_FeedbackController()
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
        /// <param name="objATI_Feedback">The ATI_FeedbackInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void AddATI_Feedback(ATI_FeedbackInfo objATI_Feedback)
        {
            if (objATI_Feedback.Content.Trim() != "")
            {
                DataProvider.Instance().AddATI_Feedback(objATI_Feedback.ModuleId, objATI_Feedback.Content, objATI_Feedback.CreatedByUser);
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
        public void DeleteATI_Feedback(int ModuleId, int ItemId) 
        {
            DataProvider.Instance().DeleteATI_Feedback(ModuleId,ItemId);
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
        public ATI_FeedbackInfo GetATI_Feedback(int ModuleId, int ItemId)
        {
            return CBO.FillObject < ATI_FeedbackInfo >(DataProvider.Instance().GetATI_Feedback(ModuleId, ItemId));
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
        public List<ATI_FeedbackInfo> GetATI_Feedbacks(int ModuleId)
        {
            return CBO.FillCollection< ATI_FeedbackInfo >(DataProvider.Instance().GetATI_Feedbacks(ModuleId));
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// saves an object to the database
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="objATI_Feedback">The ATI_FeedbackInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void UpdateATI_Feedback(ATI_FeedbackInfo objATI_Feedback)
        {
            if (objATI_Feedback.Content.Trim() != "")
            {
                DataProvider.Instance().UpdateATI_Feedback(objATI_Feedback.ModuleId, objATI_Feedback.ItemId, objATI_Feedback.Content, objATI_Feedback.CreatedByUser);
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
            List<ATI_FeedbackInfo> colATI_Feedbacks  = GetATI_Feedbacks(ModInfo.ModuleID);

            foreach (ATI_FeedbackInfo objATI_Feedback in colATI_Feedbacks)
            {
                if(objATI_Feedback != null)
                {
                    SearchItemInfo SearchItem = new SearchItemInfo(ModInfo.ModuleTitle, objATI_Feedback.Content, objATI_Feedback.CreatedByUser, objATI_Feedback.CreatedDate, ModInfo.ModuleID, objATI_Feedback.ItemId.ToString(), objATI_Feedback.Content, "ItemId=" + objATI_Feedback.ItemId.ToString());
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
            List<ATI_FeedbackInfo> colATI_Feedbacks  = GetATI_Feedbacks(ModuleID);

            if (colATI_Feedbacks.Count != 0)
            {
                strXML += "<ATI_Feedbacks>";
                foreach (ATI_FeedbackInfo objATI_Feedback in colATI_Feedbacks)
                {
                    strXML += "<ATI_Feedback>";
                    strXML += "<content>" + XmlUtils.XMLEncode(objATI_Feedback.Content) + "</content>";
                    strXML += "</ATI_Feedback>";
                }
                strXML += "</ATI_Feedbacks>";
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
            XmlNode xmlATI_Feedbacks = Globals.GetContent(Content, "ATI_Feedbacks");

            foreach (XmlNode xmlATI_Feedback in xmlATI_Feedbacks.SelectNodes("ATI_Feedback"))
            {
                ATI_FeedbackInfo objATI_Feedback = new ATI_FeedbackInfo();

                objATI_Feedback.ModuleId = ModuleID;
                objATI_Feedback.Content = xmlATI_Feedback.SelectSingleNode("content").InnerText;
                objATI_Feedback.CreatedByUser = UserId;
                AddATI_Feedback(objATI_Feedback);
            }

}

    #endregion

    }
}

