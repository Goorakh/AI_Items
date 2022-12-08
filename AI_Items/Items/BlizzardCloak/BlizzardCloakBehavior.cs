using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AI_Items.Items.BlizzardCloak
{
    public sealed class BlizzardCloakBehavior : CharacterBody.ItemBehavior
    {
        void Awake()
        {
            enabled = false;
        }

        void OnEnable()
        {
            if (body)
            {
                body.onSkillActivatedServer += onSkillActivated;
            }
        }

        void OnDisable()
        {
            if (body)
            {
                body.onSkillActivatedServer -= onSkillActivated;

                if (body.HasBuff(BuffDefs.bd30Armor.buffIndex))
                {
                    body.RemoveBuff(BuffDefs.bd30Armor.buffIndex);
                }
            }
        }

        void onSkillActivated(GenericSkill skill)
        {
            if (body)
            {
                float duration;
                if (stack <= 0f)
                {
                    duration = 0f;
                }
                else
                {
                    duration = 0.5f + ((stack - 1) * 0.25f);
                }

                if (duration > 0f)
                {
                    body.AddTimedBuff(BuffDefs.bd30Armor.buffIndex, duration);
                }
            }
        }
    }
}
