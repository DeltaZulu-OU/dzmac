using System;
using System.Diagnostics;
using BrightIdeasSoftware;

namespace MacChanger.Gui.DTO
{
    /// <summary>
    ///     A DTO for MacChanger datagrid which includes basic information
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
                        if (Detail.TryDisable())
                        {
                            enabled = false;
                        }
                    }
                    else
                    {
                        if (Detail.TryEnable())
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
        public string MacAddress => Detail.ActiveMac;

        [OLVColumn("Speed", DisplayIndex = 5)]
        public string Speed => Detail.Speed;

        internal NetworkConnectionDetail Detail { get; set; }
        private bool enabled;

        private bool disposedValue;

        public NetworkConnection(NetworkConnectionDetail advanced)
        {
            Detail = advanced;
            enabled = Detail.Enabled;
        }

        public NetworkConnection(NetworkAdapter adapter, bool showSpeedInKBytesPerSec) : this(new NetworkConnectionDetail(adapter)
        {
            ShowSpeedInKBytesPerSec = showSpeedInKBytesPerSec
        })
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