using WebApp.DB;
using WebApp.Entity;

namespace WebApp.Models
{
    public class NavigationModel
    {
        private AgencyDBContext _agencyDBContext;
        public NavigationModel(AgencyDBContext agencyDBContext)
        {
            _agencyDBContext = agencyDBContext;
        }

        public IEnumerable<Navigate> GetNavigates()
        {
            return _agencyDBContext.Navigates.OrderBy(n => n.Order);
        }

        //public IEnumerable<Navigate> GetNavigationThree()
        //{
        //    var threeRoot = _agencyDBContext.Navigates
        //        .Where(n => n.ParentID == null)
        //        .OrderBy(n => n.Order)
        //        .ToList();
        //    var allChilds = _agencyDBContext.Navigates
        //        .Where(n => n.ParentID != null)
        //        .OrderBy(n => n.Order)
        //        .ToList();
        //    foreach (var oneParent in threeRoot)
        //    {
        //        foreach (var oneChild in allChilds)
        //        {
        //            if (oneChild.ParentID == oneParent.Id)
        //            {
        //                oneParent.Childs.Add(oneChild);
        //            }
        //        }
        //    }
        //    return threeRoot;
        //}

        public IEnumerable<Navigate> GetNavigationThree()
        {
            var allItems = _agencyDBContext.Navigates
                .OrderBy(n => n.Order)
                .ToList();

            // кореневі елементи
            var roots = allItems
                .Where(n => n.ParentID == null)
                .ToList();

            foreach (var root in roots)
            {
                BuildChildTree(root, allItems);
            }

            return roots;
        }

        //рекурсивна побудова
        private void BuildChildTree(Navigate parent, List<Navigate> allItems)
        {
            var childs = allItems
                .Where(n => n.ParentID == parent.Id)
                .OrderBy(n => n.Order)
                .ToList();

            parent.Childs = childs;

            foreach (var child in childs)
            {
                BuildChildTree(child, allItems); // рекурсія
            }
        }
    }
}
