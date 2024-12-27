using System;
using System.Collections;
using System.Collections.Specialized;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Configuration
{
	// Token: 0x02000084 RID: 132
	[Serializable]
	public sealed class PropertyInformationCollection : NameObjectCollectionBase
	{
		// Token: 0x060004F0 RID: 1264 RVA: 0x000191D0 File Offset: 0x000181D0
		internal PropertyInformationCollection(ConfigurationElement thisElement)
			: base(StringComparer.Ordinal)
		{
			this.ThisElement = thisElement;
			foreach (object obj in this.ThisElement.Properties)
			{
				ConfigurationProperty configurationProperty = (ConfigurationProperty)obj;
				if (configurationProperty.Name != this.ThisElement.ElementTagName)
				{
					base.BaseAdd(configurationProperty.Name, new PropertyInformation(thisElement, configurationProperty.Name));
				}
			}
			base.IsReadOnly = true;
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x00019270 File Offset: 0x00018270
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
		}

		// Token: 0x17000167 RID: 359
		public PropertyInformation this[string propertyName]
		{
			get
			{
				PropertyInformation propertyInformation = (PropertyInformation)base.BaseGet(propertyName);
				if (propertyInformation == null)
				{
					PropertyInformation propertyInformation2 = (PropertyInformation)base.BaseGet(ConfigurationProperty.DefaultCollectionPropertyName);
					if (propertyInformation2 != null && propertyInformation2.ProvidedName == propertyName)
					{
						propertyInformation = propertyInformation2;
					}
				}
				return propertyInformation;
			}
		}

		// Token: 0x17000168 RID: 360
		internal PropertyInformation this[int index]
		{
			get
			{
				return (PropertyInformation)base.BaseGet(base.BaseGetKey(index));
			}
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x000192D4 File Offset: 0x000182D4
		public void CopyTo(PropertyInformation[] array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Length < this.Count + index)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			foreach (object obj in this)
			{
				PropertyInformation propertyInformation = (PropertyInformation)obj;
				array[index++] = propertyInformation;
			}
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x00019404 File Offset: 0x00018404
		public override IEnumerator GetEnumerator()
		{
			int c = this.Count;
			for (int i = 0; i < c; i++)
			{
				yield return this[i];
			}
			yield break;
		}

		// Token: 0x04000362 RID: 866
		private ConfigurationElement ThisElement;
	}
}
