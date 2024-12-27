using System;
using System.Reflection;

namespace System.Runtime.Remoting.Metadata
{
	// Token: 0x0200072A RID: 1834
	internal class RemotingCachedData
	{
		// Token: 0x0600421F RID: 16927 RVA: 0x000E1DDA File Offset: 0x000E0DDA
		internal RemotingCachedData(object ri)
		{
			this.RI = ri;
		}

		// Token: 0x06004220 RID: 16928 RVA: 0x000E1DEC File Offset: 0x000E0DEC
		internal SoapAttribute GetSoapAttribute()
		{
			if (this._soapAttr == null)
			{
				lock (this)
				{
					if (this._soapAttr == null)
					{
						SoapAttribute soapAttribute = null;
						ICustomAttributeProvider customAttributeProvider = (ICustomAttributeProvider)this.RI;
						if (this.RI is Type)
						{
							object[] customAttributes = customAttributeProvider.GetCustomAttributes(typeof(SoapTypeAttribute), true);
							if (customAttributes != null && customAttributes.Length != 0)
							{
								soapAttribute = (SoapAttribute)customAttributes[0];
							}
							else
							{
								soapAttribute = new SoapTypeAttribute();
							}
						}
						else if (this.RI is MethodBase)
						{
							object[] customAttributes2 = customAttributeProvider.GetCustomAttributes(typeof(SoapMethodAttribute), true);
							if (customAttributes2 != null && customAttributes2.Length != 0)
							{
								soapAttribute = (SoapAttribute)customAttributes2[0];
							}
							else
							{
								soapAttribute = new SoapMethodAttribute();
							}
						}
						else if (this.RI is FieldInfo)
						{
							object[] customAttributes3 = customAttributeProvider.GetCustomAttributes(typeof(SoapFieldAttribute), false);
							if (customAttributes3 != null && customAttributes3.Length != 0)
							{
								soapAttribute = (SoapAttribute)customAttributes3[0];
							}
							else
							{
								soapAttribute = new SoapFieldAttribute();
							}
						}
						else if (this.RI is ParameterInfo)
						{
							object[] customAttributes4 = customAttributeProvider.GetCustomAttributes(typeof(SoapParameterAttribute), true);
							if (customAttributes4 != null && customAttributes4.Length != 0)
							{
								soapAttribute = (SoapParameterAttribute)customAttributes4[0];
							}
							else
							{
								soapAttribute = new SoapParameterAttribute();
							}
						}
						soapAttribute.SetReflectInfo(this.RI);
						this._soapAttr = soapAttribute;
					}
				}
			}
			return this._soapAttr;
		}

		// Token: 0x040020F5 RID: 8437
		protected object RI;

		// Token: 0x040020F6 RID: 8438
		private SoapAttribute _soapAttr;
	}
}
