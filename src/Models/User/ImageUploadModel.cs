using System.ComponentModel.DataAnnotations;

namespace Social2s.Models.User
{
    public class ImageUploadModel
    {
        [Required]
        [Display(Name = "image file")]
        public IFormFile FormFile { get; set; }
    }
}
