using System;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;

namespace GunRev
{

	public class SingleUseGun : GunBehaviour
	{

		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Single Use Gun", "singleusegun");
			Game.Items.Rename("outdated_gun_mods:single_use_gun", "ai:single_use_gun");
			gun.gameObject.AddComponent<SingleUseGun>();
			gun.SetShortDescription("Mass Produced");
			gun.SetLongDescription("Only has 1 clip of ammo, then has to be thrown away.\n\nWhat nutbrained individual had the idea to make a gun that only fires 6 times?");
			gun.SetupSprite(null, "singleusegun_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 16);
            Gun other = PickupObjectDatabase.GetById(38) as Gun;
            gun.AddProjectileModuleFrom(other, true, false);
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.5f;
            gun.DefaultModule.cooldownTime = 0.4f;
            gun.DefaultModule.numberOfShotsInClip = 6;
            gun.SetBaseMaxAmmo(6);
            gun.quality = PickupObject.ItemQuality.D;
            gun.gunClass = GunClass.PISTOL;
            gun.encounterTrackable.EncounterGuid = "Reduce Reuse Recycle";
            gun.CanGainAmmo = false;
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage = 3f;
            projectile.baseData.speed = 17f;
            projectile.transform.parent = gun.barrelOffset;
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.SMALL_BULLET;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            PlayerController player = (PlayerController)gun.CurrentOwner;
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
    }
}
