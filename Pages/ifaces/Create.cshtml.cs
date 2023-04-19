using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MikrotikWireguardUI.Data;
using MikrotikWireguardUI.Models;

namespace MikrotikWireguardUI.Pages.ifaces
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
        ViewData["ServerId"] = new SelectList(_context.Server, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public Iface Iface { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (
                // !ModelState.IsValid ||   // This validation is removed because we are only
                                            // setting name and ServerId parameters from the model.
                _context.Iface == null ||
                Iface == null
                )
            {
                return Page();
            }
            
            await Iface.Create(Iface, _context);
            await _context.SaveChangesAsync();
            
            return RedirectToPage("./Index");
        }
        
        
    }
}
