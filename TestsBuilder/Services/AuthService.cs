using Microsoft.EntityFrameworkCore;
using Microsoft.Maui.Graphics;
using SQLite;
using TestsBuilder.Context;
using TestsBuilder.Models;

namespace TestsBuilder.Services
{
    class AuthService : IDbService
    {
        private const string _dbName = "new_dbase.db3";
        //private readonly AppDbContext _context;
        public const SQLiteOpenFlags Flags =
            // open the database in read/write mode
            SQLite.SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLiteOpenFlags.SharedCache;
        private static string _dbPath => Path.Combine(FileSystem.AppDataDirectory, _dbName);
        SQLiteConnection Database;
        public AuthService()
        {
        }

        public void Init()
        {
            if (File.Exists(_dbPath))
            {
                Database = new SQLiteConnection(_dbPath, Flags);
            }
            else
            {
                Database = new SQLiteConnection(_dbPath, Flags);
                Database.CreateTable<User>();
                Database.CreateTable<Variant>(); // Создать таблицу только для типа Variant
                Database.CreateTable<Exp>();
                Database.CreateTable<Test>();
            }
        }

        public IEnumerable<User> GetAllUsers()
        {
            Init();
            return Database.Table<User>().ToList();
        }

        public IEnumerable<Test> GetAllTests()
        {
            Init();
            return Database.Table<Test>().ToList();
        }

        public IEnumerable<Exp> GetTestExpressions(int id)
        {
            Init();
            return Database.Table<Exp>().Where(i => i.TestId == id).ToList();
        }

        public IEnumerable<Variant> GetExpressionVariants(int id)
        {
            Init();
            return Database.Table<Variant>().Where(i => i.ExpId == id).ToList();
        }

        public User GetUserByUsername(string username)
        {
            Init();
            return Database.Table<User>().FirstOrDefault(u => u.Username == username);
        }

        public Test GetTestById(int testId)
        {
            Init();
            return Database.Table<Test>().FirstOrDefault(t => t.Id == testId);
        }

        public Exp GetExpressionById(int ExpId)
        {
            Init();
            return Database.Table<Exp>().FirstOrDefault(e => e.ExpId == ExpId);
        }

        public void UpdateUser(User updatedUser)
        {
            Init();
            var existingUser = Database.Table<User>().FirstOrDefault(u => u.UserId == updatedUser.UserId);

            if (existingUser != null)
            {
                existingUser.Name = updatedUser.Name;
                existingUser.LastName = updatedUser.LastName;
                existingUser.Username = updatedUser.Username;
                existingUser.Image = updatedUser.Image;
                existingUser.Password = updatedUser.Password;

                Database.Update(existingUser);
            }
        }

        public User CheckUser(string username, string password)
        {
            Init();
            return Database.Table<User>().FirstOrDefault(u => u.Username == username && u.Password == password);
        }

        public void AddUser(User user)
        {
            Init();
            Database.Insert(user);
        }

        public void AddTest(Test test)
        {
            Init();
            Database.Insert(test);
        }

        public void AddExpressionsToTest(string testName, List<Exp> expressions)
        {
            Init();
            var test = Database.Table<Test>().FirstOrDefault(t => t.Name == testName);

            if (test == null)
            {
                Console.WriteLine($"Категория \"{testName}\" не найдена.");
                return;
            }

            foreach (var expression in expressions)
            {
                Database.Insert(expression);
            }
        }

        public void AddVariantsToExpression(int expressionId, List<Variant> variants)
        {
            Init();
            var expression =  Database.Table<Exp>().FirstOrDefault(e => e.ExpId == expressionId);

            if (expression == null)
            {
                Console.WriteLine($"Выражение с Id {expressionId} не найдено.");
                return;
            }

            foreach (var variant in variants)
            {
                variant.ExpId = expressionId;
                Database.Insert(variant);
            }
        }

        public void AddExpressionToTest(string testName, Exp expression)
        {
            Init();
            var test = Database.Table<Test>().FirstOrDefault(t => t.Name == testName);

            if (test == null)
            {
                Console.WriteLine($"Категория \"{testName}\" не найдена.");
                return;
            }

            expression.TestId = test.Id;
            Database.Insert(expression);
        }

        public void UpdateExpression(Exp updatedExp)
        {
            Init();
            var existingExp = Database.Table<Exp>().FirstOrDefault(e => e.ExpId == updatedExp.ExpId);

            if (existingExp != null)
            {
                existingExp.Text = updatedExp.Text;
                existingExp.Title = updatedExp.Title;
                existingExp.AMax = updatedExp.AMax;
                existingExp.AMin = updatedExp.AMin;
                existingExp.BMax = updatedExp.BMax;
                existingExp.BMin= updatedExp.BMin;

                Database.Update(existingExp);
            }
        }

        public void UpdateVariant(Variant updatedVariant)
        {
            Init();
            var existingVar = Database.Table<Variant>().FirstOrDefault(v => v.VariantId == updatedVariant.VariantId);

            if (existingVar != null)
            {
                existingVar.VariantNumber = updatedVariant.VariantNumber;
                existingVar.VariantExpression = updatedVariant.VariantExpression;
                existingVar.VariantAnswer = updatedVariant.VariantAnswer;

                Database.Update(existingVar);
            }
        }
    }
}
