using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Social2s.Areas.Identity.Data;
using Social2s.Models.Admin;
using Social2s.Models.User;

namespace Social2s.Pages.Admin
{
    public class AddRoleModel : AdminControlModel
    {
        private readonly DataContext _context;

        public AddRoleModel(DataContext context)
        {
            _context = context;
        }
        [BindProperty]
        public RolesAdd AddNew { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public RolesModel RolesModel { get; set; }



        public class RolesAdd
        {
            [Required]
            public string Name { get; set; }
        }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (!string.IsNullOrEmpty(AddNew.Name))
            {
                RolesModel.Name = AddNew.Name;
                RolesModel.NormalizedName = AddNew.Name.ToUpper();
                RolesModel.ConcurrencyStamp = DateTime.Now.ToString();
                _context.Roles.Add(RolesModel);

                await _context.SaveChangesAsync();

                return RedirectToPage();

            }
            return Page();


        }
    }
}
