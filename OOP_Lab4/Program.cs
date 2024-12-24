using System;

public interface IControl
{
    void Show();
    void Input();
    void Resize();
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

public sealed class Button : ControlElement
{
    public override void Show() => Console.WriteLine("Отображение Button");
    public override void Input() => Console.WriteLine("Ввод для Button");
    public override void Resize() => Console.WriteLine("Изменение размера Button");

    public override string ToString() => base.ToString() + " (Button)";
}

public class Printer
{
    public void IAmPrinting(GeometricShape shape)
    {
        Console.WriteLine(shape.ToString());
    }

    public void IAmPrinting(ControlElement element)
    {
        Console.WriteLine(element.ToString());
    }
}

class Program
{
    static void Main()
    {
        GeometricShape circle = new Circle(5);
        GeometricShape rectangle = new Rectangle(4, 6);

        ControlElement checkbox = new Checkbox();
        ControlElement radiobutton = new Radiobutton();
        ControlElement button = new Button();

        object[] objects = { circle, rectangle, checkbox, radiobutton, button };

        Printer printer = new Printer();

        foreach (var obj in objects)
        {
            if (obj is GeometricShape shape)
            {
                printer.IAmPrinting(shape);
            }
            else if (obj is ControlElement element)
            {
                printer.IAmPrinting(element);
            }
        }
    }
}
