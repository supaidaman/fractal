using UnityEngine;
using System.Collections;

public class Fractal : MonoBehaviour {

    private static Vector3[] childDirections = {
		Vector3.up,
		Vector3.right,
		Vector3.left,
        Vector3.forward,
        Vector3.back
	};

    private static Quaternion[] childOrientations = {
		Quaternion.identity,
		Quaternion.Euler(0f, 0f, -90f),
		Quaternion.Euler(0f, 0f, 90f),
        Quaternion.Euler(90f, 0f, 0f),
		Quaternion.Euler(-90f, 0f, 0f)
	};

    private Material[] materials;
    public Mesh mesh;
    public Material material;
    public int maxDepth;
    public float childScale;
    private int depth;
	// Use this for initialization

    private void InitializeMaterials()
    {
        materials = new Material[maxDepth + 1];
        for (int i = 0; i <= maxDepth; i++)
        {
            float t = i / (maxDepth - 1f);
            t *= t;
            materials[i] = new Material(material);
            materials[i].color = Color.Lerp(Color.white, Color.blue, t);
         
        }
          materials[maxDepth].color = Color.magenta;
    }


	void Start () {
        if (materials == null)
        {
            InitializeMaterials();
        }
	    gameObject.AddComponent<MeshFilter>().mesh = mesh;
		gameObject.AddComponent<MeshRenderer>().material = materials[depth];
        //renderer.material.color = Color.Lerp(Color.white, Color.blue, (float)depth / maxDepth);
        if (depth < maxDepth)
        {
            StartCoroutine(CreateChildren());
        }
	}

    private void Initialize(Fractal parent, int childIndex)
    {
        mesh = parent.mesh;
        materials = parent.materials;
        maxDepth = parent.maxDepth;
        childScale = parent.childScale;
        depth = parent.depth + 1;
        transform.parent = parent.transform;
        transform.localScale = Vector3.one * childScale;
        transform.localPosition = childDirections[childIndex] * (0.5f + 0.5f * childScale);
        transform.localRotation = childOrientations[childIndex];
    }

    private IEnumerator CreateChildren()
    {
        for (int i = 0; i < childDirections.Length; i++)
        {
            yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
            new GameObject("Fractal Child").AddComponent<Fractal>().Initialize(this, i);
        }
    }

	// Update is called once per frame
	void Update () {
	
	}
}
