<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contestant_List.aspx.cs" Inherits="MPOP_Dashboard_CMS.Contestant_List" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:GridView ID="gvContestant" runat="server" AutoGenerateColumns="False" BackColor="LightGoldenrodYellow" BorderColor="Tan" BorderWidth="1px" CellPadding="2" ForeColor="Black" GridLines="None" Width="100%">
        <AlternatingRowStyle BackColor="PaleGoldenrod" />
        <Columns>
            <asp:BoundField DataField="ContestantID" HeaderText="ContestantID" Visible="False" />
            <asp:BoundField DataField="Name" HeaderText="Name" />
            <asp:BoundField DataField="VoteCount" HeaderText="Vote Count" />
            <asp:BoundField DataField="Remark" HeaderText="Remark" />
            <asp:BoundField DataField="ContestantNo" HeaderText="ContestantNo" />
            <asp:TemplateField HeaderText="Edit">
                <ItemTemplate>
                    <asp:LinkButton ID="lbtnEdit" runat="server" CommandArgument='<%# Eval("ContestantID") %>' OnCommand="lbtnEdit_Command">Edit</asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Delete">
                <ItemTemplate>
                    <asp:LinkButton ID="lbtnDelete" runat="server" CommandArgument='<%# Eval("ContestantID") %>' OnCommand="lbtnDelete_Command">Delete</asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <FooterStyle BackColor="Tan" />
        <HeaderStyle BackColor="Tan" Font-Bold="True" />
        <PagerStyle BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
        <SortedAscendingCellStyle BackColor="#FAFAE7" />
        <SortedAscendingHeaderStyle BackColor="#DAC09E" />
        <SortedDescendingCellStyle BackColor="#E1DB9C" />
        <SortedDescendingHeaderStyle BackColor="#C2A47B" />
    </asp:GridView>
    <asp:Label ID="lblMessage" runat="server" Text="Forbidden Page!" ForeColor="Red" Visible="False"></asp:Label>
</asp:Content>