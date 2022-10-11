using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GunRev
{
    internal class SynergyForms
    {
        public static void AddSynergyForms()
        {
            AdvancedDualWieldSynergyProcessor advancedDualWieldSynergyProcessor = (PickupObjectDatabase.GetById(Vintage.BatteryBuddiesID) as Gun).gameObject.AddComponent<AdvancedDualWieldSynergyProcessor>();
            advancedDualWieldSynergyProcessor.PartnerGunID = 13;
            advancedDualWieldSynergyProcessor.SynergyNameToCheck = "Battery Buddies";
            AdvancedDualWieldSynergyProcessor advancedDualWieldSynergyProcessor1 = (PickupObjectDatabase.GetById(13) as Gun).gameObject.AddComponent<AdvancedDualWieldSynergyProcessor>();
            advancedDualWieldSynergyProcessor1.PartnerGunID = Vintage.BatteryBuddiesID;
            advancedDualWieldSynergyProcessor1.SynergyNameToCheck = "Battery Buddies";

            if (PickupObjectDatabase.GetByEncounterName("Dispenser") != null)
            {
                var bruh = PickupObjectDatabase.GetByEncounterName("Dispenser");
                AdvancedDualWieldSynergyProcessor advancedDualWieldSynergyProcessor2 = (PickupObjectDatabase.GetById(Piston.RedstoneEngineeringID) as Gun).gameObject.AddComponent<AdvancedDualWieldSynergyProcessor>();
                advancedDualWieldSynergyProcessor2.PartnerGunID = bruh.PickupObjectId;
                advancedDualWieldSynergyProcessor2.SynergyNameToCheck = "Redstone Engineering";
                AdvancedDualWieldSynergyProcessor advancedDualWieldSynergyProcessor3 = (PickupObjectDatabase.GetById(bruh.PickupObjectId) as Gun).gameObject.AddComponent<AdvancedDualWieldSynergyProcessor>();
                advancedDualWieldSynergyProcessor3.PartnerGunID = Piston.RedstoneEngineeringID;
                advancedDualWieldSynergyProcessor3.SynergyNameToCheck = "Redstone Engineering";
            }

            /*AdvancedDualWieldSynergyProcessor advancedDualWieldSynergyProcessor4 = (PickupObjectDatabase.GetById(DiscouragementBeam.ApertureScienceID) as Gun).gameObject.AddComponent<AdvancedDualWieldSynergyProcessor>();
            advancedDualWieldSynergyProcessor4.PartnerGunID = 331;
            advancedDualWieldSynergyProcessor4.SynergyNameToCheck = "Aperture Science";
            AdvancedDualWieldSynergyProcessor advancedDualWieldSynergyProcessor5 = (PickupObjectDatabase.GetById(331) as Gun).gameObject.AddComponent<AdvancedDualWieldSynergyProcessor>();
            advancedDualWieldSynergyProcessor5.PartnerGunID = DiscouragementBeam.ApertureScienceID;
            advancedDualWieldSynergyProcessor5.SynergyNameToCheck = "Aperture Science";*/
        }
    }
}