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
    public class DetailsModel : PageModel
    {
        private readonly MikrotikWireguardUI.Data.ApplicationDbContext _context;

        public DetailsModel(MikrotikWireguardUI.Data.ApplicationDbContext context)
        {
            _context = context;
        }

      public Server Server { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Server == null)
            {
                return NotFound();
            }

            var server = await _context.Server.FirstOrDefaultAsync(m => m.Id == id);
            if (server == null)
            {
                return NotFound();
            }
            else 
            {
                Server = server;
            }
            return Page();
        }
    }
}
