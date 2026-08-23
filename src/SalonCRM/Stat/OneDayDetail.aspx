<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="OneDayDetail.aspx.cs" Inherits="SalonCRM.Stat.OneDayDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="header">
        <h1>日营业明细</h1>
    </div>
    <div class="main-content">
        <form id="form1" runat="server">
            <table class="">
                <tr>
                    <td style="padding-right: 4px">
                        <dx:ASPxComboBox ID="cbBranch" runat="server" ValueType="System.String">
                        </dx:ASPxComboBox>
                    </td>
                    <td style="padding-right: 4px">
                        <dx:ASPxDateEdit ID="deStart" EditFormatString="yyyy-MM-dd" runat="server">
                        </dx:ASPxDateEdit>
                    </td>
                    <td style="padding-right: 4px">
                        <dx:ASPxButton ID="btnStat" runat="server" Text="统计" OnClick="btnStat_Click">
                        </dx:ASPxButton>
                    </td>
                    <td style="padding-right: 4px">
                        <dx:ASPxButton ID="btnXlsExport" runat="server" Text="导出Excel" UseSubmitBehavior="False" OnClick="btnXlsExport_Click" />
                    </td>
                </tr>
            </table>
            <br />
            <dx:ASPxGridView ID="grid" runat="server" SettingsBehavior-AllowSort="false" AutoGenerateColumns="False" SettingsPager-PageSize="20">
                <Columns>
                    <dx:GridViewDataTextColumn Caption="日期" FieldName="TheDay" Visible="false" VisibleIndex="0" PropertiesTextEdit-DisplayFormatString="yyyy-MM-dd" Width="100px">
                        <PropertiesTextEdit DisplayFormatString="yyyy-MM-dd"></PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataHyperLinkColumn Caption="客户名称" FieldName="MemberId" VisibleIndex="1">
                        <PropertiesHyperLinkEdit DisplayFormatString="{0}" TextField="MemberName" NavigateUrlFormatString="~/MemberAdmin/Details/{0}">
                        </PropertiesHyperLinkEdit>
                    </dx:GridViewDataHyperLinkColumn>
                    <dx:GridViewDataTextColumn Caption="卡号" FieldName="CardNo" VisibleIndex="2">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewBandColumn Caption="消费" VisibleIndex="5">
                        <Columns>
                            <dx:GridViewDataTextColumn Caption="项目" FieldName="ServiceProjectName" VisibleIndex="0">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="项目量" FieldName="ServiceProjectNum" VisibleIndex="1">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="消费金额" FieldName="ExpenseAmount" VisibleIndex="2" PropertiesTextEdit-DisplayFormatString="#0">
                                <PropertiesTextEdit DisplayFormatString="#0"></PropertiesTextEdit>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="美容师" FieldName="Worker" VisibleIndex="3">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="时间" FieldName="ExpenseTime" VisibleIndex="4" PropertiesTextEdit-DisplayFormatString="HH:mm">
                                <PropertiesTextEdit DisplayFormatString="HH:mm"></PropertiesTextEdit>
                            </dx:GridViewDataTextColumn>
                        </Columns>
                    </dx:GridViewBandColumn>
                    <dx:GridViewBandColumn Caption="销售" VisibleIndex="6">
                        <Columns>
                            <dx:GridViewDataTextColumn Caption="项目" FieldName="ProjectName" VisibleIndex="0">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="数量" FieldName="ProjectNum" VisibleIndex="1">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="应收" FieldName="Amount" VisibleIndex="2" PropertiesTextEdit-DisplayFormatString="#0">
                                <PropertiesTextEdit DisplayFormatString="#0"></PropertiesTextEdit>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="欠款" FieldName="Debt" VisibleIndex="3" PropertiesTextEdit-DisplayFormatString="#0">
                                <PropertiesTextEdit DisplayFormatString="#0"></PropertiesTextEdit>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="销售" FieldName="ProjectSales" VisibleIndex="4">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="时间" FieldName="SalesTime" VisibleIndex="5" PropertiesTextEdit-DisplayFormatString="HH:mm">
                                <PropertiesTextEdit DisplayFormatString="HH:mm"></PropertiesTextEdit>
                            </dx:GridViewDataTextColumn>
                        </Columns>
                    </dx:GridViewBandColumn>
                    <dx:GridViewBandColumn Caption="充值" VisibleIndex="13">
                        <Columns>
                            <dx:GridViewDataTextColumn Caption="金额" FieldName="RechargeAmount" VisibleIndex="0" PropertiesTextEdit-DisplayFormatString="#0">
                                <PropertiesTextEdit DisplayFormatString="#0"></PropertiesTextEdit>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="方式" FieldName="RechangeType" VisibleIndex="1">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="销售" FieldName="RechangeSaleman" VisibleIndex="2">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="时间" FieldName="RechangeTime" VisibleIndex="3" PropertiesTextEdit-DisplayFormatString="HH:mm">
                                <PropertiesTextEdit DisplayFormatString="HH:mm"></PropertiesTextEdit>
                            </dx:GridViewDataTextColumn>
                        </Columns>
                    </dx:GridViewBandColumn>

                </Columns>

                <SettingsPager PageSize="20"></SettingsPager>

                <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
            </dx:ASPxGridView>

            <dx:ASPxGridViewExporter ID="gridExport" runat="server" GridViewID="grid"></dx:ASPxGridViewExporter>
        </form>
    </div>
</asp:Content>
