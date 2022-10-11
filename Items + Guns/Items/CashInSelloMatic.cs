using Alexandria.ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SaveAPI;
using BreakAbleAPI;

namespace GunRev
{
    public class CashInSelloMatic : PlayerItem
    {
        public static GameObject note;
        public static void Register()
        {
            string itemName = "CashIn SelloMatic 2300";
            string resourceName = "GunRev/Resources/instacash";
            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<CashInSelloMatic>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Items In, Cash Out";
            string longDesc = "Turns nearby items on the floor into cold, hard casings.\n\nA handheld device produced by CashIn Conglomerate, capable of converting any item it is pointed towards into money instantly, at a fee. A faded sticker on the back reads, \"Do not point at a mirror under any circumstances.\"";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ai");
            item.quality = PickupObject.ItemQuality.A;
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Timed, 3f);
            item.consumable = false;

            ETGMod.Databases.Strings.Core.Set("#SELL_NOTE", "Your item has been lost in transit. Expect a refund in around ERROR:INT.MAX days. Thank you for your patronage. - CashIn Conglomerate");

            MajorBreakable breakable = BreakAbleAPI.BreakableAPIToolbox.GenerateMajorBreakable("funny note", new[] { "GunRev/Resources/sell_note.png" }, 2, null, 5, 0, null, 0, 0, true, 0, 0);
            note = BreakAbleAPI.BreakableAPIToolbox.GenerateNoteDoer(breakable, BreakAbleAPI.BreakableAPIToolbox.GenerateTransformObject(breakable.gameObject, new Vector2(0.25f, 0.25f), "noteattachPoint").transform, "#SELL_NOTE", true).gameObject;

            item.SetupUnlockOnStat(TrackedStats.TOTAL_MONEY_COLLECTED, 3000, DungeonPrerequisite.PrerequisiteOperation.GREATER_THAN);
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }
        public override void DoEffect(PlayerController user)
        {
            AkSoundEngine.PostEvent("Play_WPN_zapper_shot_01", base.gameObject);
            IPlayerInteractable nearestInteractable = user.CurrentRoom.GetNearestInteractable(user.CenterPosition, 2f, user);
            int pickupID = -1;
            if (nearestInteractable is PassiveItem)
            {
                pickupID = (nearestInteractable as PassiveItem).PickupObjectId;
            }
            else if (nearestInteractable is PlayerItem)
            {
                pickupID = (nearestInteractable as PlayerItem).PickupObjectId;
            }
            if (pickupID != -1)
            {
                DebrisObject theone = null;
                PickupObject item = PickupObjectDatabase.GetById(pickupID);
                foreach (DebrisObject debris in StaticReferenceManager.AllDebris.ToArray())
                {
                    if (debris.IsPickupObject && debris.GetComponent<PickupObject>().PickupObjectId == item.PickupObjectId)
                    {
                        theone = debris;
                        break;
                    }
                }

                int sellPrice = Mathf.Clamp(Mathf.CeilToInt((float)item.PurchasePrice * 0.4f), 0, 200);
                if (item.quality == PickupObject.ItemQuality.SPECIAL || item.quality == PickupObject.ItemQuality.EXCLUDED)
                {
                    sellPrice = 0;
                }
                if (sellPrice != 0)
                {
                    LootEngine.SpawnCurrency(item.sprite.WorldCenter, sellPrice);
                }
                else
                {
                    GameObject thisnote = UnityEngine.Object.Instantiate(note, user.transform.position, Quaternion.identity);
                    LastOwner.CurrentRoom.RegisterInteractable(thisnote.GetComponent<NoteDoer>());
                }
                theone.TriggerDestruction(true);
            }
        }
        public override bool CanBeUsed(PlayerController user)
        {
            IPlayerInteractable nearestInteractable = user.CurrentRoom.GetNearestInteractable(user.CenterPosition, 2f, user);
            return (nearestInteractable is PassiveItem || nearestInteractable is PlayerItem);
        }
    }
}