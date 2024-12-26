using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using GeFanuc.iFixToolkit.Adapter;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.Win32;
using SafeNet.Sentinel;

namespace kvNetClass
{
	// Token: 0x02000007 RID: 7
	public class clsFixLicChk
	{
		// Token: 0x0600001B RID: 27 RVA: 0x000031E6 File Offset: 0x000013E6
		public clsFixLicChk()
		{
			this.ds = new DataSet();
			this.csCrypt = new clsCrypt();
			this.csFixHelper = new clsFixHelper();
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00003210 File Offset: 0x00001410
		private void HASPDllCheck(string filename)
		{
			string text = Application.StartupPath + "\\";
			bool flag = !File.Exists(text + filename);
			if (flag)
			{
				throw new Exception("missing " + filename + " file.");
			}
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00003258 File Offset: 0x00001458
		private string HASPgetErrorText(AdminStatus status)
		{
			string text;
			if (status != AdminStatus.StatusOk)
			{
				if (status != (AdminStatus)400)
				{
					switch (status)
					{
					case AdminStatus.InvalidContext:
						return "InvalidContext";
					case AdminStatus.LmNotFound:
						return "LmNotFound";
					case AdminStatus.LmTooOld:
						return "LmTooOld";
					case AdminStatus.BadParameters:
						return "BadParameters";
					case AdminStatus.LocalNetWorkError:
						return "LocalNetWorkError";
					case AdminStatus.CannotReadFile:
						return "CannotReadFile";
					case AdminStatus.ScopeError:
						return "ScopeError";
					case AdminStatus.PasswordRequired:
						return "PasswordRequired";
					case AdminStatus.CannotSetPassword:
						return "CannotSetPassword";
					case AdminStatus.UpdateError:
						return "UpdateError";
					case AdminStatus.BadValue:
						return "BadValue";
					case AdminStatus.ReadOnly:
						return "ReadOnly";
					case AdminStatus.ElementUndefined:
						return "ElementUndefined";
					case AdminStatus.InvalidPointer:
						return "InvalidPointer";
					case AdminStatus.NoIntegratedLm:
						return "NoIntegratedLm";
					case AdminStatus.ResultTooBig:
						return "ResultTooBig";
					case AdminStatus.InvalidVendorCode:
						return "InvalidVendorCode";
					case AdminStatus.UnknownVendorCode:
						return "UnknownVendorCode";
					}
					text = "UnknownErrorCode";
				}
				else
				{
					text = "Unable to locate dynamic library for API";
				}
			}
			else
			{
				text = "StatusOk";
			}
			return text;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000033E8 File Offset: 0x000015E8
		private void HASPcheckStatus(AdminStatus status)
		{
			string text = string.Empty;
			bool flag = status > AdminStatus.StatusOk;
			if (flag)
			{
				text = this.HASPgetErrorText(status);
				throw new Exception(text + " - " + Conversions.ToString((int)status));
			}
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00003424 File Offset: 0x00001624
		private void subCreateFixLicenseTable(DataSet ds)
		{
			try
			{
				ds.Tables.Add("License");
				DataColumn dataColumn = new DataColumn();
				DataColumn dataColumn2 = dataColumn;
				dataColumn2.DataType = Type.GetType("System.String");
				dataColumn2.MaxLength = 40;
				dataColumn2.AllowDBNull = false;
				dataColumn2.Caption = "程式名稱";
				dataColumn2.ColumnName = "程式名稱";
				ds.Tables["License"].Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				DataColumn dataColumn3 = dataColumn;
				dataColumn3.DataType = Type.GetType("System.String");
				dataColumn3.MaxLength = 40;
				dataColumn3.Caption = "FixKeyNum";
				dataColumn3.ColumnName = "FixKeyNum";
				ds.Tables["License"].Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				DataColumn dataColumn4 = dataColumn;
				dataColumn4.DataType = Type.GetType("System.String");
				dataColumn4.MaxLength = 40;
				dataColumn4.AllowDBNull = false;
				dataColumn4.DefaultValue = "TrendTek";
				dataColumn4.Caption = "NodeName";
				dataColumn4.ColumnName = "NodeName";
				ds.Tables["License"].Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				DataColumn dataColumn5 = dataColumn;
				dataColumn5.DataType = Type.GetType("System.String");
				dataColumn5.MaxLength = 40;
				dataColumn5.Caption = "MajorVersion";
				dataColumn5.ColumnName = "MajorVersion";
				ds.Tables["License"].Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				DataColumn dataColumn6 = dataColumn;
				dataColumn6.DataType = Type.GetType("System.String");
				dataColumn6.MaxLength = 40;
				dataColumn6.Caption = "MinorVersion";
				dataColumn6.ColumnName = "MinorVersion";
				ds.Tables["License"].Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				DataColumn dataColumn7 = dataColumn;
				dataColumn7.DataType = Type.GetType("System.Boolean");
				dataColumn7.AllowDBNull = false;
				dataColumn7.DefaultValue = true;
				dataColumn7.Caption = "KeyLk";
				dataColumn7.ColumnName = "KeyLk";
				ds.Tables["License"].Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				DataColumn dataColumn8 = dataColumn;
				dataColumn8.DataType = Type.GetType("System.Boolean");
				dataColumn8.AllowDBNull = false;
				dataColumn8.DefaultValue = true;
				dataColumn8.Caption = "NodeLk";
				dataColumn8.ColumnName = "NodeLk";
				ds.Tables["License"].Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				DataColumn dataColumn9 = dataColumn;
				dataColumn9.DataType = Type.GetType("System.Boolean");
				dataColumn9.AllowDBNull = false;
				dataColumn9.DefaultValue = true;
				dataColumn9.Caption = "MajorLk";
				dataColumn9.ColumnName = "MajorLk";
				ds.Tables["License"].Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				DataColumn dataColumn10 = dataColumn;
				dataColumn10.DataType = Type.GetType("System.Boolean");
				dataColumn10.AllowDBNull = false;
				dataColumn10.DefaultValue = false;
				dataColumn10.Caption = "MinorLk";
				dataColumn10.ColumnName = "MinorLk";
				ds.Tables["License"].Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				DataColumn dataColumn11 = dataColumn;
				dataColumn11.DataType = Type.GetType("System.String");
				dataColumn11.MaxLength = 40;
				dataColumn11.AllowDBNull = false;
				dataColumn11.Caption = "業務員";
				dataColumn11.ColumnName = "業務員";
				ds.Tables["License"].Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				DataColumn dataColumn12 = dataColumn;
				dataColumn12.DataType = Type.GetType("System.String");
				dataColumn12.MaxLength = 40;
				dataColumn12.AllowDBNull = false;
				dataColumn12.Caption = "客戶名稱";
				dataColumn12.ColumnName = "客戶名稱";
				ds.Tables["License"].Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				DataColumn dataColumn13 = dataColumn;
				dataColumn13.DataType = Type.GetType("System.DateTime");
				dataColumn13.AllowDBNull = false;
				dataColumn13.DefaultValue = Strings.Format(DateAndTime.Now, "yyyy/MM/dd");
				dataColumn13.Caption = "日期";
				dataColumn13.ColumnName = "日期";
				ds.Tables["License"].Columns.Add(dataColumn);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00003964 File Offset: 0x00001B64
		public void sugLogError(string sMsg)
		{
			string text = Strings.Format(DateAndTime.Today, "yyyyMMdd");
			try
			{
				sMsg = sMsg.Trim(new char[] { '\r' });
				sMsg = sMsg.Replace("\r\n", "\r\n\t\t\t");
				sMsg = "\t\t\t" + sMsg;
				StreamWriter streamWriter = File.AppendText(string.Concat(new string[]
				{
					Application.StartupPath,
					"\\",
					Application.ProductName,
					text,
					".log"
				}));
				sMsg = sMsg;
				sMsg = string.Concat(new string[]
				{
					Strings.Format(DateAndTime.Now, "MM/dd/yyyy HH:mm:ss"),
					"\tChecking License (v",
					FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion.ToString(),
					")> \r\n",
					sMsg
				});
				streamWriter.WriteLine(sMsg);
				streamWriter.Close();
			}
			catch (Exception ex)
			{
				Debug.WriteLine("subError> " + ex.Message);
			}
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00003A90 File Offset: 0x00001C90
		private void subGetKeyIdByHASPDll(string AssignedKeyNum, ref string LicenseCheckResult, ref bool bHASPKeyHasRead, ref List<string> HASPIds, ref bool bCheckMultiCondictionIsMeetRequired, ref bool GotoNextLoop)
		{
			clsCommon clsCommon = new clsCommon();
			try
			{
				bool flag = !bHASPKeyHasRead;
				if (flag)
				{
					string text = "";
					string text2 = Application.StartupPath + "\\";
					string text3 = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?> <haspscope/>";
					string text4 = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\"<admin>\"\"<hasp>\"<element name=\"vendorid\"/><element name=\"haspid\"/><element name=\"typename\"/></hasp></admin>";
					this.HASPDllCheck("hasp_net_windows.dll");
					this.HASPDllCheck("sntl_adminapi_net_windows.dll");
					this.HASPDllCheck("sntl_adminapi_windows.dll");
					this.HASPDllCheck("sntl_adminapi_windows_x64.dll");
					AdminApi adminApi = new AdminApi();
					AdminStatus adminStatus = adminApi.connect();
					this.HASPcheckStatus(adminStatus);
					adminStatus = adminApi.adminGet(text3, text4, ref text);
					this.HASPcheckStatus(adminStatus);
					bool flag2 = !string.IsNullOrEmpty(text);
					string[] array;
					if (flag2)
					{
						array = text.Split(new char[] { '\n' });
					}
					bool flag3 = array.Length > 0;
					if (flag3)
					{
						foreach (string text5 in array)
						{
							bool flag4 = text5.IndexOf("<haspid>") >= 0;
							if (flag4)
							{
								string text6 = clsCommon.funGetValueInString(text5, "<haspid>", "</haspid>");
								HASPIds.Add(text6);
							}
						}
					}
				}
				bHASPKeyHasRead = true;
				bool flag5 = HASPIds.Count < 1;
				if (flag5)
				{
					LicenseCheckResult = "Advantage KEY is not plugged in.";
					bCheckMultiCondictionIsMeetRequired = false;
					GotoNextLoop = true;
				}
				else
				{
					AssignedKeyNum = AssignedKeyNum.Substring(2);
					bool flag6 = !HASPIds.Contains(AssignedKeyNum);
					if (flag6)
					{
						LicenseCheckResult = "Fix Key Number is not correct.";
						bCheckMultiCondictionIsMeetRequired = false;
						GotoNextLoop = true;
					}
					else
					{
						bCheckMultiCondictionIsMeetRequired = true;
					}
				}
			}
			catch (Exception ex)
			{
				LicenseCheckResult = ex.Message;
				GotoNextLoop = true;
			}
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00003C5C File Offset: 0x00001E5C
		private void subGetKeyIdByiKeyDiag(string AssignedKeyNum, ref string LicenseCheckResult, ref bool biKeyDiagHasRead, ref string sKeyReportFileName, ref int lKeySerialNumber, ref int iKeyDiagSerialNumberReadTimes, ref bool bCheckMultiCondictionIsMeetRequired, ref bool GotoNextLoop)
		{
			checked
			{
				try
				{
					string text = "";
					bool flag = !biKeyDiagHasRead;
					if (!flag)
					{
						goto IL_04F5;
					}
					RegistryKey localMachine = Registry.LocalMachine;
					RegistryKey registryKey = localMachine.OpenSubKey("SYSTEM\\\\CurrentControlSet\\\\Services\\\\iLicenseSvc");
					bool flag2 = registryKey != null;
					if (flag2)
					{
						object obj = RuntimeHelpers.GetObjectValue(registryKey.GetValue("ImagePath"));
						bool flag3 = obj != null;
						if (flag3)
						{
							text = obj.ToString().Replace("iLicenseSvc.exe", "");
						}
					}
					else
					{
						registryKey = localMachine.OpenSubKey("SOFTWARE\\\\Microsoft\\\\Windows\\\\CurrentVersion\\\\App Paths\\\\iKeyDiag.exe");
						bool flag4 = registryKey != null;
						if (flag4)
						{
							object obj = RuntimeHelpers.GetObjectValue(registryKey.GetValue("path"));
							bool flag5 = obj != null;
							if (flag5)
							{
								text = obj.ToString().Replace("iKeyDiag.exe", "");
							}
						}
					}
					text = text.Replace("\"", "");
					bool flag6 = text.Length > 2 && File.Exists(text + "iKeyDiag.exe");
					if (!flag6)
					{
						text = Environment.GetFolderPath(Environment.SpecialFolder.System);
						DirectoryInfo directoryInfo = new DirectoryInfo(text);
						text = directoryInfo.Root.ToString() + "\\Program Files\\M1 Licensing\\";
						bool flag7 = File.Exists(text + "iKeyDiag.exe");
						if (!flag7)
						{
							text = directoryInfo.Root.ToString() + "\\Program Files (x86)\\M1 Licensing\\";
							bool flag8 = File.Exists(text + "iKeyDiag.exe");
							if (!flag8)
							{
								text = directoryInfo.Root.ToString() + directoryInfo.Parent.ToString() + "\\Intellution\\";
								bool flag9 = File.Exists(text + "iKeyDiag.exe");
								if (!flag9)
								{
									LicenseCheckResult = "The file <iKeyDiag.exe> of iFix is not existed.";
									bCheckMultiCondictionIsMeetRequired = false;
									GotoNextLoop = true;
									return;
								}
							}
						}
					}
					StringBuilder stringBuilder = new StringBuilder(256);
					StringBuilder stringBuilder2 = new StringBuilder(64);
					short num = (short)Helper.FixGetPath("BASPATH", stringBuilder2, 64);
					bool flag10 = num != 11000;
					if (flag10)
					{
						num = (short)Helper.NlsGetText((int)num, stringBuilder, 256);
						LicenseCheckResult = stringBuilder.ToString();
						bCheckMultiCondictionIsMeetRequired = false;
						GotoNextLoop = true;
						return;
					}
					sKeyReportFileName = this.csFixHelper.funRemoveNull(stringBuilder2.ToString()) + "\\key_report.txt";
					string text2 = text + "iKeyDiag.exe " + sKeyReportFileName;
					for (;;)
					{
						IL_025C:
						bool flag11 = !File.Exists(sKeyReportFileName);
						if (flag11)
						{
							FileStream fileStream = File.Create(sKeyReportFileName);
							File.SetAttributes(sKeyReportFileName, FileAttributes.Temporary | FileAttributes.NotContentIndexed);
							fileStream.Close();
							goto IL_038A;
						}
						DateTime lastWriteTime = File.GetLastWriteTime(sKeyReportFileName);
						int num2;
						try
						{
							num2 = Helper.FixIsFixRunning();
						}
						catch (Exception ex)
						{
							Debug.Print("在同時間內可能有數個AP要檢查發生錯誤<外部元件傳回例外狀況>: " + ex.Message);
							goto IL_0396;
						}
						bool flag12 = num2 != 1;
						if (!flag12)
						{
							DateTime dateTime;
							bool flag13 = this.funGetProcessStartTime("FIX", ref dateTime);
							if (flag13)
							{
								bool flag14 = DateTime.Compare(dateTime, lastWriteTime) < 0;
								if (flag14)
								{
									break;
								}
							}
						}
						bool flag15 = DateTime.Compare(DateAndTime.Now, DateAndTime.DateAdd(DateInterval.Minute, 1.0, lastWriteTime)) > 0;
						if (flag15)
						{
							File.SetAttributes(sKeyReportFileName, FileAttributes.Temporary | FileAttributes.NotContentIndexed);
							StreamWriter streamWriter = new StreamWriter(sKeyReportFileName, false);
							try
							{
								streamWriter.Write("");
							}
							catch (Exception ex2)
							{
								goto IL_0396;
							}
							finally
							{
								bool flag16 = !Information.IsNothing(streamWriter);
								if (flag16)
								{
									streamWriter.Close();
								}
							}
							goto IL_038A;
						}
						IL_0396:
						bool flag17 = !File.Exists(sKeyReportFileName);
						if (!flag17)
						{
							break;
						}
						int num3;
						bool flag18 = num3 < 2;
						if (flag18)
						{
							num3++;
							Thread.Sleep(500);
							continue;
						}
						goto IL_03C9;
						IL_038A:
						Interaction.Shell(text2, AppWinStyle.Hide, false, -1);
						goto IL_0396;
					}
					goto IL_03DF;
					IL_03C9:
					LicenseCheckResult = "Can not check the iFix's dongle, iKeyDiag.exe might have a problem.";
					bCheckMultiCondictionIsMeetRequired = false;
					GotoNextLoop = true;
					return;
					IL_03DF:
					StreamReader streamReader = new StreamReader(sKeyReportFileName);
					string text3 = streamReader.ReadLine();
					while (text3 != null)
					{
						bool flag19 = Strings.InStr(text3, "NO LICENSE PRESENT", CompareMethod.Text) != 0;
						if (flag19)
						{
							bool flag20 = !Information.IsNothing(streamReader);
							if (flag20)
							{
								streamReader.Close();
							}
							File.SetAttributes(sKeyReportFileName, FileAttributes.Hidden | FileAttributes.Temporary | FileAttributes.NotContentIndexed);
							LicenseCheckResult = "iFix's dongle is not plugged in.";
							bCheckMultiCondictionIsMeetRequired = false;
							biKeyDiagHasRead = true;
							GotoNextLoop = true;
							return;
						}
						bool flag21 = Strings.InStr(text3, "Serial", CompareMethod.Text) != 0;
						if (flag21)
						{
							int num4 = Strings.InStr(text3, ":", CompareMethod.Text);
							lKeySerialNumber = (int)Math.Round(Conversion.Val(Strings.Trim(Strings.Mid(text3, num4 + 1))));
							bool flag22 = lKeySerialNumber < 0;
							if (flag22)
							{
								LicenseCheckResult = "iFix's dongle is not plugged in.";
								bCheckMultiCondictionIsMeetRequired = false;
								biKeyDiagHasRead = true;
								GotoNextLoop = true;
								return;
							}
							break;
						}
						else
						{
							text3 = streamReader.ReadLine();
						}
					}
					bool flag23 = !Information.IsNothing(streamReader);
					if (flag23)
					{
						streamReader.Close();
					}
					File.SetAttributes(sKeyReportFileName, FileAttributes.Hidden | FileAttributes.Temporary | FileAttributes.NotContentIndexed);
					IL_04F5:
					bool flag24 = (lKeySerialNumber == 0) & (iKeyDiagSerialNumberReadTimes <= 0);
					if (flag24)
					{
						iKeyDiagSerialNumberReadTimes++;
						Thread.Sleep(1000);
						goto IL_025C;
					}
					biKeyDiagHasRead = true;
					bool flag25 = Conversion.Val(AssignedKeyNum) != (double)lKeySerialNumber;
					if (flag25)
					{
						LicenseCheckResult = "Fix Key Number is not correct.";
						bCheckMultiCondictionIsMeetRequired = false;
						GotoNextLoop = true;
					}
					else
					{
						bCheckMultiCondictionIsMeetRequired = true;
					}
				}
				catch (IOException ex3)
				{
					int num5;
					bool flag26 = num5 < 2;
					if (flag26)
					{
						num5++;
						Thread.Sleep(1000);
						goto IL_025C;
					}
					bCheckMultiCondictionIsMeetRequired = true;
				}
			}
		}

		// Token: 0x06000023 RID: 35 RVA: 0x0000425C File Offset: 0x0000245C
		public string funFixLicenseCheck(string licFileName, string ExeName, string encryptKey, string encryptIV)
		{
			List<string> list = new List<string>();
			string text = "";
			string text2 = "No license is found.";
			string text3 = string.Empty;
			string text4 = string.Empty;
			checked
			{
				string text5;
				try
				{
					bool flag = !File.Exists(licFileName);
					if (flag)
					{
						text5 = "Trendtek.lic is not existed.";
					}
					else
					{
						FileInfo fileInfo = new FileInfo(licFileName);
						this.subCreateFixLicenseTable(this.ds);
						this.subReadFileTog_Ds(fileInfo, encryptKey, encryptIV);
						string text6 = "程式名稱 = '" + ExeName + "'";
						text6 += " OR 程式名稱 = 'ENTERPRISE'";
						DataRow[] array = this.ds.Tables["License"].Select(text6, "");
						bool flag2 = array.Length < 1;
						if (flag2)
						{
							text2 = "This module >>" + ExeName + "<< is not authorized.";
						}
						else
						{
							int num = array.Length - 1;
							int i = 0;
							while (i <= num)
							{
								bool flag3 = Conversions.ToBoolean(array[i]["KeyLk"]);
								if (!flag3)
								{
									goto IL_015A;
								}
								bool flag4 = false;
								text3 = Conversions.ToString(array[i]["FixKeyNum"]);
								bool flag5 = text3.IndexOf("@A") == 0;
								bool flag7;
								if (flag5)
								{
									bool flag6;
									this.subGetKeyIdByHASPDll(text3, ref text2, ref flag6, ref list, ref flag7, ref flag4);
								}
								else
								{
									bool flag8;
									int num2;
									int num3;
									this.subGetKeyIdByiKeyDiag(text3, ref text2, ref flag8, ref text, ref num2, ref num3, ref flag7, ref flag4);
								}
								bool flag9 = flag4;
								if (!flag9)
								{
									goto IL_015A;
								}
								IL_07DF:
								bool flag10 = Operators.CompareString(text2, "OK", false) != 0;
								if (flag10)
								{
									text4 = string.Concat(new string[] { text4, "\r\ncheck for [", text3, "] - ", text2 });
								}
								i++;
								continue;
								IL_015A:
								bool flag11 = Conversions.ToBoolean(array[i]["NodeLk"]);
								if (flag11)
								{
									DataRow dataRow;
									object[] array2;
									bool[] array3;
									object obj = NewLateBinding.LateGet(null, typeof(Strings), "UCase", array2 = new object[] { (dataRow = array[i])["NodeName"] }, null, null, array3 = new bool[] { true });
									if (array3[0])
									{
										dataRow["NodeName"] = RuntimeHelpers.GetObjectValue(RuntimeHelpers.GetObjectValue(array2[0]));
									}
									string text7 = Conversions.ToString(obj);
									bool flag12 = Operators.CompareString(Strings.Mid(text7, 1, 2).ToUpper(), "OS", false) == 0;
									if (flag12)
									{
										ManagementClass managementClass = new ManagementClass("Win32_OperatingSystem");
										ManagementObjectCollection instances = managementClass.GetInstances();
										try
										{
											foreach (ManagementBaseObject managementBaseObject in instances)
											{
												ManagementObject managementObject = (ManagementObject)managementBaseObject;
												foreach (PropertyData propertyData in managementObject.Properties)
												{
													try
													{
														bool flag13 = Operators.CompareString(propertyData.Name.ToUpper(), "SERIALNUMBER", false) == 0;
														if (flag13)
														{
															bool flag14 = Operators.CompareString(text7.ToUpper(), "OS" + managementObject[propertyData.Name].ToString().ToUpper(), false) == 0;
															if (flag14)
															{
																flag7 = true;
																goto IL_06D1;
															}
														}
													}
													catch (NullReferenceException ex)
													{
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
									else
									{
										bool flag15 = Operators.CompareString(Strings.Mid(text7, 1, 3).ToUpper(), "CPU", false) == 0;
										if (flag15)
										{
											ManagementClass managementClass2 = new ManagementClass("Win32_Processor");
											ManagementObjectCollection instances2 = managementClass2.GetInstances();
											try
											{
												foreach (ManagementBaseObject managementBaseObject2 in instances2)
												{
													ManagementObject managementObject2 = (ManagementObject)managementBaseObject2;
													foreach (PropertyData propertyData2 in managementObject2.Properties)
													{
														try
														{
															bool flag16 = Operators.CompareString(propertyData2.Name.ToUpper(), "PROCESSORID", false) == 0;
															if (flag16)
															{
																bool flag17 = Operators.CompareString(text7.ToUpper(), "CPU" + managementObject2[propertyData2.Name].ToString().ToUpper(), false) == 0;
																if (flag17)
																{
																	flag7 = true;
																	goto IL_06D1;
																}
															}
														}
														catch (NullReferenceException ex2)
														{
														}
													}
												}
											}
											finally
											{
												ManagementObjectCollection.ManagementObjectEnumerator enumerator3;
												if (enumerator3 != null)
												{
													((IDisposable)enumerator3).Dispose();
												}
											}
										}
										else
										{
											bool flag18 = Strings.InStr(text7, ":", CompareMethod.Binary) > 0;
											if (flag18)
											{
												ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration");
												ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get();
												text7 = text7.Trim();
												try
												{
													foreach (ManagementBaseObject managementBaseObject3 in managementObjectCollection)
													{
														ManagementObject managementObject3 = (ManagementObject)managementBaseObject3;
														bool flag19 = Operators.CompareString(managementObject3["IPEnabled"].ToString(), "True", false) == 0;
														if (flag19)
														{
															bool flag20 = Operators.CompareString(text7.ToUpper(), Strings.UCase(managementObject3["MACAddress"].ToString()), false) == 0;
															if (flag20)
															{
																flag7 = true;
																goto IL_06D1;
															}
														}
													}
												}
												finally
												{
													ManagementObjectCollection.ManagementObjectEnumerator enumerator5;
													if (enumerator5 != null)
													{
														((IDisposable)enumerator5).Dispose();
													}
												}
											}
											else
											{
												bool flag21 = Strings.InStr(text7, ".", CompareMethod.Binary) > 0;
												if (flag21)
												{
													string[] array4 = text7.Split(new char[] { '.' });
													bool flag22 = array4.Length == 5;
													if (flag22)
													{
														IPHostEntry hostByName = Dns.GetHostByName(Dns.GetHostName());
														bool flag23 = Operators.CompareString(array4[4].ToUpper(), hostByName.HostName.ToUpper(), false) == 0;
														if (flag23)
														{
															text7 = string.Concat(new string[]
															{
																Conversions.ToInteger(array4[0]).ToString(),
																".",
																Conversions.ToInteger(array4[1]).ToString(),
																".",
																Conversions.ToInteger(array4[2]).ToString(),
																".",
																Conversions.ToInteger(array4[3]).ToString()
															});
															int num4 = hostByName.AddressList.Length - 1;
															for (int j = 0; j <= num4; j++)
															{
																bool flag24 = Operators.CompareString(text7, hostByName.AddressList[j].ToString(), false) == 0;
																if (flag24)
																{
																	flag7 = true;
																	goto IL_06D1;
																}
															}
														}
													}
												}
											}
										}
									}
									bool flag25 = Operators.CompareString(Strings.Mid(text7, 1, 2).ToUpper(), "OS", false) == 0;
									if (flag25)
									{
										text2 = "OS Serial Number is not correct.";
									}
									else
									{
										bool flag26 = Operators.CompareString(Strings.Mid(text7, 1, 3).ToUpper(), "CPU", false) == 0;
										if (flag26)
										{
											text2 = "CPU Serial Number is not correct.";
										}
										else
										{
											bool flag27 = Strings.InStr(text7, ":", CompareMethod.Binary) > 0;
											if (flag27)
											{
												text2 = "MAC Address is not correct.";
											}
											else
											{
												bool flag28 = Strings.InStr(text7, ".", CompareMethod.Binary) > 0;
												if (flag28)
												{
													text2 = "IP Address is not correct.";
												}
												else
												{
													text2 = "Node Name is not correct.";
												}
											}
										}
									}
									flag7 = false;
									goto IL_07DF;
								}
								IL_06D1:
								bool flag29 = Conversions.ToBoolean(array[i]["MajorLk"]);
								if (flag29)
								{
									int num5 = this.csFixHelper.funGetMajorVersion(clsFixHelper.FixVersion.Major);
									bool flag30 = num5 < 0;
									if (flag30)
									{
										text2 = "Can not get the information Fix Majorversion.";
										flag7 = false;
										goto IL_07DF;
									}
									bool flag31 = Conversion.Val(RuntimeHelpers.GetObjectValue(array[i]["MajorVersion"])) != (double)num5;
									if (flag31)
									{
										text2 = "Fix MajorVersion is not correct.";
										flag7 = false;
										goto IL_07DF;
									}
									flag7 = true;
								}
								bool flag32 = Conversions.ToBoolean(array[i]["MinorLk"]);
								if (flag32)
								{
									int num6 = this.csFixHelper.funGetMajorVersion(clsFixHelper.FixVersion.Minor);
									bool flag33 = num6 < 0;
									if (flag33)
									{
										text2 = "Can not ge t the information of Fix Minorversion.";
										flag7 = false;
										goto IL_07DF;
									}
									bool flag34 = Conversion.Val(RuntimeHelpers.GetObjectValue(array[i]["MinorVersion"])) != (double)num6;
									if (flag34)
									{
										text2 = "Fix MinorVersion is not correct.";
										flag7 = false;
										goto IL_07DF;
									}
									flag7 = true;
								}
								bool flag35 = !flag7;
								if (flag35)
								{
									text2 = "The trendtek.lic is existed, but no any module is authorized.";
									goto IL_07DF;
								}
								text2 = "OK";
								break;
							}
							bool flag36 = Operators.CompareString(text2, "OK", false) == 0;
							if (flag36)
							{
								return text2;
							}
						}
						string text8 = this.funDemoLicenseCheck(licFileName, this.ds.Tables["License"]);
						if (Operators.CompareString(text8, "OK", false) != 0)
						{
							this.sugLogError(text4);
							text5 = text2;
						}
						else
						{
							text5 = "DEMO";
						}
					}
				}
				catch (Exception ex3)
				{
					Debug.Print(ex3.Message);
					text5 = ex3.Message;
				}
				return text5;
			}
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00004BC0 File Offset: 0x00002DC0
		private string funDemoLicenseCheck(string slicFileName, DataTable dt)
		{
			string text2;
			try
			{
				string text = "程式名稱 LIKE 'DEMO*'";
				DataRow[] array = this.ds.Tables["License"].Select(text, "");
				bool flag = array.Length < 1;
				if (flag)
				{
					text2 = "NODEMO";
				}
				else
				{
					bool flag2 = Strings.Len(RuntimeHelpers.GetObjectValue(array[0]["程式名稱"])) < 5;
					int num;
					if (flag2)
					{
						num = 2;
					}
					else
					{
						num = checked(Conversions.ToInteger(Strings.Mid(Conversions.ToString(array[0]["程式名稱"]), 5)) + 2);
					}
					bool flag3 = num > 365;
					if (flag3)
					{
						num = 365;
					}
					DateTime lastWriteTime = File.GetLastWriteTime(slicFileName);
					bool flag4 = DateTime.Compare(DateAndTime.Now, lastWriteTime) < 0;
					if (flag4)
					{
						text2 = "Demo license is expired, and machine's time might have been changed.";
					}
					else
					{
						object obj = DateAndTime.Now;
						object[] array2;
						bool[] array3;
						object obj2 = NewLateBinding.LateGet(array[0]["日期"], null, "AddDays", array2 = new object[] { num }, null, null, array3 = new bool[] { true });
						if (array3[0])
						{
							num = (int)Conversions.ChangeType(RuntimeHelpers.GetObjectValue(array2[0]), typeof(int));
						}
						bool flag5 = Conversions.ToBoolean(Operators.OrObject(Operators.CompareObjectGreater(obj, obj2, false), Operators.CompareObjectLess(DateAndTime.Now, array[0]["日期"], false)));
						if (flag5)
						{
							text2 = "Demo license is expired.";
						}
						else
						{
							File.SetLastWriteTime(slicFileName, DateAndTime.Now);
							text2 = "OK";
						}
					}
				}
			}
			catch (Exception ex)
			{
				text2 = ex.Message;
			}
			return text2;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00004D84 File Offset: 0x00002F84
		private void subReadFileTog_Ds(FileInfo myfile, string encryptKey, string encryptIV)
		{
			this.ds.Tables["License"].Clear();
			FileStream fileStream = myfile.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
			StringReader stringReader = new StringReader(this.csCrypt.DecryptFileRead(fileStream, encryptKey, encryptIV));
			this.subStringTog_Ds(stringReader, this.ds, "License");
			bool flag = !Information.IsNothing(stringReader);
			if (flag)
			{
				stringReader.Close();
			}
			bool flag2 = !Information.IsNothing(fileStream);
			if (flag2)
			{
				fileStream.Close();
			}
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00004E08 File Offset: 0x00003008
		private void subStringTog_Ds(StringReader sr, DataSet Ds, string tbName)
		{
			Ds.Tables[tbName].Clear();
			checked
			{
				while (sr.Peek() >= 0)
				{
					DataRow dataRow = Ds.Tables[tbName].NewRow();
					string[] array = Strings.Split(sr.ReadLine(), ",", -1, CompareMethod.Binary);
					int num = Information.UBound(array, 1);
					for (int i = 0; i <= num; i++)
					{
						bool flag = Operators.CompareString(Ds.Tables[tbName].Columns[i].ColumnName, "日期", false) == 0;
						if (flag)
						{
							try
							{
								array[i] = Conversions.ToString(Conversions.ToDate(array[i]));
							}
							catch (Exception ex)
							{
								string text = "你設定的時間 [" + Conversions.ToString(DateAndTime.Now) + "] 格式不對,此程式必使用時間格式如下:\r日期 - 西曆 MM/DD/YYYY 或 西曆 MM/DD/YY\r時間 - HH:mm:ss\r請至 '設定\\控制台\\地區選項' 內改變日期及時間格式.";
								throw new Exception(text);
							}
						}
						dataRow[i] = array[i];
					}
					Ds.Tables[tbName].Rows.Add(dataRow);
				}
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00004F28 File Offset: 0x00003128
		private bool funGetProcessStartTime(string myProcessName, ref DateTime myDate)
		{
			ObjectQuery objectQuery = new ObjectQuery("SELECT * FROM Win32_Process");
			ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(objectQuery);
			bool flag2;
			try
			{
				try
				{
					foreach (ManagementBaseObject managementBaseObject in managementObjectSearcher.Get())
					{
						ManagementObject managementObject = (ManagementObject)managementBaseObject;
						bool flag = Conversions.ToBoolean(LikeOperator.LikeObject(NewLateBinding.LateGet(managementObject["Name"], null, "ToUpper", new object[0], null, null, null), myProcessName.ToUpper() + "*", CompareMethod.Binary));
						if (flag)
						{
							string text = Strings.Mid(Conversions.ToString(managementObject["CreationDate"]), 1, 14);
							string text2 = string.Concat(new string[]
							{
								text.Substring(0, 4),
								"/",
								text.Substring(4, 2),
								"/",
								text.Substring(6, 2),
								" ",
								text.Substring(8, 2),
								":",
								text.Substring(10, 2),
								":",
								text.Substring(12, 2)
							});
							myDate = Conversions.ToDate(text2);
							return true;
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
				myDate = DateAndTime.Now;
				flag2 = false;
			}
			catch (SystemException ex)
			{
				myDate = DateAndTime.Now;
				flag2 = false;
			}
			return flag2;
		}

		// Token: 0x04000003 RID: 3
		private DataSet ds;

		// Token: 0x04000004 RID: 4
		private clsCrypt csCrypt;

		// Token: 0x04000005 RID: 5
		private clsFixHelper csFixHelper;

		// Token: 0x04000006 RID: 6
		private const string ctbLicense = "License";
	}
}
