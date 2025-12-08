using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.DB;
using WebApp.Dto;
using WebApp.Entity;
using WebApp.Helpers;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Controllers
{

    [Authorize]
    public class AdminController : Controller
    {
        private readonly AgencyDBContext _agencyDBContext;
        private OptionModels _optionModel;
        private CategoryModel _categoryModel;

        public AdminController(AgencyDBContext agencyDBContext)
        {
            _agencyDBContext = agencyDBContext;
            _optionModel = new OptionModels(agencyDBContext);
            _categoryModel = new CategoryModel(agencyDBContext);
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            return RedirectToAction("LoginIn", "Account");
        }


        public IActionResult Config()
        {
            return View(_optionModel.GetAllOptions());
        }
        public IActionResult EditOption(int optionId)
        {
            EditedOptionViewModel editedOptionView = new EditedOptionViewModel();
            editedOptionView.Option = _optionModel.GetOptionById(optionId);
            editedOptionView.Relations = _optionModel.GetUniqueRelations();

            return View(editedOptionView);
        }

        [HttpGet]
        public string RemoveOption(int optionId)
        {
            JsonSerializerOptions jso = new JsonSerializerOptions();
            jso.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;

            Option? remove = _optionModel.GetOptionById(optionId);
            if (remove != null)
            {
                if (_optionModel.RemoveOption(remove))
                {
                    // jso.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    return JsonSerializer.Serialize(new JsonResponse()
                    {
                        Code = 200,
                        Status = StatusResponse.Success,
                        Message = "Option removed",
                    }, jso);
                }
            }

            return JsonSerializer.Serialize(new JsonResponse()
            {
                Code = 200,
                Status = StatusResponse.Error,
                Message = "Option Not removed",
            }, jso);
        }


        [HttpGet]
        public IActionResult CreateOption()
        {
            var relations = _optionModel.GetRelations();
            return View(relations);
        }

        [HttpPost]
        public IActionResult CreateOption(Option option)
        {
            if (_optionModel.CreateOption(option))
            {
                return RedirectToAction("Config", "Admin");
            }
            return RedirectToAction("Index", "Error");
        }


        [HttpPost]
        public IActionResult EditOption(Option option)
        {
            if (_optionModel.UpdateOption(option))
            {
                return RedirectToAction("Config", "Admin");
            }
            else
            {
                return RedirectToAction("Index", "Error");
            }
        }



        [HttpGet]
        public IActionResult Categories()
        {
            return View(_categoryModel.GetAllCategories());
        }

        [HttpGet]
        public IActionResult EditCategory(int categoryId)
        {
            EditedCategoryViewModel editedCategoryView = new EditedCategoryViewModel();
            editedCategoryView.EditedCategory = _categoryModel.GetCategoryById(categoryId);
            if (editedCategoryView.EditedCategory != null)
            {
                editedCategoryView.Categories = _categoryModel.GetAllCategories();

                return View(editedCategoryView);
            }
            else
            {
                return RedirectToAction("Index", "Error");
            }
        }


        [HttpPost]
        public IActionResult EditCategory(CategoryEditDto dtoCat)
        {
            if (dtoCat.Avatar == null || dtoCat.Avatar.Length == 0)
            {
                return RedirectToAction("Index", "Error");          // ответ пользователю что не так 
            }

            if (!ImageValidator.IsValidImage(dtoCat.Avatar, out string error))
            {
                Console.WriteLine(error);
                return RedirectToAction("Index", "Error");
            }

            Category? oldCategory = _categoryModel.GetCategoryById(dtoCat.Id);
            if (oldCategory == null)
            {
                return RedirectToAction("Index", "Error");          // ответ пользователю что не так 
            }

            string uploadFolder = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "assets",
                "media",
                "categories"
            );

            if (!string.IsNullOrEmpty(oldCategory.ImgSrc))
            {
                // remove old image
                FileService.DeleteFile(uploadFolder, oldCategory.ImgSrc);
            }
            string extension = Path.GetExtension(dtoCat.Avatar.FileName);

            string newFilePath = Path.Combine(uploadFolder, $"{dtoCat.Slug}{extension}");
            FileService.SaveImage(dtoCat.Avatar, newFilePath);


            if (_categoryModel.UpdateCategory(oldCategory, dtoCat, newFilePath.Split("wwwroot")[1]))
            {
                return RedirectToAction("Categories", "Admin");
            }

            return RedirectToAction("Index", "Error");          // ответ пользователю что не так 
        }

    }
}
