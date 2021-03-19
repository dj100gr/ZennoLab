using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ZennoLab.ValidationAttributes;
using ZennoLab.Infrastructure;

namespace ZennoLab.Models
{
    public class ImageSetViewModel : IValidatableObject
    {
        [Required(ErrorMessage = "Имя обязательно для заполнения")]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "Недопустимая длина имени (может быть от 4 до 8 символов)")]
        [RegularExpression(@"^[a-zA-Z]{1,8}$", ErrorMessage = "Имя может содержать только латинские буквы")]
        [CaptchaAttribute(ErrorMessage = "Имя не может содержать слово 'captcha'")]
        public string Title { get; set; }
        public bool IsCyrContains { get; set; }
        public bool IsLatContains { get; set; }
        public bool IsNumContains { get; set; }
        public bool IsScharContains { get; set; }
        public bool IsCaseSens { get; set; }
        public string AnswersLocation { get; set; }
        public IFormFile Archive { get; set; }
        public IEnumerable<IImageSet> AllSets { get; set; }
        public List<string> ValidationErrors { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            if (!this.IsCyrContains && !this.IsLatContains && !this.IsNumContains)
            {
                errors.Add(new ValidationResult("Нужно выбрать одно из: 'Содержит кириллицу', 'Содержит латиницу', 'Содержит цифры'"));
            }

            if (this.Archive == null)
            {
                errors.Add(new ValidationResult("Загрука файла обязательна"));
            }
            else 
            {
                var fileName = this.Archive.FileName;
                string fileExt = fileName.Substring(fileName.LastIndexOf('.'));
                if (fileExt != ".zip")
                {
                    errors.Add(new ValidationResult("Можно загрузить только zip архив"));
                }
            }
            return errors;
        }
    }
}
