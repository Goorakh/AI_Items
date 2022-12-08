using AI_Items.Content;
using AI_Items.Patches;
using BepInEx;
using R2API;
using R2API.Utils;
using RoR2;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

[assembly: HG.Reflection.SearchableAttribute.OptIn]

namespace AI_Items
{
    [BepInDependency(R2API.R2API.PluginGUID)]
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [R2APISubmoduleDependency(nameof(ItemAPI), nameof(LanguageAPI), nameof(RecalculateStatsAPI))]
    public class Main : BaseUnityPlugin
    {
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "Gorakh";
        public const string PluginName = "AI_Items";
        public const string PluginVersion = "1.0.0";

        void Awake()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Log.Init(Logger);

            ItemDefs.Setup();
            EquipmentDefs.Setup();
            BuffDefs.Setup();

            HealthComponentTakeDamageHook.Init();
            OnHitEnemyHook.Init();

            ContentPackHandler.Initialize();

            Log.Info($"Initialized in {stopwatch.Elapsed.TotalSeconds:F2} seconds");
        }

#if DEBUG
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                string log = string.Empty;

                List<object> availableItems = ItemCatalog.allItemDefs.Where(id =>
                {
                    return id && Language.GetString(id.nameToken) != id.nameToken && Language.GetString(id.pickupToken) != id.pickupToken && Language.GetString(id.descriptionToken) != id.descriptionToken;
                }).Cast<object>().Concat(EquipmentCatalog.equipmentDefs.Where(ed =>
                {
                    return ed && Language.GetString(ed.nameToken) != ed.nameToken && Language.GetString(ed.pickupToken) != ed.pickupToken && Language.GetString(ed.descriptionToken) != ed.descriptionToken;
                })).ToList();

                for (int i = 0; i < 10; i++)
                {
                    int itemIndex = UnityEngine.Random.Range(0, availableItems.Count);
                    object item = availableItems[itemIndex];
                    availableItems.RemoveAt(itemIndex);

                    log += $"{Language.GetString(item.GetFieldValue<string>("nameToken"))}: {Language.GetString(item.GetFieldValue<string>("pickupToken"))}\n";
                }

                Log.Debug(log);
            }
        }
#endif
    }
}
