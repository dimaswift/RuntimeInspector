using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RuntimeInspector.UI;
using RuntimeInspector;

public class InspectorDemo : MonoBehaviour
{
    // Object whoch values we want to hook to inspector
    public DemoCube cube;
    // Inspector we want to show
    public Inspector inspector;

    private void Start()
    {
        BuildInspectorNonAlloc();
    }

    // Builds inspector without allocations in update, without using reflection and specifying all the values manually.
    void BuildInspectorNonAlloc()
    {
        // Make sure inspector is initialized before using it. Can be set to initOnAwake. 
        // Will nevel initialize twice;
        inspector.Init();

        var scaleXProp = new FloatRangeProperty("Scale X", () => cube.scaleX, (v) => cube.scaleX = v, .1f, 5f);
        var scaleYProp = new FloatRangeProperty("Scale Y", () => cube.scaleY, (v) => cube.scaleY = v, .1f, 3f);
        var rotationProp = new IntRangeProperty("Rotation Y", () => cube.rotationY, (v) => cube.rotationY = v, -360, 360);
        var activeProp = new BoolProperty("Is Active", () => cube.gameObject.activeSelf, (v) => cube.gameObject.SetActive(v));
        var nameProp = new StringProperty("Name", () => cube.name, (v) => cube.name = v);
        var spaceProp = new EnumProperty("Space", () => (int)cube.space, (v) => cube.space = (Space)v, typeof(Space));

        spaceProp.onPropertyChanged += (v) => cube.UpdateState();
        scaleXProp.onPropertyChanged += (v) => cube.UpdateState();
        scaleYProp.onPropertyChanged += (v) => cube.UpdateState();
        rotationProp.onPropertyChanged += (v) => cube.UpdateState();

        // Builds inspector from properties we added manually
        inspector.Build(scaleXProp, scaleYProp, rotationProp, activeProp, nameProp, spaceProp);

        cube.UpdateState();
    }

}
