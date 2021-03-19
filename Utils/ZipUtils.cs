using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using ZennoLab.Infrastructure;

namespace ZennoLab.Utils
{
    public class ZipReader
    {
        public ZipReader()
        {
            ErrorsList = new List<string>();
            FileList = new List<string>();
            FilesCount = 0;
            FilePath = string.Empty;
        }

        public ZipReader(IImageSet model)
        {
            ErrorsList = new List<string>();
            FileList = new List<string>();
            FilesCount = 0;
            FilePath = string.Empty;
            GetErrors(model);
        }

        private string FilePath { get; set; }

        public List<string> ErrorsList { get; set; }

        private List<string> FileList { get; set; }

        private int FilesCount { get; set; }

        private void GetErrors(IImageSet model)
        {     
            GetFileList(model.ArchivePath);
            
            FilesCount = FileList.Count;
            if (model.AnswersLocation == "InDetachedFile") FilesCount -= 1;

            if (!IsZipArchive())
            {
                ErrorsList.Add("Файл не является zip архивом");
                return;
            }

            if (model.AnswersLocation == "InDetachedFile" && !FileList.Contains("answers.txt"))
            {
                ErrorsList.Add("В архиве отсутствует файл с ответами");
                return;
            }
           
            ValidateFilesCount(model);

            if (ErrorsList.Any()) return;

            if (model.AnswersLocation == "InDetachedFile")
            {
                if (!FileList.Contains("answers.txt"))
                {
                    ErrorsList.Add("В архиве отсутствует файл с ответами");
                    return;
                }
                else
                {
                    ValidateAnswersFile();
                }
            }
            return;
        }

        private bool IsZipArchive()
        {
            if (FilePath is null)
            {
               return  false;
            }
            try
            {
                using var zipFile = ZipFile.OpenRead(FilePath);
                var entries = zipFile.Entries;
                return true;
            }
            catch
            {
                return false;
            }
        }
        
        private void GetFileList(string zipPath)
        {
            FilePath = zipPath;
            if (FilePath is null)
            {
                throw new ArgumentNullException(nameof(FilePath));
            }
            using (ZipArchive zipFile = ZipFile.OpenRead(FilePath))
            {
                foreach (ZipArchiveEntry zip in zipFile.Entries)
                {
                    FileList.Add(zip.FullName);
                }
            }
        }

        public void ValidateFilesCount(IImageSet model)
        {
            var minCount = 2000;
            var maxCount = 3000;
            if (model.IsCyrContains)
            {
                minCount += 3000;
                maxCount += 3000;
            }
            if (model.IsLatContains)
            {
                minCount += 3000;
                maxCount += 3000;
            }
            if (model.IsNumContains)
            {
                minCount += 3000;
                maxCount += 3000;
            }
            if (model.IsScharContains)
            {
                minCount += 3000;
                maxCount += 3000;
            }
            if (model.IsCaseSens)
            {
                minCount += 3000;
                maxCount += 3000;
            }
            if (FilesCount < minCount || FilesCount > maxCount)
            {
                ErrorsList.Add($"При данных настройках архив может содержать от {minCount} до {maxCount} картинок");
            }
        }

        public void ValidateAnswersFile()
        {
            if (FilePath is null)
            {
                throw new ArgumentNullException(nameof(FilePath));
            }
            using (ZipArchive zipFile = ZipFile.OpenRead(FilePath))
            {
                var answers = zipFile.GetEntry("answers.txt");
                byte[] answersBytes = new byte[answers.Length];
                using (var zipEntryStream = answers.Open())
                {
                    var s = zipEntryStream.Read(answersBytes);
                }
                var answersStr = Encoding.UTF8.GetString(answersBytes).Trim();
                var answersList = answersStr.Split("\n");
                var nonEmptyAnswersList = answersList.Where(i => !String.IsNullOrEmpty(i));

                if (nonEmptyAnswersList.Count() != FilesCount)
                {
                    ErrorsList.Add("Количество ответов не совпадает с количеством файлов в архиве");
                }
            }
        }
    }
}
