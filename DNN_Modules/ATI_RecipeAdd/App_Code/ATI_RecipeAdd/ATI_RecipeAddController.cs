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

namespace Affine.Dnn.Modules.ATI_RecipeAdd
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
    public class ATI_RecipeAddController : ISearchable, IPortable
    {

    #region Constructors

        public ATI_RecipeAddController()
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
        /// <param name="objATI_RecipeAdd">The ATI_RecipeAddInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void AddATI_RecipeAdd(ATI_RecipeAddInfo objATI_RecipeAdd)
        {
            if (objATI_RecipeAdd.Content.Trim() != "")
            {
                DataProvider.Instance().AddATI_RecipeAdd(objATI_RecipeAdd.ModuleId, objATI_RecipeAdd.Content, objATI_RecipeAdd.CreatedByUser);
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
        public void DeleteATI_RecipeAdd(int ModuleId, int ItemId) 
        {
            DataProvider.Instance().DeleteATI_RecipeAdd(ModuleId,ItemId);
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
        public ATI_RecipeAddInfo GetATI_RecipeAdd(int ModuleId, int ItemId)
        {
            return CBO.FillObject < ATI_RecipeAddInfo >(DataProvider.Instance().GetATI_RecipeAdd(ModuleId, ItemId));
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
        public List<ATI_RecipeAddInfo> GetATI_RecipeAdds(int ModuleId)
        {
            return CBO.FillCollection< ATI_RecipeAddInfo >(DataProvider.Instance().GetATI_RecipeAdds(ModuleId));
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// saves an object to the database
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="objATI_RecipeAdd">The ATI_RecipeAddInfo object</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public void UpdateATI_RecipeAdd(ATI_RecipeAddInfo objATI_RecipeAdd)
        {
            if (objATI_RecipeAdd.Content.Trim() != "")
            {
                DataProvider.Instance().UpdateATI_RecipeAdd(objATI_RecipeAdd.ModuleId, objATI_RecipeAdd.ItemId, objATI_RecipeAdd.Content, objATI_RecipeAdd.CreatedByUser);
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
            List<ATI_RecipeAddInfo> colATI_RecipeAdds  = GetATI_RecipeAdds(ModInfo.ModuleID);

            foreach (ATI_RecipeAddInfo objATI_RecipeAdd in colATI_RecipeAdds)
            {
                if(objATI_RecipeAdd != null)
                {
                    SearchItemInfo SearchItem = new SearchItemInfo(ModInfo.ModuleTitle, objATI_RecipeAdd.Content, objATI_RecipeAdd.CreatedByUser, objATI_RecipeAdd.CreatedDate, ModInfo.ModuleID, objATI_RecipeAdd.ItemId.ToString(), objATI_RecipeAdd.Content, "ItemId=" + objATI_RecipeAdd.ItemId.ToString());
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
            List<ATI_RecipeAddInfo> colATI_RecipeAdds  = GetATI_RecipeAdds(ModuleID);

            if (colATI_RecipeAdds.Count != 0)
            {
                strXML += "<ATI_RecipeAdds>";
                foreach (ATI_RecipeAddInfo objATI_RecipeAdd in colATI_RecipeAdds)
                {
                    strXML += "<ATI_RecipeAdd>";
                    strXML += "<content>" + XmlUtils.XMLEncode(objATI_RecipeAdd.Content) + "</content>";
                    strXML += "</ATI_RecipeAdd>";
                }
                strXML += "</ATI_RecipeAdds>";
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
            XmlNode xmlATI_RecipeAdds = Globals.GetContent(Content, "ATI_RecipeAdds");

            foreach (XmlNode xmlATI_RecipeAdd in xmlATI_RecipeAdds.SelectNodes("ATI_RecipeAdd"))
            {
                ATI_RecipeAddInfo objATI_RecipeAdd = new ATI_RecipeAddInfo();

                objATI_RecipeAdd.ModuleId = ModuleID;
                objATI_RecipeAdd.Content = xmlATI_RecipeAdd.SelectSingleNode("content").InnerText;
                objATI_RecipeAdd.CreatedByUser = UserId;
                AddATI_RecipeAdd(objATI_RecipeAdd);
            }

}

    #endregion

    }
}

