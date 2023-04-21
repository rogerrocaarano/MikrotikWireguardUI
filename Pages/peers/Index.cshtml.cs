using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MikrotikWireguardUI.Data;
using MikrotikWireguardUI.Models;

namespace MikrotikWireguardUI.Pages.peers
{
    public class IndexModel : PageModel
    {
        private readonly MikrotikWireguardUI.Data.ApplicationDbContext _context;

        public IndexModel(MikrotikWireguardUI.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Peer> Peer { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Peer != null)
            {
                Peer = await _context.Peer
                .Include(p => p.Iface)
                .Include(p => p.Server).ToListAsync();
            }
        }
    }
}
