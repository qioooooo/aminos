using System;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	// Token: 0x02000403 RID: 1027
	public abstract class LayoutSettings
	{
		// Token: 0x06003C75 RID: 15477 RVA: 0x000D99D3 File Offset: 0x000D89D3
		protected LayoutSettings()
		{
		}

		// Token: 0x06003C76 RID: 15478 RVA: 0x000D99DB File Offset: 0x000D89DB
		internal LayoutSettings(IArrangedElement owner)
		{
			this._owner = owner;
		}

		// Token: 0x17000B7B RID: 2939
		// (get) Token: 0x06003C77 RID: 15479 RVA: 0x000D99EA File Offset: 0x000D89EA
		public virtual LayoutEngine LayoutEngine
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000B7C RID: 2940
		// (get) Token: 0x06003C78 RID: 15480 RVA: 0x000D99ED File Offset: 0x000D89ED
		internal IArrangedElement Owner
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x04001E1D RID: 7709
		private IArrangedElement _owner;
	}
}
