using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.Entities.Taxonomy
{
    class SubjectCategoryRepository
    {
        protected string _ConnectionString;

        public SubjectCategoryRepository(string cnStr)
        {
            _ConnectionString = cnStr;
        }



    }
}
