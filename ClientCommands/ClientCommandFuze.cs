using AdminToys;
using CommandSystem;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Toys;
using MEC;
using Mono.Cecil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Timers;
using UnityEngine;
using UnityEngine.Rendering;

namespace SCPR6SPlugin
{
    [CommandHandler(typeof(ClientCommandHandler))]
    internal class ClientCommandFuze : ICommand
    {
        public string Command { get; } = "fuze";

        /// <inheritdoc/>
        public string[] Aliases { get; } = new string[0];

        /// <inheritdoc/>
        public string Description { get; } = "Установка заряда Fuze";

        /// <inheritdoc />
        public bool SanitizeResponse { get; }

        private IEnumerator<float> ActivationTimeCheck(Player fuzePlayer, Door fuzeDoor)
        {
            var fuzeStartPos = fuzePlayer.Position;
            var fuzeStartRot = fuzePlayer.Rotation;

            for (var time = 0; time < 5; ++time)
            {
                var distance = Vector3.Distance(fuzeStartPos, fuzePlayer.Position);
                var angleDiff = Quaternion.Angle(fuzeStartRot, fuzePlayer.Rotation);

                if (distance < 1.2f && fuzeDoor.IsFullyClosed && angleDiff < 20.0f)
                {
                    yield return Timing.WaitForSeconds(1.0f);
                    fuzePlayer.Broadcast(1, $"Установка завершена на {(time + 1) * 20}%");
                    continue;
                }
                else
                {
                    fuzePlayer.ShowHint("Установка прервана", 3f);
                    yield break;
                }
            }

            Vector3 chargePos = new Vector3(fuzeDoor.Position.x, fuzeDoor.Position.y + 1.35f, fuzeDoor.Position.z);

            Primitive charge = Primitive.Create(UnityEngine.PrimitiveType.Cube, PrimitiveFlags.Visible, chargePos, null, new Vector3(0.6f, 0.6f, 0.6f), true, null);

            SCPR6SPlugin.Instance.FuzeCurrentCharges -= 1;

            fuzePlayer.ShowHint("Заряд установлен", 3f);

            //for (var time = 0; time < 2; ++time)
            //{
            //    yield return Timing.WaitForSeconds(1.0f);
            //}

            //charge.Destroy();
        }

        /// <inheritdoc/>
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var player = Player.Get(sender);

            if (SCPR6SPlugin.Instance.FuzeID.Count == 0 || player.Id != SCPR6SPlugin.Instance.FuzeID[0])
            {
                response = "Вы не Фьюз";
                return false;
            }

            if (SCPR6SPlugin.Instance.FuzeCurrentCharges == 0)
            {
                response = "У вас нет зарядов";
                return false;
            }

            Door door = Door.GetClosest(player.Position, out float distance);

            if (door.IsGate || door.IsElevator)
            {
                response = "Вы слишком далеко от подходящей двери";
                return false;
            }

            bool RayHit = Physics.Raycast(player.CameraTransform.position, player.CameraTransform.forward, 1);

            if (distance > 1.2f || !door.IsFullyClosed || !RayHit)
            {
                response = "Вы слишком далеко от подходящей двери";
                return false;
            }

            MEC.Timing.RunCoroutine(ActivationTimeCheck(player, door));

            response = "Установка заряда началась...";
            return true;
        }
    }
}
