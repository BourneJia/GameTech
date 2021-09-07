using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginPanel : MonoBehaviour
{
    public string userName = "";
    public string password = "";

    private void OnGUI()
    {
        //登录框
        GUI.Box(new Rect(10, 10, 200, 120), "登录框");
        //用户名
        GUI.Label(new Rect(20, 40, 50, 30), "用户名");
        userName = GUI.TextField(new Rect(70, 40, 120, 20), userName);

        //密码
        GUI.Label(new Rect(20, 40, 50, 30), "用户名");
        password = GUI.PasswordField(new Rect(70, 70, 120, 20), password, '*');

        //登录按钮
        if (GUI.Button(new Rect(70, 100, 50, 25), "登录"))
        {
            if (userName == "hellolpy" && password == "123")
                Debug.Log("登录成功");
            else
                Debug.Log("登录失败");
        }
    }
}
