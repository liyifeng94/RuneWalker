using UnityEngine;
using System.Collections;

public class ScrollScriptSingle : MonoBehaviour
{
	public float _speed = 0;

    private float _movementFactor;

    private Transform _transform;

    private int _running = 1;

    public void Pause()
    {
        _running = 0;
    }

    public void UnPause()
    {
        _running = 1;
    }

	// Use this for initialization
	void Start ()
	{
	    _transform = GetComponent<Transform>();
	    _movementFactor = 1/_transform.position.z;

        
	}
	
	// Update is called once per frame
	void Update ()
    {
        Renderer rend = GetComponent<Renderer>();
        Vector2 offest = new Vector2(Time.time*_speed*_movementFactor*_running, 0f);
        rend.material.mainTextureOffset = offest;
	}
}
