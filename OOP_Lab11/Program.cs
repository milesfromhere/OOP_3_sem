using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace LabReflection
{
    public static class Reflector
    {
        // a. Определение имени сборки, в которой определен класс
        public static string GetAssemblyName(string className)// тот метод принимает название класса (className) в виде строки.
        {
            Type type = Type.GetType(className);
            return type?.Assembly.FullName ?? "Класс не найден";
        }

        // b. Проверка, есть ли публичные конструкторы
        public static bool HasPublicConstructors(string className)
        {
            Type type = Type.GetType(className);
            return type?.GetConstructors().Any(c => c.IsPublic) ?? false;
        }

        // c. Извлечение всех общедоступных публичных методов класса
        public static IEnumerable<string> GetPublicMethods(string className)
        {
            Type type = Type.GetType(className);
            return type?.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                        .Select(m => m.Name) ?? Enumerable.Empty<string>();
        }

        // d. Получение информации о полях и свойствах класса
        public static IEnumerable<string> GetFieldsAndProperties(string className)
        {
            Type type = Type.GetType(className);
            List<string> fieldsAndProperties = new List<string>();

            if (type != null)
            {
                fieldsAndProperties.AddRange(type.GetFields(BindingFlags.Public | BindingFlags.Instance)
                                                 .Select(f => f.Name));
                fieldsAndProperties.AddRange(type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                                 .Select(p => p.Name));
            }

            return fieldsAndProperties;
        }

        // e. Получение всех реализованных классом интерфейсов
        public static IEnumerable<string> GetImplementedInterfaces(string className)
        {
            Type type = Type.GetType(className);
            return type?.GetInterfaces().Select(i => i.Name) ?? Enumerable.Empty<string>();
        }

        // f. Получение методов с заданным типом параметра
        public static IEnumerable<string> GetMethodsWithParameter(string className, Type parameterType)
        {
            Type type = Type.GetType(className);
            return type?.GetMethods().Where(m => m.GetParameters().Any(p => p.ParameterType == parameterType))
                                     .Select(m => m.Name) ?? Enumerable.Empty<string>();
        }

        // g. Метод Invoke
        public static void Invoke(string className, string methodName, StreamWriter writer)
        {
            // Чтение параметров из файла
            string[] parameters;
            using (StreamReader reader = new StreamReader($"{className}.txt"))
            {
                parameters = reader.ReadLine()?.Split(',') ?? new string[0];
            }

            Type type = Type.GetType(className);
            if (type == null)
            {
                writer.WriteLine($"Класс {className} не найден.");
                return;
            }

            MethodInfo method = type.GetMethod(methodName);
            if (method == null)
            {
                writer.WriteLine($"Метод {methodName} не найден в классе {className}.");
                return;
            }

            object instance = Activator.CreateInstance(type);
            ParameterInfo[] paramInfos = method.GetParameters();
            object[] paramValues = new object[paramInfos.Length];

            for (int i = 0; i < paramInfos.Length; i++)
            {
                paramValues[i] = GenerateValue(paramInfos[i].ParameterType, parameters[i]);
            }

            method.Invoke(instance, paramValues);
            writer.WriteLine($"Метод {methodName} успешно вызван в классе {className}.");
        }

        private static object GenerateValue(Type type, string parameter)
        {
            if (type == typeof(int))
            {
                return int.TryParse(parameter, out int result) ? result : new Random().Next(1, 100);
            }
            else if (type == typeof(string))
            {
                return string.IsNullOrEmpty(parameter) ? "DefaultString" : parameter;
            }
            else if (type == typeof(double))
            {
                return double.TryParse(parameter, out double result) ? result : new Random().NextDouble();
            }
            return null;
        }

        // 2. Обобщенный метод Create
        public static T Create<T>() where T : new()
        {
            return new T();
        }
    }

    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public Person() { }

        public Person(string name, int age)
        {
            Name = name;
            Age = age;
        }

        public void Walk()
        {
            Console.WriteLine($"{Name} идёт.");
        }

        public void Eat(string food)
        {
            Console.WriteLine($"{Name} ест {food}.");
        }
    }

    public class Circle
    {
        public double Radius { get; set; }

        public Circle() { }

        public Circle(double radius)
        {
            Radius = radius;
        }

        public void Area()
        {
            Console.WriteLine($"Площадь круга: {Math.PI * Radius * Radius}");
        }

        public void Perimeter()
        {
            Console.WriteLine($"Периметр круга: {2 * Math.PI * Radius}");
        }
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            // Создаем StreamWriter для записи в файл
            using (StreamWriter writer = new StreamWriter("output.txt", false)) // false - перезаписывает файл
            {
                // Использование Reflector для исследования классов Person и Circle
                writer.WriteLine("Assembly Name:");
                writer.WriteLine(Reflector.GetAssemblyName("LabReflection.Person"));

                writer.WriteLine("\nHas Public Constructors:");
                writer.WriteLine(Reflector.HasPublicConstructors("LabReflection.Person"));

                writer.WriteLine("\nPublic Methods:");
                foreach (var method in Reflector.GetPublicMethods("LabReflection.Person"))
                {
                    writer.WriteLine(method);
                }

                writer.WriteLine("\nFields and Properties:");
                foreach (var fieldOrProperty in Reflector.GetFieldsAndProperties("LabReflection.Circle"))
                {
                    writer.WriteLine(fieldOrProperty);
                }

                writer.WriteLine("\nImplemented Interfaces:");
                foreach (var implementedInterface in Reflector.GetImplementedInterfaces("LabReflection.Person"))
                {
                    writer.WriteLine(implementedInterface);
                }

                writer.WriteLine("\nMethods with String Parameter:");
                foreach (var method in Reflector.GetMethodsWithParameter("LabReflection.Person", typeof(string)))
                {
                    writer.WriteLine(method);
                }

                // Демонстрация работы метода Invoke
                Reflector.Invoke("LabReflection.Person", "Eat", writer);

                // Создание объекта с использованием обобщенного метода Create
                var person = Reflector.Create<Person>();
                person.Name = "Никита";
                person.Age = 21;
                person.Walk();
                writer.WriteLine($"Создан человек: {person.Name}, {person.Age} лет");
            }

            // Если нужно, можно прочитать содержимое файла и вывести его на консоль
            using (StreamReader reader = new StreamReader("output.txt"))
            {
                string text = reader.ReadToEnd();
                Console.WriteLine("Содержимое файла output.txt:");
                Console.WriteLine(text);
            }
        }
    }
}