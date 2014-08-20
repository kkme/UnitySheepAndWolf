using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class TextMeshDomino : MonoBehaviour
{
	string content;
	TextMesh mesh;
	float timeElapsed = 0, timeElapsedMax = 5.0f;
	public TextMeshDomino other;
	void Awake()
	{
		mesh = GetComponent<TextMesh>();
		content = mesh.text;
		timeElapsedMax = content.Length*.25f;
	}
	public void activate()
	{
		mesh.text = "";
		timeElapsed = 0;
		enabled = true;
	}
	public void deActivate()
	{
		mesh.text = "";
		enabled = false;
		if (other != null) other.deActivate();
	}
	void Update()
	{
		timeElapsed += Time.deltaTime;
		float ratio =Mathf.Min(1, timeElapsed / timeElapsedMax);
		mesh.text = content.Substring(0, (int)(content.Length * ratio));
		if ((int)ratio == 1)
		{
			enabled = false;
			if (other != null) other.activate();
		}
	}
}
