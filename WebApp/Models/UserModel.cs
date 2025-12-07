using WebApp.DB;
using WebApp.Entity;

namespace WebApp.Models
{
    public class UserModel
    {
        private AgencyDBContext _agencyDBContext;
        public UserModel(AgencyDBContext agencyDBContext)
        {
            _agencyDBContext = agencyDBContext;
        }

        public User? GetUserByEmail(string email)
        {
            return _agencyDBContext.Users.SingleOrDefault(u => u.Email.ToLower() == email.ToLower());
        }

        /// <summary>
        /// Асинхронно добавляет нового пользователя в базу данных.
        /// </summary>
        /// <param name="user">Объект пользователя, который нужно добавить.</param>
        /// <returns>True, если добавление прошло успешно, иначе False.</returns>
        public async Task<bool> AddUserAsync(User user)
        {
            try
            {
                await _agencyDBContext.Users.AddAsync(user);

                await _agencyDBContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при добавлении пользователя: {ex.Message}");
                return false; 
            }
        }

        // Перегрузка для синхронного вызова 
        public bool AddUser(User user)
        {
            try
            {
                _agencyDBContext.Users.Add(user);
                _agencyDBContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при добавлении пользователя: {ex.Message}");
                return false;
            }
        }
    }
}
