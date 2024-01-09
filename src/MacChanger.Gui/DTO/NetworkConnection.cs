namespace MacChanger.Gui.DTO
{
    /// <summary>
    ///     A DTO for MacChanger datagrid which includes basic information
    /// </summary>
    internal class NetworkConnection
    {
        public string ConnectionName => Detail.Name;
        public string Changed => Detail.Changed;
        public string MacAddress => Detail.ActiveMac;
        public string LinkStatus => Detail.LinkStatus;
        public string Speed => Detail.Speed;

        internal NetworkConnectionDetail Detail { get; set; }
        public NetworkConnection(NetworkConnectionDetail advanced)
        {
            Detail = advanced;
        }

        public NetworkConnection(NetworkAdapter adapter) : this(new NetworkConnectionDetail(adapter))
        {
        }
    }
}
