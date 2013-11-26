/*
' Affine Technology® - http://www.affinetechnology.com
' Copyright (c) 2009-2010
' by Affine Technology Inc. ( http://www.affinetechnology.com )
'
' Author: Corey Auger
' corey@aqufit.com
' 
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

using Affine.Data;
using Affine.Services.ThirdParty;

namespace Affine.Dnn.Modules.ATI_NikePlus
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
    partial class ViewATI_NikePlus : ATI_PermissionPageBase, IActionable
    {
        #region Private Members
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
                    imgSpinner.Src = ResolveUrl("~/DesktopModules/ATI_Base/resources/images/loading.gif");
                    atiLogin.Visible = true;
                    panelStream.Visible = false;
                    hiddenAccountType.Value = Request["a"];
                    if (!string.IsNullOrEmpty(Request["a"])) // this tells us the account type (garmin/nike+)
                    {
                        if (Request["a"].ToLower() == "nike")
                        {
                            litAccountName.Text = "<h1>Nike+</h1>";
                            litInstructions.Visible = false;
                            imgLogo.Src = ResolveUrl("~/DesktopModules/ATI_Base/resources/images/logoNike.png");
                            atiLogin.UserNameField = "Nike+ Email";
                            atiLogin.PasswordField = "Nike+ Password";
                        }
                        else
                        {
                            atiLogin.UserNameField = "Garmin Username";
                            atiLogin.PasswordField = "Garmin Password";
                            litInstructions.Text = "Enter your Garmin Connect credentials. If you don't have an account, <a href=\"http://connect.garmin.com\">sign up free</a><br /><br />";
                            litAccountName.Text = "<h1>Garmin</h1>";
                            imgLogo.Src = ResolveUrl("~/DesktopModules/ATI_Base/resources/images/logoGarmin.png");
                        }
                        aqufitEntities entities = new aqufitEntities();
                        AccountType accountType = entities.AccountType.FirstOrDefault(at => at.Name == hiddenAccountType.Value);
                        if (accountType != null)
                        {                            
                            ThirdPartyAccounts account = entities.ThirdPartyAccounts.Include("UserSettings").Include("AccountType").FirstOrDefault(a => a.UserSettings.UserKey == this.UserId && a.UserSettings.PortalKey == this.PortalId && a.AccountType.Id == accountType.Id);
                            if (account != null )    // we have already setup an account so just do the sync
                            {
                                panelLogin.Visible = false;
                                litInstructions.Visible = false;
                                if (account.AccountType.Id == (short)Affine.Services.ThirdParty.AccountTypes.NIKE)
                                {
                                    WorkoutConnectorFactory connectorFactory = new WorkoutConnectorFactory(); // TODO: Initialize to an appropriate value
                                    IWorkoutConnector connector = connectorFactory.GetWorkoutConnector(account);
                                    if (connector.Login(account))
                                    {
                                        // now try to get the records
                                        IList<Workout> workoutList = connector.SyncWorkouts();                                        
                                        BindWorkouts(workoutList, account);
                                    }                                    
                                }
                                else if(account.AccountType.Id == (short)Affine.Services.ThirdParty.AccountTypes.GARMIN)
                                {   // unfortunatly garmin fuct us up with the ssl login change .... so here is the new code..
                                    litGarminAccountName.Text = account.AccountUserName;
                                    panelGarmin.Visible = true;
                                    panelLogin.Visible = false;
                                    panelStream.Visible = false;
                                }
                            }                            
                        } // end if (nikeAccountType != null)
                    }  // end if (string.IsNullOrEmpty(Request["a"]))                                    
                }                               
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        private void BindWorkouts(IList<Workout> workouts, ThirdPartyAccounts account)
        {
            panelStream.Visible = true;
            // TODO: could be a heavy load to grab ALL nike+/garmin data and then figure out which ones are different
            aqufitEntities entities = new aqufitEntities();
            IList<long> inSystem = entities.UserStreamSet.OfType<Workout>().Where(w => w.AccoutType == account.AccountType.Id && w.UserSetting.UserKey == this.UserId && w.PortalKey == this.PortalId).Select(w => (long)w.ThirdPartyId).ToList<long>();
            // we only want to pass in a list of data the user had not synced.
            workouts = workouts.Where(np => !inSystem.Contains((long)np.ThirdPartyId)).ToList<Workout>();
            
            atiStream.UserDistanceUnits = Affine.Utils.UnitsUtil.ToUnit(Convert.ToInt32(account.UserSettings.DistanceUnits));
            atiStream.DataSource = workouts.OfType<UserStream>().OrderByDescending(us => us.Date).ToList();
            atiStream.DataBind();

            litStatus.Text = "<h3>We found <strong>(" + workouts.Count + ")</strong> workouts that need importing.</h3>";
        }
       

        protected void bLogin_Click(object sender, EventArgs e)
        {
            lStatus.Text = "There was a problem with your login";
            if (Page.IsValid)
            {
                aqufitEntities entities = new aqufitEntities();                    
                ThirdPartyAccounts account = null;
                string atypeString = hiddenAccountType.Value;
                if (string.IsNullOrEmpty(atypeString))
                {
                    atypeString = Affine.Utils.Web.WebUtils.IsEmail(atiLogin.UserName) ? "Nike" : "Garmin";   // email login is "Normally" Nike+                   
                }
                AccountType accountType = entities.AccountType.FirstOrDefault(at => at.Name == atypeString);
                UserSettings settings = entities.UserSettings.FirstOrDefault(us => us.UserKey == this.UserId && us.PortalKey == this.PortalId);
                // first thing to do is to check that it is a valid nike+ account
                account = new ThirdPartyAccounts()
                    {
                        AccountUserName = atiLogin.UserName,
                        AccountPassword = atiLogin.Password,
                        AccountType = accountType,
                        UserSettings = settings
                    };                    
                
                WorkoutConnectorFactory connectorFactory = new WorkoutConnectorFactory(); // TODO: Initialize to an appropriate value
                IWorkoutConnector connector = connectorFactory.GetWorkoutConnector(account);

                if (atypeString == "Nike" && connector.Login(account))
                {                    
                    bSaveAndClose.Visible = true;
                    entities.AddToThirdPartyAccounts(account);
                    entities.SaveChanges();

                    // now try to get the records
                    IList<Workout> workoutList = connector.SyncWorkouts();
                    BindWorkouts(workoutList, account);
                    panelLogin.Visible = false;
                    lStatus.Text = "";
                }
                else if (atypeString == "Garmin")
                {
                    // Garmin we just save ...
                    entities.AddToThirdPartyAccounts(account);
                    entities.SaveChanges();
                    lStatus.Text = "";
                    litGarminAccountName.Text = atiLogin.UserName;
                    panelGarmin.Visible = true;
                    panelLogin.Visible = false;
                }
            }
        }

        protected void bChangeAccount_Click(object sender, EventArgs e)
        {
            panelLogin.Visible = true;
            panelStream.Visible = false;
        }

        protected void bSaveAndClose_Click(object sender, EventArgs e)
        {
            // TODO: a try catch.. for failure to import

            // TODO: security check ... 
            // TODO: need to keep running totals for the user.
            // here we  add the nike+ data to the users workouts
            IList<Workout> workouts = atiStream.DataSource.Cast<Workout>().ToList<Workout>();

            // Now we know "workouts" that the user wants to import.  So we need to try to get the extended data for each workout.
          
            aqufitEntities entities = new aqufitEntities();
            AccountType accountType = entities.AccountType.FirstOrDefault(at => at.Name == hiddenAccountType.Value);
            ThirdPartyAccounts account = entities.ThirdPartyAccounts.Include("UserSettings").Include("AccountType").FirstOrDefault(a => a.UserSettings.UserKey == this.UserId && a.UserSettings.PortalKey == this.PortalId && a.AccountType.Id == accountType.Id);
            WorkoutConnectorFactory connectorFactory = new WorkoutConnectorFactory(); // TODO: Initialize to an appropriate value
            IWorkoutConnector connector = connectorFactory.GetWorkoutConnector(account);                
            if (!connector.Login(account))
            {
                // TODO: error handle
                throw new Exception("Error: login third party data sync failed.");
            }

            workouts = workouts.OrderByDescending(w => w.Date).ToList();     // we want to insert from past to present

            // make some vaiables for totals
            double totalDistance = 0.0;
            long totalTime = 0;
            // So here is the deal with the next bit of code.  If someone syncs more runs then "3"? We don't want to flood the
            // friends streams with all those runs.  So we sync all the runs to the owner steam and NOT friends (publish setting 1)
            // Then we sync a notive to ONLY the friends stream that a bunch of runs were synced (publish setting 2)
            double absTotalDist = 0.0;
            double absTotalCal = 0.0;
            long absTotalTime = 0;
            UserSettings settings = entities.UserSettings.FirstOrDefault(us => us.UserKey == this.UserId && us.PortalKey == this.PortalId);
            Affine.Data.Managers.IDataManager dataManager = Affine.Data.Managers.LINQ.DataManager.Instance;
            if (workouts.Count > 3)
            {
                foreach (Workout w in workouts)
                {
                    totalDistance += Convert.ToDouble(w.Distance);
                    absTotalDist += totalDistance;
                    totalTime += Convert.ToInt64(w.Duration);
                    absTotalTime += totalTime;
                    absTotalCal += Convert.ToDouble(w.Calories);
                    w.PublishSettings = 1; // TODO: no magic number (1) is to publish for yourself but NOT your friends
                    w.Id = 0;       // make sure our third party id does not show up in the id
                    w.UserSetting = settings;
                    WorkoutExtended ext = new WorkoutExtended();
                    w.WorkoutExtendeds.Add(ext);                    
                   // entities.AddToUserStreamSet(w);
                    
                    try
                    {
                        IList<WorkoutSample> samples = connector.SyncDeviceSamplesForWorkout(ext);
                        samples.ToList().ForEach(s => entities.AddToWorkoutSamples(s));
                    }
                    catch (NoExtendedDataException)
                    {
                        // change the workout src so we dont expect extended data with this one.
                        w.DataSrc = (int)Utils.WorkoutUtil.DataSrc.MANUAL_NO_MAP;
                    }
                    dataManager.SaveWorkout(entities, w);
                    // We got all the data that we require so save the workout    
                }
                
                Utils.UnitsUtil.MeasureUnit distUnit = Affine.Utils.UnitsUtil.ToUnit(settings.DistanceUnits.Value);
                TimeSpan duration = new TimeSpan(0, 0, 0, 0, Convert.ToInt32(totalTime));
                TimeSpan pace = new TimeSpan(0, 0, 0, 0, totalDistance > 0 ? Convert.ToInt32(totalTime / totalDistance) : 0);
                Notification workoutNotification = new Notification()
                {
                    PortalKey = this.PortalId,
                    UserSetting = settings,
                    Date = DateTime.Now.ToUniversalTime(),
                    Title = "Notification <em>"+account.AccountType.Name+" Sync</em>",                    
                    Description = "Nike+ sync of " + workouts.Count + " workouts.<br />Total Distance: "
                                                + String.Format("{0:0.0}", Affine.Utils.UnitsUtil.systemDefaultToUnits(totalDistance, distUnit)) + " " + Affine.Utils.UnitsUtil.unitToStringName(distUnit) + "<br />"
                                                + "Total Time: " + String.Format("{0:00}", duration.Hours) + ":" + String.Format("{0:00}", duration.Minutes) + ":" + String.Format("{0:00}", duration.Seconds) + "<br />"
                                                + "Avg. Pace: " + String.Format("{0:0}", pace.Minutes) + ":" + String.Format("{0:00}", pace.Seconds),
                    TimeStamp = DateTime.Now.ToUniversalTime(),
                    NotificationType = (int)Affine.Utils.ConstsUtil.NotificationTypes.NIKE_PLUS, 
                    PublishSettings = (int)Affine.Utils.ConstsUtil.PublishSettings.FRIEND_ONLY
                };
                entities.AddToUserStreamSet(workoutNotification);
                // Adjust the totals now                    
                /*
                WorkoutTotal totals = entities.WorkoutTotals.FirstOrDefault(wt => wt.UserKey == this.UserId && wt.PortalKey == this.PortalId && wt.WorkoutTypeKey == 0);   // When workout type key == 0 This is the "Entire" totals for all workout types
                if (totals == null)
                {
                    totals = new WorkoutTotal();
                    totals.PortalKey = this.PortalId;
                    totals.UserKey = this.UserId;
                    entities.AddToWorkoutTotals(totals);
                }
                totals.Distance += absTotalDist;
                totals.Calories += absTotalCal;
                totals.Time += absTotalTime;
                totals.Count+=workouts.Count;
                entities.SaveChanges();
                 */
            }
            else  // just add the 3 or less runs normaly 
            {
                foreach (Workout w in workouts)
                {
                    totalDistance += Convert.ToDouble(w.Distance);
                    absTotalDist += totalDistance;
                    totalTime += Convert.ToInt64(w.Duration);
                    absTotalTime += totalTime;
                    absTotalCal += Convert.ToDouble(w.Calories);
                    w.Id = 0;       // make sure our third party id does not show up in the id
                    w.UserSetting = settings;
                    WorkoutExtended ext = new WorkoutExtended();
                    w.WorkoutExtendeds.Add(ext);
                   // entities.AddToUserStreamSet(w);
                    try
                    {
                        IList<WorkoutSample> samples = connector.SyncDeviceSamplesForWorkout(ext);
                        samples.ToList().ForEach(s => entities.AddToWorkoutSamples(s));
                    }
                    catch (NoExtendedDataException)
                    {
                        // TODO: log this... change the workout src so we dont expect extended data with this one.
                        w.DataSrc = (int)Utils.WorkoutUtil.DataSrc.MANUAL_NO_MAP;
                    }
                    dataManager.SaveWorkout(entities, w);
                }                
            }
            RadAjaxManager1.ResponseScripts.Add("self.close();  top.location.href = '" + ResolveUrl("~") + UserSettings.UserName + "'; ");
        }

        protected void bPostBack_Click(object sender, EventArgs e)
        {
         
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

