using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RuntimeInspector.UI
{
    public class IntField : PropertyHolderGeneric<IntProperty>
    {
        public InputField inputField;

        protected override void AddLisnteners()
        {
            inputField.onValueChanged.AddListener(OnInputValueChanged);
        }

        protected override void RemoveLisnteners()
        {
            inputField.onValueChanged.RemoveAllListeners();
        }

        public override void OnValuesUpdated()
        {
            var v = property.value;
            inputField.text = v.ToString();
        }

        private void Update()
        {
            if (property != null && GetIntValue() != property.value)
                UpdateValues();
        }

        public int GetIntValue()
        {
            var v = 0;
            if (int.TryParse(inputField.text, out v))
                return v;
            return property.value;
        }

        public void OnInputValueChanged(string v)
        {
            if (string.IsNullOrEmpty(v))
            {
                property.value = 0;
                inputField.text = "0";
            }
            else
            {
                var f = 0;
                if (int.TryParse(v, out f))
                    property.value = f;
            }
        }
    }

}
