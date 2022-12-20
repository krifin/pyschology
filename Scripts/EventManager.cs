using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {
	public static bool inSession = true;

	private static List<NPCComponent> componentList = new List<NPCComponent> ();
	public static List<Quaternion> playerRotationList;
	private static List<float> timeList = new List<float>();
	private static float sessionCloseTime;

	[SerializeField]private GameObject playerPref;
	private GameObject player;
	private float recordRate = 0.1f;
	private static float delta = 0f;
	private int index = 0;

	// Use this for initialization
	void Start () {
	}

	void update(){
		if (!inSession) {
			player = (GameObject)Instantiate (playerPref);
		}
	}

	public static void closeSession () {
		delta = 0f;
		inSession = false;
		sessionCloseTime = Time.timeSinceLevelLoad;
		Debug.Log ("Session end time: " + sessionCloseTime);

		foreach (GameObject NPCObject in EntityManager.getObjectsOfType<NPCComponent>()) {
			NPCComponent[] npcComps = NPCObject.GetComponents<NPCComponent> ();
			foreach (NPCComponent npcComp in npcComps) {
				npcComp.setTimeProbability (90f);
				npcComp.stop ();
			}
		}
	}

	public static void addEvent(NPCComponent component, float time) {
		if (inSession) {
			componentList.Add (component);
			timeList.Add (time);
		}
	}

	public static void RemoveEvent (NPCComponent component) {
		componentList.Remove (component);
	}
		
	public void RepeatEvents () {
		if (!inSession) {
			RepeatPlayerMovement ();
			int s = componentList.Count;
			float diff = Time.timeSinceLevelLoad - sessionCloseTime;
			for (int i = 0; i < s; i++) {
				if (timeList [i] <= (diff - 0.1f)) {
					Debug.Log (timeList [i] + " " + diff + " " + Time.timeSinceLevelLoad);
					StartCoroutine (componentList [i].NPCRepeatAction ());
					timeList.RemoveAt (i);
					s = timeList.Count;
				}
			}
		}
	}

	public static void RecordPlayerMovement (Quaternion _rotation) {
		playerRotationList.Add (_rotation);
	}

	public void RepeatPlayerMovement () {
		delta += Time.deltaTime;
		if(delta >= 0.1f){
			if(player != null)
				player.transform.rotation = playerRotationList[index];
			index++;
			delta = 0f;
		}
	}
}
