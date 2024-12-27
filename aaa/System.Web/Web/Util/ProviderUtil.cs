using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;

namespace System.Web.Util
{
	// Token: 0x0200077B RID: 1915
	internal static class ProviderUtil
	{
		// Token: 0x06005C85 RID: 23685 RVA: 0x00172DAA File Offset: 0x00171DAA
		internal static void GetAndRemoveStringAttribute(NameValueCollection config, string attrib, string providerName, ref string val)
		{
			val = config.Get(attrib);
			config.Remove(attrib);
		}

		// Token: 0x06005C86 RID: 23686 RVA: 0x00172DBC File Offset: 0x00171DBC
		internal static void GetAndRemovePositiveAttribute(NameValueCollection config, string attrib, string providerName, ref int val)
		{
			ProviderUtil.GetPositiveAttribute(config, attrib, providerName, ref val);
			config.Remove(attrib);
		}

		// Token: 0x06005C87 RID: 23687 RVA: 0x00172DD0 File Offset: 0x00171DD0
		internal static void GetPositiveAttribute(NameValueCollection config, string attrib, string providerName, ref int val)
		{
			string text = config.Get(attrib);
			if (text == null)
			{
				return;
			}
			int num;
			try
			{
				num = Convert.ToInt32(text, CultureInfo.InvariantCulture);
			}
			catch (Exception ex)
			{
				if (ex is ArgumentException || ex is FormatException || ex is OverflowException)
				{
					throw new ConfigurationErrorsException(SR.GetString("Invalid_provider_positive_attributes", new object[] { attrib, providerName }));
				}
				throw;
			}
			if (num < 0)
			{
				throw new ConfigurationErrorsException(SR.GetString("Invalid_provider_positive_attributes", new object[] { attrib, providerName }));
			}
			val = num;
		}

		// Token: 0x06005C88 RID: 23688 RVA: 0x00172E70 File Offset: 0x00171E70
		internal static void GetAndRemovePositiveOrInfiniteAttribute(NameValueCollection config, string attrib, string providerName, ref int val)
		{
			ProviderUtil.GetPositiveOrInfiniteAttribute(config, attrib, providerName, ref val);
			config.Remove(attrib);
		}

		// Token: 0x06005C89 RID: 23689 RVA: 0x00172E84 File Offset: 0x00171E84
		internal static void GetPositiveOrInfiniteAttribute(NameValueCollection config, string attrib, string providerName, ref int val)
		{
			string text = config.Get(attrib);
			if (text == null)
			{
				return;
			}
			int num;
			if (text == "Infinite")
			{
				num = int.MaxValue;
			}
			else
			{
				try
				{
					num = Convert.ToInt32(text, CultureInfo.InvariantCulture);
				}
				catch (Exception ex)
				{
					if (ex is ArgumentException || ex is FormatException || ex is OverflowException)
					{
						throw new ConfigurationErrorsException(SR.GetString("Invalid_provider_positive_attributes", new object[] { attrib, providerName }));
					}
					throw;
				}
				if (num < 0)
				{
					throw new ConfigurationErrorsException(SR.GetString("Invalid_provider_positive_attributes", new object[] { attrib, providerName }));
				}
			}
			val = num;
		}

		// Token: 0x06005C8A RID: 23690 RVA: 0x00172F38 File Offset: 0x00171F38
		internal static void GetAndRemoveNonZeroPositiveOrInfiniteAttribute(NameValueCollection config, string attrib, string providerName, ref int val)
		{
			ProviderUtil.GetNonZeroPositiveOrInfiniteAttribute(config, attrib, providerName, ref val);
			config.Remove(attrib);
		}

		// Token: 0x06005C8B RID: 23691 RVA: 0x00172F4C File Offset: 0x00171F4C
		internal static void GetNonZeroPositiveOrInfiniteAttribute(NameValueCollection config, string attrib, string providerName, ref int val)
		{
			string text = config.Get(attrib);
			if (text == null)
			{
				return;
			}
			int num;
			if (text == "Infinite")
			{
				num = int.MaxValue;
			}
			else
			{
				try
				{
					num = Convert.ToInt32(text, CultureInfo.InvariantCulture);
				}
				catch (Exception ex)
				{
					if (ex is ArgumentException || ex is FormatException || ex is OverflowException)
					{
						throw new ConfigurationErrorsException(SR.GetString("Invalid_provider_non_zero_positive_attributes", new object[] { attrib, providerName }));
					}
					throw;
				}
				if (num <= 0)
				{
					throw new ConfigurationErrorsException(SR.GetString("Invalid_provider_non_zero_positive_attributes", new object[] { attrib, providerName }));
				}
			}
			val = num;
		}

		// Token: 0x06005C8C RID: 23692 RVA: 0x00173000 File Offset: 0x00172000
		internal static void GetAndRemoveBooleanAttribute(NameValueCollection config, string attrib, string providerName, ref bool val)
		{
			ProviderUtil.GetBooleanAttribute(config, attrib, providerName, ref val);
			config.Remove(attrib);
		}

		// Token: 0x06005C8D RID: 23693 RVA: 0x00173014 File Offset: 0x00172014
		internal static void GetBooleanAttribute(NameValueCollection config, string attrib, string providerName, ref bool val)
		{
			string text = config.Get(attrib);
			if (text == null)
			{
				return;
			}
			if (text == "true")
			{
				val = true;
				return;
			}
			if (text == "false")
			{
				val = false;
				return;
			}
			throw new ConfigurationErrorsException(SR.GetString("Invalid_provider_attribute", new object[] { attrib, providerName, text }));
		}

		// Token: 0x06005C8E RID: 23694 RVA: 0x00173072 File Offset: 0x00172072
		internal static void GetAndRemoveRequiredNonEmptyStringAttribute(NameValueCollection config, string attrib, string providerName, ref string val)
		{
			ProviderUtil.GetRequiredNonEmptyStringAttribute(config, attrib, providerName, ref val);
			config.Remove(attrib);
		}

		// Token: 0x06005C8F RID: 23695 RVA: 0x00173084 File Offset: 0x00172084
		internal static void GetRequiredNonEmptyStringAttribute(NameValueCollection config, string attrib, string providerName, ref string val)
		{
			ProviderUtil.GetNonEmptyStringAttributeInternal(config, attrib, providerName, ref val, true);
		}

		// Token: 0x06005C90 RID: 23696 RVA: 0x00173090 File Offset: 0x00172090
		private static void GetNonEmptyStringAttributeInternal(NameValueCollection config, string attrib, string providerName, ref string val, bool required)
		{
			string text = config.Get(attrib);
			if ((text == null && required) || text.Length == 0)
			{
				throw new ConfigurationErrorsException(SR.GetString("Provider_missing_attribute", new object[] { attrib, providerName }));
			}
			val = text;
		}

		// Token: 0x06005C91 RID: 23697 RVA: 0x001730D8 File Offset: 0x001720D8
		internal static void CheckUnrecognizedAttributes(NameValueCollection config, string providerName)
		{
			if (config.Count > 0)
			{
				string key = config.GetKey(0);
				if (!string.IsNullOrEmpty(key))
				{
					throw new ConfigurationErrorsException(SR.GetString("Unexpected_provider_attribute", new object[] { key, providerName }));
				}
			}
		}

		// Token: 0x04003175 RID: 12661
		internal const int Infinite = 2147483647;
	}
}
