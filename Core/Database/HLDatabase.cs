using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace HLIOSCore
{
    public static class HLDatabase
    {
        static string _dbPath;
        static string _dbName = "datav1.db";
        static SQLite.SQLiteConnection _db;
        static object _lock = new object();

        static HLDatabase()
        {
			var dir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
			_dbPath = Path.Combine(dir, _dbName);
            try
            {
                _db = new SQLite.SQLiteConnection(_dbPath, SQLite.SQLiteOpenFlags.ReadWrite);
            }
            catch (SQLite.SQLiteException e)
            {
                Debug.WriteLine("First time, need to create a new db");
            }
        }

        public static bool DbExisted
        {
            get
            {
                return _db != null;
            }

        }

        public static void CreateTables()
        {
            _db.CreateTable<Crop>();
        }

        public static void DropTables()
        {
            _db.DropTable<Crop>();
        }

        public static void AddToDb<T>(IList<T> things)
        {
            lock (_lock)
            {
                _db.InsertAll(things);
            }
        }

        public static IList<T> GetTable<T>() where T : new()
        {
            var ret = _db.Table<T>().ToList();
            return ret;
        }

        public static void CreateDummyData(IEnumerable<string> cropData)
        {
            if (!DbExisted)
            {
                _db = new SQLite.SQLiteConnection(_dbPath);
            }

            HLDatabase.DropTables();
            HLDatabase.CreateTables();

            var crops = new List<Crop>();

            foreach (var line in cropData)
            {
                var str = line.Split(new char[] { ',' });

                var currCrop = new Crop
                {
                    Name = str[0],
                    LbsPBushel = double.Parse(str[1]),
                    BushelPTonne = double.Parse(str[2]),
                    KgPBushel = double.Parse(str[3]),
                    KernelWeight = double.Parse(str[4])
                };
                crops.Add(currCrop);
            }

            HLDatabase.AddToDb(crops);
        }
    }
}

