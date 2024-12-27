using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Runtime.Serialization
{
	// Token: 0x02000348 RID: 840
	[ComVisible(true)]
	public class FormatterConverter : IFormatterConverter
	{
		// Token: 0x0600218D RID: 8589 RVA: 0x000541FB File Offset: 0x000531FB
		public object Convert(object value, Type type)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return global::System.Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
		}

		// Token: 0x0600218E RID: 8590 RVA: 0x00054217 File Offset: 0x00053217
		public object Convert(object value, TypeCode typeCode)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return global::System.Convert.ChangeType(value, typeCode, CultureInfo.InvariantCulture);
		}

		// Token: 0x0600218F RID: 8591 RVA: 0x00054233 File Offset: 0x00053233
		public bool ToBoolean(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return global::System.Convert.ToBoolean(value, CultureInfo.InvariantCulture);
		}

		// Token: 0x06002190 RID: 8592 RVA: 0x0005424E File Offset: 0x0005324E
		public char ToChar(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return global::System.Convert.ToChar(value, CultureInfo.InvariantCulture);
		}

		// Token: 0x06002191 RID: 8593 RVA: 0x00054269 File Offset: 0x00053269
		[CLSCompliant(false)]
		public sbyte ToSByte(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return global::System.Convert.ToSByte(value, CultureInfo.InvariantCulture);
		}

		// Token: 0x06002192 RID: 8594 RVA: 0x00054284 File Offset: 0x00053284
		public byte ToByte(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return global::System.Convert.ToByte(value, CultureInfo.InvariantCulture);
		}

		// Token: 0x06002193 RID: 8595 RVA: 0x0005429F File Offset: 0x0005329F
		public short ToInt16(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return global::System.Convert.ToInt16(value, CultureInfo.InvariantCulture);
		}

		// Token: 0x06002194 RID: 8596 RVA: 0x000542BA File Offset: 0x000532BA
		[CLSCompliant(false)]
		public ushort ToUInt16(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return global::System.Convert.ToUInt16(value, CultureInfo.InvariantCulture);
		}

		// Token: 0x06002195 RID: 8597 RVA: 0x000542D5 File Offset: 0x000532D5
		public int ToInt32(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return global::System.Convert.ToInt32(value, CultureInfo.InvariantCulture);
		}

		// Token: 0x06002196 RID: 8598 RVA: 0x000542F0 File Offset: 0x000532F0
		[CLSCompliant(false)]
		public uint ToUInt32(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return global::System.Convert.ToUInt32(value, CultureInfo.InvariantCulture);
		}

		// Token: 0x06002197 RID: 8599 RVA: 0x0005430B File Offset: 0x0005330B
		public long ToInt64(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return global::System.Convert.ToInt64(value, CultureInfo.InvariantCulture);
		}

		// Token: 0x06002198 RID: 8600 RVA: 0x00054326 File Offset: 0x00053326
		[CLSCompliant(false)]
		public ulong ToUInt64(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return global::System.Convert.ToUInt64(value, CultureInfo.InvariantCulture);
		}

		// Token: 0x06002199 RID: 8601 RVA: 0x00054341 File Offset: 0x00053341
		public float ToSingle(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return global::System.Convert.ToSingle(value, CultureInfo.InvariantCulture);
		}

		// Token: 0x0600219A RID: 8602 RVA: 0x0005435C File Offset: 0x0005335C
		public double ToDouble(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return global::System.Convert.ToDouble(value, CultureInfo.InvariantCulture);
		}

		// Token: 0x0600219B RID: 8603 RVA: 0x00054377 File Offset: 0x00053377
		public decimal ToDecimal(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return global::System.Convert.ToDecimal(value, CultureInfo.InvariantCulture);
		}

		// Token: 0x0600219C RID: 8604 RVA: 0x00054392 File Offset: 0x00053392
		public DateTime ToDateTime(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return global::System.Convert.ToDateTime(value, CultureInfo.InvariantCulture);
		}

		// Token: 0x0600219D RID: 8605 RVA: 0x000543AD File Offset: 0x000533AD
		public string ToString(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return global::System.Convert.ToString(value, CultureInfo.InvariantCulture);
		}
	}
}
