using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RuntimeInspector.UI
{
    public class GUIDrawer
    {
        public event Action<Property> onPropertyChanged;

        protected float elementWidthPercent = .68f;

        readonly GUIStyle nameStyle;

        bool showEnumDropdown;

        public GUIDrawer()
        {
            nameStyle = new GUIStyle() { alignment = TextAnchor.UpperLeft, clipping = TextClipping.Clip };
        }

        public static void DrawProperties(List<Property> props, GUIDrawer drawer, Rect rect)
        {
            var y = rect.y;
            foreach (var p in props)
            {
                drawer.Draw(new Rect(rect.x, y, rect.width, rect.height), p);
                y += rect.height + 8;
            }
        }

        public void Draw(Rect rect, Property p)
        {
            GUI.contentColor = Color.black;
            GUI.Box(new Rect(rect.x - 10, rect.y, rect.width + 10, rect.height + 5), "");
            GUI.contentColor = Color.white;
            if(p.type != PropertyType.Bool && p.type != PropertyType.Enum)
                GUI.Label(new Rect(rect.x - 5, rect.y + 5, rect.width * (1f - elementWidthPercent) - 5, rect.height), p.name, nameStyle);

            switch (p.type)
            {
                case PropertyType.Float:
                    var fProp = p as FloatProperty;
                    var floatValue = 0f;
                    var floatText = GUI.TextField(new Rect(rect.x + (rect.width * .3f), rect.y + 5f, rect.width * elementWidthPercent, rect.size.y), fProp.value.ToString("0.00"));
                    if(float.TryParse(floatText, out floatValue))
                    {
                        fProp.value = floatValue;
                    }
                    break;
                case PropertyType.Int:
                    var iProp = p as IntProperty;
                    var intValue = 0;
                    var intText = GUI.TextField(new Rect(rect.x + (rect.width * .3f), rect.y + 5f, rect.width * elementWidthPercent, rect.size.y), iProp.value.ToString());
                    if (int.TryParse(intText, out intValue))
                    {
                        iProp.value = intValue;
                    }
                    break;
                case PropertyType.FloatRange:
                    var floatRangeProp = p as FloatRangeProperty;
                    floatRangeProp.value = GUI.HorizontalSlider(new Rect(rect.x + (rect.width * .3f), rect.y + 5f, rect.width * elementWidthPercent, rect.size.y), floatRangeProp.value, floatRangeProp.min, floatRangeProp.max);
                    break;
                case PropertyType.IntRange:
                    var intRangeProp = p as IntRangeProperty;
                    intRangeProp.value = (int) GUI.HorizontalSlider(new Rect(rect.x + (rect.width * .3f), rect.y + 5, rect.width * elementWidthPercent, rect.size.y), Mathf.Floor(intRangeProp.value), intRangeProp.min, intRangeProp.max);
                    break;
                case PropertyType.Enum:
                    var eProp = p as EnumProperty;
                    if (GUI.Button(new Rect(rect.x - 5, rect.y + 2f, rect.width - 10, rect.size.y), eProp.names[eProp.value]))
                    {
                        showEnumDropdown = !showEnumDropdown;
                    }
                    if (showEnumDropdown)
                    {
                        var c = GUI.backgroundColor;
                        GUI.backgroundColor = Color.black;
                        GUI.Window(1, new Rect(rect.x, rect.y + 25, rect.width, rect.height * eProp.names.Length), (v) =>
                        {
                            for (int i = 0; i < eProp.names.Length; i++)
                            {
                                if (GUI.Button(new Rect(0, (21 * i), rect.width - 10, rect.size.y), eProp.names[i]))
                                {
                                    eProp.value = i;
                                    showEnumDropdown = false;
                                }
                                GUI.depth = -1;
                            }
                        }, "", GUI.skin.window);

                        GUI.backgroundColor = c;
                    }
                    break;
                case PropertyType.Bool:
                    p.rawValue = GUI.Toggle(new Rect(rect.x - 5, rect.y + 2f, rect.width * elementWidthPercent, rect.size.y), (bool) p.rawValue, p.name);
                    break;
                case PropertyType.String:
                    p.rawValue = GUI.TextField(new Rect(rect.x + (rect.width * (1 - elementWidthPercent)) - 5, rect.y + 2f, rect.width * elementWidthPercent, rect.size.y), (string) p.rawValue);
                    break;
            }

            if (GUI.changed)
            {
                if (onPropertyChanged != null)
                    onPropertyChanged(p);
            }
        }
    }

}
