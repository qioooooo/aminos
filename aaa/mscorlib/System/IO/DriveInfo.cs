using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using Microsoft.Win32;

namespace System.IO
{
	// Token: 0x0200059B RID: 1435
	[ComVisible(true)]
	[Serializable]
	public sealed class DriveInfo : ISerializable
	{
		// Token: 0x0600354B RID: 13643 RVA: 0x000B26D4 File Offset: 0x000B16D4
		public DriveInfo(string driveName)
		{
			if (driveName == null)
			{
				throw new ArgumentNullException("driveName");
			}
			if (driveName.Length == 1)
			{
				this._name = driveName + ":\\";
			}
			else
			{
				Path.CheckInvalidPathChars(driveName);
				this._name = Path.GetPathRoot(driveName);
				if (this._name == null || this._name.Length == 0 || this._name.StartsWith("\\\\", StringComparison.Ordinal))
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_MustBeDriveLetterOrRootDir"));
				}
			}
			if (this._name.Length == 2 && this._name[1] == ':')
			{
				this._name += "\\";
			}
			char c = driveName[0];
			if ((c < 'A' || c > 'Z') && (c < 'a' || c > 'z'))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeDriveLetterOrRootDir"));
			}
			string text = this._name + '.';
			new FileIOPermission(FileIOPermissionAccess.PathDiscovery, text).Demand();
		}

		// Token: 0x0600354C RID: 13644 RVA: 0x000B27DC File Offset: 0x000B17DC
		private DriveInfo(SerializationInfo info, StreamingContext context)
		{
			this._name = (string)info.GetValue("_name", typeof(string));
			string text = this._name + '.';
			new FileIOPermission(FileIOPermissionAccess.PathDiscovery, text).Demand();
		}

		// Token: 0x17000917 RID: 2327
		// (get) Token: 0x0600354D RID: 13645 RVA: 0x000B282E File Offset: 0x000B182E
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x17000918 RID: 2328
		// (get) Token: 0x0600354E RID: 13646 RVA: 0x000B2836 File Offset: 0x000B1836
		public DriveType DriveType
		{
			get
			{
				return (DriveType)Win32Native.GetDriveType(this.Name);
			}
		}

		// Token: 0x17000919 RID: 2329
		// (get) Token: 0x0600354F RID: 13647 RVA: 0x000B2844 File Offset: 0x000B1844
		public string DriveFormat
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder(50);
				StringBuilder stringBuilder2 = new StringBuilder(50);
				int num = Win32Native.SetErrorMode(1);
				try
				{
					int num2;
					int num3;
					int num4;
					if (!Win32Native.GetVolumeInformation(this.Name, stringBuilder, 50, out num2, out num3, out num4, stringBuilder2, 50))
					{
						int num5 = Marshal.GetLastWin32Error();
						if (num5 == 13)
						{
							num5 = 15;
						}
						__Error.WinIODriveError(this.Name, num5);
					}
				}
				finally
				{
					Win32Native.SetErrorMode(num);
				}
				return stringBuilder2.ToString();
			}
		}

		// Token: 0x1700091A RID: 2330
		// (get) Token: 0x06003550 RID: 13648 RVA: 0x000B28C8 File Offset: 0x000B18C8
		public bool IsReady
		{
			get
			{
				return Directory.InternalExists(this.Name);
			}
		}

		// Token: 0x1700091B RID: 2331
		// (get) Token: 0x06003551 RID: 13649 RVA: 0x000B28D8 File Offset: 0x000B18D8
		public long AvailableFreeSpace
		{
			get
			{
				int num = Win32Native.SetErrorMode(1);
				long num2;
				try
				{
					long num3;
					long num4;
					if (!Win32Native.GetDiskFreeSpaceEx(this.Name, out num2, out num3, out num4))
					{
						__Error.WinIODriveError(this.Name);
					}
				}
				finally
				{
					Win32Native.SetErrorMode(num);
				}
				return num2;
			}
		}

		// Token: 0x1700091C RID: 2332
		// (get) Token: 0x06003552 RID: 13650 RVA: 0x000B292C File Offset: 0x000B192C
		public long TotalFreeSpace
		{
			get
			{
				int num = Win32Native.SetErrorMode(1);
				long num4;
				try
				{
					long num2;
					long num3;
					if (!Win32Native.GetDiskFreeSpaceEx(this.Name, out num2, out num3, out num4))
					{
						__Error.WinIODriveError(this.Name);
					}
				}
				finally
				{
					Win32Native.SetErrorMode(num);
				}
				return num4;
			}
		}

		// Token: 0x1700091D RID: 2333
		// (get) Token: 0x06003553 RID: 13651 RVA: 0x000B2980 File Offset: 0x000B1980
		public long TotalSize
		{
			get
			{
				int num = Win32Native.SetErrorMode(1);
				long num3;
				try
				{
					long num2;
					long num4;
					if (!Win32Native.GetDiskFreeSpaceEx(this.Name, out num2, out num3, out num4))
					{
						__Error.WinIODriveError(this.Name);
					}
				}
				finally
				{
					Win32Native.SetErrorMode(num);
				}
				return num3;
			}
		}

		// Token: 0x06003554 RID: 13652 RVA: 0x000B29D4 File Offset: 0x000B19D4
		public static DriveInfo[] GetDrives()
		{
			string[] logicalDrives = Directory.GetLogicalDrives();
			DriveInfo[] array = new DriveInfo[logicalDrives.Length];
			for (int i = 0; i < logicalDrives.Length; i++)
			{
				array[i] = new DriveInfo(logicalDrives[i]);
			}
			return array;
		}

		// Token: 0x1700091E RID: 2334
		// (get) Token: 0x06003555 RID: 13653 RVA: 0x000B2A0A File Offset: 0x000B1A0A
		public DirectoryInfo RootDirectory
		{
			get
			{
				return new DirectoryInfo(this.Name);
			}
		}

		// Token: 0x1700091F RID: 2335
		// (get) Token: 0x06003556 RID: 13654 RVA: 0x000B2A18 File Offset: 0x000B1A18
		// (set) Token: 0x06003557 RID: 13655 RVA: 0x000B2A9C File Offset: 0x000B1A9C
		public string VolumeLabel
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder(50);
				StringBuilder stringBuilder2 = new StringBuilder(50);
				int num = Win32Native.SetErrorMode(1);
				try
				{
					int num2;
					int num3;
					int num4;
					if (!Win32Native.GetVolumeInformation(this.Name, stringBuilder, 50, out num2, out num3, out num4, stringBuilder2, 50))
					{
						int num5 = Marshal.GetLastWin32Error();
						if (num5 == 13)
						{
							num5 = 15;
						}
						__Error.WinIODriveError(this.Name, num5);
					}
				}
				finally
				{
					Win32Native.SetErrorMode(num);
				}
				return stringBuilder.ToString();
			}
			set
			{
				string text = this._name + '.';
				new FileIOPermission(FileIOPermissionAccess.Write, text).Demand();
				int num = Win32Native.SetErrorMode(1);
				try
				{
					if (!Win32Native.SetVolumeLabel(this.Name, value))
					{
						int lastWin32Error = Marshal.GetLastWin32Error();
						if (lastWin32Error == 5)
						{
							throw new UnauthorizedAccessException(Environment.GetResourceString("InvalidOperation_SetVolumeLabelFailed"));
						}
						__Error.WinIODriveError(this.Name, lastWin32Error);
					}
				}
				finally
				{
					Win32Native.SetErrorMode(num);
				}
			}
		}

		// Token: 0x06003558 RID: 13656 RVA: 0x000B2B20 File Offset: 0x000B1B20
		public override string ToString()
		{
			return this.Name;
		}

		// Token: 0x06003559 RID: 13657 RVA: 0x000B2B28 File Offset: 0x000B1B28
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("_name", this._name, typeof(string));
		}

		// Token: 0x04001BDC RID: 7132
		private const string NameField = "_name";

		// Token: 0x04001BDD RID: 7133
		private string _name;
	}
}
