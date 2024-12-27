using System;
using System.Collections;
using System.Globalization;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000093 RID: 147
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class CollectionConverter : TypeConverter
	{
		// Token: 0x06000562 RID: 1378 RVA: 0x00016B69 File Offset: 0x00015B69
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == typeof(string) && value is ICollection)
			{
				return SR.GetString("CollectionConverterText");
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x00016BA6 File Offset: 0x00015BA6
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			return null;
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x00016BA9 File Offset: 0x00015BA9
		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return false;
		}
	}
}
