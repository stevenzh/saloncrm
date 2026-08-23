<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Main.Master" CodeBehind="Customer_consumption_details.aspx.cs" Inherits="SalonCRM.Stat.Customer_consumption_details" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="header">
        <h1>客户消费明细</h1>
    </div>
    <div class="main-content">
        <form id="form1" runat="server">
            <table class="">
                <tr>
                    <td style="padding-right: 4px">门店</td>
                    <td style="padding-right: 4px">
                        <dx:ASPxComboBox ID="cbBranch" runat="server" ValueType="System.String">
                        </dx:ASPxComboBox>
                    </td>
                    <td style="padding-right: 4px">客户卡号</td>
                    <td style="padding-right: 4px">
                        <dx:ASPxTextBox ID="tbCardNo" runat="server"></dx:ASPxTextBox>
                    </td>
                    <td style="padding-right: 4px">起始日期</td>
                    <td style="padding-right: 4px">
                        <dx:ASPxDateEdit ID="deStart" EditFormatString="yyyy-MM-dd" runat="server">
                        </dx:ASPxDateEdit>
                    </td>
                    <td style="padding-right: 4px">截止日期</td>
                    <td style="padding-right: 4px">
                        <dx:ASPxDateEdit ID="deEnd" EditFormatString="yyyy-MM-dd" runat="server">
                        </dx:ASPxDateEdit>
                    </td>
                    <td style="padding-right: 4px">
                        <dx:ASPxButton ID="btnSearch" runat="server" Text="查询" UseSubmitBehavior="False" OnClick="btnSearch_Click" />
                        <dx:ASPxButton ID="btnXlsExport" runat="server" Text="导出Excel" UseSubmitBehavior="False" OnClick="btnXlsExport_Click" />
                    </td>
                </tr>
            </table>
            <br />
            <dx:ASPxGridView ID="grid" runat="server" Width="100%" SettingsPager-PageSize="20" AutoGenerateColumns="False">
                <Columns>
                    <dx:GridViewDataTextColumn Caption="门店名称" VisibleIndex="0" FieldName="BranchName">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="客户姓名" VisibleIndex="1">
                        <DataItemTemplate>
                            <a href="/MemberAdmin/Details/<%# Eval("MemberID") %>"><%# Eval("Name")%></a>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="卡号" VisibleIndex="2" FieldName="CardNo">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="充值金额" VisibleIndex="3" FieldName="RechargeAmount">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="剩余金额" VisibleIndex="4" FieldName="RemaindAmount">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="消费金额" VisibleIndex="5" FieldName="ExpenseAmount">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="项目编码" VisibleIndex="6" FieldName="ProjectCode">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="项目名称" VisibleIndex="7" FieldName="ProjectName">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="购买次数" VisibleIndex="8" FieldName="BookTime">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="均价" VisibleIndex="9" FieldName="jj" UnboundExpression="[ExpenseAmount]/[BookTime]" UnboundType="Decimal">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="购买日期" VisibleIndex="10" FieldName="CreatedDate" PropertiesTextEdit-DisplayFormatString="yyyy-MM-dd HH:mm:ss" SortIndex="1" SortOrder="Descending">
                        <PropertiesTextEdit DisplayFormatString="yyyy-MM-dd HH:mm:ss"></PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="客户ID" VisibleIndex="11" FieldName="MemberId" SortIndex="0" Visible="False">
                    </dx:GridViewDataTextColumn>
                </Columns>
                <%-- 
            <Templates>
                <DetailRow>
                    <b><%# Eval("Name")%>历史消费记录</b>
                    <br />
                    <br />
                    <dx:ASPxGridView ID="detailGrid" DataSourceID="detailDataSource" runat="server" Width="100%" OnBeforePerformDataSelect="detailGrid_DataSelect">
                        <Columns>
                            <dx:GridViewDataTextColumn FieldName="Name" Caption="项目名称" VisibleIndex="1" />
                            <dx:GridViewDataTextColumn FieldName="BookTime" VisibleIndex="2" />
                            <dx:GridViewDataTextColumn FieldName="Amount" VisibleIndex="3" />
                            <dx:GridViewDataTextColumn FieldName="CreateDate" Caption="购买时间" VisibleIndex="4" PropertiesTextEdit-DisplayFormatString="yyyy-MM-dd"></dx:GridViewDataTextColumn>
                        </Columns>
                    </dx:ASPxGridView>
                </DetailRow>
            </Templates>
            <SettingsDetail ShowDetailRow="true" />
                --%>

                <SettingsPager PageSize="20"></SettingsPager>
                <Settings ShowFooter="true" />
                <TotalSummary>
                    <dx:ASPxSummaryItem FieldName="zje" SummaryType="Sum" />
                    <dx:ASPxSummaryItem FieldName="zcs" SummaryType="Sum" />
                    <dx:ASPxSummaryItem FieldName="sycs" SummaryType="Sum" />
                </TotalSummary>
            </dx:ASPxGridView>
            <dx:ASPxGridViewExporter ID="gridExport" runat="server" GridViewID="grid"></dx:ASPxGridViewExporter>
            <asp:SqlDataSource ID="detailDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:DefaultConnection %>" SelectCommand="SELECT p.Name, mp.Amount, mp.CreateDate, mp.UsedTime, mp.BookTime FROM MemberProjects mp, Projects p WHERE mp.ProjectID=p.ProjectID AND mp.MemberId = @MemberId and mp.CreateDate>@StartDate and mp.CreateDate<@EndDate">
                <SelectParameters>
                    <asp:SessionParameter Name="MemberId" SessionField="MemberId" Type="Int32" />
                    <asp:SessionParameter Name="StartDate" SessionField="StartDate" Type="DateTime" />
                    <asp:SessionParameter Name="EndDate" SessionField="EndDate" Type="DateTime" />
                </SelectParameters>
            </asp:SqlDataSource>
        </form>
    </div>
</asp:Content>
