using AI_Items.Content;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AI_Items
{
    public static class BuffDefs
    {
        public static BuffDef bd30Armor { get; private set; }
        public static BuffDef bdBloodLust { get; private set; }
        public static BuffDef bdCritChanceAndDamageBoost { get; private set; }

        internal static void Setup()
        {
            // bdBlizzardCloak
            {
                bd30Armor = ScriptableObject.CreateInstance<BuffDef>();
                bd30Armor.name = nameof(bd30Armor);
                bd30Armor.canStack = false;
                bd30Armor.isDebuff = false;
                bd30Armor.isHidden = true;

                ContentPackHandler.Register(bd30Armor);
            }

            // bdBloodLust
            {
                bdBloodLust = ScriptableObject.CreateInstance<BuffDef>();
                bdBloodLust.name = nameof(bdBloodLust);
                bdBloodLust.canStack = false;
                bdBloodLust.isDebuff = false;

                ContentPackHandler.Register(bdBloodLust);
            }

            // bdCritChanceAndDamageBoost
            {
                bdCritChanceAndDamageBoost = ScriptableObject.CreateInstance<BuffDef>();
                bdCritChanceAndDamageBoost.name = nameof(bdCritChanceAndDamageBoost);
                bdCritChanceAndDamageBoost.canStack = true;
                bdCritChanceAndDamageBoost.isDebuff = false;

                ContentPackHandler.Register(bdCritChanceAndDamageBoost);
            }
        }
    }
}
