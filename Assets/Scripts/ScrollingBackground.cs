using UnityEngine;
using System.Collections;

public class ScrollingBackground : MonoBehaviour
{
    public GameObject[] BackgroundPrefabs;
    public GameObject[] TransistionsPrefabs;

    public float BackgroundWidth = 18.0f;

    public float Speed = 0.05f;

    private GameObject _currentBg;
    private Transform _currentBgTransform;
    private GameObject _transition;
    private Transform _transitionTransform;
    private GameObject _nextBg;
    private Transform _nextBgTransform;

    private bool _inTransition;
    private int _index;
    private LevelManager _levelManager;


    // Use this for initialization
    void Start ()
    {
        _index = 0;
        _currentBg = (GameObject)Instantiate(BackgroundPrefabs[_index], this.transform.position, Quaternion.identity);
        _currentBgTransform = _currentBg.GetComponent<Transform>();
    }

    void Update()
    {
        if (_inTransition == false)
        {
            return;
        }
        _currentBg.GetComponent<ParallaxMain>().Pause();
        _nextBg.GetComponent<ParallaxMain>().Pause();
        Vector3 newPos = _currentBgTransform.position;
        newPos.x -= Speed * Time.time;
        _currentBgTransform.position = newPos;

        newPos = _transitionTransform.position;
        newPos.x -= Speed * Time.time;
        _transitionTransform.position = newPos;

        newPos = _nextBgTransform.position;
        newPos.x -= Speed * Time.time;
        _nextBgTransform.position = newPos;

        if (newPos.x <= 0)
        {
            _nextBgTransform.position = Vector3.zero;
            DoneUpdate();
        }
    }

    public void NextLevel(LevelManager levelManager)
    {
        // transition
        _index = (_index + 1) %BackgroundPrefabs.Length;
        Vector3 pos = this.transform.position;

        Vector3 transitionPos = pos;
        transitionPos.x += BackgroundWidth;
        _transition = (GameObject)Instantiate(TransistionsPrefabs[_index], transitionPos, Quaternion.identity);
        _transitionTransform = _transition.GetComponent<Transform>();
        _transitionTransform.position = transitionPos;

        Vector3 nextBgPos = transitionPos;
        nextBgPos.x += BackgroundWidth;
        _nextBg = (GameObject)Instantiate(BackgroundPrefabs[_index], nextBgPos, Quaternion.identity);
        _nextBgTransform = _nextBg.GetComponent<Transform>();
        _nextBgTransform.position = nextBgPos;

        _levelManager = levelManager;
        _inTransition = true;
        
    }

    void DoneUpdate()
    {
        GameObject temp = _currentBg;
        _currentBg = _nextBg;
        Destroy(_transition);
        _transition = null;
        _nextBg = null;
        Destroy(temp);
        _levelManager.LevelBackgroundUpdated();
        _levelManager = null;
        _inTransition = false;
    }
}
