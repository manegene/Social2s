using Microsoft.AspNetCore.Mvc;
using Social2s.Models.Admin;
using Social2s.Models.Category;

namespace Social2s.Pages.Admin
{
    public class CreateModel : AdminControlModel
    {
        private readonly Social2s.Areas.Identity.Data.DataContext _context;

        public CreateModel(Social2s.Areas.Identity.Data.DataContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ParentCategory = _context.Category.ToList();
            return Page();
        }

        [BindProperty]
        public CategoryModel CategoryModel { get; set; }

        [BindProperty]
        public CategoryDTO Input { get; set; }

        public List<CategoryModel> ParentCategory { get; set; }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            if (Input.Id >= 1)
                return BadRequest();

            CategoryModel entity = new CategoryModel
            {
                Name = Input.Name,
                ParentId = Input.ParentCategoryId
            };

            _context.Category.Add(entity);
            await _context.SaveChangesAsync();

            return RedirectToPage("./CategoryView");
        }
    }
}
