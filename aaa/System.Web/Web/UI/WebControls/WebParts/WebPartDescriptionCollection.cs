using System;
using System.Collections;
using System.Collections.Specialized;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x0200071E RID: 1822
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class WebPartDescriptionCollection : ReadOnlyCollectionBase
	{
		// Token: 0x0600586F RID: 22639 RVA: 0x00163A61 File Offset: 0x00162A61
		public WebPartDescriptionCollection()
		{
		}

		// Token: 0x06005870 RID: 22640 RVA: 0x00163A6C File Offset: 0x00162A6C
		public WebPartDescriptionCollection(ICollection webPartDescriptions)
		{
			if (webPartDescriptions == null)
			{
				throw new ArgumentNullException("webPartDescriptions");
			}
			this._ids = new HybridDictionary(webPartDescriptions.Count, true);
			foreach (object obj in webPartDescriptions)
			{
				if (obj == null)
				{
					throw new ArgumentException(SR.GetString("Collection_CantAddNull"), "webPartDescriptions");
				}
				WebPartDescription webPartDescription = obj as WebPartDescription;
				if (webPartDescription == null)
				{
					throw new ArgumentException(SR.GetString("Collection_InvalidType", new object[] { "WebPartDescription" }), "webPartDescriptions");
				}
				string id = webPartDescription.ID;
				if (this._ids.Contains(id))
				{
					throw new ArgumentException(SR.GetString("WebPart_Collection_DuplicateID", new object[] { "WebPartDescription", id }), "webPartDescriptions");
				}
				base.InnerList.Add(webPartDescription);
				this._ids.Add(id, webPartDescription);
			}
		}

		// Token: 0x06005871 RID: 22641 RVA: 0x00163B8C File Offset: 0x00162B8C
		public bool Contains(WebPartDescription value)
		{
			return base.InnerList.Contains(value);
		}

		// Token: 0x06005872 RID: 22642 RVA: 0x00163B9A File Offset: 0x00162B9A
		public int IndexOf(WebPartDescription value)
		{
			return base.InnerList.IndexOf(value);
		}

		// Token: 0x170016E8 RID: 5864
		public WebPartDescription this[int index]
		{
			get
			{
				return (WebPartDescription)base.InnerList[index];
			}
		}

		// Token: 0x170016E9 RID: 5865
		public WebPartDescription this[string id]
		{
			get
			{
				if (this._ids == null)
				{
					return null;
				}
				return (WebPartDescription)this._ids[id];
			}
		}

		// Token: 0x06005875 RID: 22645 RVA: 0x00163BD8 File Offset: 0x00162BD8
		public void CopyTo(WebPartDescription[] array, int index)
		{
			base.InnerList.CopyTo(array, index);
		}

		// Token: 0x04002FE6 RID: 12262
		private HybridDictionary _ids;
	}
}
