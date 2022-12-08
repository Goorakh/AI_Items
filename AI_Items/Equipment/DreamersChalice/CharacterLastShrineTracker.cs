using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AI_Items.Equipment.DreamersChalice
{
    public sealed class CharacterLastShrineTracker : MonoBehaviour
    {
        internal static void AddListeners()
        {
            GlobalEventManager.OnInteractionsGlobal += onInteractionGlobal;
            SceneCatalog.onMostRecentSceneDefChanged += onSceneLoad;
        }

        Vector3 _lastShrineUsePosition;

        void OnEnable()
        {
            InstanceTracker.Add(this);
        }

        void OnDisable()
        {
            InstanceTracker.Remove(this);
        }

        static void onInteractionGlobal(Interactor interactor, IInteractable interactable, GameObject interactableObject)
        {
            if (!interactor || !interactableObject)
                return;

            if (interactableObject.TryGetComponent(out PurchaseInteraction purchaseInteraction) && purchaseInteraction.isShrine)
            {
                CharacterLastShrineTracker shrineTrackerComponent = interactor.GetComponent<CharacterLastShrineTracker>();
                if (!shrineTrackerComponent)
                {
                    shrineTrackerComponent = interactor.gameObject.AddComponent<CharacterLastShrineTracker>();
                }

                Vector3 position = interactor.transform.position;
                if (interactor.TryGetComponent(out CharacterBody interactorBody))
                {
                    position = interactorBody.footPosition;
                }

                shrineTrackerComponent._lastShrineUsePosition = position;
            }
        }

        static void onSceneLoad(SceneDef _)
        {
            List<CharacterLastShrineTracker> instances = InstanceTracker.GetInstancesList<CharacterLastShrineTracker>();
            while (instances.Count > 0)
            {
                Destroy(instances[0]);
            }
        }

        public static bool TryTeleportToMostRecentlyUsedShrine(CharacterBody body)
        {
            if (body && body.TryGetComponent(out CharacterLastShrineTracker shrineTracker))
            {
                TeleportHelper.TeleportBody(body, shrineTracker._lastShrineUsePosition);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
