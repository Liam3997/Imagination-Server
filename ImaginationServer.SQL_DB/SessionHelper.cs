﻿using System;
using System.IO;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using ImaginationServer.Common.Data;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace ImaginationServer.SQL_DB
{
    public class SessionHelper
    {
        private static ISessionFactory _sessionFactory;

        public static void Init()
        {
            _sessionFactory = Fluently.Configure()
                .Database(
                    SQLiteConfiguration.Standard
                        .UsingFile("Database.db")
                )
                .Mappings(
                    m =>
                        m.FluentMappings.Add<AccountMap>().Add<CharacterMap>().Add<BackpackItemMap>())
                .ExposeConfiguration(Config).BuildSessionFactory();
        }

        private static void Config(Configuration configuration)
        {
            if (!File.Exists("Database.db"))
                new SchemaExport(configuration).Create(false, true);
        }

        public static ISession CreateSession()
        {
            return _sessionFactory.OpenSession();
        }
    }
}