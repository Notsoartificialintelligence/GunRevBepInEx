using System;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;

namespace GunRev
{

    public class Atomic : GunBehaviour
    {

        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Reactor Core", "reactorcore");
            Game.Items.Rename("outdated_gun_mods:reactor_core", "ai:reactor_core");
            gun.gameObject.AddComponent<Atomic>();
            gun.SetShortDescription("SCRAM");
            gun.SetLongDescription("A highly experimental gun sent from an offworld nuclear power plant.\n\nThis thing definitely does not abide to the Nuclear Guns Treaty.");
            gun.SetupSprite(null, "reactorcore_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 13);
            Gun other = PickupObjectDatabase.GetById(38) as Gun;
            gun.AddProjectileModuleFrom(other, true, false);
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.3f;
            gun.DefaultModule.cooldownTime = 0.6f;
            gun.DefaultModule.numberOfShotsInClip = 4;
            gun.SetBaseMaxAmmo(60);
            gun.quality = PickupObject.ItemQuality.A;
            gun.gunClass = GunClass.RIFLE;
            gun.encounterTrackable.EncounterGuid = "KABOOM?";
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.SetProjectileSpriteRight("reactorcore_projectile", 6, 6, false, tk2dBaseSprite.Anchor.MiddleCenter, 4, 4);
            projectile.shouldRotate = true;
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.AppliesPoison = true;
            projectile.PoisonApplyChance = 0.8f;
            projectile.healthEffect = PickupObjectDatabase.GetById(204).GetComponent<BulletStatusEffectItem>().HealthModifierEffect;
            projectile.baseData.damage = 8f;
            projectile.baseData.speed = 18f;
            projectile.baseData.range = 30f;
            projectile.transform.parent = gun.barrelOffset;
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = "poison_blob";
            ETGMod.Databases.Items.Add(gun, null, "ANY");
        }

        public override void OnPostFired(PlayerController player, Gun gun)
        {
            gun.PreventNormalFireAudio = true;
            AkSoundEngine.PostEvent("Play_WPN_poison_impact_01", gameObject);
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
                AkSoundEngine.PostEvent("Play_WPN_m1911_reload_01", base.gameObject);
            }
        }
    }
}