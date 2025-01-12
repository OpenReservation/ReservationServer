using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace OpenReservation.Helpers;

public static class DiagnosticHelper
{
    public static ActivitySource ActivitySource { get; } = new(Constants.ServiceName);
    public static Meter Meter { get; } = new(Constants.ServiceName);
}
