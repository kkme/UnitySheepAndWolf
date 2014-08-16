using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class OS_DeskTOp : MonoBehaviour
{
	static public KDels.EVENTHDR_REQUEST_SIMPLE_INT EVENT_INPUT_PLAYER = delegate { };
	Dictionary<KeyCode, int> dirMoves =
		new Dictionary<KeyCode, int>() { {KeyCode.W, 0}, {KeyCode.S, 2},
											 {KeyCode.A,3}, {KeyCode.D, 1}};
	void Update()
	{
		foreach (var move in dirMoves)
		{
			if (Input.GetKeyDown(move.Key)) { EVENT_INPUT_PLAYER(move.Value); }
		}
	}
}