﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class RendererSprite : MonoBehaviour
{
	static Vector2 OFFSET = new Vector2(-.03f, .03f);
	public EffectFlicker PREFAB_FLICKERING;

	public Sprite mySprite;
	public Color colorMain,colorShadow;
	SpriteRenderer spriteA, spriteB;
	public AniMover ani,
		aniA, aniB; //for some reason setting them public works

	SpriteRenderer helperInstantiate(string name, Sprite s, Color c, Vector3 offset)
	{
		GameObject obj = new GameObject(name, new System.Type[]{ typeof(SpriteRenderer) ,typeof(AniMover) });
		obj.transform.parent = transform;
		obj.transform.position = transform.position + offset;
		obj.layer = gameObject.layer;
		
		var r =obj.GetComponent<SpriteRenderer>();
		r.sprite = s;
		r.color = c;
		return r;
	}
	public void Awake()
	{
		spriteA = helperInstantiate("Sprite_Main", mySprite, colorMain, new Vector3(OFFSET.x, OFFSET.y, 0));
		spriteB = helperInstantiate("Sprite_Shadow", mySprite, colorShadow, new Vector3(OFFSET.x * -1, OFFSET.y * -1, +1));
		spriteB.sortingOrder = spriteA.sortingOrder - 1;
		aniA = spriteA.GetComponent<AniMover>();
		aniB = spriteB.GetComponent<AniMover>();
		ani = gameObject.AddComponent<AniMover>();
		var unit = GetComponent<UnitBase>();
		if (unit == null) return;
		if (unit.typeMe == KEnums.UNIT.ENEMY) RATIO_SHAKE *= 2.0f;
		unit.EVENT_ATTACK += swing;
		var unitSpawn = gameObject.GetComponent<UnitEnemy_Spawn>();
		if (unitSpawn == null) return;
		var e = (Instantiate(PREFAB_FLICKERING, transform.position, Quaternion.identity) as EffectFlicker);
		e.transform.parent = this.transform;
		e.gameObject.SetActive(false);
		unitSpawn.EVENT_SPAWN_SOON += delegate{e.init();};
		unitSpawn.EVENT_SPAWN_ATTEMPTED += delegate { e.off(); };

		
	}
	void Start()
	{
		var unit = gameObject.GetComponent<UnitBase>();
		if (unit == null) return;
		int angle = (360 - 90 * unit.dirFacing);
		spriteA.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
		spriteB.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
		
	}
	public void move(float x, float y)
	{
		ani.move(x, y);
	}
	public void rotate(int n)
	{
		aniA.rotate(n);
		aniB.rotate(n);
	}

	public void initAnimation(float x, float y, int dir)
	{
		//var u = GetComponent<UnitBase>();
		move(x, y);
		rotate(dir);
	}
	public bool isAnimating()
	{
		return ani.enabled || aniA.enabled || aniB.enabled;
	}
	float RATIO_SHAKE = .3f;
	public void swing(int x, int y)
	{
		ani.swing(x * RATIO_SHAKE, y * RATIO_SHAKE);
	}
	
}
