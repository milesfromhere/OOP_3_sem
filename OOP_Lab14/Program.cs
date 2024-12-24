using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading;
using System.Threading.Tasks;

namespace Lab14_Threads
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Лабораторная работа №14. Работа с потоками выполнения\n");

            // Задание 1: Информация о процессах
            Task1_ShowProcesses();

            // Задание 2: Работа с доменом приложения
            Task2_WorkWithAppDomain();

            // Задание 3: Поток с расчетом простых чисел
            Task3_PrimeNumbersInThread();

            // Задание 4: Два потока - четные и нечетные числа
            Task4_TwoThreadsEvenOdd();

            // Задание 5: Повторяющаяся задача на основе Timer
            Task5_TimerTask();

            Console.WriteLine("Работа завершена.");
        }

        // Задание 1
        static void Task1_ShowProcesses()
        {
            Console.WriteLine("Задание 1: Вывод информации о процессах\n");

            foreach (var process in Process.GetProcesses())
            {
                try
                {
                    Console.WriteLine($"ID: {process.Id}, Имя: {process.ProcessName}, " +
                        $"Приоритет: {process.BasePriority}, " +
                        $"Время запуска: {process.StartTime}, " +
                        $"Состояние: {process.Responding}, " +
                        $"Время работы процессора: {process.TotalProcessorTime}");
                }
                catch
                {
                    // Игнорируем ошибки доступа к процессам
                }
            }

            Console.WriteLine();
        }

        // Задание 2
        static void Task2_WorkWithAppDomain()
        {
            Console.WriteLine("Задание 2: Работа с доменом приложения\n");

            // Текущий домен приложения
            AppDomain currentDomain = AppDomain.CurrentDomain;
            Console.WriteLine($"Имя домена: {currentDomain.FriendlyName}");
            Console.WriteLine($"Конфигурация: {currentDomain.SetupInformation.ApplicationBase}");

            Console.WriteLine("Загруженные сборки:");
            foreach (var assembly in currentDomain.GetAssemblies())
            {
                Console.WriteLine($"- {assembly.FullName}");
            }

            // Работа с новым контекстом загрузки сборок
            Console.WriteLine("\nСоздаем новый контекст загрузки сборки...");
            var context = new AssemblyLoadContext("НовыйКонтекст", true);

            // Загрузка сборки в новый контекст
            string assemblyPath = Path.Combine(Directory.GetCurrentDirectory(), "System.Text.Json.dll"); // Укажите реальный путь
            if (File.Exists(assemblyPath))
            {
                Assembly assembly = context.LoadFromAssemblyPath(assemblyPath);
                Console.WriteLine($"Сборка {assembly.FullName} загружена в новый контекст.");
            }
            else
            {
                Console.WriteLine("Сборка для загрузки не найдена.");
            }

            // Выгрузка контекста
            context.Unload();
            Console.WriteLine("Контекст выгружен.\n");
        }

        // Задание 3
        static void Task3_PrimeNumbersInThread()
        {
            Console.WriteLine("Задание 3: Поток с расчетом простых чисел\n");

            Console.Write("Введите n: ");
            int n = int.Parse(Console.ReadLine());

            Thread primeThread = new Thread(() => CalculateAndSavePrimes(n));
            primeThread.Name = "Поток_ПростыеЧисла";
            primeThread.Start();

            while (primeThread.IsAlive)
            {
                Console.WriteLine($"Статус потока: {primeThread.ThreadState}, Приоритет: {primeThread.Priority}, ID: {primeThread.ManagedThreadId}");
                Thread.Sleep(500);
            }

            Console.WriteLine("Поток завершен.\n");
        }

        static void CalculateAndSavePrimes(int n)
        {
            using (StreamWriter writer = new StreamWriter("primes.txt"))
            {
                for (int i = 2; i <= n; i++)
                {
                    if (IsPrime(i))
                    {
                        Console.WriteLine(i);
                        writer.WriteLine(i);
                        Thread.Sleep(100); // Имитация задержки
                    }
                }
            }
        }

        static bool IsPrime(int number)
        {
            if (number < 2) return false;
            for (int i = 2; i <= Math.Sqrt(number); i++)
            {
                if (number % i == 0) return false;
            }
            return true;
        }

        // Задание 4
        static void Task4_TwoThreadsEvenOdd()
        {
            Console.WriteLine("Задание 4: Два потока - четные и нечетные числа\n");

            Console.Write("Введите n: ");
            int n = int.Parse(Console.ReadLine());

            using (StreamWriter writer = new StreamWriter("even_odd.txt"))
            {
                object lockObject = new object();

                Thread evenThread = new Thread(() =>
                {
                    for (int i = 2; i <= n; i += 2)
                    {
                        lock (lockObject)
                        {
                            Console.WriteLine($"Четное: {i}");
                            writer.WriteLine($"Четное: {i}");
                        }
                        Thread.Sleep(300); // Разная скорость
                    }
                });

                Thread oddThread = new Thread(() =>
                {
                    for (int i = 1; i <= n; i += 2)
                    {
                        lock (lockObject)
                        {
                            Console.WriteLine($"Нечетное: {i}");
                            writer.WriteLine($"Нечетное: {i}");
                        }
                        Thread.Sleep(500); // Разная скорость
                    }
                });

                evenThread.Priority = ThreadPriority.Highest; // Высокий приоритет
                evenThread.Start();
                oddThread.Start();

                evenThread.Join();
                oddThread.Join();
            }

            Console.WriteLine("Числа записаны в файл и выведены на консоль.\n");
        }

        // Задание 5
        static void Task5_TimerTask()
        {
            Console.WriteLine("Задание 5: Повторяющаяся задача на основе Timer\n");

            Timer timer = new Timer(PrintTime, null, 0, 1000);

            Console.WriteLine("Нажмите Enter для завершения таймера...");
            Console.ReadLine();
            timer.Dispose();

            Console.WriteLine("Таймер завершен.\n");
        }

        static void PrintTime(object state)
        {
            Console.WriteLine($"Текущее время: {DateTime.Now}");
        }
    }
}
