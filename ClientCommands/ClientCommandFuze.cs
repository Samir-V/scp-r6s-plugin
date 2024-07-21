using AdminToys;
using CommandSystem;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Pickups.Projectiles;
using Exiled.API.Features.Toys;
using Exiled.CustomItems.API.Features;
using InventorySystem.Items.ThrowableProjectiles;
using MEC;
using PlayerRoles.PlayableScps.Scp939.Ripples;
using System;
using System.Collections.Generic;
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
        public string Description { get; } = "Установка заряда Fuze";

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

        private IEnumerator<float> ActivationTimeCheck(Player fuzePlayer, Door fuzeDoor)
        {
            var fuzeStartPos = fuzePlayer.Position;
            var fuzeStartRot = fuzePlayer.Rotation;

            Vector3 fuzeForwardNorm = fuzePlayer.Transform.forward.normalized;

            Vector3 directionAlignedWorld = AlignNearestWorldAxis(fuzeForwardNorm);

            Log.Info(directionAlignedWorld.x);
            Log.Info(directionAlignedWorld.z);

            for (var time = 0; time < 5; ++time)
            {
                var distance = Vector3.Distance(fuzeStartPos, fuzePlayer.Position);
                var angleDiff = Quaternion.Angle(fuzeStartRot, fuzePlayer.Rotation);

                if (distance < 1.2f && fuzeDoor.IsFullyClosed && angleDiff < 30.0f)
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

            yield return Timing.WaitForSeconds(2.0f);

            for (var time = 0; time < 6; ++time)
            {
                Exiled.API.Features.Pickups.Pickup.CreateAndSpawn(ItemType.GrenadeHE, charge.Base.transform.position + directionAlignedWorld, Quaternion.LookRotation(directionAlignedWorld));
                yield return Timing.WaitForSeconds(0.5f);
            }

            yield return Timing.WaitForSeconds(1.0f);

            charge.Destroy();

            //ThrowRequest.FullForceThrow;

            //GrenadePickup grenade = new GrenadePickup.Create(ItemType.GrenadeHE);
            
            //= new EffectGrenadeProjectile.CreateAndSpawn(ProjectileType.FragGrenade, charge.Position, fuzeStartRot);
            
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

            bool RayHit = Physics.Raycast(player.CameraTransform.position, player.CameraTransform.forward, 0.6f);

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
