using Core.Common;
using Core.Dto;
using Core.MicrosoftTranslator;
using Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace DataModel.Repositories
{
    public class FacultyRepository
    {
        private ExationisEntities context;

        public FacultyRepository()
        {
            this.context = new ExationisEntities();
        }

        public FacultyDto[] GetFacultyList()
        {
            FacultyDto[] faculty = this.context.Faculties
                .Select(f => new FacultyDto
                {
                    ID = f.FacultyID,
                    Name = f.Name
                }).ToArray();

            return faculty;
        }

        public FacultyDto[] GetFaculties()
        {
            FacultyDto[] facultys = this.context.Disciplines
                .GroupBy(f => f.Faculty.Name)
                .AsEnumerable()
                .Select(f => new FacultyDto
                {
                    Name = f.Key,
                    Disciplines = f.Select(d => new DisciplineDto
                    {
                        ID = d.DisciplineID,
                        Name = d.Name,
                        Head = d.User.Name,
                        Course = d.Course
                    })
                    .ToArray()
                })
                .ToArray();

            if (SessionManager.LoggedInUser != null && SessionManager.LoggedInUser.Language.ISO != "en")
            {
                for (int i = 0; i < facultys.Length; i++)
                {
                    facultys[i].Name = Translate.RunSample(new TranslateDto
                    {
                        Key = "1897640d20c64d9793f6cf7305da00af",
                        From = "en",
                        To = SessionManager.LoggedInUser.Language.ISO,
                        Text = facultys[i].Name
                    });
                }
            }

            CacheManager.Faculties = facultys;

            return facultys;
        }
    }
}
