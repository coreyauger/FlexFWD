using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

using Affine.Data;
using Affine.Utils;

namespace Aqufit.Helpers
{
    public class RouteItem
    {
        public long Id { get; set; }
        public double Dist { get; set; }
    }

    public class WodItem
    {
        public long Id { get; set; }
        public short Type { get; set; }
    }
}

public partial class DesktopModules_ATI_Base_controls_ATI_WorkoutScheduler : DotNetNuke.Framework.UserControlBase
{
    private System.Web.Script.Serialization.JavaScriptSerializer _serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

    public UserSettings ProfileSettings { get; set; }

    public long WODId
    {
        get
        {
            Aqufit.Helpers.WodItem wi = _serializer.Deserialize<Aqufit.Helpers.WodItem>(atiRadComboBoxCrossfitWODs.SelectedValue);
            return wi.Id;           
        }
    }

    public Affine.Utils.WorkoutUtil.WodType WodType
    {
        get
        {
            Aqufit.Helpers.WodItem wi = _serializer.Deserialize<Aqufit.Helpers.WodItem>(atiRadComboBoxCrossfitWODs.SelectedValue);
            return Affine.Utils.WorkoutUtil.IntToWODType(wi.Type);
        }
    }  

    public string Notes
    {
        get { return txtNote.Text; }
    }

    public WOD SetControlToWOD { set; get; }

    public DateTime Date
    {
        get { 
            DateTime date = DateTime.Now;
            if( atiRadDatePicker.SelectedDate != null ){
                DateTime days = Convert.ToDateTime(atiRadDatePicker.SelectedDate);
                date = new DateTime(days.Year, days.Month, days.Day, date.Hour, date.Minute, date.Second );
            }            
            return date;
        }
    }

    public string HiddenName
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtHiddenName.Text))
            {
                return txtHiddenName.Text;
            }        
            return "Workout (" + this.Date.ToShortDateString() + ")";
        }
    }

    public DateTime HideDate
    {
        get
        {
            DateTime date = DateTime.Now.AddMonths(-1);
            if (atiRadDatePickerHide.Visible && atiRadDatePickerHide.SelectedDate != null)
            {
                DateTime days = Convert.ToDateTime(atiRadDatePickerHide.SelectedDate);
                date = new DateTime(days.Year, days.Month, days.Day, date.Hour, date.Minute, date.Second);
            }
            return date;
        }
    }
 

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack && !Page.IsCallback)
        {
           // imgNikePlus.ImageUrl = ResolveUrl("~/DesktopModules/ATI_Fitness/resources/images/iSyncNikePlus.png");
            // set date control to today
            atiRadDatePicker.SelectedDate = DateTime.Now;

            // TODO: consider putting this back in futrue
            atiRadDatePickerHide.Visible = false;
            plHideDate.Visible = false;
            txtHiddenName.Visible = false;
            plHiddenName.Visible = false;
            // END

            atiRadDatePickerHide.SelectedDate = DateTime.Now.AddDays(1);
            if (this.SetControlToWOD != null)
            {
                Aqufit.Helpers.WodItem wi = new Aqufit.Helpers.WodItem() { Id = this.SetControlToWOD.Id, Type = (short)this.SetControlToWOD.WODType.Id };
                string json = _serializer.Serialize( wi );
                RadComboBoxItem item = new RadComboBoxItem(this.SetControlToWOD.Name, json);
                item.Selected = true;
                atiRadComboBoxCrossfitWODs.Items.Add(item);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "InitialWOD", "Aqufit.addLoadEvent(function(){ Aqufit.Page.Controls.atiWodSelector.SetupWOD('" + json + "') });", true);
            }
        }
    }

    // delegate declaration 
    public delegate void WodItemsRequestedHandler(object sender, RadComboBoxItemsRequestedEventArgs e);

    // event declaration 
    public event WodItemsRequestedHandler WodItemsRequested;

    public virtual void OnWodItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        if (this.WodItemsRequested != null)
        {
            this.WodItemsRequested(sender, e);
        }
    }

    public void RadComboBox2_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        OnWodItemsRequested(sender, e);        
    }
}
