using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using mikrotikWireguardHandler;
using MikrotikWireguardUI.Data;
using MikrotikWireguardUI.Models;

namespace MikrotikWireguardUI.Pages.ifaces
{
    public class IndexModel : PageModel
    {
        private readonly MikrotikWireguardUI.Data.ApplicationDbContext _context;

        public IndexModel(MikrotikWireguardUI.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Iface> Iface { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Iface != null)
            {
                Iface = await _context.Iface
                .Include(i => i.Server).ToListAsync();
            }
        }
        
        public async Task<IActionResult> OnPostRefreshInterfaces()
        {
            foreach (var server in _context.Server)
            {
                // Set API client parameters for each server.
                var router = new mikrotikApiClient(
                    server.Fqdn,
                    server.Port,
                    server.Ssl,
                    server.Username,
                    server.Password
                );
                await RefreshServerInterfaces(router, server);
            }
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }

        private async Task RefreshServerInterfaces(mikrotikApiClient router, Server server)
        {
            // Get all interfaces.
            var wg = new WireguardServer(router);
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
                foreach (var dbIface in _context.Iface)
                {
                    if (dbIface.Name == apiServerIface.Name)
                    {
                        inDb = true;
                    }
                }
                if (!inDb)
                {
                    _context.Iface.Add(iface);
                }
            }
        }
    }
}
