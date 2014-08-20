using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class TransitionEffect :MonoBehaviour
{
	static TransitionEffect me;
	enum STATE { STARTED, NOTHING_TO_WHITE,WHITE_TO_NOTHING, TRANSITION, NOTHING };
	public delegate void UpdateFunction(float ratio);
	public delegate void StartTransition(string textNow, string textBefore="");
	public static StartTransition EVENT_TRANSITION_START = delegate { };
	public static KDels.EVENTHDR_REQUEST_SIMPLE
		EVENT_FINISHED_TRANSITION = delegate { } ;

	public TextMesh tMesh,tMeshBefore;
	public MeshRenderer background;
	public SpriteRenderer logo; //one time usage but still very useful
	float timeElapsed = 0, timeElapsedMax = .3f;

	Vector4 colorFrom = new Vector4(0, 0, 0, 1),
			colorTo = new Vector4(1, 1, 1, 1);

	
	STATE stateMe = STATE.STARTED;

	void Awake()
	{
		me = this;
		background.gameObject.SetActive(true);
		logo.gameObject.SetActive(true);

		//textBefore.gameObject.SetActive(false);
		//textAfter.gameObject.SetActive(false);
		//EVENT_TRANSITION_START += initTransition;
	}
	void Start()
	{

		//EVENT_FINISHED_TRANSITION(); 
		StartCoroutine(wait());
		//timeElapsed = 90;
	}

	IEnumerator<WaitForSeconds> wait()
	{
		enabled = false;
		yield return new WaitForSeconds(1.5f);
		enabled = true;
	}
	public void initTransition(int n) { initTransition("" + n); }
	public void initTransition(string t = "", string tBefore = "")
	{
		this.gameObject.SetActive(true);
		tMesh.text = t;
		tMeshBefore.text = tBefore;
		tMeshBefore.transform.localPosition = new Vector3(0,-.35f,+1);
		tMeshBefore.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -.5f);

		timeElapsed = 0;
		stateMe = STATE.NOTHING_TO_WHITE;

		enabled = true;
	}
	void UpdateInit(float ratio)
	{
		var colorNow = colorFrom + (colorTo - colorFrom) * ratio;
		background.material.color = new Color(colorNow.x, colorNow.y, colorNow.z, colorNow.w);
		logo.color = new Color(logo.color.r, logo.color.g, logo.color.b, 1 - ratio);
		if ((int)ratio == 1)
		{
			GameObject.Destroy(logo.gameObject);
			timeElapsed = 0;

			stateMe = STATE.WHITE_TO_NOTHING;
			EVENT_FINISHED_TRANSITION();
		}
	}
	static public void EVENT_RESUME() { me.enabled = true; }
	void UpdateFromWhiteToNothing(float ratio)
	{
		background.material.color = new Color(1, 1, 1, 1 - ratio);
		tMesh.color = new Color(tMesh.color.r, tMesh.color.g, tMesh.color.b, 1 - ratio);
		tMeshBefore.color = new Color(tMeshBefore.color.r, tMeshBefore.color.g, tMeshBefore.color.b, 1-ratio);
		if ((int)ratio == 1)
		{
			timeElapsed = 0;
			stateMe = STATE.NOTHING;
		}
	}

	void UpdateFromNothingToWhite(float ratio)
	{
		background.material.color = new Color(1, 1, 1, ratio);
		tMesh.color = new Color(tMesh.color.r, tMesh.color.g, tMesh.color.b, ratio);
		tMeshBefore.color = new Color(tMeshBefore.color.r, tMeshBefore.color.g, tMeshBefore.color.b, ratio);
		if ((int)ratio == 1)
		{
			timeElapsed = 0;
			stateMe = STATE.WHITE_TO_NOTHING;
			EVENT_FINISHED_TRANSITION();
			enabled = false;
		}
	}
	void Update()
	{
		timeElapsed += Mathf.Min(Time.deltaTime, .025f);
		float ratio = Mathf.Min( timeElapsed / timeElapsedMax ,1);
		bool isDone = (int)ratio == 1.0f ;//finished
		switch (stateMe)
		{
			case STATE.STARTED:
				UpdateInit(ratio);
				break;
			case STATE.NOTHING_TO_WHITE:
				UpdateFromNothingToWhite(ratio);
				break;
			case STATE.WHITE_TO_NOTHING:
				UpdateFromWhiteToNothing(ratio);
				break;
			case STATE.NOTHING:
				this.gameObject.SetActive(false);
				//logo.gameObject.SetActive(false);
				//background.gameObject.SetActive(false);
				//textAfter.gameObject.SetActive(false);
				//textBefore.gameObject.SetActive(false);
				enabled = false;
				break;

		}
		
		//if (isDone) enabled = false;
	}
}
