using RoR2;
using System;
using System.Collections.Generic;
using System.Text;

namespace AI_Items.Items.IcyWind
{
    public sealed class IcyWindBehavior : CharacterBody.ItemBehavior
    {
        HealthComponent _healthComponent;
        bool _lastUpdateWasOverHalfHealth;

        void Awake()
        {
            enabled = false;
        }

        void OnEnable()
        {
            if (body)
            {
                _healthComponent = body.healthComponent;

                // Invert to force a stats update next FixedUpdate
                _lastUpdateWasOverHalfHealth = !(_healthComponent.combinedHealthFraction >= 0.5f);
            }
        }

        void OnDisable()
        {
            _healthComponent = null;
        }

        void FixedUpdate()
        {
            const string LOG_PREFIX = $"{nameof(IcyWindBehavior)}.{nameof(FixedUpdate)} ";

            if (!body || !_healthComponent)
                return;
            
            bool overHalfHealth = _healthComponent.combinedHealthFraction >= 0.5f;
            if (_lastUpdateWasOverHalfHealth != overHalfHealth)
            {
#if DEBUG
                Log.Debug(LOG_PREFIX + $"stats marked dirty, {nameof(overHalfHealth)}={overHalfHealth}");
#endif

                body.MarkAllStatsDirty();
                _lastUpdateWasOverHalfHealth = overHalfHealth;
            }
        }
    }
}
