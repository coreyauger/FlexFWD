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

using DotNetNuke;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;

namespace Affine.Dnn.Modules.ATI_Base
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The EditATI_Base class is used to manage content
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <history>
    /// </history>
    /// -----------------------------------------------------------------------------
    partial class EditATI_Base : PortalModuleBase
    {

    #region Private Members

        private int ItemId = Null.NullInteger;

    #endregion

    #region Event Handlers

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Page_Load runs when the control is loaded
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        protected void Page_Load(System.Object sender, System.EventArgs e)
        {
            try
            {
                //Determine ItemId of ATI_Base to Update
                if(this.Request.QueryString["ItemId"] !=null)
                {
                    ItemId = Int32.Parse(this.Request.QueryString["ItemId"]);
                }

                //If this is the first visit to the page, bind the role data to the datalist
                if (Page.IsPostBack == false)
                {
                    cmdDelete.Attributes.Add("onClick", "javascript:return confirm('" + Localization.GetString("DeleteItem") + "');");

                    if(ItemId != -1)
                    {
                        //get content
                        ATI_BaseController objATI_Bases = new ATI_BaseController();
                        ATI_BaseInfo objATI_Base = objATI_Bases.GetATI_Base(ModuleId,ItemId);
                        if (objATI_Base != null)
                        {
                            txtContent.Text = objATI_Base.Content;
                            ctlAudit.CreatedByUser = objATI_Base.CreatedByUser.ToString();
                            ctlAudit.CreatedDate = objATI_Base.CreatedDate.ToString();
                        }
                        else
                        {
                            Response.Redirect(Globals.NavigateURL(), true);
                        }
                    }
                    else
                    {
                        cmdDelete.Visible = false;
                        ctlAudit.Visible = false;
                    }
                }
           }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// cmdCancel_Click runs when the cancel button is clicked
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        protected void cmdCancel_Click(System.Object sender, System.EventArgs e)
        {
            try
            {
                this.Response.Redirect(Globals.NavigateURL(this.TabId), true);
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// cmdDelete_Click runs when the delete button is clicked
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        protected void cmdDelete_Click(System.Object sender, System.EventArgs e)
        {
            try
            {
                //Only attempt to delete the item if it exists already
                if (!Null.IsNull(ItemId))
                {
                    ATI_BaseController objATI_Bases = new ATI_BaseController();
                    objATI_Bases.DeleteATI_Base(ModuleId,ItemId);
 
                    //refresh cache
                    SynchronizeModule();
                }

                this.Response.Redirect(Globals.NavigateURL(this.TabId), true);
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// cmdUpdate_Click runs when the update button is clicked
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        protected void cmdUpdate_Click(System.Object sender, System.EventArgs e)
        {
            try
            {
                ATI_BaseController objATI_Bases = new ATI_BaseController();
                ATI_BaseInfo objATI_Base = new ATI_BaseInfo();

                objATI_Base.ModuleId = ModuleId;
                objATI_Base.ItemId = ItemId;
                objATI_Base.Content = txtContent.Text;
                objATI_Base.CreatedByUser = this.UserId;

                //Update the content within the ATI_Base table
                if(Null.IsNull(ItemId))
                {
                    objATI_Bases.AddATI_Base(objATI_Base);
                }
                else
                {
                    objATI_Bases.UpdateATI_Base(objATI_Base);
                }

                //refresh cache
                SynchronizeModule();

                //Redirect back to the portal home page
                this.Response.Redirect(Globals.NavigateURL(this.TabId), true);
             }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

    #endregion

    }
}

