using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using Microsoft.Win32;

namespace System.Diagnostics
{
	// Token: 0x0200075C RID: 1884
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public sealed class FileVersionInfo
	{
		// Token: 0x060039C2 RID: 14786 RVA: 0x000F4AAC File Offset: 0x000F3AAC
		private FileVersionInfo(string fileName)
		{
			this.fileName = fileName;
		}

		// Token: 0x17000D64 RID: 3428
		// (get) Token: 0x060039C3 RID: 14787 RVA: 0x000F4ABB File Offset: 0x000F3ABB
		public string Comments
		{
			get
			{
				return this.comments;
			}
		}

		// Token: 0x17000D65 RID: 3429
		// (get) Token: 0x060039C4 RID: 14788 RVA: 0x000F4AC3 File Offset: 0x000F3AC3
		public string CompanyName
		{
			get
			{
				return this.companyName;
			}
		}

		// Token: 0x17000D66 RID: 3430
		// (get) Token: 0x060039C5 RID: 14789 RVA: 0x000F4ACB File Offset: 0x000F3ACB
		public int FileBuildPart
		{
			get
			{
				return this.fileBuild;
			}
		}

		// Token: 0x17000D67 RID: 3431
		// (get) Token: 0x060039C6 RID: 14790 RVA: 0x000F4AD3 File Offset: 0x000F3AD3
		public string FileDescription
		{
			get
			{
				return this.fileDescription;
			}
		}

		// Token: 0x17000D68 RID: 3432
		// (get) Token: 0x060039C7 RID: 14791 RVA: 0x000F4ADB File Offset: 0x000F3ADB
		public int FileMajorPart
		{
			get
			{
				return this.fileMajor;
			}
		}

		// Token: 0x17000D69 RID: 3433
		// (get) Token: 0x060039C8 RID: 14792 RVA: 0x000F4AE3 File Offset: 0x000F3AE3
		public int FileMinorPart
		{
			get
			{
				return this.fileMinor;
			}
		}

		// Token: 0x17000D6A RID: 3434
		// (get) Token: 0x060039C9 RID: 14793 RVA: 0x000F4AEB File Offset: 0x000F3AEB
		public string FileName
		{
			get
			{
				new FileIOPermission(FileIOPermissionAccess.PathDiscovery, this.fileName).Demand();
				return this.fileName;
			}
		}

		// Token: 0x17000D6B RID: 3435
		// (get) Token: 0x060039CA RID: 14794 RVA: 0x000F4B04 File Offset: 0x000F3B04
		public int FilePrivatePart
		{
			get
			{
				return this.filePrivate;
			}
		}

		// Token: 0x17000D6C RID: 3436
		// (get) Token: 0x060039CB RID: 14795 RVA: 0x000F4B0C File Offset: 0x000F3B0C
		public string FileVersion
		{
			get
			{
				return this.fileVersion;
			}
		}

		// Token: 0x17000D6D RID: 3437
		// (get) Token: 0x060039CC RID: 14796 RVA: 0x000F4B14 File Offset: 0x000F3B14
		public string InternalName
		{
			get
			{
				return this.internalName;
			}
		}

		// Token: 0x17000D6E RID: 3438
		// (get) Token: 0x060039CD RID: 14797 RVA: 0x000F4B1C File Offset: 0x000F3B1C
		public bool IsDebug
		{
			get
			{
				return (this.fileFlags & 1) != 0;
			}
		}

		// Token: 0x17000D6F RID: 3439
		// (get) Token: 0x060039CE RID: 14798 RVA: 0x000F4B2C File Offset: 0x000F3B2C
		public bool IsPatched
		{
			get
			{
				return (this.fileFlags & 4) != 0;
			}
		}

		// Token: 0x17000D70 RID: 3440
		// (get) Token: 0x060039CF RID: 14799 RVA: 0x000F4B3C File Offset: 0x000F3B3C
		public bool IsPrivateBuild
		{
			get
			{
				return (this.fileFlags & 8) != 0;
			}
		}

		// Token: 0x17000D71 RID: 3441
		// (get) Token: 0x060039D0 RID: 14800 RVA: 0x000F4B4C File Offset: 0x000F3B4C
		public bool IsPreRelease
		{
			get
			{
				return (this.fileFlags & 2) != 0;
			}
		}

		// Token: 0x17000D72 RID: 3442
		// (get) Token: 0x060039D1 RID: 14801 RVA: 0x000F4B5C File Offset: 0x000F3B5C
		public bool IsSpecialBuild
		{
			get
			{
				return (this.fileFlags & 32) != 0;
			}
		}

		// Token: 0x17000D73 RID: 3443
		// (get) Token: 0x060039D2 RID: 14802 RVA: 0x000F4B6D File Offset: 0x000F3B6D
		public string Language
		{
			get
			{
				return this.language;
			}
		}

		// Token: 0x17000D74 RID: 3444
		// (get) Token: 0x060039D3 RID: 14803 RVA: 0x000F4B75 File Offset: 0x000F3B75
		public string LegalCopyright
		{
			get
			{
				return this.legalCopyright;
			}
		}

		// Token: 0x17000D75 RID: 3445
		// (get) Token: 0x060039D4 RID: 14804 RVA: 0x000F4B7D File Offset: 0x000F3B7D
		public string LegalTrademarks
		{
			get
			{
				return this.legalTrademarks;
			}
		}

		// Token: 0x17000D76 RID: 3446
		// (get) Token: 0x060039D5 RID: 14805 RVA: 0x000F4B85 File Offset: 0x000F3B85
		public string OriginalFilename
		{
			get
			{
				return this.originalFilename;
			}
		}

		// Token: 0x17000D77 RID: 3447
		// (get) Token: 0x060039D6 RID: 14806 RVA: 0x000F4B8D File Offset: 0x000F3B8D
		public string PrivateBuild
		{
			get
			{
				return this.privateBuild;
			}
		}

		// Token: 0x17000D78 RID: 3448
		// (get) Token: 0x060039D7 RID: 14807 RVA: 0x000F4B95 File Offset: 0x000F3B95
		public int ProductBuildPart
		{
			get
			{
				return this.productBuild;
			}
		}

		// Token: 0x17000D79 RID: 3449
		// (get) Token: 0x060039D8 RID: 14808 RVA: 0x000F4B9D File Offset: 0x000F3B9D
		public int ProductMajorPart
		{
			get
			{
				return this.productMajor;
			}
		}

		// Token: 0x17000D7A RID: 3450
		// (get) Token: 0x060039D9 RID: 14809 RVA: 0x000F4BA5 File Offset: 0x000F3BA5
		public int ProductMinorPart
		{
			get
			{
				return this.productMinor;
			}
		}

		// Token: 0x17000D7B RID: 3451
		// (get) Token: 0x060039DA RID: 14810 RVA: 0x000F4BAD File Offset: 0x000F3BAD
		public string ProductName
		{
			get
			{
				return this.productName;
			}
		}

		// Token: 0x17000D7C RID: 3452
		// (get) Token: 0x060039DB RID: 14811 RVA: 0x000F4BB5 File Offset: 0x000F3BB5
		public int ProductPrivatePart
		{
			get
			{
				return this.productPrivate;
			}
		}

		// Token: 0x17000D7D RID: 3453
		// (get) Token: 0x060039DC RID: 14812 RVA: 0x000F4BBD File Offset: 0x000F3BBD
		public string ProductVersion
		{
			get
			{
				return this.productVersion;
			}
		}

		// Token: 0x17000D7E RID: 3454
		// (get) Token: 0x060039DD RID: 14813 RVA: 0x000F4BC5 File Offset: 0x000F3BC5
		public string SpecialBuild
		{
			get
			{
				return this.specialBuild;
			}
		}

		// Token: 0x060039DE RID: 14814 RVA: 0x000F4BD0 File Offset: 0x000F3BD0
		private static string ConvertTo8DigitHex(int value)
		{
			string text = Convert.ToString(value, 16);
			text = text.ToUpper(CultureInfo.InvariantCulture);
			if (text.Length == 8)
			{
				return text;
			}
			StringBuilder stringBuilder = new StringBuilder(8);
			for (int i = text.Length; i < 8; i++)
			{
				stringBuilder.Append("0");
			}
			stringBuilder.Append(text);
			return stringBuilder.ToString();
		}

		// Token: 0x060039DF RID: 14815 RVA: 0x000F4C30 File Offset: 0x000F3C30
		private static NativeMethods.VS_FIXEDFILEINFO GetFixedFileInfo(IntPtr memPtr)
		{
			IntPtr zero = IntPtr.Zero;
			int num;
			if (UnsafeNativeMethods.VerQueryValue(new HandleRef(null, memPtr), "\\", ref zero, out num))
			{
				NativeMethods.VS_FIXEDFILEINFO vs_FIXEDFILEINFO = new NativeMethods.VS_FIXEDFILEINFO();
				Marshal.PtrToStructure(zero, vs_FIXEDFILEINFO);
				return vs_FIXEDFILEINFO;
			}
			return new NativeMethods.VS_FIXEDFILEINFO();
		}

		// Token: 0x060039E0 RID: 14816 RVA: 0x000F4C70 File Offset: 0x000F3C70
		private static string GetFileVersionLanguage(IntPtr memPtr)
		{
			int num = FileVersionInfo.GetVarEntry(memPtr) >> 16;
			StringBuilder stringBuilder = new StringBuilder(256);
			UnsafeNativeMethods.VerLanguageName(num, stringBuilder, stringBuilder.Capacity);
			return stringBuilder.ToString();
		}

		// Token: 0x060039E1 RID: 14817 RVA: 0x000F4CA8 File Offset: 0x000F3CA8
		private static string GetFileVersionString(IntPtr memPtr, string name)
		{
			string text = "";
			IntPtr zero = IntPtr.Zero;
			int num;
			if (UnsafeNativeMethods.VerQueryValue(new HandleRef(null, memPtr), name, ref zero, out num) && zero != IntPtr.Zero)
			{
				text = Marshal.PtrToStringAuto(zero);
			}
			return text;
		}

		// Token: 0x060039E2 RID: 14818 RVA: 0x000F4CEC File Offset: 0x000F3CEC
		private static int GetVarEntry(IntPtr memPtr)
		{
			IntPtr zero = IntPtr.Zero;
			int num;
			if (UnsafeNativeMethods.VerQueryValue(new HandleRef(null, memPtr), "\\VarFileInfo\\Translation", ref zero, out num))
			{
				return ((int)Marshal.ReadInt16(zero) << 16) + (int)Marshal.ReadInt16((IntPtr)((long)zero + 2L));
			}
			return 67699940;
		}

		// Token: 0x060039E3 RID: 14819 RVA: 0x000F4D3C File Offset: 0x000F3D3C
		private bool GetVersionInfoForCodePage(IntPtr memIntPtr, string codepage)
		{
			string text = "\\\\StringFileInfo\\\\{0}\\\\{1}";
			this.companyName = FileVersionInfo.GetFileVersionString(memIntPtr, string.Format(CultureInfo.InvariantCulture, text, new object[] { codepage, "CompanyName" }));
			this.fileDescription = FileVersionInfo.GetFileVersionString(memIntPtr, string.Format(CultureInfo.InvariantCulture, text, new object[] { codepage, "FileDescription" }));
			this.fileVersion = FileVersionInfo.GetFileVersionString(memIntPtr, string.Format(CultureInfo.InvariantCulture, text, new object[] { codepage, "FileVersion" }));
			this.internalName = FileVersionInfo.GetFileVersionString(memIntPtr, string.Format(CultureInfo.InvariantCulture, text, new object[] { codepage, "InternalName" }));
			this.legalCopyright = FileVersionInfo.GetFileVersionString(memIntPtr, string.Format(CultureInfo.InvariantCulture, text, new object[] { codepage, "LegalCopyright" }));
			this.originalFilename = FileVersionInfo.GetFileVersionString(memIntPtr, string.Format(CultureInfo.InvariantCulture, text, new object[] { codepage, "OriginalFilename" }));
			this.productName = FileVersionInfo.GetFileVersionString(memIntPtr, string.Format(CultureInfo.InvariantCulture, text, new object[] { codepage, "ProductName" }));
			this.productVersion = FileVersionInfo.GetFileVersionString(memIntPtr, string.Format(CultureInfo.InvariantCulture, text, new object[] { codepage, "ProductVersion" }));
			this.comments = FileVersionInfo.GetFileVersionString(memIntPtr, string.Format(CultureInfo.InvariantCulture, text, new object[] { codepage, "Comments" }));
			this.legalTrademarks = FileVersionInfo.GetFileVersionString(memIntPtr, string.Format(CultureInfo.InvariantCulture, text, new object[] { codepage, "LegalTrademarks" }));
			this.privateBuild = FileVersionInfo.GetFileVersionString(memIntPtr, string.Format(CultureInfo.InvariantCulture, text, new object[] { codepage, "PrivateBuild" }));
			this.specialBuild = FileVersionInfo.GetFileVersionString(memIntPtr, string.Format(CultureInfo.InvariantCulture, text, new object[] { codepage, "SpecialBuild" }));
			this.language = FileVersionInfo.GetFileVersionLanguage(memIntPtr);
			NativeMethods.VS_FIXEDFILEINFO fixedFileInfo = FileVersionInfo.GetFixedFileInfo(memIntPtr);
			this.fileMajor = FileVersionInfo.HIWORD(fixedFileInfo.dwFileVersionMS);
			this.fileMinor = FileVersionInfo.LOWORD(fixedFileInfo.dwFileVersionMS);
			this.fileBuild = FileVersionInfo.HIWORD(fixedFileInfo.dwFileVersionLS);
			this.filePrivate = FileVersionInfo.LOWORD(fixedFileInfo.dwFileVersionLS);
			this.productMajor = FileVersionInfo.HIWORD(fixedFileInfo.dwProductVersionMS);
			this.productMinor = FileVersionInfo.LOWORD(fixedFileInfo.dwProductVersionMS);
			this.productBuild = FileVersionInfo.HIWORD(fixedFileInfo.dwProductVersionLS);
			this.productPrivate = FileVersionInfo.LOWORD(fixedFileInfo.dwProductVersionLS);
			this.fileFlags = fixedFileInfo.dwFileFlags;
			return this.fileVersion != string.Empty;
		}

		// Token: 0x060039E4 RID: 14820 RVA: 0x000F5032 File Offset: 0x000F4032
		[FileIOPermission(SecurityAction.Assert, AllFiles = FileIOPermissionAccess.PathDiscovery)]
		private static string GetFullPathWithAssert(string fileName)
		{
			return Path.GetFullPath(fileName);
		}

		// Token: 0x060039E5 RID: 14821 RVA: 0x000F504C File Offset: 0x000F404C
		public unsafe static FileVersionInfo GetVersionInfo(string fileName)
		{
			if (!File.Exists(fileName))
			{
				string fullPathWithAssert = FileVersionInfo.GetFullPathWithAssert(fileName);
				new FileIOPermission(FileIOPermissionAccess.Read, fullPathWithAssert).Demand();
				throw new FileNotFoundException(fileName);
			}
			int num;
			int fileVersionInfoSize = UnsafeNativeMethods.GetFileVersionInfoSize(fileName, out num);
			FileVersionInfo fileVersionInfo = new FileVersionInfo(fileName);
			if (fileVersionInfoSize != 0)
			{
				byte[] array = new byte[fileVersionInfoSize];
				fixed (byte* ptr = array)
				{
					IntPtr intPtr = new IntPtr((void*)ptr);
					if (UnsafeNativeMethods.GetFileVersionInfo(fileName, 0, fileVersionInfoSize, new HandleRef(null, intPtr)))
					{
						int varEntry = FileVersionInfo.GetVarEntry(intPtr);
						if (!fileVersionInfo.GetVersionInfoForCodePage(intPtr, FileVersionInfo.ConvertTo8DigitHex(varEntry)))
						{
							int[] array2 = new int[] { 67699888, 67699940, 67698688 };
							foreach (int num2 in array2)
							{
								if (num2 != varEntry && fileVersionInfo.GetVersionInfoForCodePage(intPtr, FileVersionInfo.ConvertTo8DigitHex(num2)))
								{
									break;
								}
							}
						}
					}
				}
			}
			return fileVersionInfo;
		}

		// Token: 0x060039E6 RID: 14822 RVA: 0x000F5139 File Offset: 0x000F4139
		private static int HIWORD(int dword)
		{
			return NativeMethods.Util.HIWORD(dword);
		}

		// Token: 0x060039E7 RID: 14823 RVA: 0x000F5141 File Offset: 0x000F4141
		private static int LOWORD(int dword)
		{
			return NativeMethods.Util.LOWORD(dword);
		}

		// Token: 0x060039E8 RID: 14824 RVA: 0x000F514C File Offset: 0x000F414C
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(128);
			string text = "\r\n";
			stringBuilder.Append("File:             ");
			stringBuilder.Append(this.FileName);
			stringBuilder.Append(text);
			stringBuilder.Append("InternalName:     ");
			stringBuilder.Append(this.InternalName);
			stringBuilder.Append(text);
			stringBuilder.Append("OriginalFilename: ");
			stringBuilder.Append(this.OriginalFilename);
			stringBuilder.Append(text);
			stringBuilder.Append("FileVersion:      ");
			stringBuilder.Append(this.FileVersion);
			stringBuilder.Append(text);
			stringBuilder.Append("FileDescription:  ");
			stringBuilder.Append(this.FileDescription);
			stringBuilder.Append(text);
			stringBuilder.Append("Product:          ");
			stringBuilder.Append(this.ProductName);
			stringBuilder.Append(text);
			stringBuilder.Append("ProductVersion:   ");
			stringBuilder.Append(this.ProductVersion);
			stringBuilder.Append(text);
			stringBuilder.Append("Debug:            ");
			stringBuilder.Append(this.IsDebug.ToString());
			stringBuilder.Append(text);
			stringBuilder.Append("Patched:          ");
			stringBuilder.Append(this.IsPatched.ToString());
			stringBuilder.Append(text);
			stringBuilder.Append("PreRelease:       ");
			stringBuilder.Append(this.IsPreRelease.ToString());
			stringBuilder.Append(text);
			stringBuilder.Append("PrivateBuild:     ");
			stringBuilder.Append(this.IsPrivateBuild.ToString());
			stringBuilder.Append(text);
			stringBuilder.Append("SpecialBuild:     ");
			stringBuilder.Append(this.IsSpecialBuild.ToString());
			stringBuilder.Append(text);
			stringBuilder.Append("Language:         ");
			stringBuilder.Append(this.Language);
			stringBuilder.Append(text);
			return stringBuilder.ToString();
		}

