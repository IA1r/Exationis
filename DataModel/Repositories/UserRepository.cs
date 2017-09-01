using Core.Common;
using Core.Const;
using Core.Dto;
using Core.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace DataModel.Repositories
{
    public class UserRepository
    {
        private ExationisEntities context;
        public UserRepository()
        {
            this.context = new ExationisEntities();
        }

        public void Registration(UserDto userDto, bool IsTeacher)
        {
            if (!string.IsNullOrWhiteSpace(userDto.Group))
                if (DateTime.Now.Month >= 1 && DateTime.Now.Month <= 8)
                    userDto.Group += " - " + (DateTime.Now.Year - 1).ToString();

            User user = new User {
                Login = userDto.Login,
                Name = userDto.Name,
                SurName = userDto.SurName,
                MiddleName = userDto.MiddleName,
                Email = userDto.Email,
                Password = userDto.Password,
                ConfirmPassword = userDto.ConfirmPassword,
                Group = userDto.Group,
                RoleID = 3,
                FacultyID = userDto.FacultyID,
                Avatar = "0B12_K_kZ4YCwWmdmYWZBQ3gtZVU",
                LanguageID = 1
            };

            if (IsTeacher)
                user.RoleID = 2;

            this.context.Users.Add(user);

            this.context.SaveChanges();
        }

        public void SignIn(string userlogin, bool rememberMe)
        {
            //System.Security.Cryptography.SHA1 sha = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            //var bytes = Encoding.ASCII.GetBytes("11111111");
            //var result = sha.ComputeHash(bytes);
            //var str = ASCIIEncoding.ASCII.GetString(result);


            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket
                (
                    1,
                    userlogin,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(10),
                    rememberMe,
                    JsonConvert.SerializeObject(userlogin),
                    FormsAuthentication.FormsCookiePath
                );

            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
            cookie.Expires = rememberMe ? ticket.Expiration : DateTime.MinValue;
            HttpContext.Current.Response.SetCookie(cookie);

            SessionManager.LoggedInUser = GetUser(userlogin);
            SessionManager.Expire = 10000;
            HttpContext.Current.Cache.Remove(CacheKey.Faculties);

        }

        //public void SignOut()
        //{
        //    FormsAuthentication.SignOut();
        //    SessionManager.LoggedInUser = null;
        //}

        public UserDto GetUser(string login)
        {
            UserDto user = this.context.Users
                   .Where(u => u.Login == login)
                   .Select(u => new UserDto
                   {
                       ID = u.UserID,
                       Name = u.Name,
                       Login = u.Login,
                       Avatar = u.Avatar,
                       Password = u.Password,
                       Email = u.Email,
                       SurName = u.SurName,
                       MiddleName = u.MiddleName,
                       Role = u.Role.Name,
                       Group = u.Group,
                       Faculty = u.Faculty.Name,
                       Language = new LanguageDto
                       {
                           LanguageID = u.LanguageID,
                           Name = u.Language.Name,
                           ISO = u.Language.ISO
                       }
                   })
                   .SingleOrDefault();

            return user;
        }

        public AccessDto[] GetStudents(int id, string group)
        {
            AccessDto[] students = this.context.Users
                .Where(s => s.Group == group)
                .Select(s => new AccessDto
                {
                    UserID = s.UserID,
                    TestID = id,
                    Name = s.Name,
                    SurName = s.SurName,
                    Group = s.Group,
                    Access = this.context.Accesses.Any(a => a.UserID == s.UserID && a.TestID == id)
                }).ToArray();

            return students;
        }

        public void SetAccess(AccessDto[] accessList)
        {
            foreach (AccessDto item in accessList)
            {
                if (item.Access)
                {
                    if (!this.context.Accesses.Any(a => a.TestID == item.TestID && a.UserID == item.UserID) &&
                        !this.context.TestPoints.Any(tp => tp.UserID == item.UserID && tp.TestID == item.TestID))
                        this.context.Accesses
                            .Add(new Access { TestID = item.TestID, UserID = item.UserID });
                }

                else
                    if (this.context.Accesses.Any(a => a.TestID == item.TestID && a.UserID == item.UserID))
                    {
                        Access access = this.context.Accesses.Where(a => a.TestID == item.TestID && a.UserID == item.UserID).Single();
                        this.context.Accesses.Remove(access);
                    }
            }

            this.context.SaveChanges();
        }

        public void RemoveAccess(int userID, int testID)
        {
            if (this.context.Accesses.Any(a => a.TestID == testID && a.UserID == userID))
            {
                Access access = this.context.Accesses.Where(a => a.TestID == testID && a.UserID == userID).Single();
                this.context.Accesses.Remove(access);

                this.context.SaveChanges();
            }
        }

        public string IsExistUser(string login, string email)
        {
            if (SessionManager.LoggedInUser == null)
            {
                if (login != null)
                    if (this.context.Users.Any(u => u.Login == login))
                        return "This Login already used.";

                if (email != null)
                    if (this.context.Users.Any(u => u.Email == email))
                        return "This Email alreade used.";
            }
            else
            {
                if (login != null)
                    if (this.context.Users.Any(u => u.Login == login && login != SessionManager.LoggedInUser.Login))
                        return "This Login already used.";

                if (email != null)
                    if (this.context.Users.Any(u => u.Email == email && email != SessionManager.LoggedInUser.Email))
                        return "This Email alreade used.";
            }
            return null;
        }

        public bool IsLoginExist(string login)
        {
            if (SessionManager.LoggedInUser == null)
                if (this.context.Users.Any(u => u.Login == login))
                    return true;
                else { }
            else
                if (this.context.Users.Any(u => u.Login == login && login != SessionManager.LoggedInUser.Login))
                    return true;

            return false;
        }

        public bool IsEmailExist(string email)
        {
            if (SessionManager.LoggedInUser == null)
                if (this.context.Users.Any(u => u.Email == email))
                    return true;
                else { }
            else
                if (this.context.Users.Any(u => u.Email == email && email != SessionManager.LoggedInUser.Email))
                    return true;

            return false;
        }


        public bool ValidateUser(string login, string password, bool IsTeacher)
        {
            if (IsTeacher && this.context.Users.Any(u => u.Login == login && u.Password == password && u.Role.Name == "Teacher"))
                return true;
            if (!IsTeacher && this.context.Users.Any(u => u.Login == login && u.Password == password))
                return true;

            return false;
        }

        public string[] GetGroups()
        {
            string[] qwe = this.context.TestPoints
                .Where(tp => tp.Test.Discipline.UserID == SessionManager.LoggedInUser.ID && tp.User.RoleID == 3)
                .Select(g => g.User.Group)
                .Distinct()
                .ToArray();

            return qwe;
        }

        public UserDto GetUserProfile(int id)
        {
            if (!this.context.Users.Any(u => u.UserID == id))
            {
                if (SessionManager.LoggedInUser.Language.ISO.Equals("en"))
                    throw new ArgumentException("This user doesn't exist");
                if (SessionManager.LoggedInUser.Language.ISO.Equals("uk"))
                    throw new ArgumentException("Користувача не знайдено");
            }

            UserDto userData = this.context.Users
                .Where(u => u.UserID == id)
                .AsEnumerable()
                .Select(u => new UserDto
                {
                    ID = u.UserID,
                    Avatar = u.Avatar,
                    Name = u.Name,
                    SurName = u.SurName,
                    MiddleName = u.MiddleName,
                    Login = u.Login,
                    Email = u.Email,
                    Password = u.Password,
                    ConfirmPassword = u.ConfirmPassword,
                    Role = u.Role.Name,
                    FacultyID = u.FacultyID,
                    Faculty = u.Faculty.Name,
                    Results = u.TestPoints.Select(tp => new ShortResultDto
                    {
                        TestName = tp.Test.Name,
                        Points = tp.Points,
                        Evaluation = tp.Test.Evaluation,
                        Date = tp.Date
                    }).ToArray(),
                    Language = new LanguageDto
                    {
                        LanguageID = u.LanguageID,
                        Name = u.Language.Name,
                        ISO = u.Language.ISO
                    }
                }).FirstOrDefault();

            return userData;
        }

        public void EditUserProfile(UserDto profile)
        {
            if (!this.context.Users.Any(u => u.UserID == profile.ID))
            {
                if (SessionManager.LoggedInUser.Language.ISO.Equals("en"))
                    throw new ArgumentException("This user doesn't exist");
                if (SessionManager.LoggedInUser.Language.ISO.Equals("uk"))
                    throw new ArgumentException("Користувача не знайдено");
            }

            User user = this.context.Users.Find(profile.ID);

            user.Name = profile.Name;
            user.SurName = profile.SurName;
            user.Login = profile.Login;
            user.Email = profile.Email;
            user.Password = profile.Password;
            user.ConfirmPassword = profile.ConfirmPassword;

            this.context.SaveChanges();
        }

        public async Task AvatarUpload(int id, string avatarID)
        {
            User user = this.context.Users.Find(id);

            SessionManager.LoggedInUser.Avatar = avatarID;
            user.Avatar = avatarID;
            await this.context.SaveChangesAsync();
        }

        public bool CheckDisciplineAccess(int id)
        {
            return this.context.Disciplines.Any(d => d.DisciplineID == id && d.UserID == SessionManager.LoggedInUser.ID);
        }

        public LanguageDto[] GetLanguages()
        {
            return this.context.Languages
                .Select(l => new LanguageDto
                {
                    LanguageID = l.LanguageID,
                    Name = l.Name,
                    ISO = l.ISO
                }).ToArray();
        }

        public void ChangeLanguage(int id)
        {
            User user = this.context.Users.Find(SessionManager.LoggedInUser.ID);
            user.LanguageID = id;
            this.context.SaveChanges();

            Language language = this.context.Languages.Find(id);
            SessionManager.LoggedInUser.Language = new LanguageDto
            {
                LanguageID = language.LanguageID,
                Name = language.Name,
                ISO = language.ISO
            };

            HttpContext.Current.Cache.Remove(CacheKey.Faculties);
        }

    }
}
