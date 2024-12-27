using System;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;

namespace System.Drawing.Printing
{
	// Token: 0x0200010A RID: 266
	[Serializable]
	public class InvalidPrinterException : SystemException
	{
		// Token: 0x06000E30 RID: 3632 RVA: 0x00029B9C File Offset: 0x00028B9C
		public InvalidPrinterException(PrinterSettings settings)
			: base(InvalidPrinterException.GenerateMessage(settings))
		{
			this.settings = settings;
		}

		// Token: 0x06000E31 RID: 3633 RVA: 0x00029BB1 File Offset: 0x00028BB1
		protected InvalidPrinterException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this.settings = (PrinterSettings)info.GetValue("settings", typeof(PrinterSettings));
		}

		// Token: 0x06000E32 RID: 3634 RVA: 0x00029BDB File Offset: 0x00028BDB
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			IntSecurity.AllPrinting.Demand();
			info.AddValue("settings", this.settings);
			base.GetObjectData(info, context);
		}

		// Token: 0x06000E33 RID: 3635 RVA: 0x00029C10 File Offset: 0x00028C10
		private static string GenerateMessage(PrinterSettings settings)
		{
			if (settings.IsDefaultPrinter)
			{
				return SR.GetString("InvalidPrinterException_NoDefaultPrinter");
			}
			string text;
			try
			{
				text = SR.GetString("InvalidPrinterException_InvalidPrinter", new object[] { settings.PrinterName });
			}
			catch (SecurityException)
			{
				text = SR.GetString("InvalidPrinterException_InvalidPrinter", new object[] { SR.GetString("CantTellPrinterName") });
			}
			return text;
		}

		// Token: 0x04000B83 RID: 2947
		private PrinterSettings settings;
	}
}
