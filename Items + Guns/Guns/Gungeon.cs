using System;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;
using System.Collections.Generic;

namespace GunRev
{

    public class GungeonGun : GunBehaviour
    {
        bool HasKTGF = false;
        bool HasKaliber = false;

        static Projectile Blobulon;
        static Projectile Spectre;
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Enter The Gungeon", "gungeon");
            Game.Items.Rename("outdated_gun_mods:enter_the_gungeon", "ai:enter_the_gungeon");
            gun.gameObject.AddComponent<GungeonGun>();
            gun.SetShortDescription("2016 Video Game");
            gun.SetLongDescription("Enter the Gungeon is a bullet hell roguelike video game developed by Dodge Roll and published by Devolver Digital. It was released worldwide for Microsoft Windows, OS X, Linux, and PlayStation 4 on April 5, 2016, on Xbox One on April 5, 2017, on Nintendo Switch on December 14, 2017, and on Stadia on December 22, 2020.");
            gun.SetupSprite(null, "gungeon_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 16);
            Gun other = PickupObjectDatabase.GetById(38) as Gun;
            gun.AddProjectileModuleFrom(other, true, false);
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.5f;
            gun.DefaultModule.cooldownTime = 0.4f;
            gun.DefaultModule.numberOfShotsInClip = 16;
            gun.SetBaseMaxAmmo(256);
            gun.quality = PickupObject.ItemQuality.A;
            gun.gunClass = GunClass.SILLY;
            gun.encounterTrackable.EncounterGuid = "EEEEEEEEEEEEEEENTER THE GUN";

            Projectile BulletKin = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(56) as Gun).DefaultModule.projectiles[0]);
            BulletKin.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(BulletKin.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(BulletKin);
            BulletKin.SetProjectileSpriteRight("gungeon_projectile_bulletkin", 5, 6, false, tk2dBaseSprite.Anchor.MiddleCenter, 4, 5);
            BulletKin.shouldRotate = true;
            BulletKin.baseData.damage = 5f;
            BulletKin.baseData.speed = 14f;
            BulletKin.baseData.range = 30f;
            BulletKin.transform.parent = gun.barrelOffset;

            Projectile ShotgunKin = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(56) as Gun).DefaultModule.projectiles[0]);
            ShotgunKin.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(ShotgunKin.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(ShotgunKin);
            ShotgunKin.SetProjectileSpriteRight("gungeon_projectile_shotgunkin", 5, 7, false, tk2dBaseSprite.Anchor.MiddleCenter, 3, 5);
            ShotgunKin.shouldRotate = true;
            ShotgunKin.baseData.damage = 5f;
            ShotgunKin.baseData.speed = 14f;
            ShotgunKin.baseData.range = 20f;
            ShotgunKin.transform.parent = gun.barrelOffset;
            SpawnProjModifier spawnmodifier = ShotgunKin.gameObject.AddComponent<SpawnProjModifier>();
            spawnmodifier.spawnProjectilesOnCollision = true;
            spawnmodifier.projectileToSpawnOnCollision = ((PickupObjectDatabase.GetById(38) as Gun).DefaultModule.projectiles[0]);
            spawnmodifier.numberToSpawnOnCollison = 6;

            Projectile PinHead = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(56) as Gun).DefaultModule.projectiles[0]);
            PinHead.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(PinHead.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(PinHead);
            PinHead.SetProjectileSpriteRight("gungeon_projectile_pinhead", 3, 6, false, tk2dBaseSprite.Anchor.MiddleCenter, 3, 5);
            PinHead.shouldRotate = true;
            PinHead.baseData.damage = 6f;
            PinHead.baseData.speed = 14f;
            PinHead.baseData.range = 16f;
            PinHead.transform.parent = gun.barrelOffset;
            ExplosiveModifier explosiveModifier = PinHead.gameObject.AddComponent<ExplosiveModifier>();
            explosiveModifier.doExplosion = true;
            explosiveModifier.explosionData = DungeonDatabase.GetOrLoadByName("base_castle").sharedSettingsPrefab.DefaultSmallExplosionData;

            Projectile HollowPoint = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(56) as Gun).DefaultModule.projectiles[0]);
            HollowPoint.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(HollowPoint.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(HollowPoint);
            HollowPoint.SetProjectileSpriteRight("gungeon_projectile_hollowpoint", 5, 6, false, tk2dBaseSprite.Anchor.MiddleCenter, 5, 6);
            HollowPoint.shouldRotate = true;
            HollowPoint.baseData.damage = 4f;
            HollowPoint.baseData.speed = 12f;
            HollowPoint.baseData.range = 100f;
            HollowPoint.transform.parent = gun.barrelOffset;
            PierceProjModifier piercing = HollowPoint.gameObject.GetOrAddComponent<PierceProjModifier>();
            piercing.penetration += 3;
            piercing.penetratesBreakables = true;


            Blobulon = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(56) as Gun).DefaultModule.projectiles[0]);
            Blobulon.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(Blobulon.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(Blobulon);
            Blobulon.SetProjectileSpriteRight("gungeon_projectile_blobulon", 4, 4, false, tk2dBaseSprite.Anchor.MiddleCenter, 4, 4);
            Blobulon.shouldRotate = true;
            Blobulon.baseData.damage = 3f;
            Blobulon.baseData.speed = 12f;
            Blobulon.baseData.range = 100f;
            Blobulon.baseData.force = 15f;
            Blobulon.transform.parent = gun.barrelOffset;
            BounceProjModifier bounce = Blobulon.gameObject.GetOrAddComponent<BounceProjModifier>();
            bounce.numberOfBounces += 3;


            Spectre = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(56) as Gun).DefaultModule.projectiles[0]);
            Spectre.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(Spectre.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(Spectre);
            Spectre.SetProjectileSpriteRight("gungeon_projectile_spectre", 6, 7, false, tk2dBaseSprite.Anchor.UpperRight, 5, 6);
            Spectre.shouldRotate = true;
            Spectre.baseData.damage = 3f;
            Spectre.baseData.speed = 12f;
            Spectre.baseData.range = 100f;
            Spectre.baseData.force = 15f;
            Spectre.transform.parent = gun.barrelOffset;
            PierceProjModifier piercingspectre = Spectre.gameObject.GetOrAddComponent<PierceProjModifier>();
            piercingspectre.penetration += 3;
            piercingspectre.penetratesBreakables = true;
            Spectre.AppliesPoison = true;
            Spectre.PoisonApplyChance = 0.5f;
            Spectre.healthEffect = PickupObjectDatabase.GetById(204).GetComponent<BulletStatusEffectItem>().HealthModifierEffect;

            gun.DefaultModule.projectiles[0] = BulletKin;
            gun.DefaultModule.projectiles.Add(ShotgunKin);
            gun.DefaultModule.projectiles.Add(PinHead);
            gun.DefaultModule.projectiles.Add(HollowPoint);
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = "purple blaster";
            ETGMod.Databases.Items.Add(gun, null, "ANY");
        }

        public override void OnPostFired(PlayerController player, Gun gun)
        {
            gun.PreventNormalFireAudio = true;
            AkSoundEngine.PostEvent("Play_WPN_magnum_shot_01", gameObject);
        }
        private bool HasReloaded;
        public override void Update()
        {
            if (gun.CurrentOwner)
            {
                PlayerController player = (PlayerController)gun.CurrentOwner;
                if (player.PlayerHasActiveSynergy("Knife To A Gunfight") & HasKTGF == false)
                {

                    gun.DefaultModule.projectiles.Add(Blobulon);
                    HasKTGF = true;
                }
                else
                {
                    gun.DefaultModule.projectiles.Remove(Blobulon);
                    HasKTGF = false;
                }
                if (player.PlayerHasActiveSynergy("Children Of Kaliber") & HasKaliber == false)
                {

                    gun.DefaultModule.projectiles.Add(Spectre);
                    HasKaliber = true;
                }
                else
                {
                    gun.DefaultModule.projectiles.Remove(Spectre);
                    HasKaliber = false;
                }
                if (!gun.PreventNormalFireAudio)
                {
                    this.gun.PreventNormalFireAudio = true;
                }
                if (!gun.IsReloading && !HasReloaded)
                {
                    this.HasReloaded = true;
                }
            }
        }

        public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
        {
            if (gun.IsReloading && this.HasReloaded)
            {
                HasReloaded = false;
                AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
                base.OnReloadPressed(player, gun, bSOMETHING);
                AkSoundEngine.PostEvent("Play_WPN_magnum_reload_01", base.gameObject);
            }
        }
    }
}