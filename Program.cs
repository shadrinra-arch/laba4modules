using System;
using System.Collections.Generic;
using System.Threading;

namespace laba4modules
{
    

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
            var registry = Singleton.Instance;
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