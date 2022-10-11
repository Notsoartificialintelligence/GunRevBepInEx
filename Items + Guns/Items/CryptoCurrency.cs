using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;
using Dungeonator;
using System.Reflection;
using Random = System.Random;
using FullSerializer;
using System.Collections;
using Gungeon;
using MonoMod.RuntimeDetour;
using MonoMod;


namespace GunRev
{
    public class Cryptocurrency : PlayerItem
    {
        private Component self;
        private int hp = 5;
        public IEnumerator MoneyMoneyMoney()
        {
            if (self == null)
            {
                self = turretObject.GetComponent<tk2dBaseSprite>();
            }
            self.GetComponent<tk2dSpriteAnimator>().PlayFromFrame("crypturret_intro", 1);
            yield return new WaitForSeconds(1);
            self.GetComponent<tk2dSpriteAnimator>().PlayFromFrame("crypturret_idle", 1);
            while (turretObject != null)
            {
                yield return new WaitForSeconds(2);
                LootEngine.SpawnCurrency(turretObject.GetComponent<tk2dSprite>().WorldCenter, 1);
            }
            if (turretObject == null)
            {
                yield break;
            }
        }
        public IEnumerator Target(PlayerController user)
        {
            var room = user.GetAbsoluteParentRoom();
            while (turretObject != null)
            {
                foreach (var enemy in room.GetActiveEnemies(RoomHandler.ActiveEnemyType.All))
                {
                    enemy.OverrideTarget = turretObject.GetComponent<SpeculativeRigidbody>().specRigidbody;
                }
                yield return new WaitForSeconds(1);
            }
            if (turretObject == null)
            {
                foreach (var enemy in room.GetActiveEnemies(RoomHandler.ActiveEnemyType.All))
                {
                    enemy.OverrideTarget = turretObject.GetComponent<SpeculativeRigidbody>().specRigidbody;
                }
                yield break;
            }
        }
        public static void Init()
        {
            //The name of the item
            string itemName = "Cryptocurrency";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it.
            string resourceName = "GunRev/Resources/crypturret/crypto";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<Cryptocurrency>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "An Investment";
            string longDesc = "Places a crypto server at the cost of 10 casings. The server generates casings as long as the current room is active. Enemies will go out of their way to neutralise the server. Watch out for explosions.\n\nDISCLAIMER: NotSoAI does not endorse or support cryptocurrency mining. This is a game about a firearms themed dungeon, do not take it seriously.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"kts" here is the item pool. In the console you'd type kts:sweating_bullets
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ai");
            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.PerRoom, 3f);
            //Adds a passive modifier, like curse, coolness, damage, etc. to the item. Works for passives and actives.
            //Set some other fields
            item.consumable = false;
            item.quality = ItemQuality.B;
            BuildTurret();
        }
        public static GameObject Crypturret;
        public static List<int> spriteIds = new List<int>();
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnRoomClearEvent += this.OnRoomClear;
        }
        public static void BuildTurret()
        {
            Crypturret = SpriteBuilder.SpriteFromResource("GunRev/Resources/crypturret/crypturret_idle_001");
            tk2dBaseSprite vfxSprite = Crypturret.GetComponent<tk2dBaseSprite>();
            vfxSprite.GetCurrentSpriteDef().ConstructOffsetsFromAnchor(tk2dBaseSprite.Anchor.MiddleCenter, vfxSprite.GetCurrentSpriteDef().position3);
            FakePrefab.MarkAsFakePrefab(Crypturret);
            UnityEngine.Object.DontDestroyOnLoad(Crypturret);
            Crypturret.SetActive(false);
            List<string> IdleSprites = new List<string>
                {
                    "crypturret_idle_001.png",
                    "crypturret_idle_002.png"
                };
            List<string> IntroSprites = new List<string>
                {
                    "crypturret_intro_001.png",
                    "crypturret_intro_002.png",
                    "crypturret_intro_003.png",
                    "crypturret_intro_004.png",
                    "crypturret_intro_005.png",
                    "crypturret_intro_006.png",
                    "crypturret_intro_007.png",
                    "crypturret_intro_008.png"
                };
            var collection = Crypturret.GetComponent<tk2dSprite>().Collection;
            var idleIdsList = new List<int>();
            foreach (string sprite in IdleSprites)
            {
                idleIdsList.Add(SpriteBuilder.AddSpriteToCollection("GunRev/Resources/crypturret/" + sprite, collection));
            }
            var introIdsList = new List<int>();
            foreach (string sprite in IntroSprites)
            {
                introIdsList.Add(SpriteBuilder.AddSpriteToCollection("GunRev/Resources/crypturret/" + sprite, collection));
            }
            tk2dSpriteAnimator spriteAnimator = Crypturret.AddComponent<tk2dSpriteAnimator>();
            spriteAnimator.playAutomatically = false;
            SpriteBuilder.AddAnimation(spriteAnimator, collection, idleIdsList, "crypturret_idle", tk2dSpriteAnimationClip.WrapMode.Loop, 8);
            SpriteBuilder.AddAnimation(spriteAnimator, collection, introIdsList, "crypturret_intro", tk2dSpriteAnimationClip.WrapMode.Once, 8);
        }
        private void OnPreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myCollider, SpeculativeRigidbody other, PixelCollider otherCollider)
        {
            hp -= 1;
            if (hp <= 0)
            {
                Exploder.Explode(turretObject.GetComponent<tk2dSprite>().WorldCenter, DungeonDatabase.GetOrLoadByName("base_castle").sharedSettingsPrefab.DefaultExplosionData, turretObject.GetComponent<tk2dSprite>().WorldCenter);
                Destroy(turretObject);
            }
        }
        private void OnRoomClear(PlayerController user)
        {
            if (turretObject != null)
            {
                Exploder.Explode(turretObject.GetComponent<tk2dSprite>().WorldCenter, DungeonDatabase.GetOrLoadByName("base_castle").sharedSettingsPrefab.DefaultExplosionData, turretObject.GetComponent<tk2dSprite>().WorldCenter);
                Destroy(turretObject);
            }
        }
        public override bool CanBeUsed(PlayerController user)
        {
            return user.IsInCombat && user.carriedConsumables.Currency >= 10;
        }
        public override void DoEffect(PlayerController user)
        {
            AkSoundEngine.PostEvent("Play_OBJ_turret_set_01", gameObject);
            user.carriedConsumables.Currency -= 10;
            hp = 5;
            turretObject = UnityEngine.Object.Instantiate<GameObject>(Crypturret, user.sprite.WorldCenter, Quaternion.identity);
            var body = turretObject.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(IntVector2.Zero, new IntVector2(24, 20));
            body.CollideWithTileMap = false;
            body.CollideWithOthers = true;
            body.PrimaryPixelCollider.CollisionLayer = CollisionLayer.EnemyBulletBlocker;
            body.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(body.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.OnPreCollision));
            StartCoroutine(MoneyMoneyMoney());
            StartCoroutine(Target(user));
        }
        private GameObject turretObject;
        public override void OnPreDrop(PlayerController user)
        {
            base.OnPreDrop(user);
            user.OnRoomClearEvent -= this.OnRoomClear;
            if (turretObject != null)
            {
                Exploder.Explode(turretObject.GetComponent<tk2dSprite>().WorldCenter, DungeonDatabase.GetOrLoadByName("base_castle").sharedSettingsPrefab.DefaultExplosionData, turretObject.GetComponent<tk2dSprite>().WorldCenter);
                Destroy(turretObject);
            }
        }
        public override void OnDestroy()
        {
            if (turretObject != null)
            {
                Exploder.Explode(turretObject.GetComponent<tk2dSprite>().WorldCenter, DungeonDatabase.GetOrLoadByName("base_castle").sharedSettingsPrefab.DefaultExplosionData, turretObject.GetComponent<tk2dSprite>().WorldCenter);
                Destroy(turretObject);
            }
            base.OnDestroy();
        }
    }
}