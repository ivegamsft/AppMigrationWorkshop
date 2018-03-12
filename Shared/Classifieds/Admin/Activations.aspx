<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeFile="Activations.aspx.cs"
    Inherits="Activations_aspx" Title="Manage Activations" %>
<asp:Content ID="MainContent" ContentPlaceHolderID="Main" Runat="server">
     <div id="body">
        <div id="col_main_left">
            <div id="user_assistance">
                <a id="content_start"></a>
                <h3>
                    Actions</h3>
                     <p><asp:HyperLink ID="BackToAdminLink" Runat="server" NavigateUrl="~/Admin/Default.aspx">back to Administration</asp:HyperLink></p>
                   
         </div>
        </div>
        <div id="col_main_right">
            <h2 class="section">
                Activations</h2>
                
                <div class="content_right">
                 <asp:GridView ID="ApprovalsGrid" Runat="server" DataSourceID="ActivationsDataSource"
                        AutoGenerateColumns="False" AllowPaging="True" OnRowCommand="ApprovalsGrid_RowCommand"
                        OnDataBound="ApprovalsGrid_DataBound" CellPadding="5" Width="100%" DataKeyNames="Id" BorderWidth="1">
                        <EmptyDataTemplate>
                            There are no ads requiring activation at this point.
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField><ItemTemplate>
                                <asp:CheckBox ID="AdCheckBox" Runat="server" />
                            </ItemTemplate>
                            </asp:TemplateField>
                            <asp:HyperLinkField HeaderText="Title" DataNavigateUrlFields="Id" DataNavigateUrlFormatString="~/EditAd.aspx?id={0}"
                                DataTextField="Title" SortExpression="Title"></asp:HyperLinkField>
                            <asp:BoundField HeaderText="Entered" DataField="DateCreated" SortExpression="DateCreated"
                                DataFormatString="{0:d}"></asp:BoundField>
                            <asp:TemplateField HeaderText="Actions"><ItemTemplate>
                                <asp:LinkButton ID="ActivateButton" Runat="server" CommandArgument='<%# Eval("Id") %>'
                                    CommandName="Approve">Approve</asp:LinkButton>
                                | <a href="../EditAd.aspx?id=<%# Eval("Id") %>">Edit</a> |
                                <asp:LinkButton ID="RemoveButton" Runat="server" CommandArgument='<%# Eval("Id") %>'
                                    CommandName="MarkDeleted">Mark Deleted</asp:LinkButton>
                            </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                <p>
                    <asp:RadioButtonList ID="ActivationActionsList" Runat="server">
                        <asp:ListItem Value="Approve" Selected="True">Approve Selected</asp:ListItem>
                        <asp:ListItem Value="MarkDeleted">Mark as Deleted</asp:ListItem>
                        <asp:ListItem Value="RemoveFromDB">Remove from Database</asp:ListItem>
                        <asp:ListItem Value="ApproveAll">Approve All</asp:ListItem>
                    
                    </asp:RadioButtonList>
                    <asp:Button ID="OkButton" Runat="server" Text="  Ok  " OnClick="OkButton_Click" />
                </p>
                <h3 class="section">
                    Deletions</h3>
                <p>
                    # Ads marked for Deletion:
                    <asp:Label ID="NumDeletedLabel" Runat="server"></asp:Label>.<br />
                    <asp:LinkButton ID="RemoveDeletedAdsButton" Runat="server" Visible="false" OnClick="RemoveDeletedAdsButton_Click">Remove them from Database</asp:LinkButton></p>
                   <p>Use the 
                       <asp:HyperLink ID="AdAdminLink" Runat="server" NavigateUrl="~/Admin/Ads.aspx">Ad Administration page</asp:HyperLink> to see <em>deleted</em> ads<br />(select "Marked as Deleted" from the Status list).</p>
                   
                        <asp:ObjectDataSource ID="ActivationsDataSource" Runat="server" TypeName="AspNet.StarterKits.Classifieds.BusinessLogicLayer.AdsDB"
                            SelectMethod="GetPendingAds" OnSelected="ActivationsDataSource_Selected">
                        </asp:ObjectDataSource>
                  
            </div>
        </div>
    </div>
</asp:Content>
