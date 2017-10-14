using System.Collections.Generic;
using System;
using UnityEngine;
using System.Reflection;
namespace RuntimeInspector
{
    public enum PropertyType
    {
        Float, Int, Enum, Bool, String, FloatRange, IntRange
    }

    public class PropertyRange : Attribute
    {
        public float min;
        public float max;

        public PropertyRange(float min, float max)
        {
            this.min = min;
            this.max = max;
        }
    }

    public class CustomPropertyName : Attribute
    {
        public string name;

        public CustomPropertyName(string name)
        {
            this.name = name;
        }
    }

    public class PropertyHash : Attribute
    {
        public int hash;

        public PropertyHash(int hash)
        {
            this.hash = hash;
        }
    }

    public class ExposeProperty : Attribute
    {

    }

    public class IgnoreProperty : Attribute
    {

    }

    public enum PropertySourceType
    {
        Fields, Properties, FieldsAndProperties
    }

    public abstract class Property
    {
        public event Action<Property> onPropertyChanged;
        public PropertyType type { get; protected set; }
        public string name { get; set; }
        public abstract object rawValue { get; set; }
        public abstract Type propertyType { get; }
        public int hash { get; set; }


        public static List<Property> GetProperties<T>(T obj, PropertySourceType source, bool explosedOnly)
        {
            return GetProperties(obj, source, new List<Property>(), explosedOnly);
        }

        public static List<Property> GetProperties<T>(T obj, PropertySourceType source, List<Property> listToFill, bool explosedOnly)
        {
            if (source == PropertySourceType.Fields)
            {
                listToFill.Clear();
                return GetPropertiesFromFields(obj, listToFill, explosedOnly);
            }
          
            else if(source == PropertySourceType.Properties)
            {
                listToFill.Clear();
                return GetPropertiesFromProperties(obj, listToFill, explosedOnly);
            }
            else
            {
                listToFill.Clear();
                GetPropertiesFromFields(obj, listToFill, explosedOnly);
                return GetPropertiesFromProperties(obj, listToFill, explosedOnly);
            }
        }

        protected void OnPropertyChanged()
        {
            if (onPropertyChanged != null)
                onPropertyChanged(this);
        }


        static List<Property> GetPropertiesFromFields<T>(T obj, List<Property> props, bool explosedOnly)
        {
            var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField);
          
            foreach (var f in fields)
            {
                if (explosedOnly && !Attribute.IsDefined(f, typeof(ExposeProperty)))
                    continue;
                if (Attribute.IsDefined(f, typeof(IgnoreProperty)))
                    continue;
                Property prop = null;
                var type = f.FieldType;
                if(type == typeof(float))
                {
                    if (Attribute.IsDefined(f, typeof(PropertyRange)))
                    {
                        var rangeAttr = (PropertyRange) Attribute.GetCustomAttribute(f, typeof(PropertyRange));
                        prop = new FloatRangeProperty(f.Name, () => (float) f.GetValue(obj), (v) => f.SetValue(obj, v), rangeAttr.min, rangeAttr.max);
                    }
                    else
                    {
                        prop = new FloatProperty(f.Name, () => (float) f.GetValue(obj), (v) => f.SetValue(obj, v));
                    }
                }
                else if (type == typeof(int))
                {
                    if (Attribute.IsDefined(f, typeof(PropertyRange)))
                    {
                        var rangeAttr = (PropertyRange) Attribute.GetCustomAttribute(f, typeof(PropertyRange));
                        prop = new IntRangeProperty(f.Name, () => (int) f.GetValue(obj), (v) => f.SetValue(obj, v), (int) rangeAttr.min, (int) rangeAttr.max);
                    }
                    else
                    {
                        prop = new IntProperty(f.Name, () => (int) f.GetValue(obj), (v) => f.SetValue(obj, v));
                    }
                }
                else if (type == typeof(string))
                {
                    prop = new StringProperty(f.Name, () => (string) f.GetValue(obj), (v) => f.SetValue(obj, v));
                }
                else if (type.IsEnum)
                {
                    prop = new EnumProperty(f.Name, () => (int) f.GetValue(obj), (v) => f.SetValue(obj, v), type);
                }
                else if (type == typeof(bool))
                {
                    prop = new BoolProperty(f.Name, () => (bool) f.GetValue(obj), (v) => f.SetValue(obj, v));
                }
                if(prop != null)
                {
                    if (Attribute.IsDefined(f, typeof(PropertyHash)))
                    {
                        var hashAttr = (PropertyHash) Attribute.GetCustomAttribute(f, typeof(PropertyHash));
                        prop.hash = hashAttr.hash;
                    }
                    if (Attribute.IsDefined(f, typeof(CustomPropertyName)))
                    {
                        var nameAttr = (CustomPropertyName) Attribute.GetCustomAttribute(f, typeof(CustomPropertyName));
                        prop.name = nameAttr.name;
                    }
                    props.Add(prop);
                }
            }
            return props;
        }

        static List<Property> GetPropertiesFromProperties<T>(T obj, List<Property> props, bool explosedOnly)
        {
            var objectProps = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);
           
