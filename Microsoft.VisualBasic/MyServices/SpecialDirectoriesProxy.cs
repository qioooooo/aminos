using System;
using System.ComponentModel;
using System.Security.Permissions;
using Microsoft.VisualBasic.FileIO;

namespace Microsoft.VisualBasic.MyServices
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	[HostProtection(SecurityAction.LinkDemand, Resources = HostProtectionResource.ExternalProcessMgmt)]
	public class SpecialDirectoriesProxy
	{
		public string MyDocuments
		{
			get
			{
				return SpecialDirectories.MyDocuments;
			}
		}

		public string MyMusic
		{
			get
			{
				return SpecialDirectories.MyMusic;
			}
		}

		public string MyPictures
		{
			get
			{
				return SpecialDirectories.MyPictures;
			}
		}

		public string Desktop
		{
			get
			{
				return SpecialDirectories.Desktop;
			}
		}

		public string Programs
		{
			get
			{
				return SpecialDirectories.Programs;
			}
		}

		public string ProgramFiles
		{
			get
			{
				return SpecialDirectories.ProgramFiles;
			}
		}

		public string Temp
		{
			get
			{
				return SpecialDirectories.Temp;
			}
		}

		public string CurrentUserApplicationData
		{
			get
			{
				return SpecialDirectories.CurrentUserApplicationData;
			}
		}

		public string AllUsersApplicationData
		{
			get
			{
				return SpecialDirectories.AllUsersApplicationData;
			}
		}

		internal SpecialDirectoriesProxy()
		{
		}
	}
}
