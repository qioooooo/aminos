using System;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007DC RID: 2012
	internal sealed class NameInfo
	{
		// Token: 0x06004772 RID: 18290 RVA: 0x000F5D41 File Offset: 0x000F4D41
		internal NameInfo()
		{
		}

		// Token: 0x06004773 RID: 18291 RVA: 0x000F5D4C File Offset: 0x000F4D4C
		internal void Init()
		{
			this.NIFullName = null;
			this.NIobjectId = 0L;
			this.NIassemId = 0L;
			this.NIprimitiveTypeEnum = InternalPrimitiveTypeE.Invalid;
			this.NItype = null;
			this.NIisSealed = false;
			this.NItransmitTypeOnObject = false;
			this.NItransmitTypeOnMember = false;
			this.NIisParentTypeOnObject = false;
			this.NIisArray = false;
			this.NIisArrayItem = false;
			this.NIarrayEnum = InternalArrayTypeE.Empty;
			this.NIsealedStatusChecked = false;
		}

		// Token: 0x17000C7D RID: 3197
		// (get) Token: 0x06004774 RID: 18292 RVA: 0x000F5DB6 File Offset: 0x000F4DB6
		public bool IsSealed
		{
			get
			{
				if (!this.NIsealedStatusChecked)
				{
					this.NIisSealed = this.NItype.IsSealed;
					this.NIsealedStatusChecked = true;
				}
				return this.NIisSealed;
			}
		}

		// Token: 0x17000C7E RID: 3198
		// (get) Token: 0x06004775 RID: 18293 RVA: 0x000F5DDE File Offset: 0x000F4DDE
		// (set) Token: 0x06004776 RID: 18294 RVA: 0x000F5DFF File Offset: 0x000F4DFF
		public string NIname
		{
			get
			{
				if (this.NIFullName == null)
				{
					this.NIFullName = this.NItype.FullName;
				}
				return this.NIFullName;
			}
			set
			{
				this.NIFullName = value;
			}
		}

		// Token: 0x0400244A RID: 9290
		internal string NIFullName;

		// Token: 0x0400244B RID: 9291
		internal long NIobjectId;

		// Token: 0x0400244C RID: 9292
		internal long NIassemId;

		// Token: 0x0400244D RID: 9293
		internal InternalPrimitiveTypeE NIprimitiveTypeEnum;

		// Token: 0x0400244E RID: 9294
		internal Type NItype;

		// Token: 0x0400244F RID: 9295
		internal bool NIisSealed;

		// Token: 0x04002450 RID: 9296
		internal bool NIisArray;

		// Token: 0x04002451 RID: 9297
		internal bool NIisArrayItem;

		// Token: 0x04002452 RID: 9298
		internal bool NItransmitTypeOnObject;

		// Token: 0x04002453 RID: 9299
		internal bool NItransmitTypeOnMember;

		// Token: 0x04002454 RID: 9300
		internal bool NIisParentTypeOnObject;

		// Token: 0x04002455 RID: 9301
		internal InternalArrayTypeE NIarrayEnum;

		// Token: 0x04002456 RID: 9302
		private bool NIsealedStatusChecked;
	}
}
