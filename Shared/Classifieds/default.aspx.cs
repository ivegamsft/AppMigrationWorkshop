using System;

public partial class Default_aspx : System.Web.UI.Page
{
    protected void CommonWhatsNewButton_Click(object sender, EventArgs e)
    {
        CommonWhatsNewButton.CommandArgument = "CrossPagePost";
    }
}
