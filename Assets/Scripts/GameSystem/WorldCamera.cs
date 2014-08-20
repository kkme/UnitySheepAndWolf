using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class WorldCamera : MonoBehaviour
{ 
	AniMover mover;
	float orthographicSize;
	float timeElapsed = 0, timeElapsedMax = 3.0f;
	void Awake()
	{
		UnitPlayer.EVENT_MOVED += move;
		UnitPlayer.EVENT_SWAPPING  += move;
		UnitPlayer.EVENT_CREATED += delegate(int x, int y) { transform.position = new Vector3(x, y, transform.position.z); };
		WorldInfo.camGame = this;
		mover = gameObject.AddComponent<AniMover>();
		mover.timElapsedMax *= 1.5f;
		orthographicSize = camera.orthographicSize;
		enabled = false;
	}
	void Start()
	{
		
	}
	public void move(int x, int y)
	{
		mover.move(x, y);
	}
	public Vector3 ScreenToWorldPoint(Vector3 pos)
	{
		return camera.ScreenToWorldPoint(pos);
	}
	public void effectZoom()
	{
		timeElapsed = 0;
		camera.orthographicSize = 0.6f;
		enabled = true;
	}
	void Update()
	{
		timeElapsed += Time.deltaTime;
		float ratio = Mathf.Min(1, timeElapsed / timeElapsedMax);
		camera.orthographicSize = .6f + (orthographicSize-.6f)*ratio;
		if ((int)ratio == 1){
			camera.orthographicSize = orthographicSize;
			enabled = false;
		}
	}
}
