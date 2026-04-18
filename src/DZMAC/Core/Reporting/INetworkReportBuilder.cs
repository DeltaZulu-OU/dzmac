using System;
using System.Collections.Generic;

namespace Dzmac.Gui.Core.Reporting
{
    public interface INetworkReportBuilder
    {
        string BuildReport(IReadOnlyList<NetworkReportEntry> entries, DateTime generatedAt, string productVersion);
    }
}
