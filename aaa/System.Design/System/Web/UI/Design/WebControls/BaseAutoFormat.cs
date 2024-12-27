using System;
using System.ComponentModel;
using System.Data;
using System.Design;
using System.Globalization;
using System.Reflection;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x020003EA RID: 1002
	internal class BaseAutoFormat : DesignerAutoFormat
	{
		// Token: 0x06002506 RID: 9478 RVA: 0x000C6EF0 File Offset: 0x000C5EF0
		public BaseAutoFormat(DataRow schemeData)
			: base(SR.GetString(schemeData["SchemeName"].ToString()))
		{
			this._schemeData = schemeData;
		}

		// Token: 0x06002507 RID: 9479 RVA: 0x000C6F14 File Offset: 0x000C5F14
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

		// Token: 0x06002508 RID: 9480 RVA: 0x000C6FE8 File Offset: 0x000C5FE8
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

		// Token: 0x06002509 RID: 9481 RVA: 0x000C704C File Offset: 0x000C604C
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

		// Token: 0x0600250A RID: 9482 RVA: 0x000C7118 File Offset: 0x000C6118
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

		// Token: 0x0400196F RID: 6511
		private const char PERSIST_CHAR = '-';

		// Token: 0x04001970 RID: 6512
		private const char OM_CHAR = '.';

		// Token: 0x04001971 RID: 6513
		private DataRow _schemeData;

		// Token: 0x020003EB RID: 1003
		private struct InstanceAndPropertyInfo
		{
			// Token: 0x0600250B RID: 9483 RVA: 0x000C71CC File Offset: 0x000C61CC
			public InstanceAndPropertyInfo(object instance, PropertyInfo propertyInfo)
			{
				this.Instance = instance;
				this.PropertyInfo = propertyInfo;
			}

			// Token: 0x04001972 RID: 6514
			public object Instance;

			// Token: 0x04001973 RID: 6515
			public PropertyInfo PropertyInfo;
		}
	}
}
