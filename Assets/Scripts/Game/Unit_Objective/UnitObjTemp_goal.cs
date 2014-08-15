using System.Collections.Generic;
using System.Linq;
using System.Text;

public class UnitObjTemp_goal : UnitObj
{
	public static KDels.EVENTHDR_REQUEST_SIMPLE EVENT_DESTROYED = delegate { };
	public override UnitBase init()
	{
		base.init();
		WorldInfo.PLAYER_GOAL = pos;
		WorldInfo.door = this;
		return (UnitBase)this;
	}
	public override void Awake()
	{
		base.Awake();
		isDestroyable_simpleAttack = true;
	}
	public override void kill(int dirX, int dirY)
	{
		base.kill(dirX, dirY);
		EVENT_DESTROYED();
	}
	public override void Destroy()
	{
		base.Destroy();
		WorldInfo.door = null;
	}
}
