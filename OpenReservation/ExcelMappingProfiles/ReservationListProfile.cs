using NPOI.SS.UserModel;
using OpenReservation.ViewModels;
using WeihanLi.Extensions;
using WeihanLi.Npoi;
using WeihanLi.Npoi.Configurations;

namespace OpenReservation.ExcelMappingProfiles;

public class ReservationListMappingProfile: IMappingProfile<ReservationListViewModel>
{
    public void Configure(IExcelConfiguration<ReservationListViewModel> settings)
    {
        settings
            .HasAuthor("WeihanLi")
            .HasTitle("预约信息")
            .HasDescription("预约信息")
            .HasSheetSetting(x =>
            {
                x.AutoColumnWidthEnabled = true;
                x.SheetName = "ReservationList";
                x.RowAction = row =>
                {
                    if (row.RowNum == 0)
                    {
                        var style = row.Sheet.Workbook.CreateCellStyle();
                        style.Alignment = HorizontalAlignment.Center;
                        row.Cells.ForEach(c => c.CellStyle = style);
                    }
                };
            })
            ;

        settings.Property(r => r.ReservationId).Ignored();

        settings.Property(r => r.ReservationPlaceName)
            .HasColumnTitle("预约项目")
            .HasColumnIndex(0);
        settings.Property(r => r.ReservationForDate)
            .HasColumnTitle("预约使用日期")
            .HasColumnIndex(1);
        settings.Property(r => r.ReservationForTime)
            .HasColumnTitle("预约使用的时间段")
            .HasColumnIndex(2);
        settings.Property(r => r.ReservationUnit)
            .HasColumnTitle("预约单位")
            .HasColumnIndex(3);
        settings.Property(r => r.ReservationActivityContent)
            .HasColumnTitle("预约活动内容")
            .HasColumnIndex(4);
        settings.Property(r => r.ReservationPersonName)
            .HasColumnTitle("预约人姓名")
            .HasColumnIndex(5);
        settings.Property(r => r.ReservationPersonPhone)
            .HasColumnTitle("预约人手机号")
            .HasColumnIndex(6);
        settings.Property(r => r.ReservationTime)
            .HasColumnTitle("预约时间")
            .HasColumnFormatter("yyyy-MM-dd HH:mm:ss")
            .HasColumnIndex(7);
        settings.Property(r => r.ReservationStatus)
            .HasColumnTitle("审核状态")
            .HasColumnOutputFormatter(status => status.GetDescription())
            .HasColumnIndex(8);
    }
}