using CommandSystem;
using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCPR6SPlugin
{
    [CommandHandler(typeof(ClientCommandHandler))]
    internal class ClientCommandCraftCharges : ICommand
    {
        public string Command { get; } = "craftcharge";

        /// <inheritdoc/>
        public string[] Aliases { get; } = new string[0];

        /// <inheritdoc/>
        public string Description { get; } = "Создание заряда из 6 гранат";

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

            var currentItem = player.CurrentItem;

            if (currentItem is null || currentItem.Base.ItemTypeId != ItemType.GrenadeHE)
            {
                response = "";
                return false;
            }

            SCPR6SPlugin.Instance.FuzeCraftCount += 1;
            player.RemoveItem(currentItem);

            if (SCPR6SPlugin.Instance.FuzeCraftCount != 6)
            {
                response = $"Разобрано. Для создания заряда нужно ещё {6 - SCPR6SPlugin.Instance.FuzeCraftCount} гранат";
                return true;
            }
            else
            {
                SCPR6SPlugin.Instance.FuzeCraftCount = 0;
                SCPR6SPlugin.Instance.FuzeCurrentCharges += 1;
                response = "Заряд Fuze создан!";
                return true;
            }


        }
    }
}
