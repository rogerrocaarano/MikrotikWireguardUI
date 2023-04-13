namespace MikrotikWireguardUI.Models;

public class Iface
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? PrivateKey { get; set; }
    public string? PublicKey { get; set; }
    public int ServerId { get; set; }
    public Server Server { get; set; }
}