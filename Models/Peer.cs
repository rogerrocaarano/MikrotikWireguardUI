using mikrotikWireguardHandler.RestApiObjects;

namespace MikrotikWireguardUI.Models;

public class Peer : WireguardPeer
{
    public int Id { get; set; }
    public int IfaceId { get; set; }
    public Iface Iface { get; set; }
    public int ServerId { get; set; }
    public Server Server { get; set; }
}