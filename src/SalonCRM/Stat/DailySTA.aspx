<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="DailySTA.aspx.cs" Inherits="SalonCRM.Stat.DailySTA" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="header">
        <h1>日报表</h1>
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
                        <dx:ASPxButton ID="btnStat" runat="server" Text="统计" OnClick="btnStat_Click">
                        </dx:ASPxButton>
                    </td>
                    <td style="padding-right: 4px">
                        <dx:ASPxButton ID="btnXlsExport" runat="server" Text="导出Excel" UseSubmitBehavior="False" OnClick="btnXlsExport_Click" />
                    </td>
                </tr>
            </table>
            <br />

            <div id="chart" style="height: 300px"></div>

            <dx:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False" OnHeaderFilterFillItems="grid_HeaderFilterFillItems"
                Settings-ShowHeaderFilterButton="true" Settings-ShowFilterRow="true" SettingsPager-PageSize="20">
                <Columns>
                    <dx:GridViewDataTextColumn Caption="&nbsp;&nbsp;&nbsp;日期&nbsp;&nbsp;&nbsp;" FieldName="TheDay" SortOrder="Descending" VisibleIndex="0"
                        PropertiesTextEdit-DisplayFormatString="yyyy-MM-dd" Width="80">
                        <PropertiesTextEdit DisplayFormatString="yyyy-MM-dd"></PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="客流量" FieldName="Flow" VisibleIndex="1">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewBandColumn Caption="消耗" VisibleIndex="3">
                        <Columns>
                            <dx:GridViewDataTextColumn Caption="项目量" FieldName="ProjectNum" VisibleIndex="1">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="实操" FieldName="A1" VisibleIndex="2">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="即销<br>即耗" FieldName="A2" VisibleIndex="3">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="小计" FieldName="A4" UnboundExpression="(IsNull([A1], 0)+IsNull([A2], 0))" UnboundType="Decimal"
                                VisibleIndex="4">
                            </dx:GridViewDataTextColumn>
                        </Columns>
                    </dx:GridViewBandColumn>
                    <dx:GridViewBandColumn Caption="销售" VisibleIndex="4">
                        <Columns>
                            <dx:GridViewDataTextColumn Caption="面部" FieldName="S1" VisibleIndex="0">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="身体" FieldName="S2" VisibleIndex="1">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="仪器" FieldName="S3" VisibleIndex="2">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="家居产品" FieldName="S4" VisibleIndex="3">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="其他" FieldName="S5" VisibleIndex="5">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="卡项" FieldName="S6" VisibleIndex="4">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="小计" FieldName="S8"
                                UnboundExpression="(IsNull([S1], 0)+IsNull([S2], 0)+IsNull([S3], 0)+IsNull([S4], 0)+IsNull([S5], 0)+IsNull([S6], 0))"
                                UnboundType="Decimal" VisibleIndex="6">
                            </dx:GridViewDataTextColumn>
                        </Columns>
                    </dx:GridViewBandColumn>
                    <dx:GridViewBandColumn Caption="收款" VisibleIndex="11">
                        <Columns>
                            <dx:GridViewDataTextColumn Caption="现金" FieldName="T1" VisibleIndex="0">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="刷卡" FieldName="T2" VisibleIndex="1">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="转账" FieldName="T3" VisibleIndex="2">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="欠款" FieldName="T4" VisibleIndex="3">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="应收" FieldName="T5" VisibleIndex="4"
                                UnboundExpression="(IsNull([T1], 0)+IsNull([T2], 0)+IsNull([T3], 0)+IsNull([T4], 0))" UnboundType="Decimal">
                            </dx:GridViewDataTextColumn>
                        </Columns>
                    </dx:GridViewBandColumn>
                    <dx:GridViewDataColumn Caption="操作" VisibleIndex="12">
                        <DataItemTemplate>
                            <a href="OneDaySTA.aspx?branchId=<%# Eval("BranchId")%>&theDay=<%# Eval("TheDay") %>">详细</a>
                        </DataItemTemplate>
                    </dx:GridViewDataColumn>
                    <dx:GridViewDataTextColumn Caption="卡扣" FieldName="A3" VisibleIndex="2">
                    </dx:GridViewDataTextColumn>
                </Columns>

                <SettingsPager PageSize="20"></SettingsPager>

                <Settings ShowFilterRow="True" ShowHeaderFilterButton="True"></Settings>
                <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
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
                data: ['卡扣', '销售', '收款']
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
                    data: <%= DayStr %>
                 }
            ],
            yAxis: [
                {
                    type: 'value'
                }
            ],
            series: [
                {
                    name: '耗卡',
                    type: 'line',
                    stack: '总量',
                    data: <%=Haokai %>
                    },
                {
                    name: '销售',
                    type: 'line',
                    stack: '总量',
                    data: <%=Xiaoshou %>
                    },
                {
                    name: '收款',
                    type: 'line',
                    stack: '总量',
                    data: <%=Shoukuan %>
                    }
            ]
        };

        // 为echarts对象加载数据
        myChart.setOption(option);

    </script>
</asp:Content>
