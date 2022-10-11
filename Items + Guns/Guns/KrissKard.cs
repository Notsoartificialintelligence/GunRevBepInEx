using Alexandria.ItemAPI;
using Gungeon;
using UnityEngine;

namespace GunRev
{

    public class KrissKard : GunBehaviour
    {
        private static SlashData slash;
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("KRISS KARD", "krisskard");
            Game.Items.Rename("outdated_gun_mods:kriss_kard", "ai:kriss_kard");
            gun.gameObject.AddComponent<KrissKard>();
            gun.SetShortDescription("Built Like A Brick");
            gun.SetLongDescription("Reloading can hit nearby enemies and stun them.\n\nDefinitely one of the squarest pistols around. Be careful not to cut yourself on the corners.");
            gun.SetupSprite(null, "krisskard_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 15);
            Gun other = PickupObjectDatabase.GetById(38) as Gun;
            gun.AddProjectileModuleFrom(other, true, false);
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.3f;
            gun.DefaultModule.cooldownTime = 0.4f;
            gun.DefaultModule.numberOfShotsInClip = 8;
            gun.SetBaseMaxAmmo(160);
            gun.quality = PickupObject.ItemQuality.B;
            gun.gunClass = GunClass.PISTOL;
            gun.encounterTrackable.EncounterGuid = "brick";
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage = 6f;
            projectile.baseData.speed = 16f;
            projectile.baseData.range = 60f;
            projectile.transform.parent = gun.barrelOffset;
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.SMALL_BULLET;

            slash = ScriptableObject.CreateInstance<SlashData>();
            slash.damage = 5f;
            slash.enemyKnockbackForce = 18f;
            slash.doVFX = false;
            slash.slashDegrees = 30f;
            slash.slashRange = 0.625f;

            ETGMod.Databases.Items.Add(gun, null, "ANY");
        }

        public override void OnPostFired(PlayerController player, Gun gun)
        {
            gun.PreventNormalFireAudio = true;
            AkSoundEngine.PostEvent("Play_WPN_magnum_fire_01", gameObject);
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
                SlashDoer.DoSwordSlash(gun.barrelOffset.PositionVector2(), 0f, player, slash);
                AkSoundEngine.PostEvent("m_WPN_baseball_shot_01", base.gameObject);
            }
        }
    }
}