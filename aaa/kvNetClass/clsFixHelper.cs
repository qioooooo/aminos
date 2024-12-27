using System;
using System.Text;
using GeFanuc.iFixToolkit.Adapter;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace kvNetClass
{
	// Token: 0x02000006 RID: 6
	public class clsFixHelper
	{
		// Token: 0x06000012 RID: 18 RVA: 0x00002C90 File Offset: 0x00000E90
		public clsFixHelper()
		{
			this.m_nerrno = 0;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002CA0 File Offset: 0x00000EA0
		public void subFixSendMsg(string sMessage, string sArea)
		{
			string[] array = new string[] { sArea };
			short num = Eda.AlmSendText(sMessage, (long)((ulong)(-17)), 15, array);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002CC8 File Offset: 0x00000EC8
		public string funGetFixNodeName()
		{
			StringBuilder stringBuilder = new StringBuilder(9);
			this.m_nerrno = Helper.FixGetMyName(stringBuilder, checked((short)stringBuilder.Capacity));
			bool flag = this.m_nerrno == 11000;
			string text;
			if (flag)
			{
				text = string.Format("{0}", stringBuilder);
			}
			else
			{
				text = string.Format("Error={0}=", this.m_nerrno);
			}
			return text;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002D2C File Offset: 0x00000F2C
		public int funGetMajorVersion(clsFixHelper.FixVersion lVersion)
		{
			int num = 0;
			int num2 = 0;
			this.m_nerrno = Helper.FixGetRunningVersion(out num, out num2);
			bool flag = this.m_nerrno == 11000;
			int num3;
			if (flag)
			{
				if (lVersion != clsFixHelper.FixVersion.Major)
				{
					if (lVersion == clsFixHelper.FixVersion.Minor)
					{
						num3 = num2;
					}
				}
				else
				{
					num3 = num;
				}
			}
			else
			{
				num3 = -1;
			}
			return num3;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002D84 File Offset: 0x00000F84
		public string funRemoveNull(string sString)
		{
			int num = checked(Strings.InStr(1, sString, "\0", CompareMethod.Binary) - 1);
			bool flag = num > 0;
			string text;
			if (flag)
			{
				text = Strings.Left(sString, num);
			}
			else
			{
				text = Strings.Trim(sString);
			}
			return text;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002DC0 File Offset: 0x00000FC0
		public string funSplitTagName(string sFullName, ref string sNode, ref string sTag, ref string sField)
		{
			sFullName = Strings.UCase(sFullName);
			string[] array = Strings.Split(sFullName, ".", -1, CompareMethod.Binary);
			bool flag = array.Length == 4;
			checked
			{
				string text;
				if (flag)
				{
					sNode = array[1];
					sTag = array[2];
					sField = array[3];
					text = "OK";
				}
				else
				{
					int num = Strings.InStr(sFullName, "FIX32.", CompareMethod.Text);
					bool flag2 = num == 1;
					if (flag2)
					{
						num = Strings.InStr(sFullName, ".", CompareMethod.Text);
						sFullName = Strings.Mid(sFullName, num + 1);
					}
					array = Strings.Split(sFullName, ".", -1, CompareMethod.Binary);
					bool flag3 = array.Length == 3;
					if (flag3)
					{
						sNode = array[0];
						sTag = array[1];
						sField = array[2];
						text = "OK";
					}
					else
					{
						bool flag4 = Strings.InStr(sFullName, ".A_", CompareMethod.Binary) != 0;
						if (flag4)
						{
							num = Strings.InStr(sFullName, ".A_", CompareMethod.Text);
							sField = Strings.Mid(sFullName, num + 1);
							sFullName = Strings.Mid(sFullName, 1, num - 1);
						}
						bool flag5 = Strings.InStr(sFullName, ".F_", CompareMethod.Binary) != 0;
						if (flag5)
						{
							num = Strings.InStr(sFullName, ".F_", CompareMethod.Text);
							sField = Strings.Mid(sFullName, num + 1);
							sFullName = Strings.Mid(sFullName, 1, num - 1);
						}
						bool flag6 = Strings.InStr(sFullName, ".T_", CompareMethod.Binary) != 0;
						if (flag6)
						{
							num = Strings.InStr(sFullName, ".T_", CompareMethod.Text);
							sField = Strings.Mid(sFullName, num + 1);
							sFullName = Strings.Mid(sFullName, 1, num - 1);
						}
						bool flag7 = Strings.Len(sField) < 1;
						if (flag7)
						{
							sField = "A_CV";
						}
						array = Strings.Split(sFullName, ".", -1, CompareMethod.Binary);
						bool flag8 = array.Length > 2;
						if (flag8)
						{
							text = "TagName 語法輸入錯誤";
						}
						else
						{
							bool flag9 = Strings.InStr(sFullName, ".", CompareMethod.Text) != 0;
							if (flag9)
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
								bool flag10 = num2 == 11000;
								if (!flag10)
								{
									StringBuilder stringBuilder2 = new StringBuilder(256);
									Helper.NlsGetText((int)num2, stringBuilder2, 256);
									return stringBuilder2.ToString();
								}
								sNode = stringBuilder.ToString();
							}
							bool flag11 = (Strings.Len(sTag) < 1) | (Strings.Len(sNode) < 1);
							if (flag11)
							{
								text = "TagName 語法輸入錯誤";
							}
							else
							{
								text = "OK";
							}
						}
					}
				}
				return text;
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00003030 File Offset: 0x00001230
		public string funGetParterner(ref string sLogical, ref string sPrimary, ref string sBackup)
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
					bool flag = (num4 == 0) | (num4 == 100);
					if (flag)
					{
						foreach (string text in array)
						{
							string text2 = this.funRemoveNull(text);
							bool flag2 = Operators.CompareString(sLogical.ToUpper(), text2.ToUpper(), false) == 0;
							int num5;
							if (flag2)
							{
								num5 = (int)Math.Round(unchecked(Conversion.Fix((double)num5 / 3.0) * 3.0));
								sLogical = this.funRemoveNull(array[num5].ToString());
								sPrimary = this.funRemoveNull(array[num5 + 1].ToString());
								sBackup = this.funRemoveNull(array[num5 + 2].ToString());
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

		// Token: 0x06000019 RID: 25 RVA: 0x00003190 File Offset: 0x00001390
		public string funErrNumToString(int ErrNo)
		{
			StringBuilder stringBuilder = new StringBuilder(256);
			Helper.NlsGetText(ErrNo, stringBuilder, 256);
			return stringBuilder.ToString();
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000031C0 File Offset: 0x000013C0
		public bool funIsFixRunning()
		{
			int num = Helper.FixIsFixRunning();
			return num == 1;
		}

		// Token: 0x04000002 RID: 2
		private int m_nerrno;

		// Token: 0x0200000D RID: 13
		public enum FixVersion
		{
			// Token: 0x0400001D RID: 29
			Major,
			// Token: 0x0400001E RID: 30
			Minor
		}
	}
}
