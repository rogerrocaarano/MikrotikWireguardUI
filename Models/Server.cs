using System.ComponentModel;
using mikrotikWireguardHandler;

namespace MikrotikWireguardUI.Models;

public class Server
{
    public int Id { get; set; }
    public string Fqdn { get; set; }
    [DefaultValue(443)]
    public int Port { get; set; }
    [DefaultValue(true)]
    public bool Ssl { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }

    public static WireguardServer GetWgServer(Server server)
    {
        var apiClient = new mikrotikApiClient(
            server.Fqdn,
            server.Port,
            server.Ssl,
            server.Username,
            server.Password
        );
        var wireguardServer = new WireguardServer(apiClient);
        return wireguardServer;
    }
}