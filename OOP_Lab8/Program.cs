using System;
using System.Collections.Generic;
using System.Linq;

namespace Program
{
    public class User
    {
        public string Name { get; set; }
        public double Position { get; set; } 
        public double Size { get; set; }     

        public User(string name, double initialPosition, double initialSize)
        {
            Name = name;
            Position = initialPosition;
            Size = initialSize;
        }

        public event Action<double>? Move;  
        public event Action<double>? Compress;

        public void OnMove(double offset)
        {
        Move?.Invoke(offset);
            CheckState("Перемещение");
        }

        private void CheckState(string eventType)
        {
            Console.WriteLine($"[{eventType}] Состояние пользователя {Name}: Позиция = {Position}, Размер = {Size}" );
        }

        public void OnCompress(double factor)
        {
            Compress?.Invoke(factor);
            CheckState("Сжатие");
        }

        public void ApplyMove(double offset) => Position += offset;
        public void ApplyCompress(double factor) => Size *= factor;

        public override string ToString()
        {
            return $"{Name} | Позиция: {Position}, Размер: {Size}";
        }
    }

    class Program
    {
        public static void Main(string[] args)
        {
            var user1 = new User("Иван", 0, 100);
            var user2 = new User("Мария", 5, 150);
            var user3 = new User("Андрей", 10, 200);

            user1.Move += user1.ApplyMove;
            user1.Compress += user1.ApplyCompress;
            user2.Move += user2.ApplyMove; 
            user3.Move += user3.ApplyMove; 
            user3.Compress += user3.ApplyCompress; 

            Console.WriteLine("Начальные состояния:\n");
            Console.WriteLine(user1);
            Console.WriteLine(user2);
            Console.WriteLine(user3);

            Console.WriteLine("\nПеремещение на 10:\n");
            user1.OnMove(10);
            user2.OnMove(10);
            user3.OnMove(10);
            Console.WriteLine(user1);
            Console.WriteLine(user2);
            Console.WriteLine(user3);

            Console.WriteLine("\nСжатие в 2 раза:");
            user1.OnCompress(0.5);
            user2.OnCompress(0.5); 
            user3.OnCompress(0.5);


            Console.WriteLine("Итоговые состояния:\n");
            Console.WriteLine(user1);
            Console.WriteLine(user2);
            Console.WriteLine(user3);

            Func<string, string> removePunctuation = s => string.Join("", s.Where(c => !char.IsPunctuation(c)));
            Func<string, string> toUpperCase = s => s.ToUpper();
            Func<string, string> trimSpaces = s => s.Trim();
            Func<string, string> removeExtraSpaces = s => string.Join(" ", s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));

            string input = "  Привет, мир! Это           пример строки. ";

            var processors = new List<Func<string, string>>
            {
                removePunctuation,
                trimSpaces,
                removeExtraSpaces,
                toUpperCase,
            };

            string result = input;
            foreach (var processor in processors)
            {
                result = processor(result);
            }
            
            Console.WriteLine($"\nИсходная строка: \"{input}\"");
            Console.WriteLine($"Обработанная строка: \"{result}\"");

        }
    }
}
