using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    private void OnGUI()
    {
        AudioSource audio = GetComponent<AudioSource>();
        if (GUI.Button(new Rect(0, 0, 100, 50), "��ʼ"))
            audio.Play();
        if (GUI.Button(new Rect(100, 0, 100, 50), "ֹͣ"))
            audio.Stop();
    }
}
