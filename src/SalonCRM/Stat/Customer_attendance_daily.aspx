<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Main.Master" CodeBehind="Customer_attendance_daily.aspx.cs" Inherits="SalonCRM.Stat.Customer_attendance_daily" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="header">
        <h1>客户考勤日报表</h1>
    </div>
    <div class="main-content">
        <form name="form1" runat="server">
            <table class="">
                <tr>
                    <td style="padding-right: 4px">门店</td>
                    <td style="padding-right: 4px">
                        <dx:ASPxComboBox ID="cbBranch" runat="server" ValueType="System.String">
                        </dx:ASPxComboBox>
                    </td>
                    <td style="padding-right: 4px">客户类别</td>
                    <td style="padding-right: 4px">
                        <dx:ASPxComboBox ID="cbType" runat="server" ValueType="System.String">
                        </dx:ASPxComboBox>
                    </td>
                    <td style="padding-right: 4px">年份</td>
                    <td style="padding-right: 4px">
                        <dx:ASPxComboBox ID="cbYear" runat="server" ValueType="System.String">
                        </dx:ASPxComboBox>
                    </td>
                    <td style="padding-right: 4px">月份</td>
                    <td style="padding-right: 4px">
                        <dx:ASPxComboBox ID="cbMonth" runat="server" SelectedIndex="4">
                            <Items>
                                <dx:ListEditItem Selected="true" />
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
            <div id="chart" style="height: 300px"></div>
            <dx:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False" Width="100%"
                SettingsPager-PageSize="20" OnHtmlDataCellPrepared="grid_HtmlDataCellPrepared">
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
                    <dx:GridViewDataTextColumn Caption="1" FieldName="D1" VisibleIndex="6">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="2" FieldName="D2" VisibleIndex="7">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="3" FieldName="D3" VisibleIndex="8">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="4" FieldName="D4" VisibleIndex="9">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="5" FieldName="D5" VisibleIndex="10">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="6" FieldName="D6" VisibleIndex="11">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="7" FieldName="D7" VisibleIndex="12">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="8" FieldName="D8" VisibleIndex="13">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="9" FieldName="D9" VisibleIndex="14">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="10" FieldName="D10" VisibleIndex="15">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="11" FieldName="D11" VisibleIndex="16">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="12" FieldName="D12" VisibleIndex="17">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="13" FieldName="D13" VisibleIndex="18">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="14" FieldName="D14" VisibleIndex="19">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="15" FieldName="D15" VisibleIndex="20">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="16" FieldName="D16" VisibleIndex="21">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="17" FieldName="D17" VisibleIndex="22">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="18" FieldName="D18" VisibleIndex="23">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="19" FieldName="D19" VisibleIndex="24">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="20" FieldName="D20" VisibleIndex="25">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="21" FieldName="D21" VisibleIndex="26">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="22" FieldName="D22" VisibleIndex="27">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="23" FieldName="D23" VisibleIndex="28">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="24" FieldName="D24" VisibleIndex="29">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="25" FieldName="D25" VisibleIndex="30">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="26" FieldName="D26" VisibleIndex="31">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="27" FieldName="D27" VisibleIndex="32">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="28" FieldName="D28" VisibleIndex="33">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="29" FieldName="D29" VisibleIndex="34">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="30" FieldName="D30" VisibleIndex="35">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="31" FieldName="D31" VisibleIndex="36">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="合计" FieldName="OY" VisibleIndex="37" UnboundExpression="(IsNull([D1], 0)+IsNull([D2], 0)+IsNull([D3], 0)+IsNull([D4], 0)+IsNull([D5], 0)+IsNull([D6], 0)+IsNull([D7], 0)+IsNull([D8], 0)+IsNull([D9], 0)+IsNull([D10], 0)+IsNull([D11], 0)+IsNull([D12], 0)+IsNull([D13], 0)+IsNull([D14], 0)+IsNull([D15], 0)+IsNull([D16], 0)+IsNull([D17], 0)+IsNull([D18], 0)+IsNull([D19], 0)+IsNull([D20], 0)+IsNull([D21], 0)+IsNull([D22], 0)+IsNull([D23], 0)+IsNull([D24], 0)+IsNull([D25], 0)+IsNull([D26], 0)+IsNull([D27], 0)+IsNull([D28], 0)+IsNull([D29], 0)+IsNull([D30], 0)+IsNull([D31], 0))" UnboundType="Integer">
                    </dx:GridViewDataTextColumn>
                </Columns>
                <SettingsPager PageSize="20"></SettingsPager>
                <Settings ShowFooter="true" />
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
                    <dx:ASPxSummaryItem FieldName="OY" SummaryType="Sum" DisplayFormat="{0}" />
                </TotalSummary>
            </dx:ASPxGridView>
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
                    data: ['1','2','3','4','5','6','7','8','9','10','11','12','13','14','15','16','17','18','19','20','21','22','23','24','25','26','27','28','29','30','31']
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
