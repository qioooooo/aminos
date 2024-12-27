using System;
using System.Data;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using GeFanuc.iFixToolkit.Adapter;
using kvNetClass;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace triFixAlmSndCyl
{
	// Token: 0x02000016 RID: 22
	[StandardModule]
	internal sealed class modSub
	{
		// Token: 0x06000232 RID: 562 RVA: 0x0000594C File Offset: 0x00003D4C
		public static void subChkLicense()
		{
			string text = "";
			try
			{
				modSub.kvFixLicChk = new clsFixLicChk();
				text = modSub.kvFixLicChk.funFixLicenseCheck(Application.StartupPath + "\\TrendTek.lic", Application.ProductName, "73627273", "TrendTek");
				if (Operators.CompareString(text, "DEMO", false) == 0)
				{
					modpublic.g_Demo = true;
				}
				else if (Operators.CompareString(text, "OK", false) != 0)
				{
					modSub.kvFixLicChk = new clsFixLicChk();
					text = modSub.kvFixLicChk.funFixLicenseCheck(Application.StartupPath + "\\TrendTek.lic", "FREE", "73627273", "TrendTek");
					if (Operators.CompareString(text, "OK", false) != 0)
					{
						modSub.kvComm = new clsCommon();
						modSub.kvComm.PlaySound("tada.wav");
						modSub.kvComm = null;
						MessageBox.Show(text + modpublic.sTrendtek, "License", MessageBoxButtons.OK, MessageBoxIcon.Hand);
						modSub.subError(text, true, true, "", "", false);
					}
				}
			}
			catch (Exception ex)
			{
				modSub.subError(text + ex.Message, true, true, "", "", false);
			}
		}

		// Token: 0x06000233 RID: 563 RVA: 0x00005A8C File Offset: 0x00003E8C
		public static void subSingleInstance()
		{
			modSub.kvComm = new clsCommon();
			int num = modSub.kvComm.funSingleProcessChk(Application.ProductName);
			if (num > 0)
			{
				try
				{
					Interaction.AppActivate(num);
				}
				catch (Exception ex)
				{
				}
				finally
				{
					Environment.Exit(0);
				}
			}
			modSub.kvComm = null;
		}

		// Token: 0x06000234 RID: 564 RVA: 0x00005AF8 File Offset: 0x00003EF8
		public static void subParameterSearch()
		{
			try
			{
				string text = Environment.CommandLine;
				text = Strings.UCase(text);
				string text2 = modSub.funGetsubParameterX(text, "/M:", "", false, false);
				if (Strings.Len(text2) > 0)
				{
					modpublic.g_sCfgName = text2.Replace(" ", "");
				}
				text2 = modSub.funGetsubParameterX(text, "/D:", "", false, false);
				if (Strings.Len(text2) > 0)
				{
					modpublic.g_iDelay = Conversions.ToInteger(text2.Replace(" ", ""));
				}
				if (Strings.InStr(text, "/ONCE", CompareMethod.Text) > 0)
				{
					modpublic.g_bPlayOnce = true;
				}
				else
				{
					text2 = modSub.funGetsubParameterX(text, "/PLAYCNT:", "", false, false);
					text2 = text2.Replace(" ", "");
					if (Strings.Len(text2) > 0 && Versioned.IsNumeric(text2))
					{
						modpublic.g_nPlayCount = Conversions.ToInteger(text2);
						if (modpublic.g_nPlayCount >= 100)
						{
							modpublic.g_nPlayCount = 100;
						}
					}
					else if (Strings.InStr(text, "/PLAYFINISHEDNOW", CompareMethod.Text) > 0)
					{
						modpublic.g_bUsingPlayFinishedNowFunction = true;
					}
				}
				if (Strings.InStr(text, "/MULTI", CompareMethod.Text) > 0)
				{
					modpublic.g_bMultiInstance = true;
				}
			}
			catch (Exception ex)
			{
				string text3 = "讀取[命令列引數]> : ";
				MessageBox.Show(text3 + ex.Message, "iFix警報輪迴播音系統", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				modSub.subError(text3 + ex.Message, true, false, "", "", false);
			}
		}

		// Token: 0x06000235 RID: 565 RVA: 0x00005C78 File Offset: 0x00004078
		public static void subStringToDs(StreamReader sr, DataTable dt)
		{
			checked
			{
				try
				{
					dt.Clear();
					while (sr.Peek() >= 0)
					{
						string[] array = Strings.Split(sr.ReadLine(), ",", -1, CompareMethod.Binary);
						int num3;
						try
						{
							DataRow dataRow = dt.NewRow();
							int num = 0;
							int num2 = Math.Min(array.Length - 1, 9);
							for (int i = num; i <= num2; i++)
							{
								if (Operators.CompareString(dt.Columns[i].ColumnName.ToUpper(), "priority".ToUpper(), false) == 0)
								{
									if (Conversion.Val(array[i]) < 0.0)
									{
										array[i] = Conversions.ToString(0);
									}
									if (Conversion.Val(array[i]) >= 2.0)
									{
										array[i] = Conversions.ToString(2);
									}
								}
								else if (Operators.CompareString(dt.Columns[i].ColumnName.ToUpper(), "length".ToUpper(), false) == 0)
								{
									if (Conversion.Val(array[i]) < 0.0)
									{
										array[i] = Conversions.ToString(0);
									}
									if (Conversion.Val(array[i]) >= 30.0)
									{
										array[i] = Conversions.ToString(30);
									}
								}
								dataRow[i] = array[i];
							}
							if (Information.IsDBNull(RuntimeHelpers.GetObjectValue(dataRow["field"])))
							{
								dataRow["field"] = "NOPRI";
							}
							else if (Operators.ConditionalCompareObjectLess(dataRow["blocktype"], 240, false))
							{
								if (Array.IndexOf(modpublic.g_aPriorityForAnalogTag, RuntimeHelpers.GetObjectValue(NewLateBinding.LateGet(dataRow["field"], null, "ToUpper", new object[0], null, null, null))) < 0)
								{
									dataRow["field"] = "NOPRI";
								}
							}
							else if (Array.IndexOf(modpublic.g_aPriorityForArea, RuntimeHelpers.GetObjectValue(NewLateBinding.LateGet(dataRow["field"], null, "ToUpper", new object[0], null, null, null))) < 0)
							{
								dataRow["field"] = "NOPRI";
							}
							dt.Rows.Add(dataRow);
						}
						catch (Exception ex)
						{
							string text = "第 (" + (num3 + 1).ToString() + ") 筆資料有問題, 無法匯入.\r原因>: " + ex.Message;
							MessageBox.Show(text, "資料匯入", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
						}
						num3++;
					}
				}
				catch (Exception ex2)
				{
					throw new Exception(ex2.Message);
				}
			}
		}

		// Token: 0x06000236 RID: 566 RVA: 0x00005F14 File Offset: 0x00004314
		public static void subReleaseResource(string sAppName = "", string sTitleName = "")
		{
			string text = Environment.OSVersion.ToString();
			string text2 = modSub.funGetValueInString(text, "Microsoft Windows NT ", ".");
			string text3 = modSub.funGetValueInString(text, "Microsoft Windows NT " + text2 + ".", ".");
			float num;
			if ((text2.Length > 0) & (text2.Length > 0))
			{
				num = Conversions.ToSingle(text2 + "." + text3);
			}
			else
			{
				num = 0f;
			}
			modpublic.g_Excel = null;
			if (Strings.Len(sAppName) > 0)
			{
				modSub.kvComm = new clsCommon();
				if (Strings.Len(sTitleName) > 0)
				{
					if ((double)num < 5.009)
					{
						modSub.kvComm.subKillProcessGarbage(sAppName);
					}
					else
					{
						modSub.kvComm.subKillProcessGarbage(sAppName, sTitleName);
					}
				}
				else
				{
					modSub.kvComm.subKillProcessGarbage(sAppName);
				}
			}
			Eda.DeleteGroup(modpublic.g_Gnum);
			modSub.kvComm = null;
		}

		// Token: 0x06000237 RID: 567 RVA: 0x00005FF0 File Offset: 0x000043F0
		public static void subError(string sMsg, bool bEnd = false, bool bLog = true, string sKillAppName = "", string sKillTitleName = "", bool bShowMsg = false)
		{
			string text = Strings.Format(DateAndTime.Today, "yyyyMMdd");
			try
			{
				if (bShowMsg)
				{
					MessageBox.Show(sMsg, "iFix警報輪迴播音系統", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
				if (bLog)
				{
					StreamWriter streamWriter = File.AppendText(string.Concat(new string[]
					{
						Application.StartupPath,
						"\\",
						Application.ProductName,
						text,
						".log"
					}));
					sMsg = Strings.Format(DateAndTime.Now, "MM/dd/yyyy HH:mm:ss") + "\tiFix警報輪迴播音系統 > " + sMsg;
					streamWriter.WriteLine(sMsg);
					streamWriter.Close();
				}
				sMsg = Strings.Format(DateAndTime.Now, "MM/dd/yyyy HH:mm:ss") + "\t" + sMsg;
				if (bEnd)
				{
					if (Strings.Len(sKillAppName) > 0)
					{
						if (Strings.Len(sKillTitleName) > 0)
						{
							modSub.subReleaseResource(sKillAppName, sKillTitleName);
						}
						else
						{
							modSub.subReleaseResource(sKillAppName, "");
						}
					}
					else
					{
						modSub.subReleaseResource("", "");
					}
					Environment.Exit(0);
				}
			}
			catch (Exception ex)
			{
			}
		}

		// Token: 0x06000238 RID: 568 RVA: 0x00006110 File Offset: 0x00004510
		public static string funGetsubParameterX(string sCommand, string sSearch, string sError, bool bEnd = true, bool bLog = false)
		{
			checked
			{
				string text2;
				try
				{
					int num = Strings.Len(sSearch);
					sSearch = sSearch.ToUpper();
					if (Strings.InStr(1, sCommand, sSearch, CompareMethod.Text) != 0)
					{
						int num2 = Strings.InStr(1, sCommand, sSearch, CompareMethod.Text);
						int num3 = Strings.InStr(num2 + 1, sCommand, " /", CompareMethod.Text);
						string text;
						if (num3 > 0)
						{
							text = Strings.Trim(Strings.Mid(sCommand, num2 + num, num3 - num2 - num));
						}
						else
						{
							text = Strings.Trim(Strings.Right(sCommand, Strings.Len(sCommand) - num2 - num + 1));
						}
						if (Strings.Len(text) > 0)
						{
							return text;
						}
					}
					if (Strings.Len(sError) == 0)
					{
						text2 = "";
					}
					else
					{
						if (bEnd)
						{
							if (bLog)
							{
								modSub.subError(sError, true, bLog, "", "", false);
							}
							else
							{
								MessageBox.Show(sError, "iFix警報輪迴播音系統", MessageBoxButtons.OK, MessageBoxIcon.Hand);
								modSub.subError(sError, true, bLog, "", "", false);
							}
						}
						else if (bLog)
						{
							modSub.subError(sError, false, bLog, "", "", false);
						}
						else
						{
							MessageBox.Show(sError, "iFix警報輪迴播音系統", MessageBoxButtons.OK, MessageBoxIcon.Hand);
						}
						text2 = "";
					}
				}
				catch (Exception ex)
				{
					throw new Exception(ex.Message);
				}
				return text2;
			}
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0000625C File Offset: 0x0000465C
		public static string funGetValueInString(string sSource, string sTarget1, string sTarget2)
		{
			checked
			{
				string text;
				try
				{
					int num = Strings.Len(sTarget1);
					int num2 = Strings.InStr(1, sSource, sTarget1, CompareMethod.Text);
					if (num2 < 1)
					{
						text = "";
					}
					else
					{
						int num3 = Strings.InStr(num2 + num, sSource, sTarget2, CompareMethod.Text);
						if (num3 < 1)
						{
							text = "";
						}
						else
						{
							text = Strings.Mid(sSource, num2 + num, num3 - num2 - num);
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

		// Token: 0x0600023A RID: 570 RVA: 0x000062D4 File Offset: 0x000046D4
		public static string funCorrectPahtString(string sPath)
		{
			string text = sPath;
			string text2;
			try
			{
				if (text.Length < 0)
				{
					text2 = "";
				}
				else
				{
					sPath = sPath.Trim();
					sPath = sPath.Replace(" \\ ", "\\");
					sPath = sPath.Replace(" \\", "\\");
					sPath = sPath.Replace("\\ ", "\\");
					sPath = sPath.Replace(" : ", ":");
					sPath = sPath.Replace(" :", ":");
					sPath = sPath.Replace(": ", ":");
					sPath = sPath.Replace(" . ", ".");
					sPath = sPath.Replace(" .", ".");
					sPath = sPath.Replace(". ", ".");
					text2 = sPath;
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
			return text2;
		}

		// Token: 0x0600023B RID: 571 RVA: 0x000063CC File Offset: 0x000047CC
		public static string funDSToString(DataTable dt)
		{
			string text = "";
			checked
			{
				string text2;
				try
				{
					int num = 0;
					int num2 = dt.Rows.Count - 1;
					for (int i = num; i <= num2; i++)
					{
						DataRow dataRow = dt.Rows[i];
						int num3 = 0;
						do
						{
							if (Information.IsDBNull(dataRow))
							{
								text += ",";
							}
							else
							{
								text = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(text, dataRow[num3]), ","));
							}
							num3++;
						}
						while (num3 <= 9);
						text = Strings.Mid(text, 1, Strings.Len(text) - 1) + "\r\n";
					}
					text2 = text;
				}
				catch (Exception ex)
				{
					throw new Exception(ex.Message);
				}
				return text2;
			}
		}

		// Token: 0x04000124 RID: 292
		private static clsCommon kvComm;

		// Token: 0x04000125 RID: 293
		private static clsFixLicChk kvFixLicChk;
	}
}
