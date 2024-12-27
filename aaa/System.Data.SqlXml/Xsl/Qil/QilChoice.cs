using System;

namespace System.Xml.Xsl.Qil
{
	// Token: 0x02000046 RID: 70
	internal class QilChoice : QilBinary
	{
		// Token: 0x06000489 RID: 1161 RVA: 0x0001F36E File Offset: 0x0001E36E
		public QilChoice(QilNodeType nodeType, QilNode expression, QilNode branches)
			: base(nodeType, expression, branches)
		{
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x0600048A RID: 1162 RVA: 0x0001F379 File Offset: 0x0001E379
		// (set) Token: 0x0600048B RID: 1163 RVA: 0x0001F381 File Offset: 0x0001E381
		public QilNode Expression
		{
			get
			{
				return base.Left;
			}
			set
			{
				base.Left = value;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600048C RID: 1164 RVA: 0x0001F38A File Offset: 0x0001E38A
		// (set) Token: 0x0600048D RID: 1165 RVA: 0x0001F397 File Offset: 0x0001E397
		public QilList Branches
		{
			get
			{
				return (QilList)base.Right;
			}
			set
			{
				base.Right = value;
			}
		}
	}
}
