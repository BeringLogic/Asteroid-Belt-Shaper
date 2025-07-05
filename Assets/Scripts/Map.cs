using UnityEngine;
using System.Collections.Generic;


public class Map : MonoBehaviour {

	[SerializeField] float width = 128f;
	[SerializeField] float height = 128f;

	[Header("Asteroids")]
	[SerializeField] protected GameObject asteroidCubePrefab;
	Vector2 noiseAsteroidsInitVector;
	[SerializeField] float asteroidScale = 0.1f;
	[SerializeField] float asteroidThreshold = 0.7f;
	public Texture2D DebugAsteroidTexture;

	[Header("Materials")]
	public List<Material> ListOfMaterials;
	Vector2 noiseMaterialsInitVector;
	[SerializeField] protected float materialsScale;
	public Texture2D DebugMaterialsTexture;


	void Awake()
	{
		Services.Register<Map> (this);
	}


	void Start () {
		
		noiseAsteroidsInitVector = new Vector2 (Random.Range (100f, 1000f), Random.Range (100f, 1000f));
		noiseMaterialsInitVector = new Vector2 (Random.Range (0f, 100f), Random.Range (0f, 100f));

		DebugAsteroidTexture = new Texture2D(Mathf.FloorToInt (width), Mathf.FloorToInt (height));
		DebugMaterialsTexture = new Texture2D(Mathf.FloorToInt (width), Mathf.FloorToInt (height));

		GameObject newCube;
		float noise;
		int materialIndex;

		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				noise = Mathf.PerlinNoise (noiseAsteroidsInitVector.x + x * asteroidScale, noiseAsteroidsInitVector.y + y * asteroidScale);

				if (noise > asteroidThreshold) {
					newCube = (GameObject) Instantiate (asteroidCubePrefab, new Vector3(x, y, 0), Quaternion.identity);
					newCube.transform.SetParent (this.transform);
					DebugAsteroidTexture.SetPixel (x,y, new Color(0, noise, 0));

					noise = Mathf.PerlinNoise (noiseMaterialsInitVector.x + x * materialsScale, noiseMaterialsInitVector.y + y * materialsScale);
					materialIndex = Mathf.FloorToInt (Mathf.Lerp (0, ListOfMaterials.Count-1, noise));
					DebugMaterialsTexture.SetPixel (x,y, ListOfMaterials[materialIndex].color);
					newCube.GetComponent <Renderer>().material = ListOfMaterials[materialIndex];
					
				}
				else {
					DebugAsteroidTexture.SetPixel (x,y, new Color(noise, 0, 0));

					noise = Mathf.PerlinNoise (noiseMaterialsInitVector.x + x * materialsScale, noiseMaterialsInitVector.y + y * materialsScale);
					materialIndex = Mathf.FloorToInt (Mathf.Lerp (0, ListOfMaterials.Count-1, noise));
					DebugMaterialsTexture.SetPixel (x,y, ListOfMaterials[materialIndex].color);
				}
			}
		}

		DebugAsteroidTexture.Apply ();
		DebugMaterialsTexture.Apply ();
	}


	public Vector3 GetCenterOfMap()
	{
		return new Vector3(width / 2f, height / 2f, 0);
	}


	public GameObject CreateCube(Transform t, Material material)
	{
		// TODO: Refactor the rest of this class to use this function

		GameObject newCube = (GameObject) Instantiate (asteroidCubePrefab, t.position, t.rotation);
		newCube.transform.SetParent (transform);
		newCube.GetComponent<AsteroidCubeScript>().SetMaterial (material);
		return newCube;
	}

}
