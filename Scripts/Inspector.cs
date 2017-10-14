using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace RuntimeInspector.UI
{
    public class Inspector : MonoBehaviour
    {
        [SerializeField]
        bool _initOnAwake;

        public RectTransform container;

        public List<PropertyHolder> holderPrefabs = new List<PropertyHolder>();

        bool _initialized;

        List<HolderPool> _pools = new List<HolderPool>();

        List<PropertyHolder> _currentBuild = new List<PropertyHolder>();

        RectTransform _rect;

        List<PropertyHolder> _currentHolderList = new List<PropertyHolder>();

        private void Awake()
        {
            if (_initOnAwake)
                Init();
        }

        public void Init()
        {
            if (_initialized)
                return;
            _rect = GetComponent<RectTransform>();
            container.pivot = new Vector2(.5f, 1);
            foreach (var h in holderPrefabs)
            {
                _pools.Add(new HolderPool(h, container, 10));
                h.gameObject.SetActive(false);
            }
            _initialized = true;
        }

        public void Build<T>(T obj, Action<Property> onPropChanged, PropertySourceType source, bool exposedOnly)
        {
            var props = Property.GetProperties(obj, source, exposedOnly);
            foreach (var p in props)
            {
                p.onPropertyChanged += onPropChanged;
            }
            Build(props);
        }

        HolderPool GetPool(Property prop)
        {
            foreach (var p in _pools)
            {
                if (p.propType == prop.propertyType)
                {
                    
                    return p;
                }
                
            }
            return null;
        }

        public void Build(IEnumerable<Property> properties)
        {
            BuildInternal(properties);
        }

        public void Build(params Property[] properties)
        {
            BuildInternal(properties);
        }

        void BuildInternal(IEnumerable<Property> properties)
        {
            if (holderPrefabs.Count == 0)
            {
                Debug.LogError("Add holder prefabs to holderPrefabs list!");
                return;
            }
            foreach (var h in _currentBuild)
            {
                h.gameObject.SetActive(false);
            }

            _currentBuild.Clear();
            foreach (var p in properties)
            {
                var holder = GetPool(p).Pick();
                holder.SetProperty(p);
                holder.gameObject.SetActive(true);
                _currentBuild.Add(holder);
            }
            if (_currentBuild.Count > 0)
            {
                float prevHeight = 0;
                for (int i = 0; i < _currentBuild.Count; i++)
                {
                    var holder = _currentBuild[i];
                    holder.rectTransform.anchorMax = new Vector2(.5f, 1);
                    holder.rectTransform.anchorMin = new Vector2(.5f, 1);
                    holder.rectTransform.pivot = new Vector2(.5f, 1);
                    holder.rectTransform.anchoredPosition = new Vector3(0, prevHeight, 0);
                    holder.rectTransform.sizeDelta = new Vector2(container.sizeDelta.x, holder.rectTransform.sizeDelta.y);
                    prevHeight -= holder.rectTransform.sizeDelta.y;
                }
                container.sizeDelta = new Vector2(container.sizeDelta.x, -prevHeight);
            }
        }

 

        void ArrangeHolders()
        {

        }
    }

    public class HolderPool
    {
        int index;
        public Type propType;
        public List<PropertyHolder> holders = new List<PropertyHolder>();
        public HolderPool(PropertyHolder p, Transform parent, int size)
        {
            propType = p.propertyType;
            for (int i = 0; i < size; i++)
            {
                var h = GameObject.Instantiate(p);
                h.transform.SetParent(parent);
                h.transform.localScale = Vector3.one;
                h.gameObject.SetActive(false);
                holders.Add(h);
            }
        }
        public PropertyHolder Pick()
        {
            if (index >= holders.Count)
                index = 0;
            return holders[index++];
        }
    }

}
