using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;


namespace RuntimeInspector.UI
{
    public class FloatSlider : PropertyHolderGeneric<FloatRangeProperty>
    {
        public Slider slider;
        public Text valueText;

        public const string FORMAT = "0.00";

        protected override void AddLisnteners()
        {
            slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        protected override void RemoveLisnteners()
        {
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
            valueText.text = slider.value.ToString(FORMAT);
        }

        private void Update()
        {
            if(property != null && slider.value != property.value)
                UpdateValues();
        }

        public virtual void OnSliderValueChanged(float v)
        {
            property.value = v;
            UpdateText();
        }
    }

}
