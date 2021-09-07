using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    private void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 100, 100), "ÇÐ»»")) 
        {
            //Application.LoadLevel("b");
            SceneManager.LoadScene("b");
        }
    }
}
