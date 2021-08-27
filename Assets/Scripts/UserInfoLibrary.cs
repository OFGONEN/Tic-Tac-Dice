/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[ CreateAssetMenu( fileName = "GameEvent", menuName = "FF/Game/UserInfoLibrary" ) ]
public class UserInfoLibrary : ScriptableObject
{
    public UserInfo[] userInfos;
}


[ System.Serializable ]
public struct UserInfo
{
	public string userName;
	public Sprite userFlag;
}