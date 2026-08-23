<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Main.Master" CodeBehind="Customer_transaction.aspx.cs" Inherits="SalonCRM.Stat.Customer_transaction" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="header">
        <h1>预约到店成交统计</h1>
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
                        <dx:ASPxButton ID="ASPxButton1" runat="server" Text="查看" OnClick="ASPxButton1_Click">
                        </dx:ASPxButton>
                    </td>
                    <td style="padding-right: 4px">
                        <dx:ASPxButton ID="btnXlsExport" runat="server" Text="导出Excel" UseSubmitBehavior="False" OnClick="btnXlsExport_Click" />
                    </td>
                </tr>
            </table>
            <br />
            <dx:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False" Width="100%" SettingsPager-PageSize="20">
                <SettingsPager PageSize="20"></SettingsPager>
                <Columns>
                    <dx:GridViewDataDateColumn Caption="日期" VisibleIndex="0" FieldName="BookDate" SortOrder="Descending" SortIndex="0" PropertiesDateEdit-DisplayFormatString="yyyy-MM-dd">
                        <PropertiesDateEdit DisplayFormatString="yyyy-MM-dd"></PropertiesDateEdit>
                    </dx:GridViewDataDateColumn>
                    <dx:GridViewDataTextColumn Caption="门店名称" VisibleIndex="1" FieldName="BranchName">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="预约总人数" VisibleIndex="2" FieldName="AppointmentPax">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="到店总人数" VisibleIndex="3" FieldName="InPax">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="消耗人数" VisibleIndex="4" FieldName="BookPax">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="消耗金额" VisibleIndex="5" FieldName="BookAmount">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="新客成交人数" VisibleIndex="6" FieldName="NewBookPax">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="新客消耗金额" VisibleIndex="7" FieldName="NewBookAmount">
                    </dx:GridViewDataTextColumn>
                </Columns>
                 <Settings ShowFooter="true" />
                <TotalSummary>
                    <dx:ASPxSummaryItem FieldName="AppointmentPax" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="InPax" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="BookPax" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="BookAmount" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="NewBookPax" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="NewBookAmount" SummaryType="Sum" DisplayFormat="{0}" />
                </TotalSummary>
            </dx:ASPxGridView>

            <dx:ASPxGridViewExporter ID="gridExport" runat="server" GridViewID="gridExport"></dx:ASPxGridViewExporter>
        </form>
    </div>
</asp:Content>
