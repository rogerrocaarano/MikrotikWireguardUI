using Microsoft.EntityFrameworkCore.Metadata.Internal;
using mikrotikWireguardHandler;
using mikrotikWireguardHandler.RestApiObjects;
using MikrotikWireguardUI.Data;

namespace MikrotikWireguardUI.Models;

public class Iface
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? PrivateKey { get; set; }
    public string? PublicKey { get; set; }
    public int ServerId { get; set; }
    public Server Server { get; set; }
    public string? RosId { get; set; }
    public int? PortNumber { get; set; }

    public static async Task GetTable(Server server, ApplicationDbContext db)
    {
        await CleanTable(db);
        // Get all interfaces.
        var wg = Models.Server.GetWgServer(server);
        await wg.UpdateInterfaces();
        // Update database.
        foreach (var apiServerIface in wg.Interfaces)
        {
            var iface = new Iface
            {
                Name = apiServerIface.Name,
                PrivateKey = apiServerIface.PrivateKey,
                PublicKey = apiServerIface.PublicKey,
                RosId = apiServerIface.RosId,
                ServerId = server.Id,
                Server = server,
                PortNumber = apiServerIface.ListenPort
            };
            var inDb = false;
            foreach (var dbIface in db.Iface)
            {
                if (dbIface.Name == apiServerIface.Name)
                {
                    inDb = true;
                }
            }
            if (!inDb)
            {
                db.Iface.Add(iface);
            }
        }
    }

    public static async Task Create(Iface iface, ApplicationDbContext db)
    {
        var server = db.Server.Find(iface.ServerId);
        var wg = Models.Server.GetWgServer(server);
        var wgIface = new WireguardInterface
        {
            Name = iface.Name,
            ListenPort = iface.PortNumber
        };
        await wg.NewInterface(wgIface);
        await GetTable(server, db);
    }

    public static async Task CleanTable(ApplicationDbContext db)
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
    
    
}