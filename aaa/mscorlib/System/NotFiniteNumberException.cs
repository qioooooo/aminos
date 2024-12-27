using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System
{
	// Token: 0x020000DA RID: 218
	[ComVisible(true)]
	[Serializable]
	public class NotFiniteNumberException : ArithmeticException
	{
		// Token: 0x06000C1C RID: 3100 RVA: 0x00024129 File Offset: 0x00023129
		public NotFiniteNumberException()
			: base(Environment.GetResourceString("Arg_NotFiniteNumberException"))
		{
			this._offendingNumber = 0.0;
			base.SetErrorCode(-2146233048);
		}

		// Token: 0x06000C1D RID: 3101 RVA: 0x00024155 File Offset: 0x00023155
		public NotFiniteNumberException(double offendingNumber)
		{
			this._offendingNumber = offendingNumber;
			base.SetErrorCode(-2146233048);
		}

		// Token: 0x06000C1E RID: 3102 RVA: 0x0002416F File Offset: 0x0002316F
		public NotFiniteNumberException(string message)
			: base(message)
		{
			this._offendingNumber = 0.0;
			base.SetErrorCode(-2146233048);
		}

		// Token: 0x06000C1F RID: 3103 RVA: 0x00024192 File Offset: 0x00023192
		public NotFiniteNumberException(string message, double offendingNumber)
			: base(message)
		{
			this._offendingNumber = offendingNumber;
			base.SetErrorCode(-2146233048);
		}

		// Token: 0x06000C20 RID: 3104 RVA: 0x000241AD File Offset: 0x000231AD
		public NotFiniteNumberException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.SetErrorCode(-2146233048);
		}

		// Token: 0x06000C21 RID: 3105 RVA: 0x000241C2 File Offset: 0x000231C2
		public NotFiniteNumberException(string message, double offendingNumber, Exception innerException)
			: base(message, innerException)
		{
			this._offendingNumber = offendingNumber;
			base.SetErrorCode(-2146233048);
		}

		// Token: 0x06000C22 RID: 3106 RVA: 0x000241DE File Offset: 0x000231DE
		protected NotFiniteNumberException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this._offendingNumber = (double)info.GetInt32("OffendingNumber");
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06000C23 RID: 3107 RVA: 0x000241FA File Offset: 0x000231FA
		public double OffendingNumber
		{
			get
			{
				return this._offendingNumber;
			}
		}

		// Token: 0x06000C24 RID: 3108 RVA: 0x00024202 File Offset: 0x00023202
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			base.GetObjectData(info, context);
			info.AddValue("OffendingNumber", this._offendingNumber, typeof(int));
		}

		// Token: 0x0400041E RID: 1054
		private double _offendingNumber;
	}
}
