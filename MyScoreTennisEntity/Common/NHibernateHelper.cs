using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyScoreTennisEntity.Models;
using NHibernate.Tool.hbm2ddl;
using System.IO;


namespace Entity.Common
{
    public class NHibernateHelper
    {
        private static ISessionFactory _sessionFactory;

        private static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    var configuration = new Configuration();
                    configuration.Configure();
                    
                    configuration.AddAssembly("MyScoreTennisEntity");

                    _sessionFactory = configuration.BuildSessionFactory();
                }
                return _sessionFactory;
            }
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }

        public static void UpdateSchema()
        {
            var configuration = new Configuration();
            configuration.Configure();
            configuration.AddAssembly("MyScoreTennisEntity");

            NHibernate.Tool.hbm2ddl.SchemaUpdate schemaUpdate
                = new NHibernate.Tool.hbm2ddl.SchemaUpdate(configuration);
            schemaUpdate.Execute(true, true);

            foreach (var item in schemaUpdate.Exceptions)
            {
                Console.WriteLine(item.Message + "\n\n" + item.Source + "\n\n" + item.StackTrace + "\n\n");
                if (item.InnerException != null) Console.WriteLine(item.InnerException.Message);
            }
        }
    }
}