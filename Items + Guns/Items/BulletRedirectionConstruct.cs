using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;
using Dungeonator;
using System.Collections;

namespace GunRev
{
    public class BulletRedirectionConstruct : AdvancedPlayerOrbitalItem
    {
        public static PlayerOrbital orbitalPrefab;
        public static PlayerOrbital upgradeOrbitalPrefab;
        public static void Init()
        {
            string itemName = "Bullet Redirection Construct";
            string resourceName = "GunRev/Resources/bulletredirector";

            GameObject obj = new GameObject();
            var item = obj.AddComponent<BulletRedirectionConstruct>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Make It Go The Other Way";
            string longDesc = "When shot with one of your bullets, gives the bullet homing cababilities and sets it to target the nearest enemy.\n\nWARNING: Always ensure the marked arrows on the side of the Construct point away from you at all times. Warranty void if the user fails to adhere to these precautions, and Explosive Engineering Corp will not be held accountable for any damage caused through incorrect usage.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ai");
            item.quality = PickupObject.ItemQuality.B;

            BuildPrefab();
            item.OrbitalPrefab = orbitalPrefab;
            BuildSynergyPrefab();

            item.HasAdvancedUpgradeSynergy = true; //Set this to true if you want a synergy that changes the appearance of the Guon Stone. All base game guons have a [colour]-er Guon Stone synergy that makes them bigger and brighter.
            item.AdvancedUpgradeSynergy = "Bullet Precision Construct"; //This is the name of the synergy that changes the appearance, if you have one.
            item.AdvancedUpgradeOrbitalPrefab = BulletRedirectionConstruct.upgradeOrbitalPrefab.gameObject;
        }

