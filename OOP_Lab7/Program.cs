using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Lab7
{
    public interface ICollectionOperations<T>
    {
        void Add(T item);
        void Remove(T item);
        IEnumerable<T> View();
        T Find(Predicate<T> predicate);
    }

    public class CollectionType<T> : ICollectionOperations<T> where T : IComparable
    {
        private readonly List<T> _collection;

        public CollectionType()
        {
            _collection = new List<T>();
        }

        public void Add(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item), "Элемент не может быть null.");
            _collection.Add(item);
        }

        public void Remove(T item)
        {
            if (!_collection.Remove(item))
                Console.WriteLine("Элемент не найден в коллекции.");
        }

        public IEnumerable<T> View()
        {
            return _collection;
        }

        public T Find(Predicate<T> predicate)
        {
            return _collection.Find(predicate) ?? throw new InvalidOperationException("Элемент не найден.");
        }

        public void SaveToFile(string filePath)
        {
            try
            {
                string json = JsonSerializer.Serialize(_collection, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePath, json);
                Console.WriteLine("Данные успешно сохранены в файл.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении в файл: {ex.Message}");
            }
        }

        public void LoadFromFile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    throw new FileNotFoundException("Файл не найден.");

                string json = File.ReadAllText(filePath);
                var data = JsonSerializer.Deserialize<List<T>>(json);
                if (data != null)
                    _collection.AddRange(data);
                Console.WriteLine("Данные успешно загружены из файла.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке из файла: {ex.Message}");
            }
        }
    }

    // Пользовательский класс Лаб 4
    public class Developer : IComparable
    {
        public string FullName { get; set; }
        public int Id { get; set; }
        public string Department { get; set; }

        public Developer(string fullName, int id, string department)
        {
            FullName = fullName;
            Id = id;
            Department = department;
        }

        public int CompareTo(object obj)
        {
            if (obj is Developer other)
                return Id.CompareTo(other.Id);
            throw new ArgumentException("Сравнение возможно только с другим объектом Developer.");
        }

        public override string ToString()
        {
            return $"{FullName} (ID: {Id}), Department: {Department}";
        }
    }

    class Program
    {
        static void Main()
        {
            CollectionType<int> intCollection = new CollectionType<int>();
            intCollection.Add(5);
            intCollection.Add(10);
            intCollection.Add(15);

            Console.WriteLine("Коллекция целых чисел:");
            foreach (var item in intCollection.View())
                Console.WriteLine(item);

            intCollection.Remove(10);
            Console.WriteLine("\nПосле удаления 10:");
            foreach (var item in intCollection.View())
                Console.WriteLine(item);

            int found = intCollection.Find(x => x > 5);
            Console.WriteLine($"\nНайдено число больше 5: {found}");

            intCollection.SaveToFile("intCollection.json");
            intCollection.LoadFromFile("intCollection.json");

            CollectionType<Developer> developerCollection = new CollectionType<Developer>();
            developerCollection.Add(new Developer("Вася Петькин", 1, "Тестировщик"));
            developerCollection.Add(new Developer("Иван Иванов", 2, "Разработчик"));

            Console.WriteLine("\nКоллекция разработчиков:");
            foreach (var dev in developerCollection.View())
                Console.WriteLine(dev);

            Developer foundDev = developerCollection.Find(d => d.FullName.Contains("Иван"));
            Console.WriteLine($"\nНайден разработчик: {foundDev}");

            developerCollection.SaveToFile("developerCollection.json");
            developerCollection.LoadFromFile("developerCollection.json");
        }
    }
}

