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
}
