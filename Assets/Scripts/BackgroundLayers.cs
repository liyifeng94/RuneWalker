using UnityEngine;
using System.Collections;

public class BackgroundLayers : MonoBehaviour
{
    public GameObject[] Layers;

    public Transform[] BackgroundsTransforms;

    private Transform _transform;

	// Use this for initialization
	void Start ()
	{
        _transform = GetComponent<Transform>();
        BackgroundsTransforms = new Transform[Layers.Length];
	    for (int i = 0; i < Layers.Length; ++i)
	    {
            Transform layerTransform = Layers[i].GetComponent<Transform>();
            GameObject layer = (GameObject) Instantiate(Layers[i], layerTransform.position, Quaternion.identity);

            BackgroundsTransforms[i] = layer.GetComponent<Transform>();

            BackgroundsTransforms[i].rotation = layerTransform.rotation;
            BackgroundsTransforms[i].localScale = layerTransform.localScale;


            BackgroundsTransforms[i].parent = _transform;

	    }
    }
	

}
