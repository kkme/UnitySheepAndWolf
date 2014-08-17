using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtensionsUnityVectors;

public class AniMover : MonoBehaviour
{
	delegate void UDPATE_(float ratio);
	UDPATE_ UpdateCalls = delegate { };

	bool	isMoving = false,
			isRotating = false,
			isSwinging = false;
	Vector3 angleFrom , angleTo, angleDis;
	Vector3 moveFrom, moveTo,moveDis;
	Vector3 swingFrom,swingDis;
	float timeElapsed = 0;
	internal float timElapsedMax = 0.13f;

	void init()
	{
		timeElapsed = 0;
	}
	public void rotate(int dir) // I want to have clockwise
	{
		timeElapsed = 0;
		enabled = true;
		if (!isRotating) UpdateCalls += updateRotate;
		isRotating = true;
		angleFrom = transform.rotation.eulerAngles;
		float
			temp = 360 + dir * -90,
			A = temp - angleFrom.z, B = (angleFrom.z   + 360 -temp)*-1,
			C = (Mathf.Abs(A) < Mathf.Abs(B)) ? A : B;

		angleDis = new Vector3(0,0, C);
		//Debug.Log("anglefrom "   + angleFrom + " "  +angleTo + " " +angleDis);
	}
	public void move(Vector2 v){move((int)v.x, (int)v.y);}
	public void move(float x, float y)
	{
		timeElapsed = 0;
		enabled = true;

		if (!isMoving) UpdateCalls += updateMove;
		isMoving = true;
		moveFrom = transform.position;
		swingFrom = transform.position;
		moveTo = new Vector3(x, y, moveFrom.z);
		moveDis = moveTo - moveFrom;
	}
	public void swing(float magnitudeX, float magnitudeY)
	{
		timeElapsed = 0;
		enabled = true;
		if (!isSwinging)
		{
			UpdateCalls += updateSwing;
			swingFrom = transform.position;
		}
		isSwinging = true;
		swingDis = new Vector3( magnitudeX, magnitudeY, 0);
	}
	//update calls
	void updateMove(float ratio)
	{
		transform.position = moveFrom + moveDis * ratio;
		swingFrom = transform.position;
	}
	void updateRotate(float ratio)
	{
		transform.rotation = Quaternion.Euler(angleFrom + angleDis * ratio);
	}
	void updateSwing(float ratio)
	{
		float ratioNew = Mathf.Sin(ratio * 3.14f);
		transform.position = swingFrom + swingDis * ratioNew;
	}
	public void Update()
	{
		timeElapsed += Mathf.Min( Time.deltaTime, .025f);
		float ratio = Mathf.Min(1, timeElapsed / timElapsedMax);
		//transform.position = moveFrom + moveDis * ratio;
		UpdateCalls(ratio);
		if (ratio >= 1.0f)
		{
			isMoving = false;
			isRotating = false;
			isSwinging = false;
			enabled = false;
			UpdateCalls = delegate { };
		}
		//Debug.Log("updating " + timeElapsed + " " + timElapsedMax + " " + isMoving + " " + isRotating + "IS ENABLED " + (!isMoving && !isRotating));
	}

}