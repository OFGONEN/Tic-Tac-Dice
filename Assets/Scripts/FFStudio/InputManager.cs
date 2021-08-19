/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using Lean.Touch;

namespace FFStudio
{
    public class InputManager : MonoBehaviour
    {
#region Fields
		[ Header( "Fired Events" ) ]
		public SwipeInputEvent swipeInputEvent;
		public IntGameEvent tapInputEvent;

		[ Header( "Shared Variables" ) ]
		public SharedReferenceProperty mainCamera_ReferenceProperty;
		public SharedVector2Property inputDirectionProperty;
		public SharedBoolProperty inputActiveProperty;

		// Privat fields
		private LeanFingerDelegate fingerDownMethod;
		private int swipeThreshold;
		private Vector2 inputOrigin;

		// Components
		private Transform mainCamera_Transform;
		private Camera mainCamera;
		private LeanTouch leanTouch;
#endregion

#region Unity API
		private void OnEnable()
		{
			mainCamera_ReferenceProperty.changeEvent += OnCameraReferenceChange;
		}

		private void OnDisable()
		{
			mainCamera_ReferenceProperty.changeEvent -= OnCameraReferenceChange;
		}

		private void Awake()
		{
			swipeThreshold = Screen.width * GameSettings.Instance.swipeThreshold / 100;

			leanTouch         = GetComponent<LeanTouch>();
			leanTouch.enabled = false;

			fingerDownMethod = FingerDown;
		}
#endregion
		
#region API
		public void Swiped( Vector2 delta )
		{
			swipeInputEvent.ReceiveInput( delta );
		}
		
		public void Tapped( int count )
		{
			tapInputEvent.eventValue = count;

			tapInputEvent.Raise();
		}

		public void LeanFingerUpdate( LeanFinger finger )
		{
			fingerDownMethod( finger );
		}

		public void LeanFingerUp( LeanFinger finger )
		{
			inputActiveProperty.SetValue( false );

			fingerDownMethod = FingerDown;
		}
#endregion

#region Implementation
		private void FingerDown( LeanFinger finger )
		{
			inputActiveProperty.SetValue( true );

			fingerDownMethod = FingerUpdate;
			inputOrigin      = finger.ScreenPosition;
		}

		private void FingerUpdate( LeanFinger finger )
		{
			var input = finger.ScreenPosition - inputOrigin;
			inputDirectionProperty.SetValue( input.normalized );
		}

		private void OnCameraReferenceChange()
		{
			var value = mainCamera_ReferenceProperty.sharedValue;

			if( value == null )
			{
				mainCamera_Transform = null;
				leanTouch.enabled = false;
			}
			else 
			{
				mainCamera_Transform = value as Transform;
				mainCamera           = mainCamera_Transform.GetComponent< Camera >();
				leanTouch.enabled    = true;
			}
		}
#endregion
    }
}