<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Customer_consumption.aspx.cs" Inherits="SalonCRM.Stat.Customer_consumption" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="header">
        <h1>客户消费/消耗一览表</h1>
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
                    <td style="padding-right: 4px">姓名</td>
                    <td style="padding-right: 4px">
                        <dx:ASPxTextBox ID="txt_Name" runat="server" Width="170px"></dx:ASPxTextBox>
                    </td>
                    <td style="padding-right: 4px">卡号</td>
                    <td style="padding-right: 4px">
                        <dx:ASPxTextBox ID="txt_CardNo" runat="server" Width="170px"></dx:ASPxTextBox>
                    </td>
                    <td style="padding-right: 4px">金额</td>
                    <td style="padding-right: 4px">
                        <dx:ASPxTextBox ID="txt_Amount_s" runat="server" Width="70px"></dx:ASPxTextBox>
                    </td>
                    <td style="padding-right: 4px">至</td>
                    <td style="padding-right: 4px">
                        <dx:ASPxTextBox ID="txt_Amount_b" runat="server" Width="70px"></dx:ASPxTextBox>
                    </td>
                    <td></td>
                </tr>
                <tr>
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
                    <td style="padding-right: 4px">美容顾问</td>
                    <td style="padding-right: 4px">
                        <dx:ASPxComboBox ID="cbSalesman" ClientInstanceName="cbSalesman" runat="server">
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
            <dx:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False" Width="100%" KeyFieldName="MemberId" SettingsPager-PageSize="20">
                <Columns>
                    <dx:GridViewDataTextColumn Caption="门店名称" VisibleIndex="0" FieldName="BranchName">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="客户姓名" VisibleIndex="1" FieldName="Name">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="销售金额" VisibleIndex="2" FieldName="RechargeAmount">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption=" 虚耗金额（改实耗）" VisibleIndex="3" FieldName="ExpenseAmount">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="项目编码" VisibleIndex="4" FieldName="ProjectCode">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="项目名称" VisibleIndex="5" FieldName="ProjectName">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="购买次数" VisibleIndex="6" FieldName="BookTime">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="购买日期" VisibleIndex="7" FieldName="CreatedDate" PropertiesTextEdit-DisplayFormatString="yyyy-MM-dd HH:mm:ss" SortIndex="1" SortOrder="Descending">
                        <PropertiesTextEdit DisplayFormatString="yyyy-MM-dd HH:mm:ss"></PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="客户ID" VisibleIndex="8" FieldName="MemberId" SortIndex="0" Visible="False">
                    </dx:GridViewDataTextColumn>
                </Columns>
            </dx:ASPxGridView>

            <dx:ASPxGridViewExporter ID="gridExport" runat="server" GridViewID="grid"></dx:ASPxGridViewExporter>

        </form>
    </div>
</asp:Content>

