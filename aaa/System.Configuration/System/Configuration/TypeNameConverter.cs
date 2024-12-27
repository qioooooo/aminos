using System;
using System.ComponentModel;
using System.Globalization;

namespace System.Configuration
{
	// Token: 0x020000AA RID: 170
	public sealed class TypeNameConverter : ConfigurationConverterBase
	{
		// Token: 0x06000669 RID: 1641 RVA: 0x0001D3A4 File Offset: 0x0001C3A4
		public override object ConvertTo(ITypeDescriptorContext ctx, CultureInfo ci, object value, Type type)
		{
			if (!(value is Type))
			{
				base.ValidateType(value, typeof(Type));
			}
			string text = null;
			if (value != null)
			{
				text = ((Type)value).AssemblyQualifiedName;
			}
			return text;
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x0001D3DC File Offset: 0x0001C3DC
		public override object ConvertFrom(ITypeDescriptorContext ctx, CultureInfo ci, object data)
		{
			Type typeWithReflectionPermission = TypeUtil.GetTypeWithReflectionPermission((string)data, false);
			if (typeWithReflectionPermission == null)
			{
				throw new ArgumentException(SR.GetString("Type_cannot_be_resolved", new object[] { (string)data }));
			}
			return typeWithReflectionPermission;
		}
	}
}
