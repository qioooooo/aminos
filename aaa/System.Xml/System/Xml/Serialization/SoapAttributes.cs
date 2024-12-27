using System;
using System.ComponentModel;
using System.Reflection;

namespace System.Xml.Serialization
{
	// Token: 0x020002E9 RID: 745
	public class SoapAttributes
	{
		// Token: 0x060022CF RID: 8911 RVA: 0x000A3B77 File Offset: 0x000A2B77
		public SoapAttributes()
		{
		}

		// Token: 0x060022D0 RID: 8912 RVA: 0x000A3B80 File Offset: 0x000A2B80
		public SoapAttributes(ICustomAttributeProvider provider)
		{
			object[] customAttributes = provider.GetCustomAttributes(false);
			for (int i = 0; i < customAttributes.Length; i++)
			{
				if (customAttributes[i] is SoapIgnoreAttribute || customAttributes[i] is ObsoleteAttribute)
				{
					this.soapIgnore = true;
					break;
				}
				if (customAttributes[i] is SoapElementAttribute)
				{
					this.soapElement = (SoapElementAttribute)customAttributes[i];
				}
				else if (customAttributes[i] is SoapAttributeAttribute)
				{
					this.soapAttribute = (SoapAttributeAttribute)customAttributes[i];
				}
				else if (customAttributes[i] is SoapTypeAttribute)
				{
					this.soapType = (SoapTypeAttribute)customAttributes[i];
				}
				else if (customAttributes[i] is SoapEnumAttribute)
				{
					this.soapEnum = (SoapEnumAttribute)customAttributes[i];
				}
				else if (customAttributes[i] is DefaultValueAttribute)
				{
					this.soapDefaultValue = ((DefaultValueAttribute)customAttributes[i]).Value;
				}
			}
			if (this.soapIgnore)
			{
				this.soapElement = null;
				this.soapAttribute = null;
				this.soapType = null;
				this.soapEnum = null;
				this.soapDefaultValue = null;
			}
		}

		// Token: 0x1700087F RID: 2175
		// (get) Token: 0x060022D1 RID: 8913 RVA: 0x000A3C80 File Offset: 0x000A2C80
		internal SoapAttributeFlags SoapFlags
		{
			get
			{
				SoapAttributeFlags soapAttributeFlags = (SoapAttributeFlags)0;
				if (this.soapElement != null)
				{
					soapAttributeFlags |= SoapAttributeFlags.Element;
				}
				if (this.soapAttribute != null)
				{
					soapAttributeFlags |= SoapAttributeFlags.Attribute;
				}
				if (this.soapEnum != null)
				{
					soapAttributeFlags |= SoapAttributeFlags.Enum;
				}
				if (this.soapType != null)
				{
					soapAttributeFlags |= SoapAttributeFlags.Type;
				}
				return soapAttributeFlags;
			}
		}

		// Token: 0x17000880 RID: 2176
		// (get) Token: 0x060022D2 RID: 8914 RVA: 0x000A3CC0 File Offset: 0x000A2CC0
		// (set) Token: 0x060022D3 RID: 8915 RVA: 0x000A3CC8 File Offset: 0x000A2CC8
		public SoapTypeAttribute SoapType
		{
			get
			{
				return this.soapType;
			}
			set
			{
				this.soapType = value;
			}
		}

		// Token: 0x17000881 RID: 2177
		// (get) Token: 0x060022D4 RID: 8916 RVA: 0x000A3CD1 File Offset: 0x000A2CD1
		// (set) Token: 0x060022D5 RID: 8917 RVA: 0x000A3CD9 File Offset: 0x000A2CD9
		public SoapEnumAttribute SoapEnum
		{
			get
			{
				return this.soapEnum;
			}
			set
			{
				this.soapEnum = value;
			}
		}

		// Token: 0x17000882 RID: 2178
		// (get) Token: 0x060022D6 RID: 8918 RVA: 0x000A3CE2 File Offset: 0x000A2CE2
		// (set) Token: 0x060022D7 RID: 8919 RVA: 0x000A3CEA File Offset: 0x000A2CEA
		public bool SoapIgnore
		{
			get
			{
				return this.soapIgnore;
			}
			set
			{
				this.soapIgnore = value;
			}
		}

		// Token: 0x17000883 RID: 2179
		// (get) Token: 0x060022D8 RID: 8920 RVA: 0x000A3CF3 File Offset: 0x000A2CF3
		// (set) Token: 0x060022D9 RID: 8921 RVA: 0x000A3CFB File Offset: 0x000A2CFB
		public SoapElementAttribute SoapElement
		{
			get
			{
				return this.soapElement;
			}
			set
			{
				this.soapElement = value;
			}
		}

		// Token: 0x17000884 RID: 2180
		// (get) Token: 0x060022DA RID: 8922 RVA: 0x000A3D04 File Offset: 0x000A2D04
		// (set) Token: 0x060022DB RID: 8923 RVA: 0x000A3D0C File Offset: 0x000A2D0C
		public SoapAttributeAttribute SoapAttribute
		{
			get
			{
				return this.soapAttribute;
			}
			set
			{
				this.soapAttribute = value;
			}
		}

		// Token: 0x17000885 RID: 2181
		// (get) Token: 0x060022DC RID: 8924 RVA: 0x000A3D15 File Offset: 0x000A2D15
		// (set) Token: 0x060022DD RID: 8925 RVA: 0x000A3D1D File Offset: 0x000A2D1D
		public object SoapDefaultValue
		{
			get
			{
				return this.soapDefaultValue;
			}
			set
			{
				this.soapDefaultValue = value;
			}
		}

		// Token: 0x040014D9 RID: 5337
		private bool soapIgnore;

		// Token: 0x040014DA RID: 5338
		private SoapTypeAttribute soapType;

		// Token: 0x040014DB RID: 5339
		private SoapElementAttribute soapElement;

		// Token: 0x040014DC RID: 5340
		private SoapAttributeAttribute soapAttribute;

		// Token: 0x040014DD RID: 5341
		private SoapEnumAttribute soapEnum;

		// Token: 0x040014DE RID: 5342
		private object soapDefaultValue;
	}
}
