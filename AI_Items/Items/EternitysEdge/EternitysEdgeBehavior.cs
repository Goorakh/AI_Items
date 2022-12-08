using AI_Items.Patches;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace AI_Items.Items.EternitysEdge
{
    public sealed class EternitysEdgeBehavior : CharacterBody.ItemBehavior
    {
        int _critChanceAndDamageBoostCount;
        int critChanceAndDamageBoostCount
        {
            get => _critChanceAndDamageBoostCount;
            set
            {
                _critChanceAndDamageBoostCount = value;
                if (body)
                {
                    body.SetBuffCount(BuffDefs.bdCritChanceAndDamageBoost.buffIndex, value);
                }
            }
        }

        void OnEnable()
        {
            OnHitEnemyHook.OnHitEnemy += OnHitEnemyHook_OnHitEnemy;
        }

        void OnDisable()
        {
            OnHitEnemyHook.OnHitEnemy -= OnHitEnemyHook_OnHitEnemy;
        }

        void OnHitEnemyHook_OnHitEnemy(DamageInfo damageInfo, GameObject victim)
        {
            const string LOG_PREFIX = $"{nameof(EternitysEdgeBehavior)}.{nameof(OnHitEnemyHook_OnHitEnemy)} ";

            if (stack <= 0)
                return;

            if (!damageInfo.attacker)
                return;

            if (damageInfo.attacker.TryGetComponent(out CharacterBody attackerBody))
            {
                if (attackerBody == body)
                {
                    if (damageInfo.crit)
                    {
#if DEBUG
                        Log.Debug(LOG_PREFIX + "landed crit, resetting boost");
#endif
                        critChanceAndDamageBoostCount = 0;
                    }
                    else
                    {
#if DEBUG
                        Log.Debug(LOG_PREFIX + "landed non-crit, adding boost");
#endif
                        critChanceAndDamageBoostCount++;
                    }
                }
            }
        }
    }
}
