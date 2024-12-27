using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Drawing.Printing
{
	// Token: 0x02000106 RID: 262
	public abstract class PrintController
	{
		// Token: 0x06000E1B RID: 3611 RVA: 0x00029493 File Offset: 0x00028493
		protected PrintController()
		{
			IntSecurity.SafePrinting.Demand();
		}

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x06000E1C RID: 3612 RVA: 0x000294A5 File Offset: 0x000284A5
		public virtual bool IsPreview
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000E1D RID: 3613 RVA: 0x000294A8 File Offset: 0x000284A8
		internal void Print(PrintDocument document)
		{
			IntSecurity.SafePrinting.Demand();
			PrintAction printAction;
			if (this.IsPreview)
			{
				printAction = PrintAction.PrintToPreview;
			}
			else
			{
				printAction = (document.PrinterSettings.PrintToFile ? PrintAction.PrintToFile : PrintAction.PrintToPrinter);
			}
			PrintEventArgs printEventArgs = new PrintEventArgs(printAction);
			document._OnBeginPrint(printEventArgs);
			if (printEventArgs.Cancel)
			{
				document._OnEndPrint(printEventArgs);
				return;
			}
			this.OnStartPrint(document, printEventArgs);
			if (printEventArgs.Cancel)
			{
				document._OnEndPrint(printEventArgs);
				this.OnEndPrint(document, printEventArgs);
				return;
			}
			bool flag = true;
			try
			{
				flag = this.PrintLoop(document);
			}
			finally
			{
				try
				{
					try
					{
						document._OnEndPrint(printEventArgs);
						printEventArgs.Cancel = flag | printEventArgs.Cancel;
					}
					finally
					{
						this.OnEndPrint(document, printEventArgs);
					}
				}
				finally
				{
					if (!IntSecurity.HasPermission(IntSecurity.AllPrinting))
					{
						IntSecurity.AllPrinting.Assert();
						document.PrinterSettings.PrintDialogDisplayed = false;
					}
				}
			}
		}

		// Token: 0x06000E1E RID: 3614 RVA: 0x00029598 File Offset: 0x00028598
		private bool PrintLoop(PrintDocument document)
		{
			QueryPageSettingsEventArgs queryPageSettingsEventArgs = new QueryPageSettingsEventArgs((PageSettings)document.DefaultPageSettings.Clone());
			for (;;)
			{
				document._OnQueryPageSettings(queryPageSettingsEventArgs);
				if (queryPageSettingsEventArgs.Cancel)
				{
					break;
				}
				PrintPageEventArgs printPageEventArgs = this.CreatePrintPageEvent(queryPageSettingsEventArgs.PageSettings);
				Graphics graphics = this.OnStartPage(document, printPageEventArgs);
				printPageEventArgs.SetGraphics(graphics);
				try
				{
					document._OnPrintPage(printPageEventArgs);
					this.OnEndPage(document, printPageEventArgs);
				}
				finally
				{
					printPageEventArgs.Dispose();
				}
				if (printPageEventArgs.Cancel)
				{
					return true;
				}
				if (!printPageEventArgs.HasMorePages)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000E1F RID: 3615 RVA: 0x00029624 File Offset: 0x00028624
		private PrintPageEventArgs CreatePrintPageEvent(PageSettings pageSettings)
		{
			IntSecurity.AllPrintingAndUnmanagedCode.Assert();
			Rectangle bounds = pageSettings.GetBounds(this.modeHandle);
			Rectangle rectangle = new Rectangle(pageSettings.Margins.Left, pageSettings.Margins.Top, bounds.Width - (pageSettings.Margins.Left + pageSettings.Margins.Right), bounds.Height - (pageSettings.Margins.Top + pageSettings.Margins.Bottom));
			return new PrintPageEventArgs(null, rectangle, bounds, pageSettings);
		}

		// Token: 0x06000E20 RID: 3616 RVA: 0x000296B3 File Offset: 0x000286B3
		public virtual void OnStartPrint(PrintDocument document, PrintEventArgs e)
		{
			IntSecurity.AllPrintingAndUnmanagedCode.Assert();
			this.modeHandle = (PrintController.SafeDeviceModeHandle)document.PrinterSettings.GetHdevmode(document.DefaultPageSettings);
		}

		// Token: 0x06000E21 RID: 3617 RVA: 0x000296DB File Offset: 0x000286DB
		public virtual Graphics OnStartPage(PrintDocument document, PrintPageEventArgs e)
		{
			return null;
		}

		// Token: 0x06000E22 RID: 3618 RVA: 0x000296DE File Offset: 0x000286DE
		public virtual void OnEndPage(PrintDocument document, PrintPageEventArgs e)
		{
		}

		// Token: 0x06000E23 RID: 3619 RVA: 0x000296E0 File Offset: 0x000286E0
		public virtual void OnEndPrint(PrintDocument document, PrintEventArgs e)
		{
			IntSecurity.UnmanagedCode.Assert();
			if (this.modeHandle != null)
			{
				this.modeHandle.Close();
			}
		}

		// Token: 0x04000B7B RID: 2939
		internal PrintController.SafeDeviceModeHandle modeHandle;

		// Token: 0x02000107 RID: 263
		[SecurityCritical]
		internal sealed class SafeDeviceModeHandle : SafeHandle
		{
			// Token: 0x06000E24 RID: 3620 RVA: 0x000296FF File Offset: 0x000286FF
			private SafeDeviceModeHandle()
				: base(IntPtr.Zero, true)
			{
			}

			// Token: 0x06000E25 RID: 3621 RVA: 0x0002970D File Offset: 0x0002870D
			internal SafeDeviceModeHandle(IntPtr handle)
				: base(IntPtr.Zero, true)
			{
				base.SetHandle(handle);
			}

			// Token: 0x17000390 RID: 912
			// (get) Token: 0x06000E26 RID: 3622 RVA: 0x00029722 File Offset: 0x00028722
			public override bool IsInvalid
			{
				get
				{
					return this.handle == IntPtr.Zero;
				}
			}

			// Token: 0x06000E27 RID: 3623 RVA: 0x00029734 File Offset: 0x00028734
			[SecurityCritical]
			protected override bool ReleaseHandle()
			{
				if (!this.IsInvalid)
				{
					SafeNativeMethods.GlobalFree(new HandleRef(this, this.handle));
				}
				this.handle = IntPtr.Zero;
				return true;
			}

			// Token: 0x06000E28 RID: 3624 RVA: 0x0002975C File Offset: 0x0002875C
			public static implicit operator IntPtr(PrintController.SafeDeviceModeHandle handle)
			{
				if (handle != null)
				{
					return handle.handle;
				}
				return IntPtr.Zero;
			}

			// Token: 0x06000E29 RID: 3625 RVA: 0x0002976D File Offset: 0x0002876D
			public static explicit operator PrintController.SafeDeviceModeHandle(IntPtr handle)
			{
				return new PrintController.SafeDeviceModeHandle(handle);
			}
		}
	}
}
