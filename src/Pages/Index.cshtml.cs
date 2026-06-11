using AspNetCore.ReCaptcha;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using Social2s.Areas.Identity.Data;
using Social2s.Models.Category;
using Social2s.Models.Contact;
using Social2s.Models.LinkedModels;
using Social2s.Models.Store;
using Social2s.Models.User;

namespace Social2s.Pages
{
    [ValidateReCaptcha]
    [AllowAnonymous]
    public class IndexModel(ILogger<IndexModel> logger, DataContext dataContext, IEmailSender emailSender, UserManager<UserModel> userManager) : PageModel
    {
        private readonly ILogger<IndexModel> _logger = logger;
        private readonly DataContext _dataContext = dataContext;
        private readonly IEmailSender _emailSender = emailSender;
        private readonly UserManager<UserModel> _userManager = userManager;
        private const int MAX_BODY_LENGTH = 5000;
        private const int MAX_JSON_LENGTH = 10000;
        public HomeModel Home { get; set; }
        public List<UserPublicModel> Users { get; set; }
        public List<CategoryModel> Categories { get; set; }
        public List<PublicProfile_Category> PublicProfiles { get; set; }
        [BindProperty]
        public ContactModel EmailDetails { get; set; }
        [BindProperty]
        public int ProfId { get; set; }
        public List<UserImageModel> UImage { get; set; }
        [BindProperty]
        public List<UserImageModel> SelectedUserImages { get; set; }
        public UserModel LoggedIn { get; set; }
        public bool IsSubscribed { get; set; }
        [TempData]
        public string ResponseMessage { get; set; }
        public async Task<ActionResult> OnGet()
        {
            try
            {
                LoggedIn = await _userManager.GetUserAsync(User);
                if (LoggedIn != null)
                {
                    IsSubscribed = await _dataContext.Subscriptions.AnyAsync(usr => usr.UserProfile == LoggedIn);
                }
                ;
                PublicProfiles = [.. from UserPublicModel upm in _dataContext.PublicProfile
                         join CategoryModel cm in _dataContext.Category
                         on upm.CategoryId equals cm.Id
                         select new PublicProfile_Category
                         {
                             Profile=upm,
                             Category=cm
                         }];

                Home = await _dataContext.Home.FirstOrDefaultAsync();

                UImage = await _dataContext.Images.ToListAsync();

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while loading the home page.");
                ResponseMessage = "An error occurred while loading the page. Please try again later." + ex.Message;
                return Page();
            }
        }

        public async Task<ActionResult> OnGetSelectedUserAsync(int profileId)
        {
            if (profileId <= 0)
                profileId = 1;

            SelectedUserImages = await _dataContext.Images.Where(id => id.UserProfile.Id == profileId).ToListAsync();

            return new JsonResult(SelectedUserImages.ToJson());
        }

        public async Task<IActionResult> OnPostSendEmail()
        {
            UserModel destinationUser = await _dataContext.PublicProfile
                .Where(p => p.Id == Convert.ToInt32(EmailDetails.Receiver))
                .Select(p => p.User)
                .FirstOrDefaultAsync();

            UserModel destinationuser = await _dataContext.PublicProfile.Where(profid => profid.Id == Convert.ToInt32(EmailDetails.Receiver)).Select(usr => usr.User).FirstOrDefaultAsync();

            EmailDetails.Receiver = await _dataContext.Users.Where(uid => uid == destinationuser).Select(uemail => uemail.Email).FirstOrDefaultAsync();
            await _emailSender.SendEmailAsync(EmailDetails.Receiver, EmailDetails.Title, EmailDetails.Body);

            string senderReponse = "Your message was sent successfully";
            await _emailSender.SendEmailAsync(EmailDetails.Sender, EmailDetails.Title, senderReponse);

            var CurrMsg = await _dataContext.ContactQueue.
                Where(em => (em.Receiver == EmailDetails.Receiver) && (em.Title == EmailDetails.Title) && (em.Body == EmailDetails.Body)).
                FirstOrDefaultAsync();

            if (CurrMsg != null)
            {
                CurrMsg.Sender = EmailDetails.Sender;
                await _dataContext.SaveChangesAsync();
            }

            ResponseMessage = "email message sent successfully";
            return RedirectToPage();
        }

        public async Task<ActionResult> OnPostUpdateSubscriptionAsync(string TransactionID, string JsonData)
        {
            var user = await _userManager.GetUserAsync(User);

            var publicUser = await _dataContext.PublicProfile.Where(puid => puid.User == user).FirstOrDefaultAsync();

            var subscription = new Subscription
            {
                SubStaDate = DateTime.UtcNow.ToString(),
                SubEndDate = DateTime.UtcNow.AddYears(1).ToString(),
                IsSubscribed = true,
                TransId = TransactionID,
                JsonTrans = JsonData,
                UserProfile = user,
                UserPublicProfile = publicUser
            };

            await _dataContext.AddAsync(subscription);
            var posted = _dataContext.SaveChangesAsync();

            //database operation failed
            if (posted.Result <= 0)
                return new JsonResult("user error: Error saving the values");

            return new JsonResult("subscriptin update successful");
        }
    }
}