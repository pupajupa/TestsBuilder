using SQLite;
using SQLiteNetExtensions.Extensions;
using System.Diagnostics;
using System.Threading.Tasks;
using TestsBuilder.Interfaces;
using TestsBuilder.Models;

namespace TestsBuilder.Services
{
    public class DbService : IDbService
    {
        private const string _dbName = "TestsBuilderDb9.db3";
        public const SQLiteOpenFlags Flags =
            SQLite.SQLiteOpenFlags.ReadWrite |
            SQLiteOpenFlags.Create |
            SQLiteOpenFlags.SharedCache;
        private const string Name = "TestsBuilder.Resources.Images.default_profile_image.png";

        private static string _dbPath => Path.Combine(FileSystem.AppDataDirectory, _dbName);
        SQLiteConnection Database;
        public Student CurrentStudent { get; private set; }
        public Teacher CurrentTeacher { get; private set; }
        public Test CurrentTest { get; private set; }
        public Example CurrentExample { get; private set; }
        public ExampleVariant CurrentExampleVariant { get; private set; }
        public DbService()
        {
            Init();
            AddInitialTeacher();
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
                Database.CreateTable<Material>();
                Database.CreateTable<Student>();
                Database.CreateTable<Teacher>();
                Database.CreateTable<Answer>();
                Database.CreateTable<ExampleVariant>();
                Database.CreateTable<BaseAnswer>();
                Database.CreateTable<Example>();
                Database.CreateTable<Test>();
                Database.CreateTable<TestResult>();
            }
        }

        // CRUD operations for Student
        public IEnumerable<Student> GetAllStudents()
        {
            Init();
            return Database.Table<Student>().ToList();
        }

        public Student GetStudentById(int id)
        {
            Init();
            return Database.Table<Student>().FirstOrDefault(s => s.Id == id);
        }

        public void AddStudent(Student student)
        {
            Init();
            Database.Insert(student);
        }

        public void UpdateStudent(Student student)
        {
            Init();
            Database.Update(student);
        }

        public void DeleteStudent(int id)
        {
            Init();
            var student = GetStudentById(id);
            if (student != null)
            {
                Database.Delete(student);
            }
        }

        // CRUD operations for Teacher
        public IEnumerable<Teacher> GetAllTeachers()
        {
            Init();
            return Database.Table<Teacher>().ToList();
        }

        public Teacher GetTeacherById(int id)
        {
            Init();
            return Database.Table<Teacher>().FirstOrDefault(t => t.Id == id);
        }

        public Teacher GetTeacherByLogin(string login)
        {
            Init();
            return Database.Table<Teacher>().FirstOrDefault(t => t.Login == login);
        }

        public Student GetStudentByLogin(string login)
        {
            Init();
            return Database.Table<Student>().FirstOrDefault(t => t.Login == login);
        }
        public void AddTeacher(Teacher teacher)
        {
            Init();
            Database.Insert(teacher);
        }

        public void UpdateTeacher(Teacher teacher)
        {
            Init();
            Database.Update(teacher);
        }

        public void DeleteTeacher(int id)
        {
            Init();
            var teacher = GetTeacherById(id);
            if (teacher != null)
            {
                Database.Delete(teacher);
            }
        }

        // CRUD operations for Test
        public IEnumerable<Test> GetAllTests()
        {
            Init();
            return Database.Table<Test>().ToList();
        }

        public Test GetTestById(int id)
        {
            Init();
            return Database.Table<Test>().FirstOrDefault(t => t.Id == id);
        }

        public void AddTest(Test test)
        {
            Init();
            Database.Insert(test);
        }

        public void UpdateTest(Test test)
        {
            Init();
            Database.Update(test);
        }

        public void DeleteTest(int id)
        {
            Init();
            var test = GetTestById(id);
            if (test != null)
            {
                Database.Delete(test);
            }
        }

        // CRUD operations for Example
        public IEnumerable<Example> GetAllExamples()
        {
            Init();
            return Database.Table<Example>().ToList();
        }

        public Example GetExampleById(int id)
        {
            Init();
            return Database.Table<Example>().FirstOrDefault(e => e.Id == id);
        }

        public void AddExample(Example example)
        {
            Init();
            Database.Insert(example);
        }

        public void UpdateExample(Example example)
        {
            Init();
            Database.Update(example);
        }

        public void DeleteExample(int id)
        {
            Init();
            var example = GetExampleById(id);
            if (example != null)
            {
                Database.Delete(example);
            }
        }

        // CRUD operations for ExampleVariant
        public IEnumerable<ExampleVariant> GetAllExampleVariants()
        {
            Init();
            return Database.Table<ExampleVariant>().ToList();
        }

        public ExampleVariant GetExampleVariantById(int id)
        {
            Init();
            return Database.Table<ExampleVariant>().FirstOrDefault(ev => ev.Id == id);
        }

        public void AddExampleVariant(ExampleVariant exampleVariant)
        {
            Init();
            Database.Insert(exampleVariant);
        }

        public void UpdateExampleVariant(ExampleVariant exampleVariant)
        {
            Init();
            Database.Update(exampleVariant);
        }

        public void DeleteExampleVariant(int id)
        {
            Init();
            var exampleVariant = GetExampleVariantById(id);
            if (exampleVariant != null)
            {
                Database.Delete(exampleVariant);
            }
        }

