using WebApp.DB;
using WebApp.Entity;

namespace WebApp.Models
{
    public class CommentsModel
    {
        private AgencyDBContext _agencyDBContext;
        public CommentsModel(AgencyDBContext agencyDBContext)
        {
            _agencyDBContext = agencyDBContext;
        }
        public List<Comment> GetAllComments(int postId)
        {
            return _agencyDBContext.Comments.Where(c => c.IsRequired == true && c.PostId == postId).OrderBy(c => c.DateOfCreated).ToList();
        }
    }
}
