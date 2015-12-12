using UnityEngine;
using System.Collections;

public class BoxSpawner : MonoBehaviour {
	public float repeatTime;
	public Transform[] boxPrefabs;
	public Vector3 spawnSize;

	void Start () {
		InvokeRepeating("SpawnBox", 1.0f, repeatTime);
	}

	void SpawnBox() {
		if (boxPrefabs.Length > 0) {
			Transform prefabBox = boxPrefabs[Random.Range(0, boxPrefabs.Length)];
			Vector3 boxSize = prefabBox.GetComponent<MeshFilter>().sharedMesh.bounds.size;
			Vector3 adjustedSize = Vector3.Max(spawnSize - boxSize, new Vector3());
			Vector3 position = new Vector3(Random.value*adjustedSize.x, Random.value*adjustedSize.y, Random.value*adjustedSize.z) - (adjustedSize*0.5f);
			position += transform.position;
			Instantiate(prefabBox, position, Quaternion.identity);
		}
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube(transform.position, spawnSize);
	}
}
