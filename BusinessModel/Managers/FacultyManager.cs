using Core.Dto;
using DataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BusinessModel.Managers
{
	public class FacultyManager
	{
		private FacultyRepository repository;

		public FacultyManager()
		{
			this.repository = new FacultyRepository();
		}

		public FacultyDto[] GetFacultyList()
		{
			return this.repository.GetFacultyList();
		}

        public FacultyDto[] GetFaculties()
        {
            return this.repository.GetFaculties();
        }
	}
}
