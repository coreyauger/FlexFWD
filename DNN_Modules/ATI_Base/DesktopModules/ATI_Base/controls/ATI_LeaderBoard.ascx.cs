using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

using Affine.Data;

public partial class DesktopModules_ATI_Base_controls_ATI_LeaderBoard : DotNetNuke.Framework.UserControlBase
{
    public Affine.Data.json.LeaderBoardWOD[] LeaderWODList { get; set; }

    public int Cols
    {
        get;
        set;
    }

    public string CssClass { get; set; }
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack && !Page.IsCallback && this.LeaderWODList != null)
        {
            string baseUrl = ResolveUrl("~");
            Table table = new Table();
            table.CssClass = this.CssClass; 
            TableRow tr = new TableRow();
            int i = 0;
            foreach (Affine.Data.json.LeaderBoardWOD t in LeaderWODList)
            {
                i++;                
                TableCell td = new TableCell();
                td.Width = Unit.Percentage(100/this.Cols);
         //       if (!string.IsNullOrEmpty(t.Icon))
         //       {
         //           td.Controls.Add(new System.Web.UI.WebControls.Image()
         //           {
         //               ImageUrl = t.Icon
         //           });
         //       }
                string linkStart = string.Empty;
                string linkEnd = string.Empty;

                td.Controls.Add(new Literal() { Text = "<a class=\"wodLink\" href=\"" + baseUrl + "workout/"+t.Id+"\">" + t.Name + "</a>" });
                string html = "<dl>";
                for (int m = 0; m < t.Data.Length; m++)
                {
                    Affine.Data.json.LeaderBoardData data = t.Data[m];
                    html += "<dt><a href=\"" + baseUrl + data.UserName +"\">" + data.UserName + "</dt>";
                    html += "<dd>" + data.Score + "</dd>";
                }
                html += "</dl>";
               
                td.Controls.Add(new Literal() { Text = html });
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
