using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Serialization;

using DotNetNuke.Common.Lists;
using DotNetNuke.Entities.Users;

using Affine.Data;

//using Myren.GeoNames.Client;


namespace Affine.WebService
{
    public class RegionalInfo
    {
        public RegionalInfo()
        {
            this.TextArray = new List<string>();
            this.ValueArray = new List<string>();
        }
        public IList<string> TextArray { get; set; }
        public IList<string> ValueArray { get; set; }
        public string RegionText { get; set; }
        public string PostalText { get; set; }        
        public double Lat { get; set; }
        public double Lng { get; set; }
    }

    /// <summary>
    /// Summary description for RegisterService
    /// </summary>
    [WebService(Namespace = "http://aqufit.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class RegisterService : System.Web.Services.WebService
    {

        public RegisterService()
        {

            //Uncomment the following line if using designed components 
            //InitializeComponent(); 
        }

        [WebMethod]
        public bool UserNameExists(string UserName, int pid)
        {
            UserInfo objUserInfo = UserController.GetUserByName(pid, UserName.ToLower());
            if (objUserInfo != null)
            {
                return true;
            }
            return false;
        }
        
        [WebMethod]
        public RegionalInfo GetRegionsFromCountry(string countryCode)
        {
            RegionalInfo ri = new RegionalInfo();
            ListController ctlEntry = new ListController();         // listKey in format "Country.US:Region"
            string listKey = "Country." + countryCode;
            ListEntryInfoCollection entryCollection = ctlEntry.GetListEntryInfoCollection("Region", listKey);

            if (entryCollection.Count != 0)
            {
                foreach (ListEntryInfo lei in entryCollection)
                {
                    ri.TextArray.Add(lei.Text);
                    ri.ValueArray.Add(lei.Value);
                }
                switch (countryCode)
                {
                    case "US":
                        ri.PostalText = "Zip";
                        ri.RegionText = "State";
                        break;
                    case "CA":
                        ri.RegionText = "Province";
                        ri.PostalText = "Postal";
                        break;
                }
            }
            else
            {
                ri.RegionText = "Region";
                ri.PostalText = "Postal";
            }
            return ri;
        }


        public UserInfo InitialiseUser(int PortalId)
        {
            UserInfo newUser = new UserInfo();
            // Initialise the ProfileProperties Collection
            newUser.Profile.InitialiseProfile(PortalId);
            return newUser;
        }
        /*
        [WebMethod]
        public string GetRegionInfoFromGeoCode(string postal)
        {
            GeoNamesClient client = new GeoNamesClient();
            GeoPostalCodeListResponse list = client.SearchPostalCodes(new GeoPostalCodeSearchCriteria() { PostalCode = postal });
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize( list.PostalCodes.Select(p => new { CountryCode = p.CountryCode, PlaceName = p.PlaceName, PostalCode = p.PostalCode }).ToArray() );
        }

        public double GetTimeZone(double lat, double lng)
        {
            GeoNamesClient client = new GeoNamesClient();
            GeoTimeZone gt = client.GetTimeZone(lat, lng);
            return gt.GmtOffset;
        }

        public string GetRegionInfoFromGeoCode(double lat, double lng)
        {
            GeoNamesClient client = new GeoNamesClient();
            double n, s, e, w;
            client.GetBoundingBox(lat, lng, 1, out n, out s, out e, out w);
            GeoListResponse<GeoCity> cities = client.GetCities(n, s, e, w);
            foreach (GeoCity city in cities.Items)
            {
                string test = city.Name;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(cities.Items.Select(c => new { Name = c.Name, Country = c.CountryName, TimeZoone = c.TimeZone.GmtOffset }).ToArray());
        }
        */
        public void populateDnnUserInfo(int portalId, ref UserInfo user, string fname, string lname, string username, string email, string password, string postal, string address, double? timezone)
        {
            user.PortalID = portalId;
            user.Profile.FirstName = fname;
            user.Profile.LastName = lname;
            user.FirstName = user.Profile.FirstName;
            user.LastName = user.Profile.LastName;
            user.Username = username;
            user.DisplayName = user.Username;
            user.Profile.Unit = string.Empty;
            user.Profile.Street = string.Empty;
            user.Profile.City = string.Empty;
            user.Profile.Region = string.Empty;
            user.Profile.PostalCode = string.Empty;
            user.Profile.Country = string.Empty;
            user.Email = email.ToLower();
            if( timezone != null ){
             user.Profile.TimeZone = (int)timezone.Value;
            }
            if (password != null)    // only set it on a user create.. not an edit
            {
                user.Membership.Password = password;
            }
            user.Membership.Approved = true; // Convert.ToBoolean((PortalSettings.UserRegistration != PortalRegistrationType.PublicRegistration ? false : true));
            user.Profile.PostalCode = postal;
            if (!string.IsNullOrEmpty(address))
            {
                string[] addParts = address.Split(' ');
                if (addParts.Length == 3)
                {
                    user.Profile.Country = addParts[2];
                    user.Profile.Region = addParts[0];
                }
            }
        }

        

    }
}
