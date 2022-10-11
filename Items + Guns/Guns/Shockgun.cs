using System;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;

namespace GunRev
{

	public class Shockgun : GunBehaviour
	{

		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Shockgun", "shockgun");
			Game.Items.Rename("outdated_gun_mods:shockgun", "ai:shockgun");
			gun.gameObject.AddComponent<Shockgun>();
			gun.SetShortDescription("Electrifying!");
			gun.SetLongDescription("Shots are chained with lightning.\n\nAn exceptionally advanced handheld machine. Contains the energy of 17 supermassive potato batteries.");
			gun.SetupSprite(null, "shockgun_idle_001", 8);
			gun.SetAnimationFPS(gun.shootAnimation, 16);
			gun.SetBaseMaxAmmo(256);
			gun.quality = PickupObject.ItemQuality.B;
			gun.gunClass = GunClass.SHOTGUN;
			gun.encounterTrackable.EncounterGuid = "jabhdbdshbdshbadshsdbdhsab";

			for (int i = 0; i < 4; i++)
			{
				gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(30) as Gun, true, false);
				gun.gunSwitchGroup = (PickupObjectDatabase.GetById(30) as Gun).gunSwitchGroup;
			}


			foreach (ProjectileModule projectileModule in gun.Volley.projectiles)
			{
				Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
				projectileModule.projectiles[0] = projectile;
				projectileModule.ammoCost = 1;
				projectileModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
				projectileModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
				projectileModule.cooldownTime = 0.4f;
				projectileModule.angleVariance = 60f;
				projectileModule.numberOfShotsInClip = 4;
				gun.reloadTime = 1.2f;
				gun.DefaultModule.cooldownTime = .5f;

				projectile.shouldRotate = true;
				projectile.gameObject.SetActive(false);
				FakePrefab.MarkAsFakePrefab(projectile.gameObject);
				UnityEngine.Object.DontDestroyOnLoad(projectile);

				projectile.gameObject.SetActive(false);
				projectile.baseData.damage = 3f;
				projectile.baseData.speed = 17f;
				projectile.baseData.range = 10f;
				projectile.transform.parent = gun.barrelOffset;

				bool flag = projectileModule != gun.DefaultModule;
				if (flag)
				{
					projectileModule.ammoCost = 0;
				}
				projectile.transform.parent = gun.barrelOffset;
			}

			gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.BLUE_SHOTGUN;
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