using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoxSpawner : MonoBehaviour {
	public float repeatTime;
	public Vector3 spawnSize;
	public Vector3 globalVelocity;
	[System.Serializable]
	public class LevelMapEntry {
		public char levelChar;
		public Transform prefab;
	}
	public List<LevelMapEntry> boxList;

	TextAsset level;
	int levelNumber;
	int waveStartLevelIndex = 0;
	Dictionary<char, Transform> boxMap = new Dictionary<char, Transform>();

	void Start () {
		foreach (LevelMapEntry lme in boxList) {
			boxMap[lme.levelChar] = lme.prefab;
		}
		boxList = null;

		levelNumber = 1;
		LoadLevel();
	}

	void LoadLevel() {
		StopSpawning();

		bool firstLoad = level == null;
		level = Resources.Load<TextAsset>("Levels/level"+levelNumber.ToString());
		if (level == null) {
			Debug.LogFormat("Can't load level {0}", levelNumber);
			// Start procedural levels
			Invoke("SpawnWave", repeatTime);
			return;
		}
		int i = level.text.IndexOf(' ')+1;
		float speed = float.Parse(level.text.Substring(0, i));
		GameObject.FindWithTag("Player").GetComponent<Controls>().movementVelocity = new Vector3(0, 0, speed);
		waveStartLevelIndex = level.text.IndexOf('\n')+1;
		repeatTime = float.Parse(level.text.Substring(i, waveStartLevelIndex-i));
		InvokeRepeating("SpawnWave", firstLoad ? 0.0f : repeatTime, repeatTime);
		GetComponentInParent<Scoring>().levelNumber = levelNumber;
		GetComponentInParent<Scoring>().Invoke ("LevelSplash", firstLoad ? 0.0f : repeatTime);
	}

	char[,] GetNextWave() {
		char[,] wave = new char[5,10];
		if (level == null) {
			// gradually increase speed & repeat time
			GameObject.FindWithTag("Player").GetComponent<Controls>().movementVelocity += new Vector3(0, 0, 1.0f);
			repeatTime *= 0.94f;
			// no level was loaded, so InvokeRepeating was never called
			// repeatedly call invoke so that the repeat time updates
			Invoke("SpawnWave", repeatTime);

			int maxCubes = 6;
			for (int i = 0; i < wave.GetLength(0); i++) {
				for (int j = 0; j < wave.GetLength (1); j++) {
					wave[i,j] = ' ';
				}
			}
			for (int i = 0; i < maxCubes; i++) {
				wave[Random.Range(0, wave.GetLength (0)), Random.Range(0, wave.GetLength (1))] = 'W';
			}
			return wave;
		}

		if (waveStartLevelIndex >= level.text.Length) {
			return null;
		}
		int waveEndIdx = level.text.IndexOf("\n\n", waveStartLevelIndex+2);
		if (waveEndIdx == -1) {
			waveEndIdx = level.text.Length-1;
		}
		string waveString = level.text.Substring(waveStartLevelIndex, waveEndIdx-waveStartLevelIndex+1);
		waveStartLevelIndex = waveEndIdx+2;
		if (waveStartLevelIndex >= level.text.Length) {
		}
		if (waveString.Length != 55) {
			Debug.LogErrorFormat("Wrong wave size {0} (char {1})", waveString.Length, waveStartLevelIndex);
		}

		int rowIndex = 0;
		int lineEndIdx;
		for (int lineStartIdx = 0; lineStartIdx < waveString.Length; lineStartIdx = lineEndIdx+1) {
			lineEndIdx = waveString.IndexOf('\n', lineStartIdx+1);
			if (lineEndIdx == -1) {
				lineEndIdx = waveString.Length-1;
			}
			if (lineEndIdx - lineStartIdx != 10) {
				Debug.LogErrorFormat("Wrong line length {0}", lineEndIdx-lineStartIdx);
				rowIndex++;
				continue;
			}
			for (int i = lineStartIdx; i < lineEndIdx; i++) {
				wave[rowIndex,i-lineStartIdx] = waveString[i];
			}
			rowIndex++;
		}
		return wave;
	}

	void SpawnWave() {
		char[,] wave = GetNextWave();
		if (wave == null) {
			levelNumber++;
			LoadLevel ();
			return;
		}
		for (int rowIndex = 0; rowIndex < wave.GetLength(0); rowIndex++) {
			for (int colIdx = 0; colIdx < wave.GetLength(1); colIdx++) {
				Transform prefabBox = boxMap[wave[rowIndex,colIdx]];
				if (prefabBox == null) {
					continue;
				}
				Vector3 boxSize = prefabBox.GetComponent<MeshFilter>().sharedMesh.bounds.size;
				//Vector3 adjustedSize = Vector3.Max(spawnSize - boxSize, new Vector3());
				Vector3 position = new Vector3(spawnSize.x*(colIdx/(float)(wave.GetLength(1))), spawnSize.y*(1.0f-rowIndex/(float)wave.GetLength(0)) - boxSize.y, 0) - (spawnSize*0.5f) + (boxSize * 0.5f);
				Transform newBox = (Transform)Instantiate(prefabBox, position, Quaternion.identity);
				newBox.SetParent(transform, false);
			}
		}
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube(transform.position, spawnSize);
	}

	public void StopSpawning() {
		CancelInvoke("SpawnWave");
	}
}
