using System;
using System.Runtime.InteropServices;

namespace System.Resources
{
	// Token: 0x0200041E RID: 1054
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
	public sealed class NeutralResourcesLanguageAttribute : Attribute
	{
		// Token: 0x06002B87 RID: 11143 RVA: 0x0009266F File Offset: 0x0009166F
		public NeutralResourcesLanguageAttribute(string cultureName)
		{
			if (cultureName == null)
			{
				throw new ArgumentNullException("cultureName");
			}
			this._culture = cultureName;
			this._fallbackLoc = UltimateResourceFallbackLocation.MainAssembly;
		}

		// Token: 0x06002B88 RID: 11144 RVA: 0x00092694 File Offset: 0x00091694
		public NeutralResourcesLanguageAttribute(string cultureName, UltimateResourceFallbackLocation location)
		{
			if (cultureName == null)
			{
				throw new ArgumentNullException("cultureName");
			}
			if (!Enum.IsDefined(typeof(UltimateResourceFallbackLocation), location))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_InvalidNeutralResourcesLanguage_FallbackLoc", new object[] { location }));
			}
			this._culture = cultureName;
			this._fallbackLoc = location;
		}

		// Token: 0x1700081B RID: 2075
		// (get) Token: 0x06002B89 RID: 11145 RVA: 0x000926FB File Offset: 0x000916FB
		public string CultureName
		{
			get
			{
				return this._culture;
			}
		}

		// Token: 0x1700081C RID: 2076
		// (get) Token: 0x06002B8A RID: 11146 RVA: 0x00092703 File Offset: 0x00091703
		public UltimateResourceFallbackLocation Location
		{
			get
			{
				return this._fallbackLoc;
			}
		}

		// Token: 0x0400150A RID: 5386
		private string _culture;

		// Token: 0x0400150B RID: 5387
		private UltimateResourceFallbackLocation _fallbackLoc;
	}
}
