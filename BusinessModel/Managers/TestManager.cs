using Core.Common;
using Core.Dto;
using Core.Helpers;
using DataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.Managers
{
    public class TestManager
    {
        private TestRepository repository;
        private UserRepository userRepository;

        public TestManager()
        {
            this.repository = new TestRepository();
            this.userRepository = new UserRepository();
        }

        public void CreateTest(int id, TestDto test)
        {
            this.repository.CreateTest(id, test);
        }

        public string IsTestExist(int id, string name)
        {
            if (this.repository.IsTestExist(id, name))
            {
                if (SessionManager.LoggedInUser.Language.ISO.Equals("en"))
                    throw new ArgumentException("This Test already exist.");

                if (SessionManager.LoggedInUser.Language.ISO.Equals("uk"))
                    throw new ArgumentException("Цей тест уже існує.");
            }

            return null;
        }

        public TestDto GetTest(int id)
        {
            return this.repository.GetTest(id);
        }

        public int GetTestID(int id, string name)
        {
            return this.repository.GetTestID(id, name);
        }

        public TestDto[] GetTests(int id)
        {
            return this.repository.GetTests(id);
        }

        public TestDto[] GetActualTests(int id)
        {
            if (this.repository.IsDisciplineExist(id))
                if (!this.userRepository.CheckDisciplineAccess(id))
                {
                    if (SessionManager.LoggedInUser.Language.ISO.Equals("en"))
                        throw new ArgumentException("You do not have access to this discipline");
                    if (SessionManager.LoggedInUser.Language.ISO.Equals("uk"))
                        throw new ArgumentException("У вас немає доступу до цієї дисципліни");
                }

                else { }
            else
            {
                if (SessionManager.LoggedInUser.Language.ISO.Equals("en"))
                    throw new NullReferenceException("This discipline doesn't exist");
                if (SessionManager.LoggedInUser.Language.ISO.Equals("uk"))
                    throw new ArgumentException("Цієї дисципліни не існує");
            }

            return this.repository.GetActualTests(id);
        }

        public QuestionDto[] GetContent(int id, bool isShuffle)
        {
            return this.repository.GetContent(id, isShuffle);
        }

        public string GetTestLimitation(int id)
        {
            return this.repository.GetTestLimitation(id);
        }

        public void AddQuestion(QuestionDto question)
        {
            this.repository.AddQuestion(question);
        }
        public void EditTest(TestDto newTest)
        {
            this.repository.EditTest(newTest);
        }
        public void EditQuestion(QuestionDto newQuestion)
        {
            this.repository.EditQuestion(newQuestion);
        }

        public void SendResult(QuestionDto[] results)
        {
            this.repository.SendResult(results);
        }


        public ShortResultDto[] GetShortResult(int id)
        {
            return this.repository.GetShortResult(id);
        }

        public QuestionDto[] GetFullResult(int testID, int userID)
        {
            return this.repository.GetFullResult(testID, userID);
        }

        public async Task UploadImage(int questionID, string imageID)
        {
            await this.repository.UploadImage(questionID, imageID);
        }

        public async Task<ImageDto[]> GetImages(int questionID)
        {
            return await this.repository.GetImages(questionID);
        }

        public void DeleteImage(string id)
        {
            this.repository.DeleteImage(id);
        }
        public ChartOneGroupDto[] GetChartData(int testID, string[] groups)
        {
            return this.repository.GetChartData(testID, groups);
        }

        public ChartOneGroupDto GetChartData(int testID, string group)
        {
            return this.repository.GetChartData(testID, group);
        }
    }
}
