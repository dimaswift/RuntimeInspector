using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RuntimeInspector.UI
{
    public class IntSlider : PropertyHolderGeneric<IntRangeProperty>
    {
        public Slider slider;
        public Text valueText;
 
        protected override void AddLisnteners()
        {
            slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        protected override void RemoveLisnteners()
        {
            slider.wholeNumbers = true;
            slider.onValueChanged.RemoveAllListeners();
        }

        public override void OnValuesUpdated()
        {
            var v = property.value;
            slider.minValue = property.min;
            slider.maxValue = property.max;
            slider.value = v;
            UpdateText();
        }

        void UpdateText()
        {
            valueText.text = slider.value.ToString();
        }

        private void Update()
        {
            if(property != null && slider.value != property.value)
                UpdateValues();
        }

        public void OnSliderValueChanged(float v)
        {
            property.value = (int) v;
            UpdateText();
        }
    }

}
