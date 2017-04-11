using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BM.Droid
{
    [Table(nameof(questions))]
    public class questions
    {
        /// <summary>
        /// ИД вопроса в базе
        /// </summary>
        [PrimaryKey, AutoIncrement, Column(nameof(_id))]
        public int _id { get; set; }

        /// <summary>
        /// Уровень сложности вопроса
        /// </summary>
        [Column(nameof(level))]
        public int level { get; set; }

        /// <summary>
        /// Текст вопроса
        /// </summary>
        [Column(nameof(questionText))]
        public string questionText { get; set; }

        /// <summary>
        /// Вариант 1
        /// </summary>
        [Column(nameof(variant1))]
        public string variant1 { get; set; }

        /// <summary>
        /// Вариант 2
        /// </summary>
        [Column(nameof(variant2))]
        public string variant2 { get; set; }

        /// <summary>
        /// Вариант 3
        /// </summary>
        [Column(nameof(variant3))]
        public string variant3 { get; set; }

        /// <summary>
        /// Вариант 4
        /// </summary>
        [Column(nameof(variant4))]
        public string variant4 { get; set; }

        /// <summary>
        /// Правильный ответ
        /// </summary>
        [Column(nameof(answer))]
		public int answer { get; set; }

        /// <summary>
        /// Текст из Библии
        /// </summary>
        [Column(nameof(bibleText))]
        public string bibleText { get; set; }
    }
}
