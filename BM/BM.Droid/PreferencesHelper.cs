using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Preferences;

namespace BM.Droid
{
    class PreferencesHelper
    {
        private ISharedPreferences _prefs;
        private ISharedPreferencesEditor _editor;
        private List<GameRecord> _records;
        private int[] _questionNumbers = new int[7];
        private bool _isSoundEnabled;
        private int _firstStarted;
        private int _millionsCount;
        private int _cleverCount;

        public PreferencesHelper()
        {
            
        }

        public void ProcessFirstStarted(Context context)
        {
            if (_prefs == null)
                _prefs = PreferenceManager.GetDefaultSharedPreferences(context);

            _firstStarted = _prefs.GetInt("firstStarted", 0);
            
            if (_editor == null)
                _editor = _prefs.Edit();

            _editor.PutInt("firstStarted", _firstStarted >= 365 ? 365 : _firstStarted + 1);
            _editor.Commit();
        }

        public void InitHelperForRecords(Context context)
        {
            _prefs = PreferenceManager.GetDefaultSharedPreferences(context);
            _editor = _prefs.Edit();

            _firstStarted = _prefs.GetInt("firstStarted", 0);
            _millionsCount = _prefs.GetInt("millionsCount", 0);
            _cleverCount = _prefs.GetInt("cleverCount", 0);

            _records = new List<GameRecord>();
            _records.Add(new GameRecord() { QuestionNumber = _prefs.GetInt("record1Number", 0), RecordDate = _prefs.GetString("record1Date", "-") });
            _records.Add(new GameRecord() { QuestionNumber = _prefs.GetInt("record2Number", 0), RecordDate = _prefs.GetString("record2Date", "-") });
            _records.Add(new GameRecord() { QuestionNumber = _prefs.GetInt("record3Number", 0), RecordDate = _prefs.GetString("record3Date", "-") });
            _records.Add(new GameRecord() { QuestionNumber = _prefs.GetInt("record4Number", 0), RecordDate = _prefs.GetString("record4Date", "-") });
            _records.Add(new GameRecord() { QuestionNumber = _prefs.GetInt("record5Number", 0), RecordDate = _prefs.GetString("record5Date", "-") });
            _records.Add(new GameRecord() { QuestionNumber = _prefs.GetInt("record6Number", 0), RecordDate = _prefs.GetString("record6Date", "-") });
            _records.Add(new GameRecord() { QuestionNumber = _prefs.GetInt("record7Number", 0), RecordDate = _prefs.GetString("record7Date", "-") });

            _questionNumbers[0] = _prefs.GetInt("record1Number", 0);
            _questionNumbers[1] = _prefs.GetInt("record2Number", 0);
            _questionNumbers[2] = _prefs.GetInt("record3Number", 0);
            _questionNumbers[3] = _prefs.GetInt("record4Number", 0);
            _questionNumbers[4] = _prefs.GetInt("record5Number", 0);
            _questionNumbers[5] = _prefs.GetInt("record6Number", 0);
            _questionNumbers[6] = _prefs.GetInt("record7Number", 0);

            _editor.Commit();    // applies changes synchronously on older APIs
            //_editor.Apply();
        }

        public void InitHeplerForSound(Context context)
        {
            if (_prefs == null || _editor == null)
            {
                _prefs = PreferenceManager.GetDefaultSharedPreferences(context);
                _editor = _prefs.Edit();
            }

            _firstStarted = _prefs.GetInt("firstStarted", 0);

            _isSoundEnabled = _prefs.GetBoolean("soundEnabled", true);
            _editor.Commit();
        }

        public bool GetSoundEnabled()
        {
            return _isSoundEnabled;
        }

        public void SetSoundEnabled(bool enabled)
        {
            _editor.PutBoolean("soundEnabled", enabled);
            _isSoundEnabled = enabled;
            _editor.Commit();
        }

        private void SaveRecords()
        {
            _editor.PutInt("record1Number", _records[0].QuestionNumber);
            _editor.PutString("record1Date", _records[0].RecordDate);
            _editor.PutInt("record2Number", _records[1].QuestionNumber);
            _editor.PutString("record2Date", _records[1].RecordDate);
            _editor.PutInt("record3Number", _records[2].QuestionNumber);
            _editor.PutString("record3Date", _records[2].RecordDate);
            _editor.PutInt("record4Number", _records[3].QuestionNumber);
            _editor.PutString("record4Date", _records[3].RecordDate);
            _editor.PutInt("record5Number", _records[4].QuestionNumber);
            _editor.PutString("record5Date", _records[4].RecordDate);
            _editor.PutInt("record6Number", _records[5].QuestionNumber);
            _editor.PutString("record6Date", _records[5].RecordDate);
            _editor.PutInt("record7Number", _records[6].QuestionNumber);
            _editor.PutString("record7Date", _records[6].RecordDate);

            _editor.Commit();
            //_editor.Apply();
        }

        public List<GameRecord> GetRecords()
        {
            return _records;
        }

        public int GetMillionsCount()
        {
            return _millionsCount;
        }

        public int GetEntersCount()
        {
            return _firstStarted;
        }

        public int GetCleverCount()
        {
            return _cleverCount;
        }

        public void ProcessRecord(int lastQuestion, bool gotMillion, bool gameWasLose, bool withoutHelp)
        {
            _questionNumbers.OrderByDescending(t => t);

            if (lastQuestion <= _questionNumbers[6])
                return;

            if (gotMillion)
            {
                _records.Insert(0, new GameRecord()
                {
                    QuestionNumber = 17,
                    RecordDate = DateTime.Now.ToString("dd/MM/yyyy")
                });

                ProcessMillionsCount();

                if (withoutHelp)
                    ProcessCleverCount();

                SaveRecords();
                return;
            }

            for (int i = 0; i < 7; i++)
            {
                if (ValuesConverter.LastQuestionToPoints(lastQuestion, gameWasLose) >= _questionNumbers[i])
                {
                    _records.Insert(i, new GameRecord()
                    {
                        QuestionNumber = ValuesConverter.LastQuestionToPoints(lastQuestion, gameWasLose),
                        RecordDate = DateTime.Now.ToString("dd/MM/yyyy")
                    });

                    SaveRecords();
                    return;
                }
            }
        }

        private void ProcessMillionsCount()
        {
            _millionsCount = _prefs.GetInt("millionsCount", 0);

            if (_editor == null)
                _editor = _prefs.Edit();

            _editor.PutInt("millionsCount", _millionsCount >= 50 ? 50 : _millionsCount + 1);
            _editor.Commit();
        }

        private void ProcessCleverCount()
        {
            _cleverCount = _prefs.GetInt("cleverCount", 0);

            if (_editor == null)
                _editor = _prefs.Edit();

            _editor.PutInt("cleverCount", _cleverCount >= 20 ? 20 : _cleverCount + 1);
            _editor.Commit();
        }
    }
}