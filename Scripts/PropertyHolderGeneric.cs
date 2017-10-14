using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RuntimeInspector.UI
{
    public abstract class PropertyHolderGeneric<T> : PropertyHolder where T : Property
    {
        public Text nameText;

        public new T property { get; private set; }

        public override Type propertyType
        {
            get
            {
                return typeof(T);
            }
        }

        public void SetProperty(T property)
        {
            this.property = property;
            nameText.text = property.name;
            RemoveLisnteners();
            OnValuesUpdated();
            AddLisnteners();
        }

        protected override void OnPropertySet(Property prop)
        {
            if(prop is T)
            {
                SetProperty(prop as T);
            }
        }
    }

}
