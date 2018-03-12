<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeFile="AdvancedSearch.aspx.cs"
    Inherits="AdvancedSearch_aspx" Title="Advanced Search" %>
<%@ Register TagPrefix="uc1" TagName="AdvancedSearch" Src="Controls/AdvancedSearch.ascx" %>
<asp:Content ID="MainContent" ContentPlaceHolderID="Main" Runat="server">
    <div id="body">
        <div id="col_main_left">
            <div id="user_assistance">
                <a id="content_start"></a>
                <h3>
                    Advanced Search Help</h3>
                <p>
                    Lorem ipsum dolor sit amet, consectetuer adipiscing elit, 
                    sed diam nonummy nibh euismod tincidunt ut laoreet dolore 
                    magna aliquam erat volutpat.
                </p>
            </div>
        </div>
        <div id="col_main_right">
            <h2 class="section">
                Advanced Search</h2>
            <div class="content_right">
                <fieldset>
                    <uc1:AdvancedSearch ID="AdvancedSearch" Runat="server" />
                    <p>
                        <asp:Button ID="SearchButton" Runat="server" Text="Search" Width="96px" Height="24px"
                            PostBackUrl="~/Search.aspx" />
                    </p>
                </fieldset>
            </div>
        </div>
    </div>
</asp:Content>
