using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RuntimeInspector.UI
{
    public class FloatField : PropertyHolderGeneric<FloatProperty>
    {
        public InputField inputField;

        const string FORMAT = "0.00";

        float lastValue;

        protected override void AddLisnteners()
        {
            inputField.onValueChanged.AddListener(OnInputFieldValueChanged);
        }

        protected override void RemoveLisnteners()
        {
            inputField.onValueChanged.RemoveAllListeners();
        }

        public override void OnValuesUpdated()
        {
            var v = property.value;
            lastValue = v;
            inputField.text = v.ToString(FORMAT);
        }

        private void Update()
        {
            if (property != null && lastValue != property.value)
                UpdateValues();
        }

        public float GetFloatValue()
        {
            var f = 0f;
            if (float.TryParse(inputField.text, out f))
                return f;
            else return property.value;
        }

        public void OnInputFieldValueChanged(string v)
        {
            if (string.IsNullOrEmpty(v))
            {
                property.value = 0;
                inputField.text = "0";
            }
            else
            {
                var f = 0f;
                if (float.TryParse(v, out f))
                    property.value = f;
            }
        }
    }

}
