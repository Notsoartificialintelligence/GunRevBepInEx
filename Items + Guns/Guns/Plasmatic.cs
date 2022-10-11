using System;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;

namespace GunRev
{

    public class Plasmatic : GunBehaviour
    {

        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Plasmatic", "plasmatic");
            Game.Items.Rename("outdated_gun_mods:plasmatic", "ai:plasmatic");
            gun.gameObject.AddComponent<Plasmatic>();
            gun.SetShortDescription("Better In Blue");
            gun.SetLongDescription("Spits plasma-enriched fire quickly and accurately.\n\nThis device is nearly identical to a standard-issue flamethrower, with one exception: it spits blue fire! This ensures the enemy will be scorched within seconds, and gives the manufacturer yet another reason to bump up that price tag.");
            gun.SetupSprite(null, "plasmatic_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 13);
            tk2dSpriteAnimationClip fireClip = gun.sprite.spriteAnimator.GetClipByName("plasmatic_fire");
            fireClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.LoopSection;
            fireClip.loopStart = 1;
            Gun other = PickupObjectDatabase.GetById(30) as Gun;
            gun.AddProjectileModuleFrom(other, true, false);
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.5f;
            gun.DefaultModule.cooldownTime = 0.05f;
            gun.DefaultModule.numberOfShotsInClip = 20;
            gun.SetBaseMaxAmmo(540);
            gun.quality = PickupObject.ItemQuality.B;
            gun.gunClass = GunClass.FIRE;
            gun.encounterTrackable.EncounterGuid = "then it go *fire sounds*";
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.SetProjectileSpriteRight("plasmathrower_projectile_001", 14, 7, false, tk2dBaseSprite.Anchor.MiddleCenter, 12, 5);
            projectile.shouldRotate = true;
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.AppliesFire = true;
            projectile.FireApplyChance = 0.4f;
            projectile.healthEffect = PickupObjectDatabase.GetById(278).GetComponent<BulletStatusEffectItem>().HealthModifierEffect;
            projectile.baseData.damage = 7f;
            projectile.baseData.speed = 8f;
            projectile.baseData.range = 12f;
            projectile.gameObject.GetOrAddComponent<SlowingBulletsEffect>();
            projectile.transform.parent = gun.barrelOffset;
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.BEAM;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
        }

        public override void OnPostFired(PlayerController player, Gun gun)
        {
            gun.PreventNormalFireAudio = true;
            AkSoundEngine.PostEvent("Play_WPN_flame_impact_01", gameObject);
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