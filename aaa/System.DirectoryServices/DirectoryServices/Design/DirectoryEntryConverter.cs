using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;

namespace System.DirectoryServices.Design
{
	// Token: 0x02000048 RID: 72
	internal class DirectoryEntryConverter : TypeConverter
	{
		// Token: 0x060001EF RID: 495 RVA: 0x00007E32 File Offset: 0x00006E32
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x00007E4C File Offset: 0x00006E4C
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value != null && value is string)
			{
				string text = ((string)value).Trim();
				if (text.Length == 0)
				{
					return null;
				}
				if (text.CompareTo(Res.GetString("DSNotSet")) != 0 && DirectoryEntryConverter.GetFromCache(text) == null)
				{
					DirectoryEntry directoryEntry = new DirectoryEntry(text);
					DirectoryEntryConverter.componentsCreated[text] = directoryEntry;
					if (context != null)
					{
						context.Container.Add(directoryEntry);
					}
					return directoryEntry;
				}
			}
			return null;
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x00007EBB File Offset: 0x00006EBB
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null || destinationType != typeof(string))
			{
				return base.ConvertTo(context, culture, value, destinationType);
			}
			if (value != null)
			{
				return ((DirectoryEntry)value).Path;
			}
			return Res.GetString("DSNotSet");
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x00007EF4 File Offset: 0x00006EF4
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			if (DirectoryEntryConverter.values == null)
			{
				object[] array = new object[1];
				DirectoryEntryConverter.values = new TypeConverter.StandardValuesCollection(array);
			}
			return DirectoryEntryConverter.values;
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x00007F20 File Offset: 0x00006F20
		internal static DirectoryEntry GetFromCache(string path)
		{
			if (DirectoryEntryConverter.componentsCreated.ContainsKey(path))
			{
				DirectoryEntry directoryEntry = (DirectoryEntry)DirectoryEntryConverter.componentsCreated[path];
				if (directoryEntry.Site == null)
				{
					DirectoryEntryConverter.componentsCreated.Remove(path);
				}
				else
				{
					if (directoryEntry.Path == path)
					{
						return directoryEntry;
					}
					DirectoryEntryConverter.componentsCreated.Remove(path);
				}
			}
			return null;
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x00007F7C File Offset: 0x00006F7C
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return false;
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x00007F7F File Offset: 0x00006F7F
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x04000204 RID: 516
		private static TypeConverter.StandardValuesCollection values;

		// Token: 0x04000205 RID: 517
		private static Hashtable componentsCreated = new Hashtable(StringComparer.OrdinalIgnoreCase);
	}
}
