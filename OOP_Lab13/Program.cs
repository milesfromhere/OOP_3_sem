using System;
using System.IO;
using System.Text.Json;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace OOP_Lab13
{
    // Общий интерфейс для сериализаторов
    public interface IAVFSerializer
    {
        void Serialize<T>(T obj, string filePath);
        T Deserialize<T>(string filePath);
    }

    // JSON
    public class AVFJsonSerializer : IAVFSerializer
    {
        public void Serialize<T>(T obj, string filePath)
        {
            var options = new JsonSerializerOptions { WriteIndented = true }; // Форматирование
            var json = JsonSerializer.Serialize(obj, options);
            File.WriteAllText(filePath, json); // Запись JSON в файл
        }

        public T Deserialize<T>(string filePath)
        {
            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<T>(json);
        }
    }

    // XML
    public class AVFXmlSerializer : IAVFSerializer
    {
        public void Serialize<T>(T obj, string filePath)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, obj);
            }
        }

        public T Deserialize<T>(string filePath)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var reader = new StreamReader(filePath))
            {
                return (T)serializer.Deserialize(reader);
            }
        }
    }

    // Binary
    public class AVFDataContractSerializer : IAVFSerializer
    {
        public void Serialize<T>(T obj, string filePath)
        {
            var serializer = new DataContractSerializer(typeof(T));
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                serializer.WriteObject(stream, obj);
            }
        }

        public T Deserialize<T>(string filePath)
        {
            var serializer = new DataContractSerializer(typeof(T));
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                return (T)serializer.ReadObject(stream);
            }
        }
    }

    // Основной класс
    [DataContract]
    public class AVFPerson
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int Age { get; set; }

        [IgnoreDataMember] // Запрещаем сериализацию этого поля
        public string Secret { get; set; }

        public AVFPerson() { }

        public AVFPerson(string name, int age, string secret)
        {
            Name = name;
            Age = age;
            Secret = secret;
        }

        public override string ToString()
        {
            return $"Имя: {Name}, Возраст: {Age}, Скрытая информация: {Secret}";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Объект для теста
            var person = new AVFPerson("Никита", 21, "секрет");

            // Тест JSON
            TestSerializer(new AVFJsonSerializer(), person, "person.json");

            // Тест XML
            TestSerializer(new AVFXmlSerializer(), person, "person.xml");

            // Тест Binary
            TestSerializer(new AVFDataContractSerializer(), person, "person.bin");

            // XPath и LINQ to XML
            PerformXmlQueries("person.xml");
        }

        static void TestSerializer(IAVFSerializer serializer, AVFPerson person, string filePath)
        {
            Console.WriteLine($"Testing {serializer.GetType().Name}...");

            // Сериализация
            serializer.Serialize(person, filePath);
            Console.WriteLine($"Serialized to {filePath}");

            // Десериализация
            var deserializedPerson = serializer.Deserialize<AVFPerson>(filePath);
            Console.WriteLine($"Deserialized: {deserializedPerson}");
        }

        static void PerformXmlQueries(string xmlFilePath)
        {
            Console.WriteLine("\nPerforming XML Queries...");

            // Загружаем XML-документ
            var xmlDoc = XDocument.Load(xmlFilePath);

            // XPath-запросы
            var nameXPath = xmlDoc.XPathSelectElement("//Name");
            Console.WriteLine($"XPath Query (Name): {nameXPath?.Value}");

            // LINQ to XML: Все элементы
            var elements = xmlDoc.Descendants().Select(e => e.Name + ": " + e.Value);
            Console.WriteLine("LINQ to XML Query (All Elements):");
            foreach (var element in elements)
            {
                Console.WriteLine(element);
            }
        }
    }
}
