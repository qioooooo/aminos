using System;
using System.IO;
using System.Security.Permissions;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;

namespace Microsoft.VisualBasic.FileIO
{
	[HostProtection(SecurityAction.LinkDemand, Resources = HostProtectionResource.ExternalProcessMgmt)]
	public class SpecialDirectories
	{
		public static string MyDocuments
		{
			get
			{
				return SpecialDirectories.GetDirectoryPath(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "IO_SpecialDirectory_MyDocuments");
			}
		}

		public static string MyMusic
		{
			get
			{
				return SpecialDirectories.GetDirectoryPath(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic), "IO_SpecialDirectory_MyMusic");
			}
		}

		public static string MyPictures
		{
			get
			{
				return SpecialDirectories.GetDirectoryPath(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "IO_SpecialDirectory_MyPictures");
			}
		}

		public static string Desktop
		{
			get
			{
				return SpecialDirectories.GetDirectoryPath(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "IO_SpecialDirectory_Desktop");
			}
		}

		public static string Programs
		{
			get
			{
				return SpecialDirectories.GetDirectoryPath(Environment.GetFolderPath(Environment.SpecialFolder.Programs), "IO_SpecialDirectory_Programs");
			}
		}

		public static string ProgramFiles
		{
			get
			{
				return SpecialDirectories.GetDirectoryPath(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "IO_SpecialDirectory_ProgramFiles");
			}
		}

		public static string Temp
		{
			get
			{
				return SpecialDirectories.GetDirectoryPath(Path.GetTempPath(), "IO_SpecialDirectory_Temp");
			}
		}

		public static string CurrentUserApplicationData
		{
			get
			{
				return SpecialDirectories.GetDirectoryPath(Application.UserAppDataPath, "IO_SpecialDirectory_UserAppData");
			}
		}

		public static string AllUsersApplicationData
		{
			get
			{
				return SpecialDirectories.GetDirectoryPath(Application.CommonAppDataPath, "IO_SpecialDirectory_AllUserAppData");
			}
		}

		private static string GetDirectoryPath(string Directory, string DirectoryNameResID)
		{
			if (Operators.CompareString(Directory, "", false) == 0)
			{
				throw ExceptionUtils.GetDirectoryNotFoundException("IO_SpecialDirectoryNotExist", new string[] { Utils.GetResourceString(DirectoryNameResID) });
			}
			return FileSystem.NormalizePath(Directory);
		}
	}
}