            foreach (var p in objectProps)
            {
                if (explosedOnly && !Attribute.IsDefined(p, typeof(ExposeProperty)))
                    continue;
                if (Attribute.IsDefined(p, typeof(IgnoreProperty)))
                    continue;
                if (p.CanWrite == false)
                    continue;
                Property prop = null;
                var type = p.PropertyType;
                if (type == typeof(float))
                {
                    if (Attribute.IsDefined(p, typeof(PropertyRange)))
                    {
                        var rangeAttr = (PropertyRange) Attribute.GetCustomAttribute(p, typeof(PropertyRange));
                        prop = new FloatRangeProperty(p.Name, () => (float) p.GetValue(obj, null), (v) => p.SetValue(obj, v, null), rangeAttr.min, rangeAttr.max);
                    }
                    else
                    {
                        prop = new FloatProperty(p.Name, () => (float) p.GetValue(obj, null), (v) => p.SetValue(obj, v, null));
                    }
                }
                else if (type == typeof(int))
                {
                    if (Attribute.IsDefined(p, typeof(PropertyRange)))
                    {
                        var rangeAttr = (PropertyRange) Attribute.GetCustomAttribute(p, typeof(PropertyRange));
                        prop = new IntRangeProperty(p.Name, () => (int) p.GetValue(obj, null), (v) => p.SetValue(obj, v, null), (int) rangeAttr.min, (int) rangeAttr.max);
                    }
                    else
                    {
                        prop = new IntProperty(p.Name, () => (int) p.GetValue(obj, null), (v) => p.SetValue(obj, v, null));
                    }
                }
                else if (type == typeof(string))
                {
                    prop = new StringProperty(p.Name, () => (string) p.GetValue(obj, null), (v) => p.SetValue(obj, v, null));
                }
                else if (type.IsEnum)
                {
                    prop = new EnumProperty(p.Name, () => (int) p.GetValue(obj, null), (v) => p.SetValue(obj, v, null), type);
                }
                else if (type == typeof(bool))
                {
                    prop = new BoolProperty(p.Name, () => (bool) p.GetValue(obj, null), (v) => p.SetValue(obj, v, null));
                }
                if (prop != null)
                {
                    if (Attribute.IsDefined(p, typeof(PropertyHash)))
                    {
                        var hashAttr = (PropertyHash) Attribute.GetCustomAttribute(p, typeof(PropertyHash));
                        prop.hash = hashAttr.hash;
                    }
                    if (Attribute.IsDefined(p, typeof(CustomPropertyName)))
                    {
                        var nameAttr = (CustomPropertyName) Attribute.GetCustomAttribute(p, typeof(CustomPropertyName));
                        prop.name = nameAttr.name;
                    }
                    props.Add(prop);
                }
            }
            return props;
        }
    }

    public abstract class Property<T> : Property
    {
        Func<T> getter;
        Action<T> setter;

        public override Type propertyType
        {
            get
            {
                return typeof(Property);
            }
        }
        public T value
        {
            get { return getter(); }
            set
            {
                setter(value);
                OnPropertyChanged();
            }
        }


        public override object rawValue
        {
            get
            {
                return value;
            }
            set
            {
                this.value = (T)value;
            }
        }

        public Property(Func<T> getter, Action<T> setter)
        {
            this.setter = setter;
            this.getter = getter;
        }
    }

    public class FloatProperty : Property<float>
    {
        public override Type propertyType
        {
            get
            {
                return typeof(FloatProperty);
            }
        }
        public FloatProperty(string name, Func<float> getter, Action<float> setter)
            : base(getter, setter)
        {
            type = PropertyType.Float;
            this.name = name;
        }
    }

    public class FloatRangeProperty : Property<float>
    {
        public float max { get; private set; }
        public float min { get; private set; }
        public override Type propertyType
        {
            get
            {
                return typeof(FloatRangeProperty);
            }
        }
        public FloatRangeProperty(string name, Func<float> getter, Action<float> setter, float min = 0, float max = 1) 
            : base(getter, setter)
        {
            type = PropertyType.FloatRange;
            this.name = name;
            this.min = min;
            this.max = max;
        }
    }

    public class IntRangeProperty : Property<int>
    {
        public int max { get; private set; }
        public int min { get; private set; }
        public override Type propertyType
        {
            get
            {
                return typeof(IntRangeProperty);
            }
        }
        public IntRangeProperty(string name, Func<int> getter, Action<int> setter, int min = 0, int max = 10) : base(getter, setter)
        {
            type = PropertyType.IntRange;
            this.name = name;
            this.max = max;
            this.min = min;
        }
    }

    public class IntProperty : Property<int>
    {

        public override Type propertyType
        {
            get
            {
                return typeof(IntProperty);
            }
        }
        public IntProperty(string name, Func<int> getter, Action<int> setter, int min = 0, int max = 0) : base(getter, setter)
        {
            type = PropertyType.Int;
            this.name = name;
        }
    }

    public class StringProperty : Property<string>
    {
        public override Type propertyType
        {
            get
            {
                return typeof(StringProperty);
            }
        }
        public StringProperty(string name, Func<string> getter, Action<string> setter) : base(getter, setter)
        {
            type = PropertyType.String;
            this.name = name;
        }
    }

    public class BoolProperty : Property<bool>
    {
        public override Type propertyType
        {
            get
            {
                return typeof(BoolProperty);
            }
        }

        public BoolProperty(string name, Func<bool> getter, Action<bool> setter) : base(getter, setter)
        {
            type = PropertyType.Bool;
            this.name = name;
        }
    }

    public class EnumProperty : Property<int>
    {
        public string[] names { get; private set; }
        bool showNames;
        public override Type propertyType
        {
            get
            {
                return typeof(EnumProperty);
            }
        }

        public EnumProperty(string name, Func<int> getter, Action<int> setter, Type enumType) : base(getter, setter)
        {
            type = PropertyType.Enum;
            this.name = name;
            names = Enum.GetNames(enumType);
        }
    }

}
