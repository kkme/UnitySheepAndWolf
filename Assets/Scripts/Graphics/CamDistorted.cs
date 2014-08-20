using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class CamDistorted : MonoBehaviour
{
	float zoom = 0;
	AniMover ani;
	public float x,y , w,h;
	void Awake()
	{
		float xRate = 1 / (8.0f * Camera.main.aspect);
		float YRate = 1 / 8.0f;
		camera.rect = new Rect(
			(.5f+-.5f * xRate) + x * xRate,
			3.5f * YRate + y * YRate,
			w * xRate, h * YRate);
		optimizeRect();
		//8 ratioW? < what is this fuck how many r there  
		
		ani = gameObject.AddComponent<AniMover>();
		gameObject.SetActive(false);
	}
	void Start()
	{
		fixView();
	}
	Rect r = new Rect(0, 0, 1, 1);
	void optimizeRect()
	{
		var x = Mathf.Max(0, camera.rect.x);
		var y = Mathf.Max(0, camera.rect.y);
		var w = Mathf.Min(camera.rect.width, 1 - camera.rect.x);
		var h = Mathf.Min(camera.rect.height, 1 - camera.rect.y);
		camera.rect = new Rect(x, y, w,h);
	}
	public void swing(float dirX, float dirY,float duration = 1.1f)
	{
		transform.localPosition = Vector3.zero;
		fixView();
		var meHere = new Vector2(transform.position.x, transform.position.y);
		transform.position += new Vector3(dirX, dirY,0);
		ani.move(meHere.x, meHere.y);
		ani.timElapsedMax = duration;
		//ani.swing(dirX, dirY);
		gameObject.SetActive(true);
	}
	public void fixView()
	{
		optimizeRect();
		camera.orthographicSize = WorldInfo.camGame.camera.orthographicSize * camera.rect.height;
		float newY = 
			((.5f - camera.rect.height * .5f) - camera.rect.y)
			* WorldInfo.camGame.camera.orthographicSize * -2.0f;
		float newX =
			(Camera.main.aspect * WorldInfo.camGame.camera.orthographicSize) 
			* (camera.rect.x * 2.0f - (1 - camera.rect.width));
		transform.localPosition= new Vector3(newX, newY, -14);
	}
	void Update()
	{
		
		if (!ani.enabled) gameObject.SetActive(false);
		transform.position = WorldInfo.camGame.transform.position;
		//fixView();
		//ani.swingFrom = transform.localPosition;
		//fixView();
		
	}
}
