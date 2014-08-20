using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtensionsTransform;
using ExtensionsUnityVectors;

public class UIItem : UIOrganizer
{
	public TextMesh TextMesh;
	public KDels.EVENTHDR_REQUEST_SIMPLE EVENT_CLICK = delegate { };
	public void position(Vector2 leftTop, Vector2 rightBottom)
	{position(leftTop.y, rightBottom.y, leftTop.x, rightBottom.x);}
	public void position(Vector4 square) { position(square.y, square.w, square.x, square.z); }
	public virtual void position(float top, float bottom, float left, float right)
	{
		Vector2 ratioSpace = new Vector2(right - left, top - bottom);
		Vector2 space = ratioSpace.mult(UIInfo.WORLD_SCALE);

		Vector2 pos = UIInfo.WORLD_ZERO +
			(new Vector2(left, bottom) + ratioSpace.mult(.5f, .5f)).mult(UIInfo.WORLD_SCALE);
		transform.position = pos.XYZ();
		if (renderer == null) return;
		Vector2 size = renderer.bounds.size.XY();
		Vector2 ratio = new Vector2(space.x / size.x, space.y / size.y);
		float r = Mathf.Min(ratio.x, ratio.y);

		transform.localScale = new Vector3(transform.localScale .x*r,transform.localScale .y* r, 1);
	}
	public virtual void OnMouseDown()
	{
		EVENT_CLICK();
		//Debug.Log("EVENT CLICK HERE " + gameObject.name);
	}

	void helperIsEnabled(GameObject obj, bool b)
	{
		obj.SetActive( b);
		foreach (Transform t in obj.transform)
			helperIsEnabled(t.gameObject, b);
	}
	public bool IsEnalbed
	{
		
		set
		{
			helperIsEnabled(gameObject, value);
		}
	}

}