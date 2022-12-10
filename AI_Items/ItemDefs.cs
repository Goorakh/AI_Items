using AI_Items.Items;
using AI_Items.Items.BlizzardCloak;
using AI_Items.Items.EternitysEdge;
using AI_Items.Items.IcyWind;
using AI_Items.Items.PotentGem;
using AI_Items.Patches;
using R2API;
using RoR2;
using RoR2.Orbs;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace AI_Items
{
    public static class ItemDefs
    {
        static readonly Dictionary<string, string> _languageTokens = new Dictionary<string, string>();

        public static ItemDef CoreVitality { get; private set; }
        public static ItemDef IcyWinds { get; private set; }
        public static ItemDef BlizzardCloak { get; private set; }
        public static ItemDef PotentGem { get; private set; }
        public static ItemDef FreezeOnHit { get; private set; }
        public static ItemDef BloodyKnife { get; private set; }
        public static ItemDef ScholarsMask { get; private set; }
        public static ItemDef BodyArmor { get; private set; }
        public static ItemDef BodyArmor_Consumed { get; private set; }
        public static ItemDef EternitysEdge { get; private set; }
        public static ItemDef OrbOfEnlightenment { get; private set; }
        public static ItemDef ImmortalSoulstone { get; private set; }

        internal static void Setup()
        {
            ItemTierDef itemTier1Def = Addressables.LoadAssetAsync<ItemTierDef>("RoR2/Base/Common/Tier1Def.asset").WaitForCompletion();
            ItemTierDef itemTier2Def = Addressables.LoadAssetAsync<ItemTierDef>("RoR2/Base/Common/Tier2Def.asset").WaitForCompletion();
            ItemTierDef itemTier3Def = Addressables.LoadAssetAsync<ItemTierDef>("RoR2/Base/Common/Tier3Def.asset").WaitForCompletion();

            const string TOKEN_PREFIX = "AI_ITEMS_";

            const string NAME_TOKEN_SUFFIX = "_ITEM_NAME";
            const string PICKUP_TOKEN_SUFFIX = "_ITEM_PICKUP";
            const string DESC_TOKEN_SUFFIX = "_ITEM_DESC";
            const string LORE_TOKEN_SUFFIX = "_ITEM_LORE";

            // Core Vitality
            {
                const string ITEM_TOKEN = "CORE_VITALITY";

                const string NAME_TOKEN = TOKEN_PREFIX + ITEM_TOKEN + NAME_TOKEN_SUFFIX;
                const string PICKUP_TOKEN = TOKEN_PREFIX + ITEM_TOKEN + PICKUP_TOKEN_SUFFIX;
                const string DESCRIPTION_TOKEN = TOKEN_PREFIX + ITEM_TOKEN + DESC_TOKEN_SUFFIX;
                const string LORE_TOKEN = TOKEN_PREFIX + ITEM_TOKEN + LORE_TOKEN_SUFFIX;

                CoreVitality = ScriptableObject.CreateInstance<ItemDef>();
                CoreVitality.name = $"{Main.PluginGUID}_{nameof(CoreVitality)}";
                CoreVitality.nameToken = NAME_TOKEN;
                CoreVitality.pickupToken = PICKUP_TOKEN;
                CoreVitality.descriptionToken = DESCRIPTION_TOKEN;
                CoreVitality.loreToken = LORE_TOKEN;

                CoreVitality._itemTierDef = itemTier2Def;

                CoreVitality.tags = new ItemTag[]
                {
                    ItemTag.Utility,
                    ItemTag.AIBlacklist
                };

                CoreVitality.pickupIconSprite = Resources.Load<Sprite>("Textures/MiscIcons/texMysteryIcon");
                CoreVitality.pickupModelPrefab = Resources.Load<GameObject>("Prefabs/PickupModels/PickupMystery");

                CoreVitality.canRemove = true;

                ItemDisplayRuleDict displayRules = new ItemDisplayRuleDict(null);
                ItemAPI.Add(new CustomItem(CoreVitality, displayRules));

                HealthComponentTakeDamageHook.ModifyDamageHook += static (HealthComponent instance, DamageInfo info, ref float resultingDamage) =>
                {
                    if (!info.attacker)
                        return;

                    if (!info.attacker.TryGetComponent<CharacterBody>(out CharacterBody attackerBody))
                        return;

                    if (!instance || !instance.body)
                        return;

                    Inventory inventory = instance.body.inventory;
                    if (!inventory)
                        return;

                    if (attackerBody.isElite)
                    {
                        int itemCount = inventory.GetItemCount(CoreVitality.itemIndex);
                        if (itemCount > 0)
                        {
#if DEBUG
                            float oldDamage = resultingDamage;
#endif

                            resultingDamage = Mathf.Max(1, resultingDamage - (10 + ((itemCount - 1) * 10)));

#if DEBUG
                            if (oldDamage != resultingDamage)
                            {
                                Log.Debug($"{nameof(CoreVitality)} damage hook: {oldDamage}->{resultingDamage}");
                            }
#endif
                        }
                    }
                };

                _languageTokens.Add(NAME_TOKEN, "Core Vitality");
                _languageTokens.Add(PICKUP_TOKEN, "Receive a flat damage reduction from all elites.");
                _languageTokens.Add(DESCRIPTION_TOKEN, "Damage taken from any elite type is decreased by <style=cIsDamage>10 <style=cStack>(+10 per stack)</style></style>. Cannot be reduced below <style=cIsDamage>1</style>.");
                _languageTokens.Add(LORE_TOKEN, "Core Vitality was the most important invention since the wheel. It was a small, round, battery-sized device that you inserted into your chest. Once it was in place, it provided you with a never-ending source of energy and health.\n\nNaturally, the Core Vitality was a hot commodity, and people were willing to pay any price to get their hands on one. The inventor, a small, bald man named Jerry, became a millionaire overnight.\n\nJerry was a kind man, and he wanted to make sure that everyone had access to Core Vitality. So he decided to open up his own store, and sell them for a fraction of the price.\n\nPeople were skeptical at first, but when they saw how healthy and energetic Jerry looked, they were convinced. Jerry's store became a huge success, and Core Vitality became a household name.");
            }

            // Icy Winds
            {
                const string ITEM_TOKEN = "ICY_WINDS";

                const string NAME_TOKEN = TOKEN_PREFIX + ITEM_TOKEN + NAME_TOKEN_SUFFIX;
                const string PICKUP_TOKEN = TOKEN_PREFIX + ITEM_TOKEN + PICKUP_TOKEN_SUFFIX;
                const string DESCRIPTION_TOKEN = TOKEN_PREFIX + ITEM_TOKEN + DESC_TOKEN_SUFFIX;
                const string LORE_TOKEN = TOKEN_PREFIX + ITEM_TOKEN + LORE_TOKEN_SUFFIX;

                IcyWinds = ScriptableObject.CreateInstance<ItemDef>();
                IcyWinds.name = $"{Main.PluginGUID}_{nameof(IcyWinds)}";
                IcyWinds.nameToken = NAME_TOKEN;
                IcyWinds.pickupToken = PICKUP_TOKEN;
                IcyWinds.descriptionToken = DESCRIPTION_TOKEN;
                IcyWinds.loreToken = LORE_TOKEN;

                IcyWinds._itemTierDef = itemTier1Def;

                IcyWinds.tags = new ItemTag[]
                {
                    ItemTag.Damage
                };

                IcyWinds.pickupIconSprite = Resources.Load<Sprite>("Textures/MiscIcons/texMysteryIcon");
                IcyWinds.pickupModelPrefab = Resources.Load<GameObject>("Prefabs/PickupModels/PickupMystery");

                IcyWinds.canRemove = true;

                ItemDisplayRuleDict displayRules = new ItemDisplayRuleDict(null);
                ItemAPI.Add(new CustomItem(IcyWinds, displayRules));

                RecalculateStatsAPI.GetStatCoefficients += static (sender, args) =>
                {
                    if (!sender)
                        return;

                    Inventory inventory = sender.inventory;
                    if (!inventory)
                        return;

                    HealthComponent healthComponent = sender.healthComponent;
                    if (!healthComponent)
                        return;

                    if (healthComponent.combinedHealthFraction < 0.5f)
                    {
                        args.critAdd += inventory.GetItemCount(IcyWinds.itemIndex) * 10f;
                    }
                };

                CharacterBody.onBodyInventoryChangedGlobal += static body =>
                {
                    if (!body)
                        return;

                    Inventory inventory = body.inventory;
                    if (!inventory)
                        return;

                    body.AddItemBehavior<IcyWindBehavior>(inventory.GetItemCount(IcyWinds.itemIndex));
                };

                _languageTokens.Add(NAME_TOKEN, "Icy Winds");
                _languageTokens.Add(PICKUP_TOKEN, "Increased critical hit chance when at low health.");
                _languageTokens.Add(DESCRIPTION_TOKEN, "<style=cIsUtility>+10%</style> <style=cStack>(+10% per stack)</style> critical hit chance when below 50% health.");
                _languageTokens.Add(LORE_TOKEN, "The icy wind was blowing again. Anna had forgotten to bring her gloves, and now her hands were numb and tingling. She wished she had remembered her scarf too, but it was in her other coat pocket. She wrapped her arms around herself and trudged onwards.\n\nSuddenly, she heard a strange noise. It sounded like... sing-song laughter? She turned around, and saw a group of small creatures walking towards her. They were dressed in strange clothes, and they had the most ridiculous haircuts.\n\nThe creatures walked right past her, still laughing. Anna was about to continue on her way when she heard one of them say \"Icy Wind, Icy Wind!\". She looked back and saw that the creatures had all stopped and were looking at her.\n\n\"What?\" she asked.\n\n\"Icy Wind, Icy Wind!\" the creatures sang again.\n\nAnna tried to walk away, but the creatures followed her. \"Icy Wind, Icy Wind!\" they chanted.\n\nAnna was getting angry now. \"What do you want with me?\" she yelled.\n\nThe creatures stopped chanting and looked at each other. One of them stepped forward. \"We just wanted to say hello,\" he said.\n\n\"Hello,\" Anna replied coldly.\n\nThe creature smiled. \"Have a nice day.\"");
            }

            // Blizzard Cloak
            {
                const string ITEM_TOKEN = "BLIZZARD_CLOAK";

                const string NAME_TOKEN = TOKEN_PREFIX + ITEM_TOKEN + NAME_TOKEN_SUFFIX;
                const string PICKUP_TOKEN = TOKEN_PREFIX + ITEM_TOKEN + PICKUP_TOKEN_SUFFIX;
                const string DESCRIPTION_TOKEN = TOKEN_PREFIX + ITEM_TOKEN + DESC_TOKEN_SUFFIX;
                const string LORE_TOKEN = TOKEN_PREFIX + ITEM_TOKEN + LORE_TOKEN_SUFFIX;

                BlizzardCloak = ScriptableObject.CreateInstance<ItemDef>();
                BlizzardCloak.name = $"{Main.PluginGUID}_{nameof(BlizzardCloak)}";
                BlizzardCloak.nameToken = NAME_TOKEN;
                BlizzardCloak.pickupToken = PICKUP_TOKEN;
                BlizzardCloak.descriptionToken = DESCRIPTION_TOKEN;
                BlizzardCloak.loreToken = LORE_TOKEN;

                BlizzardCloak._itemTierDef = itemTier2Def;

                BlizzardCloak.pickupIconSprite = Resources.Load<Sprite>("Textures/MiscIcons/texMysteryIcon");
                BlizzardCloak.pickupModelPrefab = Resources.Load<GameObject>("Prefabs/PickupModels/PickupMystery");

                BlizzardCloak.canRemove = true;

                BlizzardCloak.tags = new ItemTag[]
                {
                    ItemTag.Utility
                };

                ItemDisplayRuleDict displayRules = new ItemDisplayRuleDict(null);
                ItemAPI.Add(new CustomItem(BlizzardCloak, displayRules));

                CharacterBody.onBodyInventoryChangedGlobal += static body =>
                {
                    if (!body)
                        return;

                    Inventory inventory = body.inventory;
                    if (!inventory)
                        return;

                    body.AddItemBehavior<BlizzardCloakBehavior>(inventory.GetItemCount(BlizzardCloak.itemIndex));
                };

                RecalculateStatsAPI.GetStatCoefficients += static (body, args) =>
                {
                    if (!body)
                        return;

                    if (body.HasBuff(BuffDefs.bd30Armor.buffIndex))
                    {
                        args.armorAdd += 30;
                    }
                };

                _languageTokens.Add(NAME_TOKEN, "Blizzard Cloak");
                _languageTokens.Add(PICKUP_TOKEN, "Reduced damage taken for a short period after using a skill");
                _languageTokens.Add(DESCRIPTION_TOKEN, "<style=cIsUtility>+30</style> armor for <style=cIsUtility>0.5 seconds</style> <style=cStack>(+0.25s per stack)</style> after using a skill.");
                _languageTokens.Add(LORE_TOKEN, "I was having the worst day. My alarm didn't go off, I spilled coffee on my shirt, and then I got a ticket on the way to work. I thought things couldn't possibly get worse, but they did.\n\nI was walking down the street when I saw a guy in a cloak walking towards me. It was a blizzard outside and I could see the guy's breath fogging up in the air. As he got closer, I could see that his eyes were glowing a bright blue.\n\nI started to run, but the guy was too fast. He grabbed me and put his hand over my mouth. I could feel his freezing cold fingers on my skin. Then everything went black.\n\nWhen I woke up, I was in a dark room. The cloak guy was there, laughing at me. He told me that he had taken me to his lair, and that I was going to be his slave forever.\n\nI didn't know what to do, but then I had an idea. I asked the guy if I could borrow his cloak for a little bit. He said yes, and I put it on.\n\nAs soon as the cloak touched my skin, I felt a warm rush of energy course through my body. I knew what I had to do. I took the cloak off and hit the guy over the head with it. Then I took off running.\n\nI don't know what the cloak does, but I'm not going to question my good luck. I'm just going to enjoy my new-found freedom.");
            }

            // Potent Gem
            {
                const string ITEM_TOKEN = "POTENT_GEM";

                const string NAME_TOKEN = TOKEN_PREFIX + ITEM_TOKEN + NAME_TOKEN_SUFFIX;
                const string PICKUP_TOKEN = TOKEN_PREFIX + ITEM_TOKEN + PICKUP_TOKEN_SUFFIX;
                const string DESCRIPTION_TOKEN = TOKEN_PREFIX + ITEM_TOKEN + DESC_TOKEN_SUFFIX;
                const string LORE_TOKEN = TOKEN_PREFIX + ITEM_TOKEN + LORE_TOKEN_SUFFIX;

                PotentGem = ScriptableObject.CreateInstance<ItemDef>();
                PotentGem.name = $"{Main.PluginGUID}_{nameof(PotentGem)}";
                PotentGem.nameToken = NAME_TOKEN;
                PotentGem.pickupToken = PICKUP_TOKEN;
                PotentGem.descriptionToken = DESCRIPTION_TOKEN;
                PotentGem.loreToken = LORE_TOKEN;

                PotentGem._itemTierDef = itemTier2Def;

                PotentGem.tags = new ItemTag[]
                {
                    ItemTag.Utility
                };

                PotentGem.pickupIconSprite = Resources.Load<Sprite>("Textures/MiscIcons/texMysteryIcon");
                PotentGem.pickupModelPrefab = Resources.Load<GameObject>("Prefabs/PickupModels/PickupMystery");

                PotentGem.canRemove = true;

                ItemDisplayRuleDict displayRules = new ItemDisplayRuleDict(null);
                ItemAPI.Add(new CustomItem(PotentGem, displayRules));

                CharacterBody.onBodyInventoryChangedGlobal += static body =>
                {
                    if (!body)
                        return;

                    Inventory inventory = body.inventory;
                    if (!inventory)
                        return;

                    body.AddItemBehavior<PotentGemBehavior>(inventory.GetItemCount(PotentGem.itemIndex));
                };

                _languageTokens.Add(NAME_TOKEN, "Potent Gem");
                _languageTokens.Add(PICKUP_TOKEN, "Constantly apply a slow to nearby enemies");
                _languageTokens.Add(DESCRIPTION_TOKEN, "<style=cIsUtility>Slows</style> all enemies within <style=cIsDamage>15m</style> <style=cStack>(+2m per stack)</style>");
                _languageTokens.Add(LORE_TOKEN, "A gem found on a distant planet was found to have unusual properties. It was so potent that even a small fragment could create a powerful energy field. The gem was quickly claimed by the planet's government and put on display in the capital city.\n\nTourists and visitors from other planets came to see the amazing gem. Some were even brave enough to touch it. But no one knew its true power until one brave scientist decided to study it more closely.\n\nHe discovered that the gem could not only create energy fields, but it could also control minds. The government quickly took the gem back and put it in a secret room where no one could find it.\n\nBut even with the gem hidden away, the scientist's mind had been taken over by the gem. He could not stop thinking about it and how he could get his hands on it again. The scientist became a shadow of his former self and soon died, still thinking about the gem.");
            }

            // Frostwyrmkin's Wail
            {
                const string ITEM_TOKEN = "FREEZE_ON_HIT";

                const string NAME_TOKEN = TOKEN_PREFIX + ITEM_TOKEN + NAME_TOKEN_SUFFIX;
                const string PICKUP_TOKEN = TOKEN_PREFIX + ITEM_TOKEN + PICKUP_TOKEN_SUFFIX;
                const string DESCRIPTION_TOKEN = TOKEN_PREFIX + ITEM_TOKEN + DESC_TOKEN_SUFFIX;
                const string LORE_TOKEN = TOKEN_PREFIX + ITEM_TOKEN + LORE_TOKEN_SUFFIX;

                FreezeOnHit = ScriptableObject.CreateInstance<ItemDef>();
                FreezeOnHit.name = $"{Main.PluginGUID}_{nameof(FreezeOnHit)}";
                FreezeOnHit.nameToken = NAME_TOKEN;
                FreezeOnHit.pickupToken = PICKUP_TOKEN;
                FreezeOnHit.descriptionToken = DESCRIPTION_TOKEN;
                FreezeOnHit.loreToken = LORE_TOKEN;

                FreezeOnHit._itemTierDef = itemTier3Def;

                FreezeOnHit.tags = new ItemTag[]
                {
                    ItemTag.Damage
                };

                FreezeOnHit.pickupIconSprite = Resources.Load<Sprite>("Textures/MiscIcons/texMysteryIcon");
                FreezeOnHit.pickupModelPrefab = Resources.Load<GameObject>("Prefabs/PickupModels/PickupMystery");

                FreezeOnHit.canRemove = true;

                ItemDisplayRuleDict displayRules = new ItemDisplayRuleDict(null);
                ItemAPI.Add(new CustomItem(FreezeOnHit, displayRules));

                On.RoR2.HealthComponent.TakeDamage += static (orig, self, damageInfo) =>
                {
                    if (damageInfo != null && damageInfo.attacker && damageInfo.attacker.TryGetComponent<CharacterBody>(out CharacterBody body))
                    {
                        Inventory inventory = body.inventory;
                        if (inventory)
                        {
                            int itemCount = inventory.GetItemCount(FreezeOnHit.itemIndex);
                            if (itemCount > 0)
                            {
                                if (Util.CheckRoll(5f + ((itemCount - 1) * 2.5f), body.master))
                                {
                                    damageInfo.damageType |= DamageType.Freeze2s;
                                }
                            }
                        }
                    }

                    orig(self, damageInfo);
                };

                _languageTokens.Add(NAME_TOKEN, "Frostwyrmkin's Wail");
                _languageTokens.Add(PICKUP_TOKEN, "Chance to freeze enemies on hit");
                _languageTokens.Add(DESCRIPTION_TOKEN, "<style=cIsUtility>5%</style> <style=cStack>(+2.5% per stack)</style> chance to freeze enemies on hit.");
                _languageTokens.Add(LORE_TOKEN, "It's the end of the world. That's what they say. But who knows for sure? The only thing that's for certain is that the Frostwyrmkin's Wail is coming.\n\nSome people say it's a mythical creature, a dragon made of ice that heralds the end of days. Others say it's an alien spaceship, come to wipe us all out. But no one really knows for sure.\n\nWhat we do know is that it's coming, and that's enough to send chills down our spines. We've all heard the stories, seen the evidence. The world is ending, and there's nothing we can do about it.\n\nExcept maybe laugh about it.\n\nYes, maybe we can't save the world, but that doesn't mean we can't have a little fun with it. Let's face it, the end of the world is going to be pretty boring if we're all too scared to have any fun.\n\nSo let's party like there's no tomorrow. Let's dance until we can't dance anymore. Let's drink until we can't drink anymore. Let's laugh until we can't laugh anymore.\n\nAnd when the Frostwyrmkin's Wail comes, let's welcome it with open arms. It might be the end of the world, but at least we'll go out with a smile on our faces.");
            }

            // Bloody Knife
            {
                const string ITEM_TOKEN = "BLOODY_KNIFE";

                const string NAME_TOKEN = TOKEN_PREFIX + ITEM_TOKEN + NAME_TOKEN_SUFFIX;
                const string PICKUP_TOKEN = TOKEN_PREFIX + ITEM_TOKEN + PICKUP_TOKEN_SUFFIX;
                const string DESCRIPTION_TOKEN = TOKEN_PREFIX + ITEM_TOKEN + DESC_TOKEN_SUFFIX;
                const string LORE_TOKEN = TOKEN_PREFIX + ITEM_TOKEN + LORE_TOKEN_SUFFIX;

                BloodyKnife = ScriptableObject.CreateInstance<ItemDef>();
                BloodyKnife.name = $"{Main.PluginGUID}_{nameof(BloodyKnife)}";
                BloodyKnife.nameToken = NAME_TOKEN;
                BloodyKnife.pickupToken = PICKUP_TOKEN;
                BloodyKnife.descriptionToken = DESCRIPTION_TOKEN;
                BloodyKnife.loreToken = LORE_TOKEN;

                BloodyKnife._itemTierDef = itemTier2Def;

                BloodyKnife.tags = new ItemTag[]
                {
                    ItemTag.Damage,
                    ItemTag.OnKillEffect
                };

                BloodyKnife.pickupIconSprite = Resources.Load<Sprite>("Textures/MiscIcons/texMysteryIcon");
                BloodyKnife.pickupModelPrefab = Resources.Load<GameObject>("Prefabs/PickupModels/PickupMystery");

                BloodyKnife.canRemove = true;

                ItemDisplayRuleDict displayRules = new ItemDisplayRuleDict(null);
                ItemAPI.Add(new CustomItem(BloodyKnife, displayRules));

                GlobalEventManager.onCharacterDeathGlobal += static (damageReport) =>
                {
                    if (!NetworkServer.active)
                        return;

                    if (damageReport == null)
                        return;

                    if (!damageReport.victim || !damageReport.attackerMaster || !damageReport.attackerBody)
                        return;

                    Inventory attackerInventory = damageReport.attackerMaster.inventory;
                    if (!attackerInventory)
                        return;

                    int itemCount = attackerInventory.GetItemCount(BloodyKnife.itemIndex);
                    if (itemCount <= 0)
                        return;

                    float buffDuration = 1f + ((itemCount - 1) * 0.25f);
                    damageReport.attackerBody.AddTimedBuff(BuffDefs.bdBloodLust, buffDuration, 1);
                };

                RecalculateStatsAPI.GetStatCoefficients += static (sender, args) =>
                {
                    if (!sender)
                        return;

                    Inventory inventory = sender.inventory;
                    if (!inventory)
                        return;

                    if (sender.HasBuff(BuffDefs.bdBloodLust))
                    {
                        const float PERCENTAGE_ADD = 0.4f;

                        args.attackSpeedMultAdd += PERCENTAGE_ADD;
                        args.critAdd += PERCENTAGE_ADD * 100f;
                    }
                };

                _languageTokens.Add(NAME_TOKEN, "Bloody Knife");
                _languageTokens.Add(PICKUP_TOKEN, "Gain attack speed and crit chance on kill");
                _languageTokens.Add(DESCRIPTION_TOKEN, "On kill: Gain <style=cIsUtility>+40%</style> <style=cIsDamage>attack speed</style> and <style=cIsDamage>crit chance</style> for <style=cIsUtility>1 second</style> <style=cStack>(+0.25s per stack)</style>.");
                _languageTokens.Add(LORE_TOKEN, "The scene was like something out of a sci-fi horror film. The small townspeople were gathered around a figure sprawled on the ground, their eyes wide with shock. Then, from the corner of the crowd, came a woman dressed entirely in black with a gleaming, blood red blade in one hand. With an intense look, she held the Bloody Knife up for all to see and declared, “This is why we can never trust technology!”\r\n\r\nThere was an audible gasp from the crowd as the realization of what had happened sunk in. It was true; too many people had been lost to advances in robotics that just couldn't replace a human touch. They'd all heard the rumors about robots malfunctioning and taking innocent lives—and now this.\r\n\r\nThe woman smiled sheepishly as she tucked the Bloody Knife away, an apology in her eyes. She explained that she'd been trying to program a robot to work a kitchen knife but as it turned out, it didn't know when to stop slicing tomatoes. Unfortunately, it also didn't know when to stop slicing people. \r\n\r\nA few chuckles broke out amidst the murmurs that followed as people processed what they'd just heard. In spite of it all, it was probably normal for something like this to happen in a world this complicated. Besides, it could have been much worse, though the Bloody Knife still remained a shining reminder of the dangers of technology.");
            }

            // Scholar's Mask
            {
                const string ITEM_TOKEN = "SCHOLARS_MASK";

                const string NAME_TOKEN = TOKEN_PREFIX + ITEM_TOKEN + NAME_TOKEN_SUFFIX;
                const string PICKUP_TOKEN = TOKEN_PREFIX + ITEM_TOKEN + PICKUP_TOKEN_SUFFIX;
                const string DESCRIPTION_TOKEN = TOKEN_PREFIX + ITEM_TOKEN + DESC_TOKEN_SUFFIX;
                const string LORE_TOKEN = TOKEN_PREFIX + ITEM_TOKEN + LORE_TOKEN_SUFFIX;

                ScholarsMask = ScriptableObject.CreateInstance<ItemDef>();
                ScholarsMask.name = $"{Main.PluginGUID}_{nameof(ScholarsMask)}";
                ScholarsMask.nameToken = NAME_TOKEN;
                ScholarsMask.pickupToken = PICKUP_TOKEN;
                ScholarsMask.descriptionToken = DESCRIPTION_TOKEN;
                ScholarsMask.loreToken = LORE_TOKEN;

                ScholarsMask._itemTierDef = itemTier2Def;

                ScholarsMask.tags = new ItemTag[]
                {
                    ItemTag.Healing,
                    ItemTag.AIBlacklist,
                    ItemTag.InteractableRelated
                };

                ScholarsMask.pickupIconSprite = Resources.Load<Sprite>("Textures/MiscIcons/texMysteryIcon");
                ScholarsMask.pickupModelPrefab = Resources.Load<GameObject>("Prefabs/PickupModels/PickupMystery");

                ScholarsMask.canRemove = true;

                ItemDisplayRuleDict displayRules = new ItemDisplayRuleDict(null);
                ItemAPI.Add(new CustomItem(ScholarsMask, displayRules));

                GlobalEventManager.OnInteractionsGlobal += static (interactor, interactable, interactableObject) =>
                {
                    if (!interactor || !interactableObject)
                        return;

                    CharacterBody interactorBody = interactor.GetComponent<CharacterBody>();
                    if (!interactorBody)
                        return;

                    Inventory inventory = interactorBody.inventory;
                    if (!inventory)
                        return;

                    TeamComponent teamComponent = interactorBody.teamComponent;
                    if (!teamComponent)
                        return;

                    int maskCount = inventory.GetItemCount(ScholarsMask.itemIndex);
                    if (maskCount <= 0)
                        return;

                    if (!Util.CheckRoll(30f))
                        return;

                    Vector3 interactablePosition = interactableObject.transform.position;

                    float percentageHeal = (5f / 100f) + (maskCount * (1f / 100f));
                    foreach (TeamComponent teamMember in TeamComponent.GetTeamMembers(teamComponent.teamIndex))
                    {
                        if (teamMember)
                        {
                            CharacterBody teamMemberBody = teamMember.body;
                            if (teamMemberBody)
                            {
                                HealthComponent teamMemberHealthComponent = teamMemberBody.healthComponent;
                                if (teamMemberHealthComponent)
                                {
                                    HealOrb healOrb = new HealOrb()
                                    {
                                        origin = interactablePosition,
                                        target = teamMemberBody.mainHurtBox,
                                        healValue = teamMemberHealthComponent.fullHealth * percentageHeal
                                    };
                                    healOrb.overrideDuration = Mathf.Clamp(healOrb.distanceToTarget / 100f, healOrb.overrideDuration, 2f);

                                    OrbManager.instance.AddOrb(healOrb);
                                }
                            }
                        }
                    }
                };

                _languageTokens.Add(NAME_TOKEN, "Scholar's Mask");
                _languageTokens.Add(PICKUP_TOKEN, "Chance on activation of an interactable to heal all allies.");
                _languageTokens.Add(DESCRIPTION_TOKEN, "<style=cIsUtility>30%</style> chance on activating an interactable to heal all allies for <style=cIsHealing>5%</style> <style=cStack>(+1% per stack)</style> <style=cIsHealing>of their maximum health</style>");
                _languageTokens.Add(LORE_TOKEN, "Marta looked up from her book and admired the mask on the shelf. It was beautiful, with its intricate carvings and delicate gold trim. She had always loved masks, and this one was no exception.\n\nShe was about to put it back when she heard a voice behind her.\n\n\"That's a Scholar's Mask.\"\n\nMarta turned around to see a man standing there, watching her.\n\n\"I know,\" she said, \"I just love masks.\"\n\n\"It's not just a mask,\" the man said. \"It's a very powerful artifact.\"\n\nMarta was intrigued. \"What does it do?\"\n\n\"It amplifies the wearer's intelligence and wisdom,\" the man said.\n\nMarta was sold. She grabbed the mask and put it on. Immediately, she could feel the power coursing through her. She felt smarter, wiser. She could feel the knowledge of centuries flowing through her mind.\n\nThe man smiled. \"Now you are a true scholar.\"");
            }

            // Body Armor
            {
                const string ITEM_TOKEN = "BODY_ARMOR";

                const string NAME_TOKEN = TOKEN_PREFIX + ITEM_TOKEN + NAME_TOKEN_SUFFIX;
                const string PICKUP_TOKEN = TOKEN_PREFIX + ITEM_TOKEN + PICKUP_TOKEN_SUFFIX;
                const string DESCRIPTION_TOKEN = TOKEN_PREFIX + ITEM_TOKEN + DESC_TOKEN_SUFFIX;
                const string LORE_TOKEN = TOKEN_PREFIX + ITEM_TOKEN + LORE_TOKEN_SUFFIX;

                BodyArmor = ScriptableObject.CreateInstance<ItemDef>();
                BodyArmor.name = $"{Main.PluginGUID}_{nameof(BodyArmor)}";
                BodyArmor.nameToken = NAME_TOKEN;
                BodyArmor.pickupToken = PICKUP_TOKEN;
                BodyArmor.descriptionToken = DESCRIPTION_TOKEN;
                BodyArmor.loreToken = LORE_TOKEN;

                BodyArmor._itemTierDef = itemTier1Def;

                BodyArmor.tags = new ItemTag[]
                {
                    ItemTag.Utility
                };

                BodyArmor.pickupIconSprite = Resources.Load<Sprite>("Textures/MiscIcons/texMysteryIcon");
                BodyArmor.pickupModelPrefab = Resources.Load<GameObject>("Prefabs/PickupModels/PickupMystery");

                BodyArmor.canRemove = true;

                ItemDisplayRuleDict displayRules = new ItemDisplayRuleDict(null);
                ItemAPI.Add(new CustomItem(BodyArmor, displayRules));

                static void tryBlockWithBodyArmor(HealthComponent self, DamageInfo damageInfo)
                {
                    if (!NetworkServer.active)
                        return;

                    if (!self.alive || self.godMode)
                        return;

                    if (self.ospTimer > 0f)
                        return;

                    if (!self.body)
                        return;

                    Inventory inventory = self.body.inventory;
                    if (!inventory)
                        return;

                    if (inventory.GetItemCount(BodyArmor.itemIndex) > 0)
                    {
                        inventory.RemoveItem(BodyArmor.itemIndex);
                        inventory.GiveItem(BodyArmor_Consumed.itemIndex);
                        CharacterMasterNotificationQueue.SendTransformNotification(self.body.master, BodyArmor.itemIndex, BodyArmor_Consumed.itemIndex, CharacterMasterNotificationQueue.TransformationType.Default);

                        EffectData effectData = new EffectData
                        {
                            origin = damageInfo.position,
                            rotation = Util.QuaternionSafeLookRotation((damageInfo.force != Vector3.zero) ? damageInfo.force : UnityEngine.Random.onUnitSphere)
                        };
                        EffectManager.SpawnEffect(HealthComponent.AssetReferences.bearEffectPrefab, effectData, true);

                        damageInfo.rejected = true;
                    }
                }

                On.RoR2.HealthComponent.TakeDamage += static (orig, self, damageInfo) =>
                {
                    tryBlockWithBodyArmor(self, damageInfo);
                    orig(self, damageInfo);
                };

                _languageTokens.Add(NAME_TOKEN, "Body Armor");
                _languageTokens.Add(PICKUP_TOKEN, "Block a single source of damage. Consumed on use.");
                _languageTokens.Add(DESCRIPTION_TOKEN, "Blocks a single source of damage, consumed on use.");
                _languageTokens.Add(LORE_TOKEN, "The body armor was created to help protect people from harm. It is made of a strong, durable material that can withstand a lot of damage. The armor is meant to be worn by people who are in danger or who need to be protected. It is a lifesaver for many people.\r\n\r\nThe armor is not perfect, however. There have been cases where the armor has failed to protect people from harm. In some cases, people have even been killed while wearing the armor. Despite these failures, the body armor remains a vital part of many people's lives.\r\n\r\nThank you for choosing our body armor. We hope it will help keep you safe from harm.\r\n\r\nPlease note: This armor is not intended to be worn while pregnant.\r\n\r\nEditor's note: I'm not sure what you were trying to go for with this one, but it didn't quite make the cut.");
            }

            // Body Armor (Consumed)
            {
                const string ITEM_TOKEN = "BODY_ARMOR_CONSUMED";

                const string NAME_TOKEN = TOKEN_PREFIX + ITEM_TOKEN + NAME_TOKEN_SUFFIX;
                const string PICKUP_TOKEN = TOKEN_PREFIX + ITEM_TOKEN + PICKUP_TOKEN_SUFFIX;
                const string DESCRIPTION_TOKEN = TOKEN_PREFIX + ITEM_TOKEN + DESC_TOKEN_SUFFIX;
                const string LORE_TOKEN = TOKEN_PREFIX + ITEM_TOKEN + LORE_TOKEN_SUFFIX;

                BodyArmor_Consumed = ScriptableObject.CreateInstance<ItemDef>();
                BodyArmor_Consumed.name = $"{Main.PluginGUID}_{nameof(BodyArmor_Consumed)}";
                BodyArmor_Consumed.nameToken = NAME_TOKEN;
                BodyArmor_Consumed.pickupToken = PICKUP_TOKEN;
                BodyArmor_Consumed.descriptionToken = DESCRIPTION_TOKEN;
                BodyArmor_Consumed.loreToken = LORE_TOKEN;

                BodyArmor_Consumed._itemTierDef = null;
#pragma warning disable CS0618 // Type or member is obsolete
                BodyArmor_Consumed.deprecatedTier = ItemTier.NoTier;
#pragma warning restore CS0618 // Type or member is obsolete

                BodyArmor_Consumed.canRemove = false;

                BodyArmor_Consumed.pickupIconSprite = Resources.Load<Sprite>("Textures/MiscIcons/texMysteryIcon");
                BodyArmor_Consumed.pickupModelPrefab = Resources.Load<GameObject>("Prefabs/PickupModels/PickupMystery");

                ItemDisplayRuleDict displayRules = new ItemDisplayRuleDict(null);
                ItemAPI.Add(new CustomItem(BodyArmor_Consumed, displayRules));

                _languageTokens.Add(NAME_TOKEN, "Body Armor (Consumed)");
                _languageTokens.Add(PICKUP_TOKEN, "Broken body armor. Does nothing.");
            }

            // Eternity's Edge
            {
                const string ITEM_TOKEN = "ETERNITYS_EDGE";

                const string NAME_TOKEN = TOKEN_PREFIX + ITEM_TOKEN + NAME_TOKEN_SUFFIX;
                const string PICKUP_TOKEN = TOKEN_PREFIX + ITEM_TOKEN + PICKUP_TOKEN_SUFFIX;
                const string DESCRIPTION_TOKEN = TOKEN_PREFIX + ITEM_TOKEN + DESC_TOKEN_SUFFIX;
                const string LORE_TOKEN = TOKEN_PREFIX + ITEM_TOKEN + LORE_TOKEN_SUFFIX;

                EternitysEdge = ScriptableObject.CreateInstance<ItemDef>();
                EternitysEdge.name = $"{Main.PluginGUID}_{nameof(EternitysEdge)}";
                EternitysEdge.nameToken = NAME_TOKEN;
                EternitysEdge.pickupToken = PICKUP_TOKEN;
                EternitysEdge.descriptionToken = DESCRIPTION_TOKEN;
                EternitysEdge.loreToken = LORE_TOKEN;

                EternitysEdge._itemTierDef = itemTier3Def;

                EternitysEdge.tags = new ItemTag[]
                {
                    ItemTag.Damage
                };

                EternitysEdge.pickupIconSprite = Resources.Load<Sprite>("Textures/MiscIcons/texMysteryIcon");
                EternitysEdge.pickupModelPrefab = Resources.Load<GameObject>("Prefabs/PickupModels/PickupMystery");

                EternitysEdge.canRemove = true;

                ItemDisplayRuleDict displayRules = new ItemDisplayRuleDict(null);
                ItemAPI.Add(new CustomItem(EternitysEdge, displayRules));

                CharacterBody.onBodyInventoryChangedGlobal += static body =>
                {
                    if (!body)
                        return;

                    Inventory inventory = body.inventory;
                    if (!inventory)
                        return;

                    body.AddItemBehavior<EternitysEdgeBehavior>(inventory.GetItemCount(EternitysEdge.itemIndex));
                };

                RecalculateStatsAPI.GetStatCoefficients += static (sender, args) =>
                {
                    if (!sender)
                        return;

                    Inventory inventory = sender.inventory;
                    if (!inventory)
                        return;

                    int itemCount = inventory.GetItemCount(EternitysEdge);
                    if (itemCount <= 0)
                        return;

                    int boostCount = sender.GetBuffCount(BuffDefs.bdCritChanceAndDamageBoost);
                    if (boostCount <= 0)
                        return;

                    args.critAdd += (Mathf.Pow(1.05f, boostCount) - 1f) * 100f;
                    args.critDamageMultAdd += (0.2f + ((itemCount - 1) * 0.1f)) * boostCount;
                };

                _languageTokens.Add(NAME_TOKEN, "Eternity's Edge");
                _languageTokens.Add(PICKUP_TOKEN, "Increases critical chance and critical hit damage with each successful non-crit hit, resets when you land a crit.");
                _languageTokens.Add(DESCRIPTION_TOKEN, "Increases <style=cIsDamage>critical hit chance by 10%</style> and <style=cIsDamage>critical hit damage by 20% <style=cStack>(+10% per stack)</style></style> for every non-crit hit, landing a crit resets the critical hit chance and damage boosts.");
                _languageTokens.Add(LORE_TOKEN, "When longtime scientist and inventor Dr. Harold Andrews created Eternity's Edge, he had no idea of the mayhem and destruction it would cause. The experimental time machine was supposed to allow humans to travel through time for study and exploration, but instead it repeatedly sends people to random moments in history, with no way to return home. Andrews and his team spend their days and nights trying to fix the machine, but it seems to have a mind of its own.\r\n\r\nIn the meantime, the lucky (or unlucky) people who have been sent through time are stuck in strange and often dangerous situations. Some have even ended up in the past or future of other worlds, where they are unable to return to their own time. Andrews is determined to find a way to fix the machine, but he may never get the chance. Eternity's Edge seems determined to keep its hold on humanity, for better or worse.");
            }

            // Orb of Enlightenment
            {
                const string ITEM_TOKEN = "ORB_OF_ENLIGHTENMENT";

                const string NAME_TOKEN = TOKEN_PREFIX + ITEM_TOKEN + NAME_TOKEN_SUFFIX;
                const string PICKUP_TOKEN = TOKEN_PREFIX + ITEM_TOKEN + PICKUP_TOKEN_SUFFIX;
                const string DESCRIPTION_TOKEN = TOKEN_PREFIX + ITEM_TOKEN + DESC_TOKEN_SUFFIX;
                const string LORE_TOKEN = TOKEN_PREFIX + ITEM_TOKEN + LORE_TOKEN_SUFFIX;

                OrbOfEnlightenment = ScriptableObject.CreateInstance<ItemDef>();
                OrbOfEnlightenment.name = $"{Main.PluginGUID}_{nameof(OrbOfEnlightenment)}";
                OrbOfEnlightenment.nameToken = NAME_TOKEN;
                OrbOfEnlightenment.pickupToken = PICKUP_TOKEN;
                OrbOfEnlightenment.descriptionToken = DESCRIPTION_TOKEN;
                OrbOfEnlightenment.loreToken = LORE_TOKEN;

                OrbOfEnlightenment._itemTierDef = itemTier1Def;

                OrbOfEnlightenment.tags = new ItemTag[]
                {
                    ItemTag.Utility
                };

                OrbOfEnlightenment.pickupIconSprite = Resources.Load<Sprite>("Textures/MiscIcons/texMysteryIcon");
                OrbOfEnlightenment.pickupModelPrefab = Resources.Load<GameObject>("Prefabs/PickupModels/PickupMystery");

                OrbOfEnlightenment.canRemove = true;

                ItemDisplayRuleDict displayRules = new ItemDisplayRuleDict(null);
                ItemAPI.Add(new CustomItem(OrbOfEnlightenment, displayRules));

                On.RoR2.TeamManager.GiveTeamExperience += static (orig, self, teamIndex, experience) =>
                {
                    int itemCount = Util.GetItemCountForTeam(teamIndex, OrbOfEnlightenment.itemIndex, true);

#if DEBUG
                    Log.Debug($"GiveTeamExperience {nameof(teamIndex)}={teamIndex} {nameof(experience)}={experience} {nameof(itemCount)}={itemCount}");
#endif
                    
                    if (itemCount > 0)
                    {
#if DEBUG
                        ulong oldExp = experience;
#endif
                        experience = (ulong)(experience * (1f + (0.3f * itemCount)));

#if DEBUG
                        if (oldExp != experience)
                        {
                            Log.Debug($"changed exp gain for team {teamIndex}: {oldExp}->{experience}");
                        }
#endif
                    }

                    orig(self, teamIndex, experience);
                };

                _languageTokens.Add(NAME_TOKEN, "Orb of Enlightenment");
                _languageTokens.Add(PICKUP_TOKEN, "Increase experience gained by 30%");
                _languageTokens.Add(DESCRIPTION_TOKEN, "Increases experience gained by <style=cIsUtility>30% <style=cStack>(+30% per stack)</style></style>");
                _languageTokens.Add(LORE_TOKEN, "As soon as the Orb of Enlightenment was touched, the ancient runes on its surface began to glow. The light grew brighter and brighter until it filled the entire room. Then, just as suddenly as it had begun, the light faded and the room was dark again.\r\n\r\nWhen his eyes had adjusted, the first thing Ian saw was the box the orb had been sitting in. It was open now, and the orb was gone. Ian quickly looked around, but there was no sign of it. Then he heard a voice in his head.\r\n\r\n\"You have found the Orb of Enlightenment,\" the voice said. \"With this orb, you will be able to see the truth in all things.\"\r\n\r\nIan didn't know what to make of this. He had always been a skeptic, and he wasn't sure he wanted to see the truth in all things. But then he remembered how much his life had changed since he'd started using the orb. He had stopped drinking and smoking, and he was getting along better with his wife and kids. Maybe the orb could help him change other parts of his life that he wasn't happy with.\r\n\r\nSo Ian took a deep breath and decided to take the orb's advice. He would see the truth in all things, and he would change the parts of his life that he didn't like. He was ready to face whatever the orb had in store for him.");
            }

            // Immortal Soulstone
            {
                const string ITEM_TOKEN = "IMMORTAL_SOULSTONE";

                const string NAME_TOKEN = TOKEN_PREFIX + ITEM_TOKEN + NAME_TOKEN_SUFFIX;
                const string PICKUP_TOKEN = TOKEN_PREFIX + ITEM_TOKEN + PICKUP_TOKEN_SUFFIX;
                const string DESCRIPTION_TOKEN = TOKEN_PREFIX + ITEM_TOKEN + DESC_TOKEN_SUFFIX;
                const string LORE_TOKEN = TOKEN_PREFIX + ITEM_TOKEN + LORE_TOKEN_SUFFIX;

                ImmortalSoulstone = ScriptableObject.CreateInstance<ItemDef>();
                ImmortalSoulstone.name = $"{Main.PluginGUID}_{nameof(ImmortalSoulstone)}";
                ImmortalSoulstone.nameToken = NAME_TOKEN;
                ImmortalSoulstone.pickupToken = PICKUP_TOKEN;
                ImmortalSoulstone.descriptionToken = DESCRIPTION_TOKEN;
                ImmortalSoulstone.loreToken = LORE_TOKEN;

                ImmortalSoulstone._itemTierDef = itemTier3Def;

                ImmortalSoulstone.tags = new ItemTag[]
                {
                    ItemTag.Utility
                };

                ImmortalSoulstone.pickupIconSprite = Resources.Load<Sprite>("Textures/MiscIcons/texMysteryIcon");
                ImmortalSoulstone.pickupModelPrefab = Resources.Load<GameObject>("Prefabs/PickupModels/PickupMystery");

                ImmortalSoulstone.canRemove = true;

                ItemDisplayRuleDict displayRules = new ItemDisplayRuleDict(null);
                ItemAPI.Add(new CustomItem(ImmortalSoulstone, displayRules));

                ImmortalSoulstoneController.InitHooks();

                _languageTokens.Add(NAME_TOKEN, "Immortal Soulstone");
                _languageTokens.Add(PICKUP_TOKEN, "Double the duration of all buffs.");
                _languageTokens.Add(DESCRIPTION_TOKEN, "<style=cIsUtility>+100% <style=cStack>(+100% per stack)</style></style> duration to all buffs.");
                _languageTokens.Add(LORE_TOKEN, "The technician carefully disconnected the last fiber optic cable from the back of the server and placed it in the box. He closed the lid and carried the box to the loading dock, where his colleague was waiting with the truck.\r\n\r\n\"Another one down,\" the technician said.\r\n\r\n\"Damn, that thing was huge,\" his colleague said. \"What the hell do they make these things for?\"\r\n\r\n\"I have no idea. But whatever it is, it's gotta be important. They've been using these things for years and nobody knows what they do.\"\r\n\r\nThe two men loaded the box into the truck and drove away.\r\n\r\nAs they pulled out of the loading dock, the technician's phone rang.\r\n\r\n\"Hello?\" he said.\r\n\r\n\"We need you back at the lab,\" his boss said. \"There's been a change in plans.\"\r\n\r\nThe technician sighed.\r\n\r\n\"I'll be there in a few hours.\"\r\n\r\nWhen he arrived at the lab, his boss was waiting for him.\r\n\r\n\"We need you to take a look at this,\" his boss said, pointing to a monitor.\r\n\r\nThe technician looked at the monitor and saw a picture of the server he had just taken apart.\r\n\r\n\"What am I looking at?\" he asked.\r\n\r\n\"That's the Immortal Soulstone,\" his boss said. \"It's been stolen.\"");
            }

            LanguageAPI.Add(_languageTokens);
        }
    }
}
