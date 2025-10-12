using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCameraController : MonoBehaviour
{
    private ICamera iCamera;
    private IInputController iInputController;
    private float speed = 10.0f;
    private int dir;

    private   void Start()
    {
        iCamera = GetComponent<ICamera>();
        iInputController = AllServices.Container.Get<IInputController>();
        iCamera.SetPosition(EMapDirection.north);
        dir = (int)EMapDirection.north;
    }

    private void Update()
    {
        if (iCamera.IsBusy()) return;
        if(iInputController.IsPressed(MyButton.LBumper))
        {
            dir++;
            if (dir == 9) dir = 1;
            iCamera.SetPosition((EMapDirection)dir);
        }
        if (iInputController.IsPressed(MyButton.RBumper))
        {
            dir--;
            if (dir == 0) dir = 8;
            iCamera.SetPosition((EMapDirection)dir);
        }
        if (iInputController.IsPressed(MyButton.Rstick))
        {
            iCamera.SetPosition((EMapDirection)dir);
        }

        iInputController.GetRightStick(out float h, out float v);

        RelativeMove(h, v);
    }
    private void RelativeMove(float _horizontal, float _vertical)
    {
        Vector3 v = transform.rotation.eulerAngles;
        float tmpCos = Mathf.Cos(v.y * Mathf.Deg2Rad);
        float tmpSin = Mathf.Sin(v.y * Mathf.Deg2Rad);

        float offsetH = _horizontal * speed * Time.deltaTime;
        float offsetV = _vertical * speed * Time.deltaTime;
        float x = offsetH * tmpCos + offsetV * tmpSin;
        float z = -offsetH * tmpSin + offsetV * tmpCos;
        Vector3 pos = new Vector3(x, 0, z);
        iCamera.SetPositionInstant(transform.position + pos);
    }
}
