<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MyTelBillCharge.aspx.cs" Inherits="MPOP_Dashboard_CMS.MyTelBillCharge" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <table class="nav-justified">
        <tr>
            <td>
                &nbsp;</td>
            <td>
                <asp:Label ID="Label6" runat="server" Text="MyTel Bill Charge" Font-Bold="True" Font-Size="Large" Font-Underline="True" ForeColor="#3333FF"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" Text="PhoneNo :"></asp:Label>
            </td>
            <td>
                <%--<asp:TextBox ID="txtMSISDNNo" runat="server"></asp:TextBox>--%>
                <asp:TextBox ID="txtMSISDNNo" runat="server" onkeydown="return (!(event.keyCode>=65) && event.keyCode!=32);"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvMSISDNNo" runat="server" ControlToValidate="txtMSISDNNo" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label2" runat="server" Text="Amount :"></asp:Label>
            </td>
            <td>
                <%--<asp:TextBox ID="txtAmount" runat="server" TextMode="Number"></asp:TextBox>--%>
                <asp:TextBox ID="txtAmount" runat="server" onkeydown="return (!(event.keyCode>=65) && event.keyCode!=32);"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvAmount" runat="server" ControlToValidate="txtAmount" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>
                <asp:Button ID="btnSync" runat="server" OnClick="btnSync_Click" Text="Sync" />
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label5" runat="server" Text="Result :"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtResult" runat="server" Height="100px" TextMode="MultiLine" Width="491px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>
                <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>