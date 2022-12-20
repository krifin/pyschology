using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(EntityComponent))]
abstract public class NPCComponent : AbstractComponent {
	abstract public IEnumerator NPCAction ();
	abstract public void setTimeProbability (float tp);
	abstract public float getTimeProbability ();
	abstract public IEnumerator NPCRepeatAction ();
	abstract public void stop ();
	abstract public void start ();
	abstract public void setSessionEndTime (float t);
	//abstract public IEnumerator NPCActionOnSpot ();
}