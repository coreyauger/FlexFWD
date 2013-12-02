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

namespace Affine.Dnn.Modules.ATI_RecipeAdd
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
    partial class ViewATI_RecipeAdd : Affine.Dnn.Modules.ATI_PermissionPageBase
    {

        #region Private Members
        #endregion       

        #region Public Methods
        
        public long RecipeEditId{ get; private set; } 
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
                atiRecipeCategoriesPanel.Controls.Add(new CheckBox()
                {
                    ID = "breakfast",
                    Text = "Breakfast",
                    Checked = false
                });
                atiRecipeCategoriesPanel.Controls.Add(new CheckBox()
                {
                    ID = "lunch",
                    Text = "Lunch",
                    Checked = false
                });
                atiRecipeCategoriesPanel.Controls.Add(new CheckBox()
                {
                    ID = "dinner",
                    Text = "Dinner",
                    Checked = false
                });
                atiRecipeCategoriesPanel.Controls.Add(new CheckBox()
                {
                    ID = "snack",
                    Text = "Snack",
                    Checked = false
                });
                atiRecipeCategoriesPanel.Controls.Add(new CheckBox()
                {
                    ID = "dessert",
                    Text = "Dessert",
                    Checked = false
                });

                this.RecipeEditId = 0;
                base.Page_Load(sender, e);
                if (!Page.IsPostBack && !Page.IsCallback)
                {
                    ServiceReference service = new ServiceReference("~/DesktopModules/ATI_Base/resources/services/StreamService.asmx");
                    service.InlineScript = true;
                    ScriptManager.GetCurrent(Page).Services.Add(service);
                    atiProfileImage.Settings = base.ProfileSettings;
                    atiProfileImage.IsOwner = base.Permissions == AqufitPermission.OWNER;
                    atiUploadifyImg1.Action = "recipe";
                    atiUploadifyImg2.Action = "recipe";
                    atiUploadifyImg3.Action = "recipe";
                    atiUploadifyImg4.Action = "recipe";
                    aqufitEntities entities = new aqufitEntities();
                    if (Request["s"] != null)
                    {
                        long rid = Convert.ToInt64(Request["s"]);                        
                        Affine.Data.Recipe rec = entities.UserStreamSet.OfType<Recipe>().FirstOrDefault(r => r.Id == rid && r.UserSetting.UserKey == this.UserId && r.PortalKey == this.ProfileSettings.PortalKey);
                        this.RecipeEditId = rec != null ? rec.Id : 0;
                        atHiddenRecipeId.Value = "" + this.RecipeEditId;
                    }
                    litFriendsTitle.Text = "Chefs " + ProfileSettings.UserName + " follows";
                    IList<long> friendIds = entities.UserFriends.Where(f => (f.SrcUserSettingKey == this.ProfileSettings.Id)).Select(f => f.DestUserSettingKey).ToList();
                    UserSettings[] firendSettings = entities.UserSettings.Where(LinqUtils.BuildContainsExpression<UserSettings, long>(s => s.Id, friendIds)).Where(f => f.PortalKey == this.PortalId).ToArray();
                    atiFriendsPhotos.RelationshipType = Affine.Relationship.FOLLOWING;
                    atiFriendsPhotos.FriendKeyList = firendSettings;
                    atiFriendsPhotos.User = base.ProfileSettings;
                    atiFriendsPhotos.FriendCount = friendIds.Count;                    
                }               
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }          
        }

        protected void bSaveRecipe_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    // TODO: category tags..
                    long rid = Convert.ToInt64(atHiddenRecipeId.Value);
                    if (rid > 0)
                    {
                        aqufitEntities entities = new aqufitEntities();
                        Recipe r = entities.UserStreamSet.OfType<Recipe>().FirstOrDefault(s => s.Id == rid && s.UserSetting.UserKey == this.UserId && s.UserSetting.PortalKey == this.PortalId);
                        if (r == null)  // security exception
                        {
                            // TODO: send a notification email
                            throw new Exception("Security Exception:  User does not own data.  Action has been logged");
                        }
                    }
                    string[] ingredients = atiRecipeIngredients.Text.Split('\n');
                    Affine.Data.json.RecipeIngredient[] riArray = ingredients.Where( i => !string.IsNullOrEmpty(i) ).Select(i => new Affine.Data.json.RecipeIngredient() { Name = i }).ToArray();                   

                    Affine.Data.json.RecipeExtended re = new Data.json.RecipeExtended()
                    {
                        Directions = atiRecipeDirections.Text,
                        Ingredients = riArray
                    };
                    // TODO: Ingredients
                    string extraTags = string.Empty;
                    CheckBox cbBreakfast = (CheckBox)atiRecipeCategoriesPanel.FindControl("breakfast");
                    if( cbBreakfast.Checked ){
                        extraTags += ","+cbBreakfast.Text;
                    }
                    CheckBox cbLunch = (CheckBox)atiRecipeCategoriesPanel.FindControl("lunch");
                    if (cbLunch.Checked)
                    {
                        extraTags += "," + cbLunch.Text;
                    }
                    CheckBox cbDinner = (CheckBox)atiRecipeCategoriesPanel.FindControl("dinner");
                    if (cbDinner.Checked)
                    {
                        extraTags += "," + cbDinner.Text;
                    }
                    CheckBox cbSnack = (CheckBox)atiRecipeCategoriesPanel.FindControl("snack");
                    if (cbSnack.Checked)
                    {
                        extraTags += "," + cbSnack.Text;
                    }
                    CheckBox cbDessert = (CheckBox)atiRecipeCategoriesPanel.FindControl("dessert");
                    if (cbDessert.Checked)
                    {
                        extraTags += "," + cbDessert.Text;
                    }
                    Affine.Data.json.StreamData recipe = new Data.json.StreamData()
                    {
                        Id = rid,
                        Title = atiRecipeName.Text,
                        Description = atiRecipeDescription.Text,
                        AvrRating = Convert.ToDouble(atiHiddenRecipeRate.Value),
                        AvrStrictness = Convert.ToDouble(atiHiddenRecipeStrict.Value),
                        Tags = atiRecipeTags.Text + extraTags,
                        NumServings = Convert.ToInt32(atiRecipeServings.Text),
                        TimePrep = Convert.ToInt32(atiRecipePrep.Text),
                        TimeCook = Convert.ToInt32(atiRecipeCook.Text),
                        RecipeExtended = re
                    };

                    Affine.WebService.StreamService service = new WebService.StreamService();
                    long id = service.SaveRecipe(this.UserId, this.PortalId, recipe);
                   // long id = 0;
                    litStatus.Text = "<em>SUCESS</em> Your recipe has been saved.  To Finish attach some image files then press 'done'.";
                    RadAjaxManager1.ResponseScripts.Add(" Paleo.AddMedia("+id+"); ");
                    
                }
            }
            catch (Exception ex)
            {
                litStatus.Text = "<span style=\"color: red;\"><em>FAIL</em> "+ex.Message+"</span>";
                RadAjaxManager1.ResponseScripts.Add(" Paleo.OnFail(); ");
            }
        }

        protected void bRemoveImg1_Click(object sender, EventArgs e)
        {
            
            long rid = Convert.ToInt64(Request["s"]);            
            if (rid > 0)
            {
                aqufitEntities entities = new aqufitEntities();
                Recipe r = entities.UserStreamSet.OfType<Recipe>().Include("RecipeExtendeds").Include("RecipeExtendeds.Image").FirstOrDefault(s => s.Id == rid && s.UserSetting.UserKey == this.UserId && s.UserSetting.PortalKey == this.PortalId);
                if (r == null)  // security exception
                {
                    // TODO: send a notification email
                //    throw new Exception("Security Exception:  User does not own data.  Action has been logged");
                }
                Affine.Data.Image img = r.RecipeExtendeds.First().Image;
                entities.DeleteObject(img);
                entities.SaveChanges();
                litStatus.Text = "Image Removed.";
                RadAjaxManager1.ResponseScripts.Add(" $('#atiRecipeImg1Div').hide(); Aqufit.Page.atiUploadifyImg1.show();");
            }                       
        }

        protected void bRemoveImg_Click(object sender, EventArgs e)
        {
            try
            {
                int buttonNum = Convert.ToInt32(((Button)sender).ID.Replace("bRemoveImg", ""));
                long rid = Convert.ToInt64(Request["s"]);
                if (rid > 0)
                {
                    aqufitEntities entities = new aqufitEntities();
                    Recipe r = entities.UserStreamSet.OfType<Recipe>().Include("RecipeExtendeds").FirstOrDefault(s => s.Id == rid && s.UserSetting.UserKey == this.UserId && s.UserSetting.PortalKey == this.PortalId);
                    Affine.Data.Image img = null;
                    if (buttonNum == 2)
                    {
                        long key = Convert.ToInt64(r.RecipeExtendeds.First().Image2Key);
                        img = entities.Image.FirstOrDefault(i => i.Id == key);
                        r.RecipeExtendeds.First().Image2Key = null;
                    }
                    else if (buttonNum == 3)
                    {   
                        long key = Convert.ToInt64( r.RecipeExtendeds.First().Image3Key );
                        img = entities.Image.FirstOrDefault(i => i.Id == key);
                        r.RecipeExtendeds.First().Image3Key = null;
                    }
                    else if (buttonNum == 4)
                    {
                        long key = Convert.ToInt64( r.RecipeExtendeds.First().Image4Key );
                        img = entities.Image.FirstOrDefault(i => i.Id == key);
                        r.RecipeExtendeds.First().Image4Key = null;
                    }
                    entities.DeleteObject(img);
                    entities.SaveChanges();
                    litStatus.Text = "Image Removed.";
                    RadAjaxManager1.ResponseScripts.Add(" $('#atiRecipeImg" + buttonNum + "Div').hide(); Aqufit.Page.atiUploadifyImg" + buttonNum + ".show();");
                }
            }
            catch (Exception ex)
            {
                litStatus.Text = ex.Message;
            }
        }
                      

        #endregion

        #region Optional Interfaces

        #endregion

    }
}

