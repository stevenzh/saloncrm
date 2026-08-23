<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Main.Master" CodeBehind="Financial_Receivables.aspx.cs" Inherits="SalonCRM.Stat.Financial_Receivables" %>

<asp:Content ID="Content2" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="header">
        <h1>应收款一览表</h1>
    </div>
    <div class="main-content">
        <form id="form1" runat="server">
            <table class="">
                <tr>
                    <td style="padding-right: 4px">门店</td>
                    <td style="padding-right: 4px">
                        <dx:ASPxComboBox ID="cbBranch" runat="server">
                            <ClientSideEvents SelectedIndexChanged="function(s) { cbSalesman.PerformCallback(s.GetValue()) }" />
                        </dx:ASPxComboBox>
                    </td>
                    <td style="padding-right: 4px">开始日期</td>
                    <td style="padding-right: 4px;">
                        <dx:ASPxDateEdit ID="deStart" EditFormatString="yyyy-MM-dd" runat="server">
                        </dx:ASPxDateEdit>
                    </td>
                    <td style="padding-right: 4px">结束日期</td>
                    <td style="padding-right: 4px">
                        <dx:ASPxDateEdit ID="deDate" EditFormatString="yyyy-MM-dd" runat="server">
                        </dx:ASPxDateEdit>
                    </td>
                    <td style="padding-right: 4px">美容顾问</td>
                    <td style="padding-right: 4px">
                        <dx:ASPxComboBox ID="cbSalesman" ClientInstanceName="cbSalesman" runat="server" EnableCallbackMode="true" OnCallback="cbSalesman_Callback">
                        </dx:ASPxComboBox>
                    </td>
                    <td style="padding-right: 4px">
                        <dx:ASPxButton ID="ASPxButton1" runat="server" Text="查询" OnClick="ASPxButton1_Click">
                        </dx:ASPxButton>
                    </td>
                    <td style="padding-right: 4px">
                        <dx:ASPxButton ID="btnXlsExport" runat="server" Text="导出Excel" UseSubmitBehavior="False" OnClick="btnXlsExport_Click" />
                    </td>
                </tr>
            </table>
            <br />
            <dx:ASPxGridView ID="grid" runat="server" Settings-ShowHeaderFilterButton="true" Settings-ShowFilterRow="true" AutoGenerateColumns="False" Width="100%" SettingsPager-PageSize="20">
                <Columns>
                    <dx:GridViewDataTextColumn Caption="门店" VisibleIndex="1" FieldName="BranchName">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="销售金额" VisibleIndex="2" FieldName="Sales">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="现金" VisibleIndex="3" FieldName="Cash">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="刷卡" VisibleIndex="4" FieldName="CardMoney">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="转账" VisibleIndex="5" FieldName="Transfer">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="美容顾问" VisibleIndex="6" FieldName="Salesman">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="销售日期" VisibleIndex="7" FieldName="CreatedDate" SortOrder="Descending" SortIndex="1" PropertiesTextEdit-DisplayFormatString="yyyy-MM-dd">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="操作日期" VisibleIndex="8" FieldName="fdfd" UnboundExpression="GetDate(CreatedDate)" UnboundType="DateTime" GroupIndex="0" PropertiesTextEdit-DisplayFormatString="yyyy-MM-dd">
                    </dx:GridViewDataTextColumn>
                </Columns>
                <Settings ShowGroupPanel="True" ShowFooter="True" ShowGroupFooter="VisibleIfExpanded" />
                <TotalSummary>
                    <dx:ASPxSummaryItem FieldName="Sales" SummaryType="Sum" />
                    <dx:ASPxSummaryItem FieldName="Cash" SummaryType="Sum" />
                    <dx:ASPxSummaryItem FieldName="CardMoney" SummaryType="Sum" />
                    <dx:ASPxSummaryItem FieldName="Transfer" SummaryType="Sum" />
                </TotalSummary>
                <GroupSummary>
                    <dx:ASPxSummaryItem FieldName="Cash" ShowInGroupFooterColumn="现金" SummaryType="Sum" />
                    <dx:ASPxSummaryItem FieldName="CardMoney" ShowInGroupFooterColumn="刷卡" SummaryType="Sum" />
                    <dx:ASPxSummaryItem FieldName="Transfer" ShowInGroupFooterColumn="转账" SummaryType="Sum" />
                </GroupSummary>
            </dx:ASPxGridView>

            <dx:ASPxGridViewExporter ID="gridExport" runat="server" GridViewID="grid"></dx:ASPxGridViewExporter>

        </form>
    </div>
</asp:Content>

