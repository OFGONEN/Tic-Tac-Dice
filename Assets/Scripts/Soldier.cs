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
	public void Spawn( Vector3 position, Quaternion rotation )
	{
		gameObject.SetActive( true );

		transform.position = position;
		transform.rotation = rotation;
	}
#endregion

#region Implementation
#endregion
}
