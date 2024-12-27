using System;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design
{
	// Token: 0x02000380 RID: 896
	[Obsolete("Use of this type is not recommended because template editing is handled in ControlDesigner. To support template editing expose template data in the TemplateGroups property and call SetViewFlags(ViewFlags.TemplateEditing, true). http://go.microsoft.com/fwlink/?linkid=14202")]
	public interface ITemplateEditingFrame : IDisposable
	{
		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x0600213C RID: 8508
		Style ControlStyle { get; }

		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x0600213D RID: 8509
		string Name { get; }

		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x0600213E RID: 8510
		// (set) Token: 0x0600213F RID: 8511
		int InitialHeight { get; set; }

		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x06002140 RID: 8512
		// (set) Token: 0x06002141 RID: 8513
		int InitialWidth { get; set; }

		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x06002142 RID: 8514
		string[] TemplateNames { get; }

		// Token: 0x17000606 RID: 1542
		// (get) Token: 0x06002143 RID: 8515
		Style[] TemplateStyles { get; }

		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x06002144 RID: 8516
		// (set) Token: 0x06002145 RID: 8517
		TemplateEditingVerb Verb { get; set; }

		// Token: 0x06002146 RID: 8518
		void Close(bool saveChanges);

		// Token: 0x06002147 RID: 8519
		void Open();

		// Token: 0x06002148 RID: 8520
		void Resize(int width, int height);

		// Token: 0x06002149 RID: 8521
		void Save();

		// Token: 0x0600214A RID: 8522
		void UpdateControlName(string newName);
	}
}
