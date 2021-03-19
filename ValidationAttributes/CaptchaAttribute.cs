using System.ComponentModel.DataAnnotations;
using ZennoLab.Models;

namespace ZennoLab.ValidationAttributes
{
    public class CaptchaAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value != null)
            {
                string str = value.ToString().ToLower();
                if (!str.Contains("captcha"))
                    return true;
                return false;
            }
            return false;
        }
    }
}


