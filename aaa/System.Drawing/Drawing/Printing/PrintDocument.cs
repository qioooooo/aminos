using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Security;
using System.Security.Permissions;

namespace System.Drawing.Printing
{
	// Token: 0x02000116 RID: 278
	[DefaultEvent("PrintPage")]
	[ToolboxItemFilter("System.Drawing.Printing")]
	[DefaultProperty("DocumentName")]
	[SRDescription("PrintDocumentDesc")]
	public class PrintDocument : Component
	{
		// Token: 0x06000E8D RID: 3725 RVA: 0x0002B452 File Offset: 0x0002A452
		public PrintDocument()
		{
			this.defaultPageSettings = new PageSettings(this.printerSettings);
		}

		// Token: 0x170003AD RID: 941
		// (get) Token: 0x06000E8E RID: 3726 RVA: 0x0002B481 File Offset: 0x0002A481
		// (set) Token: 0x06000E8F RID: 3727 RVA: 0x0002B489 File Offset: 0x0002A489
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("PDOCdocumentPageSettingsDescr")]
		public PageSettings DefaultPageSettings
		{
			get
			{
				return this.defaultPageSettings;
			}
			set
			{
				if (value == null)
				{
					value = new PageSettings();
				}
				this.defaultPageSettings = value;
				this.userSetPageSettings = true;
			}
		}

