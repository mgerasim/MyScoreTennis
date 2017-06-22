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
    public class Match : Entity.Common.BaseClass<Match>
    {
        public virtual string Number { get; set; }
        public virtual int Status { get; set; }
        
        public Match()
        {
            Number = "";
            Status = 1;
        }

        public static List<Match> GetAllByStatus(int Status)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var result = session.CreateCriteria(typeof(Match))
                        .Add(Restrictions.Eq("Status", Status))
                        .List<Match>().ToList<Match>();

                return result;
            }
        }

        public static Match GetByNumber(string Number)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var result = session.CreateCriteria(typeof(Match))
                    .Add(Restrictions.Eq("Number", Number))
                    .UniqueResult<Match>();

                return result;
            }
        }

        public virtual List<Sethistory> Sets
        {
            get
            {
                return Sethistory.GetAllByMatch(this);
            }
        }

    }
}
