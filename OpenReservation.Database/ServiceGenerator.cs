using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace OpenReservation.Database;

[Generator]
public class ServiceGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        // Debugger.Launch();
    }

    public void Execute(GeneratorExecutionContext context)
    {
        var dbContextType = context.Compilation.GetTypeByMetadataName(typeof(DbSet<>).FullName);
        var reservationDbContextType = context.Compilation.GetTypeByMetadataName(typeof(ReservationDbContext).FullName);
        var propertySymbols = reservationDbContextType.GetMembers()
                .OfType<IMethodSymbol>()
                .Where(x => x.IsVirtual
                            && x.MethodKind == MethodKind.PropertyGet
                            && x.ReturnType is INamedTypeSymbol
                            {
                                IsGenericType: true,
                                IsUnboundGenericType: false,
                            } typeSymbol
                            && ReferenceEquals(typeSymbol.ConstructedFrom.ContainingAssembly, dbContextType.ContainingAssembly)
                )
                .ToArray()
            ;
        var propertyReturnType = propertySymbols
            .Select(r => ((INamedTypeSymbol)r.ReturnType))
            .ToArray();
        var modelTypeNames = propertyReturnType
            .Select(t => t.TypeArguments)
            .SelectMany(x => x)
            .Select(x => x.Name)
            .ToArray();

        var codeBuilder = new StringBuilder();
        codeBuilder.AppendLine(@"
using OpenReservation.Database;
using OpenReservation.Models;
using WeihanLi.EntityFramework;

namespace OpenReservation.Business
{");
        foreach (var item in modelTypeNames)
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