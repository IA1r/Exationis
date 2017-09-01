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
    public class UserManager
    {
        private UserRepository repository;
        public UserManager()
        {
            this.repository = new UserRepository();
        }

        public void Registration(UserDto user, string app)
        {
            if (app.Equals("Admin"))
                if (!user.KeyWord.Equals("keyword"))
                    throw new ArgumentException("Invalid KeyWord");
                else { this.repository.Registration(user, true); }

            else
                this.repository.Registration(user, false);
        }

        public string IsExistUser(string login = null, string email = null)
        {
            return repository.IsExistUser(login, email);
        }

        public bool IsLoginExist(string login)
        {
            if (this.repository.IsLoginExist(login))
                return true;
            return false;
        }
        public bool IsEmailExist(string email)
        {
            if (this.repository.IsEmailExist(email))
                return true;
            return false;
        }

        public void SignIn(UserDto user, bool IsTeacher)
        {
            if (this.repository.ValidateUser(user.Login, user.Password, IsTeacher))
                this.repository.SignIn(user.Login, user.RememberMe);
            else
                throw new ArgumentException("Invalid login or password");
        }

        public AccessDto[] GetStudents(int id, string group)
        {
            return this.repository.GetStudents(id, group);
        }

        public void SetAccess(AccessDto[] model)
        {
            this.repository.SetAccess(model);
        }
        public void RemoveAccess(int userID, int testID)
        {
            this.repository.RemoveAccess(userID, testID);
        }

        public string[] GetGroups()
        {
            return this.repository.GetGroups();
        }

        public UserDto GetUserProfile(int id)
        {
            return this.repository.GetUserProfile(id);
        }
        public void EditUserProfile(UserDto profile)
        {
            this.repository.EditUserProfile(profile);
        }

        public async Task AvatarUpload(int id, string filePath)
        {
            await this.repository.AvatarUpload(id, filePath);
        }

        //public bool CheckDisciplineAccess(int id)
        //{
        //    return this.repository.CheckDisciplineAccess(id);
        //}
        public LanguageDto[] GetLanguages()
        {
            return this.repository.GetLanguages();
        }

        public void ChangeLanguage(int id)
        {
            this.repository.ChangeLanguage(id);
        }
    }
}
