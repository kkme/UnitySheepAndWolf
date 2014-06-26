﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class UIOrganizer : MonoBehaviour
{
	protected List<UIItem> myItems = new List<UIItem>();
	public virtual void Start()
	{
		positionReset();
	}

	public virtual void positionReset()
	{

	}
	public void show()
	{
		Debug.Log(gameObject.name + " " + "SHOW");
		foreach (var i in myItems) i.IsEnalbed = true;
	}
	public void hide()
	{
		Debug.Log(gameObject.name + " " + "HIDING");
		foreach (var i in myItems) i.IsEnalbed = false;

	}
	
}