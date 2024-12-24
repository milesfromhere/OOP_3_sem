using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Лабораторная работа №15: Платформа параллельных вычислений\n");

        Task1_LongRunningTask();

        Task2_WithCancellationToken();

        Task3_TasksWithResults();

        Task4_ContinuationTask();

        Task5_ParallelForAndForEach();

        Task6_ParallelInvoke();

        Task7_BlockingCollection();

        await Task8_AsyncAwait();

        Console.WriteLine("\nРабота завершена.");
    }

    // Задание 1: Длительная задача на основе Task
    static void Task1_LongRunningTask()
    {
        Console.WriteLine("Задание 1: Длительная задача на основе Task (поиск простых чисел)\n");

        int limit = 100000;
        var stopwatch = Stopwatch.StartNew();

        Task task = Task.Run(() =>
        {
            Console.WriteLine($"Идентификатор задачи: {Task.CurrentId}");
            var primes = SieveOfEratosthenes(limit);
            Console.WriteLine($"Найдено {primes.Length} простых чисел до {limit}.");
        });

        while (!task.IsCompleted)
        {
            Console.WriteLine($"Статус задачи: {task.Status}");
            Thread.Sleep(100);
        }

        stopwatch.Stop();
        Console.WriteLine($"Задача завершена. Время выполнения: {stopwatch.ElapsedMilliseconds} мс.\n");
    }

    static int[] SieveOfEratosthenes(int limit)
    {
        bool[] isPrime = Enumerable.Repeat(true, limit + 1).ToArray();
        isPrime[0] = isPrime[1] = false;

        for (int i = 2; i * i <= limit; i++)
        {
            if (isPrime[i])
            {
                for (int j = i * i; j <= limit; j += i)
                {
                    isPrime[j] = false;
                }
            }
        }

        return Enumerable.Range(0, limit + 1).Where(i => isPrime[i]).ToArray();
    }

    // Задание 2: Задача с токеном отмены
    static void Task2_WithCancellationToken()
    {
        Console.WriteLine("Задание 2: Задача с токеном отмены\n");

        CancellationTokenSource cts = new CancellationTokenSource();

        Task task = Task.Run(() =>
        {
            try
            {
                for (int i = 1; i <= 100; i++)
                {
                    cts.Token.ThrowIfCancellationRequested();
                    Console.WriteLine($"Итерация {i}");
                    Thread.Sleep(50);
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Задача отменена.");
            }
        }, cts.Token);

        Thread.Sleep(500);
        cts.Cancel(); // Отмена задачи
        task.Wait();

        Console.WriteLine("Задача завершена.\n");
    }

    // Задание 3: Задачи с результатами
    static void Task3_TasksWithResults()
    {
        Console.WriteLine("Задание 3: Задачи с возвратом результатов\n");

        Task<int> task1 = Task.Run(() => 10 + 20);
        Task<int> task2 = Task.Run(() => 30 * 2);
        Task<int> task3 = Task.Run(() => 100 - 50);

        Task fourthTask = Task.WhenAll(task1, task2, task3).ContinueWith(prevTasks =>
        {
            int sum = task1.Result + task2.Result + task3.Result;
            Console.WriteLine($"Сумма результатов: {sum}");
        });

        fourthTask.Wait();
        Console.WriteLine();
    }

    // Задание 4: Continuation Task
    static void Task4_ContinuationTask()
    {
        Console.WriteLine("Задание 4: Задача продолжения\n");

        Task firstTask = Task.Run(() =>
        {
            Console.WriteLine("Первая задача выполняется...");
            Thread.Sleep(500);
        });

        Task continuationTask = firstTask.ContinueWith(t =>
        {
            Console.WriteLine("Задача продолжения: завершение первой задачи.");
        });

        continuationTask.Wait();
        Console.WriteLine();
    }

    // Задание 5: Parallel.For и Parallel.ForEach
    static void Task5_ParallelForAndForEach()
    {
        Console.WriteLine("Задание 5: Parallel.For и Parallel.ForEach\n");

        int[] array = Enumerable.Range(1, 1000000).ToArray();

        Stopwatch stopwatch = Stopwatch.StartNew();
        Parallel.For(0, array.Length, i =>
        {
            array[i] *= 2;
        });
        stopwatch.Stop();
        Console.WriteLine($"Parallel.For: {stopwatch.ElapsedMilliseconds} мс.");

        stopwatch.Restart();
        for (int i = 0; i < array.Length; i++)
        {
            array[i] *= 2;
        }
        stopwatch.Stop();
        Console.WriteLine($"Обычный For: {stopwatch.ElapsedMilliseconds} мс.\n");
    }

    // Задание 6: Parallel.Invoke
    static void Task6_ParallelInvoke()
    {
        Console.WriteLine("Задание 6: Parallel.Invoke\n");

        Parallel.Invoke(
            () => Console.WriteLine($"Задача 1 выполняется потоком {Task.CurrentId}"),
            () => Console.WriteLine($"Задача 2 выполняется потоком {Task.CurrentId}"),
            () => Console.WriteLine($"Задача 3 выполняется потоком {Task.CurrentId}")
        );

        Console.WriteLine();
    }

    // Задание 7: BlockingCollection
    static void Task7_BlockingCollection()
    {
        Console.WriteLine("Задание 7: BlockingCollection\n");

        BlockingCollection<string> warehouse = new BlockingCollection<string>();
        CancellationTokenSource cts = new CancellationTokenSource();

        // Поставщики
        Task[] suppliers = Enumerable.Range(1, 5).Select(i => Task.Run(() =>
        {
            while (!cts.Token.IsCancellationRequested)
            {
                string item = $"Товар от поставщика {i}";
                warehouse.Add(item);
                Console.WriteLine($"Поставщик {i} завез: {item}");
                Thread.Sleep(100 * i); // Имитация разной скорости
            }
        }, cts.Token)).ToArray();

        // Покупатели
        Task[] customers = Enumerable.Range(1, 10).Select(i => Task.Run(() =>
        {
            while (!cts.Token.IsCancellationRequested)
            {
                if (warehouse.TryTake(out string item, 500))
                {
                    Console.WriteLine($"Покупатель {i} купил: {item}");
                }
                else
                {
                    Console.WriteLine($"Покупатель {i} ушел без покупки.");
                }
            }
        }, cts.Token)).ToArray();

        // Ожидание выполнения в течение 5 секунд
        Task.Delay(5000).Wait();

        // Завершаем добавление товаров
        cts.Cancel();
        warehouse.CompleteAdding();

        // Ожидаем завершения задач
        Task.WaitAll(suppliers);
        Task.WaitAll(customers);

        Console.WriteLine("Все задачи завершены.\n");
    }
    // Задание 8: async/await
    static async Task Task8_AsyncAwait()
    {
        Console.WriteLine("Задание 8: async/await\n");

        int result = await Task.Run(() => SlowOperation());
        Console.WriteLine($"Результат асинхронной операции: {result}\n");
    }

    static int SlowOperation()
    {
        Thread.Sleep(2000);
        return 42;
    }
}
