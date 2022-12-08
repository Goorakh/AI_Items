using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace AI_Items.Items.PotentGem
{
    public sealed class PotentGemBehavior : CharacterBody.ItemBehavior
    {
        const float UPDATE_INTERVAL = 0.2f;
        const float SLOW_DURATION = 0.5f;

        TeamComponent _ownerTeamComponent;
        float _updateTimer = 0f;

        void Awake()
        {
            enabled = false;
        }

        void OnEnable()
        {
            if (body)
            {
                _ownerTeamComponent = body.teamComponent;
            }
        }

        void OnDisable()
        {
            _ownerTeamComponent = null;
        }

        void FixedUpdate()
        {
            if (!NetworkServer.active)
                return;

            if (stack <= 0)
                return;

            if (_updateTimer <= 0f)
            {
                slowAllEnemiesInRange(15f + ((stack - 1) * 1f));
                _updateTimer = UPDATE_INTERVAL;
            }
            else
            {
                _updateTimer -= Time.fixedDeltaTime;
            }
        }

        void slowAllEnemiesInRange(float radius)
        {
            const string LOG_PREFIX = $"{nameof(PotentGemBehavior)}.{nameof(slowAllEnemiesInRange)} ";

            if (!_ownerTeamComponent)
            {
                Log.Warning(LOG_PREFIX + "null team component");
                return;
            }

            TeamIndex ownerTeam = _ownerTeamComponent.teamIndex;

            Collider[] overlapColliders = Physics.OverlapSphere(body.transform.position, radius, LayerIndex.entityPrecise.mask, QueryTriggerInteraction.Collide);
            foreach (Collider collider in overlapColliders)
            {
                CharacterBody victim = Util.HurtBoxColliderToBody(collider);
                if (victim && victim != body)
                {
                    HealthComponent healthComponent = victim.healthComponent;
                    if (healthComponent)
                    {
                        if (FriendlyFireManager.ShouldDirectHitProceed(healthComponent, ownerTeam))
                        {
                            victim.AddTimedBuff(RoR2Content.Buffs.Slow50, SLOW_DURATION, 1);
                        }
                    }
                }
            }
        }
    }
}
