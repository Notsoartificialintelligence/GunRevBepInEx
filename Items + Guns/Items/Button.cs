using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;

namespace GunRev
{
    public class Button : PlayerItem
    {
        public static void Register()
        {
            string itemName = "Multi-Purpose Button";
            string resourceName = "GunRev/Resources/button_idle_001";
            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<Button>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Numerous Applications";
            string longDesc = "Has various effects depending on what it synergises with.\n\nA pocket sized button. Unfortunately, this one does not launch an atomic bomb.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ai");
            item.quality = PickupObject.ItemQuality.B;
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 500f);
            item.consumable = false;

            AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
            string text = "assets/data/goops/oil goop.asset";
            GoopDefinition goopDefinition;
            try
            {
                GameObject gameObject = assetBundle.LoadAsset(text) as GameObject;
                goopDefinition = gameObject.GetComponent<GoopDefinition>();
            }
            catch
            {
                goopDefinition = (assetBundle.LoadAsset(text) as GoopDefinition);
            }
            goopDefinition.name = text.Replace("assets/data/goops/", "").Replace(".asset", "");
            Button.DefaultOilGoop = goopDefinition;


            List<string> mandatorySynergyItems = new List<string>() { "ai:multi-purpose_button", "gunther" };
            CustomSynergies.Add("Moral Support", mandatorySynergyItems);
            List<string> mandatorySynergyItems2 = new List<string>() { "ai:multi-purpose_button", "the_scrambler" };
            CustomSynergies.Add("Freshly Hatched", mandatorySynergyItems2);
            List<string> mandatorySynergyItems3 = new List<string>() { "ai:multi-purpose_button", "microtransaction_gun" };
            CustomSynergies.Add("MTX FTW", mandatorySynergyItems3);
            List<string> mandatorySynergyItems4 = new List<string>() { "ai:multi-purpose_button" };
            List<string> optionalSynergyItems4 = new List<string>() { "oiled_cylinder", "ai:oil_jar", "ai:barrel_bullets" };
            CustomSynergies.Add("Oil And Trouble", mandatorySynergyItems4, optionalSynergyItems4);
            List<string> MandatorySynergyItems5 = new List<string>() { "ai:multi-purpose_button", "trashcannon" };
            CustomSynergies.Add("In The Dumps", MandatorySynergyItems5);
            List<string> MandatorySynergyItems6 = new List<string>() { "ai:multi-purpose_button", "au_gun" };
            CustomSynergies.Add("Personal Assistant", MandatorySynergyItems6);
        }
        public static GoopDefinition DefaultOilGoop;
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }
        public override void DoEffect(PlayerController user)
        {
            AkSoundEngine.PostEvent("Play_OBJ_turret_set_01", base.gameObject);
            if (user.PlayerHasActiveSynergy("Moral Support"))
            {
                user.healthHaver.ApplyHealing(0.5f);
            }
            if (user.PlayerHasActiveSynergy("Freshly Hatched"))
            {
                SpawnGigi();
            }
            if (user.PlayerHasActiveSynergy("MTX FTW"))
            {
                int currencyamount = UnityEngine.Random.Range(5, 20);
                user.carriedConsumables.Currency += currencyamount;
            }
            if (user.PlayerHasActiveSynergy("Oil And Trouble"))
            {
                AddOilGoop(user);
            }
            if (user.PlayerHasActiveSynergy("In The Dumps"))
            {
                SpawnBlob();
            }
            if (user.PlayerHasActiveSynergy("Personal Assistant"))
            {
                SpawnKnight();
            }
        }
        private void AddOilGoop(PlayerController user)
        {
            DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(Button.DefaultOilGoop).TimedAddGoopCircle(user.specRigidbody.UnitCenter, 3f, 0.5f, false);
        }
        private void SpawnBlob()
        {
                var Blob = EnemyDatabase.GetOrLoadByGuid("e61cab252cfb435db9172adc96ded75f");
                IntVector2? spawnPos = LastOwner.CurrentRoom.GetRandomAvailableCell();
                if (spawnPos.HasValue)
                {
                    AIActor TargetActor = AIActor.Spawn(Blob.aiActor, spawnPos.Value, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(spawnPos.Value), true, AIActor.AwakenAnimationType.Default, true);
                    TargetActor.ApplyEffect(GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultPermanentCharmEffect, 1f, null);
                    PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(TargetActor.specRigidbody, null, false);
                    TargetActor.IsHarmlessEnemy = true;
                    TargetActor.IgnoreForRoomClear = true;
                    TargetActor.HandleReinforcementFallIntoRoom(0f);
                }
        }
        private void SpawnGigi()
        {
                var Gigi = EnemyDatabase.GetOrLoadByGuid("ed37fa13e0fa4fcf8239643957c51293");
                IntVector2? spawnPos = LastOwner.CurrentRoom.GetRandomAvailableCell();
                if (spawnPos.HasValue)
                {
                    AIActor TargetActor = AIActor.Spawn(Gigi.aiActor, spawnPos.Value, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(spawnPos.Value), true, AIActor.AwakenAnimationType.Default, true);
                    TargetActor.ApplyEffect(GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultPermanentCharmEffect, 1f, null);
                    PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(TargetActor.specRigidbody, null, false);
                    TargetActor.IsHarmlessEnemy = true;
                    TargetActor.IgnoreForRoomClear = true;
                    TargetActor.HandleReinforcementFallIntoRoom(0f);
                }
        }
        private void SpawnKnight()
        {
            var Knight = EnemyDatabase.GetOrLoadByGuid("463d16121f884984abe759de38418e48");
            IntVector2? spawnPos = LastOwner.CurrentRoom.GetRandomAvailableCell();
            if (spawnPos.HasValue)
            {
                AIActor TargetActor = AIActor.Spawn(Knight.aiActor, spawnPos.Value, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(spawnPos.Value), true, AIActor.AwakenAnimationType.Default, true);
                TargetActor.ApplyEffect(GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultPermanentCharmEffect, 1f, null);
                PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(TargetActor.specRigidbody, null, false);
                TargetActor.IsHarmlessEnemy = true;
                TargetActor.IgnoreForRoomClear = true;
                TargetActor.HandleReinforcementFallIntoRoom(0f);
            }
        }
    }
}