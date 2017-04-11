using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BM.Droid
{
    public class QuestionsDatabase : SQLiteConnection
    {
        public const string DATABASE_NAME = "millionaire.db";

        public QuestionsDatabase(string path) : base($"{path}/{DATABASE_NAME}")
        {
        }

        public IReadOnlyList<questions> GetAllItems()
        {
            BeginTransaction();
            var result = Table<questions>().OrderBy(t => t.level);
            return result.ToList();
        }
    }
}
