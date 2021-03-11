using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ZennoLab.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using ZennoLab.Utils;

namespace ZennoLab.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        IWebHostEnvironment _appEnvironment;
        private SqlContext db;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment appEnvironment, SqlContext context)
        {
            _logger = logger;
            _appEnvironment = appEnvironment;
            db = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddImageSet(ImageSetViewModel viewModel)
        {
            viewModel.ValidationErrors = new List<string>();
            var results = new List<ValidationResult>();
            var context = new ValidationContext(viewModel);

            if (!Validator.TryValidateObject(viewModel, context, results, true))
            {
                foreach (var error in results)
                {
                    viewModel.ValidationErrors.Add(error.ErrorMessage);
                }
                return RedirectToAction("Index", viewModel);
            }

            if (!viewModel.IsCyrContains && !viewModel.IsLatContains && !viewModel.IsNumContains)
            {
                viewModel.ValidationErrors.Add("Нужно выбрать одно из: 'Содержит кириллицу', 'Содержит латиницу', 'Содержит цифры'");
                return RedirectToAction("Index", viewModel);
            }

            if (viewModel.Archive == null)
            {
                viewModel.ValidationErrors.Add("Загрука файла обязательна");
                return RedirectToAction("Index", viewModel);
            }

            var fileName = viewModel.Archive.FileName;
            string fileExt = fileName.Substring(fileName.LastIndexOf('.'));
            if (fileExt != ".zip")
            {
                viewModel.ValidationErrors.Add("Можно загрузить только zip архив");
                return RedirectToAction("Index", viewModel);
            }

            var path = $"{_appEnvironment.WebRootPath}/files/{fileName}";
            FileStream fileStream = new FileStream(path, FileMode.Create);
            viewModel.Archive.CopyTo(fileStream);
            fileStream.Close();

            var reader = new ZipReader();
            if (!reader.IsZipArchive(path))
            {
                viewModel.ValidationErrors.Add("Файл не является zip архивом");
                return RedirectToAction("Index", viewModel);
            }

            var zipList = reader.GetFileList(path);
            var imageCount = zipList.Count;
            if (viewModel.AnswersLocation == "InDetachedFile") imageCount -= 1;

            var minCount = 2000;
            var maxCount = 3000;
            if (viewModel.IsCyrContains) 
            {
                minCount += 3000;
                maxCount += 3000;
            }
            if (viewModel.IsLatContains)
            {
                minCount += 3000;
                maxCount += 3000;
            }
            if (viewModel.IsNumContains)
            {
                minCount += 3000;
                maxCount += 3000;
            }
            if (viewModel.IsScharContains)
            {
                minCount += 3000;
                maxCount += 3000;
            }
            if (viewModel.IsCaseSens)
            {
                minCount += 3000;
                maxCount += 3000;
            }
            if (imageCount < minCount || imageCount > maxCount)
            {
                viewModel.ValidationErrors.Add($"При данных настройках архив может содержать от {minCount} до {maxCount} картинок");
                return RedirectToAction("Index", viewModel);
            }

            if (viewModel.AnswersLocation == "InDetachedFile" && !zipList.Contains("answers.txt"))
            {
                viewModel.ValidationErrors.Add("В архиве отсутствует файл с ответами");
                return RedirectToAction("Index", viewModel);
            }

            if (viewModel.AnswersLocation == "InDetachedFile")
            {
                var errorsList = reader.ValidateAnswersFile(path, imageCount);
                if (errorsList.Count > 0)
                {
                    viewModel.ValidationErrors.AddRange(errorsList);
                    return RedirectToAction("Index", viewModel);
                }
            }

            var imageSet = new ImageSet
            {
                Title = viewModel.Title,
                IsCyrContains = viewModel.IsCyrContains,
                IsLatContains = viewModel.IsLatContains,
                IsNumContains = viewModel.IsNumContains,
                IsScharContains = viewModel.IsScharContains,
                IsCaseSens = viewModel.IsCaseSens,
                AnswersLocation = viewModel.AnswersLocation,
                ArchivePath = path,
                CreateDate = DateTime.Now
            };
            db.ImageSets.Add(imageSet);
            await db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Index(ImageSetViewModel viewModel)
        {
            viewModel.AllSets = await db.ImageSets.ToListAsync();
            if (viewModel.ValidationErrors == null)
            {
                viewModel.ValidationErrors = new List<string>();
            }
            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
