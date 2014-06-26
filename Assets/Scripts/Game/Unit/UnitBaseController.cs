using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class UnitBaseController : MonoBehaviour
{
	UnitBase myUnit;
	Dictionary<KeyCode, Vector2> dirMoves =
		new Dictionary<KeyCode, Vector2>() { {KeyCode.W, new Vector2(0,1)}, {KeyCode.S, new Vector2(0,-1)},
											 {KeyCode.A, new Vector2(-1,0)}, {KeyCode.D, new Vector2(1,0)}};
	void Awake()
	{
		myUnit = GetComponent<UnitBase>();
		if (myUnit == null) new UnityException("Object needs UnitBase script.");
	}
	void Start()
	{

	}
	void Update()
	{
		foreach (var move in dirMoves)
		{

			if (Input.GetKeyDown(move.Key)) { myUnit.move(move.Value); }
		}
		//Input.GetKeyDown(KeyCode.A)
	}
}