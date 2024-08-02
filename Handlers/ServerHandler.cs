

namespace SCPR6SPlugin
{
    internal sealed class ServerHandler
    {
        public void OnWaitingPlayers()
        {
            MEC.Timing.KillCoroutines();
        }

    }
}
