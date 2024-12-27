using System;
using System.Deployment.Internal.Isolation.Manifest;
using System.IO;
using System.Reflection;
using System.Security.Permissions;
using System.Threading;

namespace System.Runtime.Hosting
{
	// Token: 0x02000048 RID: 72
	internal sealed class ManifestRunner
	{
		// Token: 0x06000413 RID: 1043 RVA: 0x00010660 File Offset: 0x0000F660
		[SecurityPermission(SecurityAction.Assert, Unrestricted = true)]
		internal ManifestRunner(AppDomain domain, ActivationContext activationContext)
		{
			this.m_domain = domain;
			string text;
			string text2;
			CmsUtils.GetEntryPoint(activationContext, out text, out text2);
			if (string.IsNullOrEmpty(text))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_NoMain"));
			}
			if (string.IsNullOrEmpty(text2))
			{
				this.m_args = new string[0];
			}
			else
			{
				this.m_args = text2.Split(new char[] { ' ' });
			}
			this.m_apt = ApartmentState.Unknown;
			string applicationDirectory = activationContext.ApplicationDirectory;
			this.m_path = Path.Combine(applicationDirectory, text);
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000414 RID: 1044 RVA: 0x000106E6 File Offset: 0x0000F6E6
		internal Assembly EntryAssembly
		{
			[SecurityPermission(SecurityAction.Assert, Unrestricted = true)]
			[FileIOPermission(SecurityAction.Assert, Unrestricted = true)]
			get
			{
				if (this.m_assembly == null)
				{
					this.m_assembly = Assembly.LoadFrom(this.m_path);
				}
				return this.m_assembly;
			}
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x00010707 File Offset: 0x0000F707
		private void NewThreadRunner()
		{
			this.m_runResult = this.Run(false);
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x00010718 File Offset: 0x0000F718
		private int RunInNewThread()
		{
			Thread thread = new Thread(new ThreadStart(this.NewThreadRunner));
			thread.SetApartmentState(this.m_apt);
			thread.Start();
			thread.Join();
			return this.m_runResult;
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x00010758 File Offset: 0x0000F758
		private int Run(bool checkAptModel)
		{
			if (checkAptModel && this.m_apt != ApartmentState.Unknown)
			{
				if (Thread.CurrentThread.GetApartmentState() != ApartmentState.Unknown && Thread.CurrentThread.GetApartmentState() != this.m_apt)
				{
					return this.RunInNewThread();
				}
				Thread.CurrentThread.SetApartmentState(this.m_apt);
			}
			return this.m_domain.nExecuteAssembly(this.EntryAssembly, this.m_args);
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x000107C0 File Offset: 0x0000F7C0
		internal int ExecuteAsAssembly()
		{
			object[] array = this.EntryAssembly.EntryPoint.GetCustomAttributes(typeof(STAThreadAttribute), false);
			if (array.Length > 0)
			{
				this.m_apt = ApartmentState.STA;
			}
			array = this.EntryAssembly.EntryPoint.GetCustomAttributes(typeof(MTAThreadAttribute), false);
			if (array.Length > 0)
			{
				if (this.m_apt == ApartmentState.Unknown)
				{
					this.m_apt = ApartmentState.MTA;
				}
				else
				{
					this.m_apt = ApartmentState.Unknown;
				}
			}
			return this.Run(true);
		}

		// Token: 0x04000187 RID: 391
		private AppDomain m_domain;

		// Token: 0x04000188 RID: 392
		private string m_path;

		// Token: 0x04000189 RID: 393
		private string[] m_args;

		// Token: 0x0400018A RID: 394
		private ApartmentState m_apt;

		// Token: 0x0400018B RID: 395
		private Assembly m_assembly;

		// Token: 0x0400018C RID: 396
		private int m_runResult;
	}
}
