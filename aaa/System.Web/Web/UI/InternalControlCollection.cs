using System;

namespace System.Web.UI
{
	// Token: 0x02000410 RID: 1040
	internal class InternalControlCollection : ControlCollection
	{
		// Token: 0x060032AC RID: 12972 RVA: 0x000DD695 File Offset: 0x000DC695
		internal InternalControlCollection(Control owner)
			: base(owner)
		{
		}

		// Token: 0x060032AD RID: 12973 RVA: 0x000DD6A0 File Offset: 0x000DC6A0
		private void ThrowNotSupportedException()
		{
			throw new HttpException(SR.GetString("Control_does_not_allow_children", new object[] { base.Owner.GetType().ToString() }));
		}

		// Token: 0x060032AE RID: 12974 RVA: 0x000DD6D7 File Offset: 0x000DC6D7
		public override void Add(Control child)
		{
			this.ThrowNotSupportedException();
		}

		// Token: 0x060032AF RID: 12975 RVA: 0x000DD6DF File Offset: 0x000DC6DF
		public override void AddAt(int index, Control child)
		{
			this.ThrowNotSupportedException();
		}
	}
}
