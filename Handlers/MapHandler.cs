using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Map;
using UnityEngine;

namespace SCPR6SPlugin
{
    internal sealed class MapHandler
    {
        public void OnExplosion(ExplodingGrenadeEventArgs ev)
        {
            for (int index = 0; index < SCPR6SPlugin.Instance.Kapkans.Count; ++index)
            {
                if (Vector3.Distance(ev.Position, SCPR6SPlugin.Instance.Kapkans[index].Position) < 6.0f)
                {
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
