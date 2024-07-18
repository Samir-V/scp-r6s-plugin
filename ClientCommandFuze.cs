using AdminToys;
using CommandSystem;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Toys;
using System;

using UnityEngine;

namespace SCPR6SPlugin
{
    [CommandHandler(typeof(ClientCommandHandler))]
    internal class ClientCommandFuze : ICommand
    {
        public string Command { get; } = "fuze";

        /// <inheritdoc/>
        public string[] Aliases { get; } = new string[0];

        /// <inheritdoc/>
        public string Description { get; } = "test";

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

            Door door = Door.GetClosest(player.Position, out float distance);

            if (distance > 1.5 || !door.IsFullyClosed)
            {
                response = "Вы слишком далеко от подходящей двери.";
                return false;
            }

            Primitive charge = Primitive.Create(UnityEngine.PrimitiveType.Cube, PrimitiveFlags.Visible, player.Position, null, new Vector3(1, 1, 1), true, null);

            response = "Успех!";

            return true;
        }
    }
}
