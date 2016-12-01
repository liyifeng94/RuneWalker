using UnityEngine;
using System.Collections;

public class ScrollScriptSingle : MonoBehaviour
{
	public float _speed = 0;

    private float _movementFactor;

    private Transform _transform;

    private bool _running = true;

    public void Pause()
    {
        _running = false;
    }

    public void UnPause()
    {
        _running = true;
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
        if (_running == false)
        {
            return;
        }
        Renderer rend = GetComponent<Renderer>();
        Vector2 offest = new Vector2(Time.time*_speed*_movementFactor, 0f);
        rend.material.mainTextureOffset = offest;
	}
}
