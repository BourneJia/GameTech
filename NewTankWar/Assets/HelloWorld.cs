using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelloWorld : MonoBehaviour
{
    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 200),"Hello World!");
    }
}
