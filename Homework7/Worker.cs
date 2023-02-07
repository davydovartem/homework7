using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework7
{
    struct Worker
    {
        public int Id { get; private set; }
        public DateTime AdditionDate { get; private set; }
        public string Fullname { get; private set; }
        public int Age { get; private set; }
        public int Height { get; private set; }
        public DateTime BirthDate { get; private set; }
        public string BirthPlace { get; private set; }
        public Worker(int id, DateTime additionDate, string fullname, int age, int height, DateTime birthDate, string birthPlace)
        {
            Id = id;
            AdditionDate = additionDate;
            Fullname = fullname;
            Age = age;
            Height = height;
            BirthDate = birthDate;
            BirthPlace = birthPlace;
        }
        /// <summary>
        /// Вывод в форматированную строку для выода на экран
        /// </summary>
        /// <returns></returns>
        public string Print()
        {
            return $"{Id,-5}{AdditionDate,-25}{Fullname,-35}{Age,-10}{Height,-5}{BirthDate.Date,-15:d}{BirthPlace}\n";
        }
        /// <summary>
        /// Формирует строку для вывода в файл
        /// </summary>
        /// <returns></returns>
        public string PrintToFile()
        {
            return $"{Id}#{AdditionDate}#{Fullname}#{Age}#{Height}#{BirthDate.Date:d}#{BirthPlace}\n";
        }
    }

}
