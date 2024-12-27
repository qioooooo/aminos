using System;
using System.Configuration;
using System.IO;

namespace System.Xml.Serialization.Configuration
{
	// Token: 0x02000356 RID: 854
	public class RootedPathValidator : ConfigurationValidatorBase
	{
		// Token: 0x06002946 RID: 10566 RVA: 0x000D3A18 File Offset: 0x000D2A18
		public override bool CanValidate(Type type)
		{
			return type == typeof(string);
		}

		// Token: 0x06002947 RID: 10567 RVA: 0x000D3A28 File Offset: 0x000D2A28
		public override void Validate(object value)
		{
			string text = value as string;
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			text = text.Trim();
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			if (!Path.IsPathRooted(text))
			{
				throw new ConfigurationErrorsException();
			}
			char c = text[0];
			if (c == Path.DirectorySeparatorChar || c == Path.AltDirectorySeparatorChar)
			{
				throw new ConfigurationErrorsException();
			}
		}
	}
}
