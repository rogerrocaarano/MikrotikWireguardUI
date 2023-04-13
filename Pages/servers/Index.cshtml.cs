using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MikrotikWireguardUI.Data;
using MikrotikWireguardUI.Models;

namespace MikrotikWireguardUI.Pages.servers
{
    public class IndexModel : PageModel
    {
        private readonly MikrotikWireguardUI.Data.ApplicationDbContext _context;

        public IndexModel(MikrotikWireguardUI.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Server> Server { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Server != null)
            {
                Server = await _context.Server.ToListAsync();
            }
        }
    }
}
