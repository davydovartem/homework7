using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Homework7
{
    class Repository
    {
        //массив с сотдрудниками
        private List<Worker> _workers = new List<Worker>();
        //массив с добавленными в процессе работы программы сотрудниками
        private List<Worker> _addedWorkers = new List<Worker>();
        //флаг удаления сотрудника в процессе работы программы
        private bool _workerDeleted = false;
        //имя файла, из которого загружаются и в который сохраняются сотрудники
        private string _fileName;
        public Repository(string fileName)
        {
            _fileName = fileName;
            _workers = GetAllWorkers();
        }
        #region Methods
        private int GetMaxId()
        {
            return _workers.Max(worker => worker.Id);
        }
        private List<Worker> GetAllWorkers()
        {
            // здесь происходит чтение из файла
            // и возврат массива считанных экземпляров
            var fileLines = File.ReadAllLines(_fileName);
            foreach (string v in fileLines)
            {
                var fLine = v.Split('#');
                _workers.Add(new Worker(Convert.ToInt32(fLine[0]),
                                        DateTime.Parse(fLine[1]),
                                        fLine[2],
                                        Convert.ToInt32(fLine[3]),
                                        Convert.ToInt32(fLine[4]),
                                        DateTime.Parse(fLine[5]),
                                        fLine[6]));
            }
            return _workers;
        }
        /// <summary>
        /// возвращается Worker с запрашиваемым ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Worker GetWorkerById(int id)
        {
            return _workers.Find(worker => worker.Id == id);
        }
        /// <summary>
        /// Удаляет сотрудника из списка _workers, если будет найден соответствующий Id
        /// </summary>
        /// <param name="id"></param>
        public bool DeleteWorker(int id)
        {
            return _workerDeleted = _workers.Remove(GetWorkerById(id));
        }
        /// <summary>
        /// присваиваем worker уникальный ID, добавляем нового worker в список _workers
        /// </summary>
        public void AddWorker()
        {
            Console.Write("\nВведите Ф.И.О.: ");
            string fio = Console.ReadLine();
            Console.Write("\nВведите возраст: ");
            int age = Program.GetIntegerFromConsole();
            Console.Write("\nВведите рост: ");
            int height = Program.GetIntegerFromConsole();
            Console.Write("\nВведите дату рождения в формате ДД.ММ.ГГГГ: ");
            DateTime birthDate = Program.GetDateTimeFromConsole();
            Console.Write("\nВведите место рождения: ");
            string birthPlace = Console.ReadLine();

            Worker worker = new Worker(this.GetMaxId() + 1, DateTime.Now, fio, age, height, birthDate, birthPlace);
            _workers.Add(worker);
            _addedWorkers.Add(worker);
        }
        /// <summary>
        /// Фильтрация записей и возврат массива считанных экземпляров
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        public List<Worker> FilterWorkers(int? fldNumber, object fldFrom, object fldTo)
        {
            switch (fldNumber)
            {
                case 1:
                    return (_workers.Where(w => w.Id <= (int)fldTo && w.Id >= (int)fldFrom)).ToList();
                case 2:
                    return (_workers.Where(w => w.AdditionDate <= (DateTime)fldTo && w.AdditionDate >= (DateTime)fldFrom)).ToList();
                case 3:
                    return null;
                case 4:
                    return (_workers.Where(w => w.Age <= (int)fldTo && w.Age >= (int)fldFrom)).ToList();
                case 5:
                    return (_workers.Where(w => w.Height <= (int)fldTo && w.Height >= (int)fldFrom)).ToList();
                case 6:
                    return (_workers.Where(w => w.BirthDate <= (DateTime)fldTo && w.BirthDate >= (DateTime)fldFrom)).ToList();
                case 7:
                    return null;
                default:
                    return null;
            }
        }
        /// <summary>
        /// Формирует таблицу записей о сотрудниках в форматированную строку
        /// </summary>
        /// <param name="sortField">Необязательный параметр, определяющий поле для сортировки, от 1 до 7</param>
        /// <param name="filteredWorkers">Необязательный параметр, если не задан, то выводится весь список _workers</param>
        /// <returns></returns>
        public string Print(int sortField = 1, List<Worker> filteredWorkers = null)
        {
            if (filteredWorkers == null) filteredWorkers = _workers;
            IOrderedEnumerable<Worker> sortedWorkers = null;
            switch (sortField)
            {
                case 1:
                    sortedWorkers = filteredWorkers.OrderBy(w => w.Id);
                    break;
                case 2:
                    sortedWorkers = filteredWorkers.OrderBy(w => w.AdditionDate);
                    break;
                case 3:
                    sortedWorkers = filteredWorkers.OrderBy(w => w.Fullname);
                    break;
                case 4:
                    sortedWorkers = filteredWorkers.OrderBy(w => w.Age);
                    break;
                case 5:
                    sortedWorkers = filteredWorkers.OrderBy(w => w.Height);
                    break;
                case 6:
                    sortedWorkers = filteredWorkers.OrderBy(w => w.BirthDate);
                    break;
                case 7:
                    sortedWorkers = filteredWorkers.OrderBy(w => w.BirthPlace);
                    break;
                default:
                    break;
            }
            string header = $"{"ID",-5}{"Дата и время добавления",-25}{"Ф.И.О.",-35}{"Возраст",-10}{"Рост",-5}"
                                            + $"{"Дата рождения",-15}{"Место рождения"}\n";
            string s = header;
            foreach (var w in sortedWorkers)
            {
                s += w.Print();
            }
            return s;
        }
        /// <summary>
        /// Сохраняет файл
        /// </summary>
        /// <param name="append">Если true - добавляет к текущему файлу новые записи из списка _addedWorkers</param>
        public void SaveRepository(bool append = false)
        {
            var sb = new StringBuilder();
            //Если было удаление, то все равно нужно переписывать весь файл
            if (append && !_workerDeleted)
            {
                foreach (var w in _addedWorkers)
                {
                    sb.Append(w.PrintToFile());
                }
                File.AppendAllText(_fileName, sb.ToString());
            }
            else
            {
                foreach (var w in _workers)
                {
                    sb.Append(w.PrintToFile());
                }
                File.WriteAllText(_fileName, sb.ToString());
            }
        }
        #endregion
    }

}
