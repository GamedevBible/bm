using Java.IO;

namespace BM
{
    public class Question : Java.Lang.Object, ISerializable
    {
        public int Id { get; set; }

        public int Level { get; set; }

        public string QuestionText { get; set; }

        public int Answer { get; set; }

        public string Variant1 { get; set; }

        public string Variant2 { get; set; }

        public string Variant3 { get; set; }

        public string Variant4 { get; set; }
    }
}
