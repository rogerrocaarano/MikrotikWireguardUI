using mikrotikWireguardHandler.RestApiObjects;
using MikrotikWireguardUI.Data;

namespace MikrotikWireguardUI.Models;

public class Peer : WireguardPeer
{
    public int Id { get; set; }
    public int IfaceId { get; set; }
    public Iface Iface { get; set; }
    public int ServerId { get; set; }
    public Server Server { get; set; }
    
    /// <summary>
    /// Default constructor.
    /// </summary>
    public Peer()
    {
        
    }

    /// <summary>
    /// This constructor is used to reflect the properties of an already existing object of the
    /// parent class WireguardPeer.
    /// </summary>
    /// <param name="peer">Existing WireguardPeer object.</param>
    /// <param name="server">Server than the peer belongs.</param>
    public Peer(WireguardPeer peer, Server server, Iface iface)
    {
        this.ServerId = server.Id;
        this.Server = server;
        this.IfaceId = iface.Id;
        this.Iface = iface;

        // Property reflection
        foreach (var property in peer.GetType().GetProperties())
        {
            this.GetType().GetProperty(property.Name).SetValue(this, property.GetValue(peer, null), null);
        }
    }
    
    /// <summary>
    /// Gets all peers from the server and updates the peers view.
    /// </summary>
    /// <param name="server">Server to get updates.</param>
    /// <param name="db">Database context.</param>
    public static async Task GetTable(Server server, ApplicationDbContext db)
    {
        // Update the peers view.
        await Models.Iface.GetTable(server, db);
        await CleanTable(db);
        // Get all peers.
        var wg = Models.Server.GetWgServer(server);
        await wg.UpdatePeers();
        // Update database.
        foreach (var  apiServerPeer in wg.Peers)
        {
            var ifaceString = apiServerPeer.Interface;
            // find the interface on the database.
            var iface = db.Iface
                .Where(i => i.ServerId == server.Id)
                .FirstOrDefault(i => i.Name == ifaceString);
            var peer = new Peer(apiServerPeer, server, iface);
            UpdateOnTable(peer, db);
        }
    }
    
    /// <summary>
    /// Updates a Peer from peers view, if it doesn't exists, creates a new record on the database.
    /// </summary>
    /// <param name="peer">Peer object to update/create.</param>
    /// <param name="db">Database context.</param>
    private static void UpdateOnTable(Peer peer, ApplicationDbContext db)
    {
        // Find in the database context if the interface exists.
        var current = db.Peer
            .Where(i => i.ServerId == peer.ServerId)
            .FirstOrDefault(i => i.RosId == peer.RosId);
        if (current==null)
        {
            // Create a new item on the table.
            db.Peer.Add(peer);
        }
        else
        {
            // If found, set peer.Id to the founded id and set values to the new ones.
            peer.Id = current.Id;
            db.Entry(current).CurrentValues.SetValues(peer);
        }
    }
    
    /// <summary>
    /// Removes peers not present on the servers from the peers view.
    /// </summary>
    /// <param name="db">Database context.</param>
    private static async Task CleanTable(ApplicationDbContext db)
    {
        foreach (var server in db.Server)
        {
            var wireguardServer = Models.Server.GetWgServer(server);
            await wireguardServer.UpdatePeers();
            var validRosIds = new List<string>();
            wireguardServer.Peers.ForEach(i=>validRosIds.Add(i.RosId));
            foreach (var peer in db.Peer)
            {
                if (peer.ServerId != server.Id) continue;
                if (!validRosIds.Exists(x => x == peer.RosId))
                {
                    db.Peer.Remove(peer);
                }
            }
        }
    }
}