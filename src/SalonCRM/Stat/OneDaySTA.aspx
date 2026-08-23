<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="OneDaySTA.aspx.cs" Inherits="SalonCRM.Stat.OneDaySTA" %>

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
            <dx:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False" Width="100%"
                OnHeaderFilterFillItems="grid_HeaderFilterFillItems" SettingsPager-PageSize="20">
                <Columns>
                    <dx:GridViewDataTextColumn Caption="序号" FieldName="RowNum" VisibleIndex="0">
                        <Settings AllowSort="False" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="时间" FieldName="TheDay" Visible="false" VisibleIndex="1" PropertiesTextEdit-DisplayFormatString="yyyy-MM-dd">
                        <PropertiesTextEdit DisplayFormatString="yyyy-MM-dd"></PropertiesTextEdit>
                        <Settings AllowSort="False" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataHyperLinkColumn Caption="客户名称" FieldName="MemberId" VisibleIndex="2">
                        <PropertiesHyperLinkEdit DisplayFormatString="{0}" TextField="MemberName" NavigateUrlFormatString="~/MemberAdmin/Details/{0}"></PropertiesHyperLinkEdit>
                        <Settings AllowSort="False" />
                    </dx:GridViewDataHyperLinkColumn>
                    <dx:GridViewDataTextColumn Caption="卡号" FieldName="CardNo" VisibleIndex="3">
                        <Settings AllowSort="False" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewBandColumn Caption="耗卡" VisibleIndex="4">
                        <Columns>
                            <dx:GridViewDataTextColumn Caption="时间" FieldName="ExpenseTime" VisibleIndex="6" PropertiesTextEdit-DisplayFormatString="HH:mm">
                                <PropertiesTextEdit DisplayFormatString="HH:mm"></PropertiesTextEdit>
                                <Settings AllowSort="False" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="项目名称" FieldName="ProjectName" VisibleIndex="0" Width="200">
                                <Settings AllowSort="False" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="美容师" FieldName="Worker" VisibleIndex="1">
                                <Settings AllowSort="False" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="项目量" FieldName="ProjectNum" VisibleIndex="2">
                                <Settings AllowSort="False" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="实操" FieldName="A1" VisibleIndex="3">
                                <Settings AllowSort="False" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="卡扣" FieldName="A3" VisibleIndex="4">
                                <Settings AllowSort="False" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="即销<br>即耗" FieldName="A2" VisibleIndex="5">
                                <Settings AllowSort="False" />
                            </dx:GridViewDataTextColumn>
                        </Columns>
                    </dx:GridViewBandColumn>
                    <dx:GridViewBandColumn Caption="销售" VisibleIndex="11">
                        <Columns>
                            <dx:GridViewDataTextColumn Caption="面部" FieldName="S1" VisibleIndex="0">
                                <Settings AllowSort="False" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="身体" FieldName="S2" VisibleIndex="1">
                                <Settings AllowSort="False" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="仪器" FieldName="S3" VisibleIndex="2">
                                <Settings AllowSort="False" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="家居产品" FieldName="S4" VisibleIndex="3">
                                <Settings AllowSort="False" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="卡项" FieldName="S6" VisibleIndex="4">
                                <Settings AllowSort="False" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="其他" FieldName="S5" VisibleIndex="5">
                                <Settings AllowSort="False" />
                            </dx:GridViewDataTextColumn>
                        </Columns>
                    </dx:GridViewBandColumn>
                    <dx:GridViewBandColumn Caption="收款方式" VisibleIndex="18">
                        <Columns>
                            <dx:GridViewDataTextColumn Caption="现金" FieldName="T1" VisibleIndex="0">
                                <Settings AllowSort="False" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="刷卡" FieldName="T2" VisibleIndex="1">
                                <Settings AllowSort="False" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="转账" FieldName="T3" VisibleIndex="2">
                                <Settings AllowSort="False" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="欠款" FieldName="T4" VisibleIndex="3">
                                <Settings AllowSort="False" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="小计" FieldName="T5" VisibleIndex="4"
                                UnboundExpression="(IsNull([T1], 0)+IsNull([T2], 0)+IsNull([T3], 0)+ IsNull([T4], 0))" UnboundType="Decimal">
                                <Settings AllowSort="False" />
                            </dx:GridViewDataTextColumn>
                        </Columns>
                    </dx:GridViewBandColumn>

                </Columns>
                <SettingsPager PageSize="20"></SettingsPager>
                <Settings ShowFooter="true"></Settings>
                <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
                <TotalSummary>
                    <dx:ASPxSummaryItem FieldName="RowNum" SummaryType="Count" />
                    <dx:ASPxSummaryItem FieldName="ProjectNum" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="A1" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="A2" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="A3" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="S1" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="S2" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="S3" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="S4" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="S5" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="S6" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="T1" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="T2" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="T3" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="T4" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="T5" SummaryType="Sum" DisplayFormat="{0}" />
                </TotalSummary>
            </dx:ASPxGridView>

            <dx:ASPxGridViewExporter ID="gridExport" runat="server" GridViewID="grid"></dx:ASPxGridViewExporter>
        </form>
    </div>
</asp:Content>
