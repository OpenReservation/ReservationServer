using System;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CSharp.Testing.XUnit;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using OpenReservation.Database;
using Xunit;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Text;
using System.Text;
using Microsoft.EntityFrameworkCore;
using OpenReservation.Models;
using WeihanLi.EntityFramework;
using WeihanLi.Common.Data;
using System.Reflection;

namespace OpenReservation.SourceGeneratorTest
{
    public class ServiceGeneratorTester : CSharpSourceGeneratorTest<ServiceGenerator, XUnitVerifier>
    {
        public LanguageVersion LanguageVersion { get; set; } = LanguageVersion.Latest;

        protected override ParseOptions CreateParseOptions()
        {
            return ((CSharpParseOptions)base.CreateParseOptions()).WithLanguageVersion(LanguageVersion);
        }
    }

    public class ServiceGeneratorTest
    {
        [Fact]
        public async Task Test1()
        {
            var code = @"
using System;
using Microsoft.EntityFrameworkCore;
using OpenReservation.Models;

namespace OpenReservation.Database
{
    public class ReservationDbContext : DbContext
    {
        public ReservationDbContext(DbContextOptions<ReservationDbContext> options) : base(options)
        {
        }
        public virtual DbSet<SystemSettings> SystemSettings { get; set; }
    }
}
";
            var item = "SystemSettings";
            var generated = $@"
using OpenReservation.Database;
using OpenReservation.Models;
using WeihanLi.EntityFramework;

namespace OpenReservation.Business
{{

    public partial interface IBLL{item}: IEFRepository<ReservationDbContext, {item}>{{}}

    public partial class BLL{item} : EFRepository<ReservationDbContext, {item}>,  IBLL{item}
    {{
        public BLL{item}(ReservationDbContext dbContext) : base(dbContext)
        {{
        }}
    }}

}}
";
            var tester = new ServiceGeneratorTester()
            {
                TestState =
                {
                    Sources = { code },
                    GeneratedSources =
                    {
                        (typeof(ServiceGenerator), $"{nameof(ServiceGenerator)}.cs", SourceText.From(generated, Encoding.UTF8)),
                    },
                    AdditionalReferences = 
                    {
                        MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                        MetadataReference.CreateFromFile(typeof(DbContext).Assembly.Location),
                        MetadataReference.CreateFromFile(typeof(Repository<>).Assembly.Location),
                        MetadataReference.CreateFromFile(typeof(EFRepository<,>).Assembly.Location),
                        MetadataReference.CreateFromFile(typeof(SystemSettings).Assembly.Location),
                        MetadataReference.CreateFromFile(Assembly.Load("netstandard").Location),
                        MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location),
                    }
                },
            };

            tester.TestBehaviors = Microsoft.CodeAnalysis.Testing.TestBehaviors.SkipSuppressionCheck;
            await tester.RunAsync();
        }
    }
}
