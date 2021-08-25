/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FFStudio
{
	public class AppManager : MonoBehaviour
	{
#region Fields
		[Header( "Event Listeners" )]
		public EventListenerDelegateResponse loadNewLevelListener;
		public EventListenerDelegateResponse resetLevelListener;

		[Header( "Fired Events" )]
		public GameEvent levelLoaded;
		public GameEvent cleanUpEvent;

		[Header( "Fired Events" )]
		public SharedFloatProperty levelProgress;

#endregion

#region Unity API
		private void OnEnable()
		{
			loadNewLevelListener.OnEnable();
			resetLevelListener  .OnEnable();
		}
		private void OnDisable()
		{
			loadNewLevelListener.OnDisable();
			resetLevelListener  .OnDisable();

		}
		private void Awake()
		{
			loadNewLevelListener.response = LoadNewLevel;
			resetLevelListener.response   = ResetLevel;
		}

		private void Start()
		{
			StartCoroutine( LoadLevel() );
		}
#endregion

#region API
#endregion

#region Implementation
		private void ResetLevel()
		{
			var _operation = SceneManager.UnloadSceneAsync( CurrentLevelData.Instance.levelData.sceneIndex );
			_operation.completed += ( AsyncOperation operation ) => StartCoroutine( LoadLevel() );
		}

		private IEnumerator LoadLevel()
		{
			CurrentLevelData.Instance.currentLevel = PlayerPrefs.GetInt( "Level", 1 );
			CurrentLevelData.Instance.currentConsecutiveLevel = PlayerPrefs.GetInt( "Consecutive Level", 1 );

			CurrentLevelData.Instance.LoadCurrentLevelData();

			cleanUpEvent.Raise();
			// SceneManager.LoadScene( CurrentLevelData.Instance.levelData.sceneIndex, LoadSceneMode.Additive );
			var operation = SceneManager.LoadSceneAsync( CurrentLevelData.Instance.levelData.sceneIndex, LoadSceneMode.Additive );

			levelProgress.SetValue( 0 );

			while( !operation.isDone )
			{
				yield return null;

				levelProgress.SetValue( operation.progress );
			}

			levelLoaded.Raise();
		}
		private void LoadNewLevel()
		{
			CurrentLevelData.Instance.currentLevel++;
			CurrentLevelData.Instance.currentConsecutiveLevel++;
			PlayerPrefs.SetInt( "Level", CurrentLevelData.Instance.currentLevel );
			PlayerPrefs.SetInt( "Consecutive Level", CurrentLevelData.Instance.currentConsecutiveLevel );

			var _operation = SceneManager.UnloadSceneAsync( CurrentLevelData.Instance.levelData.sceneIndex );
			_operation.completed += ( AsyncOperation operation ) => StartCoroutine( LoadLevel() );
		}
#endregion
	}
}