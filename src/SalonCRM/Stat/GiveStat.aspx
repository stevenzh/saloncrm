<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="GiveStat.aspx.cs" Inherits="SalonCRM.Stat.GiveStat" %>

<asp:Content ID="Content2" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="header">
        <h1>客户赠送一览表</h1>
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
                        <dx:ASPxDateEdit ID="deEnd" EditFormatString="yyyy-MM-dd" runat="server">
                        </dx:ASPxDateEdit>
                    </td>
                    <td style="padding-right: 4px">
                        <dx:ASPxButton ID="btnStat" runat="server" Text="查询" OnClick="btnStat_Click">
                        </dx:ASPxButton>
                    </td>
                    <td style="padding-right: 4px">
                        <dx:ASPxButton ID="btnXlsExport" runat="server" Text="导出Excel" UseSubmitBehavior="False" OnClick="btnXlsExport_Click" />
                    </td>
                </tr>
            </table>
            <br />
            <dx:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False" Width="100%" SettingsPager-PageSize="20">
                <Columns>
                    <dx:GridViewDataTextColumn Caption="门店ID" VisibleIndex="0" FieldName="OrganID" Visible="false">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="门店名称" VisibleIndex="1" FieldName="BranchName">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="卡号" VisibleIndex="2" FieldName="MemberCardNo">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="客户" VisibleIndex="3" FieldName="MemberName">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="赠送积分" VisibleIndex="4" FieldName="InPoints">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="可用积分" VisibleIndex="5" FieldName="RemainPoints" Visible="false">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="赠送项目" VisibleIndex="6" FieldName="ProjectName">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="次数" VisibleIndex="7" FieldName="BookTime">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataDateColumn Caption="赠送时间" FieldName="CreatedDate" VisibleIndex="8" PropertiesDateEdit-DisplayFormatString="yyyy-MM-dd HH:mm:ss">
                    </dx:GridViewDataDateColumn>
                    <dx:GridViewDataDateColumn Caption="使用有效期" FieldName="ExpiryDate" VisibleIndex="9" PropertiesDateEdit-DisplayFormatString="yyyy-MM-dd">
                    </dx:GridViewDataDateColumn>
                    <dx:GridViewDataTextColumn Caption="使用项目" VisibleIndex="10" FieldName="FinalProject">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataDateColumn Caption="使用日期" FieldName="ServiceDate" VisibleIndex="11" PropertiesDateEdit-DisplayFormatString="yyyy-MM-dd">
                    </dx:GridViewDataDateColumn>

                </Columns>
            </dx:ASPxGridView>

            <dx:ASPxGridViewExporter ID="gridExport" runat="server" GridViewID="grid"></dx:ASPxGridViewExporter>

        </form>
    </div>
</asp:Content>
