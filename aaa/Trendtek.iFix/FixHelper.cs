using System;
using System.Data;
using System.Management;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using GeFanuc.iFixToolkit.Adapter;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.Win32;

namespace Trendtek.iFix
{
	// Token: 0x02000008 RID: 8
	public class FixHelper
	{
		// Token: 0x06000014 RID: 20 RVA: 0x00002520 File Offset: 0x00000920
		public FixHelper()
		{
			this.m_nerrno = 0;
		}

		// Token: 0x06000015 RID: 21
		[DllImport("AADWrapFileAPI.dll", CharSet = CharSet.Ansi, EntryPoint = "AlarmAreasGetAvailNames@20", ExactSpelling = true, SetLastError = true)]
		private static extern short AlarmAreasGetAvailNames(int lStartIndex, int lBufSize, ref int plBufSizeNeeded, ref int plNumReturned, [MarshalAs(UnmanagedType.VBByRefStr)] ref string aReturnList);

		// Token: 0x06000016 RID: 22
		[DllImport("vdba.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		private static extern int eda_get_my_physical_name([MarshalAs(UnmanagedType.VBByRefStr)] ref string Value);

		// Token: 0x06000017 RID: 23 RVA: 0x00002530 File Offset: 0x00000930
		public short FixGetPath(string sPathId, ref string retPath)
		{
			StringBuilder stringBuilder = new StringBuilder(256);
			StringBuilder stringBuilder2 = new StringBuilder(64);
			short num = checked((short)Helper.FixGetPath(sPathId, stringBuilder2, 64));
			if (num == 11000)
			{
				retPath = stringBuilder2.ToString();
			}
			else
			{
				retPath = "";
			}
			return num;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002578 File Offset: 0x00000978
		public string GetFixLogicalNodeName()
		{
			StringBuilder stringBuilder = new StringBuilder(9);
			this.m_nerrno = Helper.FixGetMyName(stringBuilder, checked((short)stringBuilder.Capacity));
			if (this.m_nerrno == 11000)
			{
				return string.Format("{0}", stringBuilder);
			}
			return string.Format("Error={0}=", this.m_nerrno);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000025D0 File Offset: 0x000009D0
		public string GetFixLocalNodeName()
		{
			string text2;
			try
			{
				string text = "         ";
				int num = FixHelper.eda_get_my_physical_name(ref text);
				long num2 = (long)(checked(Strings.InStr(1, text, "\0", CompareMethod.Binary) - 1));
				if (num2 > 0L)
				{
					text = Strings.Left(text, checked((int)num2));
				}
				text = text.Replace(" ", "");
				if (text.Length < 1)
				{
					throw new Exception("Local Node Name is not legal");
				}
				text2 = text;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
			return text2;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x0000265C File Offset: 0x00000A5C
		public int GetMajorVersion(FixHelper.FixVersion lVersion)
		{
			int num = 0;
			int num2 = 0;
			this.m_nerrno = Helper.FixGetRunningVersion(out num, out num2);
			if (this.m_nerrno != 11000)
			{
				return -1;
			}
			switch (lVersion)
			{
			case FixHelper.FixVersion.Major:
				return num;
			case FixHelper.FixVersion.Minor:
				return num2;
			default:
			{
				int num3;
				return num3;
			}
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000026A0 File Offset: 0x00000AA0
		public string GetiFixInstallVersion()
		{
			RegistryKey classesRoot = Registry.ClassesRoot;
			RegistryKey registryKey = classesRoot.OpenSubKey("Fix32\\\\Install");
			object objectValue = RuntimeHelpers.GetObjectValue(registryKey.GetValue("iFIX Version"));
			string text;
			if (objectValue != null)
			{
				text = objectValue.ToString();
			}
			else
			{
				text = "";
			}
			return text;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000026E8 File Offset: 0x00000AE8
		public string RemoveNull(string sString)
		{
			int num = checked(Strings.InStr(1, sString, "\0", CompareMethod.Binary) - 1);
			if (num > 0)
			{
				return Strings.Left(sString, num);
			}
			return Strings.Trim(sString);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002718 File Offset: 0x00000B18
		public string SplitTagName(string sFullName, ref string sNode, ref string sTag, ref string sField)
		{
			sFullName = Strings.UCase(sFullName);
			string[] array = Strings.Split(sFullName, ".", -1, CompareMethod.Binary);
			if (array.Length == 4)
			{
				sNode = array[1];
				sTag = array[2];
				sField = array[3];
				return "OK";
			}
			int num = Strings.InStr(sFullName, "FIX32.", CompareMethod.Text);
			checked
			{
				if (num == 1)
				{
					num = Strings.InStr(sFullName, ".", CompareMethod.Text);
					sFullName = Strings.Mid(sFullName, num + 1);
				}
				array = Strings.Split(sFullName, ".", -1, CompareMethod.Binary);
				if (array.Length == 3)
				{
					sNode = array[0];
					sTag = array[1];
					sField = array[2];
					return "OK";
				}
				if (Strings.InStr(sFullName, ".A_", CompareMethod.Binary) != 0)
				{
					num = Strings.InStr(sFullName, ".A_", CompareMethod.Text);
					sField = Strings.Mid(sFullName, num + 1);
					sFullName = Strings.Mid(sFullName, 1, num - 1);
				}
				if (Strings.InStr(sFullName, ".F_", CompareMethod.Binary) != 0)
				{
					num = Strings.InStr(sFullName, ".F_", CompareMethod.Text);
					sField = Strings.Mid(sFullName, num + 1);
					sFullName = Strings.Mid(sFullName, 1, num - 1);
				}
				if (Strings.InStr(sFullName, ".T_", CompareMethod.Binary) != 0)
				{
					num = Strings.InStr(sFullName, ".T_", CompareMethod.Text);
					sField = Strings.Mid(sFullName, num + 1);
					sFullName = Strings.Mid(sFullName, 1, num - 1);
				}
				if (Strings.Len(sField) < 1)
				{
					sField = "A_CV";
				}
				array = Strings.Split(sFullName, ".", -1, CompareMethod.Binary);
				if (array.Length > 2)
				{
					return "TagName 語法輸入錯誤";
				}
				if (Strings.InStr(sFullName, ".", CompareMethod.Text) != 0)
				{
					num = Strings.InStr(sFullName, ".", CompareMethod.Text);
					sTag = Strings.Mid(sFullName, num + 1);
					sNode = Strings.Mid(sFullName, 1, num - 1);
				}
				else
				{
					sTag = sFullName;
					StringBuilder stringBuilder = new StringBuilder(256);
					short num2 = (short)Helper.FixGetMyName(stringBuilder, 256);
					if (num2 != 11000)
					{
						StringBuilder stringBuilder2 = new StringBuilder(256);
						Helper.NlsGetText((int)num2, stringBuilder2, 256);
						return stringBuilder2.ToString();
					}
					sNode = stringBuilder.ToString();
				}
				if ((Strings.Len(sTag) < 1) | (Strings.Len(sNode) < 1))
				{
					return "TagName 語法輸入錯誤";
				}
				return "OK";
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002928 File Offset: 0x00000D28
		public string GetParterner(ref string sLogical, ref string sPrimary, ref string sBackup)
		{
			short num = 0;
			StringBuilder stringBuilder = new StringBuilder(80);
			checked
			{
				string text3;
				try
				{
					short num2 = 99;
					short num3 = 0;
					string[] array;
					short num4 = Eda.EnumAllScadaNodes(out array, ref num3, num2, out num);
					if ((num4 == 0) | (num4 == 100))
					{
						foreach (string text in array)
						{
							string text2 = this.RemoveNull(text);
							int num5;
							if (Operators.CompareString(sLogical.ToUpper(), text2.ToUpper(), false) == 0)
							{
								num5 = (int)Math.Round(unchecked(Conversion.Fix((double)num5 / 3.0) * 3.0));
								sLogical = this.RemoveNull(array[num5].ToString());
								sPrimary = this.RemoveNull(array[num5 + 1].ToString());
								sBackup = this.RemoveNull(array[num5 + 2].ToString());
								return "OK";
							}
							num5++;
						}
						text3 = "No this Remote connection in SCU";
					}
					else
					{
						Helper.NlsGetText((int)num4, stringBuilder, (short)stringBuilder.Capacity);
						text3 = stringBuilder.ToString();
					}
				}
				catch (Exception ex)
				{
					text3 = ex.Message;
				}
				return text3;
			}
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002A60 File Offset: 0x00000E60
		public string ErrNumToString(int ErrNo)
		{
			StringBuilder stringBuilder = new StringBuilder(256);
			Helper.NlsGetText(ErrNo, stringBuilder, 256);
			return stringBuilder.ToString();
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002A8C File Offset: 0x00000E8C
		public bool IsFixRunning()
		{
			int num = Helper.FixIsFixRunning();
			return num == 1;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002AA8 File Offset: 0x00000EA8
		public void FixSendMsg(string sMessage, string sArea)
		{
			string[] array = new string[] { sArea };
			short num = Eda.AlmSendText(sMessage, (long)((ulong)(-17)), 15, array);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002AD0 File Offset: 0x00000ED0
		public void GetRemoteNodeNameList(ComboBox myList)
		{
			string text = "";
			StringBuilder stringBuilder = new StringBuilder(9);
			checked
			{
				short num = (short)Helper.FixGetMyName(stringBuilder, (short)stringBuilder.Capacity);
				if (num == 11000)
				{
					text = stringBuilder.ToString();
				}
				int num2 = Helper.FixIsFixRunning();
				if (num2 == 1)
				{
					short num3 = 0;
					short num4 = 99;
					int num6;
					short num5 = (short)num6;
					string[] array;
					short num7 = Eda.EnumScadaNodes(out array, ref num3, num4, out num5);
					num6 = (int)num5;
					num = num7;
					string[] array2 = new string[num6 - 1 + 1];
					if ((num == 0) | (num == 100))
					{
						int num8 = 0;
						int num9 = num6 - 1;
						for (int i = num8; i <= num9; i++)
						{
							if (Operators.CompareString(this.RemoveNull(array[i]).ToUpper(), "THISNODE", false) != 0)
							{
								array2[i] = this.RemoveNull(array[i]);
							}
						}
						Array.Sort<string>(array2);
						int num10 = 0;
						int num11 = num6 - 1;
						for (int i = num10; i <= num11; i++)
						{
							if (!Information.IsNothing(array2[i]))
							{
								myList.Items.Add(array2[i]);
							}
						}
						if (myList.Items.Count > 0)
						{
							myList.SelectedIndex = 0;
						}
					}
				}
				int num12 = myList.FindString(text.ToUpper());
				if (myList.Items.Count > 0 && num12 < 0)
				{
					num12 = 0;
				}
				myList.SelectedIndex = num12;
			}
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002C0C File Offset: 0x0000100C
		public void GetTagsList(string sNode, short nType, short nStartipn, short nMax, ref DataTable dt)
		{
			try
			{
				short num = 0;
				StringBuilder stringBuilder = new StringBuilder(80);
				short num2;
				for (;;)
				{
					string[] array;
					short[] array2;
					num2 = Eda.EnumTags(sNode, out array, out array2, nType, ref nStartipn, nMax, out num);
					if (!((num2 == 0) | (num2 == 1210)))
					{
						break;
					}
					foreach (string text in array)
					{
						text = this.RemoveNull(text).Trim();
						if (text.Length >= 1)
						{
							DataRow dataRow = dt.NewRow();
							dataRow[0] = text;
							if (dt.Columns.Count > 1)
							{
								dataRow[1] = nType;
							}
							dt.Rows.Add(dataRow);
						}
					}
					if (nMax != num)
					{
						goto Block_6;
					}
				}
				Helper.NlsGetText((int)num2, stringBuilder, checked((short)stringBuilder.Capacity));
				throw new Exception(stringBuilder.ToString());
				Block_6:;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002D0C File Offset: 0x0000110C
		public void GetAllTagsList(string sNode, short nType, short nStartipn, short numRequest, ref DataTable dt)
		{
			try
			{
				short num = 0;
				StringBuilder stringBuilder = new StringBuilder(80);
				short num2;
				for (;;)
				{
					string[] array;
					ENUMBUF enumbuf;
					num2 = Eda.EnumAllTags(sNode, nType, "", out array, numRequest, out num, out enumbuf);
					if (!((num2 == 0) | (num2 == 1210)))
					{
						break;
					}
					foreach (string text in array)
					{
						text = this.RemoveNull(text).Trim();
						if (text.Length >= 1)
						{
							DataRow dataRow = dt.NewRow();
							dataRow[0] = text;
							if (dt.Columns.Count > 1)
							{
								dataRow[1] = nType;
							}
							dt.Rows.Add(dataRow);
						}
					}
					if (numRequest != num)
					{
						goto Block_6;
					}
				}
				Helper.NlsGetText((int)num2, stringBuilder, checked((short)stringBuilder.Capacity));
				throw new Exception(stringBuilder.ToString());
				Block_6:;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002E0C File Offset: 0x0000120C
		public void GetAlarmAreaListFromAPI(ref ComboBox lstAreaAlias, ref ListBox lstAreaName)
		{
			checked
			{
				try
				{
					string text = "";
					byte[] array = new byte[30];
					long num = 0L;
					text = Strings.Space(150000);
					int num2 = (int)num;
					int num3 = (int)num;
					long num5;
					int num4 = (int)num5;
					long num7;
					int num6 = (int)num7;
					FixHelper.AlarmAreasGetAvailNames(num2, num3, ref num4, ref num6, ref text);
					byte[] bytes;
					int num8;
					unchecked
					{
						num7 = (long)num6;
						num5 = (long)num4;
						bytes = Encoding.Default.GetBytes(text);
						num8 = 0;
					}
					int num9 = (int)(num7 - 1L);
					for (int i = num8; i <= num9; i++)
					{
						Array.Copy(bytes, i * 30, array, 0, 30);
						string text2 = Encoding.Default.GetString(array);
						text2 = this.RemoveNull(text2);
						if (lstAreaName.Items.IndexOf(text2) < 0)
						{
							lstAreaAlias.Items.Add(text2);
							lstAreaName.Items.Add(text2);
						}
					}
				}
				catch (Exception ex)
				{
					throw new Exception(ex.Message);
				}
			}
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002F04 File Offset: 0x00001304
		public bool GetWorkspaceObject(ref object WorkSPace, ref bool RunMode)
		{
			bool flag;
			try
			{
				ObjectQuery objectQuery = new ObjectQuery("SELECT * FROM Win32_Process");
				ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(objectQuery);
				try
				{
					foreach (ManagementBaseObject managementBaseObject in managementObjectSearcher.Get())
					{
						ManagementObject managementObject = (ManagementObject)managementBaseObject;
						if (Conversions.ToBoolean(LikeOperator.LikeObject(NewLateBinding.LateGet(managementObject["Name"], null, "ToUpper", new object[0], null, null, null), "WorkSpace".ToUpper() + "*", CompareMethod.Binary)))
						{
							WorkSPace = RuntimeHelpers.GetObjectValue(Interaction.GetObject("", "Workspace.Application"));
							break;
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
				RunMode = false;
				if (Information.IsNothing(RuntimeHelpers.GetObjectValue(WorkSPace)))
				{
					flag = false;
				}
				else
				{
					if (Operators.ConditionalCompareObjectEqual(NewLateBinding.LateGet(WorkSPace, null, "Mode", new object[0], null, null, null), 4, false))
					{
						RunMode = true;
					}
					flag = true;
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
			return flag;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00003020 File Offset: 0x00001420
		public bool GetFixBackGroundServer(ref object FixBackGroundServer)
		{
			bool flag;
			try
			{
				ObjectQuery objectQuery = new ObjectQuery("SELECT * FROM Win32_Process");
				ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(objectQuery);
				try
				{
					foreach (ManagementBaseObject managementBaseObject in managementObjectSearcher.Get())
					{
						ManagementObject managementObject = (ManagementObject)managementBaseObject;
						if (Conversions.ToBoolean(LikeOperator.LikeObject(NewLateBinding.LateGet(managementObject["Name"], null, "ToUpper", new object[0], null, null, null), "FixBackGroundServer".ToUpper() + "*", CompareMethod.Binary)))
						{
							FixBackGroundServer = RuntimeHelpers.GetObjectValue(Interaction.GetObject(null, "FixBackGroundServer.Application"));
							break;
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
				if (Information.IsNothing(RuntimeHelpers.GetObjectValue(FixBackGroundServer)))
				{
					flag = false;
				}
				else
				{
					flag = true;
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
			return flag;
		}

		// Token: 0x04000006 RID: 6
		private int m_nerrno;

		// Token: 0x02000009 RID: 9
		public enum FixVersion
		{
			// Token: 0x04000008 RID: 8
			Major,
			// Token: 0x04000009 RID: 9
			Minor
		}
	}
}
