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
    public class DeleteModel : PageModel
    {
        private readonly MikrotikWireguardUI.Data.ApplicationDbContext _context;

        public DeleteModel(MikrotikWireguardUI.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Iface Iface { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Iface == null)
            {
                return NotFound();
            }

            var iface = await _context.Iface.FirstOrDefaultAsync(m => m.Id == id);

            if (iface == null)
            {
                return NotFound();
            }
            else 
            {
                Iface = iface;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Iface == null)
            {
                return NotFound();
            }
            var iface = await _context.Iface.FindAsync(id);

            if (iface != null)
            {
                Iface = iface;
                // _context.Iface.Remove(Iface);
                await Models.Iface.Delete(Iface, _context);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
