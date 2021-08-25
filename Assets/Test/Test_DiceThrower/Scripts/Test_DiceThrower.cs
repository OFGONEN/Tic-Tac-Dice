/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using FFStudio;
using NaughtyAttributes;

public class Test_DiceThrower : MonoBehaviour
{
#region Fields
	[ BoxGroup( "Setup" ) ] public DicePool allyDicePool;
	[ BoxGroup( "Setup" ) ] public DiceThrower diceThrower;

	[ BoxGroup( "Level Start Test" ) ] public GameEvent levelStart;
	[ BoxGroup( "Update Target Position Test" ), InfoBox( "Move Target Point transform using gizmos!" ) ] public Transform targetPoint;
	[ BoxGroup( "Update Target Position Test" ) ] public Transform closestPoint;

	//Private Fields
	private UnityMessage updateMethod;
#endregion

#region Unity API
	private void Awake()
	{
		allyDicePool.InitPool( transform, false );

		updateMethod = ExtensionMethods.EmptyMethod;
	}

	private void Update()
	{
		updateMethod();
	}
#endregion

#region API
	[ Button( "#Test1: Level Started" )  ]
	public void LevelStart()
	{
		levelStart.Raise();
	}

	[ Button( "#Test2: Update Target Point" ) ]
	public void UpdateTargetPointTest()
	{
		levelStart.Raise();

		targetPoint.position = closestPoint.position;
		diceThrower.EnableTrajectoryLine();
		UpdateTargetPoint();

		updateMethod = UpdateTargetPoint;
	}


	[ Button( "#Test3: Launch" ) ]
	public void Launch()
	{
		updateMethod = ExtensionMethods.EmptyMethod;

		diceThrower.Launch();
	}
#endregion

#region Implementation

	private void UpdateTargetPoint()
	{
		diceThrower.UpdateTargetPoint( targetPoint.position );
	}
#endregion
}
