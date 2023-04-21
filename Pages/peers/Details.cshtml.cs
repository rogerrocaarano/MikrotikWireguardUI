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
    public class DetailsModel : PageModel
    {
        private readonly MikrotikWireguardUI.Data.ApplicationDbContext _context;

        public DetailsModel(MikrotikWireguardUI.Data.ApplicationDbContext context)
        {
            _context = context;
        }

      public Peer Peer { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Peer == null)
            {
                return NotFound();
            }

            var peer = await _context.Peer.FirstOrDefaultAsync(m => m.Id == id);
            if (peer == null)
            {
                return NotFound();
            }
            else 
            {
                Peer = peer;
            }
            return Page();
        }
    }
}
