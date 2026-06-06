using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Social2s.Models.Amin
{
    [Authorize(Roles = "admin")]
    public class AdminControlModel : PageModel
    {

    }
}
