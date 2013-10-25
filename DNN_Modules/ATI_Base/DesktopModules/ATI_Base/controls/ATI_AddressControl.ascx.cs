using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DotNetNuke.Entities.Users;
using DotNetNuke;
using DotNetNuke.Common.Lists;
using DotNetNuke.Services.Localization;

public partial class DesktopModules_ATI_Base_controls_ATI_AddressControl : DotNetNuke.Framework.UserControlBase
{

    private string MyFileName = "Address.ascx";
    private int _StartTabIndex = 1;

    public string Region
    {
        get {
            this.EnsureChildControls();
            if (ddlRegion.Visible)
            {
                return ddlRegion.SelectedValue;
            }
            else
            {
                return txtRegion.Text;
            }
        }
    }

    public string Country
    {
        get { this.EnsureChildControls();  return ddlCountry.SelectedValue; }
    }
   

    public string Street
    {
        get { this.EnsureChildControls(); return atiTxtAddress.Text; }
    }

    public string City
    {
        get { this.EnsureChildControls(); return atiTxtCity.Text; }
    }

    public string Postal
    {
        get { this.EnsureChildControls(); return atiTxtPostal.Text; }
    }

    public double Lat
    {
        get { this.EnsureChildControls(); return Convert.ToDouble( hiddenLat.Value ); }
    }

    public double Lng
    {
        get { this.EnsureChildControls(); return Convert.ToDouble(hiddenLng.Value); }
    }

    public double LatHome
    {
        get { this.EnsureChildControls(); return Convert.ToDouble(HiddenHomeLat.Value); }
    }
    public double LngHome
    {
        get { this.EnsureChildControls(); return Convert.ToDouble(HiddenHomeLng.Value); }
    }

    public Unit Width { get; set; }


    /// -----------------------------------------------------------------------------
    /// <summary>
    /// Localize correctly sets up the control for US/Canada/Other Countries
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <history>
    ///  [cnurse]	10/08/2004	Updated to reflect design changes for Help, 508 support
    /// and localisation
    /// </history>
    /// -----------------------------------------------------------------------------
    /*
    private void Localize()
    {
            string countryCode = cboCountry.SelectedItem.Value;
            ListController ctlEntry = new ListController();         // listKey in format "Country.US:Region"
            string listKey = "Country." + countryCode;
            ListEntryInfoCollection entryCollection = ctlEntry.GetListEntryInfoCollection("Region", listKey);

            if( entryCollection.Count != 0 )
            {
                cboRegion.Visible = true;
                txtRegion.Visible = false;
                cboRegion.DataSource = entryCollection;
                cboRegion.DataBind();
                cboRegion.Items.Insert(0, new ListItem("<" + DotNetNuke.Services.Localization.Localization.GetString("Not_Specified", DotNetNuke.Services.Localization.Localization.SharedResourceFile) + ">", ""));                
                if( countryCode.ToLower() == "us")
                {
                    valRegion1.Enabled = true;
                    valRegion2.Enabled = false;
                    //valRegion1.ErrorMessage = DotNetNuke.Services.Localization.Localization.GetString("StateRequired", DotNetNuke.Services.Localization.Localization.GetResourceFile(this, MyFileName));
                    valRegion1.ErrorMessage = "State Required";
                    plRegion.Text = "State:";
                    //plRegion.Text = DotNetNuke.Services.Localization.Localization.GetString("plState", DotNetNuke.Services.Localization.Localization.GetResourceFile(this, MyFileName));
                    //plRegion.HelpText = DotNetNuke.Services.Localization.Localization.GetString("plState.Help", DotNetNuke.Services.Localization.Localization.GetResourceFile(this, MyFileName));
                    plPostal.Text = "Zip:";
                    //plPostal.Text = DotNetNuke.Services.Localization.Localization.GetString("plZip", DotNetNuke.Services.Localization.Localization.GetResourceFile(this, MyFileName));
                    //plPostal.HelpText = DotNetNuke.Services.Localization.Localization.GetString("plZip.Help", DotNetNuke.Services.Localization.Localization.GetResourceFile(this, MyFileName));
                }else{
                    valRegion1.ErrorMessage = "Province Required";
                    //valRegion1.ErrorMessage = DotNetNuke.Services.Localization.Localization.GetString("ProvinceRequired", DotNetNuke.Services.Localization.Localization.GetResourceFile(this, MyFileName));
                    plRegion.Text = "Province:";
                    //plRegion.Text = DotNetNuke.Services.Localization.Localization.GetString("plProvince", DotNetNuke.Services.Localization.Localization.GetResourceFile(this, MyFileName));
                    //plRegion.HelpText = DotNetNuke.Services.Localization.Localization.GetString("plProvince.Help", DotNetNuke.Services.Localization.Localization.GetResourceFile(this, MyFileName));
                    plRegion.Text = "Postal:";
                    //plPostal.Text = DotNetNuke.Services.Localization.Localization.GetString("plPostal", DotNetNuke.Services.Localization.Localization.GetResourceFile(this, MyFileName));
                    //plPostal.HelpText = DotNetNuke.Services.Localization.Localization.GetString("plPostal.Help", DotNetNuke.Services.Localization.Localization.GetResourceFile(this, MyFileName));
                }
                valRegion1.Enabled = true;
                valRegion2.Enabled = false;
            }else{
                cboRegion.ClearSelection();
                cboRegion.Visible = false;
                txtRegion.Visible = true;
                valRegion1.Enabled = false;
                valRegion2.Enabled = true;
                //valRegion2.ErrorMessage = DotNetNuke.Services.Localization.Localization.GetString("RegionRequired", DotNetNuke.Services.Localization.Localization.GetResourceFile(this, MyFileName));
                valRegion2.ErrorMessage = "Region Required";
                plRegion.Text = "Region";
                //plRegion.Text = DotNetNuke.Services.Localization.Localization.GetString("plRegion", DotNetNuke.Services.Localization.Localization.GetResourceFile(this, MyFileName));
                //plRegion.HelpText = DotNetNuke.Services.Localization.Localization.GetString("plRegion.Help", DotNetNuke.Services.Localization.Localization.GetResourceFile(this, MyFileName));
                plPostal.Text = "Postal";
                //plPostal.Text = DotNetNuke.Services.Localization.Localization.GetString("plPostal", DotNetNuke.Services.Localization.Localization.GetResourceFile(this, MyFileName));
                //plPostal.HelpText = DotNetNuke.Services.Localization.Localization.GetString("plPostal.Help", DotNetNuke.Services.Localization.Localization.GetResourceFile(this, MyFileName));
            }        
    }
     */
    public string ValidationGroupName
    {
        get;
        set;
    }

    


