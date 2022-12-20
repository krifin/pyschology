using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ControllerComponent : NetworkBehaviour {

	[SerializeField] private List<Transform> tppCameraPositions;
	[SerializeField] private List<GameObject> NPCPrefabs;
	[SerializeField] private List<Transform> NPCSpawnPoints;

	private int npcObjectsSelected;
	private int currentCameraIndex = 0;
	private bool updatedOnce = false;
	private bool updatedSpawnPoints = false;
	private bool spawnedAllNPCObjects = false;
	private bool controllerUISet = false;

	private Vector3 npcObjectPosition = new Vector3 (0f, -1237f, 665f);
	private Quaternion npcObjectRotation = Quaternion.identity;
	private Vector3 npcObjectScale = new Vector3 (6722f, 6722f, 6722f);

	void Start () {
		npcObjectsSelected = Random.Range (2, 5);
	}

	void UpdatePositions () {
		if (!updatedOnce) {
			for (int i = 0; i < tppCameraPositions.Count; i++) {
				if (transform.position.Equals (tppCameraPositions [i].position)) {
					currentCameraIndex = i;
					updatedOnce = true;
					break;
				}
			}
		}
	}

	void Update () {
		if (spawnedAllNPCObjects && !controllerUISet) {
			UIManager.instance.ControllerUISetup ();
			controllerUISet = true;
		}

		if (!updatedSpawnPoints) {
			GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag ("Chairs");
			foreach (GameObject spawnPoint in spawnPoints) {
				NPCSpawnPoints.Add (spawnPoint.transform);
			}
			if (NPCSpawnPoints.Count == 6) {
				updatedSpawnPoints = true;
			}
		}

		if (tppCameraPositions.Count == 0) {
			tppCameraPositions = NetworkManager.singleton.startPositions;
		} else {
			CmdSpawnNPCObjects ();
			UpdatePositions ();
			if (Input.GetKeyDown (KeyCode.A)) {
				currentCameraIndex--;
				if (currentCameraIndex < 0) {
					currentCameraIndex = tppCameraPositions.Count - 1;
				}
				WarpTo ();
			} else if (Input.GetKeyDown (KeyCode.D)) {
				currentCameraIndex++;
				if (currentCameraIndex > tppCameraPositions.Count - 1) {
					currentCameraIndex = 0;
				}
				WarpTo ();
			}
		}
	}

	void WarpTo () {
		transform.position = tppCameraPositions [currentCameraIndex].position;
		transform.rotation = tppCameraPositions [currentCameraIndex].rotation;
	}
		
	[Command]
	private void CmdSpawnNPCObjects () {
		GameObject _npcPrefab = NPCPrefabs[Random.Range(0, NPCPrefabs.Count)];
		if (NPCSpawnPoints.Count > 6 - npcObjectsSelected) {
			Transform _spawnPoint = NPCSpawnPoints [Random.Range (0, NPCSpawnPoints.Count)];
			GameObject _serverObj = Instantiate (_npcPrefab) as GameObject;
			_serverObj.transform.SetParent (_spawnPoint);
			_serverObj.transform.localPosition = npcObjectPosition;
			_serverObj.transform.localRotation = npcObjectRotation;
			_serverObj.transform.localScale = npcObjectScale;
			NetworkServer.SpawnWithClientAuthority (_serverObj, NetworkServer.connections [0]);
			NPCSpawnPoints.Remove (_spawnPoint);
		} else {
			spawnedAllNPCObjects = true;
		}
	}
}
