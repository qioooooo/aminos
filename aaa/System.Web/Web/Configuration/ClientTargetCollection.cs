using System;
using System.Configuration;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web.Configuration
{
	// Token: 0x020001BD RID: 445
	[ConfigurationCollection(typeof(ClientTarget))]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class ClientTargetCollection : ConfigurationElementCollection
	{
		// Token: 0x06001987 RID: 6535 RVA: 0x000791F2 File Offset: 0x000781F2
		public ClientTargetCollection()
			: base(StringComparer.OrdinalIgnoreCase)
		{
		}

		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x06001988 RID: 6536 RVA: 0x000791FF File Offset: 0x000781FF
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return ClientTargetCollection._properties;
			}
		}

		// Token: 0x170004B7 RID: 1207
		// (get) Token: 0x06001989 RID: 6537 RVA: 0x00079206 File Offset: 0x00078206
		public string[] AllKeys
		{
			get
			{
				return StringUtil.ObjectArrayToStringArray(base.BaseGetAllKeys());
			}
		}

		// Token: 0x0600198A RID: 6538 RVA: 0x00079213 File Offset: 0x00078213
		public string GetKey(int index)
		{
			return (string)base.BaseGetKey(index);
		}

		// Token: 0x0600198B RID: 6539 RVA: 0x00079221 File Offset: 0x00078221
		public void Add(ClientTarget clientTarget)
		{
			this.BaseAdd(clientTarget);
		}

		// Token: 0x0600198C RID: 6540 RVA: 0x0007922A File Offset: 0x0007822A
		public void Remove(string name)
		{
			base.BaseRemove(name);
		}

		// Token: 0x0600198D RID: 6541 RVA: 0x00079233 File Offset: 0x00078233
		public void Remove(ClientTarget clientTarget)
		{
			base.BaseRemove(this.GetElementKey(clientTarget));
		}

		// Token: 0x0600198E RID: 6542 RVA: 0x00079242 File Offset: 0x00078242
		public void RemoveAt(int index)
		{
			base.BaseRemoveAt(index);
		}

		// Token: 0x170004B8 RID: 1208
		public ClientTarget this[string name]
		{
			get
			{
				return (ClientTarget)base.BaseGet(name);
			}
		}

		// Token: 0x170004B9 RID: 1209
		public ClientTarget this[int index]
		{
			get
			{
				return (ClientTarget)base.BaseGet(index);
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

		// Token: 0x06001992 RID: 6546 RVA: 0x00079281 File Offset: 0x00078281
		public void Clear()
		{
			base.BaseClear();
		}

		// Token: 0x06001993 RID: 6547 RVA: 0x00079289 File Offset: 0x00078289
		protected override ConfigurationElement CreateNewElement()
		{
			return new ClientTarget();
		}

		// Token: 0x06001994 RID: 6548 RVA: 0x00079290 File Offset: 0x00078290
		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((ClientTarget)element).Alias;
		}

		// Token: 0x0400174C RID: 5964
		private static readonly ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();
	}
}
