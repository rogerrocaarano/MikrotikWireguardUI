using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using mikrotikWireguardHandler;
using mikrotikWireguardHandler.RestApiObjects;
using MikrotikWireguardUI.Data;

namespace MikrotikWireguardUI.Models;

public class Iface : WireguardInterface
{
    public int Id { get; set; }
    public int ServerId { get; set; }
    public Server Server { get; set; }

    /// <summary>
    /// Default constructor.
    /// </summary>
    public Iface()
    {
        
    }

    /// <summary>
    /// This constructor is used to reflect the properties of an already existing object of the
    /// parent class WireguardInterface.
    /// </summary>
    /// <param name="iface">Existing WireguardInterface object.</param>
    /// <param name="server">Server than the iface belongs.</param>
    public Iface(WireguardInterface iface, Server server)
    {
        this.ServerId = server.Id;
        this.Server = server;
        // Property reflection
        foreach (var property in iface.GetType().GetProperties())
        {
            this.GetType().GetProperty(property.Name).SetValue(this, property.GetValue(iface, null), null);
        }
    }
    
    /// <summary>
    /// This method creates a new interface on the router based on the Iface model and update the ifaces view.
    /// </summary>
    /// <param name="iface">Iface object with properties.</param>
    /// <param name="db">Database context.</param>
    public static async Task Create(Iface iface, ApplicationDbContext db)
    {
        // Create a WireguardServer object from the server in database.
        var server = db.Server.Find(iface.ServerId);
        var wg = Models.Server.GetWgServer(server);
        // Create the new object and sent it to the API.
        var wgIface = new WireguardInterface
        {
            Name = iface.Name,
            ListenPort = iface.ListenPort
        };
        await wg.NewInterface(wgIface);
        // Update the ifaces view.
        await GetTable(server, db);
    }
    
    /// <summary>
    /// Deletes an interface from the router and removes it from the database.
    /// </summary>
    /// <param name="iface">Iface type object to remove.</param>
    /// <param name="db">Database context.</param>
    public static async Task Delete(Iface iface, ApplicationDbContext db)
    {
        var server = db.Server.Find(iface.ServerId);
        var wireguardServer = Models.Server.GetWgServer(server);
        await wireguardServer.UpdateInterfaces();
        var wireguardIface = new WireguardInterface
        {
            Name = iface.Name
        };
        await wireguardServer.DeleteInterface(wireguardIface);
        db.Iface.Remove(iface);
    }

    /// <summary>
    /// Gets all interfaces from the server and updates the ifaces view.
    /// </summary>
    /// <param name="server">Server to get updates.</param>
    /// <param name="db">Database context.</param>
    public static async Task GetTable(Server server, ApplicationDbContext db)
    {
        await CleanTable(db);
        // Get all interfaces.
        var wg = Models.Server.GetWgServer(server);
        await wg.UpdateInterfaces();
        // Update database.
        foreach (var apiServerIface in wg.Interfaces)
        {
            var iface = new Iface(apiServerIface, server);
            UpdateOnTable(iface, db);
        }
    }

    /// <summary>
    /// Updates an interface from iface view, if it doesn't exists, creates a new record on the database.
    /// </summary>
    /// <param name="iface">Iface object to update/create.</param>
    /// <param name="db">Database context.</param>
    private static void UpdateOnTable(Iface iface, ApplicationDbContext db)
    {
        // Find in the database context if the interface exists.
        var current = db.Iface
            .Where(i => i.ServerId == iface.ServerId)
            .FirstOrDefault(i => i.RosId == iface.RosId);
        if (current==null)
        {
            // Create a new item on the table.
            db.Iface.Add(iface);
        }
        else
        {
            // If found, set iface.Id to the founded id and set values to the new ones.
            iface.Id = current.Id;
            db.Entry(current).CurrentValues.SetValues(iface);
        }
    }
    
    /// <summary>
    /// Removes interfaces not present on the servers from the iface view.
    /// </summary>
    /// <param name="db">Database context.</param>
    private static async Task CleanTable(ApplicationDbContext db)
    {
        foreach (var server in db.Server)
        {
            var wireguardServer = Models.Server.GetWgServer(server);
            await wireguardServer.UpdateInterfaces();
            var validRosIds = new List<string>();
            wireguardServer.Interfaces.ForEach(i=>validRosIds.Add(i.RosId));
            foreach (var iface in db.Iface)
            {
                if (iface.ServerId != server.Id) continue;
                if (!validRosIds.Exists(x => x == iface.RosId))
                {
                    db.Iface.Remove(iface);
                }
            }
        }
    }


    
    
}