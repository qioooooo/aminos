using System;
using System.Configuration;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web.Configuration
{
	// Token: 0x020001C8 RID: 456
	[ConfigurationCollection(typeof(Compiler), AddItemName = "compiler", CollectionType = ConfigurationElementCollectionType.BasicMap)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class CompilerCollection : ConfigurationElementCollection
	{
		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x060019F2 RID: 6642 RVA: 0x0007A97C File Offset: 0x0007997C
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return CompilerCollection._properties;
			}
		}

		// Token: 0x060019F3 RID: 6643 RVA: 0x0007A983 File Offset: 0x00079983
		public CompilerCollection()
			: base(StringComparer.OrdinalIgnoreCase)
		{
		}

		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x060019F4 RID: 6644 RVA: 0x0007A990 File Offset: 0x00079990
		public string[] AllKeys
		{
			get
			{
				return StringUtil.ObjectArrayToStringArray(base.BaseGetAllKeys());
			}
		}

		// Token: 0x170004E1 RID: 1249
		public Compiler this[string language]
		{
			get
			{
				return (Compiler)base.BaseGet(language);
			}
		}

		// Token: 0x170004E2 RID: 1250
		public Compiler this[int index]
		{
			get
			{
				return (Compiler)base.BaseGet(index);
			}
		}

		// Token: 0x060019F7 RID: 6647 RVA: 0x0007A9B9 File Offset: 0x000799B9
		protected override ConfigurationElement CreateNewElement()
		{
			return new Compiler();
		}

		// Token: 0x060019F8 RID: 6648 RVA: 0x0007A9C0 File Offset: 0x000799C0
		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((Compiler)element).Language;
		}

		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x060019F9 RID: 6649 RVA: 0x0007A9CD File Offset: 0x000799CD
		protected override string ElementName
		{
			get
			{
				return "compiler";
			}
		}

		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x060019FA RID: 6650 RVA: 0x0007A9D4 File Offset: 0x000799D4
		public override ConfigurationElementCollectionType CollectionType
		{
			get
			{
				return ConfigurationElementCollectionType.BasicMap;
			}
		}

		// Token: 0x060019FB RID: 6651 RVA: 0x0007A9D7 File Offset: 0x000799D7
		public Compiler Get(int index)
		{
			return (Compiler)base.BaseGet(index);
		}

		// Token: 0x060019FC RID: 6652 RVA: 0x0007A9E5 File Offset: 0x000799E5
		public Compiler Get(string language)
		{
			return (Compiler)base.BaseGet(language);
		}

		// Token: 0x060019FD RID: 6653 RVA: 0x0007A9F3 File Offset: 0x000799F3
		public string GetKey(int index)
		{
			return (string)base.BaseGetKey(index);
		}

		// Token: 0x040017AD RID: 6061
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();
	}
}
