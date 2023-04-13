using System.ComponentModel;

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
}