using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityComponent))]
public class PlayerComponent : AbstractComponent {
	
	private void Start () {
		InvokeRepeating ("SendPlayerTransform", 0.1f, 0.1f);
	}

	private void SendPlayerTransform () {
		EventManager.RecordPlayerMovement (transform.rotation);
	}
}
