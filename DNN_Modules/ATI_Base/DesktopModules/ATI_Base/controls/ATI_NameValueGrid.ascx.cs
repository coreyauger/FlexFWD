using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

using Affine.Data;

public partial class DesktopModules_ATI_Base_controls_ATI_NameValueGrid : DotNetNuke.Framework.UserControlBase
{

    public class TotalItem
    {
        public string Link { get; set; }
        public string Total { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
    }

    private IList<TotalItem> _Totals = new List<TotalItem>();
    public IList<TotalItem> Totals { get { return _Totals; } set { _Totals = value; } }

    public int Cols
    {
        get;
        set;
    }

    public string CssClass { get; set; }
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Table table = new Table();
            table.CssClass = this.CssClass; 
            TableRow tr = new TableRow();
            int i = 0;
            foreach (TotalItem t in Totals)
            {
                i++;
                TableCell td = new TableCell();
                if (!string.IsNullOrEmpty(t.Icon))
                {
                    td.Controls.Add(new System.Web.UI.WebControls.Image()
                    {
                        ImageUrl = t.Icon
                    });
                }
                string linkStart = string.Empty;
                string linkEnd = string.Empty;
                if (!string.IsNullOrEmpty(t.Link))
                {
                    linkStart = "<a href=\"" + t.Link + "\">";
                    linkEnd = "</a>";
                }
                td.CssClass = "nvName";
                td.Controls.Add(new Literal() { Text = linkStart + t.Name + linkEnd });
                tr.Controls.Add(td);
                td = new TableCell();
                td.CssClass = "nvValue";
                td.Controls.Add(new Literal() { Text =  linkStart + t.Total + linkEnd });
                tr.Controls.Add(td);
                if (i == Cols)
                {
                    table.Rows.Add(tr);
                    tr = new TableRow();
                    i = 0;
                }
            }
            for (; i < Cols; i++)
            {
                tr.Controls.Add(new TableCell());
                tr.Controls.Add(new TableCell());
            }
            table.Rows.Add(tr);
            
            phWorkoutTotals.Controls.Add(table);
        }
    }
}
