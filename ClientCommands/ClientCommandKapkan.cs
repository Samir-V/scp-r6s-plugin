using AdminToys;
using CommandSystem;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Toys;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SCPR6SPlugin
{
    [CommandHandler(typeof(ClientCommandHandler))]
    internal class ClientCommandKapkan : ICommand
    {
        public string Command { get; } = "kapkan";

        /// <inheritdoc/>
        public string[] Aliases { get; } = new string[0];

        /// <inheritdoc/>
        public string Description { get; } = "Установка капкана.";

        /// <inheritdoc />
        public bool SanitizeResponse { get; }
        Vector3 AlignNearestWorldAxis(Vector3 direction)
        {
            float x;
            float z;

            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
            {
                x = Mathf.Sign(direction.x);
                z = 0;
            }
            else
            {
                x = 0;
                z = Mathf.Sign(direction.z);
            }

            return new Vector3(x, 0, z).normalized;
        }

        private IEnumerator<float> ActivationTimeCheck(Player kapkanPlayer, Door kapkanDoor)
        {
            var kapkanStartPos = kapkanPlayer.Position;
            var kapkanStartRot = kapkanPlayer.Rotation;

            Vector3 kapkanForwardNorm = kapkanPlayer.Transform.forward.normalized;

            Vector3 directionAlignedWorld = AlignNearestWorldAxis(kapkanForwardNorm);

            Log.Info(directionAlignedWorld.x);
            Log.Info(directionAlignedWorld.z);

            for (var time = 0; time < 3; ++time)
            {
                var distance = Vector3.Distance(kapkanStartPos, kapkanPlayer.Position);
                var angleDiff = Quaternion.Angle(kapkanStartRot, kapkanPlayer.Rotation);

                if (distance < 1.2f && kapkanDoor.IsFullyClosed && angleDiff < 30.0f && kapkanPlayer.CurrentItem.Base.ItemTypeId == ItemType.GrenadeHE)
                {
                    yield return Timing.WaitForSeconds(1.0f);
                    kapkanPlayer.Broadcast(1, $"Установка завершена на {(time + 1) * 33}%");
                    continue;
                }
                else
                {
                    kapkanPlayer.ShowHint("Установка прервана", 3f);
                    yield break;
                }
            }

            Vector3 trapPos = Vector3.zero;

            if (directionAlignedWorld.x > 0)
            {
                Vector3 kapkanPos = new Vector3(kapkanDoor.Position.x - directionAlignedWorld.x * 0.25f, kapkanDoor.Position.y + 0.2f, kapkanDoor.Position.z + 0.4f);

                trapPos = kapkanPos;
            }
            else if (directionAlignedWorld.x < 0)
            {
                Vector3 kapkanPos = new Vector3(kapkanDoor.Position.x - directionAlignedWorld.x * 0.25f, kapkanDoor.Position.y + 0.2f, kapkanDoor.Position.z - 0.4f);

                trapPos = kapkanPos;
            }
            else if (directionAlignedWorld.z > 0)
            {
                Vector3 kapkanPos = new Vector3(kapkanDoor.Position.x - 0.4f, kapkanDoor.Position.y + 0.2f, kapkanDoor.Position.z - directionAlignedWorld.z * 0.25f);

                trapPos = kapkanPos;
            }
            else if (directionAlignedWorld.z < 0)
            {
                Vector3 kapkanPos = new Vector3(kapkanDoor.Position.x + 0.4f, kapkanDoor.Position.y + 0.2f, kapkanDoor.Position.z - directionAlignedWorld.z * 0.25f);

                trapPos = kapkanPos;
            }

            Primitive charge = Primitive.Create(UnityEngine.PrimitiveType.Cube, PrimitiveFlags.Visible, trapPos, null, new Vector3(0.4f, 0.4f, 0.4f), true, null);

            var currentItem = kapkanPlayer.CurrentItem;

            kapkanPlayer.RemoveItem(currentItem);

            kapkanPlayer.ShowHint("Заряд установлен", 3f);
        }

        /// <inheritdoc/>
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var player = Player.Get(sender);

            if (SCPR6SPlugin.Instance.KapkanID.Count == 0 || player.Id != SCPR6SPlugin.Instance.KapkanID[0])
            {
                response = "Вы не Капкан";
                return false;
            }

            var currentItem = player.CurrentItem;

            if (currentItem is null || currentItem.Base.ItemTypeId != ItemType.GrenadeHE)
            {
                response = "Возьмите гранату в руки для установки";
                return false;
            }

            Door door = Door.GetClosest(player.Position, out float distance);

            if (door.IsGate || door.IsElevator)
            {
                response = "Вы слишком далеко от подходящей двери";
                return false;
            }

            bool RayHit = Physics.Raycast(player.CameraTransform.position, player.CameraTransform.forward, 0.6f);

            if (distance > 1.2f || !door.IsFullyClosed || !RayHit)
            {
                response = "Вы слишком далеко от подходящей двери";
                return false;
            }

            MEC.Timing.RunCoroutine(ActivationTimeCheck(player, door));

            response = "Установка капкана началась...";
            return true;
        }
    }
}
