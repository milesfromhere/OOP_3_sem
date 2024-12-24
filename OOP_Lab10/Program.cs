using System;
using System.Collections.Generic;
using System.Linq;
/*
class Program
{
    static void Main()
    {
        // Массив месяцев
        string[] months = {
            "Июнь",
            "Июль",
            "Май",
            "Декабрь",
            "Январь",
            "Февраль",
            "Март",
            "Апрель",
            "Август",
            "Сентябрь",
            "Октябрь",
            "Ноябрь"
        };

        int n = 4;
        var monthsWithLengthN = months.Where(m => m.Length == n);
        Console.WriteLine("Месяцы с длиной больше чем " + n + ": " + string.Join(", ", monthsWithLengthN));

        var summerAndWinterMonths = months.Where(m => new[] { "Июнь", "Июль", "Август", "Декабрь", "Январь", "Февраль" }.Contains(m));
        Console.WriteLine("Летние и зимние месяцы: " + string.Join(", ", summerAndWinterMonths));

        var sortedMonths = months.OrderBy(m => m);
        Console.WriteLine("Месяцы в алфавитном порядке: " + string.Join(", ", sortedMonths));

        var filteredMonths = months.Where(m => m.Contains('а') && m.Length >= 4);
        Console.WriteLine("Месяцы, содержащие 'а' и имеющие длину >= 4:\n" + string.Join(", ", filteredMonths));

        // Коллекция
        var flights = new List<Flight>
        {
            new Flight("Минск", "Лондон", "Вторник", new TimeSpan(9, 0, 0)),
            new Flight("Минск", "Берлин", "Среда", new TimeSpan(14, 30, 0)),
            new Flight("Лондон", "Париж", "Пятница", new TimeSpan(7, 15, 1)),
            new Flight("Минск", "Варшава", "Понедельник", new TimeSpan(6, 50, 0)),
            new Flight("Минск", "Москва", "Воскресенье", new TimeSpan(12, 0, 0)),
            new Flight("Варшава", "Берлин", "Четверг", new TimeSpan(16, 20, 0)),
            new Flight("Берлин", "Минск", "Вторник", new TimeSpan(22, 10, 0)),
            new Flight("Минск", "Париж", "Пятница", new TimeSpan(19, 5, 0)),
            new Flight("Париж", "Минск", "Среда", new TimeSpan(20, 0, 0)),
            new Flight("Москва", "Минск", "Понедельник", new TimeSpan(4, 45, 0))
        };

        var day = "Понедельник";
        var flightsOnDay = flights.Count(f => f.Day == day);
        Console.WriteLine($"Количество рейсов в {day}: {flightsOnDay}");

        var earliestMondayFlight = flights.Where(f => f.Day == "Понедельник").OrderBy(f => f.Time).FirstOrDefault();
        Console.WriteLine("Ранний вылет в понедельник: " + earliestMondayFlight);

        var latestMidweekFlight = flights
            .Where(f => f.Day == "Среда" || f.Day == "Пятница")
            .OrderByDescending(f => f.Time)
            .FirstOrDefault();
        Console.WriteLine("Последний вылет в среду или пятницу: " + latestMidweekFlight);

        var sortedFlights = flights.OrderBy(f => f.Time);
        Console.WriteLine("Полёты отсортированы по времени:");
        foreach (var flight in sortedFlights)
        {
            Console.WriteLine(flight);
        }

        //запрос 
        var complexQuery = flights
            .Where(f => f.Destination == "Минск")
            .OrderBy(f => f.Time)
            .Select(f => new { f.Origin, f.Time })
            .GroupBy(f => f.Time.Hours >= 12 ? "Вечер" : "Утро")
            .Select(g => $"{g.Key}: {g.Count()} полётов");

        Console.WriteLine("Собственный запрос:");
        foreach (var line in complexQuery)
        {
            Console.WriteLine(line);
        }
    }

    public class Flight
    {
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string Day { get; set; }
        public TimeSpan Time { get; set; }

        public Flight(string origin, string destination, string day, TimeSpan time)
        {
            Origin = origin;
            Destination = destination;
            Day = day;
            Time = time;
        }

        public override string ToString()
        {
            return $"Полёт из {Origin} в {Destination}, {Day}, в {Time}";
        }
    }
}*/
//массив строк слово длинной 5, найти с буквой а
class Program
{
    static void Main()
    {
string[] words = { "apple", "grape", "stone","wood","bread" };
string[] filteredWords = words.Where(word => word.Length == 5 && word.ToLower().Contains('a')).ToArray();

foreach (string word in filteredWords)
   {
    Console.WriteLine(word);
    }
}
}