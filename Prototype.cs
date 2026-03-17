using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
