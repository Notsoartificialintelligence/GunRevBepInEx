using Alexandria.ItemAPI;
using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using Alexandria.DungeonAPI;
using BepInEx;
using System.IO;
using HarmonyLib;
using Alexandria;
using SoundAPI;

namespace GunRev
{
    [BepInDependency("etgmodding.etg.mtgapi")]
    [BepInPlugin(GUID, NAME, VERSION)]
    public class Module : BaseUnityPlugin
    {
        public const string GUID = "ai.etg.gunrev";
        public const string NAME = "Gundustrial Revolution";
        public const string VERSION = "2.0.0";
        public static readonly string MOD_NAME = "Gundustrial Revolution";
        public static readonly string TEXT_COLOR = "#677e9e";
        public static readonly string ERROR_COLOR = "FF0000";
        public static string ZipFilePath;
        public static string FilePath;
        public static AdvancedStringDB Strings;
        public void Start()
        {
            ETGModMainBehaviour.WaitForGameManagerStart(GMStart);
        }

        public void GMStart(GameManager game)
        {
            try
            {
                new Harmony(GUID).PatchAll();

                ETGMod.Assets.SetupSpritesFromAssembly(Assembly.GetExecutingAssembly(), "GunRev/sprites");
                FilePath = Path.Combine(Info.Location, "..");

                ToolsOther.Init();
                EnemyAPI.Hooks.Init();
                EnemyAPI.EnemyTools.Init();
                EnemyAPI.EnemyBuilder.Init();
                ItemBuilder.Init();
                Module.Strings = new AdvancedStringDB();
                StaticReferences.Init();
                DungeonHandler.Init();
                CustomClipAmmoTypeToolbox.Init();
                EnemyAPI.BossBuilder.Init();
                SoundManager.Init();
                SoundManager.LoadBanksFromModProject();

                Hook foyerCallbacksHook = new Hook(
                    typeof(Foyer).GetMethod("Awake", BindingFlags.NonPublic | BindingFlags.Instance),
                    typeof(Module).GetMethod(nameof(Module.FoyerAwake))
                );

                Hook quickRestartHook = new Hook(
                    typeof(GameManager).GetMethod("DelayedQuickRestart", BindingFlags.Public | BindingFlags.Instance),
                    typeof(Module).GetMethod(nameof(Module.OnQuickRestart))
                );

                //version 1.0.0
                Vintage.Add();
                Atomic.Add();
                Graph.Register();
                BoltBlaster.Add();
                FamiliarPhone.Register();
                Keyboard.Add();
                MegaCannon.Add();
                BarrelBullets.Register();
                BeamTurret.Init();
                HoloBullet.Init();
                RadCan.Register();
                WhileGunShoot.Add();
                OilJar.Register();
                MagnesiumGuonStone.Init();
                Catalyst.Add();
                SingleUseGun.Add();
                TableTechPause.Register();

                //version 1.1.0
                Gunbatus.Add();
                Trigunometry.Add();
                SilverCursor.Register();

                //version 1.1.1
                SlimePendant.Init();
                ACERifle.Add();

                //version 1.1.2
                Dematerialiser.Register();
                GungeonGun.Add();
                Chair.Add();

                //version 1.1.3
                Robullets.Register();
                Cryptocurrency.Init();
                GildedInfusion.Register();

                //version 1.1.4
                Piston.Add();
                TNTItem.Init();
                LaserShotgun.Add();
                DiscouragementBeam.Add();

                //version 2.0.0
                Drillets.Register();
                Omegear.Init();
                Servont.Init();
                InertiaBullets.Register();
                ScienceBook.Register();
                DisasterRecipe.Register();
                Plasmatic.Add();
                FaultyCapacitor.Register();
                BulletRedirectionConstruct.Init();
                //NEEDS FIXING, BLACK HOLE EFFECT DOES NOT WORK
                //NeutronStarBullets.Register();
                //SPRITE/NAME RANDOMISATION NEEDS FIXING
                //NonFungiBullets.Register();
                CashInSelloMatic.Register();
                MiningLaser.Add();
                MoonShots.Register();
                KrissKard.Add();

                AudioResourceLoader.InitAudio();

                var syn = CustomSynergies.Add("Meltdown Averted", new List<string> { "ai:reactor_core", "ice_cube" }, null, false);
                syn.ActiveWhenGunUnequipped = false;
                syn.statModifiers = new List<StatModifier>(0)
                {
                StatModifier.Create(PlayerStats.StatType.RateOfFire,StatModifier.ModifyMethod.ADDITIVE, 1f)
                };
                syn.bonusSynergies = new List<CustomSynergyType>();

                var syn1 = CustomSynergies.Add("Battery Buddies", new List<string> { "ai:vintage", "thunderclap" }, null, true);
                syn1.ActiveWhenGunUnequipped = false;
                syn1.bonusSynergies = new List<CustomSynergyType>();

                var syn2 = CustomSynergies.Add("Explosiver Guon Stone", new List<string> { "ai:magnesium_guon_stone", "bomb" }, null, true);
                syn2.bonusSynergies = new List<CustomSynergyType>();

                var syn3 = CustomSynergies.Add("2 Kool 4 U", new List<string> { "ai:rad_can", "rad_gun" }, null, false);
                syn3.ActiveWhenGunUnequipped = false;
                syn3.statModifiers = new List<StatModifier>(0)
                {
                    StatModifier.Create(PlayerStats.StatType.ReloadSpeed,StatModifier.ModifyMethod.ADDITIVE, -0.3f),
                    StatModifier.Create(PlayerStats.StatType.Coolness,StatModifier.ModifyMethod.ADDITIVE, 2f)
                };
                syn3.bonusSynergies = new List<CustomSynergyType>();

                var syn4 = CustomSynergies.Add("All That Glitters Is Gold", new List<string> { "au_gun" }, new List<string> { "gold_junk", "gold_ammolet", "gilded_bullets", }, false);
                syn4.ActiveWhenGunUnequipped = false;
                syn4.statModifiers = new List<StatModifier>(0)
                {
                    StatModifier.Create(PlayerStats.StatType.MoneyMultiplierFromEnemies,StatModifier.ModifyMethod.ADDITIVE,1f),
                    StatModifier.Create(PlayerStats.StatType.Damage,StatModifier.ModifyMethod.ADDITIVE,1f),
                };
                syn4.bonusSynergies = new List<CustomSynergyType>();

                var syn5 = CustomSynergies.Add("Ultimate Automation", new List<string> { "nanomachines", "bionic_leg" }, null, true);
                syn5.statModifiers = new List<StatModifier>(0)
                {
                    StatModifier.Create(PlayerStats.StatType.Curse,StatModifier.ModifyMethod.ADDITIVE,1f),
                    StatModifier.Create(PlayerStats.StatType.Accuracy,StatModifier.ModifyMethod.ADDITIVE,-3f),
                    StatModifier.Create(PlayerStats.StatType.ReloadSpeed,StatModifier.ModifyMethod.MULTIPLICATIVE,0.75f),
                };
                syn5.bonusSynergies = new List<CustomSynergyType>();

                var syn6 = CustomSynergies.Add("Knife To A Gunfight", new List<string> { "ai:enter_the_gungeon", "ai:slime_pendant" }, null, false);
                syn6.bonusSynergies = new List<CustomSynergyType>();

                var syn7 = CustomSynergies.Add("Children Of Kaliber", new List<string> { "ai:enter_the_gungeon", "high_kaliber" }, null, false);
                syn7.bonusSynergies = new List<CustomSynergyType>();

                var syn8 = CustomSynergies.Add("Sentry Mode Activated", new List<string> { "ai:robullets", "portable_turret" }, null, true);
                syn8.bonusSynergies = new List<CustomSynergyType>();

                var syn9 = CustomSynergies.Add("Isn't It Iron Pick", new List<string> { "ai:piston", "big_iron" }, null, true);
                syn9.bonusSynergies = new List<CustomSynergyType>();

                if (PickupObjectDatabase.GetByEncounterName("Dispenser") != null)
                {
                    var syn10 = CustomSynergies.Add("Redstone Engineering", new List<string> { "ai:piston", "cel:dispenser" }, null, true);
                    syn10.bonusSynergies = new List<CustomSynergyType>();
                }

                //var syn11 = CustomSynergies.Add("Cobblestone", new List<string> { "ai:stack_of_dirt" }, new List<string> { "pink_guon_stone", "red_guon_stone", "white_guon_stone", "orange_guon_stone", "clear_guon_stone", "blue_guon_stone", "green_guon_stone", "glass_guon_stone" }, true);
                //syn11.bonusSynergies = new List<CustomSynergyType>();

                //var syn12 = CustomSynergies.Add("Brick Block", new List<string> { "ai:stack_of_dirt" }, new List<string> { "brick_breaker", "brick_of_cash" }, true);
                //syn12.bonusSynergies = new List<CustomSynergyType>();

                //var syn13 = CustomSynergies.Add("Aperture Science", new List<string> { "ai:thermal_discouragement_beam", "science_cannon" }, null, true);
                //syn13.bonusSynergies = new List<CustomSynergyType>();

                var syn14 = CustomSynergies.Add("Upgrades People, Upgrades!", new List<string> { "ai:laser_shotgun", "laser_sight" }, null, false);
                syn14.ActiveWhenGunUnequipped = false;
                syn14.statModifiers = new List<StatModifier>(0)
                {
                    StatModifier.Create(PlayerStats.StatType.RangeMultiplier,StatModifier.ModifyMethod.ADDITIVE,0.5f),
                };
                syn14.bonusSynergies = new List<CustomSynergyType>();

                var syn15 = CustomSynergies.Add("Bullet Precision Construct", new List<string> { "ai:bullet_redirection_construct", "homing_bullets" }, null, true);
                syn15.bonusSynergies = new List<CustomSynergyType>();

                SynergyForms.AddSynergyForms();

                Log($"{MOD_NAME} v{VERSION} started successfully.", TEXT_COLOR);

                List<string> Quotes = new List<string>
                {
                    "*microwave noises*",
                    "System.NullReferenceException",
                    "50% of this code is from OMITB...",
                    "hmmmmmmmmmmmmmmmmmmm",
                    "Rad Cans: Now with 10% more rad!",
                    "It's not a bug, it's a feature",
                    "JokeReferenceException: Punchline not assigned to object Joke in static class Humour",
                    "THIS LINE HAS BEEN HASHED BY THE R&G DEPARTMENT FOR YOUR OWN SAFETY. DISREGARD ANY MENTION OF A SUSPICIOUS RED ASTRONAUT AND REPORT SUCH ACTIVITIES.",
                    "Scientist's Guide To The Gungeon: Now with 2 pages of even more science!",
                    "...",
                    "A wild null appeared!",
                    "Ensure your mods are up-to-date!",
                    "rest in piss mods.txt you will not be missed",
                    "Install your custom characters correctly!",
                    "Trans Rights!",
                    "hooks are the bane of my existence",
                    "Also try Prismatism!",
                    "Also try Planetside of Gunymede!",
                    "Also try OMITB!",
                    "Also try Children Of Kaliber!",
                    "Also try Cutting Room Floor!",
                    "Also try A Bleaker Item Pack!",
                    "Also try King's Items!",
                    "Also try Knife To A Gunfight!",
                    "Also try Enter The Beyond!",
                    "Oops, all gears!",
                    "This is a certified mod classic"
                };
                System.Random randomselector = new System.Random();
                int index = randomselector.Next(Quotes.Count);
                string idkwhattocallthis = Quotes[index];
                Log(idkwhattocallthis, TEXT_COLOR);
            }

            catch (Exception e)
            {
                Log(e.Message, ERROR_COLOR);
                Log(e.StackTrace, ERROR_COLOR);
                Log("shit went wrong, here ^", TEXT_COLOR);
                Log("@ me in #modding in the Gungeon Discord and I will try fixing your issue ASAP.", TEXT_COLOR);
            }
        }
        public static List<AGDEnemyReplacementTier> m_cachedReplacementTiers = GameManager.Instance.EnemyReplacementTiers;
        public static void OnQuickRestart(Action<GameManager, float, QuickRestartOptions> orig, GameManager self, float duration, QuickRestartOptions options)
        {
            orig(self, duration, options);
            EnemyReplace.RunReplace(m_cachedReplacementTiers);
        }
        public static void FoyerAwake(Action<Foyer> orig, Foyer self)
        {
            orig(self);
            EnemyReplace.RunReplace(m_cachedReplacementTiers);
        }
        public static void Log(string text, string color = "#FFFFFF")
        {
            ETGModConsole.Log($"<color={color}>{text}</color>");
        }
        public void Awake() { }
    }
}