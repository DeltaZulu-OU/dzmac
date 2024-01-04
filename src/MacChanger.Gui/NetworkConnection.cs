using System;
using System.Net.NetworkInformation;

namespace MacChanger.Gui
{
    internal class NetworkConnection : IDisposable
    {
        private bool disposedValue;

        public bool Enabled { get; set; }
        public string Name { get; set; }
        public string Changed { get; set; }
        public PhysicalAddress MacAddress { get; set; }
        public string LinkStatus => Adapter.ManagedAdapter.OperationalStatus.ToString();
        public string Speed => ReadableSpeed(Adapter.ManagedAdapter.Speed);

        internal NetworkAdapter Adapter { get; set; }

        public NetworkConnection(NetworkAdapter adapter)
        {
            Enabled = adapter.Enabled;
            Name = adapter.ManagedAdapter.Name;
            Changed = "No"; // TODO: Keep state of changes
            MacAddress = adapter.MacAddress;
            Adapter = adapter;
        }

        private string ReadableSpeed(long speed)
        {
            if (speed >= 1000000000)
            {
                float v = speed / 1000000000;
                return $"{v:F2} Gbps";
            }
            else if (speed >= 1000000)
            {
                float v = speed / 1000000;
                return $"{v:F2} Mbps";
            }
            else if (speed >= 1000)
            {
                float v = speed / 1000;
                return $"{v:F2} Kbps";
            }
            else if (speed == -1)
            {
                return "0 bps";
            }
            else
            {
                return $"{speed} bps";
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Adapter?.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
