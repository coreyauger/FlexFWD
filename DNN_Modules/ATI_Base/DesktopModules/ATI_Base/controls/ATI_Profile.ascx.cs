using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

using Affine.Data;

public partial class DesktopModules_ATI_Base_controls_ATI_Profile : DotNetNuke.Framework.UserControlBase
{
    public UserSettings ProfileSettings { get; set; }
    private bool _IsOwner = false;
    public bool IsOwner { get { return _IsOwner; } set { _IsOwner = value; } }

    private bool _IsFriend = false;
    public bool IsFriend { get { return _IsFriend; } set { _IsFriend = value; } }

    private bool _IsFollowing = false;
    public bool IsFollowing { get { return _IsFollowing; } set { _IsFollowing = value; } }

    private bool _IsSmall = false;
    public bool IsSmall { get { return _IsSmall; } set { _IsSmall = value; } }

    private bool _ShowCompareButton = true;
    public bool ShowCompareButton { get { return _ShowCompareButton; } set { _ShowCompareButton = value; } }

    public Group MainGroup { get; set; }


    private Affine.Utils.ConstsUtil.ProfileMode _ProfileMode = Affine.Utils.ConstsUtil.ProfileMode.NORMAL;
    public Affine.Utils.ConstsUtil.ProfileMode Mode { get { return _ProfileMode; } set { _ProfileMode = value; } }

