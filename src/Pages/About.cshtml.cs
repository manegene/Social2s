using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Social2s.Models.Store;

namespace Social2s.Pages
{
    [AllowAnonymous]
    public class AboutModel : PageModel
    {
        private readonly Social2s.Areas.Identity.Data.DataContext _context;

        public AboutModel(Social2s.Areas.Identity.Data.DataContext context)
        {
            _context = context;
        }

        public AboutUsModel About { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.About != null)
            {
                About = await _context.About.FirstOrDefaultAsync();
            }
        }
    }
}
