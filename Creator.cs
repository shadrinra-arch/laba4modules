using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laba4modules
{
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
}
