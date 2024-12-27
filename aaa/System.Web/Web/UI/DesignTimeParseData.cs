using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x020003E5 RID: 997
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class DesignTimeParseData
	{
		// Token: 0x06003058 RID: 12376 RVA: 0x000D53F4 File Offset: 0x000D43F4
		public DesignTimeParseData(IDesignerHost designerHost, string parseText)
			: this(designerHost, parseText, string.Empty)
		{
		}

		// Token: 0x06003059 RID: 12377 RVA: 0x000D5403 File Offset: 0x000D4403
		public DesignTimeParseData(IDesignerHost designerHost, string parseText, string filter)
		{
			if (string.IsNullOrEmpty(parseText))
			{
				throw new ArgumentNullException("parseText");
			}
			this._designerHost = designerHost;
			this._parseText = parseText;
			this._filter = filter;
		}

		// Token: 0x17000A7E RID: 2686
		// (get) Token: 0x0600305A RID: 12378 RVA: 0x000D5433 File Offset: 0x000D4433
		// (set) Token: 0x0600305B RID: 12379 RVA: 0x000D543B File Offset: 0x000D443B
		public bool ShouldApplyTheme
		{
			get
			{
				return this._shouldApplyTheme;
			}
			set
			{
				this._shouldApplyTheme = value;
			}
		}

		// Token: 0x17000A7F RID: 2687
		// (get) Token: 0x0600305C RID: 12380 RVA: 0x000D5444 File Offset: 0x000D4444
		// (set) Token: 0x0600305D RID: 12381 RVA: 0x000D544C File Offset: 0x000D444C
		public EventHandler DataBindingHandler
		{
			get
			{
				return this._dataBindingHandler;
			}
			set
			{
				this._dataBindingHandler = value;
			}
		}

		// Token: 0x17000A80 RID: 2688
		// (get) Token: 0x0600305E RID: 12382 RVA: 0x000D5455 File Offset: 0x000D4455
		public IDesignerHost DesignerHost
		{
			get
			{
				return this._designerHost;
			}
		}

		// Token: 0x17000A81 RID: 2689
		// (get) Token: 0x0600305F RID: 12383 RVA: 0x000D545D File Offset: 0x000D445D
		// (set) Token: 0x06003060 RID: 12384 RVA: 0x000D5473 File Offset: 0x000D4473
		public string DocumentUrl
		{
			get
			{
				if (this._documentUrl == null)
				{
					return string.Empty;
				}
				return this._documentUrl;
			}
			set
			{
				this._documentUrl = value;
			}
		}

		// Token: 0x17000A82 RID: 2690
		// (get) Token: 0x06003061 RID: 12385 RVA: 0x000D547C File Offset: 0x000D447C
		public string Filter
		{
			get
			{
				if (this._filter == null)
				{
					return string.Empty;
				}
				return this._filter;
			}
		}

		// Token: 0x17000A83 RID: 2691
		// (get) Token: 0x06003062 RID: 12386 RVA: 0x000D5492 File Offset: 0x000D4492
		public string ParseText
		{
			get
			{
				return this._parseText;
			}
		}

		// Token: 0x17000A84 RID: 2692
		// (get) Token: 0x06003063 RID: 12387 RVA: 0x000D549A File Offset: 0x000D449A
		public ICollection UserControlRegisterEntries
		{
			get
			{
				return this._userControlRegisterEntries;
			}
		}

		// Token: 0x06003064 RID: 12388 RVA: 0x000D54A4 File Offset: 0x000D44A4
		internal void SetUserControlRegisterEntries(ICollection userControlRegisterEntries, List<TagNamespaceRegisterEntry> tagRegisterEntries)
		{
			if (userControlRegisterEntries == null && tagRegisterEntries == null)
			{
				return;
			}
			List<Triplet> list = new List<Triplet>();
			if (userControlRegisterEntries != null)
			{
				foreach (object obj in userControlRegisterEntries)
				{
					UserControlRegisterEntry userControlRegisterEntry = (UserControlRegisterEntry)obj;
					list.Add(new Triplet(userControlRegisterEntry.TagPrefix, new Pair(userControlRegisterEntry.TagName, userControlRegisterEntry.UserControlSource.ToString()), null));
				}
			}
			if (tagRegisterEntries != null)
			{
				foreach (TagNamespaceRegisterEntry tagNamespaceRegisterEntry in tagRegisterEntries)
				{
					list.Add(new Triplet(tagNamespaceRegisterEntry.TagPrefix, null, new Pair(tagNamespaceRegisterEntry.Namespace, tagNamespaceRegisterEntry.AssemblyName)));
				}
			}
			this._userControlRegisterEntries = list;
		}

		// Token: 0x04002216 RID: 8726
		private IDesignerHost _designerHost;

		// Token: 0x04002217 RID: 8727
		private string _documentUrl;

		// Token: 0x04002218 RID: 8728
		private EventHandler _dataBindingHandler;

		// Token: 0x04002219 RID: 8729
		private string _parseText;

		// Token: 0x0400221A RID: 8730
		private string _filter;

		// Token: 0x0400221B RID: 8731
		private bool _shouldApplyTheme;

		// Token: 0x0400221C RID: 8732
		private ICollection _userControlRegisterEntries;
	}
}
