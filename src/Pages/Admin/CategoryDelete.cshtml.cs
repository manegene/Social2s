using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Social2s.Models.Amin;
using Social2s.Models.Category;

namespace Social2s.Pages.Admin
{
    public class DeleteModel : AdminControlModel
    {
        private readonly Social2s.Areas.Identity.Data.DataContext _context;

        public DeleteModel(Social2s.Areas.Identity.Data.DataContext context)
        {
            _context = context;
        }

        [BindProperty]
        public CategoryModel CategoryModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Category == null)
            {
                return NotFound();
            }

            CategoryModel categorymodel = await _context.Category.FirstOrDefaultAsync(m => m.Id == id);

            if (categorymodel == null)
            {
                return NotFound();
            }
            else
            {
                CategoryModel = categorymodel;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Category == null)
            {
                return NotFound();
            }
            CategoryModel categorymodel = await _context.Category.FindAsync(id);

            if (categorymodel != null)
            {
                CategoryModel = categorymodel;
                _context.Category.Remove(CategoryModel);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./CategoryView");
        }
    }
}
