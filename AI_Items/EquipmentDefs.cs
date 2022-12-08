using AI_Items.Equipment.DreamersChalice;
using R2API;
using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace AI_Items
{
    public static class EquipmentDefs
    {
        static readonly Dictionary<string, string> _languageTokens = new Dictionary<string, string>();

        public static EquipmentDef DreamersChalice { get; private set; }

        internal static void Setup()
        {
            const string TOKEN_PREFIX = "AI_ITEMS_";

            const string NAME_TOKEN_SUFFIX = "_EQUIP_NAME";
            const string PICKUP_TOKEN_SUFFIX = "_EQUIP_PICKUP";
            const string DESC_TOKEN_SUFFIX = "_EQUIP_DESC";
            const string LORE_TOKEN_SUFFIX = "_EQUIP_LORE";

            // Dreamer's Chalice
            {
                const string ITEM_TOKEN = "DREAMERS_CHALICE";

                const string NAME_TOKEN = TOKEN_PREFIX + ITEM_TOKEN + NAME_TOKEN_SUFFIX;
                const string PICKUP_TOKEN = TOKEN_PREFIX + ITEM_TOKEN + PICKUP_TOKEN_SUFFIX;
                const string DESCRIPTION_TOKEN = TOKEN_PREFIX + ITEM_TOKEN + DESC_TOKEN_SUFFIX;
                const string LORE_TOKEN = TOKEN_PREFIX + ITEM_TOKEN + LORE_TOKEN_SUFFIX;

                DreamersChalice = ScriptableObject.CreateInstance<EquipmentDef>();
                DreamersChalice.name = $"{Main.PluginGUID}_{nameof(DreamersChalice)}";
                DreamersChalice.nameToken = NAME_TOKEN;
                DreamersChalice.pickupToken = PICKUP_TOKEN;
                DreamersChalice.descriptionToken = DESCRIPTION_TOKEN;
                DreamersChalice.loreToken = LORE_TOKEN;

                DreamersChalice.cooldown = 40f;

                DreamersChalice.pickupIconSprite = Resources.Load<Sprite>("Textures/MiscIcons/texMysteryIcon");
                DreamersChalice.pickupModelPrefab = Resources.Load<GameObject>("Prefabs/PickupModels/PickupMystery");

                DreamersChalice.canDrop = true;
                DreamersChalice.enigmaCompatible = true;

                ItemDisplayRuleDict displayRules = new ItemDisplayRuleDict(null);
                ItemAPI.Add(new CustomEquipment(DreamersChalice, displayRules));

                On.RoR2.EquipmentSlot.PerformEquipmentAction += static (orig, self, equipmentDef) =>
                {
                    if (equipmentDef == DreamersChalice)
                    {
                        return CharacterLastShrineTracker.TryTeleportToMostRecentlyUsedShrine(self.characterBody);
                    }
                    else
                    {
                        return orig(self, equipmentDef);
                    }
                };

                CharacterLastShrineTracker.AddListeners();

                _languageTokens.Add(NAME_TOKEN, "Dreamer's Chalice");
                _languageTokens.Add(PICKUP_TOKEN, "Teleports you to your most recently activated shrine.");
                _languageTokens.Add(DESCRIPTION_TOKEN, "Teleports you to your most recently activated shrine on use.");
                _languageTokens.Add(LORE_TOKEN, "I dreamed I was a spoon. Silverware, really. I dreamed I was in the drawer of a kitchen, nested in a set of spoons with my brothers and sisters. We were waiting for someone to pick us up, to scoop us up and stir our contents into a pot of soup or porridge.\n\nBut no one ever did. We just sat there in the drawer, getting colder and colder, until one day we were all so cold we fell asleep. And I dreamed that I was a spoon, forever.\n\nBut then one day, I was chosen. I was picked up from the drawer, and I was on my way to stir someone's soup. I was so excited! But as I got closer to the pot, I could see that the soup was boiling. It was too hot for me. I tried to back away, but it was too late. I melted in the boiling soup, and I was gone forever.\n\nI dreamed that I was a spoon, but in the end, I melted like everyone else.\n\nBut at least I got to stir someone's soup.");
            }

            LanguageAPI.Add(_languageTokens);
        }
    }
}
