using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoCube : MonoBehaviour
{
    public float scaleX = 1;
    public float scaleY = 1;
    public int rotationY = 45;
    public Space space;

    public void UpdateState()
    {
        transform.localScale = new Vector3(scaleX, scaleY, transform.localScale.z);
        if(space == Space.Self)
            transform.localEulerAngles = new Vector3(0, rotationY, 0);
        else transform.eulerAngles = new Vector3(0, rotationY, 0);
    }

}
