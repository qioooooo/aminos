using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System
{
	// Token: 0x0200006F RID: 111
	[ComVisible(true)]
	[Serializable]
	public class ArgumentOutOfRangeException : ArgumentException, ISerializable
	{
		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000654 RID: 1620 RVA: 0x00015AC1 File Offset: 0x00014AC1
		private static string RangeMessage
		{
			get
			{
				if (ArgumentOutOfRangeException._rangeMessage == null)
				{
					ArgumentOutOfRangeException._rangeMessage = Environment.GetResourceString("Arg_ArgumentOutOfRangeException");
				}
				return ArgumentOutOfRangeException._rangeMessage;
			}
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x00015ADE File Offset: 0x00014ADE
		public ArgumentOutOfRangeException()
			: base(ArgumentOutOfRangeException.RangeMessage)
		{
			base.SetErrorCode(-2146233086);
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x00015AF6 File Offset: 0x00014AF6
		public ArgumentOutOfRangeException(string paramName)
			: base(ArgumentOutOfRangeException.RangeMessage, paramName)
		{
			base.SetErrorCode(-2146233086);
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x00015B0F File Offset: 0x00014B0F
		public ArgumentOutOfRangeException(string paramName, string message)
			: base(message, paramName)
		{
			base.SetErrorCode(-2146233086);
		}

		// Token: 0x06000658 RID: 1624 RVA: 0x00015B24 File Offset: 0x00014B24
		public ArgumentOutOfRangeException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.SetErrorCode(-2146233086);
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x00015B39 File Offset: 0x00014B39
		public ArgumentOutOfRangeException(string paramName, object actualValue, string message)
			: base(message, paramName)
		{
			this.m_actualValue = actualValue;
			base.SetErrorCode(-2146233086);
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x0600065A RID: 1626 RVA: 0x00015B58 File Offset: 0x00014B58
		public override string Message
		{
			get
			{
				string message = base.Message;
				if (this.m_actualValue == null)
				{
					return message;
				}
				string text = string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_ActualValue"), new object[] { this.m_actualValue.ToString() });
				if (message == null)
				{
					return text;
				}
				return message + Environment.NewLine + text;
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x0600065B RID: 1627 RVA: 0x00015BB2 File Offset: 0x00014BB2
		public virtual object ActualValue
		{
			get
			{
				return this.m_actualValue;
			}
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x00015BBA File Offset: 0x00014BBA
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			base.GetObjectData(info, context);
			info.AddValue("ActualValue", this.m_actualValue, typeof(object));
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x00015BED File Offset: 0x00014BED
		protected ArgumentOutOfRangeException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this.m_actualValue = info.GetValue("ActualValue", typeof(object));
		}

		// Token: 0x040001F7 RID: 503
		private static string _rangeMessage;

		// Token: 0x040001F8 RID: 504
		private object m_actualValue;
	}
}
