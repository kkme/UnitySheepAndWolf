using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class EffectManager : MonoBehaviour
{
	static EffectManager me;
	static int effectBaseCount = 0;
	static public List<EffectBase> effectsBase = new List<EffectBase>();
	
	static void helperInstantiateSimpleExplosion(int posX, int posY, int dirX, int dirY, Color c)
	{
		effectBaseCount = (effectBaseCount + 1) % effectsBase.Count;
		var o = effectsBase[effectBaseCount];
		o.transform.position = new Vector3(posX + .25f * Random.Range(-1.0f, 1.0f), posY + .25f * Random.Range(-1.0f, 1.0f));
		o.move(dirX * Random.Range(2, 5.0f), dirY * Random.Range(2, 5.0f));
		o.renderer.material.color = c;
	}
	static public void ExplosionRed(int posX, int posY, int posZ, int dirX, int dirY)
	{
		helperInstantiateSimpleExplosion(posX, posY, dirX, dirY, me.colorEnemy);
		helperInstantiateSimpleExplosion(posX, posY, dirX, dirY, me.colorEnemy);
		helperInstantiateSimpleExplosion(posX, posY, dirX, dirY, me.colorEnemy);
		//me.initCmas();
	
	}
	static public void CameraDistortion(float dirX, float dirY)
	{
		if (dirX != 0)
		{
			me.camHorizontal00.swing(-dirX * Random.Range(.05f, .5f), -dirY, Random.Range(.05f, .25f));
			me.camHorizontal01.swing(-dirX * Random.Range(.05f, .5f), -dirY, Random.Range(.05f, .25f));
		}
		if (dirY != 0)
		{

			me.camVertical00.swing(-dirX, -dirY * Random.Range(.05f, .5f), Random.Range(.05f, .25f));
			me.camVertical01.swing(-dirX, -dirY * Random.Range(.05f, .5f), Random.Range(.05f, .25f));
		}
	}
	static public void ExplosionBlue(int posX, int posY, int posZ, int dirX, int dirY)
	{
		helperInstantiateSimpleExplosion(posX, posY, dirX, dirY, me.colorPlayer);
		helperInstantiateSimpleExplosion(posX, posY, dirX, dirY, me.colorPlayer);
		helperInstantiateSimpleExplosion(posX, posY, dirX, dirY, me.colorPlayer);
	}
	static public void Clear()
	{
		foreach (var v in effectsBase) v.gameObject.SetActive(false);
	}
	public Color colorPlayer, colorEnemy, colorDefault;
	public EffectBase EFFECT_SMALL;
	public GameObject 
		EFFECT_DEAD;
	public EffectExplosion
		EFFECT_EXPLOSION;
	public CamDistorted 
		camHorizontal00, camHorizontal01,
		camVertical00, camVertical01;
	
	void Awake()
	{
		me = this;
		UnitPlayer.EVENT_EXPLOSION += delegate(int x, int y) { Debug.Log("PLAYER EXPLODED " + x + " " + y); explosion(x, y, colorPlayer); };
		UnitEnemy.EVENT_EXPLOSION += delegate(int x, int y) { explosion(x, y, colorEnemy); };
		UnitPlayer_Bush.EVENT_EXPLOSION += delegate(int x, int y) { explosion(x, y, colorPlayer); };
		UnitPlayer_Mimic.EVENT_EXPLOSION += delegate(int x, int y) { explosion(x, y, colorPlayer); };
	
	}
	void initCmas()
	{
		camHorizontal00.transform.position = WorldInfo.unitPlayer_real.transform.position;
		camHorizontal01.transform.position = WorldInfo.unitPlayer_real.transform.position;
		camVertical00.transform.position = WorldInfo.unitPlayer_real.transform.position;
		camVertical01.transform.position = WorldInfo.unitPlayer_real.transform.position;
	}
	void explosion(int x, int y, Color c)
	{
		(Instantiate(EFFECT_EXPLOSION, new Vector3(x + .25f, y - .25f, 0), Quaternion.identity) as EffectExplosion).renderer.material.color = c;
		(Instantiate(EFFECT_EXPLOSION, new Vector3(x - .25f, y - .25f, 0), Quaternion.identity) as EffectExplosion).renderer.material.color = c;
		(Instantiate(EFFECT_EXPLOSION, new Vector3(x + .25f, y + .25f, 0), Quaternion.identity) as EffectExplosion).renderer.material.color = c;
		(Instantiate(EFFECT_EXPLOSION, new Vector3(x - .25f, y + .25f, 0), Quaternion.identity) as EffectExplosion).renderer.material.color = c;
	}
}
