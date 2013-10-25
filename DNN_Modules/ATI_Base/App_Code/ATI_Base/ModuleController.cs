using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Text;

/// <summary>
/// Summary description for ModuleController
/// </summary>

namespace Affine.Dnn
{

    public class ModuleController : DotNetNuke.Entities.Modules.ModuleController
    {

        public ModuleController()
            : base()
        {
        }

        public void UpdateLargeTabModuleSetting(Hashtable tabModuleSettings, Int32 tabModuleID, string settingName, string settingValue)
        {

            Int32 cntDel = 0;
            object o = null;
            bool continueDeleting = false;
            continueDeleting = true;

            //Delete all multiple-value module settings, if they exist. 
            while (continueDeleting == true)
            {
                o = tabModuleSettings[settingName + "_" + cntDel.ToString()];
                if ((o != null))
                {
                    DeleteTabModuleSetting(tabModuleID, settingName + "_" + cntDel.ToString());
                    cntDel += 1;
                }
                else
                {
                    continueDeleting = false;
                }
            }

            //Guard - if setting value is less than 2KB, update normally and exit 
            if (settingValue.Length < 2000)
            {
                //Normal value 
                UpdateTabModuleSetting(tabModuleID, settingName, settingValue);
                return;
            }

            //If we get to this point, then setting value is more than 2KB. 
            //Delete the original setting (if it exists) so as not to get confused. 
            DeleteTabModuleSetting(tabModuleID, settingName);


            //Split the value in 2KB chunks 
            List<string> stringList = new List<string>();
            StringBuilder sb = new StringBuilder(settingValue);

            while (sb.Length >= 2000)
            {
                stringList.Add(sb.ToString().Substring(0, 1999));
                sb.Remove(0, 1999);
            }

            //Add the last chunk 
            if (sb.Length > 0) stringList.Add(sb.ToString());

            //Now do the update changing the setting name with the suffix _x (x=0,1,2,etc.) for 
            //each update 
            Int32 cnt = 0;
            foreach (string s in stringList)
            {
                UpdateTabModuleSetting(tabModuleID, settingName + "_" + cnt.ToString(), s);
                cnt += 1;

            }
        }

        public static string ReadLargeTabModuleSetting(Hashtable tabModuleSettings, Int32 tabModuleID, string settingName)
        {

            //Guard - if there is a single setting, just return that and exit 
            object objTester = null;
            objTester = tabModuleSettings[settingName];
            if ((objTester != null))
            {
                return ((string)objTester);
            }

            //If we got to this point, there's a large value stored. 
            //Loop through the records and reconstruct the value. 
            StringBuilder sb = new StringBuilder();
            Int32 cnt = 0;
            object o = null;
            bool continueAdding = false;
            continueAdding = true;

            while (continueAdding == true)
            {
                o = tabModuleSettings[settingName + "_" + cnt.ToString()];
                if ((o != null))
                {
                    sb.Append((string)o);
                    cnt += 1;
                }
                else
                {
                    continueAdding = false;
                }
            }


            return sb.ToString();
        }

    }
}


