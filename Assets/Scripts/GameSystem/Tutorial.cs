using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class Tutorial : MonoBehaviour
{
	public GameObject text00,text01;
	public void Awake(){
		text00.SetActive(false);
		text01.SetActive(false);
		GameSetting.EVENT_GAME_INITIATED += onTutorial;
	}
	void onTutorial(int n)
	{
		if (n != 0) return;
		text00.SetActive(true);
		text01.SetActive(true);
		GameSetting.EVENT_GAME_DESTROYED += offTutorial;
	}
	void offTutorial()
	{
		text00.SetActive(false);
		text01.SetActive(false);
		GameSetting.EVENT_GAME_DESTROYED -= offTutorial;
	}
}
