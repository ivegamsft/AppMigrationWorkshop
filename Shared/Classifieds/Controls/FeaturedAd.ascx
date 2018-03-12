<%@ Control Language="C#" CodeFile="FeaturedAd.ascx.cs" Inherits="FeaturedAd_ascx" %>
<asp:Repeater ID="FeaturedAd" Runat="server" DataSourceID="FeaturedAdDataSource">
<ItemTemplate>
<div id="featured_item"><a id="content_start"></a> 
      <h2>Featured Item</h2> 
      <p><asp:HyperLink ID="PhotoLink" Runat="server" NavigateUrl='<%# Eval("AdId", "~/ShowAd.aspx?id={0}") %>'><img src='<%# Eval("PreviewImageId", "PhotoDisplay.ashx?photoid={0}&size=medium") %>' alt="Featured Image" style="border:0;" /></asp:HyperLink></p> 
      <p><asp:HyperLink ID="TitleLink" Runat="server" Text='<%# Eval("Title") %>'  NavigateUrl='<%# Eval("AdId", "~/ShowAd.aspx?id={0}") %>'></asp:HyperLink></p> 
    </div> 
</ItemTemplate>
</asp:Repeater>
<asp:ObjectDataSource ID="FeaturedAdDataSource" Runat="server" TypeName="AspNet.StarterKits.Classifieds.Web.FeaturedAdCache" SelectMethod="GetFeaturedAd" OnSelected="FeaturedAdDataSource_Selected">
</asp:ObjectDataSource>