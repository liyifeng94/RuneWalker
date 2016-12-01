using UnityEngine;
using System.Collections;

public class ScrollingBackground : MonoBehaviour
{
    public GameObject[] BackgroundPrefabs;
    public GameObject[] TransistionsPrefabs;
    private GameObject _currentBg;
    private int _index;
    private LevelManager _levelManager;

    // Use this for initialization
    void Start ()
    {
        _index = 0;
        _currentBg = (GameObject)Instantiate(BackgroundPrefabs[_index], this.transform.position, Quaternion.identity);
    }

    void Update()
    {
        
    }

    public void NextLevel(LevelManager levelManager)
    {
        Destroy(_currentBg);
        _index = (_index + 1) %BackgroundPrefabs.Length;
        _currentBg = (GameObject)Instantiate(BackgroundPrefabs[_index], this.transform.position, Quaternion.identity);
        _levelManager = levelManager;

        DoneUpdate();
    }

    void DoneUpdate()
    {
        _levelManager.LevelBackgroundUpdated();
        _levelManager = null;
    }
}
