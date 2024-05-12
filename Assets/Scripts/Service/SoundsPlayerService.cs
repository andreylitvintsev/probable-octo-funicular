using System;
using System.Collections.Generic;
using UnityEngine;

namespace Service
{
    public class SoundsPlayerService : AbstractService
    {
        [Serializable]
        public struct AudioClipSettings
        {
            [field: SerializeField] public string Id { get; private set; }
            [field: SerializeField] public AudioClip AudioClip { get; private set; }
        }
        
        [SerializeField] private List<AudioClipSettings> _settingsList = null;
        [SerializeField] private AudioSource _audioSource = null;
        private Dictionary<string, AudioClipSettings> _settingsLookup = new();

        protected override void OnInitialize()
        {
            foreach (var settings in _settingsList)
            {
                _settingsLookup[settings.Id] = settings;
            }
        }

        protected override void OnDeinitialize() {}

        public void PlayOneShot(string soundId)
        {
            if (_settingsLookup.TryGetValue(soundId, out var settings))
                _audioSource.PlayOneShot(settings.AudioClip);
        }
    }
}