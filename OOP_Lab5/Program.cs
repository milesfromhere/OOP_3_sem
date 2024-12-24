using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public interface IControl
{
    void Show();
    void Input();
    void Resize();
}

public enum ControlType
{
    Button,
    Checkbox,
    Radiobutton
}

public struct Size
{
    public double Width { get; }
    public double Height { get; }

    public Size(double width, double height)
    {
        Width = width;
        Height = height;
    }

    public override string ToString() => $"Ширина: {Width}, Высота: {Height}";
}

public abstract class GeometricShape
{
    public abstract double Area();
    public abstract double Perimeter();

    public override string ToString()
    {
        return $"Тип объекта: {this.GetType().Name}, Площадь: {Area()}, Периметр: {Perimeter()}";
    }
}

public class Circle : GeometricShape
{
    public double Radius { get; set; }

    public Circle(double radius)
    {
        Radius = radius;
    }

    public override double Area() => Math.PI * Radius * Radius;
    public override double Perimeter() => 2 * Math.PI * Radius;
}

public class Rectangle : GeometricShape
{
    public double Width { get; set; }
    public double Height { get; set; }

    public Rectangle(double width, double height)
    {
        Width = width;
        Height = height;
    }

    public override double Area() => Width * Height;
    public override double Perimeter() => 2 * (Width + Height);
}

public abstract class ControlElement : IControl
{
    public abstract void Show();
    public abstract void Input();
    public abstract void Resize();

    public override string ToString()
    {
        return $"Тип элемента управления: {this.GetType().Name}";
    }
}

public class Checkbox : ControlElement
{
    public override void Show() => Console.WriteLine("Отображение Checkbox");
    public override void Input() => Console.WriteLine("Ввод для Checkbox");
    public override void Resize() => Console.WriteLine("Изменение размера Checkbox");

    public override string ToString() => base.ToString() + " (Checkbox)";
}

public class Radiobutton : ControlElement
{
    public override void Show() => Console.WriteLine("Отображение Radiobutton");
    public override void Input() => Console.WriteLine("Ввод для Radiobutton");
    public override void Resize() => Console.WriteLine("Изменение размера Radiobutton");

    public override string ToString() => base.ToString() + " (Radiobutton)";
}

public partial class Button : ControlElement
{
    public override void Show() => Console.WriteLine("Отображение Button");
    public override void Input() => Console.WriteLine("Ввод для Button");
    public override void Resize() => Console.WriteLine("Изменение размера Button");

    public override string ToString() => base.ToString() + " (Button)";
}

public partial class Button
{
    public string Label { get; set; }

    public Button(string label)
    {
        Label = label;
    }

    public void Click()
    {
        Console.WriteLine($"{Label} был нажат");
    }
}

public class Container<T> where T : class
{
    private List<T> _items = new List<T>();

    public void Add(T item)
    {
        _items.Add(item);
    }

    public void Remove(T item)
    {
        _items.Remove(item);
    }

    public T Get(int index)
    {
        if (index >= 0 && index < _items.Count)
        {
            return _items[index];
        }
        return null; // Возвращаем null, если индекс вне диапазона
    }

    public void ShowAll()
    {
        foreach (var item in _items)
        {
            Console.WriteLine(item.ToString());
        }
    }

    public int Count => _items.Count;

    public List<T> GetItems() => _items; // Возвращаем список элементов
}

public class Controller
{
    private Container<ControlElement> controlElements = new Container<ControlElement>();
    private Container<GeometricShape> geometricShapes = new Container<GeometricShape>();

    public void AddControlElement(ControlElement element)
    {
        controlElements.Add(element);
    }

    public void AddGeometricShape(GeometricShape shape)
    {
        geometricShapes.Add(shape);
    }

    public void ShowControlElements()
    {
        controlElements.ShowAll();
    }

    public void ShowGeometricShapes()
    {
        geometricShapes.ShowAll();
    }

    public int TotalElementCount()
    {
        return controlElements.Count + geometricShapes.Count;
    }

    public double TotalArea()
    {
        double area = 0;
        foreach (var shape in geometricShapes.GetItems()) // Используем GetItems() для доступа к элементам
        {
            area += shape.Area();
        }
        return area;
    }

    public void PrintAll()
    {
        ShowControlElements();
        ShowGeometricShapes();
    }
}

class Program
{
    static void Main()
    {
        GeometricShape circle = new Circle(5);
        GeometricShape rectangle = new Rectangle(4, 6);
        ControlElement button = new Button("Submit");
        ControlElement checkbox = new Checkbox();
        ControlElement radiobutton = new Radiobutton();

        Controller controller = new Controller();

        controller.AddGeometricShape(circle);
        controller.AddGeometricShape(rectangle);
        controller.AddControlElement(button);
        controller.AddControlElement(checkbox);
        controller.AddControlElement(radiobutton);

        controller.PrintAll();

        Console.WriteLine($"Общее количество элементов: {controller.TotalElementCount()}");
        Console.WriteLine($"Общая площадь геометрических объектов: {controller.TotalArea()}");
    }
}