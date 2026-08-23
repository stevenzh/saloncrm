<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Main.Master" CodeBehind="Financial_Debts.aspx.cs" Inherits="SalonCRM.Stat.Financial_Debts" %>

<asp:Content ID="Content2" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="header">
        <h1>欠款一览表</h1>
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
                    <td style="padding-right: 4px">卡号</td>
                    <td style="padding-right: 4px">
                        <dx:ASPxTextBox ID="txt_CardNo" runat="server" Width="170px"></dx:ASPxTextBox>
                    </td>
                    <td style="padding-right: 4px">姓名</td>
                    <td style="padding-right: 4px">
                        <dx:ASPxTextBox ID="txt_Name" runat="server" Width="170px"></dx:ASPxTextBox>
                    </td>
                    <td style="padding-right: 4px">美容顾问</td>
                    <td style="padding-right: 4px">
                        <dx:ASPxTextBox ID="txt_xsr" runat="server" Width="170px"></dx:ASPxTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="padding-right: 4px">日期</td>
                    <td style="padding-right: 4px">
                        <dx:ASPxDateEdit ID="deStart" EditFormatString="yyyy-MM-dd" runat="server">
                        </dx:ASPxDateEdit>
                    </td>
                    <td style="padding-right: 4px">日期</td>
                    <td style="padding-right: 4px">
                        <dx:ASPxDateEdit ID="deEnd" EditFormatString="yyyy-MM-dd" runat="server">
                        </dx:ASPxDateEdit>
                    </td>
                    <td style="padding-right: 4px">欠款人</td>
                    <td style="padding-right: 4px">
                        <dx:ASPxTextBox ID="txt_qkr" runat="server" Width="170px"></dx:ASPxTextBox>
                    </td>
                    <td style="padding-right: 4px">还款状态</td>
                    <td style="padding-right: 4px">
                        <dx:ASPxRadioButtonList ID="ASPxRadioButtonList1" runat="server" Border-BorderStyle="None" RepeatDirection="Horizontal" RepeatLayout="Flow" SelectedIndex="0">
                            <Items>
                                <dx:ListEditItem Selected="True" Text="未还" Value="1" />
                                <dx:ListEditItem Text="已还" Value="2" />
                            </Items>
                        </dx:ASPxRadioButtonList>
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
            <dx:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False" Width="100%"
                OnHeaderFilterFillItems="grid_HeaderFilterFillItems">
                <Columns>
                    <dx:GridViewDataTextColumn Caption="门店名称" VisibleIndex="1" FieldName="BranchName">
                        <FooterTemplate>
                            汇总
                        </FooterTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="客户" VisibleIndex="2" FieldName="MemberName">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="卡号" VisibleIndex="3" FieldName="CardNo">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="项目" VisibleIndex="5" FieldName="ProjectName">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="次数" VisibleIndex="6" FieldName="Quantity">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="总应收" VisibleIndex="7" FieldName="Amount">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="欠款金额" VisibleIndex="8" FieldName="Debt">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataColumn Caption="欠款状态" VisibleIndex="9">
                        <DataItemTemplate>
                             <%# Eval("Status").ToString().Equals("2") ? "已还": "未还"  %>
                        </DataItemTemplate>
                    </dx:GridViewDataColumn>
                    <dx:GridViewDataTextColumn Caption="美容顾问" VisibleIndex="10" FieldName="Salesman">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataDateColumn Caption="销售日期" VisibleIndex="11" FieldName="CreatedDate" PropertiesDateEdit-DisplayFormatString="yyyy-MM-dd HH:mm">
                        <PropertiesDateEdit DisplayFormatString="yyyy-MM-dd HH:mm"></PropertiesDateEdit>
                    </dx:GridViewDataDateColumn>
                    <dx:GridViewDataColumn Caption="操作" VisibleIndex="12">
                        <DataItemTemplate>
                             <%# Eval("Status").ToString().Equals("2") ? "": "<a href=\"/Debt/Repayment/"+ Eval("MemberProjectId")+"?cardid=" + Eval("MemberCardId") + "\">还款</a>"  %>
                        </DataItemTemplate>
                    </dx:GridViewDataColumn>
                    <dx:GridViewDataTextColumn Caption="卡标题" VisibleIndex="4" FieldName="CardTitle">
                    </dx:GridViewDataTextColumn>
                </Columns>
                <Settings ShowFooter="true" />
                <TotalSummary>
                    <dx:ASPxSummaryItem FieldName="Debt" SummaryType="Sum" />
                </TotalSummary>
            </dx:ASPxGridView>

            <dx:ASPxGridViewExporter ID="gridExport" runat="server" GridViewID="grid"></dx:ASPxGridViewExporter>

        </form>
    </div>
</asp:Content>
