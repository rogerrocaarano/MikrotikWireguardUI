using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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
                var apiClient = Models.Server.CreateApiClient(server);
                await Models.Iface.RefreshServerInterfaces(apiClient, server, _context);
            }
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
