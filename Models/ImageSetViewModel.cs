using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ZennoLab.Attributes;

namespace ZennoLab.Models
{
    public class ImageSetViewModel
    {
        [Required(ErrorMessage = "Имя обязательно для заполнения")]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "Недопустимая длина имени (может быть от 4 до 8 символов)")]
        [RegularExpression(@"^[a-zA-Z]{1,8}$", ErrorMessage = "Имя может содержать только латинские буквы")]
        [CaptchaAttribute( ErrorMessage = "Имя не может содержать слово 'captcha'")]
        public string Title { get; set; }
        public bool IsCyrContains { get; set; }
        public bool IsLatContains { get; set; }
        public bool IsNumContains { get; set; }
        public bool IsScharContains { get; set; }
        public bool IsCaseSens { get; set; }
        public string AnswersLocation { get; set; }
        public IFormFile Archive { get; set; }
        public IEnumerable<ImageSet> AllSets { get; set; }
        public List<string> ValidationErrors { get; set; }
    }
}
