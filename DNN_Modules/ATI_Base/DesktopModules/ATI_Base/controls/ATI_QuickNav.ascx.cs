using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Xml.Linq;

public partial class DesktopModules_ATI_Base_controls_ATI_QuickNav : DotNetNuke.Entities.Modules.PortalModuleBase
{
    // TODO: move this tree out of memory when there is time..
    private XDocument xdoc = new XDocument(new XElement("RadTreeNodes",      
                                                new XElement("Node", new XAttribute("Text", "Friends"),
                                                      new XElement("RadTreeNodes",
                                                          new XElement("Node", new XAttribute("Text", "My Friends"), new XAttribute("NavUrl", "javascript: Aqufit.NavigationRule.Call('Friends:All');")),
                                                          new XElement("Node", new XAttribute("Text", "Find"), new XAttribute("NavUrl", "javascript: Aqufit.NavigationRule.Call('Friends:Find');")),
                                                          new XElement("Node", new XAttribute("Text", "Requests"), new XAttribute("NavUrl", "javascript: Aqufit.NavigationRule.Call('Friends:Invite');"))
                                                          )),
                                                new XElement("Node", new XAttribute("Text", "Groups"),
                                                    new XElement("RadTreeNodes",
                                                          new XElement("Node", new XAttribute("Text", "Find"), new XAttribute("NavUrl", "javascript: alert('test');")),
                                                          new XElement("Node", new XAttribute("Text", "Invite"), new XAttribute("NavUrl", "javascript: alert('test');"))
                                                          )),
                                                new XElement("Node", new XAttribute("Text", "Requests"),
                                                    new XElement("RadTreeNodes",
                                                          new XElement("Node", new XAttribute("Text", "Inbox"), new XAttribute("NavUrl", "javascript: Aqufit.NavigationRule.Call('Messages:Inbox');")),
                                                          new XElement("Node", new XAttribute("Text", "Sent"), new XAttribute("NavUrl", "javascript: Aqufit.NavigationRule.Call('Messages:Sent');"))
                                                          )),
                                                 new XElement("Node", new XAttribute("Text", "Messages"),
                                                    new XElement("RadTreeNodes",
                                                          new XElement("Node", new XAttribute("Text", "Inbox"), new XAttribute("NavUrl", "javascript: Aqufit.NavigationRule.Call('Messages:Inbox');")),
                                                          new XElement("Node", new XAttribute("Text", "Sent"), new XAttribute("NavUrl", "javascript: Aqufit.NavigationRule.Call('Messages:Sent');"))
                                                          )),
                                                new XElement("Node", new XAttribute("Text", "Photos"),
                                                    new XElement("RadTreeNodes",
                                                          new XElement("Node", new XAttribute("Text", "Find"), new XAttribute("NavUrl", "javascript: alert('test');")),
                                                          new XElement("Node", new XAttribute("Text", "Invite"), new XAttribute("NavUrl", "javascript: alert('test');"))
                                                          )),
                                                new XElement("Node", new XAttribute("Text", "Routes"),
                                                    new XElement("RadTreeNodes",
                                                          new XElement("Node", new XAttribute("Text", "Find"), new XAttribute("NavUrl", "javascript: alert('test');")),
                                                          new XElement("Node", new XAttribute("Text", "Invite"), new XAttribute("NavUrl", "javascript: alert('test');"))
                                                          ))));


    public string ExpandNode { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack && !Page.IsCallback)
        {
            foreach (XElement elm in xdoc.Element("RadTreeNodes").Elements())
            {
                radQuickNav.Nodes.Add(new Telerik.Web.UI.RadTreeNode(elm.Attribute("Text").Value));
            }
            if (!string.IsNullOrEmpty(this.ExpandNode))
            {
                Telerik.Web.UI.RadTreeNode node = radQuickNav.FindNodeByText(this.ExpandNode);
                foreach (XElement elm in xdoc.Element("RadTreeNodes").Elements().First(el => el.Attribute("Text").Value == this.ExpandNode).Element("RadTreeNodes").Elements() )
                {
                    node.Nodes.Add(new Telerik.Web.UI.RadTreeNode(elm.Attribute("Text").Value, "", elm.Attribute("NavUrl").Value));
                }
                if (node.Nodes.Count > 0)
                {
                    node.Nodes[0].Selected = true;
                }
                node.Expanded = true;                
            }
            
        }
    }
}
