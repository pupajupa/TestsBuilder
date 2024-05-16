using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestsBuilder.Models;

namespace TestsBuilder.Services
{
    public interface IDbService
    {
        void Init();
        IEnumerable<User> GetAllUsers();
        IEnumerable<Test> GetAllTests();
        IEnumerable<Exp> GetTestExpressions(int id);
        IEnumerable<Variant> GetExpressionVariants(int id);
        User GetUserByUsername(string username);
        void UpdateUser(User updatedUser);
        User CheckUser(string username, string password);
        void AddUser(User user);
        void AddTest(Test test);
        void AddExpressionsToTest(string testName, List<Exp> expressions);
        void AddVariantsToExpression(int expressionId, List<Variant> variants);
        void AddExpressionToTest(string testName, Exp expression);
        void UpdateExpression(Exp updatedExp);
        Test GetTestById(int testId);
        void UpdateVariant(Variant updatedVariant);
        Exp GetExpressionById(int ExpId);
    }
}
