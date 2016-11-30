using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour
{
    public const int ScreenWidth = 1280;
    public const int ScreenHeight = 720;

    public GameObject MainGameManager;

	// Use this for initialization
	void Awake ()
    {
        if (GameManager.Instance == null)
        {
            Instantiate(MainGameManager);
        }

        //Fix resolution and aspect ratio of the game
        Screen.SetResolution(ScreenWidth, ScreenHeight, false);
    }
}
