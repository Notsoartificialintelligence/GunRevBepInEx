using System;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;

namespace GunRev
{

    public class Trigunometry : GunBehaviour
    {
        int counter = 0;
        int randomdmg1 = 0;
        int randomdmg2 = 0;
        int combineddmg = 0;
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Trigunometry", "trigunometry");
            Game.Items.Rename("outdated_gun_mods:trigunometry", "ai:trigunometry");
            gun.gameObject.AddComponent<Trigunometry>();
            gun.SetShortDescription("It's A Sine!");
            gun.SetLongDescription("The damage of the first two bullets is randomised, while the third is the sum of the previous two shot's damage.\n\nRequired theory in advanced gunometry, along with the Bullet Bisector theory and the Chamber area equation.");
            gun.SetupSprite(null, "trigunometry_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 16);
            Gun other = PickupObjectDatabase.GetById(38) as Gun;
            gun.AddProjectileModuleFrom(other, true, false);
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.5f;
            gun.DefaultModule.cooldownTime = 0.3f;
            gun.DefaultModule.numberOfShotsInClip = 3;
            gun.SetBaseMaxAmmo(333);
            gun.quality = PickupObject.ItemQuality.A;
            gun.gunClass = GunClass.PISTOL;
            gun.encounterTrackable.EncounterGuid = "a^2 + b^2 = c^2";
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.SetProjectileSpriteRight("trigunometry_projectile_001", 10, 10, false, tk2dBaseSprite.Anchor.MiddleCenter, 8, 8);
            projectile.shouldRotate = true;
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage = 7f;
            projectile.baseData.speed = 17f;
            projectile.baseData.range = 8f;
            projectile.transform.parent = gun.barrelOffset;
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = "white";
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            PlayerController player = (PlayerController)gun.CurrentOwner;
        }

        public override void OnPostFired(PlayerController player, Gun gun)
        {
            gun.PreventNormalFireAudio = true;
            AkSoundEngine.PostEvent("Play_WPN_magnum_shot_01", gameObject);
            player.PostProcessProjectile += this.PostProcessProjectile;
        }
        public void PostProcessProjectile(Projectile projectile, float f)
        {
            int randomdmg = UnityEngine.Random.Range(1, 5);
            if (counter == 0)
            {
                randomdmg1 = randomdmg*randomdmg;
                projectile.baseData.damage = randomdmg1;
                counter += 1;
            }
            else if (counter == 1)
            {
                randomdmg2 = randomdmg*randomdmg;
                projectile.baseData.damage = randomdmg2;
                counter += 1;
            }
            else if (counter == 2)
            {
                combineddmg = (int)Mathf.Sqrt(randomdmg1+randomdmg2);
                projectile.baseData.damage = combineddmg;
                combineddmg = 0;
                randomdmg1 = 0;
                randomdmg2 = 0;
                counter = 0;
            }
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
                counter = 0;
                combineddmg = 0;
                randomdmg1 = 0;
                randomdmg2 = 0;
                HasReloaded = false;
                AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
                base.OnReloadPressed(player, gun, bSOMETHING);
                AkSoundEngine.PostEvent("Play_WPN_zapper_reload_01", base.gameObject);
            }
        }
    }
}