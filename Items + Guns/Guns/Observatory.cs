using System;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;

namespace GunRev
{

    public class Observatory : GunBehaviour
    {

        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Observatory", "observatory");
            Game.Items.Rename("outdated_gun_mods:observatory", "ai:observatory");
            gun.gameObject.AddComponent<Observatory>();
            gun.SetShortDescription("Observe.");
            gun.SetLongDescription("A miniature universe in the comfort of your own dungeon!\n\nCAUTION: Contains small parts. Not for children under 6 years of age. Assembly required. Batteries not incuded. Do not submerge in water. Warranty void if universe chamber is opened.");
            gun.SetupSprite(null, "observatory_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 16);
            Gun other = PickupObjectDatabase.GetById(38) as Gun;
            gun.AddProjectileModuleFrom(other, true, false);
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 2f;
            gun.DefaultModule.cooldownTime = 1.5f;
            gun.DefaultModule.numberOfShotsInClip = 8;
            gun.SetBaseMaxAmmo(80);
            gun.quality = PickupObject.ItemQuality.S;
            gun.gunClass = GunClass.SILLY;
            gun.encounterTrackable.EncounterGuid = "Music Of The Spheres";

            Projectile BlackHole = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(169) as Gun).DefaultModule.projectiles[0]);
            BlackHole.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(BlackHole.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(BlackHole);
            BlackHole.transform.parent = gun.barrelOffset;

            Projectile Planet1 = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(597) as Gun).DefaultModule.projectiles[0]);
            Planet1.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(Planet1.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(Planet1);
            Planet1.transform.parent = gun.barrelOffset;

            Projectile Planet2 = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(597) as Gun).DefaultModule.projectiles[1]);
            Planet2.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(Planet2.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(Planet2);
            Planet2.transform.parent = gun.barrelOffset;

            Projectile Planet3 = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(597) as Gun).DefaultModule.projectiles[2]);
            Planet3.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(Planet3.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(Planet3);
            Planet3.transform.parent = gun.barrelOffset;

            Projectile Planet4 = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(597) as Gun).DefaultModule.projectiles[3]);
            Planet4.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(Planet4.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(Planet4);
            Planet4.transform.parent = gun.barrelOffset;

            Projectile Planet5 = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(597) as Gun).DefaultModule.projectiles[4]);
            Planet5.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(Planet5.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(Planet5);
            Planet5.transform.parent = gun.barrelOffset;

            Projectile Planet6 = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(597) as Gun).DefaultModule.projectiles[5]);
            Planet6.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(Planet6.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(Planet6);
            Planet6.transform.parent = gun.barrelOffset;

            Projectile Planet7 = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(597) as Gun).DefaultModule.projectiles[6]);
            Planet7.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(Planet7.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(Planet7);
            Planet7.transform.parent = gun.barrelOffset;

            Projectile Planet8 = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(597) as Gun).DefaultModule.projectiles[7]);
            Planet8.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(Planet8.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(Planet8);
            Planet8.transform.parent = gun.barrelOffset;

            gun.DefaultModule.projectiles[0] = BlackHole;
            gun.DefaultModule.projectiles.Add(Planet1);
            gun.DefaultModule.projectiles.Add(Planet2);
            gun.DefaultModule.projectiles.Add(Planet3);
            gun.DefaultModule.projectiles.Add(Planet4);
            gun.DefaultModule.projectiles.Add(Planet5);
            gun.DefaultModule.projectiles.Add(Planet6);
            gun.DefaultModule.projectiles.Add(Planet7);
            gun.DefaultModule.projectiles.Add(Planet8);
            gun.barrelOffset.transform.localPosition = new Vector3(2.0625f, 0.875f, 0f);
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = "planet";
            ETGMod.Databases.Items.Add(gun, null, "ANY");
        }

        public override void OnPostFired(PlayerController player, Gun gun)
        {
            gun.PreventNormalFireAudio = true;
            AkSoundEngine.PostEvent("m_WPN_planetgun_shot_01", gameObject);
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
                AkSoundEngine.PostEvent("m_WPN_planetgun_reload_01", base.gameObject);
            }
        }
    }
}