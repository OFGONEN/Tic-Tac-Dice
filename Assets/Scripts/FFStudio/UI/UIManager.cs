using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using NaughtyAttributes;

namespace FFStudio
{
    public class UIManager : MonoBehaviour
    {
        #region Fields

        [Header("Event Listeners")]
        public EventListenerDelegateResponse levelLoadedResponse;
        public EventListenerDelegateResponse levelCompleteResponse;
        public EventListenerDelegateResponse levelFailResponse;
        public EventListenerDelegateResponse tapInputListener;

        [Header("UI Elements")]
        public UILoadingBar levelLoadingBar;
		public UIText levelLoadingText;
		public UILoadingBar levelProgressBar;
        public UIText levelCountText;
        public UIText informationText;
        public Image loadingScreenImage;
        public Image foreGroundImage;
        public RectTransform tutorialObjects;

        [Header("Fired Events")]
        public GameEvent levelRevealedEvent;
		public GameEvent levelStartedEvent;
		public GameEvent loadNewLevelEvent;
        public GameEvent resetLevelEvent;
        public ElephantLevelEvent elephantLevelEvent;


        #endregion

        #region UnityAPI

        private void OnEnable()
        {
            levelLoadedResponse.OnEnable();
            levelFailResponse.OnEnable();
            levelCompleteResponse.OnEnable();
            tapInputListener.OnEnable();
        }

        private void OnDisable()
        {
            levelLoadedResponse.OnDisable();
            levelFailResponse.OnDisable();
            levelCompleteResponse.OnDisable();
            tapInputListener.OnDisable();
        }

        private void Awake()
        {
            levelLoadedResponse.response = LevelLoadedResponse;
            levelFailResponse.response = LevelFailResponse;
            levelCompleteResponse.response = LevelCompleteResponse;
            tapInputListener.response = ExtensionMethods.EmptyMethod;

            informationText.textRenderer.text = "Tap to Start";
        }
        #endregion

        #region Implementation

        void LevelLoadedResponse()
        {
            var sequance = DOTween.Sequence();

            sequance.Append(levelLoadingBar.GoPopIn());
            sequance.Append(levelLoadingText.GoPopIn());
            sequance.Append(loadingScreenImage.DOFade(0, GameSettings.Instance.ui_Entity_Fade_TweenDuration)); 
            sequance.AppendCallback(() => tapInputListener.response = StartLevel);

            levelCountText.textRenderer.text = "Level " + CurrentLevelData.Instance.currentConsecutiveLevel;

            levelLoadedResponse.response = NewLevelLoaded;
        }

        [Button]
        void LevelCompleteResponse()
        {
            var sequence = DOTween.Sequence();

            Tween tween = null;

            informationText.textRenderer.text = "Completed \n\n Tap to Continue";

            sequence.Append(tween); //TODO: UIElements tween
            sequence.Append(foreGroundImage.DOFade(0.5f, GameSettings.Instance.ui_Entity_Fade_TweenDuration));
            sequence.Append(informationText.GoPopOut());
            sequence.AppendCallback(() => tapInputListener.response = LoadNewLevel);

            elephantLevelEvent.level = CurrentLevelData.Instance.currentConsecutiveLevel;
            elephantLevelEvent.elephantEventType = ElephantEvent.LevelCompleted;
            elephantLevelEvent.Raise();
        }

        [Button]
        void LevelFailResponse()
        {
            var sequence = DOTween.Sequence();

            Tween tween = null;

            informationText.textRenderer.text = "Level Failed \n\n Tap to Continue";

            sequence.Append(tween); //TODO: UIElements tween
            sequence.Append(foreGroundImage.DOFade(0.5f, GameSettings.Instance.ui_Entity_Fade_TweenDuration));
            sequence.Append(informationText.GoPopOut());
            sequence.AppendCallback(() => tapInputListener.response = Resetlevel);

            elephantLevelEvent.level = CurrentLevelData.Instance.currentConsecutiveLevel;
            elephantLevelEvent.elephantEventType = ElephantEvent.LevelFailed;
            elephantLevelEvent.Raise();
        }

        void NewLevelLoaded()
        {
            levelCountText.textRenderer.text = "Level " + CurrentLevelData.Instance.currentConsecutiveLevel;

            var sequence = DOTween.Sequence();

            Tween tween = null;

            sequence.Append(foreGroundImage.DOFade(0, GameSettings.Instance.ui_Entity_Fade_TweenDuration));
            sequence.Append(tween); //TODO: UIElements tween
            sequence.AppendCallback(levelRevealedEvent.Raise);
			sequence.AppendCallback( levelStartedEvent.Raise );

			elephantLevelEvent.level = CurrentLevelData.Instance.currentConsecutiveLevel;
            elephantLevelEvent.elephantEventType = ElephantEvent.LevelStarted;
            elephantLevelEvent.Raise();
        }

        void StartLevel()
        {
			var sequence = DOTween.Sequence();

			sequence.Append( foreGroundImage.DOFade( 0, GameSettings.Instance.ui_Entity_Fade_TweenDuration ) );
			sequence.Join( informationText.GoPopIn() );
			sequence.AppendCallback( levelRevealedEvent.Raise );
			sequence.AppendCallback( levelStartedEvent.Raise );

			tutorialObjects.gameObject.SetActive(false);

            tapInputListener.response = ExtensionMethods.EmptyMethod;

            elephantLevelEvent.level = CurrentLevelData.Instance.currentConsecutiveLevel;
            elephantLevelEvent.elephantEventType = ElephantEvent.LevelStarted;
            elephantLevelEvent.Raise();
        }

        void LoadNewLevel()
        {
            FFLogger.Log("Load New Level");
            tapInputListener.response = ExtensionMethods.EmptyMethod;

            var sequence = DOTween.Sequence();

            sequence.Append(foreGroundImage.DOFade(1f, GameSettings.Instance.ui_Entity_Fade_TweenDuration));
            sequence.Join(informationText.GoPopIn());
            sequence.AppendCallback(loadNewLevelEvent.Raise);
        }

        void Resetlevel()
        {
            FFLogger.Log("Reset Level");
            tapInputListener.response = ExtensionMethods.EmptyMethod;

            var sequence = DOTween.Sequence();

            sequence.Append(foreGroundImage.DOFade(1f, GameSettings.Instance.ui_Entity_Fade_TweenDuration));
            sequence.Join(informationText.GoPopIn());
            sequence.AppendCallback(resetLevelEvent.Raise);

            elephantLevelEvent.level = CurrentLevelData.Instance.currentConsecutiveLevel;
            elephantLevelEvent.elephantEventType = ElephantEvent.LevelStarted;
            elephantLevelEvent.Raise();
        }


        #endregion
    }
}