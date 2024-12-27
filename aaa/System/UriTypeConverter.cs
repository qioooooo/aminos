using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace System
{
	// Token: 0x02000367 RID: 871
	public class UriTypeConverter : TypeConverter
	{
		// Token: 0x06001BAB RID: 7083 RVA: 0x0006876B File Offset: 0x0006776B
		public UriTypeConverter()
			: this(UriKind.RelativeOrAbsolute)
		{
		}

		// Token: 0x06001BAC RID: 7084 RVA: 0x00068774 File Offset: 0x00067774
		internal UriTypeConverter(UriKind uriKind)
		{
			this.m_UriKind = uriKind;
		}

		// Token: 0x06001BAD RID: 7085 RVA: 0x00068783 File Offset: 0x00067783
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == null)
			{
				throw new ArgumentNullException("sourceType");
			}
			return sourceType == typeof(string) || typeof(Uri).IsAssignableFrom(sourceType) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x06001BAE RID: 7086 RVA: 0x000687BE File Offset: 0x000677BE
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || destinationType == typeof(string) || destinationType == typeof(Uri) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x06001BAF RID: 7087 RVA: 0x000687F8 File Offset: 0x000677F8
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			string text = value as string;
			if (text != null)
			{
				return new Uri(text, this.m_UriKind);
			}
			Uri uri = value as Uri;
			if (uri != null)
			{
				return new Uri(uri.OriginalString, (this.m_UriKind == UriKind.RelativeOrAbsolute) ? (uri.IsAbsoluteUri ? UriKind.Absolute : UriKind.Relative) : this.m_UriKind);
			}
			return base.ConvertFrom(context, culture, value);
		}

		// Token: 0x06001BB0 RID: 7088 RVA: 0x00068860 File Offset: 0x00067860
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			Uri uri = value as Uri;
			if (uri != null && destinationType == typeof(InstanceDescriptor))
			{
				ConstructorInfo constructor = typeof(Uri).GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, new Type[]
				{
					typeof(string),
					typeof(UriKind)
				}, null);
				return new InstanceDescriptor(constructor, new object[]
				{
					uri.OriginalString,
					(this.m_UriKind == UriKind.RelativeOrAbsolute) ? (uri.IsAbsoluteUri ? UriKind.Absolute : UriKind.Relative) : this.m_UriKind
				});
			}
			if (uri != null && destinationType == typeof(string))
			{
				return uri.OriginalString;
			}
			if (uri != null && destinationType == typeof(Uri))
			{
				return new Uri(uri.OriginalString, (this.m_UriKind == UriKind.RelativeOrAbsolute) ? (uri.IsAbsoluteUri ? UriKind.Absolute : UriKind.Relative) : this.m_UriKind);
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		// Token: 0x06001BB1 RID: 7089 RVA: 0x00068968 File Offset: 0x00067968
		public override bool IsValid(ITypeDescriptorContext context, object value)
		{
			string text = value as string;
			if (text != null)
			{
				Uri uri;
				return Uri.TryCreate(text, this.m_UriKind, out uri);
			}
			return value is Uri;
		}

		// Token: 0x04001C50 RID: 7248
		private UriKind m_UriKind;
	}
}
