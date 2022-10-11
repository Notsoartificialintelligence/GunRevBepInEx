using System;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;

namespace GunRev
{

    public class BOLTDuo : GunBehaviour
    {

        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("BOLT Duo", "boltduo");
            Game.Items.Rename("outdated_gun_mods:bolt_duo", "ai:bolt_duo");
            gun.gameObject.AddComponent<BOLTDuo>();
            gun.SetShortDescription("Double The Danger");
            gun.SetLongDescription("One of BOLT Incorporated's most popular mecha weapons, they even made a handheld version for manual use. This isn't one of those handheld versions.\n\nStill works, somehow. Just don't touch the wires.");
            gun.SetupSprite(null, "boltduo_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 17);
            gun.SetAnimationFPS(gun.reloadAnimation, 10);
            gun.SetBaseMaxAmmo(300);
            gun.quality = PickupObject.ItemQuality.C;
            gun.gunClass = GunClass.PISTOL;
            gun.encounterTrackable.EncounterGuid = "double bzzt";
            gun.reloadTime = 1f;
            gun.carryPixelOffset = new IntVector2(2, 2);
            int iterator = 0;
            for (int i = 0; i < 2; i++)
            {
                gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(157) as Gun, true, false);
                gun.gunSwitchGroup = (PickupObjectDatabase.GetById(157) as Gun).gunSwitchGroup;
            }
            foreach (ProjectileModule projectileModule in gun.Volley.projectiles)
            {
                if (iterator == 0)
                {
                    projectileModule.angleFromAim = 10;
                }
                if (iterator == 1)
                {
                    projectileModule.angleFromAim = -10;
                }
                projectileModule.ammoCost = 2;
                projectileModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
                projectileModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
                projectileModule.cooldownTime = 0.7f;
                projectileModule.numberOfShotsInClip = 6;
                Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(projectileModule.projectiles[0]);
                projectile.gameObject.SetActive(false);
                FakePrefab.MarkAsFakePrefab(projectile.gameObject);
                UnityEngine.Object.DontDestroyOnLoad(projectile);
                projectileModule.projectiles[0] = projectile;
                projectile.baseData.damage = 6f;
                projectile.baseData.speed = 20f;
                projectile.baseData.range = 4f;
                projectile.transform.parent = gun.barrelOffset;
                projectile.collidesWithProjectiles = false;
                projectile.gameObject.GetOrAddComponent<LightningProjectileComp>();
                projectileModule.ammoType = GameUIAmmoType.AmmoType.NAIL;
                iterator++;
            }
            ETGMod.Databases.Items.Add(gun, null, "ANY");
        }
        public override void OnPostFired(PlayerController player, Gun gun)
        {
            gun.PreventNormalFireAudio = true;
            AkSoundEngine.PostEvent("Play_WPN_zapper_shot_01", gameObject);
        }
        private bool HasReloaded;
        public override void Update()
        {
            if (gun.CurrentOwner)
            {

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
                AkSoundEngine.PostEvent("Play_WPN_zapper_reload_01", base.gameObject);
            }
        }
    }
}