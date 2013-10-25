using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Serialization;
using System.Xml.Linq;

using Affine.Data;
using Affine.Utils.Linq;

using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Users;
using DotNetNuke.Entities.Tabs;

using Telerik.Web.UI;

namespace Affine.WebService
{
    /// <summary>
    /// Summary description for StreamService
    /// </summary>
    [WebService(Namespace = "http://aqufit.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class StreamService : System.Web.Services.WebService
    {
        private Affine.Data.Managers.IStreamManager _IStreamManager = Affine.Data.Managers.LINQ.StreamManager.Instance;
        private JavaScriptSerializer _jserializer = new JavaScriptSerializer();

        public StreamService()
        {
            //Uncomment the following line if using designed components 
            //InitializeComponent(); 
        }

        private int TakeGaurd(int take, int gaurd = 25)
        {
            return take > gaurd ? gaurd : take;
            //return 5;
        }

        /// <summary>
        /// Helper method
        /// </summary>
        /// <param name="us"></param>
        /// <param name="settings"></param>
        /// <param name="comment"></param>
        private void sendStreamEmailAsync(UserStream us, UserSettings settings, UserComment comment)
        {
            string emailBody = settings.UserName + " commented.\n\n";
            emailBody += "\"" + Affine.Utils.Web.WebUtils.FromWebSafeString( comment.Text ) + "\"\n\n";
            emailBody += "To see the comment thread, follow the link below:\n";

            string ts = Convert.ToString(DateTime.Now.Ticks);
            if (us is Recipe)
            {
                emailBody += "http://" + HttpContext.Current.Request.Url.Host.Replace("www.","") + System.Web.VirtualPathUtility.ToAbsolute("~/Search.aspx") + "?r="+us.Id + "&ts="+ts;
            }
            else
            {
                emailBody += "http://" + HttpContext.Current.Request.Url.Host.Replace("www.", "") + "/" + us.UserSetting.UserName + "?s=" + us.Id + "&ts=" + ts;
            }
            string subject = settings.UserName + " commented on your post...";

            Affine.Utils.GmailUtil gmail = new Utils.GmailUtil();
            // Send an email to the owner of the post
            if (us.UserSetting.Id != settings.Id)
            {
                gmail.Send(us.UserSetting.UserEmail, subject, emailBody);
            }
            
            aqufitEntities entities = new aqufitEntities();
            IQueryable<UserComment> comments = entities.UserComment.Include("UserSetting").Where(uc => uc.UserStream.Id == us.Id);
            string[] emails = comments.Select(uc => uc.UserSetting.UserEmail).Distinct().ToArray();
            foreach (string email in emails)
            {
                if (email != us.UserSetting.UserEmail && email != settings.UserEmail)
                {
                    gmail.Send(email, settings.UserName + " commented.", emailBody);
                }
            }
        }
        private void sendReplyEmailAsync(Message m, UserSettings settings)
        {
            string emailBody = settings.UserName + " Replyed to your message\n\n";
            emailBody += "\"" + Affine.Utils.Web.WebUtils.FromWebSafeString( m.LastText ) + "\"\n\n";
            emailBody += "To see the message thread, follow the link below:\n";

            string ts = Convert.ToString(DateTime.Now.Ticks);
            long mId = m.ParentKey > 0 ? m.ParentKey : m.Id;
            emailBody += "http://" + HttpContext.Current.Request.Url.Host + System.Web.VirtualPathUtility.ToAbsolute("~/Profile/Inbox") + "?m=" + mId + "&ts="+ts;

            string subject = settings.UserName + " Replyed to your message...";
            
            Affine.Utils.GmailUtil gmail = new Utils.GmailUtil();
            long[] idArray = m.MessageRecipiants.Select(mr => mr.UserSettingsKey).ToArray();

            aqufitEntities entities = new aqufitEntities();
            string[] emails = entities.UserSettings.Where(us => us.PortalKey == settings.PortalKey).Where(Affine.Utils.Linq.LinqUtils.BuildContainsExpression<UserSettings, long>(us => us.UserKey, idArray)).Select(us => us.UserEmail).Distinct().ToArray();
            foreach (string email in emails)
            {
                if (email != settings.UserEmail)
                {
                    gmail.Send(email, subject, emailBody);
                }
            }
        }
        
        
        private void sendUserFollowEmailAsync(UserSettings src, UserSettings dst)
        {
            string emailBody = "Friend Request: " + src.UserName + " Accepted\n\n";
            emailBody += "\"" + src.UserFirstName + " " + src.UserLastName + " has accepted your friend request.\"\n\n";            
            // Send an email to the owner of the post

            Affine.Utils.GmailUtil gmail = new Utils.GmailUtil();
            gmail.Send(dst.UserEmail, "Friend Request: " + src.UserName + " accepted", emailBody);
        }
        private void sendContactEmailAsync(UserSettings src, string email, string txt)
        {
            //From: host@aqufit.com
            //Subject: Host has invited you to join Aqufit
            //Hi,

            //Come join in... it's free!
            //http://dev.aqufit.com/www.aqufit.com/Home/Register

            //Check out my profile:
            //http://dev.aqufit.com/www.aqufit.com/host
            
            string emailBody = "Hi,\n" + txt + "\n\n";
            string subject = string.Empty;
            if (src is User)
            {
                emailBody += "Come join in... it's free!\n";
                emailBody += "http://" + HttpContext.Current.Request.Url.Host + System.Web.VirtualPathUtility.ToAbsolute("~/") + "Home/Register\n\n";
                emailBody += "Check out my profile:\n";
                emailBody += "http://" + HttpContext.Current.Request.Url.Host + System.Web.VirtualPathUtility.ToAbsolute("~/") + src.UserName + "\n\n";
                subject = src.UserFirstName + " " + src.UserLastName + " (" + src.UserName + ") has invited you to join FlexFWD";
            }
            else
            {   // group invite
                emailBody += "Come join in... it's free!\n";
                emailBody += "http://" + HttpContext.Current.Request.Url.Host + System.Web.VirtualPathUtility.ToAbsolute("~/") + "Home/Register?g="+src.Id+"&ts="+DateTime.Now.Ticks+"\n\n";
                emailBody += "Check out the group page:\n";
                emailBody += "http://" + HttpContext.Current.Request.Url.Host + System.Web.VirtualPathUtility.ToAbsolute("~/") + "group/"+ src.UserName + "\n\n";
                subject = src.UserFirstName + " has invited you to join their group on FlexFWD";
            }
            // Send an email to the owner of the post

            Affine.Utils.GmailUtil gmail = new Utils.GmailUtil();
            gmail.Send(email, subject, emailBody);
             
        }
        private void sendEmailAsync(Message message)
        {
            string ts = Convert.ToString(DateTime.Now.Ticks);
            string emailBody = string.Empty;
            emailBody += "Subject: \"" + Affine.Utils.Web.WebUtils.FromWebSafeString(message.Subject) + "\"\n\n";
            emailBody += Affine.Utils.Web.WebUtils.FromWebSafeString(message.Text) + "\n\n";
            emailBody += "To see reply to the message, follow the link below:\n";
            emailBody += "http://" + HttpContext.Current.Request.Url.Host + System.Web.VirtualPathUtility.ToAbsolute("~/") +"Profile/Inbox?m=" + message.Id+"&ts="+ts;

            aqufitEntities entities = new aqufitEntities();
            UserSettings sender = entities.UserSettings.FirstOrDefault(us => us.Id == message.UserSetting.Id);
            Affine.Utils.GmailUtil email = new Utils.GmailUtil();
            foreach (MessageRecipiant r in message.MessageRecipiants)
            {
                if (message.UserSetting.Id != r.UserSettingsKey)
                {
                    // TODO: cache
                    UserSettings settings = entities.UserSettings.FirstOrDefault(us => us.Id == r.UserSettingsKey );
                    if (settings != null)
                    {
                        email.Send(settings.UserEmail, "New message from " + sender.UserName, "New message from " + sender.UserName + "\n\n" + emailBody);
                    }
                }
            }
        }              

        public string getWorkout(long wid)
        {
            aqufitEntities entities = new aqufitEntities();
            Workout workout = entities.UserStreamSet.Include("WorkoutExtendeds").Include("WorkoutExtendeds.MapRoute").Include("UserSetting").OfType<Workout>().FirstOrDefault(w => w.Id == wid);
            if (workout != null)
            {
                Affine.Data.json.Workout json = new Affine.Data.json.Workout()
                {
                    Calories = workout.Calories,
                    Date = workout.Date.ToLocalTime().ToShortDateString(),
                    Description = workout.Description,
                    Distance = workout.Distance,
                    Duration = workout.Duration,
                    Emotion = workout.Emotion,
                    Id = workout.Id,
                    Title = workout.Title,
                    PortalKey = workout.PortalKey,
                    UserKey = workout.UserSetting.UserKey,
                    Weather = workout.Weather,
                    DataSrc = workout.DataSrc,
                    WorkoutTypeKey = Convert.ToInt64( workout.WorkoutTypeKey )
                };
                if (workout.WorkoutExtendeds.FirstOrDefault() != null)
                {
                    WorkoutExtended ext = workout.WorkoutExtendeds.First();
                    Affine.Data.json.WorkoutExtended jsonWorkoutExtended = new Affine.Data.json.WorkoutExtended()
                    {
                        Id = ext.Id,
                        LatMax = ext.LatMax,
                        LatMin = ext.LatMin,
                        LngMax = ext.LngMax,
                        LngMin = ext.LngMin
                    };
                    Affine.Data.json.WorkoutSample[] samples = entities.WorkoutSamples.Where(s => s.WorkoutExtended.Id == ext.Id).OrderBy(s => s.SampleNumber)
                                                                .Select(s => new Affine.Data.json.WorkoutSample()
                                                                {
                                                                    Date = s.Date,
                                                                    Time = 0,
                                                                    Distance = s.Distance,
                                                                    Elevation = s.Elevation,
                                                                    HeartRate = s.HeartRate,
                                                                    Id = s.Id,
                                                                    Lat = s.Lat,
                                                                    Lng = s.Lng,
                                                                    SampleNumber = s.SampleNumber
                                                                }).ToArray();
                    foreach (Affine.Data.json.WorkoutSample s in samples)
                    {
                        s.Time = s.Date.Ticks;
                    }
                    jsonWorkoutExtended.WorkoutSamples = samples;
                    json.WorkoutExtended = jsonWorkoutExtended;
                    if (ext.MapRoute != null)
                    {
                        MapRoute mr = ext.MapRoute;
                        Affine.Data.json.MapRoute jsonMapRoute = new Affine.Data.json.MapRoute()
                        {
                            Id = mr.Id,
                            LatMax = mr.LatMax,
                            LatMin = mr.LatMin,
                            LngMax = mr.LngMax,
                            LngMin = mr.LngMin,
                            City = mr.City,
                            Name = mr.Name,
                            PortalKey = mr.PortalKey,
                            Rating = mr.Rating,
                            RouteDistance = mr.RouteDistance,
                            UserKey = workout.UserSetting.Id,
                            Region = mr.Region
                        };
                        Affine.Data.json.MapRoutePoint[] points = entities.MapRoutePoints.Where( mp => mp.MapRoute.Id == mr.Id ).OrderBy( mp => mp.Order ).Select(p => new Affine.Data.json.MapRoutePoint()
                                                                                                        {
                                                                                                            Id = p.Id,
                                                                                                            Lat = p.Lat,
                                                                                                            Lng = p.Lng,
                                                                                                            Order = p.Order
                                                                                                        }).ToArray();
                        jsonMapRoute.MapRoutePoints = points;
                        json.WorkoutExtended.MapRoute = jsonMapRoute;
                    }
                }
                JavaScriptSerializer jserializer = new JavaScriptSerializer();
                return jserializer.Serialize(json);
            }
            return "";
        }


        private string SerializeMapRoute(IQueryable<MapRoute> mapRouteQuery)
        {
            var ret = mapRouteQuery.Select(m => new { Id = m.Id, Name = m.Name, LatMin = m.LatMin, LngMin = m.LngMin, LatMax = m.LatMax, LngMax = m.LngMax, Distance = m.RouteDistance, PolyLine = m.PolyLineEncoding, Rating = m.Rating, Zoom = m.ThumbZoom, UserKey = m.UserSetting.UserKey, UserName = m.UserSetting.UserName, WorkoutTypeKey = m.WorkoutTypeId }).ToArray();            
            return _jserializer.Serialize(ret);
        }

        private string SerializeWOD(IQueryable<WOD> wodQuery)
        {
            var ret = wodQuery.Select(w => new { Id = w.Id, Name = w.Name, WODTypekey = w.WODType.Id, WODTypeName = w.WODType.Name, Description = w.Description, CreationDate = w.CreationDate, NumDone = w.UserStreams.OfType<Workout>().Count(), UserSettingsKey = w.UserSettingsKey, UserName = w.UserName, IsGroup = w.IsGroup, Standard = w.Standard }).ToArray();
            return _jserializer.Serialize(ret);
        }


        [WebMethod]
        public string GetSimilarRoutes(long rid, double radius, int skip, int take, string order)
        {
            take = TakeGaurd(take);
            aqufitEntities entities = new aqufitEntities();
            MapRoute route = entities.MapRoutes.FirstOrDefault(r => r.Id == rid);
            double extra = Affine.Utils.LatLngUntil.KMRoughDegree * radius;
            double lat = (route.LatMax + route.LatMin) / 2;
            double lng = (route.LngMax + route.LngMin) / 2;
            double latL =  lat + extra;
            double latM = lat - extra;
            double lngL = lng + extra;
            double lngM = lng - extra;
            
            if (order == "distance")
            {
                return SerializeMapRoute(entities.MapRoutes.Where(m => m.Id != rid && latL > m.LatMin && latM < m.LatMax && lngL > m.LngMin && lngM < m.LngMax).OrderBy(m => m.RouteDistance).Skip(skip).Take(take));
                //var ret = entities.MapRoutes.Where(m => m.Id != rid && latL > m.LatMin && latM < m.LatMax && lngL > m.LngMin && lngM < m.LngMax).OrderBy(m => m.RouteDistance).Skip(skip).Take(take).Select(m => new { Id = m.Id, Name = m.Name, LatMin = m.LatMin, LngMin = m.LngMin, LatMax = m.LatMax, LngMax = m.LngMax, Distance = m.RouteDistance, PolyLine = m.PolyLineEncoding, Rating = m.Rating, Zoom = m.ThumbZoom, UserKey = m.UserSetting.Id, UserName = m.UserSetting.UserName, WorkoutType = m.WorkoutType.Name }).ToArray();
                //return _jserializer.Serialize(ret);
            }
            else
            {
                return SerializeMapRoute(entities.MapRoutes.Where(m => m.Id != rid && latL > m.LatMin && latM < m.LatMax && lngL > m.LngMin && lngM < m.LngMax).OrderByDescending(m => m.Id).Skip(skip).Take(take));
                //var ret = entities.MapRoutes.Where(m => m.Id != rid && latL > m.LatMin && latM < m.LatMax && lngL > m.LngMin && lngM < m.LngMax).OrderByDescending(m => m.Id).Skip(skip).Take(take).Select(m => new { Id = m.Id, Name = m.Name, LatMin = m.LatMin, LngMin = m.LngMin, LatMax = m.LatMax, LngMax = m.LngMax, Distance = m.RouteDistance, PolyLine = m.PolyLineEncoding, Rating = m.Rating, Zoom = m.ThumbZoom, UserKey = m.UserSetting.Id, UserName = m.UserSetting.UserName, WorkoutType = m.WorkoutType.Name }).ToArray();
                //return _jserializer.Serialize(ret);
            }
        }

       

        [WebMethod]
        public string GetRoutes(double lat, double lng, double radius, int skip, int take, string order)
        {
            take = TakeGaurd(take);
           // double minLat = lat - r
            double extra = Affine.Utils.LatLngUntil.KMRoughDegree* radius;
            double latL = lat + extra;
            double latM = lat - extra;
            double lngL = lng + extra;
            double lngM = lng - extra;
            aqufitEntities entities = new aqufitEntities();
            if (order == "distance")
            {
                return SerializeMapRoute(entities.MapRoutes.Where(m => latL > m.LatMin && latM < m.LatMax && lngL > m.LngMin && lngM < m.LngMax).OrderBy(m => m.RouteDistance).Skip(skip).Take(take));
                //var ret = entities.MapRoutes.Where(m => latL > m.LatMin && latM < m.LatMax && lngL > m.LngMin && lngM < m.LngMax).OrderBy(m => m.RouteDistance).Skip(skip).Take(take).Select(m => new { Id = m.Id, Name = m.Name, LatMin = m.LatMin, LngMin = m.LngMin, LatMax = m.LatMax, LngMax = m.LngMax, Distance = m.RouteDistance, PolyLine = m.PolyLineEncoding, Rating = m.Rating, Zoom = m.ThumbZoom, UserKey = m.UserSetting.Id, UserName = m.UserSetting.UserName, WorkoutType = m.WorkoutType.Name }).ToArray();
                //return _jserializer.Serialize(ret);
            }
            else
            {
                return SerializeMapRoute(entities.MapRoutes.Where(m => latL > m.LatMin && latM < m.LatMax && lngL > m.LngMin && lngM < m.LngMax).OrderByDescending(m => m.Id).Skip(skip).Take(take));
                //var ret = entities.MapRoutes.Where(m => latL > m.LatMin && latM < m.LatMax && lngL > m.LngMin && lngM < m.LngMax).OrderByDescending(m => m.Id).Skip(skip).Take(take).Select(m => new { Id = m.Id, Name = m.Name, LatMin = m.LatMin, LngMin = m.LngMin, LatMax = m.LatMax, LngMax = m.LngMax, Distance = m.RouteDistance, PolyLine = m.PolyLineEncoding, Rating = m.Rating, Zoom = m.ThumbZoom, UserKey = m.UserSetting.Id, UserName = m.UserSetting.UserName, WorkoutType = m.WorkoutType.Name }).ToArray();
                //return _jserializer.Serialize(ret);
            }
        }

        

        [WebMethod]
        public string GetWorkouts(long userSettingsKey, int skip, int take, string order, long[] exerciseArray)
        {
            take = TakeGaurd(take);
            aqufitEntities entities = new aqufitEntities();
            IQueryable<WOD> wods = null;
            if (userSettingsKey > 0)
            {
                wods = entities.User2WODFav.Where(w => w.UserSetting.Id == userSettingsKey).Select(w => w.WOD);
                wods = wods.Union(entities.WODs.Where(w => w.Standard > 0));
            }
            else
            {
                wods = entities.WODs.OrderByDescending(w => w.CreationDate);
            }
            if (exerciseArray != null && exerciseArray.Length > 0)
            {
                long len = exerciseArray.Length;
                wods = wods.Intersect( entities.WODSets.Where(s => exerciseArray.Intersect( s.WODExercises.Select(x => x.Exercise.Id) ).Count() == len ).Select(s => s.WOD) );
            }
            if (order == "date")
            {
                wods = wods.OrderByDescending(w => w.CreationDate).Skip(skip).Take(take);
            }
            else
            {
                wods = wods.OrderByDescending(w => w.UserStreams.Count() ).Skip(skip).Take(take);
            }
            return SerializeWOD(wods);
        }


        // TODO: security here...
        [WebMethod]
        public string GetMyRoutes( long userSettingId, int skip, int take, string order)
        {
            take = TakeGaurd(take);       
            aqufitEntities entities = new aqufitEntities();
            if (order == "distance")
            {
                return SerializeMapRoute(entities.User2MapRouteFav.Include("MapRoutes").Where(m => m.UserSettingsKey == userSettingId).Select(m => m.MapRoute).OrderBy(m => m.RouteDistance).Skip(skip).Take(take));
            }
            else
            {
                return SerializeMapRoute(entities.User2MapRouteFav.Include("MapRoutes").Where(m => m.UserSettingsKey == userSettingId).Select(m => m.MapRoute).OrderByDescending(m => m.Id).Skip(skip).Take(take));
            }
        }
        

        // TODO: take this out..
        [WebMethod]
        public string addComment(long uid, long pid, long profile, long sid, string comment)
        {
            Affine.Data.Managers.IDataManager dataManager = Affine.Data.Managers.LINQ.DataManager.Instance;
            UserComment uc = dataManager.AddComment(uid, pid, profile, sid, comment);
            if (uc != null )
            {
                Affine.Data.json.StreamComment ret = new Data.json.StreamComment()
                {
                    UserSettingId = uc.UserSetting.Id,
                    PortalKey = uc.PortalKey,
                    UserKey = uc.UserSetting.UserKey,
                    Text = uc.Text,
                    StreamKey = uc.UserStream.Id,
                    DateTime = uc.DateTime.ToShortDateString(),
                    UserName = uc.UserSetting.UserName,
                    DateTicks = uc.DateTime.ToShortDateString() + " " + uc.DateTime.ToLongTimeString()
                };
                // TODO: an event manager that handdles this kind of stuff.   ( put this in the  dataManager )
                sendStreamEmailAsync(uc.UserStream, uc.UserSetting, uc);

                return _jserializer.Serialize(ret);
            }           
            Affine.Data.json.StreamComment err = new Data.json.StreamComment()
            {   // this could be handled better
                Id = -1,
                PortalKey = -1,
                UserKey = -1,
                Text = "",
                StreamKey = -1,
                UserName = "",
                DateTime = null,
                DateTicks = ""
            };
            return _jserializer.Serialize(err);
        }

        [WebMethod]
        public string getRecipeIndex(long pid)
        {
            aqufitEntities entities = new aqufitEntities();
            //   string[] ingArray = entities.RecipeIngredients.OrderBy(r => r.Text).Select(r => r.Text).ToArray();
            var indexArray = entities.UserStreamSet.OfType<Recipe>().OrderBy(r => r.Title).Select(r => new { Title = r.Title, Id = r.Id }).ToArray();
            return _jserializer.Serialize(indexArray);
        }

        // TODO: this is not a safe method ...
        [WebMethod]
        public string getMessageListData(long usid, int start, int take, int mode)
        {
            take = TakeGaurd(take);
            aqufitEntities entities = new aqufitEntities();
            long[] messageIds = mode == 0 ? entities.MessageRecipiants.Include("Message").
                                                            Where(mr => 
                                                                (mr.UserSettingsKey == usid && mr.Message.ParentKey == 0 && mr.Message.UserSetting.Id != usid)
                                                                ||
                                                                (mr.Message.UserSetting.Id == usid && mr.Message.LastUserKey != usid)).
                                                          //  Where(mr => mr.UserSettingsKey == usid && mr.Message.ParentKey == 0 && mr.Message.UserSetting.Id != mr.Message.LastUserKey).
                                                            OrderByDescending(mr => mr.Message.LastDateTime).Skip(start).Take(take).Select(mr => mr.Message.Id).ToArray()
                                                            :
                                            entities.MessageRecipiants.Include("Message").
                                                            Where(mr => mr.UserSettingsKey == usid && mr.Message.ParentKey == 0 && mr.Message.UserSetting.Id == usid).
                                                            OrderByDescending(mr => mr.Message.LastDateTime).Skip(start).Take(take).Select(mr => mr.Message.Id).ToArray();
            IList<Message> messages = entities.Messages.Include("UserSetting").Include("MessageRecipiants").Where(Affine.Utils.Linq.LinqUtils.BuildContainsExpression<Message, long>(m => m.Id, messageIds)).OrderByDescending(m => m.LastDateTime).ToList<Message>();  // TODO: check that this operation is deffered and we are not querying the whole stream (profiling)                  
            IList<Affine.Data.json.Message> mList = _IStreamManager.MessageArrayToMessageDataArray(messages);            
            // Need to set an unread status for each Message
            for (int i = 0; i < messages.Count; i++ )
            {
                MessageRecipiant recip = messages[i].MessageRecipiants.Where(mr => mr.UserSettingsKey == usid).FirstOrDefault();
                if (recip != null )    // if there is a recipiant and the status id "read"
                {
                    mList[i].Unread = recip.Status == 0;    // set to "unread"
                }
            }            
           // string json = _jserializer.Serialize(mList.Where( m => !(m.UserKey == uid && m.SecondUserKey == uid && m.LastUserKey == uid) ).ToList() );  // dont pass back messages that this user write (with no reply)
            string json = _jserializer.Serialize(mList); 
            return json;
        }

        // TODO: take out..
        [WebMethod]
        public string getMessage(long usid, long mid)
        {
            aqufitEntities entities = new aqufitEntities();
            IList<Message> messageHistory = entities.Messages.Include("UserSetting").Include("MessageRecipiants").Where(m => m.Id == mid || m.ParentKey == mid).OrderBy(m => m.DateTime).ToList();
            if (messageHistory.Count > 0)
            {
                MessageRecipiant recip = messageHistory[0].MessageRecipiants.Where(mr => mr.UserSettingsKey == usid).FirstOrDefault();
                if( recip != null )
                {
                    recip.Status = 1;           // set this user to status "read"
                    entities.SaveChanges();
                }
            }
            IList<Affine.Data.json.Message> mList = _IStreamManager.MessageArrayToMessageDataArray(messageHistory);                        
            string json = _jserializer.Serialize(mList);
            return json;
        }

        // TODO: take out..
        [WebMethod]
        public string saveReply(long usid, long mid, string txt)
        {
            // TODO: make sure that the person can reply to this message
            aqufitEntities entities = new aqufitEntities();            
            DateTime dt = DateTime.Now.ToUniversalTime();
            // TODO: cache settings ?
            UserSettings settings = entities.UserSettings.FirstOrDefault(us => us.Id == usid );
            txt = Utils.Web.WebUtils.MakeWebSafeString(txt);
            Message message = new Message()
            {
                UserSetting = settings,
                PortalKey = settings.PortalKey,
                Status = 0, // TODO: make nice message status ( 0 = unread )
                DateTime = dt,
                ParentKey = mid,
                Text = txt
            };            
            // This is how we handle repys ... we want to be able to query the messages easy and know what has changed... so we store some
            // data about the last reply in the "source" message.  That way we can update and show that there is a new message for people
            // but not have to search the entire message history to do this.
            Message reply = entities.Messages.Include("MessageRecipiants").FirstOrDefault(m => m.Id == mid);
            if (reply.LastUserKey != usid)
            {
                reply.SecondUserName = reply.LastUserName;
                reply.SecondUserKey = reply.LastUserKey;
                reply.SecondText = reply.LastText;
                reply.SecondDateTime = reply.LastDateTime;
            }
            reply.LastUserName = settings.UserName;
            reply.LastUserKey = usid;
            reply.LastText = txt.Length > 128 ? txt.Substring(0,128) + "..." : txt;
            reply.LastDateTime = dt;
            Message parent = entities.Messages.FirstOrDefault(m => m.Id == mid);
            foreach (MessageRecipiant mr in reply.MessageRecipiants)
            {
                if (mr.UserSettingsKey != settings.Id)  // dont notify the person who just replied...
                {
                    // mark the message as unread for all recipiants... But the person who wrote this.
                    if (mr.UserSettingsKey != usid && mr.Status == 1) mr.Status = 0;
                    UserSettings toUser = entities.UserSettings.FirstOrDefault(u => u.Id == mr.UserSettingsKey);
                    Notification mailNotification = new Notification()
                    {
                        PortalKey = toUser.PortalKey,
                        UserSetting = toUser,
                        Date = DateTime.Now.ToUniversalTime(),
                        Title = settings.UserName + " has replied you a message.",
                        Description = settings.UserName + " (" + settings.UserFirstName + " " + settings.UserLastName + ") has replied to a message.",
                        TimeStamp = DateTime.Now.ToUniversalTime(),
                        NotificationType = (int)Affine.Utils.ConstsUtil.NotificationTypes.NEW_MESSAGE,
                        PublishSettings = (int)Affine.Utils.ConstsUtil.PublishSettings.NO_STREAM,
                        Message = parent
                    };
                    entities.AddToUserStreamSet(mailNotification);
                }

            }
            entities.AddToMessages(message);
            try
            {
                entities.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message.Replace("'",""));
            }

            sendReplyEmailAsync(reply, settings);            
            Message ret = entities.Messages.Where( m => m.ParentKey == mid ).OrderByDescending( m => m.Id ).FirstOrDefault();
            string json = _jserializer.Serialize(_IStreamManager.MessageEntityToMessageData(ret));
            return json;
        }

        public string addStreamToFavorites(long uid, long pid, long sid)
        {
            try
            {
                aqufitEntities entities = new aqufitEntities();
                UserSettings settings = entities.UserSettings.FirstOrDefault(us => us.UserKey == uid && us.PortalKey == pid);
                UserStream stream = entities.UserStreamSet.FirstOrDefault(s => s.Id == sid);
                User2StreamFavorites u2s = new User2StreamFavorites()
                {
                    UserStream = stream,
                    UserKey = uid,
                    PortalKey = pid,                  
                };
                entities.AddToUser2StreamFavorites(u2s);
                entities.SaveChanges();
                return "{ 'status':'success' }";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string getWokoutStreamData(long userSettingsKey, Workout workout)
        {
            aqufitEntities entities = new aqufitEntities();
            //UserSettings settings = gid > 0 ? entities.UserSettings.FirstOrDefault(s => s.Id == gid) : entities.UserSettings.FirstOrDefault(s => s.UserKey == uid && s.PortalKey == pid);
            
            //IList<long> friendIds = new List<long>();
            /*
            if (mode == 0)  // NORMAL MODE
            {
                friendIds = ((perm == Convert.ToInt32(Affine.Dnn.Modules.ATI_PermissionPageBase.AqufitPermission.PUBLIC)) ? new List<long>() : entities.UserFriends.Where(f => (f.SrcUserSettingKey == profileSettings.Id || f.DestUserSettingKey == profileSettings.Id) && f.Relationship == (int)Affine.Utils.ConstsUtil.Relationships.FRIEND ).Select(f => (f.SrcUserSettingKey == profileSettings.Id ? f.DestUserSettingKey : f.SrcUserSettingKey)).ToList());
                friendIds.Add(profileSettings.Id);    // PERMISSIONS: add the users own stream (we always show the users stream unless they change settings )  TODO: permissions user settings
            }
            else if (mode == 1) // FOLOW MODE
            {
                friendIds = entities.UserFriends.Where(f => f.SrcUserSettingKey == profileSettings.Id && f.Relationship == (int)Affine.Utils.ConstsUtil.Relationships.FOLLOW ).Select(f => f.DestUserSettingKey).ToList();
                friendIds.Add(profileSettings.Id);    // PERMISSIONS: add the users own stream (we always show the users stream unless they change settings )  TODO: permissions user settings
            }
            else if (mode == 3) // Just "My" stream
            {
                friendIds.Add(profileSettings.Id);    // PERMISSIONS: add the users own stream (we always show the users stream unless they change settings )  TODO: permissions user settings
            }
            IQueryable<UserStream> stream = null;
            if (mode == 2) // Favorites
            {
                stream = entities.User2StreamFavorites.Include("UserStreamSet").Where(f => f.UserKey == profile && f.PortalKey == pid).Select(s => s.UserStream).OrderByDescending(s => s.TimeStamp).ThenByDescending(s => s.Date).Skip(skip).Take(take);
                // hydrate the usersettings
                stream.Select(s => s.UserSetting).ToArray();
            }
            else
            {

                stream = entities.UserStreamSet.Include("UserSetting").Where(LinqUtils.BuildContainsExpression<UserStream, long>(s => s.UserSetting.Id, friendIds)).Where(s => s.PortalKey == pid && (s.GroupKey == null || s.GroupKey == gid))
                                                    .Where(s => (s.PublishSettings == 0) || (s.PublishSettings == 1 && s.UserSetting.UserKey == profile) || (s.PublishSettings == 2 && s.UserSetting.UserKey != profile))  // This statment takes out publishing to a firends stream (PublishSettings == 1) byt leaves it in your own                                                     
                                                    .OrderByDescending(s => s.TimeStamp).ThenByDescending(s => s.Date).Skip(skip).Take(take);  // TODO: check that this operation is deffered and we are not querying the whole stream (profiling)                                                                          
            }
             */
            IQueryable<Workout> stream = null;
            // TODO: this sux harsh... but all my Garmin/Nike workouts come in as "Untitled" ... so lets ignore garmin
            if (workout.DataSrc == (int)Affine.Utils.WorkoutUtil.DataSrc.GARMIN || workout.DataSrc == (int)Affine.Utils.WorkoutUtil.DataSrc.NIKE_NO_MAP)
            {
                stream = entities.UserStreamSet.OfType<Workout>().Include("WOD").Include("UserSetting").Where(w => w.Id == workout.Id)
                                                        .OrderByDescending(s => s.TimeStamp).ThenByDescending(s => s.Date);
            }
            else
            {
                stream = entities.UserStreamSet.OfType<Workout>().Include("WOD").Include("UserSetting").Where(w => w.UserSetting.Id == userSettingsKey && w.WorkoutTypeKey == workout.WorkoutTypeKey && w.Title == workout.Title)
                                                        .OrderByDescending(s => s.TimeStamp).ThenByDescending(s => s.Date);
            }
                // TODO: this is the only way i can hydrate the Metrics..
            stream.Select(m => m.UserSetting.Metrics).ToArray();
            UserComment[] comments = entities.UserComment.Include("UserSetting").Where(LinqUtils.BuildContainsExpression<UserComment, long>(c => c.UserStream.Id, stream.Select(s => s.Id))).ToArray();

            string json = _jserializer.Serialize(_IStreamManager.UserStreamArrayToStreamDataArray(stream.ToArray(), comments));
            return json;
        }

        [WebMethod]
        public string getStreamItem(long id)
        {
            aqufitEntities entities = new aqufitEntities();
            IQueryable<UserStream> stream = entities.UserStreamSet.Include("WOD").Include("UserAttachments").Where(s => s.Id == id);
            stream.Select(m => m.UserSetting.Metrics).ToArray();
            // TODO: we should be able to have the comments just added to the stream with a navigation rule..
            UserComment[] comments = entities.UserComment.Include("UserSetting").Where(LinqUtils.BuildContainsExpression<UserComment, long>(c => c.UserStream.Id, stream.Select(s => s.Id))).ToArray();

            string json = _jserializer.Serialize(_IStreamManager.UserStreamArrayToStreamDataArray(stream.ToArray(), comments));
            return json;
        }

        // Mode = 0 - normal mode
        // Mode = 1 - follow mode
        // Mode = 2 - favorites
        // Mode = 3 - only "my" stream data

        [WebMethod]
        public string getStreamData(long gid, long uid, long pid, long profile, int perm, int mode, int skip, int take)
        {
            take = TakeGaurd(take);
            aqufitEntities entities = new aqufitEntities();            
            UserSettings profileSettings = gid > 0 ? entities.UserSettings.FirstOrDefault(s => s.Id == gid) :  entities.UserSettings.FirstOrDefault(s => s.UserKey == profile && s.PortalKey == pid);
            //UserSettings settings = gid > 0 ? entities.UserSettings.FirstOrDefault(s => s.Id == gid) : entities.UserSettings.FirstOrDefault(s => s.UserKey == uid && s.PortalKey == pid);
            
                    
            // TODO: need group permissions as well here.    
            // PERMISSIONS: we dont show friends streams to the public


            IList<long> friendIds = new List<long>();
            if (mode == 0)  // NORMAL MODE
            {
                friendIds = ((perm == (int)Affine.Dnn.Modules.ATI_PermissionPageBase.AqufitPermission.PUBLIC && gid <= 0) 
                    ? new List<long>() 
                    : 
                    entities.UserFriends.Where(f => ((f.SrcUserSettingKey == profileSettings.Id || f.DestUserSettingKey == profileSettings.Id) && f.Relationship != (int)Affine.Utils.ConstsUtil.Relationships.FOLLOW)
                                                    ||
                                                    ((f.SrcUserSettingKey == profileSettings.Id) && f.Relationship == (int)Affine.Utils.ConstsUtil.Relationships.FOLLOW) 
                                                    ).Select(f => (f.SrcUserSettingKey == profileSettings.Id ? f.DestUserSettingKey : f.SrcUserSettingKey)).ToList());
                //friendIds =  entities.UserFriends.Where(f => (f.SrcUserSettingKey == profileSettings.Id || f.DestUserSettingKey == profileSettings.Id) && f.Relationship != (int)Affine.Utils.ConstsUtil.Relationships.FOLLOW).Select(f => (f.SrcUserSettingKey == profileSettings.Id ? f.DestUserSettingKey : f.SrcUserSettingKey)).ToList();
                friendIds.Add(profileSettings.Id);    // PERMISSIONS: add the users own stream (we always show the users stream unless they change settings )  TODO: permissions user settings
            }
            else if (mode == 1) // FOLOW MODE
            {
                friendIds = entities.UserFriends.Where(f => f.SrcUserSettingKey == profileSettings.Id && f.Relationship == (int)Affine.Utils.ConstsUtil.Relationships.FOLLOW ).Select(f => f.DestUserSettingKey).ToList();
                friendIds.Add(profileSettings.Id);    // PERMISSIONS: add the users own stream (we always show the users stream unless they change settings )  TODO: permissions user settings
            }             
            else if (mode == 3) // Just "My" stream
            {
                friendIds.Add(profileSettings.Id);    // PERMISSIONS: add the users own stream (we always show the users stream unless they change settings )  TODO: permissions user settings
            }
            IQueryable<UserStream> stream = null;
            if (mode == 2) // Favorites
            {
                stream = entities.User2StreamFavorites.Include("UserStreamSet").Include("UserStreamSet.WOD").Include("UserStreamSet.UserAttachments").Where(f => f.UserKey == profile && f.PortalKey == pid).Select(s => s.UserStream).OrderByDescending(s => s.TimeStamp).ThenByDescending(s => s.Date).Skip(skip).Take(take);
                // hydrate the usersettings
                stream.Select(s => s.UserSetting).ToArray();
            }else{
              
                stream = entities.UserStreamSet.Include("WOD").Include("UserSetting").Include("ToUser").Include("UserAttachments").Where(LinqUtils.BuildContainsExpression<UserStream, long>(s => s.UserSetting.Id, friendIds))
                    // took this link out so we see group posts in the users stream...                               
                    // .Where( s => s.PortalKey == pid && (s.GroupKey == null || s.GroupKey == gid)  )
                                                    .Where(s => (s.PublishSettings == 0 && (s.ToUser == null || (s.ToUser != null && s.ToUser.Id == profileSettings.Id)))
                                                        || (s.PublishSettings == 1 && s.UserSetting.Id == profile)
                                                        || (s.PublishSettings == 2 && s.UserSetting.Id != profile)
                                                        )  // This statment takes out publishing to a firends stream (PublishSettings == 1) byt leaves it in your own                                                     
                                                    .OrderByDescending(s => s.TimeStamp).ThenByDescending(s => s.Date).Skip(skip).Take(take);
                                        
            }
            // TODO: this is the only way i can hydrate the Metrics..
            stream.Select(m => m.UserSetting.Metrics).ToArray();        
            // TODO: we should be able to have the comments just added to the stream with a navigation rule..
            UserComment[] comments = entities.UserComment.Include("UserSetting").Where(LinqUtils.BuildContainsExpression<UserComment, long>(c => c.UserStream.Id, stream.Select(s => s.Id))).ToArray();

            string json = _jserializer.Serialize(_IStreamManager.UserStreamArrayToStreamDataArray(stream.ToArray(), comments));
            return json;
        }

        [WebMethod]
        public string GetPageMetaData(string url)
        {
            Data.json.PageMetaData page = Affine.Utils.Web.WebUtils.GetPageMetaData(url);
            string json = _jserializer.Serialize(page);
            return json;
        }


        [WebMethod]
        public string getRecentWorkouts(long pid, int skip, int take)
        {
            take = TakeGaurd(take);
            aqufitEntities entities = new aqufitEntities();           
            IQueryable<UserStream> stream = null;
            stream = entities.UserStreamSet.Include("WOD").Include("UserSetting").Include("UserAttachments").OfType<Workout>().OrderByDescending(s => s.TimeStamp).ThenByDescending(s => s.Date).Skip(skip).Take(take);                                                                          
            // this is the only way i can hydrate the Metrics..
            stream.Select(m => m.UserSetting.Metrics).ToArray();
            // TODO: we should be able to have the comments just added to the stream with a navigation rule..
            UserComment[] comments = entities.UserComment.Include("UserSetting").Where(LinqUtils.BuildContainsExpression<UserComment, long>(c => c.UserStream.Id, stream.Select(s => s.Id))).ToArray();
            try
            {
                string json = _jserializer.Serialize(_IStreamManager.UserStreamArrayToStreamDataArray(stream.ToArray(), comments));
                return json;
            }
            catch (Exception ex)
            {
                return ex.Message + ex.StackTrace;
            }
        }

        [WebMethod]
        public string getNotifications(long usid, int skip, int take)
        {
            take = TakeGaurd(take);
            aqufitEntities entities = new aqufitEntities();
            IQueryable<UserStream> stream = null;
            stream = entities.UserStreamSet.Include("UserSetting").Include("WOD").Include("Message").OfType<Notification>().Where(n => n.UserSetting.Id == usid).OrderByDescending(s => s.TimeStamp).ThenByDescending(s => s.Date).Skip(skip).Take(take);
            // this is the only way i can hydrate the Metrics..
            stream.Select(m => m.UserSetting.Metrics).ToArray();
            // TODO: we should be able to have the comments just added to the stream with a navigation rule..
            UserComment[] comments = entities.UserComment.Include("UserSetting").Where(LinqUtils.BuildContainsExpression<UserComment, long>(c => c.UserStream.Id, stream.Select(s => s.Id))).ToArray();
            string json = _jserializer.Serialize(_IStreamManager.UserStreamArrayToStreamDataArray(stream.ToArray(), comments));
            return json;
        }


        public string getStreamData(UserStream us)
        {
            aqufitEntities entities = new aqufitEntities();
            UserComment[] comments = entities.UserComment.Include("UserSetting").Include("UserStream").Where( uc => uc.UserStream.Id == us.Id ).ToArray();

            string json = _jserializer.Serialize(_IStreamManager.UserStreamArrayToStreamDataArray(new UserStream[] { us }, comments));
            return json;
        }

        [WebMethod]
        public string getTutorialToolTip(object context)
        {
            // We cannot use a dictionary as a parameter, because it is only supported by script services.
            // The context object should be cast to a dictionary at runtime.
            IDictionary<string, object> contextDictionary = (IDictionary<string, object>)context;

            // keys TargetControlID,Value
            // we use an xml file to get the tutorial content that is in HTML format
            XDocument doc = XDocument.Load( HttpContext.Current.Server.MapPath("~/DesktopModules/ATI_Base/resources/xml/StreamTutorial.xml"));
            XElement content = doc.Root.Descendants("content").FirstOrDefault(d => d.Attribute("id").Value.Equals( (string)contextDictionary["Value"]) );
            string test = "<h1>Testing '" + doc.Root.Descendants("content").First().Attribute("id").Value + "'   =    '" + contextDictionary["Value"] + "'</h1>";
            if (content != null)
            {
                test = content.Value;
            }
            
            return test;
        }
       

        [WebMethod]
        public string getStreamDataForWOD(long wId, long settingsKey, int skip, int take, bool male, bool female, int rxd, double v1, double v2)
        {
            take = TakeGaurd(take);
            aqufitEntities entities = new aqufitEntities();
            IQueryable<Workout> stream = null;
            if (settingsKey > 0)
            {
                stream = entities.UserStreamSet.OfType<Workout>().Include("WOD").Include("UserAttachments").Where(w => w.WOD.Id == wId && w.UserSetting.Id == settingsKey);
            }
            else
            {
                stream = entities.UserStreamSet.OfType<Workout>().Include("WOD").Include("UserAttachments").Where(w => w.WOD.Id == wId);
            }
            if (!male || !female)
            {
                string sex = !female ? "M" : "F";
                stream = stream.Where(w => w.UserSetting.Sex.Equals(sex));
            }
            long typeId = entities.WODs.Where(w => w.Id == wId).Select(w => w.WODType.Id ).FirstOrDefault();
            if (v2 < v1)
            {
                double temp = v2;
                v2 = v1;
                v1 = temp;
            }            
            switch (typeId)
            {
                case (long)Affine.Utils.WorkoutUtil.WodType.AMRAP:
                case (long)Affine.Utils.WorkoutUtil.WodType.SCORE:                    
                    if (v1 > 0 && v2 > 0)
                    {
                        stream = stream.Where(w => w.Score >= v1 && w.Score <= v2);
                    }
                    stream = stream.OrderByDescending(w => w.Score);                    
                    break;
                case (long)Affine.Utils.WorkoutUtil.WodType.MAX_WEIGHT:
                    if (v1 > 0 && v2 > 0)
                    {
                        stream = stream.Where(w => w.Max >= v1 && w.Max <= v2);
                    }                    
                    stream = stream.OrderByDescending(w => w.Max);                    
                    break;
                case (long)Affine.Utils.WorkoutUtil.WodType.TIMED:
                    if (v1 > 0 && v2 > 0)
                    {
                        long v1l = Convert.ToInt64(v1);
                        long v2l = Convert.ToInt64(v2);
                        stream = stream.Where(w => w.Duration >= v1l && w.Duration <= v2l);
                    }
                    stream = stream.OrderBy(w => w.Duration);                    
                    break;
                default:
                    stream = stream.OrderByDescending(w => w.TimeStamp);
                    break;
            }
            if (rxd > 0)
            {
                stream = stream.Where(w => w.RxD == true);
            }
            else if (rxd == 0)
            {
                stream = stream.Where(w => w.RxD == false);
            }
            stream = stream.Skip(skip).Take(take);

            // hydrate usersettings
            stream.Select(w => w.UserSetting.Metrics).ToArray();

            // TODO: this is the only way i can hydrate the Comments..
            UserComment[] comments = entities.UserComment.Include("UserSetting").Where(LinqUtils.BuildContainsExpression<UserComment, long>(c => c.UserStream.Id, stream.Select(s => s.Id))).ToArray();

            string json = _jserializer.Serialize(_IStreamManager.UserStreamArrayToStreamDataArray(stream.ToArray(), comments));
            return json;
        }

        public string getStreamDataForRoute(long rId, int skip, int take)
        {
            take = TakeGaurd(take);
            aqufitEntities entities = new aqufitEntities();
            
            // PERMISSIONS: we dont show friends streams to the public

            IQueryable<Workout> stream = entities.WorkoutExtendeds.Include("UserStream").Where(we => we.MapRoute != null && we.MapRoute.Id == rId).Select(we => we.UserStream).OfType<Workout>().OrderByDescending(w => w.Id);
           // IQueryable<Workout> stream = entities.UserStreamSet.OfType<Workout>().Include("UserSetting").Include("WorkoutExtendeds").Where(w => w.WorkoutExtendeds != null && w.WorkoutExtendeds.First().MapRoute != null && w.WorkoutExtendeds.First().MapRoute.Id == rId ).OrderBy(w => w.Duration).Skip(skip).Take(take);  // TODO: check that this operation is deffered and we are not querying the whole stream (profiling)                                                                          
            
            // hydrate usersettings
            stream.Select(w => w.UserSetting.Metrics).ToArray();

            // TODO: this is the only way i can hydrate the Metrics..
            UserComment[] comments = entities.UserComment.Include("UserSetting").Where(LinqUtils.BuildContainsExpression<UserComment, long>(c => c.UserStream.Id, stream.Select(s => s.Id))).ToArray();

            string json = _jserializer.Serialize(_IStreamManager.UserStreamArrayToStreamDataArray(stream.ToArray(), comments));
            return json;
        }


        [WebMethod]
        public string getGroupPlaces(double neLat, double neLng, double swLat, double swLng, int skip, int take)
        {
            take = TakeGaurd(take);
            double minLat, minLng, maxLat, maxLng;
            if (neLat < swLat)
            {
                minLat = neLat;
                maxLat = swLat;
            }
            else
            {
                minLat = swLat;
                maxLat = neLat;
            }
            if (neLng < swLng)
            {
                minLng = neLng;
                maxLng = swLng;
            }
            else
            {
                minLng = swLng;
                maxLng = neLng;
            }

            aqufitEntities entities = new aqufitEntities();
            IQueryable<Place> allFound = entities.Places.Include("UserSettings").Where(p => (p.Lat >= minLat && p.Lat <= maxLat) && (p.Lng >= minLng && p.Lng <= maxLng)).OrderBy(p => p.Name);
            IQueryable<Place> places = allFound.Skip(skip).Take(take);
            var tmp = places.Select(p => new { Address = p.Street + "," + p.City + "," + p.Postal, Id = p.UserSetting.Id, GroupKey = p.UserSetting.Id, Lat = p.Lat, Lng = p.Lng, Name = p.Name, UserName = p.UserSetting.UserName, UserKey = p.UserSetting.UserKey, ImageId = p.UserSetting.Image != null ? p.UserSetting.Image.Id : 0, Description = "" }).ToArray();
            var ret = new { PagerInfo = new { Skip = skip, Take = take, Length = allFound.Count() }, Data = tmp };
            string json = _jserializer.Serialize(ret);
            return json;
        }


        // Mode = 0 - normal mode
        // Mode = 1 - follow mode
        // Mode = 2 - favorites
        // Mode = 3 - only "my" stream data

        [WebMethod]
        public string searchStreamData(long uid, long pid, long profile, int perm, int mode, int take, int skip, string search)
        {
            take = TakeGaurd(take);
            string searchLow = search.ToLower();
            aqufitEntities entities = new aqufitEntities();
            IQueryable<UserStream> stream = null;
            if (string.IsNullOrEmpty(search))
            {
                stream = entities.UserStreamSet.Include("UserSetting").Where(r => r.PortalKey == pid).OrderByDescending(s => s.Date).Skip(skip).Take(take);                
            }
            else
            {
                string[] terms = search.Split(' ');
                IList<long> rIds = new List<long>();
                foreach( string term in terms ){
                  rIds = rIds.Concat(entities.RecipeIngredients.Where(r => r.Text.ToLower().Contains(term)).Select(r => r.RecipeExtended.UserStream.Id)).ToList();
                  rIds = rIds.Concat(entities.UserStreamSet.OfType<Recipe>().Where(r => r.Title.ToLower().Contains(term) || r.Description.ToLower().Contains(term) || r.Tags.ToLower().Contains(term)).Select(r => r.Id).ToArray()).ToList();                    
                }
                stream = entities.UserStreamSet.Where(LinqUtils.BuildContainsExpression<UserStream, long>(r => r.Id, rIds)).Where(r => r.PortalKey == pid).OrderByDescending(s => s.Date).Skip(skip).Take(take);
            }
              
            UserSettings profileSettings = entities.UserSettings.FirstOrDefault(s => s.UserKey == profile && s.PortalKey == pid);
            UserSettings settings = entities.UserSettings.FirstOrDefault(s => s.UserKey == uid && s.PortalKey == pid);
            // PERMISSIONS: we dont show friends streams to the public
            IList<long> friendIds = new List<long>();
            if (mode == 0)  // NORMAL MODE
            {
                friendIds = ((perm == Convert.ToInt32(Affine.Dnn.Modules.ATI_PermissionPageBase.AqufitPermission.PUBLIC)) ? new List<long>() : entities.UserFriends.Where(f => (f.SrcUserSettingKey == profileSettings.Id || f.DestUserSettingKey == profileSettings.Id) && f.Relationship == (int)Affine.Utils.ConstsUtil.Relationships.FRIEND ).Select(f => (f.SrcUserSettingKey == profileSettings.Id ? f.DestUserSettingKey : f.SrcUserSettingKey)).ToList());
                friendIds.Add(profileSettings.Id);    // PERMISSIONS: add the users own stream (we always show the users stream unless they change settings )  TODO: permissions user settings
            }
            else if (mode == 1) // FOLOW MODE
            {
                friendIds = entities.UserFriends.Where(f => f.SrcUserSettingKey == profileSettings.Id && f.Relationship == (int)Affine.Utils.ConstsUtil.Relationships.FOLLOW).Select(f => f.DestUserSettingKey).ToList();
                friendIds.Add(profileSettings.Id);    // PERMISSIONS: add the users own stream (we always show the users stream unless they change settings )  TODO: permissions user settings
            }
            else if (mode == 3) // Just "My" stream
            {
                friendIds.Add(profileSettings.Id);    // PERMISSIONS: add the users own stream (we always show the users stream unless they change settings )  TODO: permissions user settings
            }

            IQueryable<UserStream> stream2 = null;
            if (mode == 2) // Favorites
            {
                stream2 = entities.User2StreamFavorites.Include("UserStreamSet").Include("UserStreamSet.WOD").Where(f => f.UserKey == profile && f.PortalKey == pid).Select(s => s.UserStream).OrderByDescending(s => s.Date);
                
            }
            else
            {

                long[] ids = entities.UserStreamSet.Include("WOD").Where(LinqUtils.BuildContainsExpression<UserStream, long>(s => s.UserSetting.Id, friendIds)).Where(s => s.PortalKey == pid)
                                                    .Where(s => (s.PublishSettings == 0) || (s.PublishSettings == 1 && s.UserSetting.UserKey == profile) || (s.PublishSettings == 2 && s.UserSetting.UserKey != profile))  // This statment takes out publishing to a firends stream (PublishSettings == 1) byt leaves it in your own                                                     
                                                    .Select( s => s.Id ).ToArray();  // TODO: check that this operation is deffered and we are not querying the whole stream (profiling)                                                                          
                stream2 = stream.Where(LinqUtils.BuildContainsExpression<UserStream, long>(s => s.Id, ids));
            }
           // stream = stream.Intersect(stream2);
 
            // hydrate the usersettings
            stream2.Select(s => s.UserSetting).ToArray();
            // TODO: this is the only way i can hydrate the Metrics..
            stream2.Select(m => m.UserSetting.Metrics).ToArray();
            UserComment[] comments = entities.UserComment.Include("UserSetting").Where(LinqUtils.BuildContainsExpression<UserComment, long>(c => c.UserStream.Id, stream2.Select(s => s.Id))).ToArray();

            string json = _jserializer.Serialize(_IStreamManager.UserStreamArrayToStreamDataArray(stream2.ToArray(), comments));
            return json;
        }

        [WebMethod]
        public string getRecipeStreamData(long pid, string search, int skip, int take )
        {
            take = TakeGaurd(take);
           // UserInfo ui = (UserInfo)HttpContext.Current.Items["UserInfo"];
           // PortalSettings ps = (PortalSettings)HttpContext.Current.Items["PortalSettings"];
            string searchLow = search.ToLower();
            aqufitEntities entities = new aqufitEntities();
            IQueryable<UserStream> stream = null;
            if (string.IsNullOrEmpty(search))
            {
                stream = entities.UserStreamSet.Include("UserSetting").Where(r => r.PortalKey == pid).OrderByDescending(s => s.Date).Skip(skip).Take(take);                
            }
            else
            {
                string[] terms = search.Split(' ');
                IList<long> rIds = new List<long>();
                foreach( string term in terms ){
                    rIds = rIds.Concat(entities.RecipeIngredients.Where(r => r.Text.ToLower().Contains(term)).Select(r => r.RecipeExtended.UserStream.Id)).ToList();
                    rIds = rIds.Concat(entities.UserStreamSet.OfType<Recipe>().Where(r => r.Title.ToLower().Contains(term) || r.Description.ToLower().Contains(term) || r.Tags.ToLower().Contains(term)).Select(r => r.Id).ToArray()).ToList();                    
                }
                stream = entities.UserStreamSet.Include("UserSetting").Where(LinqUtils.BuildContainsExpression<UserStream, long>(r => r.Id, rIds)).Where(r => r.PortalKey == pid).OrderByDescending(s => s.Date).Skip(skip).Take(take);
            }
            // TODO: this is the only way i can hydrate the Metrics..
            stream.Select(m => m.UserSetting.Metrics).ToArray();
          
            // TODO: i think it is faster to hydrate this as well
            UserComment[] comments = entities.UserComment.Include("UserSetting").Where(LinqUtils.BuildContainsExpression<UserComment, long>(c => c.UserStream.Id, stream.Select(s => s.Id))).ToArray();

            string json = _jserializer.Serialize(_IStreamManager.UserStreamArrayToStreamDataArray(stream.ToArray(), comments));
            return json;
        }

        [WebMethod]
        public string getRecipe(long rid)
        {
            aqufitEntities entities = new aqufitEntities();
            IQueryable< UserStream > stream = entities.UserStreamSet.Include("UserSetting").Where(s => s.Id == rid);
            // hydrate the metrics
            stream.Select(m => m.UserSetting.Metrics).ToArray();
            UserComment[] comments = entities.UserComment.Include("UserSetting").Where(c => c.UserStream.Id == rid ).ToArray();
            RecipeExtended r = entities.RecipeExtendeds.Include("Image").Include("RecipeIngredients").FirstOrDefault(s => s.UserStream.Id == rid);
            IList<Affine.Data.json.StreamData> dsList = _IStreamManager.UserStreamArrayToStreamDataArray( new UserStream[]{ stream.First() }, comments);
            Affine.Data.json.StreamData ds = dsList[0];
            Affine.Data.json.RecipeExtended re = new Data.json.RecipeExtended()
            {
                Id = r.Id,
                NumRatings = r.NumRatings,
                NumStrictness = r.NumStrictness,
                Directions = r.Directions
            };
            re.Image1Id = r.Image != null ? r.Image.Id : 0;
            re.Image2Id = Convert.ToInt64( r.Image2Key );
            re.Image3Id = Convert.ToInt64(r.Image3Key);
            re.Image4Id = Convert.ToInt64(r.Image4Key);
            re.Ingredients = r.RecipeIngredients.Select(i => new Affine.Data.json.RecipeIngredient() { Id = i.Id, Name = i.Text }).ToArray();
            ds.RecipeExtended = re;
            string json = _jserializer.Serialize(ds);
            return json; 
        }

        [WebMethod]
        public string getActiveGroups(int skip, int take)
        {
            take = TakeGaurd(take);
            aqufitEntities entities = new aqufitEntities();
            IQueryable<Group> randGroupQuery = entities.UserSettings.OfType<Group>().Include("Places").Where(g => g.MainGroupKey > 0);
            int numGorups = randGroupQuery.Count();
            Group[] mainGroups = randGroupQuery.OrderByDescending(g => g.MainGroupKey).Skip(skip).Take(take).ToArray();

            return getJsonGroupData(mainGroups, skip, take, numGorups);
        }

        public string getJsonGroupData(UserSettings[] groupList, int skip, int take, int length)
        {
            Affine.Data.json.UserSetting[] ret = groupList.Select(f => new Affine.Data.json.UserSetting() { Id = f.Id, FirstName = f.UserFirstName, LastName = f.UserLastName, UserKey = f.UserKey, PortalKey = f.PortalKey, UserName = f.UserName }).ToArray();

            var data = new { Data = ret, PagerInfo = new { Skip = skip, Take = take, Length = length } };
            string json = _jserializer.Serialize(data);
            return json;
        }

        public string getGroupListData(long uSettingsId, int skip, int take)
        {            
            aqufitEntities entities = new aqufitEntities();
            UserSettings settings = entities.UserSettings.FirstOrDefault(s => s.Id == uSettingsId);
            long[] groupIds = null;            
            groupIds = entities.UserFriends.Where(f => (f.SrcUserSettingKey == settings.Id || f.DestUserSettingKey == settings.Id) && f.Relationship >= (int)Affine.Utils.ConstsUtil.Relationships.GROUP_OWNER ).Select(f => f.SrcUserSettingKey == settings.Id ? f.DestUserSettingKey : f.SrcUserSettingKey).ToArray();
            
            UserSettings[] groupList = entities.UserSettings.OfType<Group>().Where(LinqUtils.BuildContainsExpression<UserSettings, long>(us => us.Id, groupIds)).OrderBy(f => f.UserName).Skip(skip).Take(take).ToArray();
            return getJsonGroupData(groupList, skip, take, groupIds.Length);
        }

        [WebMethod]
        public string searchGroupListData(long pid, string query, int skip, int take)
        {
            aqufitEntities entities = new aqufitEntities();           
            IQueryable<Group> groupListQuery = !string.IsNullOrEmpty(query) ? entities.UserSettings.OfType<Group>().Where(g => g.PortalKey == pid && (g.UserName.Contains(query) || g.Places.Where( p => p.Name.Contains(query) ).FirstOrDefault() != null ) ) : entities.UserSettings.OfType<Group>().Where(g => g.PortalKey == pid);
            UserSettings[] groupList = groupListQuery.OrderBy(f => f.UserName).Skip(skip).Take(take).ToArray();
            Affine.Data.json.UserSetting[] ret = groupList.Select(f => new Affine.Data.json.UserSetting() { Id = f.Id, FirstName = f.UserFirstName, LastName = f.UserLastName, UserKey = f.UserKey, PortalKey = f.PortalKey, UserName = f.UserName }).ToArray();
            var data = new { Data = ret, PagerInfo = new { Skip = skip, Take = take, Length = groupListQuery.Count() } };
            string json = _jserializer.Serialize(data);
            return json;
        }

        [WebMethod]
        public string getFriendSuggestions(long uid, long gid, long[] avoid, int take)
        {
            take = TakeGaurd(take, 10);
            aqufitEntities entities = new aqufitEntities();
            UserSettings settings = entities.UserSettings.FirstOrDefault(s => s.Id == gid);
            long[] friendIds = entities.UserFriends.Where(f => (f.SrcUserSettingKey == settings.Id || f.DestUserSettingKey == settings.Id) && f.Relationship >= (int)Affine.Utils.ConstsUtil.Relationships.GROUP_OWNER)
                                            .Select(f => f.SrcUserSettingKey == settings.Id ? f.DestUserSettingKey : f.SrcUserSettingKey).Where(i => i != uid).ToArray();
            IQueryable<UserSettings> memberList = entities.UserSettings.Include("UserRequests").OfType<User>().Where(LinqUtils.BuildContainsExpression<UserSettings, long>(us => us.Id, friendIds)).OrderBy(f => f.UserName).AsQueryable();

            friendIds = entities.UserFriends.Where(f => (f.SrcUserSettingKey == uid || f.DestUserSettingKey == uid) && f.Relationship == (int)Affine.Utils.ConstsUtil.Relationships.FRIEND).Select(f => f.SrcUserSettingKey == uid ? f.DestUserSettingKey : f.SrcUserSettingKey).ToArray();
            IQueryable<UserSettings> alreadyFriends = memberList.Where(LinqUtils.BuildContainsExpression<UserSettings, long>(us => us.Id, friendIds));
            IQueryable<UserSettings> memberNotFriends = memberList.Except(alreadyFriends);
            if (avoid != null)
            {
                IQueryable<UserSettings> avoidMembers = memberList.Where(LinqUtils.BuildContainsExpression<UserSettings, long>(us => us.Id, avoid));
                memberNotFriends = memberNotFriends.Except(avoidMembers);
            }
            IList<Affine.Data.json.UserSetting> ret = memberNotFriends.Select(f => new Affine.Data.json.UserSetting()
            {
                Id = f.Id,
                FirstName = f.UserFirstName,
                LastName = f.UserLastName,
                UserKey = f.UserKey,
                PortalKey = f.PortalKey,
                UserName = f.UserName,
            }).OrderBy( u => u.Id ).ToList();      // These are people that are not friends
            // We need to remove people that a request has been sent to already
            long[] allreadyRequested = entities.UserRequestSet.OfType<FriendRequest>().Where(r => r.UserSetting.Id == uid).Select(r => r.FriendRequestSettingsId).ToArray();
            ret = ret.Where(f => ! allreadyRequested.Contains(f.Id) ).Take(take).ToArray();

            string json = _jserializer.Serialize(ret);
            return json;
        }

        [WebMethod]
        public string getGroupMembersToFriend(long uid, long gid, int skip, int take)
        {
            take = TakeGaurd(take, 50);
            aqufitEntities entities = new aqufitEntities();
            UserSettings settings = entities.UserSettings.FirstOrDefault(s => s.Id == gid );
            long[] friendIds = null;
            // TODO: we can not do an orderby at the "id" level to do a skip take.. 
            // Friend is (user as SRC or DST) + RelationShip == FRIEND
            friendIds = entities.UserFriends.Where(f => (f.SrcUserSettingKey == settings.Id || f.DestUserSettingKey == settings.Id) && f.Relationship >= (int)Affine.Utils.ConstsUtil.Relationships.GROUP_OWNER)
                                            .Select(f => f.SrcUserSettingKey == settings.Id ? f.DestUserSettingKey : f.SrcUserSettingKey).Where( i => i != uid ).ToArray();          
            IQueryable< UserSettings > memberList = entities.UserSettings.Include("UserRequests").OfType<User>().Where(LinqUtils.BuildContainsExpression<UserSettings, long>(us => us.Id, friendIds)).OrderBy(f => f.UserName);
            int length = memberList.Count();
            memberList = memberList.Skip(skip).Take(take);                      

            friendIds = entities.UserFriends.Where(f => (f.SrcUserSettingKey == uid || f.DestUserSettingKey == uid) && f.Relationship == (int)Affine.Utils.ConstsUtil.Relationships.FRIEND).Select(f => f.SrcUserSettingKey == uid ? f.DestUserSettingKey : f.SrcUserSettingKey).ToArray();
            IQueryable<UserSettings> alreadyFriends = memberList.Where(LinqUtils.BuildContainsExpression<UserSettings, long>(us => us.Id, friendIds));
            IQueryable< UserSettings > memberNotFriends = memberList.Except(alreadyFriends);

            IList<Affine.Data.json.UserSetting> ret = alreadyFriends.Select(f => new Affine.Data.json.UserSetting()
            {
                Id = f.Id,
                FirstName = f.UserFirstName,
                LastName = f.UserLastName,
                UserKey = f.UserKey,
                PortalKey = f.PortalKey,
                UserName = f.UserName,
                RequestStatus = 0 }).ToList();      // Zero is already a friend

            IList<Affine.Data.json.UserSetting> ret2 = memberNotFriends.Select(f => new Affine.Data.json.UserSetting()
            {
                Id = f.Id,
                FirstName = f.UserFirstName,
                LastName = f.UserLastName,
                UserKey = f.UserKey,
                PortalKey = f.PortalKey,
                UserName = f.UserName,
                RequestStatus = -1
            }).ToList();      // These are people that are not friends

            ret = ret.Concat(ret2).OrderBy(u => u.UserName).ToList();
            var data = new { Data = ret, PagerInfo = new { Skip = skip, Take = ret.Count, Length = length } };
            string json = _jserializer.Serialize(data);
            return json;
        }

        [WebMethod]
        public string getMemberListDataOfRelationship(long gid, int relationship, int skip = 0 , int take = 25, long contextUserSettings = 0)
        {
            take = TakeGaurd(take, 50);
            Affine.Data.Managers.IStreamManager streamMan = Affine.Data.Managers.LINQ.StreamManager.Instance;
            Utils.ConstsUtil.Relationships r =  Utils.ConstsUtil.IntToRelationship(relationship);
            Affine.Data.json.UserSetting[] members = streamMan.GetGroupMembersOfRelationship(gid, Utils.ConstsUtil.IntToRelationship(relationship), skip, take);
            return streamMan.ToJsonWithPager(members, take);
        }

        public string getMemberListData(long uid, long pid, int skip = 0, int take = 25, long contextUserSettingsKey = 0)
        {
            take = TakeGaurd(take, 50);
            aqufitEntities entities = new aqufitEntities();
            UserSettings settings = entities.UserSettings.FirstOrDefault(s => s.UserKey == uid && s.PortalKey == pid);
            long[] friendIds = null;
            // TODO: we can not do an orderby at the "id" level to do a skip take.. 
              // Friend is (user as SRC or DST) + RelationShip == FRIEND

            friendIds = entities.UserFriends.Where(f => (f.SrcUserSettingKey == settings.Id || f.DestUserSettingKey == settings.Id) && f.Relationship >= (int)Affine.Utils.ConstsUtil.Relationships.GROUP_OWNER).Select(f => f.SrcUserSettingKey == settings.Id ? f.DestUserSettingKey : f.SrcUserSettingKey).ToArray();
            
            // TODO: do a TAKE here.
            UserSettings[] friendList = entities.UserSettings.OfType<User>().Where(LinqUtils.BuildContainsExpression<UserSettings, long>(us => us.Id, friendIds)).OrderBy(f => f.UserName).Skip(skip).Take(take).ToArray();
            if (contextUserSettingsKey == 0 || contextUserSettingsKey == settings.Id)
            {
                Affine.Data.json.UserSetting[] ret = friendList.Select(f => new Affine.Data.json.UserSetting() { Id = f.Id, FirstName = f.UserFirstName, LastName = f.UserLastName, UserKey = f.UserKey, PortalKey = f.PortalKey, UserName = f.UserName, RequestStatus = 0 }).ToArray();
                var data = new { Data = ret, PagerInfo = new { Skip = skip, Take = take, Length = friendIds.Length } };
                string json = _jserializer.Serialize(data);
                return json;
            }
            else
            {
                // TODO: clearn this up ... useed for getFreindListData function as well 
                Affine.Data.json.UserSetting[] ret = friendList.Where(f => f.Id != contextUserSettingsKey).Select(f => new Affine.Data.json.UserSetting()
                {
                    Id = f.Id,
                    FirstName = f.UserFirstName,
                    LastName = f.UserLastName,
                    UserKey = f.UserKey,
                    PortalKey = f.PortalKey,
                    UserName = f.UserName,
                    RequestStatus = (entities.UserRequestSet.OfType<FriendRequest>().Where(rs => (rs.UserSetting.Id == contextUserSettingsKey && rs.FriendRequestSettingsId == f.Id) || (rs.UserSetting.Id == f.Id && rs.FriendRequestSettingsId == contextUserSettingsKey)).FirstOrDefault() == null ? -1 : entities.UserRequestSet.OfType<FriendRequest>().Where(rs => (rs.UserSetting.Id == contextUserSettingsKey && rs.FriendRequestSettingsId == f.Id) || (rs.UserSetting.Id == f.Id && rs.FriendRequestSettingsId == contextUserSettingsKey)).FirstOrDefault().Status.Value)
                }).ToArray();

                var data = new { Data = ret, PagerInfo = new { Skip = skip, Take = take, Length = friendIds.Length } };
                string json = _jserializer.Serialize(data);
                return json;
            }
        }

        public string getFriendListData(long uid, long pid, Affine.Utils.ConstsUtil.FriendListModes mode, int skip = 0, int take = 50, long contextUserSettingsKey = 0)
        {
            take = TakeGaurd(take, 50);
            aqufitEntities entities = new aqufitEntities();
            UserSettings settings = entities.UserSettings.FirstOrDefault(s => s.UserKey == uid && s.PortalKey == pid);
            long[] friendIds = null;
            // TODO: we can not do an orderby at the "id" level to do a skip take.. 
            if (mode == Utils.ConstsUtil.FriendListModes.FOLLOWING)
            {   // Folling is (user as SRC) + RelationShip == FOLLOWING
                friendIds = entities.UserFriends.Where(f => f.SrcUserSettingKey == settings.Id && f.Relationship == (int)Affine.Utils.ConstsUtil.Relationships.FOLLOW ).Select(f => f.DestUserSettingKey).ToArray();
            }
            else if (mode == Utils.ConstsUtil.FriendListModes.FOLLOWERS)
            {
                friendIds = entities.UserFriends.Where(f => f.DestUserSettingKey == settings.Id && f.Relationship == (int)Affine.Utils.ConstsUtil.Relationships.FOLLOW).Select(f => f.SrcUserSettingKey).ToArray();            
            }
            else
            {   // Friend is (user as SRC or DST) + RelationShip == FRIEND
                friendIds = entities.UserFriends.Where(f => (f.SrcUserSettingKey == settings.Id || f.DestUserSettingKey == settings.Id) && f.Relationship == (int)Affine.Utils.ConstsUtil.Relationships.FRIEND).Select(f => f.SrcUserSettingKey == settings.Id ? f.DestUserSettingKey : f.SrcUserSettingKey).ToArray();
            }
            UserSettings[] friendList = entities.UserSettings.OfType<User>().Where(LinqUtils.BuildContainsExpression<UserSettings, long>(us => us.Id, friendIds)).OrderBy(f => f.UserName).Skip(skip).Take(take).ToArray();
            if (contextUserSettingsKey == 0 || contextUserSettingsKey == settings.Id)
            {
                // This is kinda messed here... since request status is 0 if they are not friends
                Affine.Data.json.UserSetting[] ret = friendList.Select(f => new Affine.Data.json.UserSetting() { Id = f.Id, FirstName = f.UserFirstName, LastName = f.UserLastName, UserKey = f.UserKey, PortalKey = f.PortalKey, UserName = f.UserName,
                                                                                                                 RequestStatus = 0  // alread friends (since this is a request for that users friends)
                }).ToArray();
                var data = new { Data = ret, PagerInfo = new { Skip = skip, Take = take, Length = friendIds.Length } };
                string json = _jserializer.Serialize(data);
                return json;
            }
            else
            {
                Affine.Data.json.UserSetting[] ret = friendList.Where(f => f.Id != contextUserSettingsKey).Select(f => new Affine.Data.json.UserSetting()
                {
                    Id = f.Id,
                    FirstName = f.UserFirstName,
                    LastName = f.UserLastName,
                    UserKey = f.UserKey,
                    PortalKey = f.PortalKey,
                    UserName = f.UserName,
                    RequestStatus = (entities.UserRequestSet.OfType<FriendRequest>().Where(rs => (rs.UserSetting.Id == contextUserSettingsKey && rs.FriendRequestSettingsId == f.Id) || (rs.UserSetting.Id == f.Id && rs.FriendRequestSettingsId == contextUserSettingsKey)).FirstOrDefault() == null ? -1 : entities.UserRequestSet.OfType<FriendRequest>().Where(rs => (rs.UserSetting.Id == contextUserSettingsKey && rs.FriendRequestSettingsId == f.Id) || (rs.UserSetting.Id == f.Id && rs.FriendRequestSettingsId == contextUserSettingsKey)).FirstOrDefault().Status.Value)
                }).ToArray();

                var data = new { Data = ret, PagerInfo = new { Skip = skip, Take = take, Length = friendIds.Length } };
                string json = _jserializer.Serialize(data);
                return json;
            }
        }

        [WebMethod]
        public string GetScheduledWorkouts(long gid, string date)
        {
            DateTime datetime = DateTime.Parse(date);
            aqufitEntities entities = new aqufitEntities();
            IQueryable<WOD> wods = entities.WODSchedules.Where(w => w.UserSetting.Id == gid && w.Date.CompareTo(datetime) == 0).OrderBy(w => w.Id).Select(w => w.WOD).Take(10);
            return SerializeWOD(wods);
        }

        [WebMethod]
        public string WODNameCheck(string name)
        {
            aqufitEntities entities = new aqufitEntities();
            WOD test= entities.WODs.FirstOrDefault(w => w.Standard > 0 && string.Compare(w.Name, name, true) == 0);
            JavaScriptSerializer serial = new JavaScriptSerializer();
            if (test == null)
            {   // name is OK
                return serial.Serialize(new { Status = "SUCCESS" });
            }
            else
            {
                return serial.Serialize(new { Status = "ERROR", WODId = test.Id, Name = test.Name });
            }
        }

        /// <summary>
        /// This method is only used when you are going to type a message to someone.  DO NOT PAGE IT ... does not get used by the friend list script
        /// </summary>
        /// <param name="uid">DNN User Key</param>
        /// <param name="pid">DNN Portal Key</param>
        /// <param name="search">string [email, first name, last name, user name]</param>
        /// <returns>stirng json</returns>
        [WebMethod]
        public string searchFriends(long uid, long pid, string search)
        {                       
            aqufitEntities entities = new aqufitEntities();
            UserSettings settings = entities.UserSettings.FirstOrDefault(s => s.UserKey == uid && s.PortalKey == pid);
            IList<long> friendIds = entities.UserFriends.Where(f => (f.SrcUserSettingKey == settings.Id || f.DestUserSettingKey == settings.Id) && f.Relationship == (int)Affine.Utils.ConstsUtil.Relationships.FRIEND).Select(f => (f.SrcUserSettingKey == uid ? f.DestUserSettingKey : f.SrcUserSettingKey)).ToList();
            UserSettings[] firendSettings = entities.UserSettings.Where( us => us.PortalKey == pid ).Where(LinqUtils.BuildContainsExpression<UserSettings, long>(s => s.Id, friendIds)).Where(f => f.UserName.ToLower().Contains(search) || f.UserFirstName.ToLower().Contains(search) || f.UserLastName.ToLower().Contains(search)).ToArray();
            string path = System.Web.VirtualPathUtility.ToAbsolute("~/DesktopModules/ATI_Base/services/images/profile.aspx");
            //object[] response = firendSettings.Select(f => new object[] { "" + f.UserKey, f.UserName + " (" + f.UserFirstName + "," + f.UserLastName + ")", null, "<img src=\"" + path + "?u=" + f.UserKey + "&p="+f.PortalKey+"\" align=\"middle\"/>&nbsp;&nbsp;" + f.UserName + " (" + f.UserFirstName + "," + f.UserLastName + ")" }).ToArray();
            var response = firendSettings.Select(f => new { caption = f.UserName + " (" + f.UserFirstName + "," + f.UserLastName + ")", value = "" + f.Id }).ToArray();
            string json = _jserializer.Serialize(response);
            return json;           
        }

        [WebMethod]
        public string searchUsers(long uSettingsId, string nameOrEmail, int skip , int take)
        {
            take = TakeGaurd(take, 25);
            aqufitEntities entities = new aqufitEntities();
            User you = entities.UserSettings.OfType<User>().FirstOrDefault(u => u.Id == uSettingsId);
            IQueryable<User> userQuery = entities.UserSettings.OfType<User>().Where(us => 
                us.Id != you.Id && 
                us.PortalKey == you.PortalKey && 
                ((us.UserFirstName + us.UserLastName).Contains(nameOrEmail) /* || us.UserEmail.Contains(nameOrEmail) */ || us.UserName.Contains(nameOrEmail)) );
            int length = userQuery.Count();
            UserSettings[] people = userQuery.OrderBy( u => u.UserFirstName ).Skip(skip).Take(take).ToArray();
            Affine.Data.json.UserSetting[] ret = people.OfType<User>().Select(f => new Affine.Data.json.UserSetting()
            {
                Id = f.Id,
                FirstName = f.UserFirstName,
                LastName = f.UserLastName,
                UserKey = f.UserKey,
                PortalKey = f.PortalKey,
                UserName = f.UserName,
                RequestStatus = entities.UserRequestSet.OfType<FriendRequest>().Where(rs => rs.UserSetting.Id == you.Id && rs.FriendRequestSettingsId == f.Id).FirstOrDefault() == null ? -1 : Convert.ToInt32(entities.UserRequestSet.OfType<FriendRequest>().Where(rs => rs.UserSetting.Id == you.Id && rs.FriendRequestSettingsId == f.Id).FirstOrDefault().Status)
            }).ToArray();
            // TODO: not sure about this (for now filter out people you are already friends with )
            //ret = ret.Where(c => c.UserKey != uSettingsId).ToArray();
            var data = new { Data = ret, PagerInfo = new { Skip = skip, Take = take, Length = length } };
            string json = _jserializer.Serialize(data);
            return json; 
        }

        // TODO: take this out of [WebMethod]
        [WebMethod]
        public string SendInviteToContacts(long usid, string[] emails, string txt)
        {
            aqufitEntities entities = new aqufitEntities();
            UserSettings settings = entities.UserSettings.FirstOrDefault( us => us.Id == usid);
            if (settings != null)
            {
                // Log the invites from this user
                foreach (string email in emails)
                {
                    entities.AddToContactInvites(new ContactInvite() { UserSetting = settings, Email = email });
                }
                entities.SaveChanges(); // save changes to db before doing an email send
                // Send the email
                foreach (string email in emails)
                {
                    sendContactEmailAsync(settings, email, txt);                   
                }
                return "{ 'status':'success' }";
            }
            return "{ 'status':'fail' }";
        }

        [WebMethod]
        public string GetCompetitionAthlete(long aid)
        {
            aqufitEntities entities = new aqufitEntities();
            CompetitionAthlete a = entities.CompetitionAthletes.FirstOrDefault(aa => aa.Id == aid);
            JavaScriptSerializer serial = new JavaScriptSerializer();
            return serial.Serialize(new { Name = a.AthleteName, HomeTown = a.Hometown, Img = a.ImgUrl, Country = a.Country, Score = a.OverallScore, Rank = a.OverallRank, Affiliate = a.AffiliateName, Height = a.Height, Weight = a.Weight, Region = a.RegionName });
        }

        public string GetFriendRequests(long uSettingsId)
        {
            // Here we just force people to deal with request ... we DO NOT PAGE THEM..
            const int take = 25;
            aqufitEntities entities = new aqufitEntities();
            long[] friendRequestIds = entities.UserRequestSet.OfType<FriendRequest>().Where(ur => ur.FriendRequestSettingsId == uSettingsId && ur.Status > 0).Select( fr => fr.UserSetting.Id ).ToArray();

            // TODO: we need a cache list of all users on the system
            UserSettings[] firendSettings = entities.UserSettings.Where(LinqUtils.BuildContainsExpression<UserSettings, long>(s => s.Id, friendRequestIds)).OrderBy(f => f.UserName).Take(take).ToArray();
            Affine.Data.json.UserSetting[] ret = firendSettings.Select(f => new Affine.Data.json.UserSetting() { Id = f.Id, FirstName = f.UserFirstName, LastName = f.UserLastName, UserKey = f.UserKey, PortalKey = f.PortalKey, UserName = f.UserName, RequestStatus = f.UserRequests.OfType<FriendRequest>().FirstOrDefault(rs => rs.FriendRequestSettingsId == uSettingsId) == null ? -1 : Convert.ToInt32(f.UserRequests.OfType<FriendRequest>().FirstOrDefault(rs => rs.FriendRequestSettingsId == uSettingsId).Status) }).ToArray();
            int len = ret.Count();
            var data = new { Data = ret, PagerInfo = new { Skip = 0, Take = len, Length = len } };
            string json = _jserializer.Serialize(data);
            return json;
        }       


        public string FollowUser(long UserSettingsId, long fid)
        {
            // ok so the user has accepted the friend request. ( Followed user is always DestUserKey )
            // 1) Lets add the user to the firends
            aqufitEntities entities = new aqufitEntities();
            // Make sure that this not the same user
            if (UserSettingsId == fid)
            {
                return "{ 'status':'invalid' }";
            }
            // make sure it is not already in the db
            UserSettings you = entities.UserSettings.FirstOrDefault(s => s.Id == UserSettingsId);
            UserSettings friend = entities.UserSettings.FirstOrDefault(s => s.Id == fid );
            // check if they are already friends or in the watch list
            UserFriends uf = entities.UserFriends.FirstOrDefault(f => f.SrcUserSettingKey == UserSettingsId && f.DestUserSettingKey == fid || f.SrcUserSettingKey == fid && f.DestUserSettingKey == UserSettingsId);
            if (uf != null && uf.Relationship == (int)Affine.Utils.ConstsUtil.Relationships.FRIEND )
            {   // you can not "follow" your friends...
                return "{ 'status':'invalid' }";
            }
            else if (uf != null && uf.SrcUserSettingKey == UserSettingsId && uf.Relationship == (int)Affine.Utils.ConstsUtil.Relationships.FOLLOW)
            {   // you are already following... 
                throw new Exception("You are already following this user.");
            }
            UserFriends uf2 = new UserFriends()
            {
                SrcUserSettingKey = you.Id,
                DestUserSettingKey = friend.Id,
                PortalKey = you.PortalKey,
                Relationship = (int)Affine.Utils.ConstsUtil.Relationships.FOLLOW 
            };
            entities.AddToUserFriends(uf2);
            entities.SaveChanges();

            // Add the Num people you follow metric
            Metric met = entities.Metrics.FirstOrDefault(m => m.UserSetting.Id == you.Id && m.MetricType == (int)Affine.Utils.MetricUtil.MetricType.NUM_YOU_FOLLOW);
            if (met != null)
            {
                met.MetricValue = "" + (Convert.ToInt32(met.MetricValue) + 1);
            }
            else
            {
                met = new Metric()
                {
                    MetricType = (int)Affine.Utils.MetricUtil.MetricType.NUM_YOU_FOLLOW,
                    MetricValue = "1",
                    UserSetting = you
                };
                entities.AddToMetrics(met);
            }
            // Add the Number of follwers to the other user metric
            Metric met2 = entities.Metrics.FirstOrDefault(m => m.UserSetting.Id == friend.Id && m.MetricType == (int)Affine.Utils.MetricUtil.MetricType.NUM_FOLLOWERS);
            if (met2 != null)
            {
                met2.MetricValue = "" + (Convert.ToInt32(met2.MetricValue) + 1);
            }
            else
            {
                met2 = new Metric()
                {
                    MetricType = (int)Affine.Utils.MetricUtil.MetricType.NUM_FOLLOWERS,
                    MetricValue = "1",
                    UserSetting = friend
                };
                entities.AddToMetrics(met2);
            }
            entities.SaveChanges();

            // TODO:
            //sendUserFollowEmailAsync(src, dst);

            return "{ 'status':'success' }";
        }

        public string UnFollowUser(long uid, long pid, long fid)
        {
            aqufitEntities entities = new aqufitEntities();
            UserSettings you = entities.UserSettings.FirstOrDefault(s => s.UserKey == uid && s.PortalKey == pid);
            UserSettings friend = entities.UserSettings.FirstOrDefault(s => s.UserKey == fid && s.PortalKey == pid);
            UserFriends uf = entities.UserFriends.FirstOrDefault(f => f.SrcUserSettingKey == you.Id && f.DestUserSettingKey == friend.Id && f.Relationship == (int)Affine.Utils.ConstsUtil.Relationships.FOLLOW);
            if (uf != null)
            {
                entities.DeleteObject(uf);
                entities.SaveChanges();
            }

            // Add the Num people you follow metric
            Metric met = entities.Metrics.FirstOrDefault(m => m.UserSetting.Id == you.Id && m.MetricType == (int)Affine.Utils.MetricUtil.MetricType.NUM_YOU_FOLLOW);
            if (met != null)
            {
                met.MetricValue = "" + (Convert.ToInt32(met.MetricValue) - 1);
            }            
            // Add the Number of follwers to the other user metric
            Metric met2 = entities.Metrics.FirstOrDefault(m => m.UserSetting.Id == friend.Id && m.MetricType == (int)Affine.Utils.MetricUtil.MetricType.NUM_FOLLOWERS);
            if (met2 != null)
            {
                met2.MetricValue = "" + (Convert.ToInt32(met2.MetricValue) - 1);
            }           
            entities.SaveChanges();

            return "{ 'status':'success' }";
        }        

        [WebMethod]
        public string GetAllExercises()
        {
            aqufitEntities entities = new aqufitEntities();
            Affine.Data.json.Exercise[] exerciseList = entities.Exercises.Select(e => new Affine.Data.json.Exercise() { Id = e.Id, Name = e.Name, AuthorKey = e.UserSetting.UserKey, HasDistance = e.HasDistance, HasWeight = e.HasWeight }).OrderBy( e => e.Name ).ToArray();
            string json = _jserializer.Serialize(exerciseList);
            return json;
        }

        public string SaveStreamShout(long gid, long uid, long pid, string txt)
        {
            aqufitEntities entities = new aqufitEntities();
            UserSettings settings = entities.UserSettings.FirstOrDefault(us => us.UserKey == uid && us.PortalKey == pid);
            // create a new workout record
            Shout shout = new Shout()
            {
                PortalKey = pid,
                UserSetting = settings,
                Date = DateTime.Now.ToUniversalTime(),
                Description = txt,
                TimeStamp = DateTime.Now.ToUniversalTime(),
               
            };
            if (gid > 0)
            {
                // TODO: check that the user is an admin of the group..
                Group group = entities.UserSettings.OfType<Group>().FirstOrDefault(g => g.Id == gid);
                if (group != null)
                {
                    shout.GroupKey = gid;
                    shout.UserSetting = group;
                    settings = group;
                }

            }
            entities.AddToUserStreamSet(shout);
            entities.SaveChanges();

            // Need the shout ID so query for it
            Shout shoutRet = entities.UserStreamSet.OfType<Shout>().Where(s => s.UserSetting.UserKey == settings.UserKey).OrderByDescending(u => u.Id).FirstOrDefault();
            // get the json serializable version of the object.
            Affine.Data.json.StreamData sd = _IStreamManager.UserStreamEntityToStreamData(shoutRet, null);
            // RadAjaxManager1.ResponseScripts.Add(" (function(){ Aqufit.Page.Controls.atiStreamPanel.prependJson(" + _jserializer.Serialize(sd) + "); })();"); 
            return _jserializer.Serialize(sd); 
        }


        public string SendMessage(long usid, long[] toArray, string subject, string txt)
        {
            aqufitEntities entities = new aqufitEntities();
            if (string.IsNullOrEmpty(subject))
            {
                subject = "(No Subject)";
            }
            subject = Utils.Web.WebUtils.MakeWebSafeString(subject);
            txt = Utils.Web.WebUtils.MakeWebSafeString(txt);
            string shortTxt = Utils.Web.WebUtils.MakeWebSafeString( txt.Length > 128 ? txt.Substring(0, 128) + "..." : txt );
            DateTime dt = DateTime.Now.ToUniversalTime();
            UserSettings settings = entities.UserSettings.FirstOrDefault(us => us.Id == usid);
            Message message = new Message()
            {
                UserSetting = settings,
                PortalKey = settings.PortalKey,
                Status = 0, // TODO: make nice message status ( 0 = unread )
                DateTime = dt,
                ParentKey = 0, // This is not a reply so set parent to 0
                Subject = subject,
                Text = txt,
                LastText = shortTxt,
                LastDateTime = dt,
                LastUserKey = settings.Id,
                LastUserName = settings.UserName,
                SecondDateTime = dt,
                SecondText = shortTxt,
                SecondUserKey = settings.Id,
                SecondUserName = settings.UserName
            };

            MessageRecipiant recipiant2 = new MessageRecipiant()
            {
                UserSettingsKey = settings.Id,
                Message = message
            };

            message.MessageRecipiants.Add(recipiant2);
            entities.AddToMessages(message);

            IList<UserFriends> friendList = new List<UserFriends>();
            UserSettings you = settings;
            foreach (long tid in toArray)
            {
                // First thing we want to do here is to varify that the people are really friends
                UserFriends friend = entities.UserFriends.FirstOrDefault(f => ((f.SrcUserSettingKey == you.Id && f.DestUserSettingKey == tid) || (f.SrcUserSettingKey == tid && f.DestUserSettingKey == you.Id)) && f.Relationship == (int)Affine.Utils.ConstsUtil.Relationships.FRIEND  );
                if (friend != null)
                {
                    friendList.Add(friend);
                    long toId = settings.Id == friend.DestUserSettingKey ? friend.SrcUserSettingKey : friend.DestUserSettingKey;
                    MessageRecipiant recipiant = new MessageRecipiant()
                    {
                        UserSettingsKey = toId,
                        Message = message
                    };
                    message.MessageRecipiants.Add(recipiant);
                    // create a notification Publish setting 3 so does not show in stream...                   
                    User toUser = entities.UserSettings.OfType<User>().FirstOrDefault(us => us.Id == toId);
                   
                    Notification workoutNotification = new Notification()
                    {
                        PortalKey = toUser.PortalKey,
                        UserSetting = toUser,
                        Date = DateTime.Now.ToUniversalTime(),
                        Title = settings.UserName +" has sent you a message.",
                        Description = settings.UserName +" ("+ settings.UserFirstName + " " + settings.UserLastName + ") has sent you a message.",
                        TimeStamp = DateTime.Now.ToUniversalTime(),
                        NotificationType = (int)Affine.Utils.ConstsUtil.NotificationTypes.NEW_MESSAGE,
                        PublishSettings = (int) Affine.Utils.ConstsUtil.PublishSettings.NO_STREAM,
                        Message = message
                    };
                    entities.AddToUserStreamSet(workoutNotification);  
                   
                }
            }
            entities.SaveChanges();
            sendEmailAsync(message);
            return "{ 'status':'success' }";
        }


        public long SaveRecipe(long uid, long pid, Affine.Data.json.StreamData recipe)
        {            
            aqufitEntities entities = new aqufitEntities();
            UserSettings settings = entities.UserSettings.FirstOrDefault(us => us.UserKey == uid && us.PortalKey == pid);     
            Recipe r = null;
            RecipeExtended re = null;
            if (recipe.Id > 0)
            {
                r = entities.UserStreamSet.OfType<Recipe>().Include("RecipeExtendeds").FirstOrDefault(us => us.UserSetting.UserKey == uid && us.PortalKey == pid && us.Id == recipe.Id);
                re = r.RecipeExtendeds.First();
                RecipeIngredient[] riOld = entities.RecipeIngredients.Where(ri => ri.RecipeExtended.Id == re.Id).ToArray();
                foreach (RecipeIngredient ri in riOld)
                {
                    entities.DeleteObject(ri);  // delete and re add new ing
                }
            }else{
                r = new Recipe();
                re = new RecipeExtended();
                re.NumRatings = 1;
                re.NumStrictness = 1;
                re.UserStream = r;
                entities.AddToUserStreamSet(r);
                r.Date = DateTime.Now.ToUniversalTime();      
            }               
            r.PortalKey = pid;
            r.UserSetting = settings;            
            r.TimeStamp = DateTime.Now.ToUniversalTime();
            r.Title = recipe.Title;
            r.Description = recipe.Description;
            r.AvrStrictness = recipe.AvrStrictness;
            r.AvrRating = recipe.AvrRating;
            r.NumServings = recipe.NumServings;
            r.Tags = recipe.Tags;
            r.TimeCook = recipe.TimeCook;
            r.TimePrep = recipe.TimePrep;
            re.Directions = recipe.RecipeExtended.Directions;
            RecipeIngredient[] riArray = recipe.RecipeExtended.Ingredients.Select(i => new RecipeIngredient() { Text = i.Name, RecipeExtended = re }).ToArray();                       
            entities.SaveChanges();            
            
            // Update the metrics for this user
            if (recipe.Id == 0) // if thie is NOT an edit
            {
                Metric met = entities.Metrics.FirstOrDefault(m => m.UserSetting.UserKey == uid && m.UserSetting.PortalKey == pid && m.MetricType == (int)Affine.Utils.MetricUtil.MetricType.NUM_RECIPES);
                if (met == null)
                {
                    met = new Metric()
                    {
                        UserSetting = settings,
                        MetricType = (int)Affine.Utils.MetricUtil.MetricType.NUM_RECIPES,
                        MetricValue = "1"       // This is the first recipe they have saved                   
                    };
                    entities.AddToMetrics(met);
                }
                else
                {
                    met.MetricValue = "" + (Convert.ToInt32(met.MetricValue) + 1);
                }
                entities.SaveChanges();
                UserStream ret = entities.UserStreamSet.OfType<Recipe>().Where(u => u.PortalKey == pid && u.UserSetting.UserKey == uid).OrderByDescending(u => u.Id).FirstOrDefault();
                return ret.Id; 
               
            }
            return recipe.Id;                                 
        }


        public static string ResolveUrl(string originalUrl)
        {
            if (originalUrl == null)
                return null;

            // *** Absolute path - just return
            if (originalUrl.IndexOf("://") != -1)
                return originalUrl;

            // *** Fix up image path for ~ root app dir directory
            if (originalUrl.StartsWith("~"))
            {
                string newUrl = "";
                if (HttpContext.Current != null)
                    newUrl = HttpContext.Current.Request.ApplicationPath.Substring(1) +
                            originalUrl.Substring(1).Replace("//", "/");
                else
                    // *** Not context: assume current directory is the base directory
                    throw new ArgumentException("Invalid URL: Relative URL not allowed.");
                // *** Just to be sure fix up any double slashes
                return newUrl;

            }
            return originalUrl;

        }



#region Load On Demand 

        [WebMethod]
        public RadComboBoxData GetStandardWorkoutsOnDemand(RadComboBoxContext context)
        {
            List<RadComboBoxItemData> result = new List<RadComboBoxItemData>(context.NumberOfItems);
            RadComboBoxData comboData = new RadComboBoxData();
            try
            {
                int itemsPerRequest = 10;
                int itemOffset = context.NumberOfItems;
                int endOffset = itemOffset + itemsPerRequest;

                long ProfileUserSettingsId = Convert.ToInt64(context["UserSettingsId"]);
                aqufitEntities entities = new aqufitEntities();

                UserSettings UserSettings = entities.UserSettings.FirstOrDefault(u => u.Id == ProfileUserSettingsId);                
                IQueryable<WOD> wods = entities.WODs.Where(w => w.Standard > 0);
                wods.Select(w => w.WODType).ToArray();  // hydrate WODTypes
              
                string lowerTxt = context.Text.ToLower();
                if (!string.IsNullOrWhiteSpace(context.Text))
                {
                    wods = wods.Where(w => w.Name.ToLower().Contains(lowerTxt) || w.WODSchedules.Where(ws => ws.HideTillDate.HasValue && DateTime.Now.CompareTo(ws.HideTillDate.Value) < 0).Any(ws => ws.HiddenName.ToLower().Contains(lowerTxt))).OrderBy(w => w.Name);
                }
                else
                {
                    wods = wods.OrderByDescending(w => w.CreationDate);
                }
                int length = wods.Count();
                wods = wods.Skip(itemOffset).Take(itemsPerRequest);
                WOD[] wodList = wods.ToArray();
                for (int i = 0; i < wodList.Length; i++)
                {
                    WOD w = wodList[i];
                    if (w.WODSchedules != null && w.WODSchedules.Count > 0)
                    {
                        WODSchedule ws = w.WODSchedules.OrderByDescending(s => s.HideTillDate).First();
                        if (ws.HideTillDate.HasValue && DateTime.Now.CompareTo(ws.HideTillDate.Value) < 0)
                        {
                            if (string.IsNullOrWhiteSpace(context.Text))
                            {
                                result.Add(new RadComboBoxItemData() { Text = Affine.Utils.Web.WebUtils.FromWebSafeString(ws.HiddenName), Value = "{ 'Id':" + w.Id + ", 'Type':" + w.WODType.Id + "}" });
                                if (w.Standard > 0 || w.WODSchedules.Count > 1)
                                {   // CA - here is what is going on here.  If the workout is suppost to be hidden until a date (common for crossfit gyms) then we just put a date name like up
                                    // top.  But if it is a standard WOD (or a wod that they have done before (w.WODSchedules.Count > 1) then we still need to add the WOD
                                    result.Add(new RadComboBoxItemData() { Text = Affine.Utils.Web.WebUtils.FromWebSafeString(w.Name), Value = "{ 'Id':" + w.Id + ", 'Type':" + w.WODType.Id + "}" });
                                }
                            }
                            else if (ws.HiddenName.ToLower().StartsWith(lowerTxt))
                            {
                                result.Add(new RadComboBoxItemData() { Text = Affine.Utils.Web.WebUtils.FromWebSafeString(ws.HiddenName), Value = "{ 'Id':" + w.Id + ", 'Type':" + w.WODType.Id + "}" });
                            }
                        }
                        else
                        {
                            result.Add(new RadComboBoxItemData() { Text = Affine.Utils.Web.WebUtils.FromWebSafeString(w.Name), Value = "{ 'Id':" + w.Id + ", 'Type':" + w.WODType.Id + "}" });
                        }
                    }
                    else
                    {
                        result.Add(new RadComboBoxItemData() { Text = Affine.Utils.Web.WebUtils.FromWebSafeString(w.Name), Value = "{ 'Id':" + w.Id + ", 'Type':" + w.WODType.Id + "}" });
                    }
                }
                if (endOffset > length)
                {
                    endOffset = length;
                }
                if (endOffset == length)
                {
                    comboData.EndOfItems = true;
                }
                else
                {
                    comboData.EndOfItems = false;
                }
                if (length > 0)
                {
                    comboData.Message = String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", endOffset.ToString(), length);
                }
                else
                {
                    comboData.Message = "No matches";
                }
                comboData.Items = result.ToArray();
            }
            catch (Exception ex)
            {
                comboData.Message = ex.Message;
            }
            return comboData;
        }

        [WebMethod]
        public RadComboBoxData GetExerciseListOnDemand(RadComboBoxContext context)
        {
            List<RadComboBoxItemData> result = new List<RadComboBoxItemData>(context.NumberOfItems);
            RadComboBoxData comboData = new RadComboBoxData();
            try
            {
                int itemsPerRequest = 15;
                int itemOffset = context.NumberOfItems;
                int endOffset = itemOffset + itemsPerRequest;

                aqufitEntities entities = new aqufitEntities();

                IQueryable<Exercise> erercisesQuery = string.IsNullOrEmpty(context.Text) ?
                        entities.Exercises.OrderBy(e => e.Name) :
                        entities.Exercises.Where(e => e.Name.ToLower().StartsWith(context.Text)).OrderBy(e => e.Name);
                int length = erercisesQuery.Count();
                Exercise[] exercises = erercisesQuery.Skip(itemOffset).Take(itemsPerRequest).ToArray();
                foreach (Exercise exer in exercises)
                {
                    RadComboBoxItemData item = new RadComboBoxItemData();
                    item.Text = exer.Name;
                    item.Value = "" + exer.Id;
                    result.Add(item);
                }
                if (endOffset > length)
                {
                    endOffset = length;
                }
                if (endOffset == length)
                {
                    comboData.EndOfItems = true;
                }
                else
                {
                    comboData.EndOfItems = false;
                }
                if (length > 0)
                {
                    comboData.Message = String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", endOffset.ToString(), length);
                }
                else
                {
                    comboData.Message = "No matches";
                }
                comboData.Items = result.ToArray();
            }
            catch (Exception ex)
            {
                comboData.Message = ex.Message;
            }
            return comboData;
        }

        [WebMethod]
        public RadComboBoxData GetWorkoutsOnDemand(RadComboBoxContext context)
        {
            List<RadComboBoxItemData> result = new List<RadComboBoxItemData>(context.NumberOfItems);
            RadComboBoxData comboData = new RadComboBoxData();
            try
            {
                int itemsPerRequest = 10;
                int itemOffset = context.NumberOfItems;
                int endOffset = itemOffset + itemsPerRequest;

                long ProfileUserSettingsId = Convert.ToInt64(context["UserSettingsId"]);
                aqufitEntities entities = new aqufitEntities();

                UserSettings UserSettings = entities.UserSettings.FirstOrDefault(u => u.Id == ProfileUserSettingsId);
                if (itemOffset == 0)
                {
                    RadComboBoxItemData item = new RadComboBoxItemData();
                    item.Text = "Create a New Workout";
                    item.Value = "{'Id':0, 'Type':'0'}";
                    result.Add(item);
                }
                IQueryable<WOD> wods = entities.User2WODFav.Where(w => w.UserSetting.Id == UserSettings.Id).Select(w => w.WOD);
                wods = wods.Union<WOD>(entities.WODs.Where(w => w.Standard > 0));
                wods.Select(w => w.WODType).ToArray();  // hydrate WODTypes

                long[] groupIds = null;
                groupIds = entities.UserFriends.Where(f => (f.SrcUserSettingKey == UserSettings.Id || f.DestUserSettingKey == UserSettings.Id) && f.Relationship >= (int)Affine.Utils.ConstsUtil.Relationships.GROUP_OWNER).Select(f => f.SrcUserSettingKey == UserSettings.Id ? f.DestUserSettingKey : f.SrcUserSettingKey).ToArray();
                // OK this is a bit of a trick... this query hydrates only the "WODSchedule" in the above WOD query.. so we will get the wods we are looking for..
                IEnumerable<WODSchedule>[] workoutSchedule = entities.UserSettings.OfType<Group>().Where(LinqUtils.BuildContainsExpression<UserSettings, long>(us => us.Id, groupIds)).Select(g => g.WODSchedules.Where(ws => ws.HideTillDate.HasValue && DateTime.Now.CompareTo(ws.HideTillDate.Value) < 0)).ToArray();

                string lowerTxt = context.Text.ToLower();
                if (!string.IsNullOrWhiteSpace(context.Text))
                {
                    wods = wods.Where(w => w.Name.ToLower().Contains(lowerTxt) || w.WODSchedules.Where(ws => ws.HideTillDate.HasValue && DateTime.Now.CompareTo(ws.HideTillDate.Value) < 0).Any(ws => ws.HiddenName.ToLower().Contains(lowerTxt))).OrderBy(w => w.Name);
                }
                else
                {
                    wods = wods.OrderByDescending(w => w.CreationDate);
                }
                int length = wods.Count();
                wods = wods.Skip(itemOffset).Take(itemsPerRequest);
                WOD[] wodList = wods.ToArray();
                for (int i = 0; i < wodList.Length; i++)
                {
                    WOD w = wodList[i];
                    if (w.WODSchedules != null && w.WODSchedules.Count > 0)
                    {
                        WODSchedule ws = w.WODSchedules.OrderByDescending(s => s.HideTillDate).First();
                        if (ws.HideTillDate.HasValue && DateTime.Now.CompareTo(ws.HideTillDate.Value) < 0)
                        {
                            if (string.IsNullOrWhiteSpace(context.Text))
                            {
                                result.Add(new RadComboBoxItemData() { Text = Affine.Utils.Web.WebUtils.FromWebSafeString(ws.HiddenName), Value = "{ 'Id':" + w.Id + ", 'Type':" + w.WODType.Id + "}" });
                                if (w.Standard > 0 || w.WODSchedules.Count > 1)
                                {   // CA - here is what is going on here.  If the workout is suppost to be hidden until a date (common for crossfit gyms) then we just put a date name like up
                                    // top.  But if it is a standard WOD (or a wod that they have done before (w.WODSchedules.Count > 1) then we still need to add the WOD
                                    result.Add(new RadComboBoxItemData() { Text = Affine.Utils.Web.WebUtils.FromWebSafeString(w.Name), Value = "{ 'Id':" + w.Id + ", 'Type':" + w.WODType.Id + "}" });
                                }
                            }
                            else if (ws.HiddenName.ToLower().StartsWith(lowerTxt))
                            {
                                result.Add(new RadComboBoxItemData() { Text = Affine.Utils.Web.WebUtils.FromWebSafeString(ws.HiddenName), Value = "{ 'Id':" + w.Id + ", 'Type':" + w.WODType.Id + "}" });
                            }
                        }
                        else
                        {
                            result.Add(new RadComboBoxItemData() { Text = Affine.Utils.Web.WebUtils.FromWebSafeString(w.Name), Value = "{ 'Id':" + w.Id + ", 'Type':" + w.WODType.Id + "}" });
                        }
                    }
                    else
                    {
                        result.Add(new RadComboBoxItemData() { Text = Affine.Utils.Web.WebUtils.FromWebSafeString(w.Name), Value = "{ 'Id':" + w.Id + ", 'Type':" + w.WODType.Id + "}" });
                    }
                }               
                if (endOffset > length)
                {
                    endOffset = length;
                }
                if (endOffset == length)
                {
                    comboData.EndOfItems = true;
                }
                else
                {
                    comboData.EndOfItems = false;
                }
                if (length > 0)
                {
                    comboData.Message = String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", endOffset.ToString(), length);
                }
                else
                {
                    comboData.Message = "No matches";
                }
                comboData.Items = result.ToArray();
            }
            catch (Exception ex)
            {
                comboData.Message = ex.Message;
            }
            return comboData;
        }


        [WebMethod]
        public RadComboBoxData GetRoutesOnDemand(RadComboBoxContext context)
        {
            List<RadComboBoxItemData> result = new List<RadComboBoxItemData>(context.NumberOfItems);
            RadComboBoxData comboData = new RadComboBoxData();
            try
            {
                int itemsPerRequest = 10;
                int itemOffset = context.NumberOfItems;
                int endOffset = itemOffset + itemsPerRequest;

                long ProfileUserSettingsId = Convert.ToInt64(context["UserSettingsId"]);
                aqufitEntities entities = new aqufitEntities();

                UserSettings UserSettings = entities.UserSettings.FirstOrDefault(u => u.Id == ProfileUserSettingsId);
                IQueryable<MapRoute> mapRoutesQuery = string.IsNullOrEmpty(context.Text) ?
                        entities.User2MapRouteFav.Include("MapRoutes").Where(r => r.UserSettingsKey == ProfileUserSettingsId).Select(r => r.MapRoute).OrderBy(w => w.Name) :
                        entities.User2MapRouteFav.Include("MapRoutes").Where(r => r.UserSettingsKey == ProfileUserSettingsId).Select(r => r.MapRoute).Where(r => r.Name.ToLower().StartsWith(context.Text)).OrderBy(r => r.Name);
                int length = mapRoutesQuery.Count();
                MapRoute[] mapRoutes = mapRoutesQuery.Skip(itemOffset).Take(itemsPerRequest).ToArray();                
                string mapIcon = ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iMap.png");
                if (itemOffset == 0)
                {
                    RadComboBoxItemData item = new RadComboBoxItemData();
                    item.Text = "<img src=\"" + mapIcon + "\" /> Add New Map";
                    item.Value = "{'Id':0, 'Dist':'0'}";
                    result.Add(item);
                }
                Affine.Utils.UnitsUtil.MeasureUnit unit = UserSettings.DistanceUnits != null ? Affine.Utils.UnitsUtil.ToUnit(Convert.ToInt32(UserSettings.DistanceUnits)) : Affine.Utils.UnitsUtil.MeasureUnit.UNIT_MILES;
                string unitName = Affine.Utils.UnitsUtil.unitToStringName(unit);
                foreach (MapRoute mr in mapRoutes)
                {
                    double dist = Affine.Utils.UnitsUtil.systemDefaultToUnits(mr.RouteDistance, unit);
                    dist = Math.Round(dist, 2);
                    RadComboBoxItemData item = new RadComboBoxItemData();
                    item.Text = "<img src=\"" + Affine.Utils.ImageUtil.GetGoogleMapsStaticImage(mr, 200, 150) + "\" />" + Affine.Utils.Web.WebUtils.FromWebSafeString(mr.Name) + " (" + dist + " " + unitName + ")";
                    item.Value = "{ 'Id':" + mr.Id + ", 'Dist':" + mr.RouteDistance + "}";
                    result.Add(item);
                }                
                if (endOffset > length)
                {
                    endOffset = length;
                }
                if (endOffset == length)
                {
                    comboData.EndOfItems = true;
                }
                else
                {
                    comboData.EndOfItems = false;
                }
                if (length > 0)
                {
                    comboData.Message = String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", endOffset.ToString(), length);
                }
                else
                {
                    comboData.Message = "No matches";
                }
                comboData.Items = result.ToArray();
            }
            catch (Exception ex)
            {
                comboData.Message = ex.Message;
            }           
            return comboData;
        }


        [WebMethod]
        public RadComboBoxData GetGroupSearch(RadComboBoxContext context)
        {
            List<RadComboBoxItemData> result = new List<RadComboBoxItemData>(context.NumberOfItems);
            RadComboBoxData comboData = new RadComboBoxData();
            if (!string.IsNullOrWhiteSpace(context.Text))
            {
                try
                {
                    const int TAKE = 15;
                    aqufitEntities entities = new aqufitEntities();
                    int itemOffset = context.NumberOfItems;
                    IQueryable<Group> friends = entities.UserSettings.OfType<Group>().OrderBy(w => w.UserName);
                    friends = friends.Where(w => w.UserName.ToLower().Contains(context.Text) || w.UserFirstName.ToLower().Contains(context.Text));
                    int length = friends.Count();
                    friends = friends.Skip(itemOffset).Take(TAKE);
                    Group[] groups = friends.ToArray();

                    foreach (Group g in groups)
                    {
                        RadComboBoxItemData item = new RadComboBoxItemData();
                        item.Text = g.UserFirstName;
                        item.Value = " { 'Address': '', 'GroupKey':" + g.Id + ", 'Lat':" + g.DefaultMapLat + ", 'Lng':" + g.DefaultMapLng + " , 'Name':'" + g.UserFirstName + "', 'UserName':'" + g.UserName.Replace("'", "") + "', 'UserKey':" + g.UserKey + ", 'ImageId':0, 'Description':'' }";
                        //   item.ImageUrl = ResolveUrl("~/DesktopModules/ATI_Base/services/images/profile.aspx") + "?u=" + g.UserKey + "&p=" + g.PortalKey;
                        result.Add(item);
                    }
                    int endOffset = Math.Min(itemOffset + TAKE + 1, length);
                    if (endOffset > length)
                    {
                        endOffset = length;
                    }
                    if (endOffset == length)
                    {
                        comboData.EndOfItems = true;
                    }
                    else
                    {
                        comboData.EndOfItems = false;
                    }
                    if (length > 0)
                    {
                        comboData.Message = String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", endOffset.ToString(), length);
                    }
                    else
                    {
                        comboData.Message = "No matches";
                    }
                    comboData.Items = result.ToArray();
                }
                catch (Exception ex)
                {
                    comboData.Message = ex.Message;
                }
            }
            else
            {
                comboData.Message = "Type to search";
            }
            return comboData;
            
        }

        [WebMethod]
        public RadComboBoxData GetFlexFWDSearch(RadComboBoxContext context)
        {
            List<RadComboBoxItemData> result = new List<RadComboBoxItemData>(context.NumberOfItems);
            RadComboBoxData comboData = new RadComboBoxData();
            if (!string.IsNullOrWhiteSpace(context.Text))
            {
                try
                {
                    int itemsPerRequest = 10;
                    int itemOffset = context.NumberOfItems;
                    int endOffset = itemOffset + itemsPerRequest;

                    long ProfileUserSettingsId = Convert.ToInt64(context["UserSettingsId"]);
                    aqufitEntities entities = new aqufitEntities();

                    //UserSettings profileSettings = entities.UserSettings.OfType<User>().FirstOrDefault(u => u.UserKey == PortalSettings.Current.UserId && u.PortalKey == PortalSettings.Current.PortalId);
                    IQueryable<UserSettings> friends = entities.UserSettings.Where(u => u.PortalKey == PortalSettings.Current.PortalId).OrderBy(w => w.UserName);
                    friends = friends.Where(w => w.UserName.ToLower().StartsWith(context.Text) || w.UserFirstName.ToLower().StartsWith(context.Text) || w.UserLastName.ToLower().StartsWith(context.Text));
                    int length = friends.Count();
                    var users = friends.Skip(itemOffset).Take(itemsPerRequest).Select(u => new {Id = u.Id, Type = (u is User ? "User" : "Group"), UserKey = u.UserKey, PortalKey = u.PortalKey, UserFirstName = u.UserFirstName, UserLastName = u.UserLastName, UserName = u.UserName }).ToArray();
                    foreach (var u in users)
                    {
                        RadComboBoxItemData item = new RadComboBoxItemData();
                        if (u.Type == "User")
                        {
                            item.Text = "<img style=\"float: left;\" src=\"" + StreamService.ResolveUrl("~/DesktopModules/ATI_Base/services/images/profile.aspx") + "?u=" + u.UserKey + "&p=" + u.PortalKey + "\" /><span class=\"atiTmItem\">" + u.UserName + "<br /> (" + u.UserFirstName + " " + u.UserLastName + ")</span>";
                            item.Value = "{ Type:'USER', Val:'" + u.UserName + "'}";
                        }
                        else
                        {
                            item.Text = "<img style=\"float: left;\" src=\"" + StreamService.ResolveUrl("~/DesktopModules/ATI_Base/services/images/profile.aspx") + "?us=" + u.Id + "\" /><span class=\"atiTmItem\">" + u.UserFirstName + "</span>";
                            item.Value = "{ Type:'GROUP', Val:'" + u.UserName + "'}";
                        }
                        result.Add(item);
                    }

                    if (endOffset > length)
                    {
                        endOffset = length;
                    }
                    if (endOffset == length)
                    {
                        comboData.EndOfItems = true;
                    }
                    else
                    {
                        comboData.EndOfItems = false;
                    }
                    if (length > 0)
                    {
                        comboData.Message = String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", endOffset.ToString(), length);
                    }
                    else
                    {
                        comboData.Message = "No matches";
                    }
                    comboData.Items = result.ToArray();
                }
                catch (Exception ex)
                {
                    comboData.Message = ex.Message;
                }
            }
            else
            {
                comboData.Message = "Type to search";
            }
            return comboData;
        }

#endregion
    }
}
