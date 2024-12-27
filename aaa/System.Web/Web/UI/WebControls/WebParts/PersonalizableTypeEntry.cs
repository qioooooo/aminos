using System;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006DB RID: 1755
	internal sealed class PersonalizableTypeEntry
	{
		// Token: 0x0600561E RID: 22046 RVA: 0x0015BF4C File Offset: 0x0015AF4C
		public PersonalizableTypeEntry(Type type)
		{
			this._type = type;
			this.InitializePersonalizableProperties();
		}

		// Token: 0x1700163F RID: 5695
		// (get) Token: 0x0600561F RID: 22047 RVA: 0x0015BF61 File Offset: 0x0015AF61
		public IDictionary PropertyEntries
		{
			get
			{
				return this._propertyEntries;
			}
		}

		// Token: 0x17001640 RID: 5696
		// (get) Token: 0x06005620 RID: 22048 RVA: 0x0015BF6C File Offset: 0x0015AF6C
		public ICollection PropertyInfos
		{
			get
			{
				if (this._propertyInfos == null)
				{
					PropertyInfo[] array = new PropertyInfo[this._propertyEntries.Count];
					int num = 0;
					foreach (object obj in this._propertyEntries.Values)
					{
						PersonalizablePropertyEntry personalizablePropertyEntry = (PersonalizablePropertyEntry)obj;
						array[num] = personalizablePropertyEntry.PropertyInfo;
						num++;
					}
					this._propertyInfos = array;
				}
				return this._propertyInfos;
			}
		}

		// Token: 0x06005621 RID: 22049 RVA: 0x0015BFFC File Offset: 0x0015AFFC
		private void InitializePersonalizableProperties()
		{
			this._propertyEntries = new HybridDictionary(false);
			BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
			PropertyInfo[] properties = this._type.GetProperties(bindingFlags);
			Array.Sort(properties, new PersonalizableTypeEntry.DeclaringTypeComparer());
			if (properties != null && properties.Length != 0)
			{
				foreach (PropertyInfo propertyInfo in properties)
				{
					string name = propertyInfo.Name;
					PersonalizableAttribute personalizableAttribute = Attribute.GetCustomAttribute(propertyInfo, PersonalizableAttribute.PersonalizableAttributeType, true) as PersonalizableAttribute;
					if (personalizableAttribute == null || !personalizableAttribute.IsPersonalizable)
					{
						this._propertyEntries.Remove(name);
					}
					else
					{
						ParameterInfo[] indexParameters = propertyInfo.GetIndexParameters();
						if ((indexParameters != null && indexParameters.Length > 0) || propertyInfo.GetGetMethod() == null || propertyInfo.GetSetMethod() == null)
						{
							throw new HttpException(SR.GetString("PersonalizableTypeEntry_InvalidProperty", new object[]
							{
								name,
								this._type.FullName
							}));
						}
						this._propertyEntries[name] = new PersonalizablePropertyEntry(propertyInfo, personalizableAttribute);
					}
				}
			}
		}

		// Token: 0x04002F4C RID: 12108
		private Type _type;

		// Token: 0x04002F4D RID: 12109
		private IDictionary _propertyEntries;

		// Token: 0x04002F4E RID: 12110
		private PropertyInfo[] _propertyInfos;

		// Token: 0x020006DC RID: 1756
		private sealed class DeclaringTypeComparer : IComparer
		{
			// Token: 0x06005622 RID: 22050 RVA: 0x0015C0F4 File Offset: 0x0015B0F4
			public int Compare(object x, object y)
			{
				Type declaringType = ((PropertyInfo)x).DeclaringType;
				Type declaringType2 = ((PropertyInfo)y).DeclaringType;
				if (declaringType == declaringType2)
				{
					return 0;
				}
				if (declaringType.IsSubclassOf(declaringType2))
				{
					return 1;
				}
				return -1;
			}
		}
	}
}
