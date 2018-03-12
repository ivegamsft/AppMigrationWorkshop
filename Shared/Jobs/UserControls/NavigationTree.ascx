<%@ Control Language="C#" CodeFile="NavigationTree.ascx.cs" Inherits="NavigationTree_ascx" %>
<asp:TreeView ID="TreeView1" Runat="server" DataSourceID="XmlDataSource1" OnSelectedNodeChanged="TreeView1_SelectedNodeChanged" NodeIndent="5" ShowLines="True" OnDataBound="TreeView1_DataBound" OnDataBinding="TreeView1_DataBinding" OnTreeNodeDataBound="TreeView1_TreeNodeDataBound">
    <DataBindings>
        <asp:TreeNodeBinding DataMember="TopMenu" Value="Options" Text="Menu Options"></asp:TreeNodeBinding>
        <asp:TreeNodeBinding DataMember="Login" NavigateUrlField="url" Value="Login" Text="Login"></asp:TreeNodeBinding>
        <asp:TreeNodeBinding DataMember="ChangePassword" NavigateUrlField="url" Value="ChangePassword" Text="Change Password"></asp:TreeNodeBinding>
        <asp:TreeNodeBinding DataMember="PostResume" NavigateUrlField="url" Value="Post Resume"
            Text="Post Resume"></asp:TreeNodeBinding>
        <asp:TreeNodeBinding DataMember="Register" NavigateUrlField="url" Value="Register"
            Text="Register"></asp:TreeNodeBinding>
        <asp:TreeNodeBinding DataMember="Login" NavigateUrlField="url" Value="Login" Text="Login"></asp:TreeNodeBinding>
        <asp:TreeNodeBinding DataMember="SearchJobs" NavigateUrlField="url" Value="Search Jobs"
            Text="Search Jobs"></asp:TreeNodeBinding>
        <asp:TreeNodeBinding DataMember="PostResume" NavigateUrlField="url" Value="Post Jobs"
            Text="Post Jobs"></asp:TreeNodeBinding>
        <asp:TreeNodeBinding DataMember="PostJobs" NavigateUrlField="url" Value="Post Jobs"
            Text="Post Jobs"></asp:TreeNodeBinding>
        <asp:TreeNodeBinding DataMember="Register" NavigateUrlField="url" Value="Register"
            Text="Register"></asp:TreeNodeBinding>
        <asp:TreeNodeBinding DataMember="SearchResumes" NavigateUrlField="url" Value="Search Resumes"
            Text="Search Resumes"></asp:TreeNodeBinding>
        <asp:TreeNodeBinding DataMember="JobSeekers" Value="jobseekers" Text="Job Seekers"></asp:TreeNodeBinding>
        <asp:TreeNodeBinding DataMember="ModifyProfile" NavigateUrlField="url" Value="Company Profile"
            Text="Company Profile"></asp:TreeNodeBinding>
        <asp:TreeNodeBinding DataMember="MyFavorites" NavigateUrlField="url" Value="My Favorites"
            Text="My Favorites"></asp:TreeNodeBinding>
        <asp:TreeNodeBinding DataMember="MyFavorites" NavigateUrlField="url" Value="My Favorites"
            Text="My Favorites"></asp:TreeNodeBinding>
        <asp:TreeNodeBinding DataMember="EducationLevels" NavigateUrlField="url" Value="Education Levels"
            Text="Education Levels"></asp:TreeNodeBinding>
        <asp:TreeNodeBinding DataMember="ExperienceLevels" NavigateUrlField="url" Value="Experience Levels"
            Text="Experience Levels"></asp:TreeNodeBinding>
        <asp:TreeNodeBinding DataMember="Categories" NavigateUrlField="url" Value="Categories"
            Text="Categories"></asp:TreeNodeBinding>
        <asp:TreeNodeBinding DataMember="Employers" Text="Employers" Value="employers" />
        <asp:TreeNodeBinding DataMember="Administration" Text="Administration" Value="administration" />
    </DataBindings>
</asp:TreeView>
<asp:XmlDataSource ID="XmlDataSource1" Runat="server" DataFile="~/UserControls/NavigationTree.xml">
</asp:XmlDataSource>
