using System;
using System.Web.UI.WebControls;

public partial class FeaturedAd_ascx : System.Web.UI.UserControl
{
    protected void FeaturedAdDataSource_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        this.Visible = (e.ReturnValue != null);
    }

}
