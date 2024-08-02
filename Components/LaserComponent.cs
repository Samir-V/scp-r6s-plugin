using Exiled.API.Features;
using Exiled.API.Features.Items;
using System.Linq;
using UnityEngine;

namespace SCPR6SPlugin
{
    public class LaserComponent : MonoBehaviour
    {
        public void Update()
        {
            for (int index = 0; index < SCPR6SPlugin.Instance.KapkanLasers.Count; ++index)
            {
                Collider[] hitColliders = Physics.OverlapSphere(SCPR6SPlugin.Instance.KapkanLasers[index].Position, 0.22f);

                foreach (var hitCollider in hitColliders)
                {
                    Player.TryGet(hitCollider.gameObject, out var player);

                    if (player != null)
                    {
                        if (SCPR6SPlugin.Instance.KapkanID.Count() == 0 || player.Id != SCPR6SPlugin.Instance.KapkanID[0])
                        {
                            if (SCPR6SPlugin.PluginConfig.KapkanAllyTrigger)
                            {
                                var grenade = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE);
                                grenade.FuseTime = 0.1f;
                                grenade.SpawnActive(SCPR6SPlugin.Instance.Kapkans[index].Position);

                                SCPR6SPlugin.Instance.Kapkans[index].Destroy();
                                KapkanComponent tempKComp = SCPR6SPlugin.Instance.Kapkans[index].Base.GetComponent<KapkanComponent>();
                                tempKComp.active = false;
                                SCPR6SPlugin.Instance.Kapkans.RemoveAt(index);

                                SCPR6SPlugin.Instance.KapkanLasers[index].Destroy();
                                SCPR6SPlugin.Instance.KapkanLasers.RemoveAt(index);
                            }
                            else if (!SCPR6SPlugin.Instance.KapkanAllyRoles.Contains(player.Role))
                            {
                                var grenade = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE);
                                grenade.FuseTime = 0.1f;
                                grenade.SpawnActive(SCPR6SPlugin.Instance.Kapkans[index].Position);

                                SCPR6SPlugin.Instance.Kapkans[index].Destroy();
                                KapkanComponent tempKComp = SCPR6SPlugin.Instance.Kapkans[index].Base.GetComponent<KapkanComponent>();
                                tempKComp.active = false;
                                SCPR6SPlugin.Instance.Kapkans.RemoveAt(index);
                                SCPR6SPlugin.Instance.KapkanLasers[index].Destroy();
                                SCPR6SPlugin.Instance.KapkanLasers.RemoveAt(index);
                            }
                        }
                    }
                }
            }

            //if (player != null && player.Id != SCPR6SPlugin.Instance.KapkanID[0])
            //{
            //    int nearestIndex = -1;
            //    float minDistance = 0;

            //    for (int index = 0; index < SCPR6SPlugin.Instance.Kapkans.Count; ++index)
            //    {
            //        Primitive kapkan = SCPR6SPlugin.Instance.Kapkans[index];

            //        float distance = Vector3.Distance(player.Position, kapkan.Position);

            //        if (distance < minDistance)
            //        {
            //            minDistance = distance;
            //            nearestIndex = index;
            //        }
            //    }

            //    var grenade = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE);

            //    grenade.FuseTime = 0.1f;

            //    grenade.SpawnActive(SCPR6SPlugin.Instance.Kapkans[nearestIndex].Position);

            //    SCPR6SPlugin.Instance.Kapkans[nearestIndex].Destroy();
            //    SCPR6SPlugin.Instance.Kapkans.RemoveAt(nearestIndex);
            //    SCPR6SPlugin.Instance.KapkanLasers[nearestIndex].Destroy();
            //    SCPR6SPlugin.Instance.KapkanLasers.RemoveAt(nearestIndex);
            //}
        }
    }
}