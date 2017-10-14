using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RuntimeInspector;

public class DemoReflectionCube : MonoBehaviour
{
    [ExposeProperty]
    [PropertyRange(1, 5)]
    [CustomPropertyName("X Scale")]
    public float scaleX = 1;
    [ExposeProperty]
    [PropertyRange(1, 5)]
    [CustomPropertyName("Y Scale")]
    public float scaleY = 1;
    [ExposeProperty]
    public int rotationY = 45;
    [ExposeProperty]
    public Space space;
    [ExposeProperty]
    [CustomPropertyName("Active")]
    public bool isActive = true;

    public void UpdateState()
    {
        gameObject.SetActive(isActive);
        transform.localScale = new Vector3(scaleX, scaleY, transform.localScale.z);
        if(space == Space.Self)
            transform.localEulerAngles = new Vector3(0, rotationY, 0);
        else transform.eulerAngles = new Vector3(0, rotationY, 0);
    }

}
