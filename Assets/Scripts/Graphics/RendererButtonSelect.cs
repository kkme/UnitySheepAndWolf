using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class RendererButtonSelect : MonoBehaviour
{
	//goes along with UIItem_Selector
	public UIItem_Selector item;
	public Renderer display;//will be enalbed or disabled based off of select event 
	void Awake()
	{
		item.EVENT_SELECTED += eventHdr_selected;
	}
	void eventHdr_selected(bool b)
	{
		display.enabled = b;
	}
}