		// Token: 0x170003AE RID: 942
		// (get) Token: 0x06000E90 RID: 3728 RVA: 0x0002B4A3 File Offset: 0x0002A4A3
		// (set) Token: 0x06000E91 RID: 3729 RVA: 0x0002B4AB File Offset: 0x0002A4AB
		[SRDescription("PDOCdocumentNameDescr")]
		[DefaultValue("document")]
		public string DocumentName
		{
			get
			{
				return this.documentName;
			}
			set
			{
				if (value == null)
				{
					value = "";
				}
				this.documentName = value;
			}
		}

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x06000E92 RID: 3730 RVA: 0x0002B4BE File Offset: 0x0002A4BE
		// (set) Token: 0x06000E93 RID: 3731 RVA: 0x0002B4C6 File Offset: 0x0002A4C6
		[SRDescription("PDOCoriginAtMarginsDescr")]
		[DefaultValue(false)]
		public bool OriginAtMargins
		{
			get
			{
				return this.originAtMargins;
			}
			set
			{
				this.originAtMargins = value;
			}
		}

		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x06000E94 RID: 3732 RVA: 0x0002B4D0 File Offset: 0x0002A4D0
		// (set) Token: 0x06000E95 RID: 3733 RVA: 0x0002B5B4 File Offset: 0x0002A5B4
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("PDOCprintControllerDescr")]
		[Browsable(false)]
		public PrintController PrintController
		{
			get
			{
				IntSecurity.SafePrinting.Demand();
				if (this.printController == null)
				{
					this.printController = new StandardPrintController();
					new ReflectionPermission(PermissionState.Unrestricted).Assert();
					try
					{
						Type type = Type.GetType("System.Windows.Forms.PrintControllerWithStatusDialog, System.Windows.Forms, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
						this.printController = (PrintController)Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, new object[] { this.printController }, null);
					}
					catch (TypeLoadException)
					{
					}
					catch (TargetInvocationException)
					{
					}
					catch (MissingMethodException)
					{
					}
					catch (MethodAccessException)
					{
					}
					catch (MemberAccessException)
					{
					}
					catch (FileNotFoundException)
					{
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
				}
				return this.printController;
			}
			set
			{
				IntSecurity.SafePrinting.Demand();
				this.printController = value;
			}
		}

		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x06000E96 RID: 3734 RVA: 0x0002B5C7 File Offset: 0x0002A5C7
		// (set) Token: 0x06000E97 RID: 3735 RVA: 0x0002B5CF File Offset: 0x0002A5CF
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("PDOCprinterSettingsDescr")]
		[Browsable(false)]
		public PrinterSettings PrinterSettings
		{
			get
			{
				return this.printerSettings;
			}
			set
			{
				if (value == null)
				{
					value = new PrinterSettings();
				}
				this.printerSettings = value;
				if (!this.userSetPageSettings)
				{
					this.defaultPageSettings = this.printerSettings.DefaultPageSettings;
				}
			}
		}

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000E98 RID: 3736 RVA: 0x0002B5FB File Offset: 0x0002A5FB
		// (remove) Token: 0x06000E99 RID: 3737 RVA: 0x0002B614 File Offset: 0x0002A614
		[SRDescription("PDOCbeginPrintDescr")]
		public event PrintEventHandler BeginPrint
		{
			add
			{
				this.beginPrintHandler = (PrintEventHandler)Delegate.Combine(this.beginPrintHandler, value);
			}
			remove
			{
				this.beginPrintHandler = (PrintEventHandler)Delegate.Remove(this.beginPrintHandler, value);
			}
		}

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000E9A RID: 3738 RVA: 0x0002B62D File Offset: 0x0002A62D
		// (remove) Token: 0x06000E9B RID: 3739 RVA: 0x0002B646 File Offset: 0x0002A646
		[SRDescription("PDOCendPrintDescr")]
		public event PrintEventHandler EndPrint
		{
			add
			{
				this.endPrintHandler = (PrintEventHandler)Delegate.Combine(this.endPrintHandler, value);
			}
			remove
			{
				this.endPrintHandler = (PrintEventHandler)Delegate.Remove(this.endPrintHandler, value);
			}
		}

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000E9C RID: 3740 RVA: 0x0002B65F File Offset: 0x0002A65F
		// (remove) Token: 0x06000E9D RID: 3741 RVA: 0x0002B678 File Offset: 0x0002A678
		[SRDescription("PDOCprintPageDescr")]
		public event PrintPageEventHandler PrintPage
		{
			add
			{
				this.printPageHandler = (PrintPageEventHandler)Delegate.Combine(this.printPageHandler, value);
			}
			remove
			{
				this.printPageHandler = (PrintPageEventHandler)Delegate.Remove(this.printPageHandler, value);
			}
		}

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06000E9E RID: 3742 RVA: 0x0002B691 File Offset: 0x0002A691
		// (remove) Token: 0x06000E9F RID: 3743 RVA: 0x0002B6AA File Offset: 0x0002A6AA
		[SRDescription("PDOCqueryPageSettingsDescr")]
		public event QueryPageSettingsEventHandler QueryPageSettings
		{
			add
			{
				this.queryHandler = (QueryPageSettingsEventHandler)Delegate.Combine(this.queryHandler, value);
			}
			remove
			{
				this.queryHandler = (QueryPageSettingsEventHandler)Delegate.Remove(this.queryHandler, value);
			}
		}

		// Token: 0x06000EA0 RID: 3744 RVA: 0x0002B6C3 File Offset: 0x0002A6C3
		internal void _OnBeginPrint(PrintEventArgs e)
		{
			this.OnBeginPrint(e);
		}

		// Token: 0x06000EA1 RID: 3745 RVA: 0x0002B6CC File Offset: 0x0002A6CC
		protected virtual void OnBeginPrint(PrintEventArgs e)
		{
			if (this.beginPrintHandler != null)
			{
				this.beginPrintHandler(this, e);
			}
		}

		// Token: 0x06000EA2 RID: 3746 RVA: 0x0002B6E3 File Offset: 0x0002A6E3
		internal void _OnEndPrint(PrintEventArgs e)
		{
			this.OnEndPrint(e);
		}

		// Token: 0x06000EA3 RID: 3747 RVA: 0x0002B6EC File Offset: 0x0002A6EC
		protected virtual void OnEndPrint(PrintEventArgs e)
		{
			if (this.endPrintHandler != null)
			{
				this.endPrintHandler(this, e);
			}
		}

		// Token: 0x06000EA4 RID: 3748 RVA: 0x0002B703 File Offset: 0x0002A703
		internal void _OnPrintPage(PrintPageEventArgs e)
		{
			this.OnPrintPage(e);
		}

		// Token: 0x06000EA5 RID: 3749 RVA: 0x0002B70C File Offset: 0x0002A70C
		protected virtual void OnPrintPage(PrintPageEventArgs e)
		{
			if (this.printPageHandler != null)
			{
				this.printPageHandler(this, e);
			}
		}

		// Token: 0x06000EA6 RID: 3750 RVA: 0x0002B723 File Offset: 0x0002A723
		internal void _OnQueryPageSettings(QueryPageSettingsEventArgs e)
		{
			this.OnQueryPageSettings(e);
		}

		// Token: 0x06000EA7 RID: 3751 RVA: 0x0002B72C File Offset: 0x0002A72C
		protected virtual void OnQueryPageSettings(QueryPageSettingsEventArgs e)
		{
			if (this.queryHandler != null)
			{
				this.queryHandler(this, e);
			}
		}

		// Token: 0x06000EA8 RID: 3752 RVA: 0x0002B744 File Offset: 0x0002A744
		public void Print()
		{
			if (!this.PrinterSettings.IsDefaultPrinter && !this.PrinterSettings.PrintDialogDisplayed)
			{
				IntSecurity.AllPrinting.Demand();
			}
			PrintController printController = this.PrintController;
			printController.Print(this);
		}

		// Token: 0x06000EA9 RID: 3753 RVA: 0x0002B783 File Offset: 0x0002A783
		public override string ToString()
		{
			return "[PrintDocument " + this.DocumentName + "]";
		}

		// Token: 0x04000C32 RID: 3122
		private string documentName = "document";

		// Token: 0x04000C33 RID: 3123
		private PrintEventHandler beginPrintHandler;

		// Token: 0x04000C34 RID: 3124
		private PrintEventHandler endPrintHandler;

		// Token: 0x04000C35 RID: 3125
		private PrintPageEventHandler printPageHandler;

		// Token: 0x04000C36 RID: 3126
		private QueryPageSettingsEventHandler queryHandler;

		// Token: 0x04000C37 RID: 3127
		private PrinterSettings printerSettings = new PrinterSettings();

		// Token: 0x04000C38 RID: 3128
		private PageSettings defaultPageSettings;

		// Token: 0x04000C39 RID: 3129
		private PrintController printController;

		// Token: 0x04000C3A RID: 3130
		private bool originAtMargins;

		// Token: 0x04000C3B RID: 3131
		private bool userSetPageSettings;
	}
}
