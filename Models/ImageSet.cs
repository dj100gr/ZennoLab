using System;
using ZennoLab.Interfaces;

namespace ZennoLab.Models
{
    public class ImageSet : IEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsCyrContains { get; set; }
        public bool IsLatContains { get; set; }
        public bool IsNumContains { get; set; }
        public bool IsScharContains { get; set; }
        public bool IsCaseSens { get; set; }
        public string ArchivePath { get; set; }
        public string AnswersLocation { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public DateTime? Removed { get; set; }

    }
}
