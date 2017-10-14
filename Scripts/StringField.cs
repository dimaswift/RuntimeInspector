using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace RuntimeInspector.UI
{
    public class StringField : PropertyHolderGeneric<StringProperty>
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
            inputField.text = v;
        }

        private void Update()
        {
            if (property != null && inputField.text != property.value)
                UpdateValues();
        }

        public void OnInputValueChanged(string v)
        {
            property.value = v;
        }
    }
}
