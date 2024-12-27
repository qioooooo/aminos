using System;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000252 RID: 594
	public interface IMenuEditorService
	{
		// Token: 0x060016AB RID: 5803
		Menu GetMenu();

		// Token: 0x060016AC RID: 5804
		bool IsActive();

		// Token: 0x060016AD RID: 5805
		void SetMenu(Menu menu);

		// Token: 0x060016AE RID: 5806
		void SetSelection(MenuItem item);

		// Token: 0x060016AF RID: 5807
		bool MessageFilter(ref Message m);
	}
}
