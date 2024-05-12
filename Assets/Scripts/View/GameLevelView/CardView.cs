using System;
using System.Collections;
using System.Globalization;
using ReactiveProperty;
using Service;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace View
{
    public class CardView : AbstractView<CardView.CardViewData>
    {
        public struct CardViewData
        {
            public GameLogicService.Card Card;
            public ServiceProvider ServiceProvider;
        }
        
        [SerializeField] private TMP_Text _flipText = null;
        [SerializeField] private Button _button = null;
        [SerializeField] private GameLevelView _gameLevelView = null;
        [Space]
        [SerializeField] private PlayableDirector _playableDirector = null;
        [SerializeField] private PlayableAsset _cardFlipToBackTimeline = null;
        [SerializeField] private PlayableAsset _cardFlipToFrontTimeline = null;

        private SoundsPlayerService _soundsPlayerService = null;

        public bool IsAnimated => _playableDirector.playableAsset != null &&
                                  _playableDirector.state == PlayState.Playing &&
                                  _playableDirector.duration > _playableDirector.time;
        
        private bool _isFlipped = false;
        public bool IsFlipped => _isFlipped;
        
        protected override void OnInitialize(CardView.CardViewData data)
        {
            _soundsPlayerService = data.ServiceProvider.GetService<SoundsPlayerService>();
            
            SetInitialVisualState();
            _button.SubscribeOnClick(OnClick).DisposeWith(this);
        }

        protected override void OnDeinitialize() {}

        private void OnClick()
        {
            if (IsAnimated || _isFlipped)
                return;
            
            _soundsPlayerService.PlayOneShot("CardFlip");
            
            PlayFlipToFrontAnimation();
            _gameLevelView.OnCardViewTouched(this);
            StartCoroutine(WaitAnimationFinish(() =>
            {
                _isFlipped = !_isFlipped;
                _gameLevelView.OnCheckMatch();
            }));
        }

        private void SetInitialVisualState()
        {
            _isFlipped = Data.Card.IsMatched;
            
            _playableDirector.playableAsset = Data.Card.IsMatched ? _cardFlipToBackTimeline : _cardFlipToFrontTimeline;
            _playableDirector.time = 0;
            _playableDirector.Evaluate();
            
            _flipText.text = Data.Card.Symbol.ToString(NumberFormatInfo.InvariantInfo);
        }
        
        private void PlayFlipToBackAnimation()
        {
            _playableDirector.playableAsset = _cardFlipToBackTimeline;
            _playableDirector.Play();
        }
        
        private void PlayFlipToFrontAnimation()
        {
            _playableDirector.playableAsset = _cardFlipToFrontTimeline;
            _playableDirector.Play();
        }
        
        private IEnumerator WaitAnimationFinish(Action onFinish)
        {
            yield return new WaitUntil(() => !IsAnimated);
            onFinish.Invoke();
        }
        
        public void OnMatch()
        {
            Data.Card.MarkAsMatched();
        }

        public void OnMissMatch()
        {
            PlayFlipToBackAnimation();
            StartCoroutine(WaitAnimationFinish(() =>
            {
                _isFlipped = !_isFlipped;
            }));
        }
    }
}