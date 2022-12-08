using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;

namespace AI_Items.Patches
{
    static class HealthComponentTakeDamageHook
    {
        public delegate void ModifyDamageHookDelegate(HealthComponent instance, DamageInfo info, ref float resultingDamage);
        public static event ModifyDamageHookDelegate ModifyDamageHook;

        internal static void Init()
        {
            IL.RoR2.HealthComponent.TakeDamage += static il =>
            {
                const string LOG_PREFIX = $"{nameof(HealthComponentTakeDamageHook)} ({nameof(HealthComponent.TakeDamage)}) ";

                ILCursor c = new ILCursor(il);

                int damageLocalIndex = -1;
                if (c.TryGotoNext(static x => x.MatchLdarg(out _),
                                  static x => x.MatchLdfld<DamageInfo>(nameof(DamageInfo.damage)),
                                  x => x.MatchStloc(out damageLocalIndex)))
                {
                    c.Index++;

                    c.Emit(OpCodes.Dup); // DamageInfo arg
                    c.Index++;

                    c.Emit(OpCodes.Ldarg_0);

                    c.EmitDelegate(static (DamageInfo damageInfo, float damage, HealthComponent instance) =>
                    {
#if DEBUG
                        float oldDamage = damage;
#endif

                        ModifyDamageHook?.Invoke(instance, damageInfo, ref damage);

#if DEBUG
                        if (damage != oldDamage)
                        {
                            Log.Debug(LOG_PREFIX + $"damage hook result: {oldDamage}->{damage}");
                        }
#endif

                        return damage;
                    });
                }
                else
                {
                    Log.Warning(LOG_PREFIX + "patch failed: unable to find damage stloc index");
                }
            };
        }
    }
}
