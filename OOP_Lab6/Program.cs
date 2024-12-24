using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Lab6
{
    // Пользовательские исключения
    public class InvalidShapeException : Exception
    {
        public InvalidShapeException(string message) : base(message) { }
    }

    public class NullElementException : Exception
    {
        public NullElementException(string message) : base(message) { }
    }

    public class IndexOutOfRangeCustomException : Exception
    {
        public IndexOutOfRangeCustomException(string message) : base(message) { }
    }

    // Логгер
    public interface ILogger
    {
        void Log(string message, string type = "INFO");
    }

    public class FileLogger : ILogger
    {
        private readonly string _filePath;

        public FileLogger(string filePath)
        {
            _filePath = filePath;
        }

        public void Log(string message, string type = "INFO")
        {
            using (StreamWriter writer = new StreamWriter(_filePath, true))
            {
                writer.WriteLine($"{DateTime.Now:G}, {type}: {message}");
            }
        }
    }

    public class ConsoleLogger : ILogger
    {
        public void Log(string message, string type = "INFO")
        {
            Console.WriteLine($"{DateTime.Now:G}, {type}: {message}");
        }
    }

    public abstract class GeometricShape
    {
        public abstract double Area();
        public abstract double Perimeter();
    }

    public class Circle : GeometricShape
    {
        public double Radius { get; }

        public Circle(double radius)
        {
            if (radius <= 0)
                throw new InvalidShapeException("Радиус должен быть положительным.");
            Radius = radius;
        }

        public override double Area() => Math.PI * Radius * Radius;
        public override double Perimeter() => 2 * Math.PI * Radius;

        public override string ToString() => $"Круг: радиус = {Radius}";
    }

    public class Container<T>
    {
        private readonly List<T> _items = new List<T>();
        private readonly ILogger _logger;

        public Container(ILogger logger)
        {
            _logger = logger;
        }

        public void Add(T item)
        {
            if (item == null)
                throw new NullElementException("Попытка добавить null-элемент.");
            _items.Add(item);
            _logger.Log("Элемент добавлен в контейнер.");
        }

        public T Get(int index)
        {
            if (index < 0 || index >= _items.Count)
                throw new IndexOutOfRangeCustomException("Индекс вне диапазона.");
            return _items[index];
        }

        public void ShowAll()
        {
            foreach (var item in _items)
                Console.WriteLine(item.ToString());
        }

        public int Count => _items.Count;
    }

    class Program
    {
        static void Main()
        {
            ILogger logger = new ConsoleLogger();
            Container<GeometricShape> shapes = new Container<GeometricShape>(logger);

            try
            {
                shapes.Add(new Circle(5));
                shapes.Add(null); 
            }
            catch (NullElementException ex)
            {
                logger.Log(ex.Message, "ERROR");
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            catch (Exception ex)
            {
                logger.Log(ex.Message, "ERROR");
                Console.WriteLine($"Неизвестная ошибка: {ex.Message}");
            }

            try
            {
                if (shapes.Count <= 10)
                {
                    Console.WriteLine("Индекс 10 вне диапазона.");
                }
                else
                {
                    var shape = shapes.Get(10);
                }
            }
            catch (IndexOutOfRangeCustomException ex)
            {
                logger.Log(ex.Message, "ERROR");
                Console.WriteLine($"Ошибка: {ex.Message}");
            }

            try
            {
                GeometricShape invalidCircle = new Circle(-1);
            }
            catch (InvalidShapeException ex)
            {
                logger.Log(ex.Message, "ERROR");
                Console.WriteLine($"Ошибка: {ex.Message}");
            }

            Debug.Assert(shapes != null, "Контейнер не должен быть null.");
            Debug.Assert(shapes.Count > 0, "Контейнер не должен быть пустым.");

            try
            {
                throw new InvalidOperationException("Исключение для демонстрации.");
            }
            catch (InvalidOperationException ex)
            {
                logger.Log(ex.Message, "WARNING");
                Console.WriteLine($"Предупреждение: {ex.Message}");
                throw; 
            }
            catch (Exception ex) 
            {
                logger.Log(ex.Message, "ERROR");
                Console.WriteLine($"Неизвестная ошибка: {ex.Message}");
            }
            finally
            {
                logger.Log("Программа завершила выполнение.", "INFO");
                Console.WriteLine("Выполнение завершено.");
            }
        }
    }
}
