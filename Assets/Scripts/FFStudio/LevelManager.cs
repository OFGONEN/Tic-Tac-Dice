/* Created by and for usage of FF Studios (2021). */

using UnityEngine;

namespace FFStudio
{
    public class LevelManager : MonoBehaviour
    {
#region Fields
        [ Header("Event Listeners" ) ]
        public EventListenerDelegateResponse levelLoadedListener;
        public EventListenerDelegateResponse levelRevealedListener;
        public EventListenerDelegateResponse levelStartedListener;
		public EventListenerDelegateResponse diceTriggerdNetListener; // ReferenceGameEvent

		[ Header("Fired Events" ) ]
        public GameEvent levelFailedEvent;
        public GameEvent levelCompleted;

        [ Header("Level Releated" ) ]
        public SharedFloatProperty levelProgress;
#endregion

#region UnityAPI
        private void OnEnable()
        {
            // Level Releated
            levelLoadedListener  .OnEnable();
            levelRevealedListener.OnEnable();
            levelStartedListener .OnEnable();

			// Game Releated
			diceTriggerdNetListener.OnEnable();
		}

        private void OnDisable()
        {
            // Level Releated
            levelLoadedListener  .OnDisable();
            levelRevealedListener.OnDisable();
            levelStartedListener .OnDisable();

			// Game Releated
			diceTriggerdNetListener.OnDisable();
        }

        private void Awake()
        {
            levelLoadedListener.response   = LevelLoadedResponse;
            levelRevealedListener.response = LevelRevealedResponse;
            levelStartedListener.response  = LevelStartedResponse;

			diceTriggerdNetListener.response = DiceTriggedNetResponse;
		}
#endregion

#region Implementation
        void LevelLoadedResponse()
        {
            levelProgress.SetValue(0);
        }

        void LevelRevealedResponse()
        {
        }

        void LevelStartedResponse()
        {
        }

        // Return the dice to Default when it triggered the Net
        private void DiceTriggedNetResponse()
        {
			var dice = ( ( diceTriggerdNetListener.gameEvent as ReferenceGameEvent ).eventValue as Collider ).GetComponentInParent< Dice >();
			dice.ReturnDefault();
		}
#endregion
    }
}