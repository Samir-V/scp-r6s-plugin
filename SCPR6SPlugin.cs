using Exiled.API.Enums;
using Exiled.API.Features;
using System.Collections.Generic;

namespace SCPR6SPlugin
{
    public class SCPR6SPlugin : Plugin<Config>
    {
        public static SCPR6SPlugin Instance { get; private set; } = new SCPR6SPlugin();

        public static Config PluginConfig => Instance.Config;

        private SCPR6SPlugin() { }

        private PlayerHandler playerHandler;

        public override string Name { get; } = "SCP-R6S";

        public override string Prefix { get; } = "scpr6s";

        public override string Author { get; } = "SamV";

        public override PluginPriority Priority { get; } = PluginPriority.Default;

        public override System.Version Version { get; } = new System.Version(1, 1, 0);

        public List<int> KapkanID { get; set; }

        public List<int> FuzeID { get; set; }

        public int FuzeCurrentCharges { get; set; }

        public int FuzeCraftCount { get; set; }

        public override void OnEnabled()
        {
            KapkanID = new List<int>();
            FuzeID = new List<int>();
            FuzeCurrentCharges = Instance.Config.FuzeCharges;
            FuzeCraftCount = 0;

            RegisterEvents();
            base.OnEnabled();
        }

        public override void OnDisabled()
        {

            KapkanID.Clear();
            FuzeID.Clear();

            UnregisterEvents();
            base.OnDisabled();
        }
         
        private void RegisterEvents()
        {
            playerHandler = new PlayerHandler();
            Exiled.Events.Handlers.Player.Spawned += playerHandler.OnSpawn;
        }

        private void UnregisterEvents()
        {
            Exiled.Events.Handlers.Player.Spawned -= playerHandler.OnSpawn;
            playerHandler = null;
        }
    }
}
