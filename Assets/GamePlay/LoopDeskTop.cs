using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class LoopDeskTop : MonoBehaviour
{
	public LoopGame gameLoop;
	Dictionary<KeyCode, Vector2> dirMoves =
		new Dictionary<KeyCode, Vector2>() { {KeyCode.W, new Vector2(0,1)}, {KeyCode.S, new Vector2(0,-1)},
											 {KeyCode.A, new Vector2(-1,0)}, {KeyCode.D, new Vector2(1,0)}};
	void Update()
	{
		foreach (var move in dirMoves)
		{
			if (Input.GetKeyDown(move.Key)) { gameLoop.UpdateNewInput(move.Value); }
		}
	}
}