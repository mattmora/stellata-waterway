using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ReloadAndQuit : MonoBehaviour
{
    public bool inputEnabled = false;
    public KeyCode reloadKey = KeyCode.R;
    public KeyCode quitKey = KeyCode.Escape;

    public UnityEvent preReload;
    public UnityEvent preQuit;

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

    public void Reload()
    {
        preReload.Invoke();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer) return;
        preQuit.Invoke();
        Application.Quit();
    }
}
