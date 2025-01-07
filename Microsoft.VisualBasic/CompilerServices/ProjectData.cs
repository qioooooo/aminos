using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Security;
using System.Security.Permissions;

namespace Microsoft.VisualBasic.CompilerServices
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public sealed class ProjectData
	{
		private ProjectData()
		{
			this.m_rndSeed = 327680;
			this.m_CachedMSCoreLibAssembly = typeof(int).Assembly;
			this.m_AssemblyData = new Hashtable();
			this.m_numprsPtr = new byte[24];
			this.m_DigitArray = new byte[30];
		}

		internal AssemblyData GetAssemblyData(Assembly assem)
		{
			if (assem == Utils.VBRuntimeAssembly || assem == this.m_CachedMSCoreLibAssembly)
			{
				throw new SecurityException(Utils.GetResourceString("Security_LateBoundCallsNotPermitted"));
			}
			AssemblyData assemblyData = (AssemblyData)this.m_AssemblyData[assem];
			if (assemblyData == null)
			{
				assemblyData = new AssemblyData();
				this.m_AssemblyData[assem] = assemblyData;
			}
			return assemblyData;
		}

		internal static ProjectData GetProjectData()
		{
			ProjectData projectData = ProjectData.m_oProject;
			if (projectData == null)
			{
				projectData = new ProjectData();
				ProjectData.m_oProject = projectData;
			}
			return projectData;
		}

		public static Exception CreateProjectError(int hr)
		{
			ErrObject errObject = Information.Err();
			errObject.Clear();
			int num = errObject.MapErrorNumber(hr);
			return errObject.CreateException(hr, Utils.GetResourceString((vbErrors)num));
		}

		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static void SetProjectError(Exception ex)
		{
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				Information.Err().CaptureException(ex);
			}
		}

		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static void SetProjectError(Exception ex, int lErl)
		{
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				Information.Err().CaptureException(ex, lErl);
			}
		}

		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static void ClearProjectError()
		{
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				Information.Err().Clear();
			}
		}

		[HostProtection(SecurityAction.LinkDemand, Resources = HostProtectionResource.SelfAffectingProcessMgmt)]
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void EndApp()
		{
			FileSystem.CloseAllFiles(Assembly.GetCallingAssembly());
			Environment.Exit(0);
		}

		internal ErrObject m_Err;

		internal int m_rndSeed;

		internal byte[] m_numprsPtr;

		internal byte[] m_DigitArray;

		internal Hashtable m_AssemblyData;

		[ThreadStatic]
		private static ProjectData m_oProject;

		private Assembly m_CachedMSCoreLibAssembly;
	}
}
