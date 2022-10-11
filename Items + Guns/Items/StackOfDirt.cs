using System.Collections.Generic;
using UnityEngine;
using Alexandria.ItemAPI;
using Dungeonator;
using System.Collections;


namespace GunRev
{
    public class StackOfDirt : PlayerItem
    {
        private Component self;
        public IEnumerator DirtDestroy(GameObject obj)
        {
            if (self == null)
            {
                self = obj.GetComponent<tk2dBaseSprite>();
            }
            if (obj.GetOrAddComponent<Help>().type == (int)BlockTypes.Dirt)
            {
                self.GetComponent<tk2dSpriteAnimator>().PlayFromFrame("dirt_destroy", 1);
                yield return new WaitForSeconds(1);
                AkSoundEngine.PostEvent("Play_dirt_break", obj);
            }
            else if (obj.GetOrAddComponent<Help>().type == (int)BlockTypes.Cobble)
            {
                self.GetComponent<tk2dSpriteAnimator>().PlayFromFrame("cobble_destroy", 1);
                yield return new WaitForSeconds(1);
                AkSoundEngine.PostEvent("Play_generic_break", obj);
            }
            else if (obj.GetOrAddComponent<Help>().type == (int)BlockTypes.Brick)
            {
                self.GetComponent<tk2dSpriteAnimator>().PlayFromFrame("brick_destroy", 1);
                yield return new WaitForSeconds(1);
                AkSoundEngine.PostEvent("Play_generic_break", obj);
                self.GetComponent<tk2dSprite>().specRigidbody.AddCollisionLayerIgnoreOverride((int)CollisionLayer.Projectile);
                Projectile projectile = ((Gun)ETGMod.Databases.Items[372]).DefaultModule.projectiles[0];
                for (int i = 0; i > 4; i++)
                {
                    GameObject obj1 = SpawnManager.SpawnProjectile(projectile.gameObject, self.GetComponent<tk2dSprite>().sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.LastOwner.CurrentGun == null) ? 0f : i * 90), true);
                    Projectile proj1 = obj1.GetComponent<Projectile>();
                    proj1.Owner = base.LastOwner;
                    proj1.Shooter = self.GetComponent<tk2dSprite>().specRigidbody;
                }
            }
            else if (obj.GetOrAddComponent<Help>().type == (int)BlockTypes.StoneBrick)
            {
                self.GetComponent<tk2dSpriteAnimator>().PlayFromFrame("stonebrick_destroy", 1);
                yield return new WaitForSeconds(1);
                AkSoundEngine.PostEvent("Play_generic_break", obj);
                self.GetComponent<tk2dSprite>().specRigidbody.AddCollisionLayerIgnoreOverride((int)CollisionLayer.Projectile);
                Projectile projectile = ((Gun)ETGMod.Databases.Items[372]).DefaultModule.projectiles[0];
                for (int i = 0; i > 5; i++)
                {
                    GameObject obj1 = SpawnManager.SpawnProjectile(projectile.gameObject, self.GetComponent<tk2dSprite>().sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.LastOwner.CurrentGun == null) ? 0f : i * 72), true);
                    Projectile proj1 = obj1.GetComponent<Projectile>();
                    proj1.Owner = base.LastOwner;
                    proj1.Shooter = self.GetComponent<tk2dSprite>().specRigidbody;
                }
            }
            if (obj != null)
            {
                Destroy(obj);
            }
            if (obj == null)
            {
                yield break;
            }
        }
        public static void Init()
        {
            //The name of the item
            string itemName = "Stack Of Dirt";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it.
            string resourceName = "GunRev/Resources/blocks/dirt";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<StackOfDirt>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "In The Mud";
            string longDesc = "Dirt can be used to form walls and trap enemies. It's pretty weak, though.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"kts" here is the item pool. In the console you'd type kts:sweating_bullets
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ai");
            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Timed, 64f);
            //Adds a passive modifier, like curse, coolness, damage, etc. to the item. Works for passives and actives.
            //Set some other fields
            item.consumable = false;
            item.UsesNumberOfUsesBeforeCooldown = true;
            item.numberOfUses = 64;
            item.quality = ItemQuality.C;
            BuildBlocks();
        }
        public static GameObject Block;
        public static List<int> spriteIds = new List<int>();
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }
        public static void BuildBlocks()
        {
            Block = SpriteBuilder.SpriteFromResource("GunRev/Resources/blocks/dirt_idle_001");
            tk2dBaseSprite vfxSprite = Block.GetComponent<tk2dBaseSprite>();
            vfxSprite.GetCurrentSpriteDef().ConstructOffsetsFromAnchor(tk2dBaseSprite.Anchor.MiddleCenter, vfxSprite.GetCurrentSpriteDef().position3);
            FakePrefab.MarkAsFakePrefab(Block);
            UnityEngine.Object.DontDestroyOnLoad(Block);
            Block.SetActive(false);
            List<string> DirtIdleSprites = new List<string>
                {
                    "dirt_idle_001.png"
                };
            List<string> DirtDestroySprites = new List<string>
                {
                    "dirt_destroy_001",
                    "dirt_destroy_002",
                    "dirt_destroy_003",
                    "dirt_destroy_004",
                    "dirt_destroy_005",
                    "dirt_destroy_006",
                    "dirt_destroy_007",
                    "dirt_destroy_008"
                };
            List<string> CobbleIdleSprites = new List<string>
                {
                    "cobble_idle_001.png"
                };
            List<string> CobbleDestroySprites = new List<string>
                {
                    "cobble_destroy_001",
                    "cobble_destroy_002",
                    "cobble_destroy_003",
                    "cobble_destroy_004",
                    "cobble_destroy_005",
                    "cobble_destroy_006",
                    "cobble_destroy_007",
                    "cobble_destroy_008"
                };
            List<string> BrickIdleSprites = new List<string>
                {
                    "brick_idle_001.png"
                };
            List<string> BrickDestroySprites = new List<string>
                {
                    "brick_destroy_001",
                    "brick_destroy_002",
                    "brick_destroy_003",
                    "brick_destroy_004",
                    "brick_destroy_005",
                };
            List<string> StoneBrickIdleSprites = new List<string>
                {
                    "cobble_idle_001.png"
                };
            List<string> StoneBrickDestroySprites = new List<string>
                {
                    "stonebrick_destroy_001",
                    "brick_destroy_002",
                    "brick_destroy_003",
                    "brick_destroy_004",
                    "brick_destroy_005",
                };
            var collection = Block.GetComponent<tk2dSprite>().Collection;
            var dirtidleIdsList = new List<int>();
            foreach (string sprite in DirtIdleSprites)
            {
                dirtidleIdsList.Add(SpriteBuilder.AddSpriteToCollection("GunRev/Resources/blocks/" + sprite, collection));
            }
            var dirtdestroyIdsList = new List<int>();
            foreach (string sprite in DirtDestroySprites)
            {
                dirtdestroyIdsList.Add(SpriteBuilder.AddSpriteToCollection("GunRev/Resources/blocks/" + sprite, collection));
            }
            var cobbleidleIdsList = new List<int>();
            foreach (string sprite in CobbleIdleSprites)
            {
                cobbleidleIdsList.Add(SpriteBuilder.AddSpriteToCollection("GunRev/Resources/blocks/" + sprite, collection));
            }
            var cobbledestroyIdsList = new List<int>();
            foreach (string sprite in CobbleDestroySprites)
            {
                cobbledestroyIdsList.Add(SpriteBuilder.AddSpriteToCollection("GunRev/Resources/blocks/" + sprite, collection));
            }
            var brickidleIdsList = new List<int>();
            foreach (string sprite in BrickIdleSprites)
            {
                brickidleIdsList.Add(SpriteBuilder.AddSpriteToCollection("GunRev/Resources/blocks/" + sprite, collection));
            }
            var brickdestroyIdsList = new List<int>();
            foreach (string sprite in BrickDestroySprites)
            {
                brickdestroyIdsList.Add(SpriteBuilder.AddSpriteToCollection("GunRev/Resources/blocks/" + sprite, collection));
            }
            var stonebrickidleIdsList = new List<int>();
            foreach (string sprite in StoneBrickIdleSprites)
            {
                stonebrickidleIdsList.Add(SpriteBuilder.AddSpriteToCollection("GunRev/Resources/blocks/" + sprite, collection));
            }
            var stonebrickdestroyIdsList = new List<int>();
            foreach (string sprite in StoneBrickDestroySprites)
            {
                stonebrickdestroyIdsList.Add(SpriteBuilder.AddSpriteToCollection("GunRev/Resources/blocks/" + sprite, collection));
            }
            tk2dSpriteAnimator spriteAnimator = Block.AddComponent<tk2dSpriteAnimator>();
            spriteAnimator.playAutomatically = false;
            SpriteBuilder.AddAnimation(spriteAnimator, collection, dirtidleIdsList, "dirt_idle", tk2dSpriteAnimationClip.WrapMode.Loop, 8);
            SpriteBuilder.AddAnimation(spriteAnimator, collection, dirtdestroyIdsList, "dirt_destroy", tk2dSpriteAnimationClip.WrapMode.Once, 8);
            SpriteBuilder.AddAnimation(spriteAnimator, collection, cobbleidleIdsList, "cobble_idle", tk2dSpriteAnimationClip.WrapMode.Loop, 8);
            SpriteBuilder.AddAnimation(spriteAnimator, collection, cobbledestroyIdsList, "cobble_destroy", tk2dSpriteAnimationClip.WrapMode.Once, 8);
            SpriteBuilder.AddAnimation(spriteAnimator, collection, brickidleIdsList, "brick_idle", tk2dSpriteAnimationClip.WrapMode.Loop, 8);
            SpriteBuilder.AddAnimation(spriteAnimator, collection, brickdestroyIdsList, "brick_destroy", tk2dSpriteAnimationClip.WrapMode.Once, 5);
            SpriteBuilder.AddAnimation(spriteAnimator, collection, stonebrickidleIdsList, "stonebrick_idle", tk2dSpriteAnimationClip.WrapMode.Loop, 8);
            SpriteBuilder.AddAnimation(spriteAnimator, collection, stonebrickdestroyIdsList, "stonebrick_destroy", tk2dSpriteAnimationClip.WrapMode.Once, 4);
            Help stuff = Block.GetOrAddComponent<Help>();
        }
        private void OnPreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myCollider, SpeculativeRigidbody other, PixelCollider otherCollider)
        {
            if (otherCollider.CollisionLayer == CollisionLayer.Projectile)
            {
                myRigidbody.gameObject.GetComponent<Help>().hp--;
                if (myRigidbody.gameObject.GetComponent<Help>().hp == 0 && myRigidbody.gameObject.GetComponent<Help>().alive)
                {
                    StartCoroutine(DirtDestroy(myRigidbody.gameObject));
                    myRigidbody.gameObject.GetComponent<Help>().alive = false;
                }
            }
        }
        public override void DoEffect(PlayerController user)
        {
            IntVector2 intVector = LastOwner.specRigidbody.UnitCenter.ToIntVector2(VectorConversions.Floor);
            if (GameManager.Instance.Dungeon.data.CheckInBounds(intVector))
            {
                CellData cellData = GameManager.Instance.Dungeon.data[intVector];
                if (cellData != null && cellData.type == CellType.FLOOR)
                {
                    blockObject = UnityEngine.Object.Instantiate<GameObject>(Block, intVector.ToVector3(), Quaternion.identity);
                    Help stuff = blockObject.GetComponent<Help>();
                    stuff.gameobj = blockObject;
                    stuff.alive = true;
                    stuff.body = ToolsOther.GenerateOrAddToRigidBody(blockObject, CollisionLayer.HighObstacle, PixelCollider.PixelColliderGeneration.Manual, false, true, false, false, false, false, false, true, new IntVector2(16, 16));
                    PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(stuff.body, null, false);
                    if (!LastOwner.PlayerHasActiveSynergy("Brick Block") && !LastOwner.PlayerHasActiveSynergy("Cobblestone"))
                    {
                        blockObject.GetComponent<tk2dSpriteAnimator>().PlayFromFrame("dirt_idle", 1);
                        stuff.type = (int)BlockTypes.Dirt;
                        stuff.hp = 2;
                    }
                    else if (LastOwner.PlayerHasActiveSynergy("Brick Block") && LastOwner.PlayerHasActiveSynergy("Cobblestone"))
                    {
                        blockObject.GetComponent<tk2dSpriteAnimator>().PlayFromFrame("stonebrick_idle", 1);
                        stuff.type = (int)BlockTypes.StoneBrick;
                        stuff.hp = 5;
                    }
                    else if (LastOwner.PlayerHasActiveSynergy("Brick Block") && !LastOwner.PlayerHasActiveSynergy("Cobblestone"))
                    {
                        blockObject.GetComponent<tk2dSpriteAnimator>().PlayFromFrame("brick_idle", 1);
                        stuff.type = (int)BlockTypes.Brick;
                        stuff.hp = 2;
                    }
                    else if (LastOwner.PlayerHasActiveSynergy("Cobblestone") && !LastOwner.PlayerHasActiveSynergy("Brick Block"))
                    {
                        blockObject.GetComponent<tk2dSpriteAnimator>().PlayFromFrame("cobble_idle", 1);
                        stuff.type = (int)BlockTypes.Cobble;
                        stuff.hp = 4;
                    }
                    else
                    {
                        blockObject.GetComponent<tk2dSpriteAnimator>().PlayFromFrame("stonebrick_idle", 1);
                        stuff.type = (int)BlockTypes.StoneBrick;
                        stuff.hp = 5;
                    }
                    stuff.body.OnPreRigidbodyCollision += OnPreCollision;
                }
            }
        }
        private GameObject blockObject;
        public override void OnPreDrop(PlayerController user)
        {
            base.OnPreDrop(user);
        }
        private enum BlockTypes
        {
            Dirt,
            Cobble,
            Brick,
            StoneBrick
        }
    }
    public class Help : MonoBehaviour
    {
        public Help()
        {
            alive = false;
            hp = 0;
            type = 0;
            body = null;
            gameobj = null;
        }
        private void Start()
        {
            this.gameobj = base.gameObject;
        }
        public bool alive;
        public int hp;
        public int type;
        public SpeculativeRigidbody body;
        public GameObject gameobj;
    }
}