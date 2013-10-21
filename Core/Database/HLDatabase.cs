using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace HLIOSCore
{
    public static class HLDatabase
    {
        static string _dbName = "data.db";
        static SQLite.SQLiteConnection _db;
        static object _lock = new object();

        static HLDatabase()
        {
            var docs = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var dbPath = Path.Combine(docs, _dbName);
            _db = new SQLite.SQLiteConnection(dbPath);
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

        public static void CreateDummyData()
        {
            HLDatabase.DropTables();
            HLDatabase.CreateTables();

            var crops = new List<Crop>
            {
                new Crop { Name = "Alfalfa", LbsPBushel = 60, BushelPTonne = 36.744, KgPBushel = 27.22, KernelWeight = 2 },
                new Crop { Name = "Barley", LbsPBushel = 48, BushelPTonne = 45.93, KgPBushel = 21.77, KernelWeight = 40 },
                new Crop { Name = "Brome Grass", LbsPBushel = 14, BushelPTonne = 157.5, KgPBushel = 6.35, KernelWeight = 4 },
            };

            HLDatabase.AddToDb(crops);
        }
    }
}

