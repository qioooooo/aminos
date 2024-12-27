using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x0200047F RID: 1151
	[DefaultEvent("CollectionChanged")]
	internal class ListManagerBindingsCollection : BindingsCollection
	{
		// Token: 0x06004372 RID: 17266 RVA: 0x000F1FBC File Offset: 0x000F0FBC
		internal ListManagerBindingsCollection(BindingManagerBase bindingManagerBase)
		{
			this.bindingManagerBase = bindingManagerBase;
		}

		// Token: 0x06004373 RID: 17267 RVA: 0x000F1FCC File Offset: 0x000F0FCC
		protected override void AddCore(Binding dataBinding)
		{
			if (dataBinding == null)
			{
				throw new ArgumentNullException("dataBinding");
			}
			if (dataBinding.BindingManagerBase == this.bindingManagerBase)
			{
				throw new ArgumentException(SR.GetString("BindingsCollectionAdd1"), "dataBinding");
			}
			if (dataBinding.BindingManagerBase != null)
			{
				throw new ArgumentException(SR.GetString("BindingsCollectionAdd2"), "dataBinding");
			}
			dataBinding.SetListManager(this.bindingManagerBase);
			base.AddCore(dataBinding);
		}

		// Token: 0x06004374 RID: 17268 RVA: 0x000F203C File Offset: 0x000F103C
		protected override void ClearCore()
		{
			int count = this.Count;
			for (int i = 0; i < count; i++)
			{
				Binding binding = base[i];
				binding.SetListManager(null);
			}
			base.ClearCore();
		}

		// Token: 0x06004375 RID: 17269 RVA: 0x000F2071 File Offset: 0x000F1071
		protected override void RemoveCore(Binding dataBinding)
		{
			if (dataBinding.BindingManagerBase != this.bindingManagerBase)
			{
				throw new ArgumentException(SR.GetString("BindingsCollectionForeign"));
			}
			dataBinding.SetListManager(null);
			base.RemoveCore(dataBinding);
		}

		// Token: 0x040020DC RID: 8412
		private BindingManagerBase bindingManagerBase;
	}
}
