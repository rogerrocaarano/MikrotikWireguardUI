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
    
    public static async Task RefreshServerInterfaces(Server server, ApplicationDbContext db)
    {
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
                ServerId = server.Id,
                Server = server
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

    public static async Task NewInterfaceOnServer(Iface iface, ApplicationDbContext db)
    {
        var server = db.Server.Find(iface.ServerId);
        var wg = Models.Server.GetWgServer(server);
        var wgIface = new WireguardInterface();
        wgIface.Name = iface.Name;
        await wg.NewInterface(wgIface);
        await RefreshServerInterfaces(server, db);
    }
}