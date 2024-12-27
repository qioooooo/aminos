using System;
using System.Configuration;
using System.Security.Permissions;

namespace System.Web.Configuration
{
	// Token: 0x020001DA RID: 474
	[ConfigurationCollection(typeof(ExpressionBuilder))]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class ExpressionBuilderCollection : ConfigurationElementCollection
	{
		// Token: 0x06001A7E RID: 6782 RVA: 0x0007B826 File Offset: 0x0007A826
		public ExpressionBuilderCollection()
			: base(StringComparer.OrdinalIgnoreCase)
		{
		}

		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x06001A7F RID: 6783 RVA: 0x0007B833 File Offset: 0x0007A833
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return ExpressionBuilderCollection._properties;
			}
		}

		// Token: 0x17000508 RID: 1288
		public ExpressionBuilder this[string name]
		{
			get
			{
				return (ExpressionBuilder)base.BaseGet(name);
			}
		}

		// Token: 0x17000509 RID: 1289
		public ExpressionBuilder this[int index]
		{
			get
			{
				return (ExpressionBuilder)base.BaseGet(index);
			}
			set
			{
				if (base.BaseGet(index) != null)
				{
					base.BaseRemoveAt(index);
				}
				this.BaseAdd(index, value);
			}
		}

		// Token: 0x06001A83 RID: 6787 RVA: 0x0007B870 File Offset: 0x0007A870
		public void Add(ExpressionBuilder buildProvider)
		{
			this.BaseAdd(buildProvider);
		}

		// Token: 0x06001A84 RID: 6788 RVA: 0x0007B879 File Offset: 0x0007A879
		public void Remove(string name)
		{
			base.BaseRemove(name);
		}

		// Token: 0x06001A85 RID: 6789 RVA: 0x0007B882 File Offset: 0x0007A882
		public void RemoveAt(int index)
		{
			base.BaseRemoveAt(index);
		}

		// Token: 0x06001A86 RID: 6790 RVA: 0x0007B88B File Offset: 0x0007A88B
		public void Clear()
		{
			base.BaseClear();
		}

		// Token: 0x06001A87 RID: 6791 RVA: 0x0007B893 File Offset: 0x0007A893
		protected override ConfigurationElement CreateNewElement()
		{
			return new ExpressionBuilder();
		}

		// Token: 0x06001A88 RID: 6792 RVA: 0x0007B89A File Offset: 0x0007A89A
		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((ExpressionBuilder)element).ExpressionPrefix;
		}

		// Token: 0x040017DD RID: 6109
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();
	}
}
