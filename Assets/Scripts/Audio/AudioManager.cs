using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {
	static AudioManager me;
	public static void Play_PlayerMoved()
	{
		me.Move.Play();
	}
	public static void Play_Attack()
	{
		me.Attack.Play();
	}
	static public void Play_AttackPush()
	{
		me.AttackPush.Play();
	}
	static public void Play_DeadPlayer()
	{
		me.DeadPlayer.Play();
	}
	static public void Play_DeadEnemy()
	{
		me.DeadEnemy.Play();
	}
	static public void Play_DeadDoor()
	{
		me.DeadDoor.Play();
	}
	static public void Play_Spawn()
	{
		me.numSpawnPlay++;
	}
	static public void Play_Attacked()
	{
		me.Attacked.Play();
	}
	public AudioSource 
		Move,
		Attack,
		AttackPush,
		Attacked,
		DeadPlayer,
		DeadEnemy,
		DeadDoor,
		Spawn;
	// Use this for initialization
	int numSpawnPlay = 0;
	void Awake()
	{
		AudioManager.me = this;
	}
	void Start () {

	
	}
	
	// Update is called once per frame
	void Update () {
		if (numSpawnPlay != 0 && !Spawn.isPlaying)
		{
			numSpawnPlay--;
			Spawn.Play();
		}
	
	}
}
