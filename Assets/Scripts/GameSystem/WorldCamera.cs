using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class WorldCamera : MonoBehaviour
{ 
	AniMover mover;
	void Awake()
	{
		WorldInfo.camGame = this;
		mover = gameObject.AddComponent<AniMover>();
		mover.timElapsedMax *= 1.5f;
	}
	void Start()
	{
		UnitPlayer.EVENT_MOVED += move;
		UnitPlayer.EVENT_CREATED += delegate(int x, int y) { transform.position = new Vector3(x,y,transform.position.z); };
	}
	public void move(int x, int y)
	{
		mover.move(x, y);
	}
	public Vector3 ScreenToWorldPoint(Vector3 pos)
	{
		return camera.ScreenToWorldPoint(pos);
	}
}
