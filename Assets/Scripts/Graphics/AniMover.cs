using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtensionsUnityVectors;

class AniMover : MonoBehaviour
{
	delegate void UDPATE_(float ratio);
	UDPATE_ UpdateCalls = delegate { };

	bool isMoving = false,isRotating = false;
	Vector3 angleFrom , angleTo, angleDis;
	Vector3 moveFrom, moveTo,moveDis;
	float timeElapsed = 0, timElapsedMax = 0.15f;

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
		moveTo = new Vector3(x, y, moveFrom.z);
		moveDis = moveTo - moveFrom;
	}
	public void updateMove(float ratio)
	{
		transform.position = moveFrom + moveDis * ratio;
	}
	void updateRotate(float ratio)
	{
		transform.rotation = Quaternion.Euler(angleFrom + angleDis * ratio);
	}
	
	public void Update()
	{
		timeElapsed += Mathf.Min( Time.deltaTime, .016f);
		float ratio = Mathf.Min(1, timeElapsed / timElapsedMax);
		//transform.position = moveFrom + moveDis * ratio;
		
		UpdateCalls(ratio);
		enabled = ratio < 1.0f;
		//Debug.Log("updating " + timeElapsed + " " + timElapsedMax + " " + isMoving + " " + isRotating + "IS ENABLED " + (!isMoving && !isRotating));
	}

}