using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laba4modules
{
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
}
