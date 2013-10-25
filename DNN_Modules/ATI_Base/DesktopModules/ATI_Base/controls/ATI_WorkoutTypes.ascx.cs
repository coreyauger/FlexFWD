using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Affine.Data;

using Telerik.Web.UI;

public partial class DesktopModules_ATI_Base_controls_ATI_WorkoutTypes : DotNetNuke.Framework.UserControlBase
{
    public enum DisplayMode{ CHECK_BOX, DROP_DOWN, ICONS };
       
    public DisplayMode TypeDisplayMode
    {
        get;
        set;
    }

    public string CssClass
    {
        get;
        set;
    }


    // TODO: should combine these 2
    public WorkoutType Selected
    {
        get
        {
            if (this.TypeDisplayMode != null && this.TypeDisplayMode == DisplayMode.DROP_DOWN)
            {
                return ((List<WorkoutType>)Cache["WorkoutTypeList"]).FirstOrDefault(wt => wt.Id == Convert.ToUInt32(ddlWorkoutTypeList.SelectedValue));
            }
            return null;
        }       
    }
    public Affine.Utils.WorkoutUtil.WorkoutType SelectedType { get; set; }
    // END - TODO:


    public string OnClientWorkoutTypeChanged { get; set; }

    private IList<CheckBox> _CheckBoxList = new List<CheckBox>();
    public IList<CheckBox> CheckBoxList
    {
        get
        {
            return _CheckBoxList;
        }
        private set
        {
            _CheckBoxList = value;
        }
    }

    public WorkoutType[] UncheckTypes { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {        
        ddlWorkoutTypeList.CssClass = this.CssClass; 
        aqufitEntities entities = new aqufitEntities();
        if (Cache["WorkoutTypeList"] == null)
        {
            Cache["WorkoutTypeList"] = entities.WorkoutType.Where(wt => wt.Id != 5).ToList<WorkoutType>();
        }
        // Create a check list of all the workout types
        IList<WorkoutType> wtList = (IList<WorkoutType>)Cache["WorkoutTypeList"];
        if (this.TypeDisplayMode == null || this.TypeDisplayMode == DisplayMode.CHECK_BOX)
        {
            ddlWorkoutTypeList.Visible = false;
            Table t = new Table();
            TableRow tr = new TableRow();
            int i = 0;
            foreach (WorkoutType wt in wtList)
            {
                if (i++ % 3 == 0)
                {
                    if (i > 0) t.Rows.Add(tr);
                    tr = new TableRow();
                }
                CheckBox cb = new CheckBox()
                {
                    ID = "wt_" + wt.Id,
                    Text = wt.Name,
                    Checked = true
                };
                TableCell td = new TableCell()
                {
                    CssClass = "atiThreeList"
                };
                if (this.UncheckTypes != null && this.UncheckTypes.FirstOrDefault(w => w.Id == wt.Id) != null)
                {
                    cb.Checked = false;
                }
                this.CheckBoxList.Add(cb);
                td.Controls.Add(cb);
                tr.Cells.Add(td);
            }
            t.Rows.Add(tr);
            panelWorkoutTypeList.Controls.Add(t);
            if (!Page.IsPostBack && !Page.IsCallback)
            {                
            }
        }
        else if (this.TypeDisplayMode == DisplayMode.DROP_DOWN)
        {            
            if (!Page.IsPostBack && !Page.IsCallback)
            {
                ddlWorkoutTypeList.Visible = true;
                foreach (WorkoutType wt in wtList)
                {
                    ListItem li = new ListItem() { Text = wt.Name, Value = Convert.ToString(wt.Id) };
                    if (SelectedType != null && (int)SelectedType == wt.Id)
                    {
                        li.Selected = true;
                    }
                    ddlWorkoutTypeList.Items.Add(li);                    
                }             
            }
        }
        else if (this.TypeDisplayMode == DisplayMode.ICONS)
        {
            string html = string.Empty;
            if (!Page.IsPostBack && !Page.IsCallback)
            {
                ddlWorkoutTypeList.Visible = false;
                html += "<ul class=\"hlist\">";
                foreach (WorkoutType wt in wtList)
                {
                    string css = string.Empty;
                    if (this.SelectedType != null && (int)this.SelectedType == wt.Id)
                    {
                        css = "class=\"workoutTypeSelected\" ";
                    }
                    string img = "<img src=\"" + ResolveUrl(wt.Icon) + "\" />";
                    html += "<li " + css + "><a title=\""+wt.Name+"\" href=\"javascript: ;\" " + (!string.IsNullOrWhiteSpace(this.OnClientWorkoutTypeChanged) ? "onclick=\"" + this.OnClientWorkoutTypeChanged + "(" + wt.Id + ");\"" : "") + ">" + img + "</a></li>";
                }
                html += "</ul>";
                panelWorkoutTypeList.Controls.Add(new Literal() { Text = html });
            }
        }
    }   

    /*
    protected void bAjaxPostback_Click(object sender, EventArgs e)
    {      
        OnWorkoutTypeChanged(e);
    }

    public delegate void WorkoutTypeCommandEventHandler(object sender,EventArgs e);
    public event WorkoutTypeCommandEventHandler WorkoutTypeChanged;
    protected virtual void OnWorkoutTypeChanged(EventArgs e)
    {
        if (WorkoutTypeChanged != null) WorkoutTypeChanged(ddlWorkoutTypeList, e);
    }   
    */
   
}
