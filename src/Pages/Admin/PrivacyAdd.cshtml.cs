using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Social2s.Models.Admin;
using Social2s.Models.Store;

namespace Social2s.Pages.Admin
{
    public class PrivacyAddModel : AdminControlModel
    {
        private readonly Social2s.Areas.Identity.Data.DataContext _context;

        public PrivacyAddModel(Social2s.Areas.Identity.Data.DataContext context)
        {
            _context = context;
        }

        public bool CheckModel { get; set; }
        public async Task<IActionResult> OnGet()
        {
            bool notEmpty = await _context.Privacy.AnyAsync();
            if (notEmpty)
                CheckModel = true;
            return Page();
        }

        [BindProperty]
        public PrivacyUsModel PrivacyModel { get; set; }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            PrivacyModel.Lastupdate = DateTime.UtcNow.ToString();
            _context.Privacy.Add(PrivacyModel);
            await _context.SaveChangesAsync();

            return RedirectToPage("../Index");
        }
    }
}
