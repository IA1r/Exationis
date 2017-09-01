using Core.Common;
using Core.Dto;
using Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Repositories
{
    public class DisciplineRepository
    {
        private ExationisEntities context;

        public DisciplineRepository()
        {
            this.context = new ExationisEntities();
        }

        public DisciplineDto[] GetDisciplines()
        {
            DisciplineDto[] disciplines = this.context.Disciplines
               .Where(d => d.UserID == SessionManager.LoggedInUser.ID)
               .AsEnumerable()
               .Select(d => new DisciplineDto
               {
                   Name = d.Name,
                   ID = d.DisciplineID,
                   Tests = d.Tests.Select(t => new TestDto
                   {
                       ID = t.TestID,
                       Name = t.Name
                   })
                   .ToArray()
               })
               .ToArray();

            return disciplines;
        }

        public DisciplineDto[] GetActualDisciplines()
        {
            DisciplineDto[] disciplines = this.context.TestPoints
                .Where(p => p.Test.Discipline.UserID == SessionManager.LoggedInUser.ID)
                .GroupBy(g => g.Test.Discipline.Name)
                .AsEnumerable()
                .Select(g => new DisciplineDto
                {
                    ID = g.Select(d => d.Test.DisciplineID).FirstOrDefault(),
                    Name = g.Select(d => d.Test.Discipline.Name).FirstOrDefault(),
                    Course = g.Select(d => d.Test.Discipline.Course).FirstOrDefault()
                })
                .ToArray();

            return disciplines;
        }

        public DisciplineDto GetDiscipline(int id)
        {
            if (!this.context.Disciplines.Any(d => d.DisciplineID == id))
            {
                if (SessionManager.LoggedInUser.Language.ISO.Equals("en"))
                    throw new ArgumentException("Discipline Not Found");
                if (SessionManager.LoggedInUser.Language.ISO.Equals("uk"))
                    throw new ArgumentException("Дисципліни не знайдено");
            }

            return this.context.Disciplines
                .Where(d => d.DisciplineID == id)
                .Select(d => new DisciplineDto
                {
                    ID = d.DisciplineID,
                    Name = d.Name,
                    Head = d.User.SurName + " " + d.User.Name + " " + d.User.MiddleName
                })
                .SingleOrDefault();
        }

        public void CreateDiscipline(DisciplineDto discipline)
        {

            this.context.Disciplines.Add(new Discipline
            {
                Name = discipline.Name,
                UserID = SessionManager.LoggedInUser.ID,
                FacultyID = discipline.FacultyID,
                Course = discipline.Course
            });

            this.context.SaveChanges();
        }

        public bool IsExistDiscipline(int id, string name)
        {
            return this.context.Disciplines.Any(d => d.FacultyID == id && d.Name == name);
        }

        public DisciplineDto[] SearchDiscipline(string key)
        {
            List<DisciplineDto> disciplines = new List<DisciplineDto>();
            foreach (FacultyDto faculty in CacheManager.Faculties)
            {
                foreach (DisciplineDto discipline in faculty.Disciplines)
                {
                    discipline.Faculty = faculty.Name;
                    disciplines.Add(discipline);
                }
            }

            disciplines = disciplines
                 .Where(d => d.Name.Contains(key))
                .Select(d => new DisciplineDto
                {
                    ID = d.ID,
                    Faculty = d.Faculty,
                    Name = d.Name,
                    Course = d.Course
                }).ToList();

            return disciplines.ToArray();
        }
    }
}
