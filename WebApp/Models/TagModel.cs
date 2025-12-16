using WebApp.DB;
using WebApp.Entity;

namespace WebApp.Models
{
    public class TagModel
    {
        private AgencyDBContext _agencyDBContext;
        public TagModel(AgencyDBContext agencyDBContext)
        {
            _agencyDBContext = agencyDBContext;
        }
        public IEnumerable<Tag> GetNotEmptyTags()
        {
            return _agencyDBContext.Tags
                  .Where(tag => _agencyDBContext.PostTags.Any(pt => pt.TagId == tag.Id))
                  .ToList();
        }

        public IEnumerable<Tag> GetAllTag()
        {
            return _agencyDBContext.Tags.ToList();
        }

        public bool CreateTag(Tag tag)
        {
            
            bool slugExists = _agencyDBContext.Tags.Any(t => t.Slug == tag.Slug);
            if (slugExists)
                return false;

            _agencyDBContext.Tags.Add(tag);
            return _agencyDBContext.SaveChanges() > 0;
        }


        public bool RemoveTag(Tag tag)
        {
            try
            {
                _agencyDBContext.Tags.Remove(tag);
                return _agencyDBContext.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }


        public Tag? GetTagById(int tagId)
        {
            return _agencyDBContext.Tags.FirstOrDefault(c => c.Id == tagId);
        }


        public bool UpdateTag(Tag tag)
        {
            // перевірка: такий slug є, але НЕ у цього ж тега
            bool slugExists = _agencyDBContext.Tags
                .Any(t => t.Slug == tag.Slug && t.Id != tag.Id);

            if (slugExists)
                return false;

            Tag? origin = GetTagById(tag.Id);
            if (origin == null)
                return false;

            origin.Name = tag.Name;
            origin.Slug = tag.Slug;

            return _agencyDBContext.SaveChanges() > 0;
        }

    }
}
