using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x02000035 RID: 53
	[TypeConverter(typeof(ManagementPathConverter))]
	public class ManagementPath : ICloneable
	{
		// Token: 0x1400000A RID: 10
		// (add) Token: 0x060001A5 RID: 421 RVA: 0x00008F47 File Offset: 0x00007F47
		// (remove) Token: 0x060001A6 RID: 422 RVA: 0x00008F60 File Offset: 0x00007F60
		internal event IdentifierChangedEventHandler IdentifierChanged;

		// Token: 0x060001A7 RID: 423 RVA: 0x00008F79 File Offset: 0x00007F79
		private void FireIdentifierChanged()
		{
			if (this.IdentifierChanged != null)
			{
				this.IdentifierChanged(this, null);
			}
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00008F90 File Offset: 0x00007F90
		internal static string GetManagementPath(IWbemClassObjectFreeThreaded wbemObject)
		{
			string text = null;
			if (wbemObject != null)
			{
				int num = 0;
				int num2 = 0;
				object obj = null;
				int num3 = wbemObject.Get_("__PATH", 0, ref obj, ref num, ref num2);
				if (num3 < 0 || obj == DBNull.Value)
				{
					num3 = wbemObject.Get_("__RELPATH", 0, ref obj, ref num, ref num2);
					if (num3 < 0)
					{
						if (((long)num3 & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
						{
							ManagementException.ThrowWithExtendedInfo((ManagementStatus)num3);
						}
						else
						{
							Marshal.ThrowExceptionForHR(num3, WmiNetUtilsHelper.GetErrorInfo_f());
						}
					}
				}
				if (DBNull.Value == obj)
				{
					text = null;
				}
				else
				{
					text = (string)obj;
				}
			}
			return text;
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00009028 File Offset: 0x00008028
		internal static bool IsValidNamespaceSyntax(string nsPath)
		{
			if (nsPath.Length != 0)
			{
				char[] array = new char[] { '\\', '/' };
				if (nsPath.IndexOfAny(array) == -1 && string.Compare("root", nsPath, StringComparison.OrdinalIgnoreCase) != 0)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060001AA RID: 426 RVA: 0x0000906B File Offset: 0x0000806B
		internal static ManagementPath _Clone(ManagementPath path)
		{
			return ManagementPath._Clone(path, null);
		}

		// Token: 0x060001AB RID: 427 RVA: 0x00009074 File Offset: 0x00008074
		internal static ManagementPath _Clone(ManagementPath path, IdentifierChangedEventHandler handler)
		{
			ManagementPath managementPath = new ManagementPath();
			if (handler != null)
			{
				managementPath.IdentifierChanged = handler;
			}
			if (path != null && path.wmiPath != null)
			{
				managementPath.wmiPath = path.wmiPath;
				managementPath.isWbemPathShared = (path.isWbemPathShared = true);
			}
			return managementPath;
		}

		// Token: 0x060001AC RID: 428 RVA: 0x000090B9 File Offset: 0x000080B9
		public ManagementPath()
			: this(null)
		{
		}

		// Token: 0x060001AD RID: 429 RVA: 0x000090C2 File Offset: 0x000080C2
		public ManagementPath(string path)
		{
			if (path != null && 0 < path.Length)
			{
				this.wmiPath = this.CreateWbemPath(path);
			}
		}

		// Token: 0x060001AE RID: 430 RVA: 0x000090E3 File Offset: 0x000080E3
		public override string ToString()
		{
			return this.Path;
		}

		// Token: 0x060001AF RID: 431 RVA: 0x000090EB File Offset: 0x000080EB
		public ManagementPath Clone()
		{
			return new ManagementPath(this.Path);
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x000090F8 File Offset: 0x000080F8
		object ICloneable.Clone()
		{
			return this.Clone();
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060001B1 RID: 433 RVA: 0x00009100 File Offset: 0x00008100
		// (set) Token: 0x060001B2 RID: 434 RVA: 0x00009107 File Offset: 0x00008107
		public static ManagementPath DefaultPath
		{
			get
			{
				return ManagementPath.defaultPath;
			}
			set
			{
				ManagementPath.defaultPath = value;
			}
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00009110 File Offset: 0x00008110
		private IWbemPath CreateWbemPath(string path)
		{
			IWbemPath wbemPath = (IWbemPath)MTAHelper.CreateInMTA(typeof(WbemDefPath));
			ManagementPath.SetWbemPath(wbemPath, path);
			return wbemPath;
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x0000913A File Offset: 0x0000813A
		private void SetWbemPath(string path)
		{
			if (this.wmiPath == null)
			{
				this.wmiPath = this.CreateWbemPath(path);
				return;
			}
			ManagementPath.SetWbemPath(this.wmiPath, path);
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x00009160 File Offset: 0x00008160
		private static void SetWbemPath(IWbemPath wbemPath, string path)
		{
			if (wbemPath != null)
			{
				uint num = 4U;
				if (string.Compare(path, "root", StringComparison.OrdinalIgnoreCase) == 0)
				{
					num |= 8U;
				}
				int num2 = wbemPath.SetText_(num, path);
				if (num2 < 0)
				{
					if (((long)num2 & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
					{
						ManagementException.ThrowWithExtendedInfo((ManagementStatus)num2);
						return;
					}
					Marshal.ThrowExceptionForHR(num2, WmiNetUtilsHelper.GetErrorInfo_f());
				}
			}
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x000091B9 File Offset: 0x000081B9
		private string GetWbemPath()
		{
			return ManagementPath.GetWbemPath(this.wmiPath);
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x000091C8 File Offset: 0x000081C8
		private static string GetWbemPath(IWbemPath wbemPath)
		{
			string text = string.Empty;
			if (wbemPath != null)
			{
				int num = 4;
				uint num2 = 0U;
				int num3 = wbemPath.GetNamespaceCount_(out num2);
				if (num3 >= 0)
				{
					if (num2 == 0U)
					{
						num = 2;
					}
					uint num4 = 0U;
					num3 = wbemPath.GetText_(num, ref num4, null);
					if (num3 >= 0 && 0U < num4)
					{
						text = new string('0', (int)(num4 - 1U));
						num3 = wbemPath.GetText_(num, ref num4, text);
					}
				}
				if (num3 < 0 && num3 != -2147217400)
				{
					if (((long)num3 & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
					{
						ManagementException.ThrowWithExtendedInfo((ManagementStatus)num3);
					}
					else
					{
						Marshal.ThrowExceptionForHR(num3, WmiNetUtilsHelper.GetErrorInfo_f());
					}
				}
			}
			return text;
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x0000925C File Offset: 0x0000825C
		private void ClearKeys(bool setAsSingleton)
		{
			int num = 0;
			try
			{
				if (this.wmiPath != null)
				{
					IWbemPathKeyList wbemPathKeyList = null;
					num = this.wmiPath.GetKeyList_(out wbemPathKeyList);
					if (wbemPathKeyList != null)
					{
						num = wbemPathKeyList.RemoveAllKeys_(0U);
						if (((long)num & (long)((ulong)(-2147483648))) == 0L)
						{
							sbyte b = (setAsSingleton ? -1 : 0);
							num = wbemPathKeyList.MakeSingleton_(b);
							this.FireIdentifierChanged();
						}
					}
				}
			}
			catch (COMException ex)
			{
				ManagementException.ThrowWithExtendedInfo(ex);
			}
			if (((long)num & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
			{
				ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
				return;
			}
			if (((long)num & (long)((ulong)(-2147483648))) != 0L)
			{
				Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060001B9 RID: 441 RVA: 0x00009304 File Offset: 0x00008304
		internal bool IsEmpty
		{
			get
			{
				return this.Path.Length == 0;
			}
		}

		// Token: 0x060001BA RID: 442 RVA: 0x00009314 File Offset: 0x00008314
		public void SetAsClass()
		{
			if (this.IsClass || this.IsInstance)
			{
				if (this.isWbemPathShared)
				{
					this.wmiPath = this.CreateWbemPath(this.GetWbemPath());
					this.isWbemPathShared = false;
				}
				this.ClearKeys(false);
				return;
			}
			throw new ManagementException(ManagementStatus.InvalidOperation, null, null);
		}

		// Token: 0x060001BB RID: 443 RVA: 0x00009368 File Offset: 0x00008368
		public void SetAsSingleton()
		{
			if (this.IsClass || this.IsInstance)
			{
				if (this.isWbemPathShared)
				{
					this.wmiPath = this.CreateWbemPath(this.GetWbemPath());
					this.isWbemPathShared = false;
				}
				this.ClearKeys(true);
				return;
			}
			throw new ManagementException(ManagementStatus.InvalidOperation, null, null);
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060001BC RID: 444 RVA: 0x000093BA File Offset: 0x000083BA
		// (set) Token: 0x060001BD RID: 445 RVA: 0x000093C4 File Offset: 0x000083C4
		[RefreshProperties(RefreshProperties.All)]
		public string Path
		{
			get
			{
				return this.GetWbemPath();
			}
			set
			{
				try
				{
					if (this.isWbemPathShared)
					{
						this.wmiPath = this.CreateWbemPath(this.GetWbemPath());
						this.isWbemPathShared = false;
					}
					this.SetWbemPath(value);
				}
				catch
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.FireIdentifierChanged();
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060001BE RID: 446 RVA: 0x00009420 File Offset: 0x00008420
		// (set) Token: 0x060001BF RID: 447 RVA: 0x000094A8 File Offset: 0x000084A8
		[RefreshProperties(RefreshProperties.All)]
		public string RelativePath
		{
			get
			{
				string text = string.Empty;
				if (this.wmiPath != null)
				{
					uint num = 0U;
					int num2 = this.wmiPath.GetText_(2, ref num, null);
					if (num2 >= 0 && 0U < num)
					{
						text = new string('0', (int)(num - 1U));
						num2 = this.wmiPath.GetText_(2, ref num, text);
					}
					if (num2 < 0 && num2 != -2147217400)
					{
						if (((long)num2 & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
						{
							ManagementException.ThrowWithExtendedInfo((ManagementStatus)num2);
						}
						else
						{
							Marshal.ThrowExceptionForHR(num2, WmiNetUtilsHelper.GetErrorInfo_f());
						}
					}
				}
				return text;
			}
			set
			{
				try
				{
					this.SetRelativePath(value);
				}
				catch (COMException)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.FireIdentifierChanged();
			}
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x000094E0 File Offset: 0x000084E0
		internal void SetRelativePath(string relPath)
		{
			this.wmiPath = new ManagementPath(relPath)
			{
				NamespacePath = this.GetNamespacePath(8),
				Server = this.Server
			}.wmiPath;
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x0000951C File Offset: 0x0000851C
		internal void UpdateRelativePath(string relPath)
		{
			if (relPath == null)
			{
				return;
			}
			string text = string.Empty;
			string namespacePath = this.GetNamespacePath(8);
			if (namespacePath.Length > 0)
			{
				text = namespacePath + ":" + relPath;
			}
			else
			{
				text = relPath;
			}
			if (this.isWbemPathShared)
			{
				this.wmiPath = this.CreateWbemPath(this.GetWbemPath());
				this.isWbemPathShared = false;
			}
			this.SetWbemPath(text);
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060001C2 RID: 450 RVA: 0x00009580 File Offset: 0x00008580
		// (set) Token: 0x060001C3 RID: 451 RVA: 0x00009604 File Offset: 0x00008604
		[RefreshProperties(RefreshProperties.All)]
		public string Server
		{
			get
			{
				string text = string.Empty;
				if (this.wmiPath != null)
				{
					uint num = 0U;
					int num2 = this.wmiPath.GetServer_(ref num, null);
					if (num2 >= 0 && 0U < num)
					{
						text = new string('0', (int)(num - 1U));
						num2 = this.wmiPath.GetServer_(ref num, text);
					}
					if (num2 < 0 && num2 != -2147217399)
					{
						if (((long)num2 & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
						{
							ManagementException.ThrowWithExtendedInfo((ManagementStatus)num2);
						}
						else
						{
							Marshal.ThrowExceptionForHR(num2, WmiNetUtilsHelper.GetErrorInfo_f());
						}
					}
				}
				return text;
			}
			set
			{
				string server = this.Server;
				if (string.Compare(server, value, StringComparison.OrdinalIgnoreCase) != 0)
				{
					if (this.wmiPath == null)
					{
						this.wmiPath = (IWbemPath)MTAHelper.CreateInMTA(typeof(WbemDefPath));
					}
					else if (this.isWbemPathShared)
					{
						this.wmiPath = this.CreateWbemPath(this.GetWbemPath());
						this.isWbemPathShared = false;
					}
					int num = this.wmiPath.SetServer_(value);
					if (num < 0)
					{
						if (((long)num & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
						{
							ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
						}
						else
						{
							Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
						}
					}
					this.FireIdentifierChanged();
				}
			}
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x000096AC File Offset: 0x000086AC
		internal string SetNamespacePath(string nsPath, out bool bChange)
		{
			int num = 0;
			bChange = false;
			if (!ManagementPath.IsValidNamespaceSyntax(nsPath))
			{
				ManagementException.ThrowWithExtendedInfo(ManagementStatus.InvalidNamespace);
			}
			IWbemPath wbemPath = this.CreateWbemPath(nsPath);
			if (this.wmiPath == null)
			{
				this.wmiPath = this.CreateWbemPath("");
			}
			else if (this.isWbemPathShared)
			{
				this.wmiPath = this.CreateWbemPath(this.GetWbemPath());
				this.isWbemPathShared = false;
			}
			string namespacePath = ManagementPath.GetNamespacePath(this.wmiPath, 16);
			string namespacePath2 = ManagementPath.GetNamespacePath(wbemPath, 16);
			if (string.Compare(namespacePath, namespacePath2, StringComparison.OrdinalIgnoreCase) != 0)
			{
				this.wmiPath.RemoveAllNamespaces_();
				bChange = true;
				uint num2 = 0U;
				num = wbemPath.GetNamespaceCount_(out num2);
				if (num >= 0)
				{
					for (uint num3 = 0U; num3 < num2; num3 += 1U)
					{
						uint num4 = 0U;
						num = wbemPath.GetNamespaceAt_(num3, ref num4, null);
						if (num < 0)
						{
							break;
						}
						string text = new string('0', (int)(num4 - 1U));
						num = wbemPath.GetNamespaceAt_(num3, ref num4, text);
						if (num < 0)
						{
							break;
						}
						num = this.wmiPath.SetNamespaceAt_(num3, text);
						if (num < 0)
						{
							break;
						}
					}
				}
			}
			if (num >= 0 && nsPath.Length > 1 && ((nsPath[0] == '\\' && nsPath[1] == '\\') || (nsPath[0] == '/' && nsPath[1] == '/')))
			{
				uint num5 = 0U;
				num = wbemPath.GetServer_(ref num5, null);
				if (num >= 0 && num5 > 0U)
				{
					string text2 = new string('0', (int)(num5 - 1U));
					num = wbemPath.GetServer_(ref num5, text2);
					if (num >= 0)
					{
						num5 = 0U;
						num = this.wmiPath.GetServer_(ref num5, null);
						if (num >= 0)
						{
							string text3 = new string('0', (int)(num5 - 1U));
							num = this.wmiPath.GetServer_(ref num5, text3);
							if (num >= 0 && string.Compare(text3, text2, StringComparison.OrdinalIgnoreCase) != 0)
							{
								num = this.wmiPath.SetServer_(text2);
							}
						}
						else if (num == -2147217399)
						{
							num = this.wmiPath.SetServer_(text2);
							if (num >= 0)
							{
								bChange = true;
							}
						}
					}
				}
				else if (num == -2147217399)
				{
					num = 0;
				}
			}
			if (num < 0)
			{
				if (((long)num & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
				{
					ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
				}
				else
				{
					Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
				}
			}
			return namespacePath2;
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x000098D7 File Offset: 0x000088D7
		internal string GetNamespacePath(int flags)
		{
			return ManagementPath.GetNamespacePath(this.wmiPath, flags);
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x000098E8 File Offset: 0x000088E8
		internal static string GetNamespacePath(IWbemPath wbemPath, int flags)
		{
			string text = string.Empty;
			if (wbemPath != null)
			{
				uint num = 0U;
				int num2 = wbemPath.GetNamespaceCount_(out num);
				if (num2 >= 0 && num > 0U)
				{
					uint num3 = 0U;
					num2 = wbemPath.GetText_(flags, ref num3, null);
					if (num2 >= 0 && num3 > 0U)
					{
						text = new string('0', (int)(num3 - 1U));
						num2 = wbemPath.GetText_(flags, ref num3, text);
					}
				}
				if (num2 < 0 && num2 != -2147217400)
				{
					if (((long)num2 & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
					{
						ManagementException.ThrowWithExtendedInfo((ManagementStatus)num2);
					}
					else
					{
						Marshal.ThrowExceptionForHR(num2, WmiNetUtilsHelper.GetErrorInfo_f());
					}
				}
			}
			return text;
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060001C7 RID: 455 RVA: 0x00009974 File Offset: 0x00008974
		// (set) Token: 0x060001C8 RID: 456 RVA: 0x00009980 File Offset: 0x00008980
		[RefreshProperties(RefreshProperties.All)]
		public string NamespacePath
		{
			get
			{
				return this.GetNamespacePath(16);
			}
			set
			{
				bool flag = false;
				try
				{
					this.SetNamespacePath(value, out flag);
				}
				catch (COMException)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				if (flag)
				{
					this.FireIdentifierChanged();
				}
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060001C9 RID: 457 RVA: 0x000099C0 File Offset: 0x000089C0
		// (set) Token: 0x060001CA RID: 458 RVA: 0x000099C8 File Offset: 0x000089C8
		[RefreshProperties(RefreshProperties.All)]
		public string ClassName
		{
			get
			{
				return this.internalClassName;
			}
			set
			{
				string className = this.ClassName;
				if (string.Compare(className, value, StringComparison.OrdinalIgnoreCase) != 0)
				{
					this.internalClassName = value;
					this.FireIdentifierChanged();
				}
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060001CB RID: 459 RVA: 0x000099F4 File Offset: 0x000089F4
		// (set) Token: 0x060001CC RID: 460 RVA: 0x00009A50 File Offset: 0x00008A50
		internal string internalClassName
		{
			get
			{
				string text = string.Empty;
				if (this.wmiPath != null)
				{
					uint num = 0U;
					int num2 = this.wmiPath.GetClassName_(ref num, null);
					if (num2 >= 0 && 0U < num)
					{
						text = new string('0', (int)(num - 1U));
						num2 = this.wmiPath.GetClassName_(ref num, text);
						if (num2 < 0)
						{
							text = string.Empty;
						}
					}
				}
				return text;
			}
			set
			{
				int num = 0;
				if (this.wmiPath == null)
				{
					this.wmiPath = (IWbemPath)MTAHelper.CreateInMTA(typeof(WbemDefPath));
				}
				else if (this.isWbemPathShared)
				{
					this.wmiPath = this.CreateWbemPath(this.GetWbemPath());
					this.isWbemPathShared = false;
				}
				try
				{
					num = this.wmiPath.SetClassName_(value);
				}
				catch (COMException)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				if (num < 0)
				{
					if (((long)num & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
					{
						ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
						return;
					}
					Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
				}
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060001CD RID: 461 RVA: 0x00009AFC File Offset: 0x00008AFC
		public bool IsClass
		{
			get
			{
				if (this.wmiPath == null)
				{
					return false;
				}
				ulong num = 0UL;
				int info_ = this.wmiPath.GetInfo_(0U, out num);
				if (info_ < 0)
				{
					if (((long)info_ & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
					{
						ManagementException.ThrowWithExtendedInfo((ManagementStatus)info_);
					}
					else
					{
						Marshal.ThrowExceptionForHR(info_, WmiNetUtilsHelper.GetErrorInfo_f());
					}
				}
				return 0UL != (num & 4UL);
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060001CE RID: 462 RVA: 0x00009B60 File Offset: 0x00008B60
		public bool IsInstance
		{
			get
			{
				if (this.wmiPath == null)
				{
					return false;
				}
				ulong num = 0UL;
				int info_ = this.wmiPath.GetInfo_(0U, out num);
				if (info_ < 0)
				{
					if (((long)info_ & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
					{
						ManagementException.ThrowWithExtendedInfo((ManagementStatus)info_);
					}
					else
					{
						Marshal.ThrowExceptionForHR(info_, WmiNetUtilsHelper.GetErrorInfo_f());
					}
				}
				return 0UL != (num & 8UL);
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060001CF RID: 463 RVA: 0x00009BC4 File Offset: 0x00008BC4
		public bool IsSingleton
		{
			get
			{
				if (this.wmiPath == null)
				{
					return false;
				}
				ulong num = 0UL;
				int info_ = this.wmiPath.GetInfo_(0U, out num);
				if (info_ < 0)
				{
					if (((long)info_ & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
					{
						ManagementException.ThrowWithExtendedInfo((ManagementStatus)info_);
					}
					else
					{
						Marshal.ThrowExceptionForHR(info_, WmiNetUtilsHelper.GetErrorInfo_f());
					}
				}
				return 0UL != (num & 4096UL);
			}
		}

		// Token: 0x0400014B RID: 331
		private static ManagementPath defaultPath = new ManagementPath("//./root/cimv2");

		// Token: 0x0400014C RID: 332
		private bool isWbemPathShared;

		// Token: 0x0400014E RID: 334
		private IWbemPath wmiPath;
	}
}
