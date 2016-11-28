using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour
{
    public GameObject MainGameManager;

	// Use this for initialization
	void Awake ()
    {
        if (GameManager.Instance == null)
        {
            Instantiate(MainGameManager);
        }

        //Fix resolution and aspect ratio of the game
        Screen.SetResolution(1280, 720, false);
    }
}
