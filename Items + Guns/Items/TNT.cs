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
    public class TNTItem : PlayerItem
    {
        private Component self;
        public IEnumerator Boom()
        {
            if (self == null)
            {
                self = tntObject.GetComponent<tk2dBaseSprite>();
            }
            self.GetComponent<tk2dSpriteAnimator>().PlayFromFrame("tnt_prime", 1);
            AkSoundEngine.PostEvent("Play_tnt_fuse", tntObject);
            yield return new WaitForSeconds(1);
            Exploder.Explode(tntObject.GetComponent<tk2dSprite>().WorldCenter, DungeonDatabase.GetOrLoadByName("base_castle").sharedSettingsPrefab.DefaultExplosionData, tntObject.GetComponent<tk2dSprite>().WorldCenter);
            AkSoundEngine.PostEvent("Play_tnt_boom", tntObject);
            Destroy(tntObject);
            if (tntObject == null)
            {
                yield break;
            }
        }
        public static void Init()
        {
            //The name of the item
            string itemName = "TNT";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it.
            string resourceName = "GunRev/Resources/tnt/tnt";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<TNTItem>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "SSSSS...";
            string longDesc = "Places an explosive block of TNT that can be primed by shooting it.\n\n\"We took this from an ancient temple. Here's hoping the pharoah doesn't get mad.\"";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"kts" here is the item pool. In the console you'd type kts:sweating_bullets
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ai");
            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 1500f);
            //Adds a passive modifier, like curse, coolness, damage, etc. to the item. Works for passives and actives.
            //Set some other fields
            item.consumable = false;
            item.quality = ItemQuality.B;
            BuildTNT();
        }
        public static GameObject TNT;
        public static List<int> spriteIds = new List<int>();
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }
        public static void BuildTNT()
        {
            TNT = SpriteBuilder.SpriteFromResource("GunRev/Resources/tnt/tnt_idle_001");
            tk2dBaseSprite vfxSprite = TNT.GetComponent<tk2dBaseSprite>();
            vfxSprite.GetCurrentSpriteDef().ConstructOffsetsFromAnchor(tk2dBaseSprite.Anchor.MiddleCenter, vfxSprite.GetCurrentSpriteDef().position3);
            FakePrefab.MarkAsFakePrefab(TNT);
            UnityEngine.Object.DontDestroyOnLoad(TNT);
            TNT.SetActive(false);
            List<string> IdleSprites = new List<string>
                {
                    "tnt_idle_001.png"
                };
            List<string> PrimeSprites = new List<string>
                {
                    "tnt_prime_001",
                    "tnt_prime_002",
                    "tnt_prime_003",
                    "tnt_prime_004",
                    "tnt_prime_005",
                    "tnt_prime_006",
                    "tnt_prime_007",
                    "tnt_prime_008",
                    "tnt_prime_009",
                    "tnt_prime_010"
                };
            var collection = TNT.GetComponent<tk2dSprite>().Collection;
            var idleIdsList = new List<int>();
            foreach (string sprite in IdleSprites)
            {
                idleIdsList.Add(SpriteBuilder.AddSpriteToCollection("GunRev/Resources/tnt/" + sprite, collection));
            }
            var primeIdsList = new List<int>();
            foreach (string sprite in PrimeSprites)
            {
                primeIdsList.Add(SpriteBuilder.AddSpriteToCollection("GunRev/Resources/tnt/" + sprite, collection));
            }
            tk2dSpriteAnimator spriteAnimator = TNT.AddComponent<tk2dSpriteAnimator>();
            spriteAnimator.playAutomatically = false;
            SpriteBuilder.AddAnimation(spriteAnimator, collection, idleIdsList, "tnt_idle", tk2dSpriteAnimationClip.WrapMode.Loop, 8);
            SpriteBuilder.AddAnimation(spriteAnimator, collection, primeIdsList, "tnt_prime", tk2dSpriteAnimationClip.WrapMode.Once, 10);
        }
        private void OnPreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myCollider, SpeculativeRigidbody other, PixelCollider otherCollider)
        {
            if (otherCollider.CollisionLayer == CollisionLayer.Projectile)
            {
                StartCoroutine(Boom());
            }
        }
        public override void DoEffect(PlayerController user)
        {
            tntObject = UnityEngine.Object.Instantiate<GameObject>(TNT, user.sprite.WorldCenter, Quaternion.identity);
            var body = tntObject.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(IntVector2.Zero, new IntVector2(16, 20));
            body.CollideWithTileMap = false;
            body.CollideWithOthers = true;
            body.PrimaryPixelCollider.CollisionLayer = CollisionLayer.BulletBreakable;
            tntObject.GetComponent<tk2dSpriteAnimator>().PlayFromFrame("tnt_idle", 1);
            body.OnPreRigidbodyCollision += OnPreCollision;
        }
        private GameObject tntObject;
        public override void OnPreDrop(PlayerController user)
        {
            base.OnPreDrop(user);
        }
        public override void OnDestroy()
        {
            if (tntObject != null)
            {
                Destroy(tntObject);
            }
            base.OnDestroy();
        }
    }
}