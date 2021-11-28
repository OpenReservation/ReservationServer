using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using OpenReservation.Database;
using Xunit;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Text;
using System.Text;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.EntityFrameworkCore;
using OpenReservation.Models;

namespace OpenReservation.SourceGeneratorTest
{
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
            var tester = new CSharpSourceGeneratorTest<ServiceGenerator, XUnitVerifier>()
            {
                TestState =
                {
                    Sources = { code },
                    GeneratedSources =
                    {
                        (typeof(ServiceGenerator), $"{nameof(ServiceGenerator)}.cs", SourceText.From(generated, Encoding.UTF8)),
                    }
                },
            };
            tester.ReferenceAssemblies = new ReferenceAssemblies("net6.0", 
                    new PackageIdentity("Microsoft.NETCore.App.Ref", "6.0.0"), 
                    System.IO.Path.Combine("ref", "net6.0"));

            tester.TestState.AdditionalReferences.Add(typeof(SystemSettings).Assembly);
            tester.TestState.AdditionalReferences.Add(typeof(WeihanLi.Common.DependencyResolver).Assembly);
            tester.TestState.AdditionalReferences.Add(typeof(WeihanLi.EntityFramework.EFRepository<,>).Assembly);
            tester.TestState.AdditionalReferences.Add(typeof(DbContext).Assembly); ;

            await tester.RunAsync();
        }
    }
}
