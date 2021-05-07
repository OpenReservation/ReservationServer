using System.Text;
using Microsoft.CodeAnalysis;

namespace OpenReservation.Database
{
    [Generator]
    public class ServiceGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var types = new[]{
                "BlockType",
                "BlockEntity",
                "OperationLog",
                "Reservation",
                "ReservationPlace",
                "ReservationPeriod",
                "SystemSettings",
                "Notice",
                "DisabledPeriod"
            };

            var codeBuilder = new StringBuilder();
            codeBuilder.AppendLine(@"
using OpenReservation.Database;
using OpenReservation.Models;
using WeihanLi.EntityFramework;

namespace OpenReservation.Business
{");
            foreach (var item in types)
            {
                codeBuilder.AppendLine($@"
    public partial interface IBLL{item}: IEFRepository<ReservationDbContext, {item}>{{}}

    public partial class BLL{item} : EFRepository<ReservationDbContext, {item}>,  IBLL{item}
    {{
        public BLL{item}(ReservationDbContext dbContext) : base(dbContext)
        {{
        }}
    }}
                 ");
            }
            codeBuilder.AppendLine("}");
            var codeText = codeBuilder.ToString();
            context.AddSource(nameof(ServiceGenerator), codeText);
        }
    }
}
