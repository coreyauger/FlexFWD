using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Security.Roles;
using DotNetNuke.Entities.Users;
using DotNetNuke.UI.Skins.Controls;

using Affine.Data;
using Telerik.Web.UI;

namespace Affine.Web.Controls
{

    public partial class ATI_SearchSkinObject : DotNetNuke.UI.Skins.SkinObjectBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack && !Page.IsCallback)
            {
                if (PortalSettings.UserId <= 0)
                {   // if no one is logged in then remove the searh
                    atiRadComboBoxSearch.Visible = false;
                }
            }
        }
        // this is not in the StreamService (script service) for speed..
        /*
        protected void atiRadComboBoxSearch_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.Text))
            {
                int holes = 0;
                RadComboBox atiRadComboSearchFriends = (RadComboBox)sender;
                atiRadComboSearchFriends.Items.Clear();
                const int TAKE = 10;
                aqufitEntities entities = new aqufitEntities();
                
                // ** Get all the friends for a search first..
                UserSettings profileSettings = entities.UserSettings.OfType<User>().FirstOrDefault(u => u.UserKey == PortalSettings.UserId && u.PortalKey == PortalSettings.PortalId);
                //long[] friendIds = entities.UserFriends.Where(f => (f.SrcUserSettingKey == profileSettings.Id || f.DestUserSettingKey == profileSettings.Id) && f.Relationship == (int)Affine.Utils.ConstsUtil.Relationships.FRIEND).Select(f => f.SrcUserSettingKey == profileSettings.Id ? f.DestUserSettingKey : f.SrcUserSettingKey).ToArray();
                int itemOffset = e.NumberOfItems;
                IQueryable<User> friends = entities.UserSettings.OfType<User>().Where( u => u.PortalKey == this.PortalSettings.PortalId ).OrderBy(w => w.UserName);                
                friends = friends.Where(w => w.UserName.ToLower().StartsWith(e.Text) || w.UserFirstName.ToLower().StartsWith(e.Text) || w.UserLastName.ToLower().StartsWith(e.Text));
                int length = friends.Count();
                User[] users = friends.Skip(itemOffset).Take(TAKE).ToArray();
                foreach (User u in users)
                {
                    RadComboBoxItem item = new RadComboBoxItem(u.UserName + " (" + u.UserFirstName + " " + u.UserLastName + ")");
                    item.Value = "{ Type:'USER', Val:'" + u.UserName + "'}";
                    item.ImageUrl = ResolveUrl("~/DesktopModules/ATI_Base/services/images/profile.aspx") + "?u=" + u.UserKey + "&p=" + u.PortalKey;
                    atiRadComboSearchFriends.Items.Add(item);
                    holes++;
                }

                // ** Next get all the groups that the user is in..                
                long[] groupIds = entities.UserFriends.Where(f => (f.SrcUserSettingKey == profileSettings.Id || f.DestUserSettingKey == profileSettings.Id) && f.Relationship >= (int)Affine.Utils.ConstsUtil.Relationships.GROUP_OWNER).Select(f => (f.SrcUserSettingKey == profileSettings.Id ? f.DestUserSettingKey : f.SrcUserSettingKey)).ToArray();
                IQueryable<Affine.Data.Group> groups = entities.UserSettings.OfType<Group>().Where(Affine.Utils.Linq.LinqUtils.BuildContainsExpression<Group, long>(s => s.Id, groupIds)).OrderBy(s => s.Id);                
                int gTake = TAKE - holes;
                if (gTake < 0)
                {
                    gTake = 0;
                }
                groups = groups.Where(w => w.UserName.ToLower().StartsWith(e.Text) || w.UserFirstName.ToLower().StartsWith(e.Text) || w.UserLastName.ToLower().StartsWith(e.Text));
                length += groups.Count();
                Group[] groupArray = groups.Skip(itemOffset).Take(gTake).ToArray();
                foreach (Group g in groupArray)
                {
                    RadComboBoxItem item = new RadComboBoxItem(g.UserName);
                    item.Value = "{ Type:'GROUP', Val:'" + g.UserName + "'}";
                    item.ImageUrl = ResolveUrl("~/DesktopModules/ATI_Base/services/images/profile.aspx") + "?u=" + g.UserKey + "&p=" + g.PortalKey;
                    atiRadComboSearchFriends.Items.Add(item);
                    holes++;
                }                

                // ** Next get all the Workouts by name for the user..
                int wTake = TAKE - holes;
                if (wTake < 0)
                {
                    wTake = 0;
                }
                IQueryable<Workout> workouts = entities.UserStreamSet.OfType<Workout>().Include("WOD").Where(w => w.UserSetting.Id == profileSettings.Id && w.Title.ToLower().StartsWith(e.Text)).OrderBy(w => w.Date);
                length += workouts.Count();
                Workout[] workoutArray = workouts.Skip(itemOffset).Take(wTake).ToArray();
                if (Cache["WorkoutTypeList"] == null)
                {
                    Cache["WorkoutTypeList"] = entities.WorkoutType.Select(wt => wt).ToList<WorkoutType>();
                }
                // Create a check list of all the workout types
                IList<WorkoutType> wtList = (IList<WorkoutType>)Cache["WorkoutTypeList"];
                foreach (Workout w in workoutArray)
                {
                    RadComboBoxItem item = new RadComboBoxItem(w.Title);
                    long wodId = w.WOD != null ? w.WOD.Id : -1;
                    item.Value = "{ Type:'WORKOUT', WType:'" + w.WorkoutTypeKey + "', Src:'" + w.DataSrc + "', User:'" + profileSettings.UserName + "', Val:'" + w.Id + "', WODKey:'" + wodId + "'}";
                    item.ImageUrl = ResolveUrl(wtList.Where(wt => wt.Id == w.WorkoutTypeKey).Select(wt => wt.Icon).First());
                    atiRadComboSearchFriends.Items.Add(item);
                    holes++;
                } 


                int endOffset = Math.Min(itemOffset + TAKE, length);
                e.EndOfItems = endOffset == length;
                e.Message = (length <= 0) ? "No matches" : String.Format("Items <b>1</b>-<b>{0}</b> of {1}", endOffset, length);
            }
        }
         */
    }
         
}
