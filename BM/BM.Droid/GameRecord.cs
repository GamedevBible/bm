using System;

namespace BM.Droid
{
    public class GameRecord
    {
        private int _questionNumber;
        private string _date;

        public int QuestionNumber
        {
            get { return _questionNumber; }
            set { _questionNumber = value; }
        }

        public string RecordDate
        {
            get { return _date; }
            set { _date = value; }
        }
    }
}