using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace RuntimeInspector.UI
{
    public class BoolToggle : PropertyHolderGeneric<BoolProperty>
    {
        public Toggle toggle;

        protected override void AddLisnteners()
        {
            toggle.onValueChanged.AddListener(OnToggleValueChanged);
        }

        protected override void RemoveLisnteners()
        {
            toggle.onValueChanged.RemoveAllListeners();
        }

        public override void OnValuesUpdated()
        {
            var v = property.value;
            toggle.isOn = v;
        }

        private void Update()
        {
            if (property != null && toggle.isOn != property.value)
                UpdateValues();
        }

        public void OnToggleValueChanged(bool v)
        {
            property.value = v;
        }
    }
}
