using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using ZennoLab.Models;
using ZennoLab.Utils;
using ZennoLab.Storages.MSSQL;
using ZennoLab.Infrastructure;
using System.Linq;

namespace ZennoLab.Controllers
{
    public class HomeController : Controller
    {
        IWebHostEnvironment _appEnvironment;
        private IImageSetsRepository _imageSetsRepository;

        public HomeController(IWebHostEnvironment appEnvironment, IImageSetsRepository imageSetsRepository)
        {
            _appEnvironment = appEnvironment;
            _imageSetsRepository = imageSetsRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AddImageSet(ImageSetViewModel viewModel)
        {
            viewModel.ValidationErrors = new List<string>();
            var results = new List<ValidationResult>();
            var context = new ValidationContext(viewModel);

            if (!Validator.TryValidateObject(viewModel, context, results, true))
            {
                viewModel.ValidationErrors.AddRange(results.Select(error => error.ErrorMessage));
                return RedirectToAction("Index", viewModel);
            }

            var path = UploadFile(viewModel.Archive);
            if (string.IsNullOrEmpty(path))
            {
                viewModel.ValidationErrors.Add("Ошибка при загрузке файла");
                return RedirectToAction("Index", viewModel);
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
            };

            var zipReader = new ZipReader(imageSet);

            if (zipReader.ErrorsList.Any())
            {
                viewModel.ValidationErrors.AddRange(zipReader.ErrorsList);
                return RedirectToAction("Index", viewModel);
            }

            imageSet.CreateDate = DateTime.Now;
            await _imageSetsRepository.Insert(imageSet);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Index(ImageSetViewModel viewModel)
        {
            viewModel.AllSets = await _imageSetsRepository.SelectAll();
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

        private string UploadFile(IFormFile file)
        {
            try
            {
                var uploadDir = $"{_appEnvironment.WebRootPath}/files";
                if (!Directory.Exists(uploadDir)) Directory.CreateDirectory(uploadDir);
                var path = $"{uploadDir}/{file.FileName}";
                FileStream fileStream = new FileStream(path, FileMode.Create);
                file.CopyTo(fileStream);
                fileStream.Close();
                return path;
            }
            catch 
            {
                return string.Empty;
            }
        }
    }
}
