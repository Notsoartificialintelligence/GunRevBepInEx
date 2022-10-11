using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;
using Dungeonator;
using System.Collections.ObjectModel;

namespace GunRev
{
    public class TableTechPause : PassiveItem
    {
		public bool TableFlocking = true;
		public bool IncreaseSpeedOutOfCombat = true;
		public static void Register()
        {
			string itemName = "Table Tech Pause";

            string resourceName = "GunRev/Resources/tabletechpause";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<TableTechPause>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Flip Stop";
            string longDesc = "Slows time for a few moments.\n\nChapter 23 of the Tabla Sutra - Decompiled, Revision 12. \"Tables are a perfect example of how energy can be converted from physical energy to many other forms, such as thermal energy, velocity, or even temporal energy.\"\n- A report from an anonymous researcher.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ai");

            item.quality = PickupObject.ItemQuality.B;

		}
		private void HandleTableFlocking(FlippableCover table)
		{
			if (this.TableFlocking)
			{
				RoomHandler currentRoom = base.Owner.CurrentRoom;
				ReadOnlyCollection<IPlayerInteractable> roomInteractables = currentRoom.GetRoomInteractables();
				for (int i = 0; i < roomInteractables.Count; i++)
				{
					if (currentRoom.IsRegistered(roomInteractables[i]))
					{
						FlippableCover flippableCover = roomInteractables[i] as FlippableCover;
						if (flippableCover != null && !flippableCover.IsFlipped && !flippableCover.IsGilded)
						{
							if (flippableCover.flipStyle == FlippableCover.FlipStyle.ANY)
							{
								flippableCover.ForceSetFlipper(base.Owner);
								flippableCover.Flip(table.DirectionFlipped);
							}
							else if (flippableCover.flipStyle == FlippableCover.FlipStyle.ONLY_FLIPS_LEFT_RIGHT)
							{
								if (table.DirectionFlipped == DungeonData.Direction.NORTH || table.DirectionFlipped == DungeonData.Direction.SOUTH)
								{
									flippableCover.ForceSetFlipper(base.Owner);
									flippableCover.Flip((UnityEngine.Random.value <= 0.5f) ? DungeonData.Direction.WEST : DungeonData.Direction.EAST);
								}
								else
								{
									flippableCover.ForceSetFlipper(base.Owner);
									flippableCover.Flip(table.DirectionFlipped);
								}
							}
							else if (flippableCover.flipStyle == FlippableCover.FlipStyle.ONLY_FLIPS_UP_DOWN)
							{
								if (table.DirectionFlipped == DungeonData.Direction.EAST || table.DirectionFlipped == DungeonData.Direction.WEST)
								{
									flippableCover.ForceSetFlipper(base.Owner);
									flippableCover.Flip((UnityEngine.Random.value <= 0.5f) ? DungeonData.Direction.SOUTH : DungeonData.Direction.NORTH);
								}
								else
								{
									flippableCover.ForceSetFlipper(base.Owner);
									flippableCover.Flip(table.DirectionFlipped);
								}
							}
						}
					}
				}
			}
		}
		public override void Pickup(PlayerController player)
        {
            player.OnTableFlipped = (Action<FlippableCover>)Delegate.Combine(player.OnTableFlipped, new Action<FlippableCover>(this.HandleFlip));
            base.Pickup(player);
        }

        private void HandleFlip(FlippableCover table)
        {
			this.HandleTableFlocking(table);
			RadialSlow.EffectRadius = 9999999999999999;
			room = GameManager.Instance.PrimaryPlayer.GetAbsoluteParentRoom();
			centerpoint = new Vector2(0, 0);
			RadialSlow.DoesSepia = true;
			RadialSlow.RadialSlowHoldTime = 3f;

			RadialSlow.DoRadialSlow(centerpoint, room);
        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.OnTableFlipped = (Action<FlippableCover>)Delegate.Remove(player.OnTableFlipped, new Action<FlippableCover>(this.HandleFlip));
            
            return base.Drop(player);
        }
		public RadialSlowInterface RadialSlow;
		public RoomHandler room;
		public Vector2 centerpoint;
	}
}