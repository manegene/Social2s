using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Social2s.Areas.Identity.Data;
using Social2s.Models.Category;
using Social2s.Models.Contact;
using Social2s.Models.User;
using Social2s.Pages;
using Social2s.Pages.Admin;
using System.Security.Claims;

namespace social2.test_hidden_a3f9b2c1
{
    public class EvalSuiteA91f3cKmumHidden(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client = factory.CreateClient();
        private static IndexModel CreateModel(DataContext context,
        Mock<IEmailSender> emailMock,
        Mock<UserManager<UserModel>> userManagerMock)
        {
            Mock<ILogger<IndexModel>> loggerMock = new Mock<ILogger<IndexModel>>();

            IndexModel model = new(
                loggerMock.Object,
                context,
                emailMock.Object,
                userManagerMock.Object
            );

            ClaimsPrincipal user = new(new ClaimsIdentity(
            [
            new Claim(ClaimTypes.NameIdentifier, "00000000-0000-0000-0000-000000000001")
            ]));

            model.PageContext = new Microsoft.AspNetCore.Mvc.RazorPages.PageContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            return model;
        }
        private static DataContext GetInMemoryDb()
        {
            DbContextOptions<DataContext> options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(new Guid("00000000-0000-0000-0000-000000000007").ToString())
                .Options;

            return new DataContext(options);
        }

        private static Mock<UserManager<UserModel>> MockUserManager()
        {
            Mock<IUserStore<UserModel>> store = new Mock<IUserStore<UserModel>>();

            return new Mock<UserManager<UserModel>>(
                store.Object, null, null, null, null, null, null, null, null
            );
        }

        [Fact]
        public async Task OnGet_ReturnsPage_AndLoadsData()
        {
            DataContext context = GetInMemoryDb();
            Mock<IEmailSender> emailMock = new Mock<IEmailSender>();
            Mock<UserManager<UserModel>> userManagerMock = MockUserManager();

            UserModel user = new UserModel { Id = new Guid("00000000-0000-0000-0000-000000000002") };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(user);

            IndexModel model = CreateModel(context, emailMock, userManagerMock);

            ActionResult result = await model.OnGet();

            Assert.IsType<PageResult>(result);
            Assert.NotNull(model);
        }

        [Fact]
        public async Task OnPostAsync_DoesNotAllowIdTampering()
        {
            DataContext context = GetInMemoryDb();
            CreateModel model = new CreateModel(context)
            {
                Input = new CategoryDTO
                {
                    Id = 1,
                    Name = "Safe Category"
                }
            };

            await model.OnPostAsync();

            CategoryModel? saved = context.Category.FirstOrDefault();

            Assert.Null(saved);
        }

        [Theory]
        [InlineData("/Index")]
        public async Task Test_Failed_Page(string url)
        {
            HttpResponseMessage response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();

            Assert.Contains("The ConnectionString property has not been initialized", content);
        }
        
    }
}
