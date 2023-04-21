using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MikrotikWireguardUI.Data;
using MikrotikWireguardUI.Models;

namespace MikrotikWireguardUI.Pages.peers
{
    public class CreateModel : PageModel
    {
        private readonly MikrotikWireguardUI.Data.ApplicationDbContext _context;

        public CreateModel(MikrotikWireguardUI.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["IfaceId"] = new SelectList(_context.Iface, "Id", "Id");
        ViewData["ServerId"] = new SelectList(_context.Server, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public Peer Peer { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Peer == null || Peer == null)
            {
                return Page();
            }

            _context.Peer.Add(Peer);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
