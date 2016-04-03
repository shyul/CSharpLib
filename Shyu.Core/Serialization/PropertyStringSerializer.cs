using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using System.Drawing;
using Shyu.Core;

namespace Shyu.Serialization
{
    public static class PropertyStringSerializer
    {
        public const string ObjectNameColumnName = "OBJECTNAME";
        public const string ItemNameColumnName = "ITEMNAME";
        public const string PropertyStringColumnName = "PROPERTIES";

        public static DataTable InitSerializerTable(string Name)
        {
            DataTable t = new DataTable();

            if (Name == string.Empty) Name = "SerTable";

            t.TableName = Name;

            uTable.AddColumn(t, ObjectNameColumnName, typeof(string));
            uTable.AddColumn(t, ItemNameColumnName, typeof(string));
            uTable.AddColumn(t, PropertyStringColumnName, typeof(string));

            t.AcceptChanges();

            return t;
        }
        public static void AddObjectToTable(DataTable t, object obj)
        {
            string[] res = GetPropertyString(obj);

            if (res != null)
            {
                DataRow Row = t.NewRow();
                Row[ObjectNameColumnName] = obj.GetType().FullName;
                Row[ItemNameColumnName] = res[0];
                Row[PropertyStringColumnName] = res[1];
                t.Rows.Add(Row);
                t.AcceptChanges();
            }
        }
        public static void AddObjectToTable(DataTable t, object obj, string ItemName)
        {
            string[] res = GetPropertyString(obj);

            if (res != null)
            {
                DataRow Row = t.NewRow();
                Row[ObjectNameColumnName] = obj.GetType().FullName;
                Row[ItemNameColumnName] = ItemName;
                Row[PropertyStringColumnName] = res[1];
                t.Rows.Add(Row);
                t.AcceptChanges();
            }
        }
        public static object LoadObjectFromTable(Assembly asm, DataTable t, int Pt)
        {
            object o = asm.CreateInstance(t.Rows[Pt][ObjectNameColumnName].ToString());
            //object o = Assembly.GetExecutingAssembly().CreateInstance(t.Rows[Pt][Serializer.ObjectNameColumnName].ToString());
            ApplyPropertyString(o, t.Rows[Pt][PropertyStringColumnName].ToString());
            return o;
        }
        public static void ApplyPropertyString(object obj, string p)
        {
            PropertyString u = new PropertyString(p);
            PropertyInfo[] Properties = obj.GetType().GetProperties();
            foreach (PropertyInfo Property in Properties)
            {
                if (Attribute.IsDefined(Property, typeof(uProperty)))
                {
                    if (Property.PropertyType == typeof(string))
                    {
                        Property.SetValue(obj, u.GetString(Property.Name));
                    }
                    else if (Property.PropertyType == typeof(byte))
                    {
                        Property.SetValue(obj, (byte)u.GetByte(Property.Name));
                    }
                    else if (Property.PropertyType == typeof(int))
                    {
                        Property.SetValue(obj, u.GetInt32(Property.Name));
                    }
                    else if (Property.PropertyType == typeof(long))
                    {
                        Property.SetValue(obj, u.GetInt64(Property.Name));
                    }
                    else if (Property.PropertyType == typeof(double))
                    {
                        Property.SetValue(obj, u.GetDouble(Property.Name));
                    }
                    else if (Property.PropertyType == typeof(double[]))
                    {
                        Property.SetValue(obj, u.GetDoubleArray(Property.Name));
                    }
                    else if (Property.PropertyType == typeof(float))
                    {
                        Property.SetValue(obj, u.GetFloat(Property.Name));
                    }
                    else if (Property.PropertyType == typeof(float[]))
                    {
                        Property.SetValue(obj, u.GetFloatArray(Property.Name));
                    }
                    else if (Property.PropertyType == typeof(bool))
                    {
                        Property.SetValue(obj, u.GetBoolean(Property.Name));
                    }
                    else if (Property.PropertyType == typeof(Color))
                    {
                        Property.SetValue(obj, u.GetColor(Property.Name));
                    }
                    else if (Property.PropertyType == typeof(Font))
                    {
                        Property.SetValue(obj, u.GetFont(Property.Name));
                    }
                    else
                    {
                        Property.SetValue(obj, u.GetInt32(Property.Name)); //Property.SetValue(obj, Convert.ChangeType(u.GetInt32(Property.Name), Property.PropertyType));
                    }
                }
            }
        }

        public static string[] GetPropertyString(object obj)
        {
            PropertyInfo[] Properties = obj.GetType().GetProperties();

            if (Properties != null)
            {
                PropertyString u = new PropertyString();
                foreach (PropertyInfo Property in Properties)
                {
                    bool IsSaved = Attribute.IsDefined(Property, typeof(uProperty));
                    if (IsSaved)
                    {
                        if (Property.PropertyType == typeof(string))
                        {
                            u.SetString(Property.Name, (string)Property.GetValue(obj, null));
                        }
                        else if (Property.PropertyType == typeof(int))
                        {
                            u.SetInt32(Property.Name, (int)Property.GetValue(obj, null));
                        }
                        else if (Property.PropertyType == typeof(long))
                        {
                            u.SetInt64(Property.Name, (long)Property.GetValue(obj, null));
                        }
                        else if (Property.PropertyType == typeof(double))
                        {
                            u.SetDouble(Property.Name, (double)Property.GetValue(obj, null));
                        }
                        else if (Property.PropertyType == typeof(double[]))
                        {
                            u.SetDoubleArray(Property.Name, (double[])Property.GetValue(obj, null));
                        }
                        else if (Property.PropertyType == typeof(float))
                        {
                            u.SetFloat(Property.Name, (float)Property.GetValue(obj, null));
                        }
                        else if (Property.PropertyType == typeof(float[]))
                        {
                            u.SetFloatArray(Property.Name, (float[])Property.GetValue(obj, null));
                        }
                        else if (Property.PropertyType == typeof(bool))
                        {
                            u.SetBoolean(Property.Name, (bool)Property.GetValue(obj, null));
                        }
                        else if (Property.PropertyType == typeof(Color))
                        {
                            u.SetColor(Property.Name, (Color)Property.GetValue(obj, null));
                        }
                        else if (Property.PropertyType == typeof(Font))
                        {
                            u.SetFont(Property.Name, (Font)Property.GetValue(obj, null));
                        }
                        else
                        {
                            u.SetVar(Property.Name, Property.GetValue(obj, null));
                        }
                    }
                }
                return new string[] { u.GetString("ItemName"), u.ToString() };
            }
            else
                return null;
        }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class uProperty : Attribute { }
}