    private string urlBase;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack && !Page.IsCallback && this.ProfileSettings != null)
        {
            atiProfileImage.Settings = ProfileSettings;
            atiProfileImage.Small = this.IsSmall;
            urlBase = ResolveUrl("~/");
            

            hlUserStats.HRef =  urlBase+ ProfileSettings.UserName + "/achievements";
            divAddTo.Visible = true;
            atiProfileLinks.ProfileSettings = this.ProfileSettings;
            atiProfileLinks.IsOwner = this.IsOwner;
            atiProfileLinks.IsFriend = this.IsFriend;
            atiProfileLinks.Mode = this.Mode;
            atiProfileSuggest.ProfileSettings = this.ProfileSettings;
            if (this.IsOwner)
            {
                bCompareTo.Visible = false;
                atiProfileImage.IsOwner = true;
                atiWebLinksList.IsOwner = true;
                divAddTo.Visible = false;
                atiFriendsPhotos.FriendListLink = ResolveUrl("~/" + ProfileSettings.UserName + "/Friends");
            }
            else
            {
                atiProfileImage.IsOwner = false;
                atiFriendsPhotos.FriendListLink = "javascript: Aqufit.Windows.WatchList.open();";
                aCompareTo.HRef = urlBase + "compare/" + ProfileSettings.UserName;
                bCompareTo.Visible = ShowCompareButton;
            }
            litAchievements.Text = "Achievements";

            // TODO: take this out of the control.. ( prolly put it in the pagebase .. have a setup )
            aqufitEntities entities = new aqufitEntities();
            if (this.Mode == Affine.Utils.ConstsUtil.ProfileMode.NORMAL)
            {
                
                long[] friendIds = null;
                if (this.MainGroup != null)
                {
                    litMainGroup.Text = "<a style=\"display: block; padding-top: 10px; font-weight:bold; font-size: 13px;\" href=\"/group/" + this.MainGroup.UserName + "\">" + this.MainGroup.UserName + "</a>";
                }
                else
                {
                    litMainGroup.Visible = false;
                }
                // settup the users web links                    
                atiWebLinksList.ProfileSettings = ProfileSettings;

                if (this.ProfileSettings is Group)
                {
                    bCompareTo.Visible = false;
                    litMainGroup.Visible = false;
                    divAddTo.Visible = false;
                    hlUserStats.HRef = ResolveUrl("~/") + ProfileSettings.UserName + "/achievements";
                    lAthleteTerm.Text = "<span>Members</span>";
                    atiFriendsPhotos.FriendTerm = "Member";
                    atiFriendsPhotos.FriendTermPlural = "Members";
                    atiFriendsPhotos.FriendListLink = "javascript: Aqufit.Windows.WatchList.open();";
                    // TODO: cache this grabbing of the friend ids from the stream service
                    friendIds = entities.UserFriends.Where(f => (f.SrcUserSettingKey == ProfileSettings.Id || f.DestUserSettingKey == ProfileSettings.Id) && (f.Relationship >= (int)Affine.Utils.ConstsUtil.Relationships.GROUP_OWNER)).Select(f => (f.SrcUserSettingKey == this.ProfileSettings.Id ? f.DestUserSettingKey : f.SrcUserSettingKey)).ToArray();

                    // TODO: we only want the best times / scores ect..  ( Take this out of the control as well )
                    WODSchedule lastWOD = entities.WODSchedules.Include("WOD").Include("WOD.WODType").Where(w => w.Date.CompareTo(DateTime.Now) < 0).OrderByDescending(w => w.Date).FirstOrDefault();
                    if (lastWOD != null)
                    {
                        litAchievements.Text = lastWOD.WOD.Name;
                        IQueryable<Workout> crossfitWorkouts = entities.UserStreamSet.Include("UserSetting").Include("WOD").OfType<Workout>().Where(w => w.WOD.Id == lastWOD.WOD.Id);
                        int numDistinct = crossfitWorkouts.Count();
                        const int MAX_DISPLAY = 15;
                        IQueryable<Workout> wodsToDisplay = null;
                        if (numDistinct > MAX_DISPLAY)
                        {
                            Random rand = new Random((int)DateTime.Now.Millisecond);
                            int skip = rand.Next(numDistinct - MAX_DISPLAY);
                            wodsToDisplay = crossfitWorkouts.OrderByDescending(w => w.Id).Skip(skip).Take(MAX_DISPLAY);
                        }
                        else
                        {
                            wodsToDisplay = crossfitWorkouts.OrderByDescending(w => w.Id).Take(MAX_DISPLAY);
                        }
                        Workout[] workoutArray = wodsToDisplay.ToArray();
                        IList<DesktopModules_ATI_Base_controls_ATI_NameValueGrid.TotalItem> cfTotals = new List<DesktopModules_ATI_Base_controls_ATI_NameValueGrid.TotalItem>();
                        // links are like "workout/187"
                        string baseUrl = ResolveUrl("~");
                        if (lastWOD.WOD.WODType.Id == (int)Affine.Utils.WorkoutUtil.WodType.TIMED)
                        {
                            cfTotals = workoutArray.Select(w => new DesktopModules_ATI_Base_controls_ATI_NameValueGrid.TotalItem() { Name = w.UserSetting.UserName, Total = Affine.Utils.UnitsUtil.durationToTimeString(Convert.ToInt64(w.Duration)), Link = baseUrl + w.UserSetting.UserName }).ToList();
                        }
                        else if (lastWOD.WOD.WODType.Id == (int)Affine.Utils.WorkoutUtil.WodType.SCORE || lastWOD.WOD.WODType.Id == (int)Affine.Utils.WorkoutUtil.WodType.AMRAP)
                        {
                            cfTotals = workoutArray.Select(w => new DesktopModules_ATI_Base_controls_ATI_NameValueGrid.TotalItem() { Name = w.UserSetting.UserName, Total = Convert.ToString(w.Score), Link = baseUrl + w.UserSetting.UserName }).ToList();
                        }
                        else if (lastWOD.WOD.WODType.Id == (int)Affine.Utils.WorkoutUtil.WodType.MAX_WEIGHT)
                        {
                            cfTotals = workoutArray.Select(w => new DesktopModules_ATI_Base_controls_ATI_NameValueGrid.TotalItem() { Name = w.UserSetting.UserName, Total = "" + Affine.Utils.UnitsUtil.systemDefaultToUnits(w.Max.Value, Affine.Utils.UnitsUtil.MeasureUnit.UNIT_LBS), Link = baseUrl + w.UserSetting.UserName }).ToList();
                        }
                        // TODO: we need the link (?? are we going to goto stats page.. or to the workout stream view)
                        nvgCrossfit.Totals = cfTotals;
                    }
                }
                else
                {
                    string baseUrl = ResolveUrl("~") + "workout/";
                    // TODO: cache this grabbing of the friend ids from the stream service
                    friendIds = entities.UserFriends.Where(f => (f.SrcUserSettingKey == ProfileSettings.Id || f.DestUserSettingKey == ProfileSettings.Id) && (f.Relationship == (int)Affine.Utils.ConstsUtil.Relationships.FRIEND)).Select(f => (f.SrcUserSettingKey == this.ProfileSettings.Id ? f.DestUserSettingKey : f.SrcUserSettingKey)).ToArray();
                    lAthleteTerm.Text = "<span>Athletes</span>";

                    // TODO: we only want the best times / scores ect..  ( Take this out of the control as well )
                    IQueryable<Workout> crossfitWorkouts = entities.UserStreamSet.OfType<Workout>().Include("WOD").Where(w => w.UserSetting.Id == ProfileSettings.Id && w.WorkoutTypeKey == (int)Affine.Utils.WorkoutUtil.WorkoutType.CROSSFIT && w.IsBest == true);
                    int numDistinct = crossfitWorkouts.Select(w => w.WOD).Count();
                    const int MAX_DISPLAY = 10;
                    IQueryable<Workout> wodsToDisplay = null;
                    if (numDistinct > MAX_DISPLAY)
                    {
                        Random rand = new Random((int)DateTime.Now.Millisecond);
                        int skip = rand.Next(numDistinct - MAX_DISPLAY);
                        wodsToDisplay = crossfitWorkouts.OrderByDescending(w => w.Id).Skip(skip).Take(MAX_DISPLAY);
                    }
                    else
                    {
                        wodsToDisplay = crossfitWorkouts.OrderByDescending(w => w.Id).Take(MAX_DISPLAY);
                    }
                    // We need to split up into WOD types now...
                    Workout[] timedWods = wodsToDisplay.Where(w => w.WOD.WODType.Id == (int)Affine.Utils.WorkoutUtil.WodType.TIMED).ToArray();
                    IList<DesktopModules_ATI_Base_controls_ATI_NameValueGrid.TotalItem> cfTotals = timedWods.Select(w => new DesktopModules_ATI_Base_controls_ATI_NameValueGrid.TotalItem() { Name = w.Title, Total = Affine.Utils.UnitsUtil.durationToTimeString(Convert.ToInt64(w.Duration)), Link = baseUrl + w.WOD.Id }).ToList();

                    // Now all the scored ones...
                    Workout[] scoredWods = wodsToDisplay.Where(w => w.WOD.WODType.Id == (int)Affine.Utils.WorkoutUtil.WodType.SCORE).ToArray();
                    cfTotals = cfTotals.Concat(scoredWods.Select(w => new DesktopModules_ATI_Base_controls_ATI_NameValueGrid.TotalItem() { Name = w.Title, Total = Convert.ToString(w.Score), Link = baseUrl + w.WOD.Id }).ToList()).ToList();

                    // Now all the max ones...
                    Workout[] maxWods = wodsToDisplay.Where(w => w.WOD.WODType.Id == (int)Affine.Utils.WorkoutUtil.WodType.MAX_WEIGHT).ToArray();
                    cfTotals = cfTotals.Concat(maxWods.Select(w => new DesktopModules_ATI_Base_controls_ATI_NameValueGrid.TotalItem() { Name = w.Title, Total = Affine.Utils.UnitsUtil.systemDefaultToUnits(w.Max.Value, Affine.Utils.UnitsUtil.MeasureUnit.UNIT_LBS) + " " + Affine.Utils.UnitsUtil.unitToStringName(Affine.Utils.UnitsUtil.MeasureUnit.UNIT_LBS), Link = baseUrl + w.WOD.Id }).ToList()).ToList();

                    // TODO: we need the link (?? are we going to goto stats page.. or to the workout stream view)
                    nvgCrossfit.Totals = cfTotals;
                }
                IQueryable<Affine.Data.User> friends = entities.UserSettings.OfType<User>().Where(Affine.Utils.Linq.LinqUtils.BuildContainsExpression<User, long>(s => s.Id, friendIds)).OrderBy(s => s.Id);
                int fcount = friends.Count();
                UserSettings[] firendSettings = null;
                if (fcount > 6)
                {
                    Random rand = new Random((int)DateTime.Now.Millisecond);
                    int skip = rand.Next(fcount - 6);
                    firendSettings = friends.Skip(skip).Take(6).ToArray();
                }
                else
                {
                    firendSettings = friends.Take(6).ToArray();
                }
                atiFriendsPhotos.FriendKeyList = firendSettings;
                atiFriendsPhotos.User = ProfileSettings;
                atiFriendsPhotos.FriendCount = fcount;
                //  atiSendMessage.UserSettings = firendSettings;

                
            }
            else if (this.Mode == Affine.Utils.ConstsUtil.ProfileMode.BIO)
            {
                pAchievements.Visible = false;
                pWebLinks.Visible = false;
                pFriends.Visible = false;
                bCompareTo.Visible = false;              
                if (this.IsOwner)
                {
                    pAchievements.Visible = false;
                }
                pBodyComp.Visible = true;
                pTrainingHistory.Visible = true;
                BodyComposition bc = entities.BodyComposition.FirstOrDefault(b => b.UserSetting.Id == ProfileSettings.Id);
                pBio.Visible = true;
                litBio.Text = "<p>No Info</p>";
                litTrainingHistory.Text = "<p>No Info</p>";
                string height = "Unknown";
                string weight = "Unknown";
                if (bc != null)
                {
                    litBio.Text = "<p>" + bc.Bio + "</p>";
                    litTrainingHistory.Text = "<p>" + bc.Description + "</p>";
                    if (bc.Height.HasValue)
                    {
                        double inches = Affine.Utils.UnitsUtil.systemDefaultToUnits(bc.Height.Value, Affine.Utils.UnitsUtil.MeasureUnit.UNIT_INCHES);
                        double feet = Math.Floor( inches / 12 );
                        inches =  Math.Floor(inches % 12);
                        height = feet + " feet " + inches + " inches";
                    }
                    if (bc.Weight.HasValue)
                    {
                        double lbs = Affine.Utils.UnitsUtil.systemDefaultToUnits(bc.Weight.Value, Affine.Utils.UnitsUtil.MeasureUnit.UNIT_LBS);
                        weight = lbs + " " + Affine.Utils.UnitsUtil.unitToStringName(Affine.Utils.UnitsUtil.MeasureUnit.UNIT_LBS);
                    }
                }
                IList<DesktopModules_ATI_Base_controls_ATI_NameValueGrid.TotalItem> cfHeightWeight = new List<DesktopModules_ATI_Base_controls_ATI_NameValueGrid.TotalItem>();                                
                cfHeightWeight.Add( new DesktopModules_ATI_Base_controls_ATI_NameValueGrid.TotalItem(){ Name="Height", Total=height});
                cfHeightWeight.Add(new DesktopModules_ATI_Base_controls_ATI_NameValueGrid.TotalItem() { Name = "Weight", Total = weight });
                nvgBodyComp.Totals = cfHeightWeight;
            }


            if (this.IsFriend) // You are viewing a friends profile... so should the "Send Message Button";
            {
                divAddTo.Visible = false;
            }
            else
            {
                if (this.IsFollowing)
                {   // We need to change the "following link" to a smaller "Send Friend Request"
                    bAddTo.Visible = false;
                    hlSendFriendRequest.Visible = true;
                }
            }            

        }
    }
}
