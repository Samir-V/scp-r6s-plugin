
using CommandSystem;
using Exiled.API.Features;
using System;


namespace SCPR6SPlugin
{
    [CommandHandler(typeof(ClientCommandHandler))]
    internal class ClientCommandCheckCharges : ICommand
    {
        public string Command { get; } = "checkcharges";

        /// <inheritdoc/>
        public string[] Aliases { get; } = new string[0];

        /// <inheritdoc/>
        public string Description { get; } = "Проверка оставшегося количества зарядов";

        /// <inheritdoc />
        public bool SanitizeResponse { get; }


        /// <inheritdoc/>
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var player = Player.Get(sender);

            if (SCPR6SPlugin.Instance.FuzeID.Count == 0 || player.Id != SCPR6SPlugin.Instance.FuzeID[0])
            {
                response = "Вы не Фьюз";
                return false;
            }

            response = $"Оставшееся количество зарядов - {SCPR6SPlugin.Instance.FuzeCurrentCharges}";
            return true;
        }
    }
}
