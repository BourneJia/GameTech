using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginPanel : MonoBehaviour
{
    public string userName = "";
    public string password = "";

    private void OnGUI()
    {
        //��¼��
        GUI.Box(new Rect(10, 10, 200, 120), "��¼��");
        //�û���
        GUI.Label(new Rect(20, 40, 50, 30), "�û���");
        userName = GUI.TextField(new Rect(70, 40, 120, 20), userName);

        //����
        GUI.Label(new Rect(20, 40, 50, 30), "�û���");
        password = GUI.PasswordField(new Rect(70, 70, 120, 20), password, '*');

        //��¼��ť
        if (GUI.Button(new Rect(70, 100, 50, 25), "��¼"))
        {
            if (userName == "hellolpy" && password == "123")
                Debug.Log("��¼�ɹ�");
            else
                Debug.Log("��¼ʧ��");
        }
    }
}
