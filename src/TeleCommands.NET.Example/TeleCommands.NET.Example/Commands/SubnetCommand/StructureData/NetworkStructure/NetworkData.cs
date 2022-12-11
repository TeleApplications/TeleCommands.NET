namespace TeleCommands.NET.Example.Commands.SubnetCommand.StructureData.NetworkStructure 
{
    internal readonly struct NetworkData
    {
        public string Name { get; }
        public int HostCount { get; }

        public NetworkData(string name, int hostCount) 
        {
            Name = name;
            HostCount = hostCount;
        }
    }
}
