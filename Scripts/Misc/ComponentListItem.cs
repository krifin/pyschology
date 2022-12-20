using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ComponentListItem : MonoBehaviour {
	private NPCComponent associatedComponent;

	[SerializeField] private Text buttonText;


	public void Setup (NPCComponent npcComp) {
		associatedComponent = npcComp;
		if (associatedComponent != null)
			buttonText.text = associatedComponent.GetType ().ToString ();
	}

	public void OnClickEvent () {
		associatedComponent.enabled = !associatedComponent.enabled;
		if (!associatedComponent.enabled) {
			associatedComponent.stop ();
		} else {
			associatedComponent.start ();
		}
	}
}
