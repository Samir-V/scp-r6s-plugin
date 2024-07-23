using Exiled.API.Interfaces;
using System;
using System.ComponentModel;

namespace SCPR6SPlugin
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;

        [Description("Шанс спавна Капкана 0.0 = 0% -- 1.0 = 100%")]
        public double KapkanChance { get; set; } = 0.7;

        [Description("Шанс спавна Фьюза 0.0 = 0% -- 1.0 = 100%")]
        public double FuzeChance { get; set; } = 0.7;

        [Description("Изначальное количество зарядов у Фьюза")]
        public int FuzeCharges { get; set; } = 3;
    }
}
