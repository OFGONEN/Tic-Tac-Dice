/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FFStudio;

public class UIUserInfo : UIEntity
{
#region Fields
    [ Header( "EventListeners" ) ]
    public EventListenerDelegateResponse levelLoadedListener;

    [ Header( "Setup" ) ]
    public Image userFlag;
    public TextMeshProUGUI userName;
    public UserInfoLibrary userInfoLibrary;
#endregion

#region Properties
#endregion

#region Unity API
    private void OnEnable()
    {
		levelLoadedListener.OnEnable();
	}

    private void OnDisable()
    {
		levelLoadedListener.OnDisable();
	}

    private void Awake()
    {
		levelLoadedListener.response = LevelLoadedResponse;
	}
#endregion

#region API
#endregion

#region Implementation
    private void LevelLoadedResponse()
    {
		var randomIndex = Random.Range( 0, userInfoLibrary.userInfos.Length );

        var userInfo = userInfoLibrary.userInfos[ randomIndex ];

		userName.text = userInfo.userName;
		userFlag.sprite = userInfo.userFlag;
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
