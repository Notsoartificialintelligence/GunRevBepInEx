using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;

namespace GunRev
{
    public class SilverCursor : PlayerItem
    {
        public static void Register()
        {
            string itemName = "Silver Cursor";
            string resourceName = "GunRev/Resources/silvercursor";
            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<SilverCursor>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Force The Hand Of Fate";
            string longDesc = "Spawns a Chance Kin on use.\n\nAn incredibly heavy cursor. It smells of fresh cookies.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ai");
            item.quality = PickupObject.ItemQuality.B;
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.None, 0f);
            item.consumable = true;
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }
        public override void DoEffect(PlayerController user)
        {
            SpawnChanceKin();
        }
        private void SpawnChanceKin()
        {
            var ChanceKin = EnemyDatabase.GetOrLoadByGuid("a446c626b56d4166915a4e29869737fd");
            IntVector2? spawnPos = LastOwner.CurrentRoom.GetRandomVisibleClearSpot(1, 1);
            if (spawnPos != null)
            {
                AIActor TargetActor = AIActor.Spawn(ChanceKin.aiActor, spawnPos.Value, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(spawnPos.Value), true, AIActor.AwakenAnimationType.Default, true);
                PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(TargetActor.specRigidbody, null, false);
                TargetActor.IsHarmlessEnemy = true;
                TargetActor.IgnoreForRoomClear = true;
                TargetActor.HandleReinforcementFallIntoRoom(0f);
            }
        }
    }
}