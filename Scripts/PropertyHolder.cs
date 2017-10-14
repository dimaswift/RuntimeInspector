using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RuntimeInspector.UI
{
    public abstract class PropertyHolder : MonoBehaviour
    {
        public abstract void OnValuesUpdated();

        protected abstract void AddLisnteners();

        protected abstract void RemoveLisnteners();

        public RectTransform rectTransform { get; private set; }

        public Property property { get; protected set; }

        public event System.Action<Property> onPropertyChanged;

        public abstract System.Type propertyType { get; }

        

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public void UpdateValues()
        {
            RemoveLisnteners();
            OnValuesUpdated();
            AddLisnteners();
        }

        public void SetProperty(Property prop)
        {
            property = prop;
            prop.onPropertyChanged += OnPropertyChanged;
            OnPropertySet(prop);

        }

        public virtual void OnPropertyChanged(Property p)
        {

        }

        protected virtual void OnPropertySet(Property prop)
        {

        }
    }
}
