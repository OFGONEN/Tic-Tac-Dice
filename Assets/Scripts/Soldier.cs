/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
#region Fields
#endregion

#region Unity API
#endregion

#region API
	public void Spawn( Vector3 localPosition, Quaternion localRotation )
	{
		gameObject.SetActive( true );

		transform.localPosition = localPosition;
		transform.localRotation = localRotation;
	}
#endregion

#region Implementation
#endregion
}
