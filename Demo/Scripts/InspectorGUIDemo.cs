using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RuntimeInspector.UI;
using RuntimeInspector;

public class InspectorGUIDemo : MonoBehaviour
{
    // Object whoch values we want to hook to inspector
    public DemoReflectionCube cube;

    GUIDrawer drawer;
    List<Property> properties = new List<Property>();

    private void Start()
    {
        drawer = new GUIDrawer();
        drawer.onPropertyChanged += OnPropertyChanged;
        properties = Property.GetProperties(cube, PropertySourceType.FieldsAndProperties, true);
    }


    private void OnGUI()
    {
        GUIDrawer.DrawProperties(properties, drawer, new Rect(20, 10, 300, 20));   
    }

    void OnPropertyChanged(Property p)
    {
        cube.UpdateState();
    }

}
