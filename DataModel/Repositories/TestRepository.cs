using Core.Common;
using Core.Dto;
using Core.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DataModel.Repositories
{
    public class TestRepository
    {
        private ExationisEntities context;

        public TestRepository()
        {
            this.context = new ExationisEntities();
        }

        public void CreateTest(int id, TestDto test)
        {
            this.context.Tests
                .Add(new Test
                {
                    Name = test.Name,
                    Limitation = test.Limitation,
                    Evaluation = test.Evaluation,
                    DisciplineID = id
                });
            this.context.SaveChanges();
        }

        public int GetTestID(int id, string name)
        {
            return this.context.Tests
                .Where(t => t.Name == name && t.Discipline.DisciplineID == id)
                .Select(t => t.TestID)
                .SingleOrDefault();
        }

        public TestDto GetTest(int id)
        {
            if (!this.context.Tests.Any(t => t.TestID == id))
            {
                if(SessionManager.LoggedInUser.Language.ISO.Equals("en"))
                    throw new NullReferenceException("Test Not Found");
                if (SessionManager.LoggedInUser.Language.ISO.Equals("uk"))
                    throw new NullReferenceException("Тест не знайдено");
            }
                

            TestDto test = this.context.Tests
                .Where(t => t.TestID == id)
                .Select(t => new TestDto
                {
                    ID = t.TestID,
                    Name = t.Name,
                    Limitation = t.Limitation,
                    Evaluation = t.Evaluation
                }).Single();

            test.EditAccess = !this.context.TestPoints.Any(tp => tp.TestID == id);

            return test;
        }

        public bool IsTestExist(int id, string name)
        {
            if (this.context.Tests.Any(t => t.Discipline.DisciplineID == id && t.Name == name))
                return true;
            return false;
        }

        public bool IsDisciplineExist(int id)
        {
            return this.context.Disciplines.Any(d => d.DisciplineID == id);
        }

        public TestDto[] GetTests(int id)
        {
            if (!this.context.Tests.Any(t => t.DisciplineID == id))
            {
                if (SessionManager.LoggedInUser.Language.ISO.Equals("en"))
                    throw new ArgumentException("This discipline is empty");
                if (SessionManager.LoggedInUser.Language.ISO.Equals("uk"))
                    throw new ArgumentException("Ця дисципліна порожня");
            }

            TestDto[] tests = this.context.Tests
                .Where(t => t.DisciplineID == id)
                .Select(t => new TestDto
                {
                    ID = t.TestID,
                    Name = t.Name,
                    Evaluation = t.Evaluation,
                    Limitation = t.Limitation,
                    Points = t.TestPoints.Where(p => p.TestID == t.TestID && SessionManager.LoggedInUser.ID == p.UserID).Select(p => p.Points).FirstOrDefault(),
                    Access = this.context.Accesses.Any(a => a.UserID == SessionManager.LoggedInUser.ID && a.TestID == t.TestID)
                }).ToArray();

            return tests;
        }


        public TestDto[] GetActualTests(int id)
        {
            TestDto[] tests = this.context.TestPoints
                .Where(t => t.Test.DisciplineID == id)
                .Select(t => new TestDto
                {
                    ID = t.TestID,
                    Name = t.Test.Name,
                    Evaluation = t.Test.Evaluation,
                    Limitation = t.Test.Limitation
                })
                .Distinct()
                .ToArray();

            return tests;
        }

        public string GetTestLimitation(int id)
        {
            return this.context.Tests
                .Where(t => t.TestID == id)
                .Select(t => t.Limitation)
                .Single();
        }

        public QuestionDto[] GetContent(int id, bool isShuffle)
        {
            QuestionDto[] content = this.context.Answers
                 .Where(a => a.Question.TestID == id)
                 .GroupBy(g => g.QuestionID)
                 .AsEnumerable()
                 .Select(g => new QuestionDto
                 {
                     ID = g.Key,
                     TestID = id,
                     TestName = g.Select(a => a.Question.Test.Name).FirstOrDefault(),
                     Content = g.Select(a => a.Question.Content).FirstOrDefault(),
                     Points = g.Select(a => a.Question.Points).FirstOrDefault(),
                     Answer = g.Select(a => new AnswerDto
                     {
                         ID = a.AnswerID,
                         QuestionID = a.QuestionID,
                         Content = a.Content,
                         Correct = a.Correct,
                         Result = false
                     }).ToArray()

                 }).ToArray();

            if (isShuffle)
            {
                content = content.OrderBy(c => Guid.NewGuid()).ToArray();
                for (int i = 0; i < content.Length; i++)
                    content[i].Answer = content[i].Answer.OrderBy(a => Guid.NewGuid()).ToArray();
            }

            return content;
        }

        public void AddQuestion(QuestionDto question)
        {
            this.context.Questions
                .Add(new Question
                {
                    Content = question.Content,
                    TestID = question.TestID,
                    Points = question.Points
                });
            this.context.SaveChanges();

            question.ID = GetQuestionID(question.TestID);

            for (int i = 0; i < question.Answer.Length; i++)
            {
                this.context.Answers
                            .Add(new Answer
                            {
                                Content = question.Answer[i].Content,
                                Correct = question.Answer[i].Correct,
                                QuestionID = question.ID
                            });
            }
            this.context.SaveChanges();

            Test test = this.context.Tests.Find(question.TestID);
            test.Evaluation = this.context.Questions
                .Where(q => q.TestID == test.TestID)
                .Sum(q => q.Points);

            this.context.Entry(test).State = EntityState.Modified;
            this.context.SaveChanges();
        }


        public void EditTest(TestDto сhangedTest)
        {
            if (this.context.Tests.Any(t => t.TestID == сhangedTest.ID))
            {
                Test test = this.context.Tests.Find(сhangedTest.ID);
                test.Name = сhangedTest.Name;
                test.Limitation = сhangedTest.Limitation;

                this.context.SaveChanges();
            }
        }

        public void EditQuestion(QuestionDto сhangedQuestion)
        {
            if (this.context.Questions.Any(q => q.QuestionID == сhangedQuestion.ID))
            {
                Question question = this.context.Questions.Find(сhangedQuestion.ID);
                Answer[] answers = new Answer[4];

                question.Content = сhangedQuestion.Content;
                question.Points = сhangedQuestion.Points;

                for (int i = 0; i < answers.Length; i++)
                {
                    answers[i] = this.context.Answers.Find(сhangedQuestion.Answer[i].ID);
                    answers[i].Content = сhangedQuestion.Answer[i].Content;
                    answers[i].Correct = сhangedQuestion.Answer[i].Correct;

                }

                this.context.SaveChanges();
            }
        }

        public void SendResult(QuestionDto[] results)
        {
            double testPoints = 0;

            foreach (QuestionDto question in results)
            {
                //count of correct answers in question (default)
                int countCorrectAnswers = 0;

                //count of correct answers in question (student)
                int countCorrectResultAnswers = 0;

                foreach (AnswerDto answer in question.Answer)
                {
                    if (answer.Correct)
                        countCorrectAnswers++;

                    if (answer.Correct && answer.Correct == answer.Result)
                        countCorrectResultAnswers++;

                    this.context.TestResults.Add(new TestResult
                    {
                        UserID = SessionManager.LoggedInUser.ID,
                        AnswerID = answer.ID,
                        Result = answer.Result
                    });
                }

                if (countCorrectAnswers == countCorrectResultAnswers)
                    testPoints += question.Points;
            }

            this.context.TestPoints.Add(new TestPoint
            {
                TestID = results.First().TestID,
                UserID = SessionManager.LoggedInUser.ID,
                Points = testPoints,
                Date = DateTime.Now
            });

            this.context.SaveChanges();
        }


        public ShortResultDto[] GetShortResult(int id)
        {
            ShortResultDto[] results = this.context.TestPoints
                .Where(t => t.Test.TestID == id)
                .Select(r => new ShortResultDto
                {
                    UserID = r.UserID,
                    TestID = r.TestID,
                    Name = r.User.Name,
                    SurName = r.User.SurName,
                    Points = r.Points,
                    Evaluation = r.Test.Evaluation,
                    Date = r.Date
                })
                .ToArray();

            return results;
        }

        public QuestionDto[] GetFullResult(int testID, int userID)
        {
            QuestionDto[] results = this.context.TestResults
                 .Where(t => t.Answer.Question.TestID == testID && t.UserID == userID)
                 .GroupBy(g => g.Answer.QuestionID)
                 .AsEnumerable()
                 .Select(g => new QuestionDto
                 {
                     Content = g.Select(q => q.Answer.Question.Content).FirstOrDefault(),
                     Points = g.Select(q => q.Answer.Question.Points).FirstOrDefault(),
                     Answer = g.Select(q => new AnswerDto
                     {
                         Content = q.Answer.Content,
                         Correct = q.Answer.Correct,
                         Result = q.Result
                     }).ToArray()

                 }).ToArray();

            return results;
        }

        public int GetQuestionID(int testID)
        {
            return this.context.Questions
                .Where(q => q.TestID == testID)
                .OrderByDescending(q => q.QuestionID)
                .Select(q => q.QuestionID)
                .First();
        }

        public async Task UploadImage(int questionID, string imageID)
        {
            this.context.Images
                 .Add(new Image
                 {
                     ImageID = imageID,
                     QuestionID = questionID
                 });
            await this.context.SaveChangesAsync();
        }

        public async Task<ImageDto[]> GetImages(int questionID)
        {
            return await this.context.Images
                .Where(im => im.QuestionID == questionID)
                .Select(im => new ImageDto
                {
                    ImageID = im.ImageID
                }).ToArrayAsync();
        }

        public void DeleteImage(string id)
        {
            if (this.context.Images.Any(im => im.ImageID.Equals(id)))
            {
                Image image = this.context.Images.Find(id);

                this.context.Images.Remove(image);
                this.context.SaveChanges();
            }
        }

        public ChartOneGroupDto[] GetChartData(int testID, string[] groups)
        {
            ChartOneGroupDto[] chartData = this.context.TestPoints
                .Where(tp => tp.TestID == testID && groups.Any(g => g == tp.User.Group))
                .GroupBy(g => g.User.Group)
                .AsEnumerable()
                .Select(g => new ChartOneGroupDto
                {
                    Group = g.Select(sd => sd.User.Group).FirstOrDefault(),
                    TestEvaluation = g.Select(sd => sd.Test.Evaluation).FirstOrDefault(),
                    StudentsData = g.Select(sd => new StudentChartData
                    {
                        Name = sd.User.Name,
                        SurName = sd.User.SurName,
                        StudentPoints = sd.Points,
                        Percent = Math.Round(sd.Points * 100 / sd.Test.Evaluation, 2)
                    }).ToArray()
                }).ToArray();

            for (int i = 0; i < chartData.Length; i++)
            {
                double avarage = 0;
                foreach (StudentChartData sdata in chartData[i].StudentsData)
                {
                    avarage = avarage += sdata.StudentPoints;
                }
                chartData[i].Avarage = avarage / chartData[i].StudentsData.Length;
                chartData[i].Percent = Math.Round(chartData[i].Avarage * 100 / chartData[i].TestEvaluation, 2);
            }

            return chartData;
        }

        public ChartOneGroupDto GetChartData(int testID, string group)
        {
            ChartOneGroupDto chartData = this.context.TestPoints
                .Where(tp => tp.TestID == testID && tp.User.Group == group)
                .GroupBy(g => g.User.Group)
                .AsEnumerable()
                .Select(g => new ChartOneGroupDto
                {
                    Group = g.Select(sd => sd.User.Group).FirstOrDefault(),
                    TestEvaluation = g.Select(sd => sd.Test.Evaluation).FirstOrDefault(),
                    StudentsData = g.Select(sd => new StudentChartData
                    {
                        Name = sd.User.Name,
                        SurName = sd.User.SurName,
                        StudentPoints = sd.Points,
                        Percent = Math.Round(sd.Points * 100 / sd.Test.Evaluation, 2)
                    }).ToArray()
                }).FirstOrDefault();


            double avarage = 0;
            foreach (StudentChartData sdata in chartData.StudentsData)
            {
                avarage = avarage += sdata.StudentPoints;
            }
            chartData.Avarage = avarage / chartData.StudentsData.Length;
            chartData.Percent = Math.Round(chartData.Avarage * 100 / chartData.TestEvaluation, 2);

            return chartData;
        }
    }
}
