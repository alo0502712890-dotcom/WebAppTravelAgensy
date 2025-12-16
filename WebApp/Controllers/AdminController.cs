using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using WebApp.DB;
using WebApp.Dto;
using WebApp.Entity;
using WebApp.Helpers;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Controllers
{

    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AgencyDBContext _agencyDBContext;
        private OptionModels _optionModel;
        private CategoryModel _categoryModel;
        private TagModel _tagModel;

        public AdminController(AgencyDBContext agencyDBContext)
        {
            _agencyDBContext = agencyDBContext;
            _optionModel = new OptionModels(agencyDBContext);
            _categoryModel = new CategoryModel(agencyDBContext);
            _tagModel = new TagModel(agencyDBContext);
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

        //================ Categories =====================

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
            EditedCategoryViewModel vm = new EditedCategoryViewModel();

            Category? oldCategory = _categoryModel.GetCategoryById(dtoCat.Id);
            if (oldCategory == null)
            {
                return RedirectToAction("Index", "Error");
            }

            vm.EditedCategory = oldCategory;
            vm.Categories = _categoryModel.GetAllCategories();

            // slug має бути унікальний
            if (_categoryModel.SlugExistsForEdit(dtoCat.Slug, dtoCat.Id))
            {
                ViewBag.Error = "Slug має бути унікальним";
                return View(vm);
            }

            string imgSrcToSave = oldCategory.ImgSrc;

            if (dtoCat.Avatar != null && dtoCat.Avatar.Length > 0)
            {
                if (!ImageValidator.IsValidImage(dtoCat.Avatar, out string error))
                {
                    ViewBag.Error = error;
                    return View(vm);
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
                    FileService.DeleteFile(uploadFolder, oldCategory.ImgSrc);
                }

                string extension = Path.GetExtension(dtoCat.Avatar.FileName);
                string newFilePath = Path.Combine(uploadFolder, $"{dtoCat.Slug}{extension}");
                FileService.SaveImage(dtoCat.Avatar, newFilePath);

                imgSrcToSave = newFilePath.Split("wwwroot")[1];
            }

            if (_categoryModel.UpdateCategory(oldCategory, dtoCat, imgSrcToSave))
            {
                return RedirectToAction("Categories", "Admin");
            }

            ViewBag.Error = "Не вдалося зберегти категорію";
            return View(vm);
        }



        [HttpGet]
        public string RemoveCategory(int categoryId)
        {
            JsonSerializerOptions jso = new JsonSerializerOptions();
            jso.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;

            Category? remove = _categoryModel.GetCategoryById(categoryId);
            if (remove != null)
            {
                if (_categoryModel.RemoveCategory(remove))
                {
                    // jso.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    return JsonSerializer.Serialize(new JsonResponse()
                    {
                        Code = 200,
                        Status = StatusResponse.Success,
                        Message = "Category removed",
                    }, jso);
                }
            }

            return JsonSerializer.Serialize(new JsonResponse()
            {
                Code = 200,
                Status = StatusResponse.Error,
                Message = "Category Not removed",
            }, jso);
        }

        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View(_categoryModel.GetAllCategories());
        }

        [HttpPost]
        public IActionResult CreateCategory(CategoryEditDto dtoCat)
        {

            if (dtoCat.Avatar == null || dtoCat.Avatar.Length == 0)
            {
                ViewBag.Error = "Оберіть зображення категорії";
                return View(_categoryModel.GetAllCategories());
            }

            if (!ImageValidator.IsValidImage(dtoCat.Avatar, out string error))
            {
                ViewBag.Error = error;
                return View(_categoryModel.GetAllCategories());
            }


            string uploadFolder = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "assets",
                "media",
                "categories"
            );

            string extension = Path.GetExtension(dtoCat.Avatar.FileName);

            string newFilePath = Path.Combine(uploadFolder, $"{dtoCat.Slug}{extension}");
            FileService.SaveImage(dtoCat.Avatar, newFilePath);

            Category newCat = new Category();
            newCat.ImgSrc = newFilePath.Split("wwwroot")[1];
            newCat.Slug = dtoCat.Slug;
            newCat.ParentID = dtoCat.ParentID;
            newCat.Description = dtoCat.Description;
            newCat.Name = dtoCat.Name;


            //  slug вже існує
            if (_categoryModel.SlugExists(dtoCat.Slug))
            {
                ViewBag.Error = "Категорія з таким Slug вже існує";
                return View(_categoryModel.GetAllCategories());
            }

            if (_categoryModel.CreateCategory(newCat))
            {
                return RedirectToAction("Categories", "Admin");
            }

            return RedirectToAction("Index", "Error");

        }
        //============= теги====================

        [HttpGet]
        public IActionResult Tags()
        {
            return View(_tagModel.GetAllTag());
        }

        [HttpGet]
        public IActionResult CreateTag()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateTag(Tag tag)
        {
            if (_tagModel.CreateTag(tag))
            {
                return RedirectToAction("Tags", "Admin");
            }

            // мінімально: передаємо повідомлення
            ViewBag.Error = "Тег з таким Slug вже існує";
            return View(tag);
        }



        [HttpPost]
        public IActionResult RemoveTag(int tagId)
        {
            var tag = _tagModel.GetTagById(tagId);
            if (tag == null)
            {
                return Json(new JsonResponse()
                {
                    Code = 404,
                    Status = StatusResponse.Error,
                    Message = "Tag not found"
                });
            }

            var result = _tagModel.RemoveTag(tag);

            return Json(new JsonResponse()
            {
                Code = result ? 200 : 500,
                Status = result ? StatusResponse.Success : StatusResponse.Error,
                Message = result ? "Tag removed" : "Tag not removed"
            });
        }

        [HttpGet]
        public IActionResult EditTag(int tagId)
        {
            var tag = _tagModel.GetTagById(tagId);

            if (tag == null)
                return RedirectToAction("Index", "Error");

            return View(tag);
        }


        [HttpPost]
        public IActionResult EditTag(Tag tag)
        {
            if (_tagModel.UpdateTag(tag))
            {
                return RedirectToAction("Tags", "Admin");
            }

            ViewBag.Error = "Slug має бути унікальним";
            return View(tag);
        }

    }
}
