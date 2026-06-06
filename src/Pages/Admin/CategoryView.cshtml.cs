using Microsoft.EntityFrameworkCore;
using Social2s.Models.Amin;
using Social2s.Models.Category;

namespace Social2s.Pages.Admin
{
    public class CategoryViewModel : AdminControlModel
    {
        private readonly Social2s.Areas.Identity.Data.DataContext _context;

        public CategoryViewModel(Social2s.Areas.Identity.Data.DataContext context)
        {
            _context = context;
        }

        public IList<CategoryModel> CategoryModel { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Category != null)
            {
                CategoryModel = await _context.Category.ToListAsync();
            }
        }
    }
}
