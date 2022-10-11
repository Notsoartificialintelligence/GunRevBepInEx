using System;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;

namespace GunRev
{

	public class Catalyst : GunBehaviour
	{

		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Catalyst", "catalyst");
			Game.Items.Rename("outdated_gun_mods:catalyst", "ai:catalyst");
			gun.gameObject.AddComponent<Catalyst>();
			gun.SetShortDescription("Reactive");
			gun.SetLongDescription("Shoots low damage, explosive bullets.\n\nEach bullet is specially designed to spread its damage over a small area, unfortunately reducing its damage as it does so.");
			gun.SetupSprite(null, "catalyst_idle_001", 8);
			gun.SetAnimationFPS(gun.shootAnimation, 16);
			gun.SetAnimationFPS(gun.reloadAnimation, 6);
			gun.SetBaseMaxAmmo(360);
			gun.quality = PickupObject.ItemQuality.A;
			gun.gunClass = GunClass.SHOTGUN;
			gun.encounterTrackable.EncounterGuid = "Reaction videos or something";

			for (int i = 0; i < 7; i++)
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
				projectileModule.angleVariance = 65f;
				projectileModule.numberOfShotsInClip = 6;
				gun.reloadTime = 1.5f;
				gun.DefaultModule.cooldownTime = .4f;

				projectile.shouldRotate = true;
				projectile.gameObject.SetActive(false);
				FakePrefab.MarkAsFakePrefab(projectile.gameObject);
				UnityEngine.Object.DontDestroyOnLoad(projectile);

				projectile.gameObject.SetActive(false);
				projectile.baseData.damage = 2f;
				projectile.baseData.speed = 16f;
				projectile.transform.parent = gun.barrelOffset;

				projectile.SetProjectileSpriteRight("catalyst_projectile_001", 9, 6, false, tk2dBaseSprite.Anchor.MiddleCenter, 7, 4);

				ExplosiveModifier explosiveModifier = projectile.gameObject.AddComponent<ExplosiveModifier>();
				explosiveModifier.doExplosion = true;
				explosiveModifier.explosionData = DungeonDatabase.GetOrLoadByName("base_castle").sharedSettingsPrefab.DefaultSmallExplosionData;
				explosiveModifier.IgnoreQueues = true;

				bool flag = projectileModule != gun.DefaultModule;
				if (flag)
				{
					projectileModule.ammoCost = 0;
				}
				projectile.transform.parent = gun.barrelOffset;
			}

			gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.SHOTGUN;
			ETGMod.Databases.Items.Add(gun, null, "ANY");
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
				AkSoundEngine.PostEvent("Play_WPN_shotgun_reload_01", base.gameObject);
			}
		}
	}
}
