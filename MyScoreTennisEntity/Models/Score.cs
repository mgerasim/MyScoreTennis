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
    public class Score : Entity.Common.BaseClass<Score>
    {
        public virtual int HighlightLeft { get; set; }

        public virtual int HighlightRight { get; set; }

        public virtual int Highlight { get; set; }
        public virtual string Fifteens { get; set; }
        public virtual Sethistory Set { get; set; }
        
        
        public Score()
        {
            this.HighlightLeft = 0;
            this.HighlightRight = 0;
            this.Highlight = 0;
            this.Fifteens = "";
        }

        static public List<Score> GetAllBySet(Sethistory theSet)
        {
            ISession session = theSet.session;
            if (session == null)
            {
                session = NHibernateHelper.OpenSession();
            }
            ICriteria criteria = session.CreateCriteria(typeof(Score));
            criteria.Add(Restrictions.Eq("Set", theSet));
            criteria.AddOrder(Order.Asc("ID"));
            return criteria.List<Score>().ToList<Score>();            
        }
        
        

    }
}
