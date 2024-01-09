namespace MacChanger.Gui.DTO
{
    internal class NetworkConnection
    {
        public string ConnectionName => Advanced.Name;
        public string Changed => Advanced.Changed;
        public string MacAddress => Advanced.ActiveMac;
        public string LinkStatus => Advanced.LinkStatus;
        public string Speed => Advanced.Speed;

        internal NetworkConnectionAdvanced Advanced { get; set; }
        public NetworkConnection(NetworkConnectionAdvanced advanced)
        {
            Advanced = advanced;
        }

        public NetworkConnection(NetworkAdapter adapter) : this(new NetworkConnectionAdvanced(adapter))
        {
        }
    }
}
