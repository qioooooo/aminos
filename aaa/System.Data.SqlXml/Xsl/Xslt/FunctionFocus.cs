using System;
using System.Collections.Generic;
using System.Xml.Xsl.Qil;
using System.Xml.Xsl.XPath;

namespace System.Xml.Xsl.Xslt
{
	// Token: 0x020000EC RID: 236
	internal struct FunctionFocus : IFocus
	{
		// Token: 0x06000AB7 RID: 2743 RVA: 0x00033900 File Offset: 0x00032900
		public void StartFocus(IList<QilNode> args, XslFlags flags)
		{
			int num = 0;
			if ((flags & XslFlags.Current) != XslFlags.None)
			{
				this.current = (QilParameter)args[num++];
			}
			if ((flags & XslFlags.Position) != XslFlags.None)
			{
				this.position = (QilParameter)args[num++];
			}
			if ((flags & XslFlags.Last) != XslFlags.None)
			{
				this.last = (QilParameter)args[num++];
			}
			this.isSet = true;
		}

		// Token: 0x06000AB8 RID: 2744 RVA: 0x00033974 File Offset: 0x00032974
		public void StopFocus()
		{
			this.isSet = false;
			this.current = (this.position = (this.last = null));
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06000AB9 RID: 2745 RVA: 0x000339A1 File Offset: 0x000329A1
		public bool IsFocusSet
		{
			get
			{
				return this.isSet;
			}
		}

		// Token: 0x06000ABA RID: 2746 RVA: 0x000339A9 File Offset: 0x000329A9
		public QilNode GetCurrent()
		{
			return this.current;
		}

		// Token: 0x06000ABB RID: 2747 RVA: 0x000339B1 File Offset: 0x000329B1
		public QilNode GetPosition()
		{
			return this.position;
		}

		// Token: 0x06000ABC RID: 2748 RVA: 0x000339B9 File Offset: 0x000329B9
		public QilNode GetLast()
		{
			return this.last;
		}

		// Token: 0x0400072A RID: 1834
		private bool isSet;

		// Token: 0x0400072B RID: 1835
		private QilParameter current;

		// Token: 0x0400072C RID: 1836
		private QilParameter position;

		// Token: 0x0400072D RID: 1837
		private QilParameter last;
	}
}
