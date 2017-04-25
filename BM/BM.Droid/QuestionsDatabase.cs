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

        public List<questions> GetAllItems()
        {
            List<questions> tempQuestions = new List<questions>();
            List<questions> readyQuestions = new List<questions>();

            BeginTransaction();

            for (int i = 1; i < 16; i++)
            {
                tempQuestions = Table<questions>().Where(t => t.level == i).ToList();
                var count = tempQuestions.Count;

                Random rand = new Random();
                int temp;
                temp = rand.Next(0, count);

                readyQuestions.Add(tempQuestions[temp]);
            }

            return readyQuestions;
        }

        public questions GetLastItem(int id)
        {
            questions lastQuestion;

            BeginTransaction();

            lastQuestion = Table<questions>().Where(t => t._id == id).ToList()[0];

            return lastQuestion;
        }
    }
}
