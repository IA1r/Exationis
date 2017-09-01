using Core.Common;
using Core.Dto;
using DataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.Managers
{
    public class DisciplineManager
    {
        private DisciplineRepository repository;

        public DisciplineManager()
        {
            this.repository = new DisciplineRepository();
        }

        //public FacultyDto[] GetDisciplines()
        //{
        //    return this.repository.GetDisciplines();
        //}

        public DisciplineDto GetDiscipline(int id)
        {
            return this.repository.GetDiscipline(id);
        }

        public DisciplineDto[] GetDisciplines()
        {
            return this.repository.GetDisciplines();
        }

        public DisciplineDto[] GetActualDisciplines()
        {
            return this.repository.GetActualDisciplines();
        }

        public void CreateDiscipline(DisciplineDto discipline)
        {
            if (this.repository.IsExistDiscipline(discipline.FacultyID, discipline.Name))
            {
                if (SessionManager.LoggedInUser.Language.ISO.Equals("en"))
                    throw new ArgumentException("This Discipline already exist.");
                if (SessionManager.LoggedInUser.Language.ISO.Equals("uk"))
                    throw new ArgumentException("Ця дисципліна вже існує.");
            }
                

            this.repository.CreateDiscipline(discipline);
        }

        public DisciplineDto[] SearchDiscipline(string key)
        {
            return this.repository.SearchDiscipline(key);
        }

    }
}
