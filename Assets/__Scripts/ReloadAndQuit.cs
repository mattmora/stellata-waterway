using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadAndQuit : MonoBehaviour
{
    public bool inputEnabled = false;
    public KeyCode reloadKey = KeyCode.R;
    public KeyCode quitKey = KeyCode.Escape;

    // Update is called once per frame
    void Update()
    {
        if (!inputEnabled) return;

        if (Input.GetKeyDown(reloadKey))
        {
            Reload();
        }

        if (Input.GetKeyDown(quitKey))
        {
            Quit();
        }
    }

    public static void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public static void Quit()
    {
        Application.Quit();
    }
}
