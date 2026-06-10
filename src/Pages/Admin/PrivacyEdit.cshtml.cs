using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Social2s.Models.Admin;
using Social2s.Models.Store;

namespace Social2s.Pages.Admin
{
    public class PrivacyEditModel : AdminControlModel
    {
        private readonly Social2s.Areas.Identity.Data.DataContext _context;

        public PrivacyEditModel(Social2s.Areas.Identity.Data.DataContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Social2s.Models.Store.PrivacyUsModel PrivacyModel { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            id = _context.Privacy.FirstOrDefault().Id;


            PrivacyUsModel privacymodel = await _context.Privacy.FirstOrDefaultAsync(m => m.Id == id);
            if (privacymodel == null)
            {
                return NotFound();
            }
            PrivacyModel = privacymodel;
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
            PrivacyModel.Lastupdate = DateTime.Now.ToString();
            _context.Attach(PrivacyModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PrivacyModelExists(PrivacyModel.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("../Index");
        }

        private bool PrivacyModelExists(int id)
        {
            return _context.Privacy.Any(e => e.Id == id);
        }
    }
}
