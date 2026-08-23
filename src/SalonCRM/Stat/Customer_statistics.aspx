<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Main.Master" CodeBehind="Customer_statistics.aspx.cs" Inherits="SalonCRM.Stat.Customer_statistics" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="header">
        <h1>客户统计</h1>
    </div>
    <div class="main-content">
        <form id="form1" runat="server">
            <table class="">
                <tr style="height: 45px;">
                    <td style="padding-right: 4px">门店</td>
                    <td style="padding-right: 4px" colspan="2">
                        <dx:ASPxComboBox ID="cbBranch" runat="server" ValueType="System.String" Width="180px">
                        </dx:ASPxComboBox>
                    </td>
                    <td class="dataLeftCell">客户类别</td>
                    <td style="padding-right: 4px" colspan="2">
                        <dx:ASPxComboBox ID="cbType" runat="server" ValueType="System.String" Width="180px">
                        </dx:ASPxComboBox>
                    </td>
                    <td class="dataLeftCell">客户等级</td>
                    <td style="padding-right: 4px">
                        <dx:ASPxComboBox ID="cbLevel" runat="server" ValueType="System.String" Width="180px">
                        </dx:ASPxComboBox>
                    </td>
                    <td class="dataLeftCell">姓名</td>
                    <td style="padding-right: 4px" colspan="2">
                        <dx:ASPxTextBox ID="tbName" runat="server" Width="180px"></dx:ASPxTextBox>
                    </td>
                </tr>
                <tr style="height: 45px;">
                    <td class="dataLeftCell">卡号</td>
                    <td style="padding-right: 4px" colspan="2">
                        <dx:ASPxTextBox ID="tbCard" runat="server" Width="180px"></dx:ASPxTextBox>
                    </td>
                    <td class="dataLeftCell">性别</td>
                    <td style="padding-right: 4px" colspan="2">
                        <dx:ASPxComboBox ID="cbSex" runat="server" Width="180px" ValueType="System.String">
                        </dx:ASPxComboBox>
                    </td>
                    <td class="dataLeftCell">到店频次</td>
                    <td style="padding-right: 4px">
                        <dx:ASPxTextBox ID="tbCount" runat="server" Width="180px"></dx:ASPxTextBox>
                    </td>
                    <td style="padding-right: 4px">入会区间</td>
                    <td style="padding-right: 4px">
                        <dx:ASPxDateEdit ID="deStart" EditFormatString="yyyy-MM-dd" runat="server" Width="100px">
                        </dx:ASPxDateEdit>
                    </td>
                    <td>
                        <dx:ASPxDateEdit ID="deEnd" EditFormatString="yyyy-MM-dd" runat="server" Width="100px">
                        </dx:ASPxDateEdit>
                    </td>
                </tr>
                <tr>
                    <td style="padding-right: 4px">充值金额</td>
                    <td style="padding-right: 4px;">
                        <dx:ASPxTextBox ID="tbAmtStart" runat="server" Width="90px"></dx:ASPxTextBox>
                    </td>
                    <td>
                        <dx:ASPxTextBox ID="tbAmtEnd" runat="server" Width="90px"></dx:ASPxTextBox>
                    </td>
                    <td style="padding-right: 4px">消费区间</td>
                    <td style="padding-right: 4px;">
                        <dx:ASPxTextBox ID="tbUseStart" runat="server" Width="90px"></dx:ASPxTextBox>
                    </td>
                    <td>
                        <dx:ASPxTextBox ID="tbUseEnd" runat="server" Width="90px"></dx:ASPxTextBox>
                    </td>
                    <td colspan="3"></td>
                    <td style="padding-right: 4px">
                        <dx:ASPxButton ID="btnStat1" runat="server" Text="查询" OnClick="btnStat_Click"></dx:ASPxButton>
                    </td>
                    <td style="padding-right: 4px">
                        <dx:ASPxButton ID="btnXlsExport" runat="server" Text="导出Excel" UseSubmitBehavior="False" OnClick="btnXlsExport_Click" />
                    </td>
                </tr>
            </table>
            <br />
            <div>
                <img class="left" src="../Content/stat_color.png" />
            </div>
            <dx:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False"
                OnHeaderFilterFillItems="grid_HeaderFilterFillItems" Width="100%" SettingsPager-PageSize="20" OnHtmlRowPrepared="grid_HtmlRowPrepared">
                <Columns>
                    <dx:GridViewDataTextColumn Caption="门店名称" FieldName="BranchName" VisibleIndex="0">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="卡号" FieldName="CardNo" VisibleIndex="1">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataHyperLinkColumn Caption="客户名称" FieldName="MemberId" VisibleIndex="2">
                        <PropertiesHyperLinkEdit DisplayFormatString="{0}" TextField="Name" NavigateUrlFormatString="~/MemberAdmin/Details/{0}">
                        </PropertiesHyperLinkEdit>
                        <CellStyle HorizontalAlign="Left">
                        </CellStyle>
                    </dx:GridViewDataHyperLinkColumn>
                    <dx:GridViewDataTextColumn Caption="姓名" FieldName="Name" VisibleIndex="3" Visible="false">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="电话" FieldName="MobileNumber" VisibleIndex="4">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="类别" FieldName="Type" VisibleIndex="5">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="级别" FieldName="Level" VisibleIndex="6">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="状态" FieldName="Status" VisibleIndex="7">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="入会日期" FieldName="JoinDate" VisibleIndex="8" PropertiesTextEdit-DisplayFormatString="yyyy-MM-dd">
                        <PropertiesTextEdit DisplayFormatString="yyyy-MM-dd"></PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="充值<br>金额" FieldName="RechargeAmount" VisibleIndex="9" PropertiesTextEdit-DisplayFormatString="g">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="实耗<br>金额" FieldName="ExpenseAmount" VisibleIndex="10" PropertiesTextEdit-DisplayFormatString="g">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="剩余<br>金额" FieldName="RemainingAmount" VisibleIndex="11" PropertiesTextEdit-DisplayFormatString="g">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="项目数" FieldName="ProjectNumber" VisibleIndex="12">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="剩余<br>次数" FieldName="RemainingNumber" VisibleIndex="13">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="最后服务日期" FieldName="LastService" VisibleIndex="14" PropertiesTextEdit-DisplayFormatString="yyyy-MM-dd">
                        <PropertiesTextEdit DisplayFormatString="yyyy-MM-dd"></PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="到店<br>次数" FieldName="BookTime" VisibleIndex="15">
                    </dx:GridViewDataTextColumn>
                </Columns>
                <Settings ShowFooter="true" />
                <SettingsPager PageSize="20"></SettingsPager>

                <TotalSummary>
                    <dx:ASPxSummaryItem FieldName="RechargeAmount" ValueDisplayFormat="#0" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="ExpenseAmount" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="RemainingAmount" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="ProjectNumber" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="RemainingNumber" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="BookTime" SummaryType="Sum" DisplayFormat="{0}" />
                </TotalSummary>
            </dx:ASPxGridView>

            <dx:ASPxGridViewExporter ID="gridExport" runat="server" GridViewID="grid"></dx:ASPxGridViewExporter>

        </form>
    </div>
</asp:Content>
