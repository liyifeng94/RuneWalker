using UnityEngine;
using System.Collections;

public class BackgroundManager : MonoBehaviour
{

    public GameObject[] BackgroundLayersPrefab;

    public float ScollingDistance = 0.01f;

    private BackgroundLayers _backgroundLayers;

    private GameObject _currentBackground;

    private Transform _transform;

    private Vector3 _lastMousePos;

    private Parallaxing _parallaxingManager;

    private const int ParallaxingFrames = 10;

    private int _frameCounter;

    // Use this for initialization of new background
    public void Init (int index)
	{
	    if (_currentBackground != null)
	    {
	        Destroy(_currentBackground);
	    }


        Transform[] bgTransforms = new Transform[BackgroundLayersPrefab.Length];
        _currentBackground = (GameObject)Instantiate(BackgroundLayersPrefab[index], Vector3.zero, Quaternion.identity);
        _backgroundLayers = _currentBackground.GetComponent<BackgroundLayers>();
        if (_backgroundLayers == null)
        {
            Debug.LogError("Error: invalid background prefab. At: " + index);
            return;
        }

        _currentBackground.transform.parent = _transform;
    }

    // Use this for initialization
    void Start()
    {
        _transform = GetComponent<Transform>();
        _lastMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _backgroundLayers = null;
        _currentBackground = null;
        _parallaxingManager = new Parallaxing();
        _frameCounter = 0;
        Init(0);
    }

    // Update is called once per frame
    void Update ()
    {
        if (_frameCounter >= ParallaxingFrames)
        {
            Vector3 currentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            _parallaxingManager.Parallax(_lastMousePos, currentMousePos, _backgroundLayers.BackgroundsTransforms, ScollingDistance);

            _lastMousePos = currentMousePos;
        }
        ++_frameCounter;
    }
}
