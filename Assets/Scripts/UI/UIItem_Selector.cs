using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class UIItem_Selector :UIItem
{
	public KDels.EVENTHDR_REQUEST_SIMPLE_BOOL
		EVENT_SELECTED = delegate { };

	bool isSelected = false;
	protected List<UIItem_Selector> others = new List<UIItem_Selector>(); // well, I don't want to expose this outside

	public override void OnMouseDown()
	{
		base.OnMouseDown();
		foreach (var i in others) i.setSelected(false);
		isSelected = !isSelected;
		EVENT_SELECTED(isSelected);
	}
	public void setSelected(bool b)
	{
		isSelected = b;
		EVENT_SELECTED(isSelected);
	}
	public virtual void Start()
	{
		EVENT_SELECTED(isSelected);
	}

	public static void HELPER_CREATE_LINK(List< UIItem_Selector> items)
	{
		for (int i = 0; i < items.Count; i++)
		{
			var item00 = items[i];
			for (int j = 0; j < items.Count;  j++)
			{
				if (i == j) continue;
				item00.others.Add(items[j]);
			}
		}
	}
}
