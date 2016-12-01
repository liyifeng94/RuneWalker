using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParallaxMain : MonoBehaviour
{
    private List<ScrollScriptSingle> _childLayers;

    private Transform _transform;

    public void Pause()
    {
        foreach (var layer in _childLayers)
        {
            layer.Pause();
        }
    }

    public void UnPause()
    {
        foreach (var layer in _childLayers)
        {
            layer.UnPause();
        }
    }

    // Use this for initialization
    void Start ()
	{
        _childLayers = new List<ScrollScriptSingle>();
        _transform = GetComponent<Transform>();
	    for (int i = 0; i < _transform.childCount ; i++)
	    {
	        GameObject childLayerObject = _transform.GetChild(i).gameObject;

	        ScrollScriptSingle script = childLayerObject.GetComponent<ScrollScriptSingle>();

            _childLayers.Add(script);
	    }
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
