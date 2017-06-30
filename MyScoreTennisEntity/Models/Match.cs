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

        public static List<Match> GetAllByStatus(int Status, ISession sess = null)
        {
            ISession session = sess;
            if (session == null)
            {
                session = NHibernateHelper.OpenSession();
            }            
            var result = session.CreateCriteria(typeof(Match))
                    .Add(Restrictions.Eq("Status", Status))
                    .List<Match>().ToList<Match>();

            return result;            
        }

        public static Match GetByNumber(string Number, ISession sess = null)
        {
            ISession session = sess;
            if (session == null)
            {
                session = NHibernateHelper.OpenSession();
            }
            var result = session.CreateCriteria(typeof(Match))
                .Add(Restrictions.Eq("Number", Number))
                .UniqueResult<Match>();

            return result;           
        }

        public virtual List<Sethistory> Sets
        {
            get
            {
                return Sethistory.GetAllByMatch(this);
            }
        }

        public virtual bool Analizy()
        {
            try
            {
                var SethistoryWhere = this.Sets.Where(x => x.NumberOrder == 1);
                if (SethistoryWhere == null || SethistoryWhere.Count() == 0)
                {
                    this.Status = 3;
                    this.Update();
                    return false;
                }

                var SethistoryItem = SethistoryWhere.Single();

                var ScoreWhere = SethistoryItem.Scores.Where(x => x.HighlightLeft == 6 && (x.HighlightRight == 3 || x.HighlightRight == 4));

                if (ScoreWhere == null || ScoreWhere.Count() == 0)
                {
                    this.Status = 3;
                    this.Update();
                    return false;
                }

                this.Status = 4;
                this.Update();
                return true;
            }
            catch (Exception ex)
            {
                string err = ex.Message;
            }

            return false;
        }

    }
}
