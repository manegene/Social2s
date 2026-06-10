using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Social2s.Models.Admin;
using Social2s.Models.Store;

namespace Social2s.Pages.Admin
{
    public class AboutAddModel : AdminControlModel
    {
        private readonly Social2s.Areas.Identity.Data.DataContext _context;

        public AboutAddModel(Social2s.Areas.Identity.Data.DataContext context)
        {
            _context = context;
        }
        public bool CheCk { get; set; }
        public async Task<IActionResult> OnGet()
        {
            bool exist = await _context.About.AnyAsync();
            if (exist)
                CheCk = true;
            return Page();
        }

        [BindProperty]
        public AboutUsModel AboutModel { get; set; }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            AboutModel.Lastupdated = DateTime.Now.ToString();
            _context.About.Add(AboutModel);
            await _context.SaveChangesAsync();

            return RedirectToPage("../Index");
        }
    }
}
