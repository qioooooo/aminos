using System;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace kvNetClass
{
	// Token: 0x02000002 RID: 2
	public class clsCommon
	{
		// Token: 0x06000002 RID: 2
		[DllImport("winmm.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int PlaySound([MarshalAs(UnmanagedType.VBByRefStr)] ref string name, int hmod, int flags);

		// Token: 0x06000003 RID: 3 RVA: 0x00002058 File Offset: 0x00000258
		public void PlaySound(string filename)
		{
			clsCommon.PlaySound(ref filename, 0, 131073);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x0000206C File Offset: 0x0000026C
		public void subKillProcessGarbage(string sApName)
		{
			string text = Strings.Format(DateAndTime.Now, "yyyyMMddHHmmss");
			ObjectQuery objectQuery = new ObjectQuery("SELECT * FROM Win32_Process");
			ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(objectQuery);
			try
			{
				try
				{
					foreach (ManagementBaseObject managementBaseObject in managementObjectSearcher.Get())
					{
						ManagementObject managementObject = (ManagementObject)managementBaseObject;
						bool flag = Operators.ConditionalCompareObjectEqual(NewLateBinding.LateGet(managementObject["Name"], null, "ToUpper", new object[0], null, null, null), sApName.ToUpper(), false);
						if (flag)
						{
							int num = Conversions.ToInteger(managementObject["ProcessId"].ToString());
							Process.GetProcessById(num).Kill();
						}
					}
				}
				finally
				{
					ManagementObjectCollection.ManagementObjectEnumerator enumerator;
					if (enumerator != null)
					{
						((IDisposable)enumerator).Dispose();
					}
				}
			}
			catch (SystemException ex)
			{
				throw new Exception(ex.Message);
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002168 File Offset: 0x00000368
		public void subKillProcessGarbage(string sApName, string sMainWindowTitle)
		{
			string text = Strings.Format(DateAndTime.Now, "yyyyMMddHHmmss");
			ObjectQuery objectQuery = new ObjectQuery("SELECT * FROM Win32_Process");
			ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(objectQuery);
			try
			{
				try
				{
					foreach (ManagementBaseObject managementBaseObject in managementObjectSearcher.Get())
					{
						ManagementObject managementObject = (ManagementObject)managementBaseObject;
						bool flag = Operators.ConditionalCompareObjectEqual(NewLateBinding.LateGet(managementObject["Name"], null, "ToUpper", new object[0], null, null, null), sApName.ToUpper(), false);
						if (flag)
						{
							bool flag2 = Conversions.ToBoolean(LikeOperator.LikeObject(NewLateBinding.LateGet(managementObject["CommandLine"], null, "ToUpper", new object[0], null, null, null), ("*" + sMainWindowTitle + "*").ToUpper(), CompareMethod.Binary));
							if (flag2)
							{
								int num = Conversions.ToInteger(managementObject["ProcessId"].ToString());
								Process.GetProcessById(num).Kill();
							}
						}
					}
				}
				finally
				{
					ManagementObjectCollection.ManagementObjectEnumerator enumerator;
					if (enumerator != null)
					{
						((IDisposable)enumerator).Dispose();
					}
				}
			}
			catch (SystemException ex)
			{
				throw new Exception(ex.Message);
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000022B4 File Offset: 0x000004B4
		public void subErrorLog(string sPath, string sMsg)
		{
			sMsg = Strings.Format(DateAndTime.Now, "MM/dd/yyyy HH:mm:ss") + "\t" + sMsg;
			StreamWriter streamWriter = File.AppendText(sPath + "\\kv" + Strings.Format(DateAndTime.Now, "yyyyMMdd") + ".log");
			streamWriter.WriteLine(sMsg);
			streamWriter.Flush();
			streamWriter.Close();
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002324 File Offset: 0x00000524
		public int funSingleProcessChk(string myProcessName)
		{
			string text = Strings.Format(DateAndTime.Now, "yyyyMMddHHmmss");
			ObjectQuery objectQuery = new ObjectQuery("SELECT * FROM Win32_Process");
			ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(objectQuery);
			checked
			{
				int num3;
				try
				{
					int num;
					uint num2;
					try
					{
						foreach (ManagementBaseObject managementBaseObject in managementObjectSearcher.Get())
						{
							ManagementObject managementObject = (ManagementObject)managementBaseObject;
							bool flag = Conversions.ToBoolean(LikeOperator.LikeObject(NewLateBinding.LateGet(managementObject["Name"], null, "ToUpper", new object[0], null, null, null), myProcessName.ToUpper() + "*", CompareMethod.Binary));
							if (flag)
							{
								num++;
								bool flag2 = Operators.CompareString(text, Strings.Mid(Conversions.ToString(managementObject["CreationDate"]), 1, 14), false) > 0;
								if (flag2)
								{
									num2 = Conversions.ToUInteger(managementObject["ProcessId"]);
									text = Strings.Mid(Conversions.ToString(managementObject["CreationDate"]), 1, 14);
								}
							}
						}
					}
					finally
					{
						ManagementObjectCollection.ManagementObjectEnumerator enumerator;
						if (enumerator != null)
						{
							((IDisposable)enumerator).Dispose();
						}
					}
					bool flag3 = num > 1;
					if (flag3)
					{
						num3 = Conversions.ToInteger(num2.ToString());
					}
					else
					{
						num3 = 0;
					}
				}
				catch (SystemException ex)
				{
					throw new Exception(ex.Message);
				}
				return num3;
			}
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002490 File Offset: 0x00000690
		public string funGetValueInString(string sSource, string sTarget1, string sTarget2)
		{
			checked
			{
				string text;
				try
				{
					bool flag = (Information.IsNothing(sTarget1) || sTarget1.Length < 1) && (Information.IsNothing(sTarget2) || sTarget2.Length < 1);
					if (flag)
					{
						text = sSource;
					}
					else
					{
						bool flag2 = Information.IsNothing(sTarget1) || (sTarget1.Length < 1 && !Information.IsNothing(sTarget2) && sTarget2.Length > 0);
						if (flag2)
						{
							int num = Strings.InStr(1, sSource, sTarget2, CompareMethod.Text);
							bool flag3 = num < 1;
							if (flag3)
							{
								text = "";
							}
							else
							{
								text = Strings.Mid(sSource, 1, num - 1);
							}
						}
						else
						{
							bool flag4 = Information.IsNothing(sTarget2) || (sTarget2.Length < 1 && !Information.IsNothing(sTarget1) && sTarget1.Length > 0);
							if (flag4)
							{
								int num2 = Strings.Len(sTarget1);
								int num3 = Strings.InStr(1, sSource, sTarget1, CompareMethod.Text);
								bool flag5 = num3 < 1;
								if (flag5)
								{
									text = "";
								}
								else
								{
									text = Strings.Mid(sSource, num3 + num2);
								}
							}
							else
							{
								bool flag6 = !Information.IsNothing(sTarget1) && sTarget1.Length > 0 && !Information.IsNothing(sTarget2) && sTarget2.Length > 0;
								if (flag6)
								{
									int num2 = Strings.Len(sTarget1);
									int num3 = Strings.InStr(1, sSource, sTarget1, CompareMethod.Text);
									bool flag7 = num3 < 1;
									if (flag7)
									{
										text = "";
									}
									else
									{
										int num = Strings.InStr(num3 + num2, sSource, sTarget2, CompareMethod.Text);
										bool flag8 = num < 1;
										if (flag8)
										{
											text = "";
										}
										else
										{
											text = Strings.Mid(sSource, num3 + num2, num - num3 - num2);
										}
									}
								}
								else
								{
									text = "";
								}
							}
						}
					}
				}
				catch (Exception ex)
				{
					text = "";
				}
				return text;
			}
		}

		// Token: 0x0200000B RID: 11
		protected enum SoundFlags
		{
			// Token: 0x0400000C RID: 12
			SND_SYNC,
			// Token: 0x0400000D RID: 13
			SND_ASYNC,
			// Token: 0x0400000E RID: 14
			SND_NODEFAULT,
			// Token: 0x0400000F RID: 15
			SND_MEMORY = 4,
			// Token: 0x04000010 RID: 16
			SND_LOOP = 8,
			// Token: 0x04000011 RID: 17
			SND_NOSTOP = 16,
			// Token: 0x04000012 RID: 18
			SND_NOWAIT = 8192,
			// Token: 0x04000013 RID: 19
			SND_ALIAS = 65536,
			// Token: 0x04000014 RID: 20
			SND_ALIAS_ID = 1114112,
			// Token: 0x04000015 RID: 21
			SND_FILENAME = 131072,
			// Token: 0x04000016 RID: 22
			SND_RESOURCE = 262148
		}
	}
}
