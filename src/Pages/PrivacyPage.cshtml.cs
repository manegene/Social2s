using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Social2s.Areas.Identity.Data;
using Social2s.Models.Store;

namespace Social2s.Pages
{
    [AllowAnonymous]
    public class PrivacyPageModel : PageModel
    {
        private readonly ILogger<PrivacyPageModel> _logger;
        private readonly DataContext _dataContext;

        public PrivacyPageModel(ILogger<PrivacyPageModel> logger, DataContext dataContext)
        {
            _logger = logger;
            _dataContext = dataContext;
        }
        public PrivacyUsModel PrivacyUsModel { get; set; }
        public void OnGet()
        {
            PrivacyUsModel = _dataContext.Privacy.FirstOrDefault();
        }
    }
}