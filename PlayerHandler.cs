using Exiled.Events.EventArgs.Player;
using PlayerRoles;


namespace SCPR6SPlugin
{
    internal sealed class PlayerHandler
    {
        //public void OnChangingRole(ChangingRoleEventArgs ev)
        //{
        //    ev.Player.ClearInventory();
        //}

        public void OnSpawn(SpawnedEventArgs ev)
        {
            if (ev.Player.Role == RoleTypeId.FacilityGuard) 
            {
                float chance = UnityEngine.Random.Range(0.0f, 1.0f);

                if (chance <= SCPR6SPlugin.PluginConfig.KapkanChance)
                {
                    ev.Player.ClearInventory();

                    for (int index = 0; index < 8; ++index)
                    {
                        ev.Player.AddItem(ItemType.GrenadeHE);
                    }
                }
            }
        }
    }
}
