using System;
using System.Collections;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x0200008E RID: 142
	[ComVisible(true)]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class TypeConverter
	{
		// Token: 0x060004F2 RID: 1266 RVA: 0x00015A8A File Offset: 0x00014A8A
		public bool CanConvertFrom(Type sourceType)
		{
			return this.CanConvertFrom(null, sourceType);
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x00015A94 File Offset: 0x00014A94
		public virtual bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(InstanceDescriptor);
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x00015AA6 File Offset: 0x00014AA6
		public bool CanConvertTo(Type destinationType)
		{
			return this.CanConvertTo(null, destinationType);
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x00015AB0 File Offset: 0x00014AB0
		public virtual bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(string);
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x00015ABF File Offset: 0x00014ABF
		public object ConvertFrom(object value)
		{
			return this.ConvertFrom(null, CultureInfo.CurrentCulture, value);
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x00015AD0 File Offset: 0x00014AD0
		public virtual object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			InstanceDescriptor instanceDescriptor = value as InstanceDescriptor;
			if (instanceDescriptor != null)
			{
				return instanceDescriptor.Invoke();
			}
			throw this.GetConvertFromException(value);
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x00015AF5 File Offset: 0x00014AF5
		public object ConvertFromInvariantString(string text)
		{
			return this.ConvertFromString(null, CultureInfo.InvariantCulture, text);
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x00015B04 File Offset: 0x00014B04
		public object ConvertFromInvariantString(ITypeDescriptorContext context, string text)
		{
			return this.ConvertFromString(context, CultureInfo.InvariantCulture, text);
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x00015B13 File Offset: 0x00014B13
		public object ConvertFromString(string text)
		{
			return this.ConvertFrom(null, null, text);
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x00015B1E File Offset: 0x00014B1E
		public object ConvertFromString(ITypeDescriptorContext context, string text)
		{
			return this.ConvertFrom(context, CultureInfo.CurrentCulture, text);
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x00015B2D File Offset: 0x00014B2D
		public object ConvertFromString(ITypeDescriptorContext context, CultureInfo culture, string text)
		{
			return this.ConvertFrom(context, culture, text);
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x00015B38 File Offset: 0x00014B38
		public object ConvertTo(object value, Type destinationType)
		{
			return this.ConvertTo(null, null, value, destinationType);
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x00015B44 File Offset: 0x00014B44
		public virtual object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType != typeof(string))
			{
				throw this.GetConvertToException(value, destinationType);
			}
			if (value == null)
			{
				return string.Empty;
			}
			if (culture != null && culture != CultureInfo.CurrentCulture)
			{
				IFormattable formattable = value as IFormattable;
				if (formattable != null)
				{
					return formattable.ToString(null, culture);
				}
			}
			return value.ToString();
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x00015BA5 File Offset: 0x00014BA5
		public string ConvertToInvariantString(object value)
		{
			return this.ConvertToString(null, CultureInfo.InvariantCulture, value);
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x00015BB4 File Offset: 0x00014BB4
		public string ConvertToInvariantString(ITypeDescriptorContext context, object value)
		{
			return this.ConvertToString(context, CultureInfo.InvariantCulture, value);
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x00015BC3 File Offset: 0x00014BC3
		public string ConvertToString(object value)
		{
			return (string)this.ConvertTo(null, CultureInfo.CurrentCulture, value, typeof(string));
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x00015BE1 File Offset: 0x00014BE1
		public string ConvertToString(ITypeDescriptorContext context, object value)
		{
			return (string)this.ConvertTo(context, CultureInfo.CurrentCulture, value, typeof(string));
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x00015BFF File Offset: 0x00014BFF
		public string ConvertToString(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			return (string)this.ConvertTo(context, culture, value, typeof(string));
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x00015C19 File Offset: 0x00014C19
		public object CreateInstance(IDictionary propertyValues)
		{
			return this.CreateInstance(null, propertyValues);
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x00015C23 File Offset: 0x00014C23
		public virtual object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
		{
			return null;
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x00015C28 File Offset: 0x00014C28
		protected Exception GetConvertFromException(object value)
		{
			string text;
			if (value == null)
			{
				text = SR.GetString("ToStringNull");
			}
			else
			{
				text = value.GetType().FullName;
			}
			throw new NotSupportedException(SR.GetString("ConvertFromException", new object[]
			{
				base.GetType().Name,
				text
			}));
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x00015C7C File Offset: 0x00014C7C
		protected Exception GetConvertToException(object value, Type destinationType)
		{
			string text;
			if (value == null)
			{
				text = SR.GetString("ToStringNull");
			}
			else
			{
				text = value.GetType().FullName;
			}
			throw new NotSupportedException(SR.GetString("ConvertToException", new object[]
			{
				base.GetType().Name,
				text,
				destinationType.FullName
			}));
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x00015CD7 File Offset: 0x00014CD7
		public bool GetCreateInstanceSupported()
		{
			return this.GetCreateInstanceSupported(null);
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x00015CE0 File Offset: 0x00014CE0
		public virtual bool GetCreateInstanceSupported(ITypeDescriptorContext context)
		{
			return false;
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x00015CE3 File Offset: 0x00014CE3
		public PropertyDescriptorCollection GetProperties(object value)
		{
			return this.GetProperties(null, value);
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x00015CF0 File Offset: 0x00014CF0
		public PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value)
		{
			return this.GetProperties(context, value, new Attribute[] { BrowsableAttribute.Yes });
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x00015D15 File Offset: 0x00014D15
		public virtual PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			return null;
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x00015D18 File Offset: 0x00014D18
		public bool GetPropertiesSupported()
		{
			return this.GetPropertiesSupported(null);
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x00015D21 File Offset: 0x00014D21
		public virtual bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return false;
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x00015D24 File Offset: 0x00014D24
		public ICollection GetStandardValues()
		{
			return this.GetStandardValues(null);
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x00015D2D File Offset: 0x00014D2D
		public virtual TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			return null;
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x00015D30 File Offset: 0x00014D30
		public bool GetStandardValuesExclusive()
		{
			return this.GetStandardValuesExclusive(null);
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x00015D39 File Offset: 0x00014D39
		public virtual bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return false;
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x00015D3C File Offset: 0x00014D3C
		public bool GetStandardValuesSupported()
		{
			return this.GetStandardValuesSupported(null);
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x00015D45 File Offset: 0x00014D45
		public virtual bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return false;
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x00015D48 File Offset: 0x00014D48
		public bool IsValid(object value)
		{
			return this.IsValid(null, value);
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x00015D52 File Offset: 0x00014D52
		public virtual bool IsValid(ITypeDescriptorContext context, object value)
		{
			return true;
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x00015D55 File Offset: 0x00014D55
		protected PropertyDescriptorCollection SortProperties(PropertyDescriptorCollection props, string[] names)
		{
			props.Sort(names);
			return props;
		}

		// Token: 0x02000091 RID: 145
		protected abstract class SimplePropertyDescriptor : PropertyDescriptor
		{
			// Token: 0x06000550 RID: 1360 RVA: 0x000169A0 File Offset: 0x000159A0
			protected SimplePropertyDescriptor(Type componentType, string name, Type propertyType)
				: this(componentType, name, propertyType, new Attribute[0])
			{
			}

			// Token: 0x06000551 RID: 1361 RVA: 0x000169B1 File Offset: 0x000159B1
			protected SimplePropertyDescriptor(Type componentType, string name, Type propertyType, Attribute[] attributes)
				: base(name, attributes)
			{
				this.componentType = componentType;
				this.propertyType = propertyType;
			}

			// Token: 0x17000104 RID: 260
			// (get) Token: 0x06000552 RID: 1362 RVA: 0x000169CA File Offset: 0x000159CA
			public override Type ComponentType
			{
				get
				{
					return this.componentType;
				}
			}

			// Token: 0x17000105 RID: 261
			// (get) Token: 0x06000553 RID: 1363 RVA: 0x000169D2 File Offset: 0x000159D2
			public override bool IsReadOnly
			{
				get
				{
					return this.Attributes.Contains(ReadOnlyAttribute.Yes);
				}
			}

			// Token: 0x17000106 RID: 262
			// (get) Token: 0x06000554 RID: 1364 RVA: 0x000169E4 File Offset: 0x000159E4
			public override Type PropertyType
			{
				get
				{
					return this.propertyType;
				}
			}

			// Token: 0x06000555 RID: 1365 RVA: 0x000169EC File Offset: 0x000159EC
			public override bool CanResetValue(object component)
			{
				DefaultValueAttribute defaultValueAttribute = (DefaultValueAttribute)this.Attributes[typeof(DefaultValueAttribute)];
				return defaultValueAttribute != null && defaultValueAttribute.Value.Equals(this.GetValue(component));
			}

			// Token: 0x06000556 RID: 1366 RVA: 0x00016A2C File Offset: 0x00015A2C
			public override void ResetValue(object component)
			{
				DefaultValueAttribute defaultValueAttribute = (DefaultValueAttribute)this.Attributes[typeof(DefaultValueAttribute)];
				if (defaultValueAttribute != null)
				{
					this.SetValue(component, defaultValueAttribute.Value);
				}
			}

			// Token: 0x06000557 RID: 1367 RVA: 0x00016A64 File Offset: 0x00015A64
			public override bool ShouldSerializeValue(object component)
			{
				return false;
			}

			// Token: 0x040008C6 RID: 2246
			private Type componentType;

			// Token: 0x040008C7 RID: 2247
			private Type propertyType;
		}

		// Token: 0x02000092 RID: 146
		public class StandardValuesCollection : ICollection, IEnumerable
		{
			// Token: 0x06000558 RID: 1368 RVA: 0x00016A68 File Offset: 0x00015A68
			public StandardValuesCollection(ICollection values)
			{
				if (values == null)
				{
					values = new object[0];
				}
				Array array = values as Array;
				if (array != null)
				{
					this.valueArray = array;
				}
				this.values = values;
			}

			// Token: 0x17000107 RID: 263
			// (get) Token: 0x06000559 RID: 1369 RVA: 0x00016A9E File Offset: 0x00015A9E
			public int Count
			{
				get
				{
					if (this.valueArray != null)
					{
						return this.valueArray.Length;
					}
					return this.values.Count;
				}
			}

			// Token: 0x17000108 RID: 264
			public object this[int index]
			{
				get
				{
					if (this.valueArray != null)
					{
						return this.valueArray.GetValue(index);
					}
					IList list = this.values as IList;
					if (list != null)
					{
						return list[index];
					}
					this.valueArray = new object[this.values.Count];
					this.values.CopyTo(this.valueArray, 0);
					return this.valueArray.GetValue(index);
				}
			}

			// Token: 0x0600055B RID: 1371 RVA: 0x00016B2D File Offset: 0x00015B2D
			public void CopyTo(Array array, int index)
			{
				this.values.CopyTo(array, index);
			}

			// Token: 0x0600055C RID: 1372 RVA: 0x00016B3C File Offset: 0x00015B3C
			public IEnumerator GetEnumerator()
			{
				return this.values.GetEnumerator();
			}

			// Token: 0x17000109 RID: 265
			// (get) Token: 0x0600055D RID: 1373 RVA: 0x00016B49 File Offset: 0x00015B49
			int ICollection.Count
			{
				get
				{
					return this.Count;
				}
			}

			// Token: 0x1700010A RID: 266
			// (get) Token: 0x0600055E RID: 1374 RVA: 0x00016B51 File Offset: 0x00015B51
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x1700010B RID: 267
			// (get) Token: 0x0600055F RID: 1375 RVA: 0x00016B54 File Offset: 0x00015B54
			object ICollection.SyncRoot
			{
				get
				{
					return null;
				}
			}

			// Token: 0x06000560 RID: 1376 RVA: 0x00016B57 File Offset: 0x00015B57
			void ICollection.CopyTo(Array array, int index)
			{
				this.CopyTo(array, index);
			}

			// Token: 0x06000561 RID: 1377 RVA: 0x00016B61 File Offset: 0x00015B61
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x040008C8 RID: 2248
			private ICollection values;

			// Token: 0x040008C9 RID: 2249
			private Array valueArray;
		}
	}
}
