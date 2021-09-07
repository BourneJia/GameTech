using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    private void OnGUI()
    {
        AudioSource audio = GetComponent<AudioSource>();
        if (GUI.Button(new Rect(0, 0, 100, 50), "¿ªÊ¼"))
            audio.Play();
        if (GUI.Button(new Rect(100, 0, 100, 50), "Í£Ö¹"))
            audio.Stop();
    }
}
