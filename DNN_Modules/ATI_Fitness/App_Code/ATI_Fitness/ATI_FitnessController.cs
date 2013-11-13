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

namespace Affine.Dnn.Modules.ATI_Fitness
{
    /// -----------------------------------------------------------------------------
    ///<summary>
    /// The Controller class for the ATI_Fitness
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <history>
    /// </history>
    /// -----------------------------------------------------------------------------
    public class ATI_FitnessController : ISearchable, IPortable
    {

    #region Constructors

        public ATI_FitnessController()
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
        /// <param name="objATI_Fitness">The ATI_FitnessInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void AddATI_Fitness(ATI_FitnessInfo objATI_Fitness)
        {
            if (objATI_Fitness.Content.Trim() != "")
            {
                DataProvider.Instance().AddATI_Fitness(objATI_Fitness.ModuleId, objATI_Fitness.Content, objATI_Fitness.CreatedByUser);
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
        public void DeleteATI_Fitness(int ModuleId, int ItemId) 
        {
            DataProvider.Instance().DeleteATI_Fitness(ModuleId,ItemId);
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
        public ATI_FitnessInfo GetATI_Fitness(int ModuleId, int ItemId)
        {
            return CBO.FillObject < ATI_FitnessInfo >(DataProvider.Instance().GetATI_Fitness(ModuleId, ItemId));
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
        public List<ATI_FitnessInfo> GetATI_Fitnesss(int ModuleId)
        {
            return CBO.FillCollection< ATI_FitnessInfo >(DataProvider.Instance().GetATI_Fitnesss(ModuleId));
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// saves an object to the database
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="objATI_Fitness">The ATI_FitnessInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void UpdateATI_Fitness(ATI_FitnessInfo objATI_Fitness)
        {
            if (objATI_Fitness.Content.Trim() != "")
            {
                DataProvider.Instance().UpdateATI_Fitness(objATI_Fitness.ModuleId, objATI_Fitness.ItemId, objATI_Fitness.Content, objATI_Fitness.CreatedByUser);
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
            List<ATI_FitnessInfo> colATI_Fitnesss  = GetATI_Fitnesss(ModInfo.ModuleID);

            foreach (ATI_FitnessInfo objATI_Fitness in colATI_Fitnesss)
            {
                if(objATI_Fitness != null)
                {
                    SearchItemInfo SearchItem = new SearchItemInfo(ModInfo.ModuleTitle, objATI_Fitness.Content, objATI_Fitness.CreatedByUser, objATI_Fitness.CreatedDate, ModInfo.ModuleID, objATI_Fitness.ItemId.ToString(), objATI_Fitness.Content, "ItemId=" + objATI_Fitness.ItemId.ToString());
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
            List<ATI_FitnessInfo> colATI_Fitnesss  = GetATI_Fitnesss(ModuleID);

            if (colATI_Fitnesss.Count != 0)
            {
                strXML += "<ATI_Fitnesss>";
                foreach (ATI_FitnessInfo objATI_Fitness in colATI_Fitnesss)
                {
                    strXML += "<ATI_Fitness>";
                    strXML += "<content>" + XmlUtils.XMLEncode(objATI_Fitness.Content) + "</content>";
                    strXML += "</ATI_Fitness>";
                }
                strXML += "</ATI_Fitnesss>";
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
            XmlNode xmlATI_Fitnesss = Globals.GetContent(Content, "ATI_Fitnesss");

            foreach (XmlNode xmlATI_Fitness in xmlATI_Fitnesss.SelectNodes("ATI_Fitness"))
            {
                ATI_FitnessInfo objATI_Fitness = new ATI_FitnessInfo();

                objATI_Fitness.ModuleId = ModuleID;
                objATI_Fitness.Content = xmlATI_Fitness.SelectSingleNode("content").InnerText;
                objATI_Fitness.CreatedByUser = UserId;
                AddATI_Fitness(objATI_Fitness);
            }

}

    #endregion

    }
}

