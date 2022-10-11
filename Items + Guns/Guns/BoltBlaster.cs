using System;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;

namespace GunRev
{

	public class BoltBlaster : GunBehaviour
	{

		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Bolt Blaster", "bolt");
			Game.Items.Rename("outdated_gun_mods:bolt_blaster", "ai:bolt_blaster");
			gun.gameObject.AddComponent<BoltBlaster>();
			gun.SetShortDescription("Sprocket Launcher");
			gun.SetLongDescription("A standard shotgun retrofitted to accept nuts and bolts as ammo.\n\nDeadly in some circumstances.\n\nAs it turns out, nuts and bolts are quite hard to find in the Gungeon.");
			gun.SetupSprite(null, "bolt_idle_001", 8);
			gun.SetAnimationFPS(gun.shootAnimation, 16);
			gun.SetBaseMaxAmmo(500);
			gun.quality = PickupObject.ItemQuality.D;
			gun.gunClass = GunClass.SHOTGUN;
			gun.encounterTrackable.EncounterGuid = "A Nutty Adventure";

			for (int i = 0; i < 5; i++)
			{
				gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(157) as Gun, true, false);
				gun.gunSwitchGroup = (PickupObjectDatabase.GetById(157) as Gun).gunSwitchGroup;
			}


			foreach (ProjectileModule projectileModule in gun.Volley.projectiles)
			{
				Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
				projectileModule.projectiles[0] = projectile;
				projectileModule.ammoCost = 1;
				projectileModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
				projectileModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
				projectileModule.cooldownTime = 0.4f;
				projectileModule.angleVariance = 45f;
				projectileModule.numberOfShotsInClip = 10;
				gun.reloadTime = 1.5f;
				gun.DefaultModule.cooldownTime = .4f;

				projectile.shouldRotate = true;
				projectile.gameObject.SetActive(false);
				FakePrefab.MarkAsFakePrefab(projectile.gameObject);
				UnityEngine.Object.DontDestroyOnLoad(projectile);

				projectile.gameObject.SetActive(false);
				projectile.baseData.damage = 3f;
				projectile.baseData.speed = 17f;
				projectile.baseData.range = 10f;
				projectile.transform.parent = gun.barrelOffset;

				int projectilenumber = UnityEngine.Random.Range(0, 2);

				if (projectilenumber == 1)
				{
					projectile.SetProjectileSpriteRight("bolt_projectile_001", 7, 4, false, tk2dBaseSprite.Anchor.MiddleCenter, 7, 4);
				}
				else
				{
					projectile.SetProjectileSpriteRight("bolt_projectile_002", 7, 7, false, tk2dBaseSprite.Anchor.MiddleCenter, 7, 7);
				}
				bool flag = projectileModule != gun.DefaultModule;
				if (flag)
				{
					projectileModule.ammoCost = 0;
				}
				projectile.transform.parent = gun.barrelOffset;
			}

			gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.SHOTGUN;
			ETGMod.Databases.Items.Add(gun, null, "ANY");
			PlayerController player = (PlayerController)gun.CurrentOwner;
		}

		public override void OnPostFired(PlayerController player, Gun gun)
		{
			gun.PreventNormalFireAudio = true;
			AkSoundEngine.PostEvent("Play_WPN_shotgun_shot_01", gameObject);
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
				AkSoundEngine.PostEvent("Play_WPN_shotgun_reload", base.gameObject);
			}
		}
	}
}