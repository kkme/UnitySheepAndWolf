using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class UIOrganizer : MonoBehaviour
{
	protected List<UIItem> myItems = new List<UIItem>();
	public virtual void Awake()
	{

	}
	public virtual void Start()
	{
		positionReset();
	}

	public virtual void positionReset()
	{

	}
	public virtual void show()
	{
		//Debug.Log(gameObject.name + " " + "SHOW");
		this.enabled = true;
		foreach (var i in myItems) i.IsEnalbed = true;
	}
	public virtual void hide()
	{
		//Debug.Log(gameObject.name + " " + "HIDING");
		this.enabled = false;
		foreach (var i in myItems) i.IsEnalbed = false;

	}
	
}