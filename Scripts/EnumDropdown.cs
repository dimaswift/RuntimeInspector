using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

namespace RuntimeInspector.UI
{
    public class EnumDropdown : PropertyHolderGeneric<EnumProperty>
    {
        public Dropdown dropdown;

        List<string> names = new List<string>(10);

        public override void OnValuesUpdated()
        {
            dropdown.value = property.value;
        }

        protected override void RemoveLisnteners()
        {
            dropdown.ClearOptions();
            names.Clear();
            foreach (var n in property.names)
            {
                names.Add(n);
            }
            dropdown.AddOptions(names);
            dropdown.onValueChanged.RemoveAllListeners();
        }

        protected override void AddLisnteners()
        {
            dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        }

        private void Update()
        {
            if (property != null && property.value != dropdown.value)
                UpdateValues();
        }

        public void OnDropdownValueChanged(int value)
        {
            property.value = value;
        }
    }

}
