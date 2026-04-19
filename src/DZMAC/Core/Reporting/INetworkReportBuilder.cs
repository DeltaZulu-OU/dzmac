using System;
using System.Collections.Generic;

namespace Dzmac.Core.Reporting
{
    public interface INetworkReportBuilder
    {
        string BuildReport(IReadOnlyList<NetworkReportEntry> entries, DateTime generatedAt, string productVersion);
    }
}
