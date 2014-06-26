using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class CameraSub : MonoBehaviour
{
	void Start()
	{
		var main = Camera.main;
		camera.transform.position = main.transform.position;
		camera.projectionMatrix = main.projectionMatrix;
		camera.orthographicSize = main.orthographicSize;
		camera.nearClipPlane = main.nearClipPlane;
		camera.farClipPlane = main.farClipPlane;
	}
}
