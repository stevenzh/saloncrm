<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Main.Master" CodeBehind="Customer_attendance.aspx.cs" Inherits="SalonCRM.Stat.Customer_attendance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="header">
        <h1>客户考勤月报表</h1>
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
                    <td style="padding-right: 4px">年份</td>
                    <td style="padding-right: 4px">
                        <dx:ASPxComboBox ID="cbYear" runat="server" ValueType="System.String">
                        </dx:ASPxComboBox>
                    </td>
                    <td style="padding-right: 4px">客户类别</td>
                    <td style="padding-right: 4px">
                        <dx:ASPxComboBox ID="cbType" runat="server" ValueType="System.String">
                        </dx:ASPxComboBox>
                    </td>
                    <td style="padding-right: 4px">
                        <dx:ASPxButton ID="btnStat1" runat="server" Text="查询" OnClick="btnStat_Click"></dx:ASPxButton>
                    </td>
                    <td style="padding-right: 4px">
                        <dx:ASPxButton ID="btnXlsExport" runat="server" Text="导出Excel" UseSubmitBehavior="False" OnClick="btnXlsExport_Click" />
                    </td>
                </tr>
            </table>
            <br />
            <div id="chart" style="height: 300px"></div>

            <dx:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False" Width="100%" SettingsPager-PageSize="20">
                <Columns>
                    <dx:GridViewDataTextColumn Caption="序号" FieldName="Record" VisibleIndex="0">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="门店名称" FieldName="dianmian" VisibleIndex="1">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="客户" FieldName="Name" VisibleIndex="2">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="客户类型" FieldName="Type" VisibleIndex="3">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="卡号" FieldName="CardNo" VisibleIndex="4">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="手机" FieldName="MobileNumber" VisibleIndex="5">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="一月" FieldName="Jan" VisibleIndex="6">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="二月" FieldName="Feb" VisibleIndex="7">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="三月" FieldName="Mar" VisibleIndex="8">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="四月" FieldName="Apr" VisibleIndex="9">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="五月" FieldName="May" VisibleIndex="10">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="六月" FieldName="Jun" VisibleIndex="11">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="七月" FieldName="Jul" VisibleIndex="12">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="八月" FieldName="Aug" VisibleIndex="13">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="九月" FieldName="Sep" VisibleIndex="14">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="十月" FieldName="Oct" VisibleIndex="15">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="十一月" FieldName="Nov" VisibleIndex="16">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="十二月" FieldName="Dec" VisibleIndex="17">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="合计" FieldName="OY" VisibleIndex="18" UnboundExpression="(IsNull([Jan], 0)+IsNull([Feb], 0)+IsNull([Mar], 0)+IsNull([Apr], 0)+IsNull([May], 0)+IsNull([Jun], 0)+IsNull([Jul], 0)+IsNull([Aug], 0)+IsNull([Sep], 0)+IsNull([Oct], 0)+IsNull([Nov], 0)+IsNull([Dec], 0))" UnboundType="Integer">
                    </dx:GridViewDataTextColumn>
                </Columns>

                <Settings ShowFooter="true" />
                <TotalSummary>
                    <dx:ASPxSummaryItem FieldName="Jan" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="Feb" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="Mar" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="Apr" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="May" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="Jun" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="Jul" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="Aug" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="Sep" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="Oct" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="Nov" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="Dec" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="OY" SummaryType="Sum" DisplayFormat="{0}" />
                </TotalSummary>
            </dx:ASPxGridView>

            <dx:ASPxGridViewExporter ID="gridExport" runat="server" GridViewID="grid"></dx:ASPxGridViewExporter>

        </form>

    </div>
    <!-- ECharts单文件引入 -->
    <script src="http://cdn.bootcss.com/echarts/3.4.0/echarts.min.js"></script>
    <script type="text/javascript">

        // 基于准备好的dom，初始化echarts图表
        var myChart = echarts.init(document.getElementById('chart'));

        var option = {
            tooltip: {
                trigger: 'axis'
            },
            legend: {
                data: ['总人数']
            },
            toolbox: {
                show: true,
                feature: {
                    mark: { show: true },
                    dataView: { show: true, readOnly: false },
                    magicType: { show: true, type: ['line', 'bar', 'stack', 'tiled'] },
                    restore: { show: true },
                    saveAsImage: { show: true }
                }
            },
            calculable: true,
            xAxis: [
                {
                    type: 'category',
                    boundaryGap: false,
                    data: ['一月','二月','三月','四月','五月','六月','七月','八月','九月','十月','十一月','十二月']
                }
            ],
            yAxis: [
                {
                    type: 'value'
                }
            ],
            series: [
                {
                    name: '总人数',
                    type: 'line',
                    stack: '总量',
                    data: <%=Kaoqing %>
                    }
            ]
        };

        // 为echarts对象加载数据
        myChart.setOption(option);

    </script>
</asp:Content>
