using System;
using System.Collections.Generic;
using System.Threading;

namespace laba4modules
{
    public class Computer : ICloneable
    {
        public string CPU { get; set; }
        public int Memory { get; set; }
        public string GPU { get; set; }
        public List<string> Extras { get; set; } = new List<string>();

        public override string ToString()
        {
            return $"Computer(CPU={CPU}, Memory={Memory}GB, GPU={GPU}, Extras=[{string.Join(", ", Extras)}])";
        }

        // Глубокое клонирование
        public object Clone()
        {
            var clone = (Computer)MemberwiseClone();
            clone.Extras = new List<string>(Extras);
            return clone;
        }

        // Поверхностное клонирование
        public Computer CloneShallow()
        {
            return (Computer)MemberwiseClone();
        }
    }

    // Потокобезопасный Singleton-регистратор прототипов
    public sealed class PrototypeRegistry
    {
        private static readonly Lazy<PrototypeRegistry> instance = new Lazy<PrototypeRegistry>(() => new PrototypeRegistry());
        private readonly Dictionary<string, Computer> prototypes = new Dictionary<string, Computer>();
        private static readonly object lockObj = new object();

        public static PrototypeRegistry Instance => instance.Value;

        private PrototypeRegistry() { }

        public void RegisterPrototype(string name, Computer prototype)
        {
            lock (lockObj)
            {
                prototypes[name] = prototype;
            }
        }

        public Computer GetPrototype(string name)
        {
            lock (lockObj)
            {
                if (!prototypes.ContainsKey(name))
                    throw new ArgumentException("Prototype not found");
                return prototypes[name].Clone() as Computer;
            }
        }
    }

    // Абстрактный Builder
    public abstract class ComputerBuilder
    {
        protected Computer computer = new Computer();

        public abstract void BuildCPU();
        public abstract void BuildMemory();
        public abstract void BuildGPU();
        public abstract void BuildExtras();

        public Computer GetResult()
        {
            return computer;
        }
    }

    // Конкретные сборщики
    public class OfficePCBuilder : ComputerBuilder
    {
        public override void BuildCPU() => computer.CPU = "Intel Core i3";
        public override void BuildMemory() => computer.Memory = 8;
        public override void BuildGPU() => computer.GPU = "Integrated";
        public override void BuildExtras() => computer.Extras.AddRange(new[] { "Keyboard", "Mouse" });
    }

    public class GamingPCBuilder : ComputerBuilder
    {
        public override void BuildCPU() => computer.CPU = "Intel Core i9";
        public override void BuildMemory() => computer.Memory = 32;
        public override void BuildGPU() => computer.GPU = "NVIDIA RTX 4090";
        public override void BuildExtras() => computer.Extras.AddRange(new[] { "RGB Lighting", "Gaming Mouse" });
    }

    public class HomePCBuilder : ComputerBuilder
    {
        public override void BuildCPU() => computer.CPU = "AMD Ryzen 5";
        public override void BuildMemory() => computer.Memory = 16;
        public override void BuildGPU() => computer.GPU = "Integrated";
        public override void BuildExtras() => computer.Extras.AddRange(new[] { "Speaker", "Webcam" });
    }

    // Директор сборки
    public class ComputerDirector
    {
        private readonly ComputerBuilder builder;
        public ComputerDirector(ComputerBuilder builder)
        {
            this.builder = builder;
        }

        public Computer Construct()
        {
            builder.BuildCPU();
            builder.BuildMemory();
            builder.BuildGPU();
            builder.BuildExtras();
            return builder.GetResult();
        }
    }

    // Фабрика для создания конфигураций
    public static class Creator
    {
        public static Computer CreateOfficePC()
        {
            var builder = new OfficePCBuilder();
            var director = new ComputerDirector(builder);
            return director.Construct();
        }

        public static Computer CreateGamingPC()
        {
            var builder = new GamingPCBuilder();
            var director = new ComputerDirector(builder);
            return director.Construct();
        }

        public static Computer CreateHomePC()
        {
            var builder = new HomePCBuilder();
            var director = new ComputerDirector(builder);
            return director.Construct();
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            // Создаем конфигурации через фабрику
            var officePC = Creator.CreateOfficePC();
            var gamingPC = Creator.CreateGamingPC();
            var homePC = Creator.CreateHomePC();

            Console.WriteLine($"Офисный ПК: {officePC}");
            Console.WriteLine($"Игровой ПК: {gamingPC}");
            Console.WriteLine($"Домашний ПК: {homePC}");

            // Регистрируем прототипы
            var registry = PrototypeRegistry.Instance;
            registry.RegisterPrototype("office", officePC);
            registry.RegisterPrototype("gaming", gamingPC);
            registry.RegisterPrototype("home", homePC);

            // Получение прототипа и клонирование
            var cloneOffice = registry.GetPrototype("office");
            cloneOffice.CPU = "Intel Xeon"; // изменения клона

            Console.WriteLine($"\nКлон офиса с изменениями: {cloneOffice}");

            // Демонстрация разницы между поверхностным и глубоким копированием
            var shallowCopy = officePC.CloneShallow();
            var deepCopy = officePC.Clone();

            // Модификация списка Extras в оригинале
            officePC.Extras.Add("Monitor");

            Console.WriteLine("\nПосле добавления 'Monitor' в оригинал:");
            Console.WriteLine($"Оригинал: {officePC}");
            Console.WriteLine($"Поверхностное копирование: {shallowCopy}"); // Extras — ссылка
            Console.WriteLine($"Глубокое копирование: {deepCopy}");        // независимая копия
        }
    }
}