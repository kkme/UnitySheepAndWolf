using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class UnitObjTemp : UnitObj
{
	public virtual void init()
	{
		//do what you want to do here;
	}
	public void Start()
	{
		base.Start();
		init(); 
		//kill(); // do whatever I do then just delete it right away
	}
	//temporary, they are not going to be registered;
	public void registerOnGrid() {}
	public void unRegisterOnGrid(){}
}
