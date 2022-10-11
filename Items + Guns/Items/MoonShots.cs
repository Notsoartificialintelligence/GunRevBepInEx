using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;

namespace GunRev
{
    public class MoonShots : PassiveItem
    {
        private DateTime time;
        private float DamageBonusMult = 1f;
        private float FireSpeedBonusMult = 1f;
        public static void Register()
        {
            string itemName = "Moonshots";

            string resourceName = "GunRev/Resources/moonshots";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<MoonShots>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Among The Stars";
            string longDesc = "Bullets gain a mixture of bonus bullet damage and fire rate, based on the current phase of the Moon. A new moon gives the maximum damage boost but no fire rate boost, and a full moon gives the maximum fire rate boost but no damage boost.\n\nMiniature moons, fitted on bullet shells. They still have gravity, so don't touch them if you want to keep your fingers.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ai");

            item.quality = PickupObject.ItemQuality.A;
        }

        public void PostProcessProjectile(Projectile projectile, float f)
        {
            projectile.baseData.damage *= DamageBonusMult;
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            
            time = DateTime.Now;

            int Year = time.Year;
            int Month = time.Month;
            int Day = time.Day;

            //time to do some really stupid shit

            int A = (int)Math.Truncate(Year / 100m);
            int B = (int)Math.Truncate(A / 4m);
            int C = 2 - (A + B);
            int D = (int)Math.Truncate(365.25 * (Year + 4716));
            int E = (int)Math.Truncate(30.6001 * (Month + 1));
            float JulianDay = (C + Day + D + E) - 1524.5f;

            float DaysSinceNewMoon = JulianDay - 2451549.5f;

            float NumberOfNewMoons = DaysSinceNewMoon / 29.53f;

            float MoonCycleFraction = NumberOfNewMoons - (int)Math.Truncate(NumberOfNewMoons);

            float NumberOfDaysThroughCycle = MoonCycleFraction * 29.53f;

            if (NumberOfDaysThroughCycle >= 27.684375f || NumberOfDaysThroughCycle < 1.845625)
            {
                DamageBonusMult = 1.5f;
                FireSpeedBonusMult = 1f;
            }
            else if (NumberOfDaysThroughCycle.IsBetweenRange(1.845625f, 5.536875f))
            {
                DamageBonusMult = 1.375f;
                FireSpeedBonusMult = 1.125f;
            }
            else if (NumberOfDaysThroughCycle.IsBetweenRange(5.536875f, 9.228125f))
            {
                DamageBonusMult = 1.25f;
                FireSpeedBonusMult = 1.25f;
            }
            else if (NumberOfDaysThroughCycle.IsBetweenRange(9.228125f, 12.919375f))
            {
                DamageBonusMult = 1.125f;
                FireSpeedBonusMult = 1.375f;
            }
            else if (NumberOfDaysThroughCycle.IsBetweenRange(12.919375f, 16.610625f))
            {
                DamageBonusMult = 1f;
                FireSpeedBonusMult = 1.5f;
            }
            else if (NumberOfDaysThroughCycle.IsBetweenRange(16.610625f, 20.301875f))
            {
                DamageBonusMult = 1.125f;
                FireSpeedBonusMult = 1.375f;
            }
            else if (NumberOfDaysThroughCycle.IsBetweenRange(20.301875f, 23.993125f))
            {
                DamageBonusMult = 1.25f;
                FireSpeedBonusMult = 1.25f;
            }
            else if (NumberOfDaysThroughCycle.IsBetweenRange(23.993125f, 27.684375f))
            {
                DamageBonusMult = 1.375f;
                FireSpeedBonusMult = 1.125f;
            }
            StatModifier item = new StatModifier
            {
                statToBoost = PlayerStats.StatType.RateOfFire,
                amount = FireSpeedBonusMult,
                modifyType = StatModifier.ModifyMethod.MULTIPLICATIVE
            };
            StatExtra.Add(item);

            player.ownerlessStatModifiers.Add(item);
            player.stats.RecalculateStats(player, true, true);
        }

        public override DebrisObject Drop(PlayerController player)
        {
            foreach (StatModifier stat in StatExtra)
            {
                player.ownerlessStatModifiers.Remove(stat);
                player.stats.RecalculateStats(player, true, true);
            }
            return base.Drop(player);
        }
        private List<StatModifier> StatExtra = new List<StatModifier>();
    }
}