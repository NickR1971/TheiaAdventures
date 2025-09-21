using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CGameObject : MonoBehaviour
{
    protected CPositionControl positionControl;

    void Update()
    {
        DoUpdate();
    }

    protected virtual void DoUpdate()
    {
        positionControl.Update();
    }
    protected void InitGameObject()
    {
        positionControl = new CPositionControl(transform);
    }

    public Vector3 GetPosition() => transform.position;
}