		// Token: 0x040032CD RID: 13005
		private string fileName;

		// Token: 0x040032CE RID: 13006
		private string companyName;

		// Token: 0x040032CF RID: 13007
		private string fileDescription;

		// Token: 0x040032D0 RID: 13008
		private string fileVersion;

		// Token: 0x040032D1 RID: 13009
		private string internalName;

		// Token: 0x040032D2 RID: 13010
		private string legalCopyright;

		// Token: 0x040032D3 RID: 13011
		private string originalFilename;

		// Token: 0x040032D4 RID: 13012
		private string productName;

		// Token: 0x040032D5 RID: 13013
		private string productVersion;

		// Token: 0x040032D6 RID: 13014
		private string comments;

		// Token: 0x040032D7 RID: 13015
		private string legalTrademarks;

		// Token: 0x040032D8 RID: 13016
		private string privateBuild;

		// Token: 0x040032D9 RID: 13017
		private string specialBuild;

		// Token: 0x040032DA RID: 13018
		private string language;

		// Token: 0x040032DB RID: 13019
		private int fileMajor;

		// Token: 0x040032DC RID: 13020
		private int fileMinor;

		// Token: 0x040032DD RID: 13021
		private int fileBuild;

		// Token: 0x040032DE RID: 13022
		private int filePrivate;

		// Token: 0x040032DF RID: 13023
		private int productMajor;

		// Token: 0x040032E0 RID: 13024
		private int productMinor;

		// Token: 0x040032E1 RID: 13025
		private int productBuild;

		// Token: 0x040032E2 RID: 13026
		private int productPrivate;

		// Token: 0x040032E3 RID: 13027
		private int fileFlags;
	}
}
