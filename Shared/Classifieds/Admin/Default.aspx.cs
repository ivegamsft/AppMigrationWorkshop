using System;
using System.Web.UI.WebControls;

using AspNet.StarterKits.Classifieds.BusinessLogicLayer;
using AspNet.StarterKits.Classifieds.Web;

public partial class AdminDefault_aspx : System.Web.UI.Page
{
    protected void StatsDataSource_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        Stats stats = e.ReturnValue as Stats;
        LastGeneratedLabel.Text = String.Format("{0:G}", stats.LastGenerated);
    }

    protected void RefreshStatsButton_Click(object sender, EventArgs e)
    {
        StatsCache.Clear(Context);
        StatsFormView.DataBind();
    }
}
