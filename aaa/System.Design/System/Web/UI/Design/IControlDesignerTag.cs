using System;

namespace System.Web.UI.Design
{
	// Token: 0x02000374 RID: 884
	public interface IControlDesignerTag
	{
		// Token: 0x170005EF RID: 1519
		// (get) Token: 0x06002108 RID: 8456
		bool IsDirty { get; }

		// Token: 0x06002109 RID: 8457
		string GetAttribute(string name);

		// Token: 0x0600210A RID: 8458
		string GetContent();

		// Token: 0x0600210B RID: 8459
		void RemoveAttribute(string name);

		// Token: 0x0600210C RID: 8460
		void SetAttribute(string name, string value);

		// Token: 0x0600210D RID: 8461
		void SetContent(string content);

		// Token: 0x0600210E RID: 8462
		void SetDirty(bool dirty);

		// Token: 0x0600210F RID: 8463
		string GetOuterContent();
	}
}
