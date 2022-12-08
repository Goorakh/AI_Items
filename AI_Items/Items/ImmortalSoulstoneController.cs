using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI_Items.Items
{
    static class ImmortalSoulstoneController
    {
        static BuffIndex[] _excludeBuffs;

        [SystemInitializer(typeof(BuffCatalog))]
        static void Init()
        {
            _excludeBuffs = new BuffIndex[]
            {
                BuffCatalog.FindBuffIndex("bdMedkitHeal")
            }
            .OrderBy(i => i)
            .ToArray();
        }

        internal static void InitHooks()
        {
            On.RoR2.CharacterBody.AddTimedBuff_BuffDef_float += static (orig, self, buffDef, duration) =>
            {
                if (buffDef && !buffDef.isDebuff && !buffDef.isCooldown && Array.BinarySearch(_excludeBuffs, buffDef.buffIndex) < 0)
                {
                    if (self)
                    {
                        Inventory inventory = self.inventory;
                        if (inventory)
                        {
                            int itemCount = inventory.GetItemCount(ItemDefs.ImmortalSoulstone.itemIndex);
                            if (itemCount > 0)
                            {
#if DEBUG
                                float oldDuration = duration;
#endif

                                duration *= 1f + itemCount;

#if DEBUG
                                if (oldDuration != duration)
                                {
                                    Log.Debug($"{nameof(ItemDefs.ImmortalSoulstone)} {buffDef.name} buff duration: {oldDuration}->{duration}");
                                }
#endif
                            }
                        }
                    }
                }

                orig(self, buffDef, duration);
            };
        }
    }
}
