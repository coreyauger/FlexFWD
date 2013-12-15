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
using System.Net;

using DotNetNuke;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Log.EventLog;
using DotNetNuke.Entities.Users;

using Telerik.Web.UI;

using System.Web.Script.Serialization;
using Affine.Data;

namespace Affine.Dnn.Modules.ATI_WorkoutBuilder
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
    partial class ViewATI_WorkoutBuilder : Affine.Dnn.Modules.ATI_PermissionPageBase, IActionable
    {

        #region Private Members
        
        #endregion       

        #region Public Methods
      
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
            base.Page_Load(sender, e);
            try
            {                
                if (!Page.IsPostBack && !Page.IsCallback)
                {
                    atiRadDatePicker.SelectedDate = DateTime.Today;
                    ServiceReference service = new ServiceReference("~/DesktopModules/ATI_Base/resources/services/StreamService.asmx");
                    service.InlineScript = true;
                    ScriptManager.GetCurrent(Page).Services.Add(service);

                    atiProfileImg.Settings = GroupSettings != null ? (Affine.Data.UserSettings)GroupSettings : UserSettings;
                    Affine.WebService.StreamService streamService = new Affine.WebService.StreamService();                    
                    aqufitEntities entities = new aqufitEntities();
                    WODType[] wodTypes =  entities.WODTypes.ToArray();
                    ddlWODType.Items.AddRange(wodTypes.Select(t => new ListItem() { Text = t.Name, Value = "" + t.Id }).ToArray());

                    // CA - new ...
                    ddlWODType2.Items.AddRange(wodTypes.Select(t => new ListItem() { Text = t.Name, Value = "" + t.Id }).ToArray());
                    RadListBoxExcerciseSource.DataValueField = "Value";
                    RadListBoxExcerciseSource.DataTextField = "Text";
                    RadListBoxExcerciseSource.DataSource = entities.Exercises.OrderBy(ex => ex.Name).Select(ex => new { Text = ex.Name, Value = ex.Id }).ToList();
                    RadListBoxExcerciseSource.DataBind();

                    string js = string.Empty;
                    if (UserSettings.MainGroupKey.HasValue && GroupSettings == null)   // Remember that "g" is when a group admin is scheduling a wod.. 
                    {
                        long[] groupIds = entities.UserFriends.Where(f => f.SrcUserSettingKey == UserSettings.Id && f.Relationship >= (int)Affine.Utils.ConstsUtil.Relationships.GROUP_OWNER).Select(f => f.DestUserSettingKey).ToArray();
                        lbGroups.DataTextField = "Text";
                        lbGroups.DataValueField = "Value";
                        lbGroups.DataSource = entities.UserSettings.OfType<Group>().Where(Affine.Utils.Linq.LinqUtils.BuildContainsExpression<UserSettings, long>(us => us.Id, groupIds)).Select(g => new { Text = g.UserFirstName, Value = g.Id }).ToList();
                        lbGroups.DataBind();
                        ListItem li = lbGroups.Items.FindByValue("" + UserSettings.MainGroupKey);
                        if (li != null)
                        {
                            li.Selected = true;
                        }
                    }
                    else
                    {   // we skip the first 2 steps of the workout creator.. since they have no group
                        js += "Aqufit.Page.Actions.HaveNoGroup(); ";
                    }

                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "ExerciseList",
                        "$(function(){ " +
                            js +
                         //   "Aqufit.Page.atiExerciseListScript.generateStreamDom('" + streamService.GetAllExercises() + "'); " +
                            "Aqufit.Page.Events.OnLoad();" +
                        "});", true);

                    string baseUrl = ResolveUrl("~");
                    aReturn.HRef = baseUrl + UserSettings.UserName;
                    aMyWorkouts.HRef = baseUrl + "Profile/MyWorkouts";
                    //Profile/MyWorkouts

                    
                    atiTemp.Visible = false;
                    atiNew.Visible = true;
                    
                }               
               
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        protected void bAjaxPostback_Click(object sender, EventArgs e)
        {
            try
            {
                aqufitEntities entities = new aqufitEntities();
                Affine.Data.Managers.IDataManager datMan = Affine.Data.Managers.LINQ.DataManager.Instance;
                switch (hiddenAjaxAction.Value)
                {
                    case "SaveWOD":                        
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        Affine.Data.json.WOD wodJson = serializer.Deserialize<Affine.Data.json.WOD>(hiddenAjaxValue.Value);
                        if (GroupSettings != null)
                        {   // if this was a group wod.. we want to bounce back to the scheduling page now...
                            wodJson.CreatorUId = GroupSettings.Id;                                                 
                        }                       
                        else
                        {
                            wodJson.CreatorUId = UserSettings.Id;                            
                        }
                        long wodId = datMan.CreateWOD(wodJson);
                        if (wodJson.GroupKey > 0 && (wodJson.GroupKey != 435 && wodJson.GroupKey != 516)) // TODO: this is a tmp switch
                        {
                            DateTime date = DateTime.Parse(wodJson.Date);
                            datMan.ScheduleGroupWOD(UserSettings.Id, wodJson.GroupKey, wodId, date, date.AddDays(-1), null, "");
                        }
                        if (GroupSettings != null)
                        {
                            Response.Redirect(ResolveUrl("~") + "group/" + GroupSettings.UserName + "?w=" + wodId, true);
                            return;
                        }
                        else
                        {
                            string gname = wodJson.GroupKey > 0 ? wodJson.GorupName : "";
                            RadAjaxManager1.ResponseScripts.Add("Aqufit.Windows.WorkoutSavedDialog.open(" + wodId + ",'" + gname + "');");
                        }
                        
                        break;                    
                }
            }
            catch (Exception ex)
            {
                // TODO: better error handling
              //  RadAjaxManager1.ResponseScripts.Add(" alert('"+ ex.StackTrace.Replace("'", "") + "');");
                RadAjaxManager1.ResponseScripts.Add(" alert('" + ex.Message + "');");
            }
        }

        protected void bSaveWOD_Click(object sender, EventArgs e)
        {
            string status = string.Empty;
            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Affine.Data.json.WOD wodJson = serializer.Deserialize<Affine.Data.json.WOD>(atiWodJson.Value);
                aqufitEntities entities = new aqufitEntities();
                
                // TODO: check if there is already a WOD of the same name     
                long wodTypeId = Convert.ToInt64(ddlWODType.SelectedValue);
                int standard = UserInfo.IsSuperUser ? 1 : 0;
                long userKey = GroupSettings != null ? this.GroupSettings.Id : this.UserSettings.Id;
                WOD wod = new WOD()
                {
                    Difficulty = wodJson.Difficulty,
                    Standard = (short)standard,
                    Name = Affine.Utils.Web.WebUtils.MakeWebSafeString( txtWorkoutName.Text ),
                    WODType = entities.WODTypes.First(t => t.Id == wodTypeId),
                    UserSettingsKey = userKey,
                    Description = Affine.Utils.Web.WebUtils.MakeWebSafeString(txtDescription.Text),
                    UserName = GroupSettings != null ? this.GroupSettings.UserName : this.UserSettings.UserName,
                    CreationDate = DateTime.Now.ToUniversalTime()                    
                };
                if (wodTypeId == (int)Affine.Utils.WorkoutUtil.WodType.AMRAP)
                {
                    wod.TimeSpan = atiTimeSpan.Time;
                }
                entities.AddToWODs(wod);
                foreach (Affine.Data.json.WODSet setJson in wodJson.WODSets)
                {
                    WODSet wodSet = new WODSet()
                    {
                        WOD = wod
                    };
                    //Affine.Data.Exercise[] exerciseArray = entities.Exercises.Where(Affine.Utils.Linq.LinqUtils.BuildContainsExpression<Affine.Data.Exercise, long>(ex => ex.Id, setJson.WODExercises.Select(ee => ee.ExcercisKey ).ToArray() )).ToArray();
                    foreach (Affine.Data.json.WODExercise exJson in setJson.WODExercises)
                    {
                        double mdist = exJson.MenDist > 0 ? Utils.UnitsUtil.unitsToSystemDefualt(exJson.MenDist, Utils.UnitsUtil.ToUnit(exJson.MenDistUnits)) : -1;
                        double wdist = exJson.WomenDist > 0 ? Utils.UnitsUtil.unitsToSystemDefualt(exJson.WomenDist, Utils.UnitsUtil.ToUnit(exJson.WomenDistUnits)) : -1;
                        double mweight = exJson.MenRx > 0 ? Utils.UnitsUtil.unitsToSystemDefualt(exJson.MenRx, Utils.UnitsUtil.ToUnit(exJson.MenRxUnits)) : -1;
                        double wweight = exJson.WomenRx > 0 ? Utils.UnitsUtil.unitsToSystemDefualt(exJson.WomenRx, Utils.UnitsUtil.ToUnit(exJson.WomenRxUnits)) : -1;
                        WODExercise wodEx = new WODExercise()
                        {
                            MenDist = mdist,
                            WomenDist = wdist,
                            WODSet = wodSet,
                            MenRx = mweight,
                            WomenRx = wweight,
                            Reps = exJson.Reps,
                            Order = exJson.Order,
                            Notes = Affine.Utils.Web.WebUtils.MakeWebSafeString(exJson.Notes),
                            Exercise = entities.Exercises.First(ee => ee.Id == exJson.ExcercisKey)
                        };
                    }
                }
                entities.SaveChanges();            
                // Now we add to peoples favs
                // ** This no longer happens here.. it happens when the workout is "scheduled" for the group that it gets added to the list..
                WOD newWOD = entities.WODs.Where(w => w.UserSettingsKey == userKey).OrderByDescending(w => w.Id).First();
                if (GroupSettings == null)
                {   // simply add for this user..
                    User2WODFav fav = new User2WODFav()
                    {
                        UserSetting = entities.UserSettings.First(us => us.Id == UserSettings.Id),
                        WOD = newWOD
                    };
                    entities.AddToUser2WODFav(fav);
                    entities.SaveChanges();
                }
                else
                {
                    User2WODFav fav = new User2WODFav()
                    {
                        UserSetting = entities.UserSettings.First(us => us.Id == GroupSettings.Id),
                        WOD = newWOD
                    };
                    entities.AddToUser2WODFav(fav);
                    entities.SaveChanges();
                }
                if (GroupSettings != null)
                {   // if this was a group wod.. we want to bounce back to the scheduling page now...
                    Response.Redirect(ResolveUrl("~") + "group/" + GroupSettings.UserName + "?w=" + newWOD.Id, true);
                    return;
                }
                string baseUrl = ResolveUrl("~");
                RadAjaxManager1.ResponseScripts.Add("Aqufit.Windows.WorkoutSavedDialog.open(" + newWOD.Id + ");");
                //status = "Your Workout has been saved.  It will now show up in your Workout List";                
            }
            catch (Exception ex)
            {
                status = "There was a problem saving your workout: " + ex.Message + " " + ex.InnerException;
            }
            RadAjaxManager1.ResponseScripts.Add(" UpdateStatus('"+status+"');");
            
        }   
                   
        #endregion

        #region Optional Interfaces

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Registers the module actions required for interfacing with the portal framework
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public ModuleActionCollection ModuleActions
        {
            get
            {
                ModuleActionCollection Actions = new ModuleActionCollection();
                Actions.Add(this.GetNextActionID(), Localization.GetString(ModuleActionType.AddContent, this.LocalResourceFile), ModuleActionType.AddContent, "", "", this.EditUrl(), false, SecurityAccessLevel.Edit, true, false);
                return Actions;
            }
        }

        #endregion

    }
}