        public static void BuildPrefab()
        {
            if (BulletRedirectionConstruct.orbitalPrefab != null) return;
            GameObject prefab = SpriteBuilder.SpriteFromResource("GunRev/Resources/redirectionguon"); //(ingame orbital sprite)MAKE SURE TO CHANGE THE SPRITE PATH TO YOUR MODS RESOURCES
            prefab.name = "Bullet Redirection Construct"; //The name of the orbital used by the code. Barely ever used or seen, but important to change.
            var body = prefab.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(IntVector2.Zero, new IntVector2(7, 7)); //This line sets up the hitbox of your guon, this one is set to 5 pixels across by 9 pixels high, but you can set it as big or small as you want your guon to be.           
            body.CollideWithTileMap = false;
            body.CollideWithOthers = true;
            body.PrimaryPixelCollider.CollisionLayer = CollisionLayer.BulletBlocker;
            orbitalPrefab = prefab.AddComponent<PlayerOrbital>();
            orbitalPrefab.motionStyle = PlayerOrbital.OrbitalMotionStyle.ORBIT_PLAYER_ALWAYS; //You can ignore most of this stuff, but I've commented on some of it.
            orbitalPrefab.shouldRotate = false; //This determines if the guon stone rotates. If set to true, the stone will rotate so that it always faces towards the player. Most Guons have this set to false, and you probably should too unless you have a good reason for changing it.
            orbitalPrefab.orbitRadius = 2.5f; //This determines how far away from you the guon orbits. The default for most guons is 2.5.
            orbitalPrefab.orbitDegreesPerSecond = 90f; //This determines how many degrees of rotation the guon travels per second. The default for most guons is 120.
            orbitalPrefab.perfectOrbitalFactor = 0f; //This determines how fast guons will move to catch up with their owner (regular guons have it set to 0 so they lag behind). You can probably ignore this unless you want or need your guon to stick super strictly to it's orbit.
            orbitalPrefab.SetOrbitalTier(0);

            GameObject.DontDestroyOnLoad(prefab);
            FakePrefab.MarkAsFakePrefab(prefab);
            prefab.SetActive(false);
        }
        public static void BuildSynergyPrefab()
        {
            bool flag = BulletRedirectionConstruct.upgradeOrbitalPrefab == null;
            if (flag)
            {
                GameObject gameObject = SpriteBuilder.SpriteFromResource("GunRev/Resources/precisionguon", null); //(The orbital appearance with it's special synergy) MAKE SURE TO CHANGE THE SPRITE PATH TO YOUR OWN MODS
                gameObject.name = "Bullet Precision Construct";
                SpeculativeRigidbody speculativeRigidbody = gameObject.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(IntVector2.Zero, new IntVector2(7, 7));
                BulletRedirectionConstruct.upgradeOrbitalPrefab = gameObject.AddComponent<PlayerOrbital>();
                speculativeRigidbody.CollideWithTileMap = false;
                speculativeRigidbody.CollideWithOthers = true;
                speculativeRigidbody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.BulletBlocker;
                BulletRedirectionConstruct.upgradeOrbitalPrefab.shouldRotate = false; //Determines if your guon rotates with it's special synergy
                BulletRedirectionConstruct.upgradeOrbitalPrefab.orbitRadius = 2.5f; //Determines how far your guon orbits with it's special synergy
                BulletRedirectionConstruct.upgradeOrbitalPrefab.orbitDegreesPerSecond = 90f; //Determines how fast your guon orbits with it's special synergy
                BulletRedirectionConstruct.upgradeOrbitalPrefab.perfectOrbitalFactor = 10f; //Determines how fast your guon will move to catch up with its owner with it's special synergy. By default, even though the regular guons have it at 0, the upgraded synergy guons all have a higher perfectOrbitalFactor. I find 10 to be about the same.
                BulletRedirectionConstruct.upgradeOrbitalPrefab.SetOrbitalTier(0);
                UnityEngine.Object.DontDestroyOnLoad(gameObject);
                FakePrefab.MarkAsFakePrefab(gameObject);
                gameObject.SetActive(false);
            }
        }
        public static void PointTowardsNearestEnemy(Projectile projectile)
        {
            float dist = 0f;
            List<AIActor> list = new List<AIActor>();
            var room = projectile.Owner.GetAbsoluteParentRoom();
            foreach (var enemy in room.GetActiveEnemies(RoomHandler.ActiveEnemyType.All))
            {
                list.Add(enemy);
            }
            AIActor them = GetNearestEnemy(list, projectile.gameObject.transform.PositionVector2(), out dist, new string[] {}, 999999999999999999f);
            Vector2 targetdirection = them.gameObject.transform.PositionVector2() - projectile.transform.PositionVector2();
            projectile.SendInDirection(targetdirection, false);
        }
        public static AIActor GetNearestEnemy(List<AIActor> activeEnemies, Vector2 position, out float nearestDistance, string[] filter, float range)
        {
            AIActor aiactor = null;
            nearestDistance = range;
            AIActor result;
            if (activeEnemies == null || activeEnemies.Count == 0)
            {
                result = null;
            }
            else
            {
                for (int i = 0; i < activeEnemies.Count; i++)
                {
                    AIActor aiactor2 = activeEnemies[i];
                    if (!aiactor2.healthHaver.IsDead && aiactor2.healthHaver.IsVulnerable)
                    {
                        if (filter == null || !filter.Contains(aiactor2.EnemyGuid))
                        {
                            float num = Vector2.Distance(position, aiactor2.CenterPosition);
                            if (num < nearestDistance)
                            {
                                nearestDistance = num;
                                aiactor = aiactor2;
                            }
                        }
                    }
                }
                result = aiactor;
            }
            return result;
        }
        private void OnPreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myCollider, SpeculativeRigidbody other, PixelCollider otherCollider)
        {
            if (other.projectile && other.projectile.Owner is PlayerController)
            {
                if (!other.projectile.GetComponent<BulletRedirectedByGuon>())
                {
                    Projectile projectile = other.projectile;
                    projectile.gameObject.AddComponent<BulletRedirectedByGuon>();
                    if (Owner.PlayerHasActiveSynergy("Bullet Precision Construct"))
                    {
                        HomingModifier bighoming = projectile.gameObject.GetOrAddComponent<HomingModifier>();
                        bighoming.HomingRadius = 20;
                        bighoming.AngularVelocity = 50;
                    }
                    else
                    {
                        HomingModifier homing = projectile.gameObject.GetOrAddComponent<HomingModifier>();
                        homing.HomingRadius = 5;
                        homing.AngularVelocity = 20;
                    }
                    PointTowardsNearestEnemy(projectile);
                }
                PhysicsEngine.SkipCollision = true;
            }
            else if (other.projectile && !(other.projectile.Owner is PlayerController))
            {
                PhysicsEngine.SkipCollision = true;
            }
        }
        public override void OnOrbitalCreated(GameObject orbital)
        {
            if (orbital.GetComponent<SpeculativeRigidbody>())
            {
                SpeculativeRigidbody specRigidbody = orbital.GetComponent<SpeculativeRigidbody>();
                specRigidbody.OnPreRigidbodyCollision += OnPreCollision;
            }
            base.OnOrbitalCreated(orbital);
        }
        public override void Update()
        {
            base.Update();
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            return base.Drop(player);
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
        }
        public class BulletRedirectedByGuon : MonoBehaviour { }
    }
}