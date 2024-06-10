using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestsBuilder.Models;

namespace TestsBuilder.Interfaces
{
    public interface  IDbService
    {
        void Init();
        IEnumerable<Student> GetAllStudents();
        Student GetStudentById(int id);
        void AddStudent(Student student);
        void UpdateStudent(Student student);
        void DeleteStudent(int id);
        IEnumerable<Teacher> GetAllTeachers();
        Teacher GetTeacherById(int id);
        void AddTeacher(Teacher teacher);
        void UpdateTeacher(Teacher teacher);
        void DeleteTeacher(int id);
        Teacher GetTeacherByLogin(string login);
        Student GetStudentByLogin(string login);
        IEnumerable<Test> GetAllTests();
        Test GetTestById(int id);
        void AddTest(Test test);
        void UpdateTest(Test test);
        void DeleteTest(int id);
        IEnumerable<Example> GetAllExamples();
        Example GetExampleById(int id);
        void AddExample(Example example);
        void UpdateExample(Example example);
        void DeleteExample(int id);
        IEnumerable<ExampleVariant> GetAllExampleVariants();
        ExampleVariant GetExampleVariantById(int id);
        void AddExampleVariant(ExampleVariant exampleVariant);
        void UpdateExampleVariant(ExampleVariant exampleVariant);
        void DeleteExampleVariant(int id);
        IEnumerable<Example> GetAllExpressionsByTestId(int testId);
        IEnumerable<ExampleVariant> GetAllExpressionVariantsByExampleId(int exampleId);
        void SetCurrentStudent(string username);
        void SetCurrentTeacher(string username);
        void ClearCurrentUser();
        TestResult GetTestResultByTestId(int testId);
        void SetCurrentTest(int testId);
        void SetCurrentExample(int exId);

        void SetCurrentExampleVariant(int exvarId);
        void ClearCurrentTest();
        Test GetCurrentTest();
        Example GetCurrentExample();
        ExampleVariant GetCurrentExampleVariant();

        void AddAnswerToVariant(int exampleVariantId, string answerText);

        void AddBaseAnswerToExample(int exampleId, string answerText);
    }
}
