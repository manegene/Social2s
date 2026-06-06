using Social2s.Models.Category;
using Social2s.Models.User;

namespace Social2s.Models.LinkedModels
{
    public class PublicProfile_Category
    {
        public virtual UserPublicModel Profile { get; set; }
        public virtual CategoryModel Category { get; set; }
    }
}
