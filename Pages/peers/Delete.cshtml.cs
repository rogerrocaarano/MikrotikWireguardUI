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
    public class DeleteModel : PageModel
    {
        private readonly MikrotikWireguardUI.Data.ApplicationDbContext _context;

        public DeleteModel(MikrotikWireguardUI.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Peer == null)
            {
                return NotFound();
            }
            var peer = await _context.Peer.FindAsync(id);

            if (peer != null)
            {
                Peer = peer;
                _context.Peer.Remove(Peer);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