        public IEnumerable<Example> GetAllExpressionsByTestId(int testId)
        {
            Init();

            var examples = Database.Table<Example>().Where(e => e.TestId == testId).ToList();

            foreach(var ex in examples)
            {
                Database.GetChildren(ex);
            }

            return examples;
        }
        public IEnumerable<ExampleVariant> GetAllExpressionVariantsByExampleId(int exampleId)
        {
            Init();
            var exampleVariants = Database.Table<ExampleVariant>()
                                           .Where(e => e.ExampleId == exampleId)
                                           .ToList();

            // Загрузка связанных ответов для каждого варианта
            foreach (var variant in exampleVariants)
            {
                Database.GetChildren(variant);
            }

            return exampleVariants;
        }
        public List<TestResult> GetAllResultsByTestId(int testId)
        {
            // Используем LINQ для выполнения запроса
            var results = Database.Table<TestResult>().Where(tr => tr.TestId == testId).ToList();
            return results;
        }
        public TestResult GetTestResultByTestId(int testId)
        {
            Init();
            return Database.Table<TestResult>().FirstOrDefault(ev => ev.TestId == testId);
        }
        // New method to set (remember) the current student
        public void SetCurrentStudent(string username)
        {
            Init();
            CurrentStudent = Database.Table<Student>().FirstOrDefault(u => u.Login == username);
        }

        // New method to set (remember) the current teacher
        public void SetCurrentTeacher(string username)
        {
            Init();
            CurrentTeacher = Database.Table<Teacher>().FirstOrDefault(u => u.Login == username);
        }
        public void SetCurrentTest(int testId)
        {
            Init();
            CurrentTest = Database.Table<Test>().FirstOrDefault(u => u.Id == testId);
        }

        public Test GetCurrentTest()
        {
            return CurrentTest;
        }

        public Example GetCurrentExample()
        {
            return CurrentExample;
        }

        public Student GetCurrentStudent()
        {
            return CurrentStudent;
        }

        public Teacher GetCurrentTeacher()
        {
            return CurrentTeacher;
        }

        public ExampleVariant GetCurrentExampleVariant()
        {
            return CurrentExampleVariant;
        }

        public void SetCurrentExample(int exId)
        {
            Init();
            CurrentExample = Database.Table<Example>().FirstOrDefault(u => u.Id == exId);
            CurrentExample.BaseAnswers = Database.Table<BaseAnswer>().Where(b=>b.ExampleId == exId).ToList();
        }

        public void SetCurrentExampleVariant(int exvarId)
        {
            Init();
            CurrentExampleVariant = Database.Table<ExampleVariant>().FirstOrDefault(u => u.Id == exvarId);
            CurrentExampleVariant.Answers = Database.Table<Answer>().Where(a => a.ExampleVariantId == exvarId).ToList();
        }

        public void ClearCurrentTest()
        {
            CurrentExample = null;
            CurrentTest = null;
            CurrentExampleVariant = null;

        }
        // New method to clear the current user (logout)
        public void ClearCurrentUser()
        {
            CurrentStudent = null;
            CurrentTeacher = null;
        }
        public void AddInitialTeacher()
        {
            if (!Database.Table<Teacher>().Any())
            {
                // Извлекаем изображение из ресурсов
                var assembly = typeof(DbService).Assembly;
                try
                {
                    using Stream imageStream = assembly.GetManifestResourceStream(Name);
                    byte[] imageBytes = null;
                    if (imageStream != null)
                    {
                        using MemoryStream memoryStream = new();
                        imageStream.CopyTo(memoryStream);
                        imageBytes = memoryStream.ToArray();
                    }
                    // Создайте учителя и добавьте его в базу данных
                    var initialTeacher = new Teacher
                    {
                        FirstName = "admin",
                        LastName = "admin",
                        Login = "admin",
                        Password = "admin", // Желательно хранить пароли в зашифрованном виде
                        Image = imageBytes // Если есть изображение профиля, установите здесь
                    };
                    Database.Insert(initialTeacher);
                }
                catch(Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
                // Преобразуем изображение в массив байтов
            }
        }
        public void AddAnswerToVariant(int exampleVariantId, string answerText)
        {
            var variant = Database.Find<ExampleVariant>(exampleVariantId);
            if (variant != null)
            {
                var answer = new Answer
                {
                    Text = answerText,
                    ExampleVariantId = exampleVariantId
                };
                Database.Insert(answer);
                variant.Answers.Add(answer);
            }
        }

        public void AddBaseAnswerToExample(int exampleId, string answerText)
        {
            var example = Database.Find<Example>(exampleId);
            if (example != null)
            {
                var baseAnswer = new BaseAnswer
                {
                    Text = answerText,
                    ExampleId = exampleId
                };
                Database.Insert(baseAnswer);

                example.BaseAnswers.Add(baseAnswer);
            }
        }

        public void AddMaterial(Material material)
        {
            Database.Insert(material);
        }

        public List<Material> GetAllMaterials()
        {
            return Database.Table<Material>().ToList();
        }

        public void AddTestResult(TestResult result)
        {
            Init();
            Database.Insert(result);
        }
    }
}