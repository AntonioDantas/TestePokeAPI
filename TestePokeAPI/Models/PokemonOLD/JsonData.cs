#pragma warning disable 1591

#region Header
/*
 * JsonData.cs
 *   Generic type to hold JSON data (objects, arrays, and so on). This is
 *   the default type returned by JsonMapper.ToObject().
 *
 * The authors disclaim copyright to this source code. For more details, see
 * the COPYING file included with this distribution.
 */
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace TestePokeAPI.Models.Pokemon
{
    public class JsonData : IJsonWrapper, IEquatable<JsonData>
    {
        #region Fields
        IList<JsonData> inst_array;
        bool inst_boolean;
        double inst_double;
        int inst_int;
        long inst_long;
        IDictionary<string, JsonData> inst_object;
        string inst_string;
        string json;
        JsonType type;

        // Used to implement the IOrderedDictionary interface
        IList<KeyValuePair<string, JsonData>> object_list;
        #endregion


        #region Properties
        public int Count => EnsureCollection().Count;

        public bool IsArray => type == JsonType.Array;

        public bool IsBoolean => type == JsonType.Boolean;

        public bool IsDouble => type == JsonType.Double;

        public bool IsInt => type == JsonType.Int;

        public bool IsLong => type == JsonType.Long;

        public bool IsObject => type == JsonType.Object;

        public bool IsString => type == JsonType.String;

        public JsonType JsonType => type;

        public object Value
        {
            get
            {
                switch (type)
                {
                    case JsonType.Object:
                        return inst_object;
                    case JsonType.Array:
                        return inst_array;
                    case JsonType.Boolean:
                        return inst_boolean;
                    case JsonType.Double:
                        return inst_double;
                    case JsonType.Int:
                        return inst_int;
                    case JsonType.Long:
                        return inst_long;
                    case JsonType.String:
                        return inst_string;
                    default:
                        return null;
                }
            }
        }

        public ICollection<string> Keys
        {
            get
            {
                EnsureDictionary();
                return inst_object.Keys;
            }
        }
        #endregion


        #region ICollection Properties
        int ICollection.Count => Count;

        bool ICollection.IsSynchronized => EnsureCollection().IsSynchronized;

        object ICollection.SyncRoot => EnsureCollection().SyncRoot;
        #endregion


        #region IDictionary Properties
        bool IDictionary.IsFixedSize => EnsureDictionary().IsFixedSize;

        bool IDictionary.IsReadOnly => EnsureDictionary().IsReadOnly;

        ICollection IDictionary.Keys
        {
            get
            {
                EnsureDictionary();
                IList<string> keys = new List<string>();

                foreach (KeyValuePair<string, JsonData> entry in
                         object_list)
                {
                    keys.Add(entry.Key);
                }

                return (ICollection)keys;
            }
        }

        ICollection IDictionary.Values
        {
            get
            {
                EnsureDictionary();
                IList<JsonData> values = new List<JsonData>();

                foreach (KeyValuePair<string, JsonData> entry in
                         object_list)
                {
                    values.Add(entry.Value);
                }

                return (ICollection)values;
            }
        }
        #endregion



        #region IJsonWrapper Properties
        bool IJsonWrapper.IsArray => IsArray;

        bool IJsonWrapper.IsBoolean => IsBoolean;

        bool IJsonWrapper.IsDouble => IsDouble;

        bool IJsonWrapper.IsInt => IsInt;

        bool IJsonWrapper.IsLong => IsLong;

        bool IJsonWrapper.IsObject => IsObject;

        bool IJsonWrapper.IsString => IsString;
        #endregion


        #region IList Properties
        bool IList.IsFixedSize => EnsureList().IsFixedSize;

        bool IList.IsReadOnly => EnsureList().IsReadOnly;
        #endregion


        #region IDictionary Indexer
        object IDictionary.this[object key]
        {
            get
            {
                return EnsureDictionary()[key];
            }

            set
            {
                if (!(key is String))
                    throw new ArgumentException(
                        "The key has to be a string");

                JsonData data = ToJsonData(value);

                this[(string)key] = data;
            }
        }
        #endregion


        #region IList Indexer
        object IList.this[int index]
        {
            get
            {
                return EnsureList()[index];
            }

            set
            {
                EnsureList();
                JsonData data = ToJsonData(value);

                this[index] = data;
            }
        }
        #endregion


        #region Public Indexers
        public JsonData this[string prop_name]
        {
            get
            {
                EnsureDictionary();
                return inst_object[prop_name];
            }

            set
            {
                EnsureDictionary();

                var entry =
                    new KeyValuePair<string, JsonData>(prop_name, value);

                if (inst_object.ContainsKey(prop_name))
                {
                    for (int i = 0; i < object_list.Count; i++)
                    {
                        if (object_list[i].Key == prop_name)
                        {
                            object_list[i] = entry;
                            break;
                        }
                    }
                }
                else
                    object_list.Add(entry);

                inst_object[prop_name] = value;

                json = null;
            }
        }

        public JsonData this[int index]
        {
            get
            {
                EnsureCollection();

                if (type == JsonType.Array)
                    return inst_array[index];

                return object_list[index].Value;
            }

            set
            {
                EnsureCollection();

                if (type == JsonType.Array)
                    inst_array[index] = value;
                else
                {
                    KeyValuePair<string, JsonData> entry = object_list[index];
                    var new_entry =
                        new KeyValuePair<string, JsonData>(entry.Key, value);

                    object_list[index] = new_entry;
                    inst_object[entry.Key] = value;
                }

                json = null;
            }
        }
        #endregion


        #region Constructors
        public JsonData()
        {
        }

        public JsonData(bool boolean)
        {
            type = JsonType.Boolean;
            inst_boolean = boolean;
        }

        public JsonData(double number)
        {
            type = JsonType.Double;
            inst_double = number;
        }

        public JsonData(int number)
        {
            type = JsonType.Int;
            inst_int = number;
        }

        public JsonData(long number)
        {
            type = JsonType.Long;
            inst_long = number;
        }

        public JsonData(object obj)
        {
            if (obj is Boolean)
            {
                type = JsonType.Boolean;
                inst_boolean = (bool)obj;
                return;
            }

            if (obj is Double)
            {
                type = JsonType.Double;
                inst_double = (double)obj;
                return;
            }

            if (obj is Int32)
            {
                type = JsonType.Int;
                inst_int = (int)obj;
                return;
            }

            if (obj is Int64)
            {
                type = JsonType.Long;
                inst_long = (long)obj;
                return;
            }

            if (obj is String)
            {
                type = JsonType.String;
                inst_string = (string)obj;
                return;
            }

            throw new ArgumentException(
                "Unable to wrap the given object with JsonData");
        }

        public JsonData(string str)
        {
            type = JsonType.String;
            inst_string = str;
        }
        #endregion


        #region Implicit Conversions
        public static implicit operator JsonData(Boolean data) => new JsonData(data);

        public static implicit operator JsonData(Double data) => new JsonData(data);

        public static implicit operator JsonData(Int32 data) => new JsonData(data);

        public static implicit operator JsonData(Int64 data) => new JsonData(data);

        public static implicit operator JsonData(String data) => new JsonData(data);
        #endregion


        #region Explicit Conversions
        public static explicit operator Boolean(JsonData data)
        {
            if (data.type != JsonType.Boolean)
                throw new InvalidCastException(
                    "Instance of JsonData doesn't hold a double");

            return data.inst_boolean;
        }

        public static explicit operator Double(JsonData data)
        {
            if (data.type != JsonType.Double)
                throw new InvalidCastException(
                    "Instance of JsonData doesn't hold a double");

            return data.inst_double;
        }

        public static explicit operator Int32(JsonData data)
        {
            if (data.type != JsonType.Int)
                throw new InvalidCastException(
                    "Instance of JsonData doesn't hold an int");

            return data.inst_int;
        }

        public static explicit operator Int64(JsonData data)
        {
            if (data.type != JsonType.Long)
                throw new InvalidCastException(
                    "Instance of JsonData doesn't hold an int");

            return data.inst_long;
        }

        public static explicit operator String(JsonData data)
        {
            if (data.type != JsonType.String)
                throw new InvalidCastException(
                    "Instance of JsonData doesn't hold a string");

            return data.inst_string;
        }
        #endregion


        #region ICollection Methods
        void ICollection.CopyTo(Array array, int index)
        {
            EnsureCollection().CopyTo(array, index);
        }
        #endregion


        #region IDictionary Methods
        void IDictionary.Add(object key, object value)
        {
            JsonData data = ToJsonData(value);

            EnsureDictionary().Add(key, data);

            var entry =
                new KeyValuePair<string, JsonData>((string)key, data);
            object_list.Add(entry);

            json = null;
        }

        void IDictionary.Clear()
        {
            EnsureDictionary().Clear();
            object_list.Clear();
            json = null;
        }

        bool IDictionary.Contains(object key) => EnsureDictionary().Contains(key);

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            EnsureDictionary();

            return new OrderedDictionaryEnumerator(
                object_list.GetEnumerator());
        }

        void IDictionary.Remove(object key)
        {
            EnsureDictionary().Remove(key);

            for (int i = 0; i < object_list.Count; i++)
            {
                if (object_list[i].Key == (string)key)
                {
                    object_list.RemoveAt(i);
                    break;
                }
            }

            json = null;
        }
        #endregion


        #region IEnumerable Methods
        IEnumerator IEnumerable.GetEnumerator() => EnsureCollection().GetEnumerator();
        #endregion


        #region IJsonWrapper Methods
        bool IJsonWrapper.GetBoolean()
        {
            if (type != JsonType.Boolean)
                throw new InvalidOperationException(
                    "JsonData instance doesn't hold a boolean");

            return inst_boolean;
        }

        double IJsonWrapper.GetDouble()
        {
            if (type != JsonType.Double)
                throw new InvalidOperationException(
                    "JsonData instance doesn't hold a double");

            return inst_double;
        }

        int IJsonWrapper.GetInt()
        {
            if (type != JsonType.Int)
                throw new InvalidOperationException(
                    "JsonData instance doesn't hold an int");

            return inst_int;
        }

        long IJsonWrapper.GetLong()
        {
            if (type != JsonType.Long)
                throw new InvalidOperationException(
                    "JsonData instance doesn't hold a long");

            return inst_long;
        }

        string IJsonWrapper.GetString()
        {
            if (type != JsonType.String)
                throw new InvalidOperationException(
                    "JsonData instance doesn't hold a string");

            return inst_string;
        }

        void IJsonWrapper.SetBoolean(bool val)
        {
            type = JsonType.Boolean;
            inst_boolean = val;
            json = null;
        }

        void IJsonWrapper.SetDouble(double val)
        {
            type = JsonType.Double;
            inst_double = val;
            json = null;
        }

        void IJsonWrapper.SetInt(int val)
        {
            type = JsonType.Int;
            inst_int = val;
            json = null;
        }

        void IJsonWrapper.SetLong(long val)
        {
            type = JsonType.Long;
            inst_long = val;
            json = null;
        }

        void IJsonWrapper.SetString(string val)
        {
            type = JsonType.String;
            inst_string = val;
            json = null;
        }

        string IJsonWrapper.ToJson() => ToJson();

        void IJsonWrapper.ToJson(JsonWriter writer)
        {
            ToJson(writer);
        }
        #endregion


        #region IList Methods
        int IList.Add(object value) => Add(value);

        void IList.Clear()
        {
            EnsureList().Clear();
            json = null;
        }

        bool IList.Contains(object value) => EnsureList().Contains(value);

        int IList.IndexOf(object value) => EnsureList().IndexOf(value);

        void IList.Insert(int index, object value)
        {
            EnsureList().Insert(index, value);
            json = null;
        }

        void IList.Remove(object value)
        {
            EnsureList().Remove(value);
            json = null;
        }

        void IList.RemoveAt(int index)
        {
            EnsureList().RemoveAt(index);
            json = null;
        }
        #endregion


        #region Private Methods
        ICollection EnsureCollection()
        {
            if (type == JsonType.Array)
                return (ICollection)inst_array;

            if (type == JsonType.Object)
                return (ICollection)inst_object;

            throw new InvalidOperationException(
                "The JsonData instance has to be initialized first");
        }

        IDictionary EnsureDictionary()
        {
            if (type == JsonType.Object)
                return (IDictionary)inst_object;

            if (type != JsonType.None)
                throw new InvalidOperationException(
                    "Instance of JsonData is not a dictionary");

            type = JsonType.Object;
            inst_object = new Dictionary<string, JsonData>();
            object_list = new List<KeyValuePair<string, JsonData>>();

            return (IDictionary)inst_object;
        }

        IList EnsureList()
        {
            if (type == JsonType.Array)
                return (IList)inst_array;

            if (type != JsonType.None)
                throw new InvalidOperationException(
                    "Instance of JsonData is not a list");

            type = JsonType.Array;
            inst_array = new List<JsonData>();

            return (IList)inst_array;
        }

        JsonData ToJsonData(object obj)
        {
            if (obj == null)
                return null;

            if (obj is JsonData)
                return (JsonData)obj;

            return new JsonData(obj);
        }

        static void WriteJson(IJsonWrapper obj, JsonWriter writer)
        {
            if (obj == null)
            {
                writer.Write(null);
                return;
            }

            if (obj.IsString)
            {
                writer.Write(obj.GetString());
                return;
            }

            if (obj.IsBoolean)
            {
                writer.Write(obj.GetBoolean());
                return;
            }

            if (obj.IsDouble)
            {
                writer.Write(obj.GetDouble());
                return;
            }

            if (obj.IsInt)
            {
                writer.Write(obj.GetInt());
                return;
            }

            if (obj.IsLong)
            {
                writer.Write(obj.GetLong());
                return;
            }

            if (obj.IsArray)
            {
                writer.WriteArrayStart();
                foreach (object elem in (IList)obj)
                    WriteJson((JsonData)elem, writer);
                writer.WriteArrayEnd();

                return;
            }

            if (obj.IsObject)
            {
                writer.WriteObjectStart();

                foreach (DictionaryEntry entry in ((IDictionary)obj))
                {
                    writer.WritePropertyName((string)entry.Key);
                    WriteJson((JsonData)entry.Value, writer);
                }
                writer.WriteObjectEnd();

                return;
            }
        }
        #endregion


        public int Add(object value)
        {
            JsonData data = ToJsonData(value);

            json = null;

            return EnsureList().Add(data);
        }

        public void Clear()
        {
            if (IsObject)
            {
                ((IDictionary)this).Clear();
                return;
            }

            if (IsArray)
            {
                ((IList)this).Clear();
                return;
            }
        }

        public bool Equals(JsonData x)
        {
            if (x == null)
                return false;

            if (x.type != this.type)
                return false;

            switch (this.type)
            {
                case JsonType.None:
                    return true;

                case JsonType.Object:
                    return this.inst_object.Equals(x.inst_object);

                case JsonType.Array:
                    return this.inst_array.Equals(x.inst_array);

                case JsonType.String:
                    return this.inst_string.Equals(x.inst_string);

                case JsonType.Int:
                    return this.inst_int.Equals(x.inst_int);

                case JsonType.Long:
                    return this.inst_long.Equals(x.inst_long);

                case JsonType.Double:
                    return this.inst_double.Equals(x.inst_double);

                case JsonType.Boolean:
                    return this.inst_boolean.Equals(x.inst_boolean);
            }

            return false;
        }

        public JsonType GetJsonType() => type;

        public void SetJsonType(JsonType type)
        {
            if (this.type == type)
                return;

            switch (type)
            {
                case JsonType.None:
                    break;

                case JsonType.Object:
                    inst_object = new Dictionary<string, JsonData>();
                    object_list = new List<KeyValuePair<string, JsonData>>();
                    break;

                case JsonType.Array:
                    inst_array = new List<JsonData>();
                    break;

                case JsonType.String:
                    inst_string = default(String);
                    break;

                case JsonType.Int:
                    inst_int = default(Int32);
                    break;

                case JsonType.Long:
                    inst_long = default(Int64);
                    break;

                case JsonType.Double:
                    inst_double = default(Double);
                    break;

                case JsonType.Boolean:
                    inst_boolean = default(Boolean);
                    break;
            }

            this.type = type;
        }

        public string ToJson()
        {
            if (json != null)
                return json;

            var sw = new StringWriter();
            var writer = new JsonWriter(sw);
            writer.Validate = false;

            WriteJson(this, writer);
            json = sw.ToString();

            return json;
        }

        public void ToJson(JsonWriter writer)
        {
            bool old_validate = writer.Validate;

            writer.Validate = false;

            WriteJson(this, writer);

            writer.Validate = old_validate;
        }

        public override string ToString()
        {
            switch (type)
            {
                case JsonType.Array:
                    return "JsonData array";

                case JsonType.Boolean:
                    return inst_boolean.ToString();

                case JsonType.Double:
                    return inst_double.ToString();

                case JsonType.Int:
                    return inst_int.ToString();

                case JsonType.Long:
                    return inst_long.ToString();

                case JsonType.Object:
                    return "JsonData object";

                case JsonType.String:
                    return inst_string;
            }

            return "Uninitialized JsonData";
        }

        public Type NetType()
        {
            switch (type)
            {
                case JsonType.None:
                case JsonType.Object:
                    return typeof(object);
                case JsonType.Array:
                    return typeof(JsonData[]);
                case JsonType.Boolean:
                    return typeof(bool);
                case JsonType.Double:
                    return typeof(double);
                case JsonType.Int:
                    return typeof(int);
                case JsonType.Long:
                    return typeof(long);
                case JsonType.String:
                    return typeof(string);
                default:
                    throw new InvalidOperationException();
            }
        }
        public Type NetType(Type to)
        {
            switch (type)
            {
                case JsonType.None:
                    if (!to.IsClass && !to.IsArray)
                        throw new ArgumentException();

                    return to;
                case JsonType.Object:
                    return to;
                case JsonType.Array:
                    if (!to.IsArray && Array.IndexOf(to.GetInterfaces(), typeof(IList)) == -1)
                        throw new ArgumentException();

                    return to;
                //case JsonType.Boolean:
                //    return typeof(bool);
                //case JsonType.Double:
                //    return typeof(double);
                //case JsonType.Int:
                //    return typeof(int);
                //case JsonType.Long:
                //    return typeof(long);
                //case JsonType.String:
                //    return typeof(string);
                default:
                    return to;
            }
        }
    }

    class OrderedDictionaryEnumerator : IDictionaryEnumerator
    {
        readonly IEnumerator<KeyValuePair<string, JsonData>> list_enumerator;

        public object Current => Entry;

        public DictionaryEntry Entry
        {
            get
            {
                KeyValuePair<string, JsonData> curr = list_enumerator.Current;
                return new DictionaryEntry(curr.Key, curr.Value);
            }
        }

        public object Key => list_enumerator.Current.Key;

        public object Value => list_enumerator.Current.Value;

        public OrderedDictionaryEnumerator(
            IEnumerator<KeyValuePair<string, JsonData>> enumerator)
        {
            list_enumerator = enumerator;
        }

        public bool MoveNext() => list_enumerator.MoveNext();

        public void Reset()
        {
            list_enumerator.Reset();
        }
    }
}
