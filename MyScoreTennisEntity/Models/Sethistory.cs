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
    public class Sethistory : Entity.Common.BaseClass<Sethistory>
    {
        public virtual int NumberOrder { get; set; }

        public virtual Match Match { get; set; }
        public Sethistory()
        {
            NumberOrder = 1;
        }
        
        public static Sethistory GetByNumber(int NumberOrder)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var result = session.CreateCriteria(typeof(Sethistory))
                    .Add(Restrictions.Eq("NumberOrder", NumberOrder))
                    .UniqueResult<Sethistory>();

                return result;
            }
        }

        static public List<Sethistory> GetAllByMatch(Match theMatch)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                ICriteria criteria = session.CreateCriteria(typeof(Sethistory));
                criteria.Add(Restrictions.Eq("Match", theMatch));
                criteria.AddOrder(Order.Asc("ID"));
                return criteria.List<Sethistory>().ToList<Sethistory>();
            }
        }

        public virtual List<Score> Scores
        {
            get
            {
                return Score.GetAllBySet(this);
            }
        }

    }
}
