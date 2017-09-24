using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour {

	public GameObject[] wallPrefabs;

	public GameObject waypoint;

	public GameObject keyPrefab;

	public GameObject coinPrefab;

	public float maxWorldSize = 80f;
	public int blockSize = 10;

	public int[,] mazeArray = new int[,]{
		{0,0,1,0,0,0,1,0},
		{0,0,1,0,1,0,1,0},
		{0,1,1,0,1,1,1,0},
		{0,0,0,0,0,0,0,0},
		{1,0,1,1,0,1,1,1},
		{0,0,0,1,0,1,0,0},
		{1,1,0,1,0,1,0,1},
		{0,0,0,1,0,0,0,0},
	};
	public Vector2 entrance = new Vector2 (0, 7);
	public Vector2 exit = new Vector2 (7, 1);
	public Vector2 keyLocation = new Vector2 (-1, -1); // if set to -1, will be randomized

	private int layoutSize = 8;

	public enum WallType{
		Island=0, //[0,0,0,0]
		FromUp=1, //[1,0,0,0]
		FromRight=2, //[0,1,0,0]
		UpRightCorner=3, //[1,1,0,0]
		FromDown=4, //[0,0,1,0]
		VerticalAcross=5, //[1,0,1,0]
		DownRightCorner=6, //[0,1,1,0]
		TRight=7, //[1,1,1,0]
		FromLeft=8,
		UpLeftCorner=9,
		HorizontalAcross=10, //[0,1,0,1]
		TUp=11, //[1,1,0,1]
		DownLeftCorner=12, //[0,0,1,1]
		TLeft=13, //[1,0,1,1]
		TDown=14, //[0,1,1,1]
		Cross=15, //[1,1,1,1],
		OpenHorizontal=16,
		OpenVertical=17
	};


	WallType ChooseWall(int up, int right, int down, int left){
		//convert the neighbor states to an integer for easy checking
		int wallType = (up + right * 2 + down * 4 + left * 8);
		//Debug.Log(string.Format("[{0},{1},{2},{3}] => {4} ({5})", up, right, down, left, wallType, (WallType) wallType));
		return (WallType)(up + right * 2 + down * 4 + left * 8);
	}

	private int SafeArrayLookup(int [,] m, int x, int y){
		try {
			return m [y, x];
		} catch {
			return 1;
		}
	}

	// Use this for initialization
	void Start () {
		PlayerPrefs.SetInt ("numCoins", 0);
		bool isVerticalBound = false;
		bool isHorizontalBound = false;
		bool isEntranceOrExit = false;
		int mod = 1;
		GameObject wallPrefab;

		Vector3 offset = new Vector3 (transform.position.x - (layoutSize-1) * blockSize/2, transform.position.y, transform.position.z + (layoutSize-1)* blockSize/2);

		DrawCorners(offset);
		InstantiateKey (offset);

		for (int y = 0; y < layoutSize; y++) {
			isHorizontalBound = (y == 0 || y == layoutSize-1);
			for (int x = 0; x < layoutSize; x++) {
				isVerticalBound = (x == 0 || x == layoutSize-1);
				isEntranceOrExit = (entrance.x == x && entrance.y == y) || (exit.x == x && exit.y == y);


				//Debug.Log (string.Format("{0} {1} {2} {3}", mazeArray [y,x], x,y, mazeArray.Length) );
				bool containsWall = mazeArray [y, x] != 0;
				int iContainsWall = System.Convert.ToInt16 (containsWall);

				if (containsWall) {
					int up = SafeArrayLookup (mazeArray, x, y - 1);
					int right = SafeArrayLookup (mazeArray, x + 1, y);
					int down = SafeArrayLookup (mazeArray, x, y + 1);
					int left = SafeArrayLookup (mazeArray, x - 1, y);
					int w = (int)ChooseWall (up, right, down, left);
					wallPrefab = wallPrefabs [w];
					wallPrefab.name = string.Format ("({0},{1}): {2}", x, y, ChooseWall (up, right, down, left));
					GameObject.Instantiate (wallPrefab, new Vector3 (offset.x + x * blockSize, 0, offset.z -y * blockSize), wallPrefab.transform.rotation, transform);
				} else {
					// add a waypoint
					GameObject.Instantiate (waypoint, new Vector3 (offset.x + x * blockSize, 2f, offset.z -y * blockSize), Quaternion.identity, transform);

					// add a chance for a coin
					if (Random.Range (0, 7) < 1) {
						GameObject coin = (GameObject)GameObject.Instantiate (coinPrefab, new Vector3 (offset.x + x * blockSize, 1.0f, offset.z -y * blockSize), Quaternion.identity, transform);
						coin.name = "COIN";
					}
				}

				if (isVerticalBound) {
					int w;
					if(x>0) {
						mod = -1;
						w = (int)ChooseWall(1,0,1,iContainsWall);
					} else {
						mod = 1;
						w = (int)ChooseWall(1,iContainsWall,1,0);
					}
					if (isEntranceOrExit) {
						w = (int)WallType.OpenVertical;
						GameObject.Instantiate (waypoint, new Vector3 (offset.x + (x-mod) * blockSize, 1.5f, offset.z - y * blockSize), Quaternion.identity, transform);
					}
					wallPrefab = wallPrefabs[w];wallPrefab.name = "V-bound " + w;
					GameObject.Instantiate (wallPrefab, new Vector3 (offset.x + (x-mod)*blockSize, 0, offset.z -y*blockSize), wallPrefab.transform.rotation, transform);
				}

				if (isHorizontalBound){
					int w;
					if(y>0) {
						mod = -1;
						w = (int)ChooseWall(iContainsWall,1,0,1);
					} else {
						mod = 1;
						w = (int)ChooseWall(0,1,iContainsWall,1);
					}
					if (isEntranceOrExit) {
						w = (int)WallType.OpenHorizontal;
						GameObject.Instantiate (waypoint, new Vector3 (offset.x + x * blockSize, 1.5f, offset.z -(y-mod) * blockSize), Quaternion.identity, transform);
					}
					wallPrefab = wallPrefabs[w];wallPrefab.name = "H-bound " + w;
					GameObject.Instantiate (wallPrefab, new Vector3 (offset.x + x*blockSize, 0, offset.z -(y-mod)*blockSize), wallPrefab.transform.rotation, transform);
				} 
			}
		}

		// scale the generated maze to fit within the specified bounds in the world
		float scale = maxWorldSize / ((layoutSize+1) * blockSize);
		transform.localScale = new Vector3 (scale, 1, scale);
	}

	void DrawCorners(Vector3 offset){
		GameObject wallPrefab = wallPrefabs[(int)WallType.DownRightCorner];
		GameObject.Instantiate (wallPrefab, new Vector3 (offset.x -blockSize, 0, offset.z + blockSize), wallPrefab.transform.rotation, transform);
		wallPrefab = wallPrefabs[(int)WallType.DownLeftCorner];
		GameObject.Instantiate (wallPrefab, new Vector3 (offset.x + layoutSize * blockSize, 0, offset.z + blockSize), wallPrefab.transform.rotation, transform);
		wallPrefab = wallPrefabs[(int)WallType.UpRightCorner];
		GameObject.Instantiate (wallPrefab, new Vector3 (offset.x + -blockSize, 0, offset.z + layoutSize * -blockSize), wallPrefab.transform.rotation, transform);
		wallPrefab = wallPrefabs[(int)WallType.UpLeftCorner];
		GameObject.Instantiate (wallPrefab, new Vector3 (offset.x + layoutSize * blockSize, 0, offset.z + layoutSize * -blockSize), wallPrefab.transform.rotation, transform);
	}
		
	void InstantiateKey(Vector3 offset){
		while (SafeArrayLookup (mazeArray, (int)keyLocation.x, (int)keyLocation.y) == 1) {
			int x = Random.Range (1, layoutSize-1);
			int y = Random.Range (1, layoutSize-1);
			keyLocation = new Vector2 (x,y);
		}
		GameObject.Instantiate (keyPrefab, new Vector3 (offset.x + keyLocation.x * blockSize, 0, offset.z -keyLocation.y * blockSize), Quaternion.identity);
	}
}
