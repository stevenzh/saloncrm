<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ServiceDay.aspx.cs" Inherits="SalonCRM.Stat.ServiceDay" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="header">
        <h1>项目消耗明细表</h1>
    </div>
    <div class="main-content">
        <form id="form1" runat="server">
            <table class="">
                <tr style="height: 45px;">
                    <td style="padding-right: 4px">门店</td>
                    <td style="padding-right: 4px">
                        <dx:ASPxComboBox ID="cbBranch" runat="server" ValueType="System.String">
                        </dx:ASPxComboBox>
                    </td>
                    <td style="padding-right: 4px">品牌</td>
                    <td style="padding-right: 4px">
                        <dx:ASPxTextBox ID="txt_Brand" runat="server" Width="170px"></dx:ASPxTextBox>
                    </td>
                    <td style="padding-right: 4px">类别</td>
                    <td style="padding-right: 4px">
                        <dx:ASPxComboBox ID="cbCategory" runat="server" ValueType="System.String">
                        </dx:ASPxComboBox>
                    </td>
                    <td style="padding-right: 4px">项目名</td>
                    <td style="padding-right: 4px">
                        <dx:ASPxTextBox ID="txt_Name" runat="server" Width="170px"></dx:ASPxTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="padding-right: 4px">年度</td>
                    <td style="padding-right: 4px">
                        <dx:ASPxComboBox ID="cbYear" runat="server">
                        </dx:ASPxComboBox>
                    </td>
                    <td style="padding-right: 4px">月份</td>
                    <td style="padding-right: 4px">
                        <dx:ASPxComboBox ID="cbMonth" runat="server">
                            <Items>
                                <dx:ListEditItem Text="一月" Value="1" />
                                <dx:ListEditItem Text="二月" Value="2" />
                                <dx:ListEditItem Text="三月" Value="3" />
                                <dx:ListEditItem Text="四月" Value="4" />
                                <dx:ListEditItem Text="五月" Value="5" />
                                <dx:ListEditItem Text="六月" Value="6" />
                                <dx:ListEditItem Text="七月" Value="7" />
                                <dx:ListEditItem Text="八月" Value="8" />
                                <dx:ListEditItem Text="九月" Value="9" />
                                <dx:ListEditItem Text="十月" Value="10" />
                                <dx:ListEditItem Text="十一月" Value="11" />
                                <dx:ListEditItem Text="十二月" Value="12" />
                            </Items>
                        </dx:ASPxComboBox>
                    </td>
                    <td style="padding-right: 4px">分组方式</td>
                    <td style="padding-right: 16px">
                        <dx:ASPxComboBox runat="server" ID="cbFields" ValueType="System.Int32" SelectedIndex="0">
                            <Items>
                                <dx:ListEditItem Text="门店" Value="0" />
                                <dx:ListEditItem Text="项目" Value="1" />
                            </Items>
                            <ClientSideEvents SelectedIndexChanged="function(s) { grid.PerformCallback(s.GetValue()) }" />
                        </dx:ASPxComboBox>
                    </td>
                    <td style="padding-right: 4px">
                        <dx:ASPxButton ID="btnStat" runat="server" Text="查看" OnClick="btnStat_Click">
                        </dx:ASPxButton>
                    </td>
                    <td style="padding-right: 4px">
                        <dx:ASPxButton ID="btnXlsExport" runat="server" Text="导出Excel" UseSubmitBehavior="False" OnClick="btnXlsExport_Click" />
                    </td>
                </tr>
            </table>
            <br />
            <dx:ASPxGridView ID="grid" ClientInstanceName="grid" runat="server" AutoGenerateColumns="False" Width="100%" OnCustomCallback="grid_CustomCallback" SettingsPager-PageSize="20">
                <Columns>
                    <dx:GridViewDataTextColumn Caption="序号" VisibleIndex="0" FieldName="Record" Visible="false">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="门店名称" VisibleIndex="1" FieldName="BranchName">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="项目" VisibleIndex="2" FieldName="ProjectName">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="1" FieldName="D1" VisibleIndex="3">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="2" FieldName="D2" VisibleIndex="4">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="3" FieldName="D3" VisibleIndex="5">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="4" FieldName="D4" VisibleIndex="6">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="5" FieldName="D5" VisibleIndex="7">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="6" FieldName="D6" VisibleIndex="8">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="7" FieldName="D7" VisibleIndex="9">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="8" FieldName="D8" VisibleIndex="10">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="9" FieldName="D9" VisibleIndex="11">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="10" FieldName="D10" VisibleIndex="12">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="11" FieldName="D11" VisibleIndex="13">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="12" FieldName="D12" VisibleIndex="14">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="13" FieldName="D13" VisibleIndex="15">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="14" FieldName="D14" VisibleIndex="16">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="15" FieldName="D15" VisibleIndex="17">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="16" FieldName="D16" VisibleIndex="18">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="17" FieldName="D17" VisibleIndex="19">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="18" FieldName="D18" VisibleIndex="20">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="19" FieldName="D19" VisibleIndex="21">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="20" FieldName="D20" VisibleIndex="22">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="21" FieldName="D21" VisibleIndex="23">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="22" FieldName="D22" VisibleIndex="24">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="23" FieldName="D23" VisibleIndex="25">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="24" FieldName="D24" VisibleIndex="26">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="25" FieldName="D25" VisibleIndex="27">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="26" FieldName="D26" VisibleIndex="28">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="27" FieldName="D27" VisibleIndex="29">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="28" FieldName="D28" VisibleIndex="30">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="29" FieldName="D29" VisibleIndex="31">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="30" FieldName="D30" VisibleIndex="32">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="31" FieldName="D31" VisibleIndex="33">
                    </dx:GridViewDataTextColumn>
                </Columns>
                <SettingsPager PageSize="20"></SettingsPager>
                <Settings ShowGroupPanel="True" ShowFooter="True" />
                <TotalSummary>
                    <dx:ASPxSummaryItem FieldName="D1" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="D2" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="D3" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="D4" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="D5" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="D6" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="D7" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="D8" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="D9" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="D10" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="D11" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="D12" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="D13" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="D14" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="D15" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="D16" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="D17" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="D18" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="D19" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="D20" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="D21" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="D22" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="D23" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="D24" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="D25" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="D26" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="D27" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="D28" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="D29" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="D30" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="D31" SummaryType="Sum" DisplayFormat="{0}" />
                </TotalSummary>
            </dx:ASPxGridView>

            <dx:ASPxGridViewExporter ID="gridExport" runat="server" GridViewID="grid"></dx:ASPxGridViewExporter>

        </form>
    </div>
</asp:Content>
