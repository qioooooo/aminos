using System;
using System.Configuration;
using System.Security.Permissions;

namespace System.Web.Configuration
{
	// Token: 0x020001FD RID: 509
	[ConfigurationCollection(typeof(HttpModuleAction))]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class HttpModuleActionCollection : ConfigurationElementCollection
	{
		// Token: 0x17000564 RID: 1380
		// (get) Token: 0x06001BBC RID: 7100 RVA: 0x0007FE02 File Offset: 0x0007EE02
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return HttpModuleActionCollection._properties;
			}
		}

		// Token: 0x06001BBD RID: 7101 RVA: 0x0007FE09 File Offset: 0x0007EE09
		public HttpModuleActionCollection()
			: base(StringComparer.OrdinalIgnoreCase)
		{
		}

		// Token: 0x17000565 RID: 1381
		public HttpModuleAction this[int index]
		{
			get
			{
				return (HttpModuleAction)base.BaseGet(index);
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

		// Token: 0x06001BC0 RID: 7104 RVA: 0x0007FE3E File Offset: 0x0007EE3E
		public int IndexOf(HttpModuleAction action)
		{
			return base.BaseIndexOf(action);
		}

		// Token: 0x06001BC1 RID: 7105 RVA: 0x0007FE47 File Offset: 0x0007EE47
		public void Add(HttpModuleAction httpModule)
		{
			this.BaseAdd(httpModule);
		}

		// Token: 0x06001BC2 RID: 7106 RVA: 0x0007FE50 File Offset: 0x0007EE50
		public void Remove(HttpModuleAction action)
		{
			base.BaseRemove(action.Key);
		}

		// Token: 0x06001BC3 RID: 7107 RVA: 0x0007FE5E File Offset: 0x0007EE5E
		public void Remove(string name)
		{
			base.BaseRemove(name);
		}

		// Token: 0x06001BC4 RID: 7108 RVA: 0x0007FE67 File Offset: 0x0007EE67
		public void RemoveAt(int index)
		{
			base.BaseRemoveAt(index);
		}

		// Token: 0x06001BC5 RID: 7109 RVA: 0x0007FE70 File Offset: 0x0007EE70
		protected override ConfigurationElement CreateNewElement()
		{
			return new HttpModuleAction();
		}

		// Token: 0x06001BC6 RID: 7110 RVA: 0x0007FE77 File Offset: 0x0007EE77
		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((HttpModuleAction)element).Key;
		}

		// Token: 0x06001BC7 RID: 7111 RVA: 0x0007FE84 File Offset: 0x0007EE84
		protected override bool IsElementRemovable(ConfigurationElement element)
		{
			HttpModuleAction httpModuleAction = (HttpModuleAction)element;
			if (base.BaseIndexOf(httpModuleAction) != -1)
			{
				return true;
			}
			if (HttpModuleAction.IsSpecialModuleName(httpModuleAction.Name))
			{
				throw new ConfigurationErrorsException(SR.GetString("Special_module_cannot_be_removed_manually", new object[] { httpModuleAction.Name }), httpModuleAction.FileName, httpModuleAction.LineNumber);
			}
			throw new ConfigurationErrorsException(SR.GetString("Module_not_in_app", new object[] { httpModuleAction.Name }), httpModuleAction.FileName, httpModuleAction.LineNumber);
		}

		// Token: 0x06001BC8 RID: 7112 RVA: 0x0007FF0A File Offset: 0x0007EF0A
		public void Clear()
		{
			base.BaseClear();
		}

		// Token: 0x04001882 RID: 6274
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();
	}
}
