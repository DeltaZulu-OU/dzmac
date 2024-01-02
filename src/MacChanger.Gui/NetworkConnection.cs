namespace MacChanger.Gui
{
    internal class NetworkConnection
    {
        public bool Enabled { get; set; }
        public string Name { get; set; }
        public string Changed { get; set; }
        public string MacAddress { get; set; }
        public string LinkStatus { get; set; }
        public string Speed { get; set; }

        internal Adapter Adapter { get; set; }
    }
}
