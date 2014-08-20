using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class Tutorial : MonoBehaviour
{
	public TextMeshDomino text00;
	public GameObject text01;
	public void Awake(){
		GameSetting.EVENT_GAME_INITIATED += onTutorial;
	}
	void Start()
	{

		text00.deActivate();
		text01.SetActive(false);
	}
	void onTutorial(int n)
	{
		if (n != 0) return;
		WorldInfo.camGame.effectZoom();
		AudioManager.Play_Intro();
		text00.activate();
		text01.SetActive(true);
		GameSetting.EVENT_GAME_DESTROYED += offTutorial;
	}
	void offTutorial()
	{
		GameSetting.EVENT_GAME_DESTROYED -= offTutorial;
		text00.deActivate();
		text01.SetActive(false);
	}
}
