using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Affine.Data;

namespace Affine.Data
{
    public class UserNames
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string CombinedName { get; set; }

    }
}

namespace Affine.Utils
{
    public class CacheException : Exception
    {
        public CacheException(string msg)
            : base(msg)
        {
        }
    }


    public class CacheUtil
    {
        private static IDictionary<long, Affine.Data.UserNames> UserNameMap = new Dictionary<long, Affine.Data.UserNames>();

        public static UserNames GetUserNamesData(long portalKey, long userKey)
        {
            if (portalKey != 0) // right now we only support caching on the main portal.  Could easily expand this.
            {
                throw new CacheException("Only Portal '0' supports caching in this version");
            }
            if (UserNameMap.ContainsKey(userKey))
            {
                return UserNameMap[userKey];
            }
            DotNetNuke.Entities.Users.UserInfo uinfo = DotNetNuke.Entities.Users.UserController.GetUser((int)portalKey, (int)userKey, false); // false does not hydrate user ROLES
            UserNames unames = new UserNames()
            {
                FirstName = uinfo.FirstName,
                LastName = uinfo.LastName,
                CombinedName = uinfo.FirstName + " " + uinfo.LastName.ToUpper()[0] + ".",
                Email = uinfo.Email,
                UserName = uinfo.Username
            };
            UserNameMap.Add(userKey, unames);
            return unames;
        }

    }
}
