using UnityEngine;
using System.Collections;
using ExtensionsUnityVectors;
public class UIInfo : MonoBehaviour
{
	public static Vector2 WORLD_CENTER,
							WORLD_SCALE,
							WORLD_ZERO;

	void Awake () {
		WORLD_SCALE = new Vector2(Camera.main.orthographicSize * Camera.main.aspect*2, Camera.main.orthographicSize*2);

		WORLD_CENTER = Camera.main.transform.position.XY();
		WORLD_ZERO = WORLD_CENTER + WORLD_SCALE.mult(-.5f,-.5f);

	}
}
