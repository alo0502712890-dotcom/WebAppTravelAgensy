using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using WebApp.DB;
using WebApp.Dto;
using WebApp.Entity;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class AjaxController : Controller
    {
        private readonly AgencyDBContext _agencyDBContext;

        private PostModel _postModel;
        private CommentsModel _commentsModel;

        public AjaxController(AgencyDBContext agencyDBContext)
        {
            _agencyDBContext = agencyDBContext;
            _postModel = new PostModel(agencyDBContext);
            _commentsModel = new CommentsModel(agencyDBContext);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public string GetAllComments(string slug)
        {
            JsonResponse jsonResponseError = new JsonResponse()
            {
                Code = 400,
                Status = StatusResponse.Error,
                Message = "Slug not valid data",
                Data = null
            };
            if (slug == null)
            {
                return JsonSerializer.Serialize(jsonResponseError);
            }

            Post? post = _postModel.GetPostBySlug(slug);
            if (post == null)
            {
                jsonResponseError.Message = "Post Not found";
                return JsonSerializer.Serialize(jsonResponseError);
            }
            ;


            List<Comment> comments = _commentsModel.GetAllComments(post.Id);
            List<CommentDto> commentDtos = new List<CommentDto>();
            foreach (Comment oneComment in comments)
            {
                commentDtos.Add(new CommentDto()
                {
                    Id = oneComment.Id,
                    DateOfCreated = oneComment.DateOfCreated,
                    ParentCommentId = oneComment.ParentCommentId,
                    PostId = oneComment.PostId,
                    Text = oneComment.Text,
                    UserAvatar = oneComment.UserAvatar,
                    UserEmail = oneComment.UserEmail,
                    UserLogin = oneComment.UserLogin
                });
            }

            JsonSerializerOptions jso = new JsonSerializerOptions();
            jso.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;

            // jso.ReferenceHandler = ReferenceHandler.IgnoreCycles;

            JsonResponse response = new JsonResponse()
            {
                Code = 200,
                Status = StatusResponse.Success,
                Message = "Data loaded",
                Data = JsonSerializer.Serialize(commentDtos, jso)
            };
            return JsonSerializer.Serialize(response, jso);
        }

        [HttpPost]
        public string AddComment([FromBody] CommentDto dto) 
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Text))
            {
                return JsonSerializer.Serialize(new JsonResponse
                {
                    Code = 400,
                    Status = StatusResponse.Error,
                    Message = "Invalid data"
                });
            }

            Post? post = _postModel.GetPostById(dto.PostId);
            if (post == null)
            {
                return JsonSerializer.Serialize(new JsonResponse
                {
                    Code = 404,
                    Status = StatusResponse.Error,
                    Message = "Post not found"
                });
            }

            Comment newComment = new Comment()
            {
                UserLogin = dto.UserLogin,
                UserEmail = dto.UserEmail,
                UserAvatar = dto.UserAvatar ?? "/images/avatars/anon.png",
                Text = dto.Text,
                PostId = dto.PostId,
                ParentCommentId = dto.ParentCommentId,
                DateOfCreated = DateTime.Now,
                IsRequired = false
            };

            _agencyDBContext.Comments.Add(newComment);
            _agencyDBContext.SaveChanges();

            JsonSerializerOptions jso = new JsonSerializerOptions();
            jso.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;

            return JsonSerializer.Serialize(new JsonResponse
            {
                Code = 200,
                Status = StatusResponse.Success,
                Message = "Comment added",
                Data = newComment.Id.ToString()
            }, jso);
        }

    }
}
