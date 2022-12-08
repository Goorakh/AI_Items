using RoR2;
using RoR2.ContentManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AI_Items.Content
{
    internal class ContentPackHandler : IContentPackProvider
    {
        static readonly ContentPackHandler _instance = new ContentPackHandler();
        static readonly ContentPack _contentPack = new ContentPack();

        static readonly List<BuffDef> _buffs = new List<BuffDef>();

        internal static void Initialize()
        {
            ContentManager.collectContentPackProviders += ContentManager_collectContentPackProviders;
        }

        static void ContentManager_collectContentPackProviders(ContentManager.AddContentPackProviderDelegate addContentPackProvider)
        {
            addContentPackProvider(_instance);
        }

        public static void Register(BuffDef buff)
        {
            _buffs.Add(buff);
        }

        public string identifier => Main.PluginGUID;

        public IEnumerator FinalizeAsync(FinalizeAsyncArgs args)
        {
            args.ReportProgress(1f);
            yield break;
        }

        public IEnumerator GenerateContentPackAsync(GetContentPackAsyncArgs args)
        {
            ContentPack.Copy(_contentPack, args.output);
            yield break;
        }

        public IEnumerator LoadStaticContentAsync(LoadStaticContentAsyncArgs args)
        {
            _contentPack.buffDefs.Add(_buffs.ToArray());

            args.ReportProgress(1f);

            yield break;
        }
    }
}
