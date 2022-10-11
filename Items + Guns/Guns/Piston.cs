using System;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;

namespace GunRev
{

    public class Piston : GunBehaviour
    {

        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Piston", "piston");
            Game.Items.Rename("outdated_gun_mods:piston", "ai:piston");
            gun.gameObject.AddComponent<Piston>();
            gun.SetShortDescription("Push Limit");
            gun.SetLongDescription("A beautifully simple device with infinite possibilities, from doors to giant walking mechas. And of course, you could just push enemies away with it, if that's your thing.\n\nDeals extra damage if an enemy hits a wall.");
            gun.SetupSprite(null, "piston_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 14);
            Gun other = PickupObjectDatabase.GetById(38) as Gun;
            gun.AddProjectileModuleFrom(other, true, false);
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).frames[2].eventAudio = "Play_piston_push";
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).frames[2].triggerEvent = true;
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).frames[6].eventAudio = "Play_piston_pull";
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).frames[6].triggerEvent = true;
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.reloadAnimation).frames[7].eventAudio = "Play_piston_push";
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.reloadAnimation).frames[7].triggerEvent = true;
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.reloadAnimation).frames[4].eventAudio = "Play_piston_pull";
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.reloadAnimation).frames[4].triggerEvent = true;
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 2f;
            gun.DefaultModule.cooldownTime = 1.2f;
            gun.DefaultModule.numberOfShotsInClip = 16;
            gun.SetBaseMaxAmmo(256);
            gun.quality = PickupObject.ItemQuality.B;
            gun.gunClass = GunClass.SILLY;
            gun.muzzleFlashEffects = new VFXPool { type = VFXPoolType.None, effects = new VFXComplex[0] };
            gun.encounterTrackable.EncounterGuid = "psht";
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            // too lazy to change the projectile, and who's gonna see it anyway it's invisible lol
            projectile.SetProjectileSpriteRight("reactorcore_projectile", 5, 8, false, tk2dBaseSprite.Anchor.MiddleCenter, 5, 8);
            projectile.shouldRotate = true;
            projectile.sprite.renderer.enabled = false;
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage = 8f;
            projectile.baseData.speed = 40f;
            projectile.baseData.range = 2.7f;
            projectile.baseData.force = 120f;
            PierceProjModifier piercing = projectile.gameObject.GetOrAddComponent<PierceProjModifier>();
            piercing.penetration += 9999999;
            piercing.penetratesBreakables = true;
            projectile.hitEffects.suppressMidairDeathVfx = true;
            projectile.hitEffects.HasProjectileDeathVFX = false;
            projectile.transform.parent = gun.barrelOffset;
            gun.barrelOffset.transform.localPosition = new Vector3(0.4375f, 0.34375f, 0f);
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = CustomClipAmmoTypeToolbox.AddCustomAmmoType("Redstone", "GunRev/Resources/Clips/redstone_clip", "GunRev/Resources/Clips/redstone_clip_empty");
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            Piston.RedstoneEngineeringID = gun.PickupObjectId;
        }
        public static int RedstoneEngineeringID;
        public override void OnPostFired(PlayerController player, Gun gun)
        {
            gun.PreventNormalFireAudio = true;
        }
        public void OnHitEnemy(Projectile projectile, SpeculativeRigidbody hitRigidbody, bool fatal)
        {
            if (hitRigidbody.aiActor != null)
            {
                hitRigidbody.OnTileCollision += Shit;
            }
        }
        private void Shit(CollisionData cringe)
        {
            cringe.MyRigidbody.aiActor.healthHaver.ApplyDamage(UnityEngine.Random.Range(1, 7), Vector2.zero, "piston wall dmg");
            cringe.MyRigidbody.OnTileCollision -= Shit;
        }
        public override void PostProcessProjectile(Projectile projectile)
        {
            if (gun.OwnerHasSynergy("Isn't It Iron Pick"))
            {
                projectile.baseData.damage = 12;
                projectile.baseData.force = 80;
            }
            projectile.OnHitEnemy += OnHitEnemy;
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
            }
        }
    }
}