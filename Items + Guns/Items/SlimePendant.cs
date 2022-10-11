using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using EnemyAPI;
using Gungeon;
using Alexandria.ItemAPI;
using UnityEngine;
using SaveAPI;

namespace GunRev
{
    public class SlimePendant : PassiveItem
    {
        public static void Init()
        {
            string name = "Slime Pendant";
            string resourcePath = "GunRev/Resources/slimebuddy/slimependant";
            GameObject gameObject = new GameObject();
            var companionItem = gameObject.AddComponent<CompanionItem>();
            ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
            string shortDesc = "100% Blobulite-246!";
            string longDesc = "A tiny, adorable slime clings to this bouncy pendant.\n\nBlobulite is a notoriously bouncy element, which has led to its usage in the manufacturing of rebounding artillery, tank shell protection coating, and fad children's toys.";
            companionItem.SetupItem(shortDesc, longDesc, "ai");
            companionItem.quality = PickupObject.ItemQuality.B;
            companionItem.CompanionGuid = SlimePendant.guid;

            SlimePendant.BuildPrefab();
        }
        public static void BuildPrefab()
        {
            bool flag = SlimePendant.prefab != null || CompanionBuilder.companionDictionary.ContainsKey(SlimePendant.guid);
            if (!flag)
            {
                SlimePendant.prefab = CompanionBuilder.BuildPrefab("Tiny Slime", SlimePendant.guid, "GunRev/Resources/slimebuddy/slime_idle_001", new IntVector2(0, 0), new IntVector2(7, 5));
                var companionController = SlimePendant.prefab.AddComponent<SlimeCompanionBehaviour>();
                prefab.AddAnimation("idle_left", "GunRev/Resources/slimebuddy/slime_idle", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.TwoWayHorizontal, DirectionalAnimation.FlipType.None);
                prefab.AddAnimation("idle_right", "GunRev/Resources/slimebuddy/slime_idle", 7, CompanionBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.TwoWayHorizontal, DirectionalAnimation.FlipType.None);
                prefab.AddAnimation("run_left", "GunRev/Resources/slimebuddy/slime_run_right", 12, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.TwoWayHorizontal, DirectionalAnimation.FlipType.None);
                prefab.AddAnimation("run_right", "GunRev/Resources/slimebuddy/slime_run_left", 12, CompanionBuilder.AnimationType.Move, DirectionalAnimation.DirectionType.TwoWayHorizontal, DirectionalAnimation.FlipType.None);
                companionController.CanInterceptBullets = false;
                companionController.companionID = CompanionController.CompanionIdentifier.NONE;
                companionController.aiActor.MovementSpeed = 7f;
                companionController.aiActor.healthHaver.PreventAllDamage = true;
                companionController.aiActor.CollisionDamage = 0f;
                companionController.aiActor.specRigidbody.CollideWithOthers = false;
                companionController.aiActor.specRigidbody.CollideWithTileMap = false;
                BehaviorSpeculator component = SlimePendant.prefab.GetComponent<BehaviorSpeculator>();
                CustomCompanionBehaviours.SimpleCompanionApproach approach = new CustomCompanionBehaviours.SimpleCompanionApproach();
                approach.DesiredDistance = 1;
                component.MovementBehaviors.Add(approach);
                component.MovementBehaviors.Add(new CompanionFollowPlayerBehavior
                {
                    IdleAnimations = new string[]
                    {
                        "idle"
                    }
                });
            }
        }

        public class SlimeCompanionBehaviour : CompanionController
        {
            public SlimeCompanionBehaviour()
            {
                this.DamagePerHit = 3.5f;
            }
            public override void OnDestroy()
            {
                base.OnDestroy();
            }
            private void Start()
            {
                this.Owner = this.m_owner;
            }
            private void FixedUpdate()
            {

            }
            public override void Update()
            {
                if (!GameManager.Instance.IsPaused)
                {
                    DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(PickupObjectDatabase.GetById(310)?.GetComponent<WingsItem>()?.RollGoop);
                    goopManagerForGoopType.TimedAddGoopCircle(base.specRigidbody.UnitCenter, 2, 0.1f, true);
                }
                base.Update();
            }
            public float DamagePerHit;
            private PlayerController Owner;
        }
        public static GameObject prefab;

        private static readonly string guid = "ai:tinyslime";
    }
}