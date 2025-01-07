using System;
using System.ComponentModel;
using System.Data;
using System.Design;
using System.Globalization;
using System.Reflection;

namespace System.Web.UI.Design.WebControls
{
	internal class BaseAutoFormat : DesignerAutoFormat
	{
		public BaseAutoFormat(DataRow schemeData)
			: base(SR.GetString(schemeData["SchemeName"].ToString()))
		{
			this._schemeData = schemeData;
		}

		public override void Apply(Control control)
		{
			foreach (object obj in this._schemeData.Table.Columns)
			{
				DataColumn dataColumn = (DataColumn)obj;
				string columnName = dataColumn.ColumnName;
				if (!string.Equals(columnName, "SchemeName", StringComparison.Ordinal))
				{
					if (columnName.EndsWith("--ClearDefaults", StringComparison.Ordinal))
					{
						if (this._schemeData[columnName].ToString().Equals("true", StringComparison.OrdinalIgnoreCase))
						{
							this.ClearDefaults(control, columnName.Substring(0, columnName.Length - 15));
						}
					}
					else
					{
						this.SetPropertyValue(control, columnName, this._schemeData[columnName].ToString());
					}
				}
			}
		}

		private void ClearDefaults(Control control, string propertyName)
		{
			BaseAutoFormat.InstanceAndPropertyInfo memberInfo = BaseAutoFormat.GetMemberInfo(control, propertyName);
			if (memberInfo.PropertyInfo != null && memberInfo.Instance != null)
			{
				object value = memberInfo.PropertyInfo.GetValue(memberInfo.Instance, null);
				Type type = value.GetType();
				type.InvokeMember("ClearDefaults", BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod, null, value, new object[0], CultureInfo.InvariantCulture);
			}
		}

		private static BaseAutoFormat.InstanceAndPropertyInfo GetMemberInfo(Control control, string name)
		{
			Type type = control.GetType();
			PropertyInfo propertyInfo = null;
			object obj = control;
			object obj2 = control;
			string text = name.Replace('-', '.');
			int i = 0;
			while (i < text.Length)
			{
				int num = text.IndexOf('.', i);
				string text2;
				if (num < 0)
				{
					text2 = text.Substring(i);
					i = text.Length;
				}
				else
				{
					text2 = text.Substring(i, num - i);
					i = num + 1;
				}
				BindingFlags bindingFlags = BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;
				try
				{
					propertyInfo = type.GetProperty(text2, bindingFlags);
				}
				catch (AmbiguousMatchException)
				{
					bindingFlags |= BindingFlags.DeclaredOnly;
					propertyInfo = type.GetProperty(text2, bindingFlags);
				}
				if (propertyInfo != null)
				{
					type = propertyInfo.PropertyType;
					if (obj2 != null)
					{
						obj = obj2;
						obj2 = propertyInfo.GetValue(obj, null);
					}
				}
			}
			return new BaseAutoFormat.InstanceAndPropertyInfo(obj, propertyInfo);
		}

		protected void SetPropertyValue(Control control, string propertyName, string propertyValue)
		{
			object obj = null;
			BaseAutoFormat.InstanceAndPropertyInfo memberInfo = BaseAutoFormat.GetMemberInfo(control, propertyName);
			PropertyInfo propertyInfo = memberInfo.PropertyInfo;
			TypeConverter typeConverter = null;
			TypeConverterAttribute typeConverterAttribute = Attribute.GetCustomAttribute(propertyInfo, typeof(TypeConverterAttribute), true) as TypeConverterAttribute;
			if (typeConverterAttribute != null)
			{
				Type type = Type.GetType(typeConverterAttribute.ConverterTypeName, false);
				if (type != null)
				{
					typeConverter = (TypeConverter)Activator.CreateInstance(type);
				}
			}
			if (typeConverter != null && typeConverter.CanConvertFrom(typeof(string)))
			{
				obj = typeConverter.ConvertFromInvariantString(propertyValue);
			}
			else
			{
				typeConverter = TypeDescriptor.GetConverter(propertyInfo.PropertyType);
				if (typeConverter != null && typeConverter.CanConvertFrom(typeof(string)))
				{
					obj = typeConverter.ConvertFromInvariantString(propertyValue);
				}
			}
			propertyInfo.SetValue(memberInfo.Instance, obj, null);
		}

		private const char PERSIST_CHAR = '-';

		private const char OM_CHAR = '.';

		private DataRow _schemeData;

		private struct InstanceAndPropertyInfo
		{
			public InstanceAndPropertyInfo(object instance, PropertyInfo propertyInfo)
			{
				this.Instance = instance;
				this.PropertyInfo = propertyInfo;
			}

			public object Instance;

			public PropertyInfo PropertyInfo;
		}
	}
}
