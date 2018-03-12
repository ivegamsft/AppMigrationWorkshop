using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class NavigationTree_ascx : UserControl
{
    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {

    }

    public TreeView NavigationTree
    {
        get
        {
            return TreeView1;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        
    }
    protected void TreeView1_DataBound(object sender, EventArgs e)
    {
        if (!Roles.IsUserInRole(ConfigurationManager.AppSettings["adminrolename"]))
        {
            TreeNode n=TreeView1.Nodes[0];
            n.ChildNodes.RemoveAt(5);
        }
    }

    protected void TreeView1_DataBinding(object sender, EventArgs e)
    {
        
    }
    protected void TreeView1_TreeNodeDataBound(object sender, TreeNodeEventArgs e)
    {
        if (e.Node.Parent != null)
        {
            if (e.Node.Parent.Value == "jobseekers")
            {
                e.Node.NavigateUrl = Page.ResolveUrl("~/" + ConfigurationManager.AppSettings["jobseekerfolder"] + "/" + e.Node.NavigateUrl);
            }
            if (e.Node.Parent.Value == "employers")
            {
                e.Node.NavigateUrl = Page.ResolveUrl("~/" + ConfigurationManager.AppSettings["employerfolder"] + "/" + e.Node.NavigateUrl);
            }
            if (e.Node.Parent.Value == "administration")
            {
                e.Node.NavigateUrl = Page.ResolveUrl("~/" + ConfigurationManager.AppSettings["adminfolder"] + "/" + e.Node.NavigateUrl);
            }

        }
    }
}
