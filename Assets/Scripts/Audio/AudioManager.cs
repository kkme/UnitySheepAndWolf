using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {
	static AudioManager me;
	public static void Play_PlayerMoved()
	{
		me.Move.PlayOneShot(me.Move.clip);
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
		//me.DeadPlayer.Play();
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
	static public void Play_Button00()
	{
		me.button00.PlayOneShot(me.button00.clip);
	}
	static public void Play_Button01()
	{
		me.button01.PlayOneShot(me.button01.clip);
	}
	static public void Play_Attacked()
	{
		me.Attacked.Play();
	}
	static public void Play_Intro()
	{
		me.MusicIntro.Play();
	}
	public AudioSource 
		Move,
		Attack,
		AttackPush,
		Attacked,
		DeadPlayer,
		DeadEnemy,
		DeadDoor,
		Spawn,
		Explosion,
		Swap,
		MusicIntro,
		button00,
		button01;
	// Use this for initialization
	int numSpawnPlay = 0, numExplosionPlay=0;
	void Awake()
	{
		AudioManager.me = this;
		UnitBase.EVENT_EXPLOSION += delegate { numExplosionPlay++;};
		GameLoop.EVENT_GAME_OVER += delegate { DeadPlayer.Play(); };
		UnitBase.EVENT_SWAPPING += delegate { Swap.Play(); };
	}
	void Start () {

	
	}
	
	// Update is called once per frame
	void Update()
	{
		if (numSpawnPlay != 0)
		{
			if (Spawn.isPlaying) return;
			numSpawnPlay--;
			Spawn.Play();
		}
		if (numExplosionPlay != 0)
		{
			Explosion.volume = numExplosionPlay * .2f;
			numExplosionPlay = 0;
			Explosion.PlayOneShot(Explosion.clip); 
		}
	
	}
}
