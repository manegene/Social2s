using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Social2s.Areas.Identity.Data;
using Social2s.Models.Contact;
using Social2s.Models.User;

namespace Social2s.Areas.Identity.Pages.Account.Manage
{
    public class SentChatModel : PageModel
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly DataContext _context;

        public SentChatModel(UserManager<UserModel> userManager, DataContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public List<ContactModel> Messages { get; set; }
        public async Task<ActionResult> OnGetAsync()
        {
            UserModel user = await _userManager.GetUserAsync(User);
            if (user == null)
                return BadRequest();
            Messages = await _context.ContactQueue.Where(msend => msend.Sender == user.Email).ToListAsync();
            return Page();
        }
    }
}