    public void LoadUserInfo(UserInfo user)
    {
        atiTxtCity.Text = user.Profile.City;
        atiTxtPostal.Text = user.Profile.PostalCode;
        atiTxtAddress.Text = user.Profile.Street;
    }

    public bool EnableValidation
    {
        get;
        set;
    }

    private void AddignValidationGroup(Control con)
    {
       
        foreach (Control c in con.Controls)
        {
            if (c is BaseValidator)
            {
                ((BaseValidator)c).ValidationGroup = this.ValidationGroupName;                
            }
            else
            {                
                AddignValidationGroup(c);
            }
        }
        

    }
   
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(this.ValidationGroupName))
            {
                AddignValidationGroup(this);
            }
            ListController ctlEntry = new ListController();
            ListEntryInfoCollection entryCollection = ctlEntry.GetListEntryInfoCollection("Country");
            if (!Page.IsPostBack)
            {
                ddlCountry.DataSource = entryCollection;
                ddlCountry.DataBind();
                ddlCountry.Items.FindByValue("US").Selected = true;

                string listKey = "Country.US";
                ListEntryInfoCollection regionCollection = ctlEntry.GetListEntryInfoCollection("Region", listKey);
                ddlRegion.DataSource = regionCollection;
                ddlRegion.DataBind();

                atiTxtAddress.TabIndex = Convert.ToInt16(_StartTabIndex);
                atiTxtCity.TabIndex = Convert.ToInt16(_StartTabIndex + 2);
                ddlCountry.TabIndex = Convert.ToInt16(_StartTabIndex + 3);
                ddlRegion.TabIndex = Convert.ToInt16(_StartTabIndex + 4);
                txtRegion.TabIndex = Convert.ToInt16(_StartTabIndex + 5);
                atiTxtPostal.TabIndex = Convert.ToInt16(_StartTabIndex + 6);

                if( this.Width != null )
                {
                    ddlCountry.Width = this.Width;
                    ddlRegion.Width = this.Width;
                    atiTxtPostal.Width = this.Width;
                    atiTxtCity.Width = this.Width;
                    atiTxtAddress.Width = this.Width;
                    
                }
            }
            else
            {
               // ddlCountry.SelectedValue
            }

            
            
            
        }
        catch (DotNetNuke.Services.Exceptions.ModuleLoadException mlex)
        {

        }
    }
}
