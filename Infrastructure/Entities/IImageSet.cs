namespace ZennoLab.Infrastructure
{
    public interface IImageSet : IEntity
    {
        string Title { get; set; }
        bool IsCyrContains { get; set; }
        bool IsLatContains { get; set; }
        bool IsNumContains { get; set; }
        bool IsScharContains { get; set; }
        bool IsCaseSens { get; set; }
        string ArchivePath { get; set; }
        string AnswersLocation { get; set; }
    }
}
