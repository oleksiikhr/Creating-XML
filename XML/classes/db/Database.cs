﻿using System;
using SQLite;
using System.IO;

using XML.classes.db.shop;
using XML.classes.db.offer;
using XML.classes.db.currency;
using XML.classes.db.category;
using XML.classes.db.parametrs;

namespace XML.classes.db
{
    public class Database
    {
        public static string FILE_NAME = "data.sqlite";
        public static string DIR = AppDomain.CurrentDomain.BaseDirectory;

        protected static SQLiteConnection con;
        private static bool instanse = false;

        public static bool SetConnection()
        {
            if (instanse)
                return instanse;

            try
            {
                Directory.CreateDirectory(DIR + "\\saves");
                con = new SQLiteConnection(DIR + "\\saves\\" + FILE_NAME);

                Databases();
                instanse = true;

                return instanse;
            }
            catch
            {
                return instanse;
            }
        }

        public static void CreateTable<T>()
        {
            try
            {
                con.CreateTable<T>();
            }
            catch { }
        }

        public static int Insert(object ob)
        {
            try
            {
                return con.Insert(ob);
            }
            catch
            {
                return 0;
            }
        }

        public static int Update(object ob)
        {
            try
            {
                return con.Update(ob);
            }
            catch
            {
                return 0;
            }
        }

        public static int DeleteObject<T>(object id)
        {
            try
            {
                return con.Delete<T>(id);
            }
            catch
            {
                return 0;
            }
        }

        public static void Databases()
        {
            CreateTable<OfferTable>();
            CreateTable<ShopTable>();
            CreateTable<CategoryTable>();
            CreateTable<CurrencyTable>();
            CreateTable<ParametrsTable>();
        }

        public static void CloseConnection()
        {
            con.Close();
            instanse = false;
        }

        public static bool IsConnected()
        {
            return instanse;
        }
    }
}
