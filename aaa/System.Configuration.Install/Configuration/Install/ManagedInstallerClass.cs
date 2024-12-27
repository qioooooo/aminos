using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Configuration.Install
{
	// Token: 0x02000016 RID: 22
	[ComVisible(true)]
	[Guid("42EB0342-0393-448f-84AA-D4BEB0283595")]
	public class ManagedInstallerClass : IManagedInstaller
	{
		// Token: 0x06000084 RID: 132 RVA: 0x00004278 File Offset: 0x00003278
		int IManagedInstaller.ManagedInstall(string argString, int hInstall)
		{
			try
			{
				string[] array = ManagedInstallerClass.StringToArgs(argString);
				ManagedInstallerClass.InstallHelper(array);
			}
			catch (Exception ex)
			{
				Exception ex2 = ex;
				StringBuilder stringBuilder = new StringBuilder();
				while (ex2 != null)
				{
					stringBuilder.Append(ex2.Message);
					ex2 = ex2.InnerException;
					if (ex2 != null)
					{
						stringBuilder.Append(" --> ");
					}
				}
				int num = NativeMethods.MsiCreateRecord(2);
				if (num != 0 && NativeMethods.MsiRecordSetInteger(num, 1, 1001) == 0 && NativeMethods.MsiRecordSetStringW(num, 2, stringBuilder.ToString()) == 0)
				{
					NativeMethods.MsiProcessMessage(hInstall, 16777216, num);
				}
				return -1;
			}
			return 0;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00004320 File Offset: 0x00003320
		public static void InstallHelper(string[] args)
		{
			bool flag = false;
			bool flag2 = false;
			TransactedInstaller transactedInstaller = new TransactedInstaller();
			bool flag3 = false;
			try
			{
				ArrayList arrayList = new ArrayList();
				for (int i = 0; i < args.Length; i++)
				{
					if (args[i].StartsWith("/", StringComparison.Ordinal) || args[i].StartsWith("-", StringComparison.Ordinal))
					{
						string text = args[i].Substring(1);
						if (string.Compare(text, "u", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(text, "uninstall", StringComparison.OrdinalIgnoreCase) == 0)
						{
							flag = true;
						}
						else if (string.Compare(text, "?", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(text, "help", StringComparison.OrdinalIgnoreCase) == 0)
						{
							flag3 = true;
						}
						else if (string.Compare(text, "AssemblyName", StringComparison.OrdinalIgnoreCase) == 0)
						{
							flag2 = true;
						}
						else
						{
							arrayList.Add(args[i]);
						}
					}
					else
					{
						Assembly assembly = null;
						try
						{
							if (flag2)
							{
								assembly = Assembly.Load(args[i]);
							}
							else
							{
								assembly = Assembly.LoadFrom(args[i]);
							}
						}
						catch (Exception ex)
						{
							if (args[i].IndexOf('=') != -1)
							{
								throw new ArgumentException(Res.GetString("InstallFileDoesntExistCommandLine", new object[] { args[i] }), ex);
							}
							throw;
						}
						AssemblyInstaller assemblyInstaller = new AssemblyInstaller(assembly, (string[])arrayList.ToArray(typeof(string)));
						transactedInstaller.Installers.Add(assemblyInstaller);
					}
				}
				if (flag3 || transactedInstaller.Installers.Count == 0)
				{
					flag3 = true;
					transactedInstaller.Installers.Add(new AssemblyInstaller());
					throw new InvalidOperationException(ManagedInstallerClass.GetHelp(transactedInstaller));
				}
				transactedInstaller.Context = new InstallContext("InstallUtil.InstallLog", (string[])arrayList.ToArray(typeof(string)));
			}
			catch (Exception ex2)
			{
				if (flag3)
				{
					throw ex2;
				}
				throw new InvalidOperationException(Res.GetString("InstallInitializeException", new object[]
				{
					ex2.GetType().FullName,
					ex2.Message
				}));
			}
			try
			{
				string text2 = transactedInstaller.Context.Parameters["installtype"];
				if (text2 != null && string.Compare(text2, "notransaction", StringComparison.OrdinalIgnoreCase) == 0)
				{
					string text3 = transactedInstaller.Context.Parameters["action"];
					if (text3 != null && string.Compare(text3, "rollback", StringComparison.OrdinalIgnoreCase) == 0)
					{
						transactedInstaller.Context.LogMessage(Res.GetString("InstallRollbackNtRun"));
						for (int j = 0; j < transactedInstaller.Installers.Count; j++)
						{
							transactedInstaller.Installers[j].Rollback(null);
						}
					}
					else if (text3 != null && string.Compare(text3, "commit", StringComparison.OrdinalIgnoreCase) == 0)
					{
						transactedInstaller.Context.LogMessage(Res.GetString("InstallCommitNtRun"));
						for (int k = 0; k < transactedInstaller.Installers.Count; k++)
						{
							transactedInstaller.Installers[k].Commit(null);
						}
					}
					else if (text3 != null && string.Compare(text3, "uninstall", StringComparison.OrdinalIgnoreCase) == 0)
					{
						transactedInstaller.Context.LogMessage(Res.GetString("InstallUninstallNtRun"));
						for (int l = 0; l < transactedInstaller.Installers.Count; l++)
						{
							transactedInstaller.Installers[l].Uninstall(null);
						}
					}
					else
					{
						transactedInstaller.Context.LogMessage(Res.GetString("InstallInstallNtRun"));
						for (int m = 0; m < transactedInstaller.Installers.Count; m++)
						{
							transactedInstaller.Installers[m].Install(null);
						}
					}
				}
				else if (!flag)
				{
					IDictionary dictionary = new Hashtable();
					transactedInstaller.Install(dictionary);
				}
				else
				{
					transactedInstaller.Uninstall(null);
				}
			}
			catch (Exception ex3)
			{
				throw ex3;
			}
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00004714 File Offset: 0x00003714
		private static string GetHelp(Installer installerWithHelp)
		{
			return string.Concat(new string[]
			{
				Res.GetString("InstallHelpMessageStart"),
				Environment.NewLine,
				installerWithHelp.HelpText,
				Environment.NewLine,
				Res.GetString("InstallHelpMessageEnd"),
				Environment.NewLine
			});
		}

		// Token: 0x06000087 RID: 135 RVA: 0x0000476C File Offset: 0x0000376C
		private static string[] StringToArgs(string cmdLine)
		{
			ArrayList arrayList = new ArrayList();
			StringBuilder stringBuilder = null;
			bool flag = false;
			bool flag2 = false;
			int i = 0;
			while (i < cmdLine.Length)
			{
				char c = cmdLine[i];
				if (stringBuilder != null)
				{
					goto IL_0033;
				}
				if (!char.IsWhiteSpace(c))
				{
					stringBuilder = new StringBuilder();
					goto IL_0033;
				}
				IL_00C3:
				i++;
				continue;
				IL_0033:
				if (flag)
				{
					if (flag2)
					{
						if (c != '\\' && c != '"')
						{
							stringBuilder.Append('\\');
						}
						flag2 = false;
						stringBuilder.Append(c);
						goto IL_00C3;
					}
					if (c == '"')
					{
						flag = false;
						goto IL_00C3;
					}
					if (c == '\\')
					{
						flag2 = true;
						goto IL_00C3;
					}
					stringBuilder.Append(c);
					goto IL_00C3;
				}
				else
				{
					if (char.IsWhiteSpace(c))
					{
						arrayList.Add(stringBuilder.ToString());
						stringBuilder = null;
						flag2 = false;
						goto IL_00C3;
					}
					if (flag2)
					{
						stringBuilder.Append(c);
						flag2 = false;
						goto IL_00C3;
					}
					if (c == '^')
					{
						flag2 = true;
						goto IL_00C3;
					}
					if (c == '"')
					{
						flag = true;
						goto IL_00C3;
					}
					stringBuilder.Append(c);
					goto IL_00C3;
				}
			}
			if (stringBuilder != null)
			{
				arrayList.Add(stringBuilder.ToString());
			}
			string[] array = new string[arrayList.Count];
			arrayList.CopyTo(array);
			return array;
		}
	}
}
