using System;
using System.Collections;
using System.Configuration.Install;
using System.IO;
using System.Reflection;
using System.Security.Permissions;
using System.Text;

namespace System.Management.Instrumentation
{
	// Token: 0x020000AA RID: 170
	public class ManagementInstaller : Installer
	{
		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060004EB RID: 1259 RVA: 0x00023934 File Offset: 0x00022934
		public override string HelpText
		{
			get
			{
				if (ManagementInstaller.helpPrinted)
				{
					return base.HelpText;
				}
				ManagementInstaller.helpPrinted = true;
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("/MOF=[filename]\r\n");
				stringBuilder.Append(" " + RC.GetString("FILETOWRITE_MOF") + "\r\n\r\n");
				stringBuilder.Append("/Force or /F\r\n");
				stringBuilder.Append(" " + RC.GetString("FORCE_UPDATE"));
				return stringBuilder.ToString() + base.HelpText;
			}
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x000239C0 File Offset: 0x000229C0
		public override void Install(IDictionary savedState)
		{
			FileIOPermission fileIOPermission = new FileIOPermission(FileIOPermissionAccess.Read, base.Context.Parameters["assemblypath"]);
			fileIOPermission.Demand();
			base.Install(savedState);
			base.Context.LogMessage(RC.GetString("WMISCHEMA_INSTALLATIONSTART"));
			string text = base.Context.Parameters["assemblypath"];
			Assembly assembly = Assembly.LoadFrom(text);
			SchemaNaming schemaNaming = SchemaNaming.GetSchemaNaming(assembly);
			schemaNaming.DecoupledProviderInstanceName = AssemblyNameUtility.UniqueToAssemblyFullVersion(assembly);
			if (schemaNaming == null)
			{
				return;
			}
			if (!schemaNaming.IsAssemblyRegistered() || base.Context.Parameters.ContainsKey("force") || base.Context.Parameters.ContainsKey("f"))
			{
				base.Context.LogMessage(RC.GetString("REGESTRING_ASSEMBLY") + " " + schemaNaming.DecoupledProviderInstanceName);
				schemaNaming.RegisterNonAssemblySpecificSchema(base.Context);
				schemaNaming.RegisterAssemblySpecificSchema();
			}
			this.mof = schemaNaming.Mof;
			base.Context.LogMessage(RC.GetString("WMISCHEMA_INSTALLATIONEND"));
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x00023AD0 File Offset: 0x00022AD0
		public override void Commit(IDictionary savedState)
		{
			base.Commit(savedState);
			if (base.Context.Parameters.ContainsKey("mof"))
			{
				string text = base.Context.Parameters["mof"];
				if (text == null || text.Length == 0)
				{
					text = base.Context.Parameters["assemblypath"];
					if (text == null || text.Length == 0)
					{
						text = "defaultmoffile";
					}
					else
					{
						text = Path.GetFileName(text);
					}
				}
				if (text.Length < 4)
				{
					text += ".mof";
				}
				else if (string.Compare(text.Substring(text.Length - 4, 4), ".mof", StringComparison.OrdinalIgnoreCase) != 0)
				{
					text += ".mof";
				}
				base.Context.LogMessage(RC.GetString("MOFFILE_GENERATING") + " " + text);
				using (StreamWriter streamWriter = new StreamWriter(text, false, Encoding.Unicode))
				{
					streamWriter.WriteLine("//**************************************************************************");
					streamWriter.WriteLine("//* {0}", text);
					streamWriter.WriteLine("//**************************************************************************");
					streamWriter.WriteLine(this.mof);
				}
			}
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x00023C08 File Offset: 0x00022C08
		public override void Rollback(IDictionary savedState)
		{
			base.Rollback(savedState);
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x00023C11 File Offset: 0x00022C11
		public override void Uninstall(IDictionary savedState)
		{
			base.Uninstall(savedState);
		}

		// Token: 0x040002A2 RID: 674
		private static bool helpPrinted;

		// Token: 0x040002A3 RID: 675
		private string mof;
	}
}
