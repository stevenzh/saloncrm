<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Main.Master" CodeBehind="Financial_Service_details.aspx.cs" Inherits="SalonCRM.Stat.Financial_Service_details" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="header">
        <h1>
            <asp:Label ID="lbl_Employee" runat="server"></asp:Label>
            美容师服务明细</h1>
    </div>
    <div class="main-content">
        <form id="form1" runat="server">
            <table class="OptionsTable BottomMargin">
                <tr>
                    <td style="padding-right: 4px">
                        <dx:ASPxButton ID="btnXlsExport" runat="server" Text="导出Excel" UseSubmitBehavior="False" OnClick="btnXlsExport_Click" />
                    </td>
                </tr>
            </table>
            <br />
            <dx:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False" Width="100%">
                <Columns>
                    <dx:GridViewDataTextColumn Caption="客户" VisibleIndex="0" FieldName="MemberName">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="日期" VisibleIndex="1" SortOrder="Ascending" FieldName="TheTime" PropertiesTextEdit-DisplayFormatString="yyyy-MM-dd">
                        <PropertiesTextEdit DisplayFormatString="yyyy-MM-dd"></PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="充值时间" VisibleIndex="2" FieldName="IncomeTime" PropertiesTextEdit-DisplayFormatString="HH:mm">
                        <PropertiesTextEdit DisplayFormatString="HH:mm"></PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="业绩" VisibleIndex="3" FieldName="InCome" PropertiesTextEdit-DisplayFormatString="#0.00">
                        <PropertiesTextEdit DisplayFormatString="#0.00"></PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="购买时间" VisibleIndex="4" FieldName="SalesTime" PropertiesTextEdit-DisplayFormatString="HH:mm">
                        <PropertiesTextEdit DisplayFormatString="HH:mm"></PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="购买项目" VisibleIndex="5" FieldName="InSales" PropertiesTextEdit-DisplayFormatString="#0.00">
                        <PropertiesTextEdit DisplayFormatString="#0.00"></PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="服务时间" VisibleIndex="6" FieldName="ExpendTime" PropertiesTextEdit-DisplayFormatString="HH:mm">
                        <PropertiesTextEdit DisplayFormatString="HH:mm"></PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="服务项目" VisibleIndex="7" FieldName="PPT" UnboundExpression="Iif([ProjetSaleType]=='1', '[赠]', '') + [ProjectName]" UnboundType="String">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="顾问消耗业绩" VisibleIndex="8" FieldName="SalesExpend" PropertiesTextEdit-DisplayFormatString="#0.00">
                        <PropertiesTextEdit DisplayFormatString="#0.00"></PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="消耗" VisibleIndex="9" FieldName="Expend" PropertiesTextEdit-DisplayFormatString="#0.00">
                        <PropertiesTextEdit DisplayFormatString="#0.00"></PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="服务项目量" VisibleIndex="10" FieldName="ServiceXC">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="手工费" VisibleIndex="11" FieldName="HandicraftFee" >
                    </dx:GridViewDataTextColumn>
                </Columns>
                <Settings ShowFooter="True" />
                <TotalSummary>
                    <dx:ASPxSummaryItem FieldName="InCome" SummaryType="Sum" />
                    <dx:ASPxSummaryItem FieldName="InSales" SummaryType="Sum" />
                    <dx:ASPxSummaryItem FieldName="Expend" SummaryType="Sum" />
                    <dx:ASPxSummaryItem FieldName="ServiceXC" SummaryType="Sum" />
                    <dx:ASPxSummaryItem FieldName="SalesExpend" SummaryType="Sum" />
                    <dx:ASPxSummaryItem FieldName="HandicraftFee" SummaryType="Sum" />
                </TotalSummary>
            </dx:ASPxGridView>

            <dx:ASPxGridViewExporter ID="gridExport" runat="server" GridViewID="grid"></dx:ASPxGridViewExporter>

        </form>
    </div>
</asp:Content>
