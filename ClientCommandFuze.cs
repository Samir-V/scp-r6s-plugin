using AdminToys;
using CommandSystem;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Toys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            Primitive.Create(UnityEngine.PrimitiveType.Cube, PrimitiveFlags.Visible, player.Position, null, new Vector3(10, 10, 10), true, null);

            response = "Успех!";
            return true;
        }
    }
}
