using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DotNetNuke.Entities.Users;

using DotNetNuke;

public partial class DesktopModules_ATI_Base_controls_ATI_WebLinks : DotNetNuke.Framework.UserControlBase
{
    public bool IsEditMode
    {
        get
        {
            if (ViewState["SlimIsEditMode"] == null)
            {
                return false;
            }
            return Convert.ToBoolean(ViewState["SlimIsEditMode"]);
        }
        set
        {
            ViewState["SlimIsEditMode"] = value;
        }
    }   

    public string ValidationGroupName
    {
        get;
        set;
    }

    public string Twitter
    {
        get { return hidTwitter.Value; }
        set { hidTwitter.Value = value; }
    }

    public string Facebook
    {
        get { return hidFacebook.Value; }
        set { hidFacebook.Value = value; }
    }

    public string Flickr
    {
        get { return hidFlickr.Value; }
        set { hidFlickr.Value = value; }
    }

    public string LinkedIn
    {
        get { return hidLinkedIn.Value; }
        set { hidLinkedIn.Value = value; }
    }

    public string YouTube
    {
        get { return hidYouTube.Value; }
        set { hidYouTube.Value = value; }
    }

    public string Peronsal
    {
        get { return atiPersonlUrl.Text; }
        set { atiPersonlUrl.Text = value; }
    }

    /*
    public delegate void UpdatePasswordEventHandler(object sender, EventArgs e);
    public event UpdatePasswordEventHandler UpdatePassword;       
    */
    private void AssignValidationGroup(Control con)
    {
        foreach (Control c in con.Controls)
        {
            if (c is BaseValidator)
            {
                ((BaseValidator)c).ValidationGroup = this.ValidationGroupName;
            }
            else
            {
                AssignValidationGroup(c);
            }
        }
    }

    public void SetValidationGroup(string group)
    {
        this.ValidationGroupName = group;
        AssignValidationGroup(this);  
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if ( !string.IsNullOrEmpty( this.ValidationGroupName))
            {
                AssignValidationGroup(this);           
            }            
        }
    }
  
   
}
