using Exiled.Events.EventArgs.Player;
using PlayerRoles;


namespace SCPR6SPlugin
{
    internal sealed class PlayerHandler
    {
        public void OnSpawn(SpawnedEventArgs ev)
        {
            // Если капкана переспавнили, то при спавне следующего охранника всё ещё есть шанс стать капканом. То же самое с фьюзом

            for (int index = 0; index < SCPR6SPlugin.Instance.KapkanID.Count; ++index)
            {
                if (ev.Player.Id == SCPR6SPlugin.Instance.KapkanID[index])
                {
                    SCPR6SPlugin.Instance.KapkanID.Clear();
                    break;
                }
            }

            for (int index = 0; index < SCPR6SPlugin.Instance.FuzeID.Count; ++index)
            {
                if (ev.Player.Id == SCPR6SPlugin.Instance.FuzeID[index])
                {
                    SCPR6SPlugin.Instance.FuzeID.Clear();
                    break;
                }
            }

            // Заполнение инвентаря капкана гранатами

            if (ev.Player.Role == RoleTypeId.FacilityGuard && SCPR6SPlugin.Instance.KapkanID.Count == 0) 
            {
                float chance = UnityEngine.Random.Range(0.0f, 1.0f);

                if (chance <= SCPR6SPlugin.PluginConfig.KapkanChance)
                {
                    SCPR6SPlugin.Instance.KapkanID.Add(ev.Player.Id);

                    ev.Player.ClearInventory();

                    for (int index = 0; index < 8; ++index)
                    {
                        ev.Player.AddItem(ItemType.GrenadeHE);
                    }
                }
            }

            if (ev.Player.Role == RoleTypeId.ChaosConscript && SCPR6SPlugin.Instance.FuzeID.Count == 0)
            {
                float chance = UnityEngine.Random.Range(0.0f, 1.0f);

                if (chance <= SCPR6SPlugin.PluginConfig.FuzeChance)
                {
                    SCPR6SPlugin.Instance.FuzeID.Add(ev.Player.Id);

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
