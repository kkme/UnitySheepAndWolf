using System.Collections.Generic;
using System.Linq;
using System.Text;

public class UnitObj : UnitStatic
{
	public override KEnums.UNIT typeMe
	{
		get
		{
			return KEnums.UNIT.ENVIRONMENT;
		}
	}
	override public void Awake()
	{
		base.Awake();
		isDestroyable_simpleAttack = false;
		isDestroyable_bomb = false;
	}
}
