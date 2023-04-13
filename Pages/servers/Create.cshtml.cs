using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MikrotikWireguardUI.Data;
using MikrotikWireguardUI.Models;

namespace MikrotikWireguardUI.Pages.servers
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
            return Page();
        }

        [BindProperty]
        public Server Server { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Server == null || Server == null)
            {
                return Page();
            }

            _context.Server.Add(Server);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
