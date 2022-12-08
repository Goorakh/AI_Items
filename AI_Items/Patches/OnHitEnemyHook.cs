using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace AI_Items.Patches
{
    static class OnHitEnemyHook
    {
        public delegate void OnHitEnemyDelegate(DamageInfo damageInfo, GameObject victim);
        public static event OnHitEnemyDelegate OnHitEnemy;

        internal static void Init()
        {
            On.RoR2.GlobalEventManager.OnHitEnemy += static (orig, self, damageInfo, victim) =>
            {
                orig(self, damageInfo, victim);

                if (NetworkServer.active && damageInfo.procCoefficient > 0f && !damageInfo.rejected)
                {
                    OnHitEnemy?.Invoke(damageInfo, victim);
                }
            };
        }
    }
}
