<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contestant_Entry.aspx.cs" Inherits="MPOP_Dashboard_CMS.Contestant_Entry" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <table class="nav-justified">
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" Text="Contestant Name :"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtContestantName" runat="server"></asp:TextBox>
                <asp:HiddenField ID="hfContestantID" runat="server" />
                <asp:RequiredFieldValidator ID="rfvContestantName" runat="server" ControlToValidate="txtContestantName" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label2" runat="server" Text="Vote Count :"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtVoteCount" runat="server" TextMode="Number"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvVoteCount" runat="server" ControlToValidate="txtVoteCount" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label4" runat="server" Text="Remark :"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtRemark" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="height: 22px">
                <asp:Label ID="Label3" runat="server" Text="Contestant No :"></asp:Label>
            </td>
            <td style="height: 22px">
                <asp:TextBox ID="txtContestantNo" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvContestantNo" runat="server" ControlToValidate="txtContestantNo" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" />
            </td>
            <td>&nbsp;</td>
        </tr>
    </table>
</asp:Content>