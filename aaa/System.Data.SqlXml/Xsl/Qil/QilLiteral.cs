using System;

namespace System.Xml.Xsl.Qil
{
	// Token: 0x02000054 RID: 84
	internal class QilLiteral : QilNode
	{
		// Token: 0x0600056D RID: 1389 RVA: 0x00021405 File Offset: 0x00020405
		public QilLiteral(QilNodeType nodeType, object value)
			: base(nodeType)
		{
			this.Value = value;
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x0600056E RID: 1390 RVA: 0x00021415 File Offset: 0x00020415
		// (set) Token: 0x0600056F RID: 1391 RVA: 0x0002141D File Offset: 0x0002041D
		public object Value
		{
			get
			{
				return this.value;
			}
			set
			{
				this.value = value;
			}
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x00021426 File Offset: 0x00020426
		public static implicit operator string(QilLiteral literal)
		{
			return (string)literal.value;
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x00021433 File Offset: 0x00020433
		public static implicit operator int(QilLiteral literal)
		{
			return (int)literal.value;
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x00021440 File Offset: 0x00020440
		public static implicit operator long(QilLiteral literal)
		{
			return (long)literal.value;
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x0002144D File Offset: 0x0002044D
		public static implicit operator double(QilLiteral literal)
		{
			return (double)literal.value;
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x0002145A File Offset: 0x0002045A
		public static implicit operator decimal(QilLiteral literal)
		{
			return (decimal)literal.value;
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x00021467 File Offset: 0x00020467
		public static implicit operator XmlQueryType(QilLiteral literal)
		{
			return (XmlQueryType)literal.value;
		}

		// Token: 0x04000398 RID: 920
		private object value;
	}
}
