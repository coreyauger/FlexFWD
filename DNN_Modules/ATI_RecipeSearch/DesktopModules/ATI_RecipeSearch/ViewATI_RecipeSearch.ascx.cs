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
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net.Mail;
using System.Linq;
using System.IO;

using DotNetNuke;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Log.EventLog;
using DotNetNuke.Entities.Users;


using Affine.Data;
using Affine.Utils.Linq;

namespace Affine.Dnn.Modules.ATI_RecipeSearch
{
    

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The ViewATI_Builder class displays the content
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <history>
    /// </history>
    /// -----------------------------------------------------------------------------
    partial class ViewATI_RecipeSearch : Affine.Dnn.Modules.ATI_PermissionPageBase
    {

        #region Private Members
        #endregion       

        #region Public Methods

        public bool IsFavorite { get; set; }
        public long RecipeId { get; set; }
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
                this.IsFavorite = true;
                base.Page_Load(sender, e);
                this.RecipeId = 0; 
               if (Request["r"] != null || HttpContext.Current.Items["r"] != null)
                {
                    if (Request["r"] != null)
                    {
                        this.RecipeId = Convert.ToInt64( Request["r"] );
                    }
                    else if (HttpContext.Current.Items["r"] != null)
                    {
                        this.RecipeId = Convert.ToInt64(HttpContext.Current.Items["r"]);
                    }
                }
                if (!Page.IsPostBack && !Page.IsCallback)
                {
                    atiSearchBar.Visible = Settings["ShowSearchBar"] == null ? true : Convert.ToBoolean(Settings["ShowSearchBar"]);
                    ServiceReference service = new ServiceReference("~/DesktopModules/ATI_Base/resources/services/StreamService.asmx");
                    service.InlineScript = true;
                    ScriptManager.GetCurrent(Page).Services.Add(service);
                    if (Request["r"] != null || HttpContext.Current.Items["r"] != null )
                    {
                        atiRecipePanel.Visible = true;
                        atiRecipe.ShowLogin = this.UserId <= 0;
                        atiSearchResults.Visible = false;
                        atiRecipe.RecipeId = Convert.ToString(this.RecipeId);                        
                        aqufitEntities entities = new aqufitEntities();
                        User2StreamFavorites rFav = entities.User2StreamFavorites.FirstOrDefault(f => f.UserKey == this.UserId && f.PortalKey == this.PortalId && f.UserStream.Id == this.RecipeId);
                        this.IsFavorite = (rFav != null);
                        atiAddRemFav.ImageUrl = this.IsFavorite ? ResolveUrl("~/DesktopModules/ATI_Base/resources/images/remFromFav.png"):ResolveUrl("~/DesktopModules/ATI_Base/resources/images/addToFav.png") ;                                            
                        imgCorner.Src = ResolveUrl("~/DesktopModules/ATI_Base/resources/images/corner.png");
                    }else{
                        atiSearchResults.Visible = true;
                        atiRecipePanel.Visible = false;
                        atiStreamScript.EditUrl = ResolveUrl("~/AddRecipe.aspx");
                        atiStreamScript.DefaultTake = Settings["DefaultTake"] != null ? Convert.ToInt32(Settings["DefaultTake"]) : 10;

                        // pick out a featured chef
                        aqufitEntities entities = new aqufitEntities();
                        IQueryable<UserSettings> possible = entities.UserStreamSet.OfType<Recipe>().OrderByDescending( r => r.Date ).Select( r => r.UserSetting ).Distinct();
                        // radomize 20
                        int max = possible.Count() < 20 ? possible.Count() : 20;
                        Random rand = new Random();
                        IQueryable<UserSettings> featureQuery = possible.OrderByDescending( u => u.Id ).Skip(rand.Next(max)).Take(1);
                        // Hydrate users metrics
                        featureQuery.Select( m => m.Metrics ).ToArray();
                        featureQuery.Select(s => s.UserStreams.OrderByDescending(r => r.Date)).First();
                        UserSettings featured = featureQuery.First();
                        atiFeaturedProfile.Settings = featured;
                    }
                }               
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }

        }

   
        protected void atiAddRemFav_Click(object sender, ImageClickEventArgs e)
        {
            if (this.UserId > 0)
            {   // Is this user Loged in ?
                aqufitEntities entities = new aqufitEntities();
                User2StreamFavorites rFav = entities.User2StreamFavorites.FirstOrDefault(f => f.UserKey == this.UserId && f.PortalKey == this.PortalId && f.UserStream.Id == this.RecipeId);
                if (rFav == null)
                {
                    Affine.WebService.StreamService service = new WebService.StreamService();
                    string ret = service.addStreamToFavorites(this.UserId, this.PortalId, this.RecipeId);
                    if (!ret.Contains("success"))
                    {
                        litStatus.Text = ret;
                    }
                    else
                    {
                        litStatus.Text = "SUCCESS. Recipe has been added to your favorites.";
                    }
                }
                else
                {
                    entities.DeleteObject(rFav);
                    entities.SaveChanges();
                    litStatus.Text = "Boom. Recipe has been removed from your favorites.";
                }
                RadAjaxManager1.ResponseScripts.Add("$('#atiFavContainer').fadeOut('slow');");
            }
            else
            {  // otherwise jump to a login                
                RadAjaxManager1.ResponseScripts.Add("self.location.href= Aqufit.Page.LoginUrl + \"?ReturnUrl=\" + self.location.href;");
            }
        }
       

     
        protected void bPostBack_Click(object sender, EventArgs e)
        {

        }
          
                      

        #endregion

        #region Optional Interfaces

        #endregion

    }
}

