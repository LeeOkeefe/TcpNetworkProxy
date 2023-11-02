namespace TcpNetworkProxy.Data;

public static class Paths
{
    public static string Home => "/";
    public static string Network(string host, int port, string destinationHost, int destinationPort) 
        => $"/network/{host}/{port}/{destinationHost}/{destinationPort}";
}