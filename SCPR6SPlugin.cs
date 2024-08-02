using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Toys;
using PlayerRoles;
using System.Collections.Generic;
using UnityEngine;

namespace SCPR6SPlugin
{
    public class SCPR6SPlugin : Plugin<Config>
    {
        public static SCPR6SPlugin Instance { get; private set; } = new SCPR6SPlugin();

        public static Config PluginConfig => Instance.Config;

        private SCPR6SPlugin() { }

        private PlayerHandler playerHandler;

        private MapHandler mapHandler;

        private ServerHandler serverHandler;

        public override string Name { get; } = "SCP-R6S";

        public override string Prefix { get; } = "scpr6s";

        public override string Author { get; } = "SamV";

        public override PluginPriority Priority { get; } = PluginPriority.Default;

        public override System.Version Version { get; } = new System.Version(1, 4, 0);

        public List<int> KapkanID { get; set; }

        public List<int> FuzeID { get; set; }

        public List<Primitive> Kapkans { get; set; }

        public List<Primitive> KapkanLasers { get; set; }

        public List<RoleTypeId> KapkanAllyRoles { get; set; }

        public int FuzeCurrentCharges { get; set; }

        public int FuzeCraftCount { get; set; }

        public override void OnEnabled()
        {
            KapkanID = new List<int>();
            FuzeID = new List<int>();
            Kapkans = new List<Primitive>();
            KapkanLasers = new List<Primitive>();
            KapkanAllyRoles = new List<RoleTypeId>
            {
                RoleTypeId.FacilityGuard,
                RoleTypeId.Scientist,
                RoleTypeId.NtfCaptain,
                RoleTypeId.NtfSpecialist,
                RoleTypeId.NtfSergeant,
                RoleTypeId.NtfPrivate
            };

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
            mapHandler = new MapHandler();
            serverHandler = new ServerHandler();
            Exiled.Events.Handlers.Player.Spawned += playerHandler.OnSpawn;
            Exiled.Events.Handlers.Player.Shooting += playerHandler.OnShooting;
            Exiled.Events.Handlers.Map.ExplodingGrenade += mapHandler.OnExplosion;

            Exiled.Events.Handlers.Server.WaitingForPlayers += serverHandler.OnWaitingPlayers;

        }

        private void UnregisterEvents()
        {
            Exiled.Events.Handlers.Player.Spawned -= playerHandler.OnSpawn;
            Exiled.Events.Handlers.Player.Shooting -= playerHandler.OnShooting; 
            Exiled.Events.Handlers.Map.ExplodingGrenade -= mapHandler.OnExplosion;

            Exiled.Events.Handlers.Server.WaitingForPlayers -= serverHandler.OnWaitingPlayers;
            playerHandler = null;
            mapHandler = null;
            serverHandler = null;
        } 
    }
}
