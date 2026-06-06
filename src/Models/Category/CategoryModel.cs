using Microsoft.Build.Framework;

namespace Social2s.Models.Category
{
    public class CategoryModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int ParentId { get; set; }

    }
}
