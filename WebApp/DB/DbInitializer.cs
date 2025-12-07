using Microsoft.EntityFrameworkCore;
using WebApp.Entity;

namespace WebApp.DB
{
    /// <summary>
    /// Класс инициализации базы данных.
    /// Заполняет базу данных начальными данными.
    /// </summary>
    public class DbInitializer
    {
        /// <summary>
        /// Метод для инициализации базы данных и заполнения её начальными данными.
        /// </summary>
        public static void Initialize(AgencyDBContext context)
        {
            if (!context.Database.CanConnect())
            {
                // Якщо бази немає - створюємо її
                context.Database.EnsureCreated();
            }

            context.Database.Migrate();
            SeedOptions(context);
            context.SaveChanges();
            SeedNavigates(context);
            context.SaveChanges();
            SeedClientMessages(context);
            context.SaveChanges();


            SeedTags(context);
            SeedCategories(context);
            SeedPosts(context);
            SeedPostCategories(context);
            SeedPostTags(context);
            SeedComments(context);
            SeedUsers(context);
        }
        private static bool CheckIfTablesExist(AgencyDBContext context)
        {
            try
            {
                var optionsExist = context.Options.Any();
                var navigatesExist = context.Navigates.Any();
                var clientMessagesExist = context.ClientMessages.Any();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Заполняем таблицу Options начальными данными.
        /// </summary>
        private static void SeedOptions(AgencyDBContext context)
        {
            if (!context.Options.Any())
            {
                context.Options.AddRange(
                // ================== TOPBAR ==============
                new Option()
                {
                    IsSystem = true,
                    Name = "site-logo",
                    Value = "МріяТур",
                    Key = "<i class=\"fa fa-map-marker-alt me-3\"></i>",
                },
                // ========== TOPBAR: social link ==========
                new Option()
                {
                    IsSystem = false,
                    Relation = "social-link",
                    Name = "Twitter",
                    Value = "https://x.com/?lang=ru",
                    Key = "<i class=\"fab fa-twitter fw-normal\"></i>",
                },
                new Option()
                {
                    IsSystem = false,
                    Relation = "social-link",
                    Name = "Facebook",
                    Value = "https://facebook.com/",
                    Key = "<i class=\"fab fa-facebook-f fw-normal\"></i>",
                },
                new Option()
                {
                    IsSystem = false,
                    Relation = "social-link",
                    Name = "LinkedIn",
                    Value = "https://www.linkedin.com/",
                    Key = "<i class=\"fab fa-linkedin-in fw-normal\"></i>",
                },
                new Option()
                {
                    IsSystem = false,
                    Relation = "social-link",
                    Name = "Instagram",
                    Value = "https://www.instagram.com/",
                    Key = "<i class=\"fab fa-instagram fw-normal\"></i>",
                },
                new Option()
                {
                    IsSystem = false,
                    Relation = "social-link",
                    Name = "YouTube",
                    Value = "https://www.youtube.com/",
                    Key = "<i class=\"fab fa-youtube fw-normal\"></i>",
                },
                // ========== TOPBAR: Eser menu ==========
                new Option()
                {
                    IsSystem = true,
                    Relation = "user-menu",
                    Name = "Увійти",
                    Value = "/Account/LoginIn",
                    Key = "<i class=\"fa fa-sign-in-alt me-2\"></i>"
                },
                new Option()
                {
                    IsSystem = true,
                    Relation = "user-menu",
                    Name = "Зареєструватись",
                    Value = "/Account/RegisterIn",
                    Key = "<i class=\"fa fa-user-plus me-2\"></i>"
                },
                new Option()
                {
                    IsSystem = true,
                    Relation = "user-menu",
                    Name = "Панель керування",
                    Value = "#", // головний пункт (dropdown)
                    Key = "<i class=\"fa fa-home me-2\"></i>"
                },
                new Option()
                {
                    IsSystem = true,
                    Relation = "dashboard-item",
                    Name = "My Profile",
                    Value = "/Account/Profile",
                    Key = "<i class=\"fas fa-user-alt me-2\"></i>"
                },
                new Option()
                {
                    IsSystem = true,
                    Relation = "dashboard-item",
                    Name = "Inbox",
                    Value = "/Account/Inbox",
                    Key = "<i class=\"fas fa-comment-alt me-2\"></i>"
                },
                new Option()
                {
                    IsSystem = true,
                    Relation = "dashboard-item",
                    Name = "Notifications",
                    Value = "/Account/Notifications",
                    Key = "<i class=\"fas fa-bell me-2\"></i>"
                },
                new Option()
                {
                    IsSystem = true,
                    Relation = "dashboard-item",
                    Name = "Account Settings",
                    Value = "/Account/Settings",
                    Key = "<i class=\"fas fa-cog me-2\"></i>"
                },
                new Option()
                {
                    IsSystem = true,
                    Relation = "dashboard-item",
                    Name = "Log Out",
                    Value = "/Account/Logout",
                    Key = "<i class=\"fas fa-power-off me-2\"></i>"
                },
                new Option
                {
                    IsSystem = true,
                    Relation = "navbar-button",
                    Name = "Забронювати зараз",
                    Value = "/services/booking",
                    Key = "<i class=\"fa fa-calendar-check me-2\"></i>"
                },
                // ========== FOOTER: Contacts ==========
                new Option()
                {
                    IsSystem = false,
                    Relation = "footer-contacts",
                    Name = "Address",
                    Key = "<i class=\"fas fa-home me-2\"></i>",
                    Value = "Україна, м.Полтава, вул.Незалежності, 24"
                },
                new Option()
                {
                    IsSystem = false,
                    Relation = "footer-contacts",
                    Name = "Email",
                    Key = "<i class=\"fas fa-envelope me-2\"></i>",
                    Value = "mriya@example.com"
                },
                new Option()
                {
                    IsSystem = false,
                    Relation = "footer-contacts",
                    Name = "Phone",
                    Key = "<i class=\"fas fa-phone me-2\"></i>",
                    Value = "+012 345 67890"
                },
                new Option()
                {
                    IsSystem = false,
                    Relation = "footer-contacts",
                    Name = "Fax",
                    Key = "<i class=\"fas fa-print me-2\"></i>",
                    Value = "+012 345 67890"
                },

                // ========== FOOTER: Company ==========
                new Option()
                {
                    IsSystem = false,
                    Relation = "footer-company",
                    Name = "Про нас",
                    Value = "/about/index",
                    Key = "<i class=\"fas fa-angle-right me-2\"></i>"
                },
                new Option()
                {
                    IsSystem = false,
                    Relation = "footer-company",
                    Name = "Кар'єра",
                    Value = "/about/careers",
                    Key = "<i class=\"fas fa-angle-right me-2\"></i>"
                },
                new Option()
                {
                    IsSystem = false,
                    Relation = "footer-company",
                    Name = "Блог",
                    Value = "/blog/index",
                    Key = "<i class=\"fas fa-angle-right me-2\"></i>"
                },
                new Option()
                {
                    IsSystem = false,
                    Relation = "footer-company",
                    Name = "Преса",
                    Value = "#",
                    Key = "<i class=\"fas fa-angle-right me-2\"></i>"
                },
                new Option()
                {
                    IsSystem = false,
                    Relation = "footer-company",
                    Name = "Подарункові картки",
                    Value = "#",
                    Key = "<i class=\"fas fa-angle-right me-2\"></i>"
                },

                // ========== FOOTER: Support ==========
                new Option()
                {
                    IsSystem = false,
                    Relation = "footer-support",
                    Name = "Контакти",
                    Value = "/about/contactus",
                    Key = "<i class=\"fas fa-angle-right me-2\"></i>"
                },
                new Option()
                {
                    IsSystem = false,
                    Relation = "footer-support",
                    Name = "Політика конфіденційності",
                    Value = "/about/privacy",
                    Key = "<i class=\"fas fa-angle-right me-2\"></i>"
                },
                new Option()
                {
                    IsSystem = false,
                    Relation = "footer-support",
                    Name = "Умови та положення",
                    Value = "/about/terms",
                    Key = "<i class=\"fas fa-angle-right me-2\"></i>"
                },
                new Option()
                {
                    IsSystem = false,
                    Relation = "footer-support",
                    Name = "Політика файлів cookie",
                    Value = "#",
                    Key = "<i class=\"fas fa-angle-right me-2\"></i>"
                },

                // ========== FOOTER: Language & Currency ==========
                new Option()
                {
                    IsSystem = false,
                    Relation = "footer-language",
                    Name = "Language",
                    Key = "en,uk,de,es",
                    Value = "English,Українська,Deutsch,Español"
                },
                new Option()
                {
                    IsSystem = false,
                    Relation = "footer-language",
                    Name = "Currency",
                    Key = "USD,EUR,GBP",
                    Value = "$,€,£"
                },
                new Option()
                {
                    IsSystem = false,
                    Relation = "footer-language",
                    Name = "Payments",
                    Key = "<i class='fab fa-cc-visa fa-2x'></i><i class='fab fa-cc-mastercard fa-2x'></i><i class='fab fa-cc-paypal fa-2x'></i>",
                    Value = "#"
                }


                );
            }
        }

        /// <summary>
        /// Заполняем таблицу Navigates начальными данными.
        /// </summary>
        private static void SeedNavigates(AgencyDBContext context)
        {
            if (!context.Navigates.Any())
            {
                // Открываем соединение
                context.Database.OpenConnection();

                try
                {
                    // Разрешаем вставку собственных Id
                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Navigates ON");

                    context.Navigates.AddRange(
                        new Navigate
                        {
                            Id = 1,
                            Title = "Головна",
                            Href = "/home/index",
                            Order = 1,
                            ParentID = null
                        },
                        new Navigate
                        {
                            Id = 2,
                            Title = "Блог",
                            Href = "/blog/index",
                            Order = 2,
                            ParentID = null
                        },
                        new Navigate
                        {
                            Id = 3,
                            Title = "Про Нас",
                            Href = "/about/index",
                            Order = 3,
                            ParentID = null
                        },
                        new Navigate
                        {
                            Id = 4,
                            Title = "Зв'яжіться з нами",
                            Href = "/about/contactus",
                            Order = 1,
                            ParentID = 3
                        },
                        new Navigate
                        {
                            Id = 6,
                            Title = "Новини",
                            Href = "/blog/news",
                            Order = 1,
                            ParentID = 2
                        },
                        new Navigate
                        {
                            Id = 7,
                            Title = "Інновації",
                            Href = "/blog/inovation",
                            Order = 2,
                            ParentID = 2
                        },
                        // --- 3 рівень (діти пункту "Новини") ---
                        new Navigate
                        {
                            Id = 17,
                            Title = "2025",
                            Href = "/blog/news/2025",
                            Order = 1,
                            ParentID = 6
                        },
                        new Navigate
                        {
                            Id = 18,
                            Title = "2024",
                            Href = "/blog/news/2024",
                            Order = 2,
                            ParentID = 6
                        },
                        // --- 4 рівень (діти пункту "2025") ---
                        new Navigate
                        {
                            Id = 19,
                            Title = "Січень",
                            Href = "/blog/news/2025/january",
                            Order = 1,
                            ParentID = 17
                        },
                        new Navigate
                        {
                            Id = 20,
                            Title = "Лютий",
                            Href = "/blog/news/2025/february",
                            Order = 2,
                            ParentID = 17
                        },
                        // ----
                        new Navigate
                        {
                            Id = 9,
                            Title = "Пакети турів",
                            Href = "/packages/index",
                            Order = 4,
                            ParentID = null
                        },
                        new Navigate
                        {
                            Id = 10,
                            Title = "Популярні напрямки",
                            Href = "/packages/destination",
                            Order = 1,
                            ParentID = 9
                        },
                        new Navigate
                        {
                            Id = 11,
                            Title = "Підбір туру",
                            Href = "/packages/tour",
                            Order = 2,
                            ParentID = 9
                        },
                        new Navigate
                        {
                            Id = 12,
                            Title = "Сервіси",
                            Href = "/services/index",
                            Order = 5,
                            ParentID = null
                        },
                        new Navigate
                        {
                            Id = 13,
                            Title = "Відгуки",
                            Href = "/services/testimonial",
                            Order = 1,
                            ParentID = 12
                        },
                        new Navigate
                        {
                            Id = 14,
                            Title = "Бронювання туру",
                            Href = "/services/booking",
                            Order = 2,
                            ParentID = 12
                        },
                        new Navigate
                        {
                            Id = 15,
                            Title = "Галерея",
                            Href = "/about/gallery",
                            Order = 3,
                            ParentID = 3
                        },
                        new Navigate
                        {
                            Id = 16,
                            Title = "Наші гіди",
                            Href = "/about/guides",
                            Order = 4,
                            ParentID = 3
                        }
                    );

                    context.SaveChanges();

                    // Отключаем разрешение вставки собственных Id
                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Navigates OFF");
                }
                finally
                {
                    // Закрываем соединение даже при ошибках
                    context.Database.CloseConnection();
                }
            }
        }

        /// <summary>
        /// Заполняем таблицу ClientMessages начальными данными.
        /// </summary>
        private static void SeedClientMessages(AgencyDBContext context)
        {
            if (!context.ClientMessages.Any())
            {
                context.ClientMessages.AddRange(
                    new ClientMessage
                    {
                        UserName = "John Doe",
                        UserEmail = "john.doe@example.com",
                        Subject = "Inquiry about product",
                        Message = "I would like to know more about your product.",
                        DateOfCreated = new DateTime(2025, 10, 28),
                        IsAnswered = false
                    },
                    new ClientMessage
                    {
                        UserName = "Jane Smith",
                        UserEmail = "jane.smith@example.com",
                        Subject = "Request for support",
                        Message = "Can you help me with my account issues?",
                        DateOfCreated = new DateTime(2025, 10, 29),
                        IsAnswered = false
                    },
                    new ClientMessage
                    {
                        UserName = "Michael Johnson",
                        UserEmail = "michael.johnson@example.com",
                        Subject = "Billing question",
                        Message = "I was charged twice for my last order. Please assist.",
                        DateOfCreated = new DateTime(2025, 10, 30),
                        IsAnswered = false
                    },
                    new ClientMessage
                    {
                        UserName = "Emily Davis",
                        UserEmail = "emily.davis@example.com",
                        Subject = "Feedback on website",
                        Message = "Your website is great, but the checkout process could be smoother.",
                        DateOfCreated = new DateTime(2025, 10, 31),
                        IsAnswered = false
                    },
                    new ClientMessage
                    {
                        UserName = "Robert Brown",
                        UserEmail = "robert.brown@example.com",
                        Subject = "Feature request",
                        Message = "Can you add dark mode to your app?",
                        DateOfCreated = new DateTime(2025, 11, 1),
                        IsAnswered = false
                    },
                    new ClientMessage
                    {
                        UserName = "Laura Wilson",
                        UserEmail = "laura.wilson@example.com",
                        Subject = "Login issue",
                        Message = "I can’t log into my account after the update.",
                        DateOfCreated = new DateTime(2025, 11, 2),
                        IsAnswered = false
                    },
                    new ClientMessage
                    {
                        UserName = "David Miller",
                        UserEmail = "david.miller@example.com",
                        Subject = "Refund request",
                        Message = "I would like a refund for my recent purchase.",
                        DateOfCreated = new DateTime(2025, 11, 3),
                        IsAnswered = false
                    },
                    new ClientMessage
                    {
                        UserName = "Sophia Anderson",
                        UserEmail = "sophia.anderson@example.com",
                        Subject = "Shipping delay",
                        Message = "My order hasn’t arrived yet, even though it’s been a week.",
                        DateOfCreated = new DateTime(2025, 11, 4),
                        IsAnswered = false
                    },
                    new ClientMessage
                    {
                        UserName = "Daniel Martinez",
                        UserEmail = "daniel.martinez@example.com",
                        Subject = "Account deletion",
                        Message = "Please delete my account and confirm once done.",
                        DateOfCreated = new DateTime(2025, 11, 5),
                        IsAnswered = false
                    },
                    new ClientMessage
                    {
                        UserName = "Olivia Taylor",
                        UserEmail = "olivia.taylor@example.com",
                        Subject = "Partnership inquiry",
                        Message = "I’m interested in a potential collaboration. Whom should I contact?",
                        DateOfCreated = new DateTime(2025, 11, 6),
                        IsAnswered = false
                    }
                );
            }
        }

        private static void SeedTags(AgencyDBContext context)
        {
            if (!context.Tags.Any())
            {
                var tags = new List<Tag>
        {
            new Tag { Name = "Європа", Slug = "yevropa" },
            new Tag { Name = "Азія", Slug = "aziya" },
            new Tag { Name = "Україна", Slug = "ukraina" },
            new Tag { Name = "Міські подорожі", Slug = "miski-podorozhi" },
            new Tag { Name = "Пляжний відпочинок", Slug = "plyazhnyy-vidpochynok" },
            new Tag { Name = "Гірські походи", Slug = "girski-pohody" },
            new Tag { Name = "Сімейні тури", Slug = "simeyni-tury" },
            new Tag { Name = "Бюджетні подорожі", Slug = "byudzhetni-podorozhi" },
            new Tag { Name = "Екскурсійні тури", Slug = "ekskursiyni-tury" },
            new Tag { Name = "Авторські тури", Slug = "avtorski-tury" },
            new Tag { Name = "Лайфхаки для мандрівників", Slug = "travel-lifhacks" },
            new Tag { Name = "Кулінарний туризм", Slug = "kulynarnyi-turyzm" },
            new Tag { Name = "Романтичні подорожі", Slug = "romantychni-podorozhi" },
            new Tag { Name = "Нічне життя", Slug = "nichne-zhyttya" },
            new Tag { Name = "Новини туризму", Slug = "novyny-turyzmu" }
        };

                context.Tags.AddRange(tags);
                context.SaveChanges();
            }
        }

        private static void SeedCategories(AgencyDBContext context)
        {
            if (!context.Categories.Any())
            {
                context.Database.OpenConnection();

                try
                {
                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Categories ON");

                    context.Categories.AddRange(
                        // ====== Основні категорії ======
                        new Category
                        {
                            Id = 1,
                            Name = "Європа",
                            Slug = "yevropa",
                            Description = "Найкращі міста та курорти Європи для відпочинку в будь-яку пору року.",
                            ImgSrc = "/images/categories/europe.jpg",
                            ParentID = null
                        },
                        new Category
                        {
                            Id = 2,
                            Name = "Азія",
                            Slug = "aziya",
                            Description = "Екзотичні напрямки, храми, пляжі та мегаполіси Азії.",
                            ImgSrc = "/images/categories/asia.jpg",
                            ParentID = null
                        },
                        new Category
                        {
                            Id = 3,
                            Name = "Подорожі Україною",
                            Slug = "podorozhi-ukrainoyu",
                            Description = "Натхненні маршрути Україною: від Карпат до моря.",
                            ImgSrc = "/images/categories/ukraine.jpg",
                            ParentID = null
                        },
                        new Category
                        {
                            Id = 4,
                            Name = "Гірський відпочинок",
                            Slug = "girski-tury",
                            Description = "Походи в гори, лижні курорти, панорамні види.",
                            ImgSrc = "/images/categories/mountains.jpg",
                            ParentID = null
                        },
                        new Category
                        {
                            Id = 5,
                            Name = "Міські тури",
                            Slug = "city-breaks",
                            Description = "Короткі подорожі у великі міста світу.",
                            ImgSrc = "/images/categories/city.jpg",
                            ParentID = null
                        },

                        // ====== Підкатегорії для "Подорожі Україною" ======
                        new Category
                        {
                            Id = 6,
                            Name = "Карпати",
                            Slug = "karpaty",
                            Description = "Гірські маршрути, SPA-готелі та автентичні села Карпат.",
                            ImgSrc = "/images/categories/karpaty.jpg",
                            ParentID = 3
                        },
                        new Category
                        {
                            Id = 7,
                            Name = "Чорне море",
                            Slug = "chorne-more",
                            Description = "Пляжний відпочинок на узбережжі Чорного моря.",
                            ImgSrc = "/images/categories/black-sea.jpg",
                            ParentID = 3
                        },

                        // ====== Підкатегорії для "Європа" ======
                        new Category
                        {
                            Id = 8,
                            Name = "Скандинавія",
                            Slug = "skandinaviya",
                            Description = "Північні країни, фіорди та полярне сяйво.",
                            ImgSrc = "/images/categories/scandinavia.jpg",
                            ParentID = 1
                        },
                        new Category
                        {
                            Id = 9,
                            Name = "Середземноморські курорти",
                            Slug = "sredzemnomorski-kurorty",
                            Description = "Тепле море, сонце та затишні прибережні міста.",
                            ImgSrc = "/images/categories/mediterranean.jpg",
                            ParentID = 1
                        },

                        // ====== Підкатегорії для "Азія" ======
                        new Category
                        {
                            Id = 10,
                            Name = "Південно-Східна Азія",
                            Slug = "pivdenno-shidna-aziya",
                            Description = "Таїланд, Вʼєтнам, Індонезія та інші тропічні напрямки.",
                            ImgSrc = "/images/categories/sea-asia.jpg",
                            ParentID = 2
                        },
                        new Category
                        {
                            Id = 11,
                            Name = "Острівні напрямки",
                            Slug = "ostrovi-napryamky",
                            Description = "Балі, Мальдіви, Філіппіни та інші райські острови.",
                            ImgSrc = "/images/categories/islands.jpg",
                            ParentID = 2
                        }
                    );

                    context.SaveChanges();

                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Categories OFF");
                }
                finally
                {
                    context.Database.CloseConnection();
                }
            }
        }

        private static void SeedPosts(AgencyDBContext context)
        {
            if (!context.Posts.Any())
            {
                context.Database.OpenConnection();

                try
                {
                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Posts ON");

                    context.Posts.AddRange(
                        new Post
                        {
                            Id = 1,
                            Name = "Вікенд у Парижі: як побачити головне за 3 дні",
                            Slug = "vikend-u-paryzhi-za-3-dni",
                            ShortDescription = "Маршрут по Парижу для тих, хто приїхав уперше: Ейфелева вежа, Лувр, Монмартр та не тільки.",
                            ImgSrc = "/images/posts/paris-weekend.jpg",
                            Context = "Париж — місто, куди хочеться повертатися знову і знову. У цій статті ми зібрали оптимальний маршрут на три дні: як поєднати must-see локації з атмосферними вуличками і кавʼярнями без зайвого поспіху...",
                            PostStatuses = PostStatuses.Published,
                            DateOfCreated = DateTime.Now.AddDays(-30),
                            DateOfUpdated = DateTime.Now.AddDays(-25),
                            DateOfPublished = DateTime.Now.AddDays(-25)
                        },
                        new Post
                        {
                            Id = 2,
                            Name = "Топ-7 місць у Карпатах для осінньої подорожі",
                            Slug = "top-7-misc-u-karpatah-voseni",
                            ShortDescription = "Полонини, водоспади та найкращі оглядові точки Карпат восени.",
                            ImgSrc = "/images/posts/autumn-karpaty.jpg",
                            Context = "Осінь у Карпатах — це поєднання туману, сонця та яскравих фарб. Ми зібрали для вас найцікавіші локації: від популярних маршрутів у районі Яремче до більш тихих місць біля Писаної скелі... ",
                            PostStatuses = PostStatuses.Published,
                            DateOfCreated = DateTime.Now.AddDays(-28),
                            DateOfUpdated = DateTime.Now.AddDays(-20),
                            DateOfPublished = DateTime.Now.AddDays(-20)
                        },
                        new Post
                        {
                            Id = 3,
                            Name = "Як спланувати бюджетну подорож до Рима",
                            Slug = "byudzhetna-podorozh-do-ryma",
                            ShortDescription = "Практичні поради, як зекономити на перельотах, житлі та харчуванні.",
                            ImgSrc = "/images/posts/rome-budget.jpg",
                            Context = "Рим — не обовʼязково дорогий напрямок. Якщо заздалегідь продумати дати, локацію житла та формат харчування, можна суттєво скоротити витрати. У статті — реальні приклади бюджетів та лайфхаки...",
                            PostStatuses = PostStatuses.Published,
                            DateOfCreated = DateTime.Now.AddDays(-27),
                            DateOfUpdated = DateTime.Now.AddDays(-18),
                            DateOfPublished = DateTime.Now.AddDays(-18)
                        },
                        new Post
                        {
                            Id = 4,
                            Name = "Мальдіви чи Балі: який острів обрати для відпочинку",
                            Slug = "maldivy-chy-bali",
                            ShortDescription = "Порівняння двох популярних острівних напрямків: погода, ціни, враження.",
                            ImgSrc = "/images/posts/maldives-vs-bali.jpg",
                            Context = "Обидва напрямки вважаються «райськими», але формат відпочинку на них різний. На Мальдівах — більше про релакс і all inclusive, на Балі — про атмосферу, серфінг і активності. Розбираємось детальніше...",
                            PostStatuses = PostStatuses.Published,
                            DateOfCreated = DateTime.Now.AddDays(-25),
                            DateOfUpdated = DateTime.Now.AddDays(-15),
                            DateOfPublished = DateTime.Now.AddDays(-15)
                        },
                        new Post
                        {
                            Id = 5,
                            Name = "Гід по Стамбулу: міст між Європою та Азією",
                            Slug = "gid-po-stambulu",
                            ShortDescription = "Що подивитися у Стамбулі за 2–4 дні та в яких районах зупинятися.",
                            ImgSrc = "/images/posts/istanbul-guide.jpg",
                            Context = "Стамбул вражає контрастами: старе місто, галасливі базари, сучасні райони з кавʼярнями та rooftop-барами. Ми підготували гайд по ключових районах, памʼятках та корисних дрібницях для мандрівників...",
                            PostStatuses = PostStatuses.Published,
                            DateOfCreated = DateTime.Now.AddDays(-23),
                            DateOfUpdated = DateTime.Now.AddDays(-10),
                            DateOfPublished = DateTime.Now.AddDays(-10)
                        },
                        new Post
                        {
                            Id = 6,
                            Name = "Чорне море взимку: навіщо їхати в Одесу поза сезоном",
                            Slug = "odesa-vzymku",
                            ShortDescription = "Спокійне море, низькі ціни та особлива атмосфера зимового міста.",
                            ImgSrc = "/images/posts/odesa-winter.jpg",
                            Context = "Зимова Одеса — зовсім інша, ніж літня. Менше туристів, більше локального життя, при цьому море нікуди не зникає. У статті — ідеї, що робити в місті взимку, та як поєднати подорож із роботою...",
                            PostStatuses = PostStatuses.Published,
                            DateOfCreated = DateTime.Now.AddDays(-21),
                            DateOfUpdated = DateTime.Now.AddDays(-9),
                            DateOfPublished = DateTime.Now.AddDays(-9)
                        },
                        new Post
                        {
                            Id = 7,
                            Name = "10 причин поїхати у Скандинавію",
                            Slug = "10-prychyn-u-skandynaviyu",
                            ShortDescription = "Фіорди, сауни, хюґе та найчистіші міста Європи.",
                            ImgSrc = "/images/posts/scandinavia-reasons.jpg",
                            Context = "Норвегія, Швеція, Данія та Фінляндія — регіон, який закохує в себе спокоєм та продуманістю. Розповідаємо, чому Скандинавія варта того, щоб потрапити у ваш список бажань...",
                            PostStatuses = PostStatuses.Published,
                            DateOfCreated = DateTime.Now.AddDays(-19),
                            DateOfUpdated = DateTime.Now.AddDays(-8),
                            DateOfPublished = DateTime.Now.AddDays(-8)
                        },
                        new Post
                        {
                            Id = 8,
                            Name = "Як подорожувати з дітьми і не зійти з розуму",
                            Slug = "podorozhi-z-ditmy",
                            ShortDescription = "Поради для сімейних подорожей літаком, поїздом та авто.",
                            ImgSrc = "/images/posts/family-travel.jpg",
                            Context = "Подорожі з дітьми можуть бути не стресом, а пригодою для всієї родини. Важливо правильно обрати напрямок, час перельоту, формат житла та взяти з собою «рятувальний набір» для малечі...",
                            PostStatuses = PostStatuses.Published,
                            DateOfCreated = DateTime.Now.AddDays(-17),
                            DateOfUpdated = DateTime.Now.AddDays(-7),
                            DateOfPublished = DateTime.Now.AddDays(-7)
                        },
                        new Post
                        {
                            Id = 9,
                            Name = "Романтичний вікенд у Львові",
                            Slug = "romantychnyi-vikend-u-lvovi",
                            ShortDescription = "Маршрут старим містом, кавʼярні, оглядові майданчики та атмосфера.",
                            ImgSrc = "/images/posts/lviv-weekend.jpg",
                            Context = "Львів — ідеальне місто для короткої романтичної втечі. Тут все про атмосферу: бруківка, світло ліхтарів, запах кави та шоколаду. Пропонуємо готовий маршрут на два дні...",
                            PostStatuses = PostStatuses.Published,
                            DateOfCreated = DateTime.Now.AddDays(-15),
                            DateOfUpdated = DateTime.Now.AddDays(-6),
                            DateOfPublished = DateTime.Now.AddDays(-6)
                        },
                        new Post
                        {
                            Id = 10,
                            Name = "Що взяти з собою у гірський похід",
                            Slug = "shcho-vzyaty-u-girsky-pohid",
                            ShortDescription = "Чекліст спорядження для Карпат та інших гірських маршрутів.",
                            ImgSrc = "/images/posts/hiking-checklist.jpg",
                            Context = "Правильно зібраний рюкзак — це безпека і комфорт у поході. У цьому матеріалі — список базового спорядження, речей «на випадок...» та кілька порад, як не перевантажити рюкзак...",
                            PostStatuses = PostStatuses.Published,
                            DateOfCreated = DateTime.Now.AddDays(-14),
                            DateOfUpdated = DateTime.Now.AddDays(-5),
                            DateOfPublished = DateTime.Now.AddDays(-5)
                        },
                        new Post
                        {
                            Id = 11,
                            Name = "Як знайти дешеві авіаквитки: детальний гід",
                            Slug = "deshevi-aviakvytky-gid",
                            ShortDescription = "Пошуковики, дати, гнучкість і трішки терпіння.",
                            ImgSrc = "/images/posts/cheap-flights.jpg",
                            Context = "Економія на квитках часто дозволяє подорожувати частіше. Розповідаємо, як працювати з гнучкими датами, якими сервісами користуватися та коли краще купувати авіаквитки...",
                            PostStatuses = PostStatuses.Published,
                            DateOfCreated = DateTime.Now.AddDays(-12),
                            DateOfUpdated = DateTime.Now.AddDays(-4),
                            DateOfPublished = DateTime.Now.AddDays(-4)
                        },
                        new Post
                        {
                            Id = 12,
                            Name = "Нічне життя Бангкока: що варто побачити",
                            Slug = "nichne-zhyttya-bangkoka",
                            ShortDescription = "Руфто-пи, нічні ринки та бари з видом на мегаполіс.",
                            ImgSrc = "/images/posts/bangkok-nightlife.jpg",
                            Context = "Бангкок ніколи не спить: вночі тут кипить своє життя. У цьому гіді ми зібрали rooftop-бари, нічні ринки, вулиці з атмосферою та важливі правила безпеки...",
                            PostStatuses = PostStatuses.Published,
                            DateOfCreated = DateTime.Now.AddDays(-10),
                            DateOfUpdated = DateTime.Now.AddDays(-2),
                            DateOfPublished = DateTime.Now.AddDays(-2)
                        }
                    );

                    context.SaveChanges();

                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Posts OFF");
                }
                finally
                {
                    context.Database.CloseConnection();
                }
            }
        }

        private static void SeedPostTags(AgencyDBContext context)
        {
            if (!context.PostTags.Any())
            {
                context.Database.OpenConnection();

                try
                {
                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT PostTags ON");

                    context.PostTags.AddRange(
                        // Пост 1 — Париж
                        new PostTags { Id = 1, PostId = 1, TagId = 1 },  // Європа
                        new PostTags { Id = 2, PostId = 1, TagId = 4 },  // Міські подорожі
                        new PostTags { Id = 3, PostId = 1, TagId = 13 }, // Романтичні подорожі

                        // Пост 2 — Карпати
                        new PostTags { Id = 4, PostId = 2, TagId = 3 },  // Україна
                        new PostTags { Id = 5, PostId = 2, TagId = 6 },  // Гірські походи
                        new PostTags { Id = 6, PostId = 2, TagId = 8 },  // Бюджетні подорожі

                        // Пост 3 — Рим бюджетно
                        new PostTags { Id = 7, PostId = 3, TagId = 1 },  // Європа
                        new PostTags { Id = 8, PostId = 3, TagId = 8 },  // Бюджетні подорожі
                        new PostTags { Id = 9, PostId = 3, TagId = 11 }, // Лайфхаки

                        // Пост 4 — Мальдіви чи Балі
                        new PostTags { Id = 10, PostId = 4, TagId = 2 },  // Азія
                        new PostTags { Id = 11, PostId = 4, TagId = 5 },  // Пляжний відпочинок
                        new PostTags { Id = 12, PostId = 4, TagId = 10 }, // Авторські тури

                        // Пост 5 — Стамбул
                        new PostTags { Id = 13, PostId = 5, TagId = 2 },  // Азія/Європа
                        new PostTags { Id = 14, PostId = 5, TagId = 4 },  // Міські подорожі
                        new PostTags { Id = 15, PostId = 5, TagId = 9 },  // Екскурсійні тури

                        // Пост 6 — Одеса взимку
                        new PostTags { Id = 16, PostId = 6, TagId = 3 },  // Україна
                        new PostTags { Id = 17, PostId = 6, TagId = 5 },  // Пляжний відпочинок
                        new PostTags { Id = 18, PostId = 6, TagId = 8 },  // Бюджетні подорожі

                        // Пост 7 — Скандинавія
                        new PostTags { Id = 19, PostId = 7, TagId = 1 },  // Європа
                        new PostTags { Id = 20, PostId = 7, TagId = 6 },  // Гірські походи
                        new PostTags { Id = 21, PostId = 7, TagId = 15 }, // Новини туризму

                        // Пост 8 — Подорожі з дітьми
                        new PostTags { Id = 22, PostId = 8, TagId = 7 },  // Сімейні тури
                        new PostTags { Id = 23, PostId = 8, TagId = 11 }, // Лайфхаки
                        new PostTags { Id = 24, PostId = 8, TagId = 8 },  // Бюджетні

                        // Пост 9 — Львів
                        new PostTags { Id = 25, PostId = 9, TagId = 3 },  // Україна
                        new PostTags { Id = 26, PostId = 9, TagId = 4 },  // Міські подорожі
                        new PostTags { Id = 27, PostId = 9, TagId = 13 }, // Романтичні подорожі

                        // Пост 10 — Гірський похід
                        new PostTags { Id = 28, PostId = 10, TagId = 6 },  // Гірські походи
                        new PostTags { Id = 29, PostId = 10, TagId = 11 }, // Лайфхаки
                        new PostTags { Id = 30, PostId = 10, TagId = 8 },  // Бюджетні

                        // Пост 11 — Дешеві авіаквитки
                        new PostTags { Id = 31, PostId = 11, TagId = 8 },  // Бюджетні подорожі
                        new PostTags { Id = 32, PostId = 11, TagId = 11 }, // Лайфхаки
                        new PostTags { Id = 33, PostId = 11, TagId = 15 }, // Новини туризму

                        // Пост 12 — Нічне життя Бангкока
                        new PostTags { Id = 34, PostId = 12, TagId = 2 },  // Азія
                        new PostTags { Id = 35, PostId = 12, TagId = 14 }, // Нічне життя
                        new PostTags { Id = 36, PostId = 12, TagId = 4 }   // Міські подорожі
                    );

                    context.SaveChanges();

                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT PostTags OFF");
                }
                finally
                {
                    context.Database.CloseConnection();
                }
            }
        }

        private static void SeedPostCategories(AgencyDBContext context)
        {
            if (!context.PostCategories.Any())
            {
                context.Database.OpenConnection();

                try
                {
                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT PostCategories ON");

                    context.PostCategories.AddRange(
                        new PostCategories { Id = 1, PostId = 1, CategoryId = 5 },  // Міські тури
                        new PostCategories { Id = 2, PostId = 2, CategoryId = 6 },  // Карпати
                        new PostCategories { Id = 3, PostId = 3, CategoryId = 1 },  // Європа
                        new PostCategories { Id = 4, PostId = 4, CategoryId = 11 }, // Острівні напрямки
                        new PostCategories { Id = 5, PostId = 5, CategoryId = 2 },  // Азія
                        new PostCategories { Id = 6, PostId = 6, CategoryId = 7 },  // Чорне море
                        new PostCategories { Id = 7, PostId = 7, CategoryId = 8 },  // Скандинавія
                        new PostCategories { Id = 8, PostId = 8, CategoryId = 3 },  // Подорожі Україною (загальне)
                        new PostCategories { Id = 9, PostId = 9, CategoryId = 3 },  // Подорожі Україною
                        new PostCategories { Id = 10, PostId = 10, CategoryId = 4 },// Гірський відпочинок
                        new PostCategories { Id = 11, PostId = 11, CategoryId = 5 },// Міські тури
                        new PostCategories { Id = 12, PostId = 12, CategoryId = 10 }// Південно-Східна Азія
                    );

                    context.SaveChanges();

                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT PostCategories OFF");
                }
                finally
                {
                    context.Database.CloseConnection();
                }
            }
        }

        private static void SeedComments(AgencyDBContext context)
        {
            if (!context.Comments.Any())
            {
                context.Database.OpenConnection();

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Comments ON");

                var comments = new List<Comment>
                {
                    // ===== Кореневі коментарі =====
                    new Comment {
                        Id = 1,
                        UserLogin = "andriy",
                        UserEmail = "andriy@example.com",
                        UserAvatar = "/images/avatars/avatar.jpg",
                        Text = "Супер-маршрут, якраз плануємо поїздку",
                        DateOfCreated = DateTime.Now.AddMinutes(-200),
                        PostId = 12,
                        ParentCommentId = null,
                        IsRequired = true
                    },
                    new Comment {
                        Id = 4,
                        UserLogin = "olena",
                        UserEmail = "olena@example.com",
                        UserAvatar = "/images/avatars/avatar1.jpg",
                        Text = "Було б класно побачити окремо гайд по кавʼярнях :)",
                        DateOfCreated = DateTime.Now.AddMinutes(-150),
                        PostId = 12,
                        ParentCommentId = null,
                        IsRequired = true
                    },
                    new Comment {
                        Id = 6,
                        UserLogin = "igor",
                        UserEmail = "igor@example.com",
                        UserAvatar = "/images/avatars/avatar3.jpg",
                        Text = "Дякую за статтю, дуже корисні поради!",
                        DateOfCreated = DateTime.Now.AddMinutes(-120),
                        PostId = 12,
                        ParentCommentId = null,
                        IsRequired = true
                    },
                    new Comment {
                        Id = 7,
                        UserLogin = "maria",
                        UserEmail = "maria@example.com",
                        UserAvatar = "/images/avatars/avatar4.jpg",
                        Text = "Чи підійде цей маршрут, якщо подорожуємо з дитиною?",
                        DateOfCreated = DateTime.Now.AddMinutes(-100),
                        PostId = 12,
                        ParentCommentId = null,
                        IsRequired = true
                    },

                    // ===== Відповіді (1 рівень вкладеності) =====
                    new Comment {
                        Id = 2,
                        UserLogin = "admin",
                        UserEmail = "info@mriya-tour.com",
                        UserAvatar = "/images/avatars/avatar2.jpg",
                        Text = "Раді, що було корисно! Якщо потрібна допомога з туром — пишіть нам у форму контактів.",
                        DateOfCreated = DateTime.Now.AddMinutes(-180),
                        PostId = 12,
                        ParentCommentId = 1,
                        IsRequired = true
                    },
                    new Comment {
                        Id = 3,
                        UserLogin = "yana",
                        UserEmail = "yana@example.com",
                        UserAvatar = "/images/avatars/avatar5.jpg",
                        Text = "Підтримую, маршрут дуже зручний, ми пройшли його майже повністю!",
                        DateOfCreated = DateTime.Now.AddMinutes(-170),
                        PostId = 12,
                        ParentCommentId = 1,
                        IsRequired = true
                    },
                    new Comment {
                        Id = 5,
                        UserLogin = "serhii",
                        UserEmail = "serhii@example.com",
                        UserAvatar = "/images/avatars/avatar7.jpg",
                        Text = "Так, теж чекаю окрему статтю про гастрономію:)",
                        DateOfCreated = DateTime.Now.AddMinutes(-140),
                        PostId = 12,
                        ParentCommentId = 4,
                        IsRequired = true
                    },
                    new Comment {
                        Id = 8,
                        UserLogin = "natalia",
                        UserEmail = "natalia@example.com",
                        UserAvatar = "/images/avatars/avatar8.jpg",
                        Text = "Мені теж цікаво, як адаптувати маршрут під подорож з дитиною.",
                        DateOfCreated = DateTime.Now.AddMinutes(-90),
                        PostId = 12,
                        ParentCommentId = 7,
                        IsRequired = true
                    },

                    // ===== Вкладеність 2 рівня =====
                    new Comment {
                        Id = 9,
                        UserLogin = "admin",
                        UserEmail = "info@mriya-tour.com",
                        UserAvatar = "/images/avatars/avatar2.jpg",
                        Text = "Можна скоротити кількість локацій на день і додати більше перерв у парках — ми якраз готуємо окремий матеріал про це :)",
                        DateOfCreated = DateTime.Now.AddMinutes(-70),
                        PostId = 12,
                        ParentCommentId = 8,
                        IsRequired = true
                    }
                };

                context.Comments.AddRange(comments);

                context.SaveChanges();

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Comments OFF");

                context.Database.CloseConnection();
            }
        }

        private static void SeedUsers(AgencyDBContext context)
        {
            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new User()
                    {
                        Email = "admin@admin.com",
                        PasswordHash = "$MYHASH$V1$10000$BZEG8cCbKd2UzB//x2wbns80+9KuNBnUihqOaDCn2b/caR1+",
                        Login = "Admin Erastovich"
                    }
                );
                context.SaveChanges();
            }
        }

    }
}
