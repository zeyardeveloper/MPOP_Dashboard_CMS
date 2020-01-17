<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserPasswordChange.aspx.cs" Inherits="MPOP_Dashboard_CMS.UserPasswordChange" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <table class="auto-style1">
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" Text="Old Password:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtOldPassword" runat="server" TextMode="Password"></asp:TextBox>
                <asp:HiddenField ID="hfOldPassword" runat="server" />
                <asp:RequiredFieldValidator ID="rfvOldPassword" runat="server" ErrorMessage="*" ControlToValidate="txtOldPassword" ForeColor="Red"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label2" runat="server" Text="New Password:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvNewPassword" runat="server" ErrorMessage="*" ControlToValidate="txtNewPassword" ForeColor="Red"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="btnChangePassword" runat="server" OnClick="btnChangePassword_Click" Text="Change Password" />
                <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Text="Your old password is incorrect!" Visible="False"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>