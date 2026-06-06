using Microsoft.EntityFrameworkCore;
using Social2s.Models.Amin;
using Social2s.Models.User;

namespace Social2s.Pages.Admin
{
    public class ListRolesModel : AdminControlModel
    {
        private readonly Social2s.Areas.Identity.Data.DataContext _context;

        public ListRolesModel(Social2s.Areas.Identity.Data.DataContext context)
        {
            _context = context;
        }

        public IList<RolesModel> RolesModel { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Roles != null)
            {
                RolesModel = await _context.Roles.ToListAsync();
            }
        }
    }
}
