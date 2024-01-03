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
        public string LinkStatus { get; set; }
        public string Speed { get; set; }

        internal Adapter Adapter { get; set; }

        public NetworkConnection(Adapter adapter)
        {
            Enabled = adapter.Enabled;
            Name = adapter.ManagedAdapter.Name;
            Changed = "No"; // TODO: Keep state of changes
            MacAddress = adapter.MacAddress;
            LinkStatus = adapter.ManagedAdapter.OperationalStatus.ToString();
            Speed = ReadableSpeed(adapter.ManagedAdapter.Speed.ToString());
            Adapter = adapter;
        }

        private string ReadableSpeed(string speed)
        {
            var s = long.Parse(speed);

            if (s >= 1000000000)
            {
                float v = s / 1000000000;
                return $"{v:F2} Gbps";
            }
            else if (s >= 1000000)
            {
                float v = s / 1000000;
                return $"{v:F2} Mbps";
            }
            else if (s >= 1000)
            {
                float v = s / 1000;
                return $"{v:F2} Kbps";
            }
            else if (s == -1)
            {
                return "0 bps";
            }

            return speed;
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
