using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MikrotikWireguardUI.Data;
using MikrotikWireguardUI.Models;

namespace MikrotikWireguardUI.Pages.ifaces
{
    public class EditModel : PageModel
    {
        private readonly MikrotikWireguardUI.Data.ApplicationDbContext _context;

        public EditModel(MikrotikWireguardUI.Data.ApplicationDbContext context)
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

            var iface =  await _context.Iface.FirstOrDefaultAsync(m => m.Id == id);
            if (iface == null)
            {
                return NotFound();
            }
            Iface = iface;
           ViewData["ServerId"] = new SelectList(_context.Server, "Id", "Id");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Iface).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IfaceExists(Iface.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool IfaceExists(int id)
        {
          return (_context.Iface?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
