using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoxSpawner : MonoBehaviour {
	public float repeatTime;
	public Vector3 spawnSize;
	public Vector3 globalVelocity;
	public TextAsset level;
	[System.Serializable]
	public class LevelMapEntry {
		public char levelChar;
		public Transform prefab;
	}
	public List<LevelMapEntry> boxList;

	int waveStartLevelIndex = 0;
	Dictionary<char, Transform> boxMap = new Dictionary<char, Transform>();

	void Start () {
		InvokeRepeating("SpawnWave", 1.0f, repeatTime);
		foreach (LevelMapEntry lme in boxList) {
			boxMap[lme.levelChar] = lme.prefab;
		}
		boxList = null;
	}

	string GetNextWave() {
		if (waveStartLevelIndex >= level.text.Length) {
			return "";
		}
		int waveEndIdx = level.text.IndexOf("\n\n", waveStartLevelIndex+2);
		if (waveEndIdx == -1) {
			waveEndIdx = level.text.Length-1;
		}
		string wave = level.text.Substring(waveStartLevelIndex, waveEndIdx-waveStartLevelIndex+1);
		waveStartLevelIndex = waveEndIdx+2;
		if (waveStartLevelIndex >= level.text.Length) {
		}
		if (wave.Length != 55) {
			Debug.LogErrorFormat("Wrong wave size {0}", wave.Length);
		}
		return wave;
	}

	void SpawnWave() {
		string wave = GetNextWave();
		if (wave == "") {
			return;
		}
		int lineStartIdx = 0;
		while (lineStartIdx < wave.Length) {
			int lineEndIdx = wave.IndexOf('\n', lineStartIdx+1);
			if (lineEndIdx == -1) {
				lineEndIdx = wave.Length-1;
			}
			if (lineEndIdx - lineStartIdx != 10) {
				Debug.LogErrorFormat("Wrong line length {0}", lineEndIdx-lineStartIdx);
			}
			for (int i = lineStartIdx; i < lineEndIdx; i++) {
				char c = wave[i];
				Transform prefabBox = boxMap[c];
				if (prefabBox == null) {
					continue;
				}
				Vector3 boxSize = prefabBox.GetComponent<MeshFilter>().sharedMesh.bounds.size;
				//Vector3 adjustedSize = Vector3.Max(spawnSize - boxSize, new Vector3());
				Vector3 position = new Vector3(spawnSize.x*((i-lineStartIdx)/(float)(lineEndIdx-lineStartIdx)), spawnSize.y*(1.0f-lineStartIdx/(float)wave.Length) - boxSize.y, 0) - (spawnSize*0.5f) + (boxSize * 0.5f);
				Transform newBox = (Transform)Instantiate(prefabBox, position, Quaternion.identity);
				newBox.SetParent(transform, false);
			}
			lineStartIdx = lineEndIdx+1;
		}
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube(transform.position, spawnSize);
	}
}
