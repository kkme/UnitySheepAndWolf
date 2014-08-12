using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class WorldCamera : MonoBehaviour
{
	void Awake()
	{
		WorldInfo.camGame = this.camera;
	}
}
