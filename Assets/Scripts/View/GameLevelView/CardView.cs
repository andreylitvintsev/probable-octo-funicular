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
    public class CardView : AbstractView<GameLogicService.Card>
    {
        [SerializeField] private TMP_Text _flipText = null;
        [SerializeField] private Button _button = null;
        [SerializeField] private GameLevelView _gameLevelView = null;
        [Space]
        [SerializeField] private PlayableDirector _playableDirector = null;
        [SerializeField] private PlayableAsset _cardFlipToBackTimeline = null;
        [SerializeField] private PlayableAsset _cardFlipToFrontTimeline = null;

        public bool IsAnimated => _playableDirector.playableAsset != null &&
                                  _playableDirector.state == PlayState.Playing &&
                                  _playableDirector.duration > _playableDirector.time;
        
        private bool _isFlipped = false;
        public bool IsFlipped => _isFlipped;
        
        protected override void OnInitialize(GameLogicService.Card data)
        {
            SetInitialVisualState();
            _button.SubscribeOnClick(() =>
            {
                if (IsAnimated || _isFlipped)
                    return;
                PlayFlipToFrontAnimation();
                _gameLevelView.OnCardViewTouched(this);
                StartCoroutine(WaitAnimationFinish(() =>
                {
                    _isFlipped = !_isFlipped;
                    _gameLevelView.OnCheckMatch();
                }));
            }).DisposeWith(this);
        }

        protected override void OnDeinitialize() {}

        private void SetInitialVisualState()
        {
            _playableDirector.playableAsset = _cardFlipToFrontTimeline;
            _playableDirector.time = 0;
            _playableDirector.Evaluate();
            
            _flipText.text = Data.Symbol.ToString(NumberFormatInfo.InvariantInfo);
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
            Data.MarkAsMatched();
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