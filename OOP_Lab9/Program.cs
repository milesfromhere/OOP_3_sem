using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TaskSolution
{
    public class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }

        // Конструктор
        public Book(string title, string author, int year)
        {
            Title = title;
            Author = author;
            Year = year;
        }

        public override string ToString()
        {
            return $"{Title} by {Author}, {Year}";
        }
    }

    public class BookManager
    {
        public List<Book> Books { get; set; }

        public BookManager()
        {
            Books = new List<Book>();
        }

        public void AddBook(Book book)
        {
            Books.Add(book);
        }

        public void RemoveBook(Book book)
        {
            Books.Remove(book);
        }

        public Book FindBook(string title)
        {
            return Books.Find(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        }

        public void PrintBooks()
        {
            foreach (var book in Books)
            {
                Console.WriteLine(book);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {


            Console.WriteLine("=== Задание 1 ===");
            BookManager bookManager = new BookManager();
            bookManager.AddBook(new Book("Книга A", "Автор 1", 2001));
            bookManager.AddBook(new Book("Книга B", "Автор 2", 2002));
            bookManager.AddBook(new Book("Книга C", "Автор 3", 2003));

            Console.WriteLine("Все книги:");
            bookManager.PrintBooks();

            Console.WriteLine("\nУдаление книги 'Книга B':");
            var bookToRemove = bookManager.FindBook("Книга B");
            bookManager.RemoveBook(bookToRemove);
            bookManager.PrintBooks();

            Console.WriteLine("\nПоиск книги 'Книга A':");
            var foundBook = bookManager.FindBook("Книга A");
            Console.WriteLine(foundBook != null ? foundBook.ToString() : "Книга не найдена");



            Console.WriteLine("\n=== Задание 2 ===");
            List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            Console.WriteLine("Начальная коллекция List<int>:");
            foreach (var num in numbers)
                Console.Write(num + " ");
            Console.WriteLine();

            Console.WriteLine("\nУдаление 3 элементов начиная с индекса 2:");
            numbers.RemoveRange(2, 3);
            foreach (var num in numbers)
                Console.Write(num + " ");
            Console.WriteLine();

            Console.WriteLine("\nДобавление новых элементов:");
            numbers.Add(11);
            numbers.Insert(0, 0);
            numbers.AddRange(new[] { 12, 13, 14 });
            foreach (var num in numbers)
                Console.Write(num + " ");
            Console.WriteLine();

            Console.WriteLine("\nСоздание второй коллекции Dictionary<int, int> из List<int>:");
            Dictionary<int, int> dictionary = new Dictionary<int, int>();
            for (int i = 0; i < numbers.Count; i++)
                dictionary[i] = numbers[i];

            Console.WriteLine("Вывод второй коллекции:");
            foreach (var kvp in dictionary)
                Console.WriteLine($"Ключ: {kvp.Key}, Значение: {kvp.Value}");

            Console.WriteLine("\nПоиск значения '5' во второй коллекции:");
            int searchValue = 5;
            bool found = dictionary.ContainsValue(searchValue);
            Console.WriteLine(found ? $"Значение {searchValue} найдено" : $"Значение {searchValue} не найдено");



            Console.WriteLine("\n=== Задание 3 ===");
            ObservableCollection<Book> observableBooks = new ObservableCollection<Book>();
            observableBooks.CollectionChanged += ObservableBooks_CollectionChanged;

            Console.WriteLine("Добавление книг в ObservableCollection:");
            observableBooks.Add(new Book("Observable Book 1", "Observer Author 1", 2021));
            observableBooks.Add(new Book("Observable Book 2", "Observer Author 2", 2022));

            Console.WriteLine("\nУдаление книги из ObservableCollection:");
            observableBooks.RemoveAt(0);
        }


        private static void ObservableBooks_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Console.WriteLine("Изменение в ObservableCollection:");
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (Book newBook in e.NewItems)
                {
                    Console.WriteLine($"Добавлена книга: {newBook}");
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (Book oldBook in e.OldItems)
                {
                    Console.WriteLine($"Удалена книга: {oldBook}");
                }
            }
        }
    }
}