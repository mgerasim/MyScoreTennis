using Entity.Common;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyScoreTennisEntity.Models
{
    public class Telephone : Entity.Common.BaseClass<Telephone>
    {
        public virtual string Number { get; set; }
        
        public Telephone()
        {
            Number = "";
        }



    }
}
