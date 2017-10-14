using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RuntimeInspector.UI;
using RuntimeInspector;

public class InspectorReflectionDemo : MonoBehaviour
{
    // Object whoch values we want to hook to inspector
    public DemoReflectionCube cube;
    // Inspector we want to show
    public Inspector inspector;

    private void Start()
    {
        BuildInspectorUsingReflection();
    }

    // Builds inspector without allocations in update, without using reflection and specifying all the values manually.
    void BuildInspectorUsingReflection()
    {
        // Make sure inspector is initialized before using it. Can be set to initOnAwake. 
        // Will nevel initialize twice;
        inspector.Init();

        inspector.Build(cube, OnPropertyChanged, PropertySourceType.Fields, true);

        cube.UpdateState();
    }

    void OnPropertyChanged(Property p)
    {
        cube.UpdateState();
    }

}
