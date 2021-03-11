using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.IO.Packaging;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ZennoLab.Utils
{
    public class ZipReader
    {
        public bool IsZipArchive(string zipPath)
        {
            if (zipPath is null)
            {
               return  false;
            }
            try
            {
                using (var zipFile = ZipFile.OpenRead(zipPath))
                {
                    var entries = zipFile.Entries;
                    return true;
                }
            }
            catch(Exception e)
            {
                var ee = e.Message;
                return false;
            }
        }
        
        public List<string> GetFileList(string zipPath)
        {
            var fileList = new List<string>();
            if (zipPath is null)
            {
                throw new ArgumentNullException(nameof(zipPath));
            }
            using (ZipArchive zipFile = ZipFile.OpenRead(zipPath))
            {
                foreach (ZipArchiveEntry zip in zipFile.Entries)
                {
                    fileList.Add(zip.FullName);
                }
            }
            return fileList;
        }

        public List<string> ValidateAnswersFile(string zipPath, int filesCount)
        {
            var errorsList = new List<string>();
            if (zipPath is null)
            {
                throw new ArgumentNullException(nameof(zipPath));
            }
            using (ZipArchive zipFile = ZipFile.OpenRead(zipPath))
            {
                var answers = zipFile.GetEntry("answers.txt");
                if (answers == null)
                {
                    errorsList.Add("В архиве отсутствует файл с ответами");
                    return errorsList;
                }
                byte[] answersBytes = new byte[answers.Length];
                using (var zipEntryStream = answers.Open())
                {
                    var s = zipEntryStream.Read(answersBytes);
                }
                var answersStr = Encoding.UTF8.GetString(answersBytes).Trim();
                var answersList = answersStr.Split("\n");
                var nonEmptyAnswersList = answersList.Where(i => !String.IsNullOrEmpty(i));

                if (nonEmptyAnswersList.Count() != filesCount)
                {
                    errorsList.Add("Количество ответов не совпадает с количеством файлов в архиве");
                    return errorsList;
                }
            }
            return errorsList;
        }
    }
}
