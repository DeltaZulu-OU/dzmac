using System;
using System.Diagnostics;
using BrightIdeasSoftware;
using Dzmac.Gui.Core;

namespace Dzmac.Gui.DTO
{
    /// <summary>
    ///     A DTO for DZMAC datagrid which includes basic information
    /// </summary>
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    internal class NetworkConnection : IDisposable
    {
        // TODO: Disabling does not work
        [OLVColumn("Enabled", CheckBoxes = true, IsEditable = true, DisplayIndex = 0)]
        public bool Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                if (Detail.Enabled != value)
                {
                    if (Detail.Enabled)
                    {
                        if (_adminService.SetAdapterEnabled(Detail.Adapter, false).IsSuccess)
                        {
                            enabled = false;
                        }
                    }
                    else
                    {
                        if (_adminService.SetAdapterEnabled(Detail.Adapter, true).IsSuccess)
                        {
                            enabled = true;
                        }
                    }
                }
            }
        }

        [OLVColumn("Network Connections", DisplayIndex = 1)]
        public string ConnectionName => Detail.Name;

        [OLVColumn("Changed", DisplayIndex = 2)]
        public string Changed => Detail.Changed;

        [OLVColumn("Link Status", DisplayIndex = 4)]
        public string LinkStatus => Detail.LinkStatus;

        [OLVColumn("MAC Address", DisplayIndex = 3)]
        public string MacAddress => Changed == "Yes" ? $"{Detail.ActiveMac} (Changed)" : Detail.OriginalMac;

        [OLVColumn("Speed", DisplayIndex = 5)]
        public string Speed => Detail.Speed;

        internal NetworkConnectionDetail Detail { get; set; }
        private readonly IAdapterAdminService _adminService;
        private bool enabled;

        private bool disposedValue;

        public NetworkConnection(NetworkConnectionDetail advanced, IAdapterAdminService adminService)
        {
            Detail = advanced;
            _adminService = adminService;
            enabled = Detail.Enabled;
        }

        public NetworkConnection(NetworkAdapter adapter, bool showSpeedInKBytesPerSec, IAdapterAdminService adminService) : this(new NetworkConnectionDetail(adapter, showSpeedInKBytesPerSec), adminService)
        {
        }

        private string GetDebuggerDisplay() => ConnectionName;

        #region Dispose

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Detail?.Dispose();
                }

                disposedValue = true;
            }
        }

        #endregion Dispose
    }
}
