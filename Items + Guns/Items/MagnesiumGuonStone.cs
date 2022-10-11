using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;

namespace GunRev
{
    public class MagnesiumGuonStone : AdvancedPlayerOrbitalItem
    {
        public static PlayerOrbital orbitalPrefab;
        public static PlayerOrbital upgradeOrbitalPrefab;
        public static void Init()
        {
            string itemName = "Magnesium Guon Stone";
            string resourceName = "GunRev/Resources/magnesiumguonstone";

            GameObject obj = new GameObject();
            var item = obj.AddComponent<MagnesiumGuonStone>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "OOOH, BURN!";
            string longDesc = "Gives bullets a chance to explode on hit.\n\nMagnesium is a metal that is known for its reactive nature in air, and its bright flame it produces.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ai");
            item.quality = PickupObject.ItemQuality.A;

            BuildPrefab();
            item.OrbitalPrefab = orbitalPrefab;
            BuildSynergyPrefab();

            item.HasAdvancedUpgradeSynergy = true; //Set this to true if you want a synergy that changes the appearance of the Guon Stone. All base game guons have a [colour]-er Guon Stone synergy that makes them bigger and brighter.
            item.AdvancedUpgradeSynergy = "Explosiver Guon Stone"; //This is the name of the synergy that changes the appearance, if you have one.
            item.AdvancedUpgradeOrbitalPrefab = MagnesiumGuonStone.upgradeOrbitalPrefab.gameObject;
        }

        public static void BuildPrefab()
        {
            if (MagnesiumGuonStone.orbitalPrefab != null) return;
            GameObject prefab = SpriteBuilder.SpriteFromResource("GunRev/Resources/magnesiumguonorbital"); //(ingame orbital sprite)MAKE SURE TO CHANGE THE SPRITE PATH TO YOUR MODS RESOURCES
            prefab.name = "Magnesium Guon Orbital"; //The name of the orbital used by the code. Barely ever used or seen, but important to change.
            var body = prefab.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(IntVector2.Zero, new IntVector2(5, 9)); //This line sets up the hitbox of your guon, this one is set to 5 pixels across by 9 pixels high, but you can set it as big or small as you want your guon to be.           
            body.CollideWithTileMap = false;
            body.CollideWithOthers = true;
            body.PrimaryPixelCollider.CollisionLayer = CollisionLayer.EnemyBulletBlocker;

            orbitalPrefab = prefab.AddComponent<PlayerOrbital>();
            orbitalPrefab.motionStyle = PlayerOrbital.OrbitalMotionStyle.ORBIT_PLAYER_ALWAYS; //You can ignore most of this stuff, but I've commented on some of it.
            orbitalPrefab.shouldRotate = false; //This determines if the guon stone rotates. If set to true, the stone will rotate so that it always faces towards the player. Most Guons have this set to false, and you probably should too unless you have a good reason for changing it.
            orbitalPrefab.orbitRadius = 2.5f; //This determines how far away from you the guon orbits. The default for most guons is 2.5.
            orbitalPrefab.orbitDegreesPerSecond = 120f; //This determines how many degrees of rotation the guon travels per second. The default for most guons is 120.
            orbitalPrefab.perfectOrbitalFactor = 0f; //This determines how fast guons will move to catch up with their owner (regular guons have it set to 0 so they lag behind). You can probably ignore this unless you want or need your guon to stick super strictly to it's orbit.
            orbitalPrefab.SetOrbitalTier(0);

                GameObject.DontDestroyOnLoad(prefab);
            FakePrefab.MarkAsFakePrefab(prefab);
            prefab.SetActive(false);
        }
        public static void BuildSynergyPrefab()
        {
            bool flag = MagnesiumGuonStone.upgradeOrbitalPrefab == null;
            if (flag)
            {
                GameObject gameObject = SpriteBuilder.SpriteFromResource("GunRev/Resources/unstableguonorbital", null); //(The orbital appearance with it's special synergy) MAKE SURE TO CHANGE THE SPRITE PATH TO YOUR OWN MODS
                gameObject.name = "Unstable Guon Stone";
                SpeculativeRigidbody speculativeRigidbody = gameObject.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(IntVector2.Zero, new IntVector2(9, 13));
                MagnesiumGuonStone.upgradeOrbitalPrefab = gameObject.AddComponent<PlayerOrbital>();
                speculativeRigidbody.CollideWithTileMap = false;
                speculativeRigidbody.CollideWithOthers = true;
                speculativeRigidbody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.EnemyBulletBlocker;
                MagnesiumGuonStone.upgradeOrbitalPrefab.shouldRotate = false; //Determines if your guon rotates with it's special synergy
                MagnesiumGuonStone.upgradeOrbitalPrefab.orbitRadius = 2.5f; //Determines how far your guon orbits with it's special synergy
                MagnesiumGuonStone.upgradeOrbitalPrefab.orbitDegreesPerSecond = 120f; //Determines how fast your guon orbits with it's special synergy
                MagnesiumGuonStone.upgradeOrbitalPrefab.perfectOrbitalFactor = 10f; //Determines how fast your guon will move to catch up with its owner with it's special synergy. By default, even though the regular guons have it at 0, the upgraded synergy guons all have a higher perfectOrbitalFactor. I find 10 to be about the same.
                MagnesiumGuonStone.upgradeOrbitalPrefab.SetOrbitalTier(0);
                UnityEngine.Object.DontDestroyOnLoad(gameObject);
                FakePrefab.MarkAsFakePrefab(gameObject);
                gameObject.SetActive(false);
            }
        }
        public void PostProcessProjectile(Projectile projectile, float f)
        {
            bool flag = MagnesiumGuonStone.upgradeOrbitalPrefab == null;
            if (flag)
            {
                int FUCK = UnityEngine.Random.Range(1, 11);
                if (FUCK >= 6)
                {
                    ExplosiveModifier explosiveModifier = projectile.gameObject.AddComponent<ExplosiveModifier>();
                    explosiveModifier.doExplosion = true;
                    explosiveModifier.explosionData = GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultSmallExplosionData;
                }
            }
            else
            {
                int FUCK = UnityEngine.Random.Range(1, 11);
                if (FUCK >= 9)
                {
                    ExplosiveModifier explosiveModifier = projectile.gameObject.AddComponent<ExplosiveModifier>();
                    explosiveModifier.doExplosion = true;
                    explosiveModifier.explosionData = GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultSmallExplosionData;
                }
            }
        }
        public override void Update()
        {
            base.Update();
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.PostProcessProjectile += this.PostProcessProjectile;
        }
        public override DebrisObject Drop(PlayerController player)
        {
            player.PostProcessProjectile -= this.PostProcessProjectile;
            return base.Drop(player);
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}
