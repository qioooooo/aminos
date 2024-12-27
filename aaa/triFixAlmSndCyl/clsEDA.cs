using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using GeFanuc.iFixToolkit.Adapter;
using kvNetClass;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace triFixAlmSndCyl
{
	// Token: 0x02000008 RID: 8
	public class clsEDA
	{
		// Token: 0x06000026 RID: 38 RVA: 0x00004774 File Offset: 0x00002B74
		public void subGetNodeList(ComboBox myList)
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
				clsFixHelper clsFixHelper = new clsFixHelper();
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
							if (Operators.CompareString(clsFixHelper.funRemoveNull(array[i]).ToUpper(), "THISNODE", false) != 0)
							{
								array2[i] = clsFixHelper.funRemoveNull(array[i]);
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
				if (myList.Items.Count < 1)
				{
					myList.Items.Add(text);
				}
				int num12 = myList.FindString(text.ToUpper());
				if (myList.Items.Count > 0 && num12 < 0)
				{
					num12 = 0;
				}
				myList.SelectedIndex = num12;
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000048D4 File Offset: 0x00002CD4
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
						text = this.funRemoveNull(text).Trim();
						if (text.Length >= 1)
						{
							DataRow dataRow = dt.NewRow();
							dataRow[0] = text;
							dataRow[1] = nType;
							dt.Rows.Add(dataRow);
						}
					}
					if (nMax != num)
					{
						goto Block_5;
					}
				}
				Helper.NlsGetText((int)num2, stringBuilder, checked((short)stringBuilder.Capacity));
				throw new Exception(stringBuilder.ToString());
				Block_5:;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000049C4 File Offset: 0x00002DC4
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
						text = this.funRemoveNull(text).Trim();
						if (text.Length >= 1)
						{
							DataRow dataRow = dt.NewRow();
							dataRow[0] = text;
							dataRow[1] = nType;
							dt.Rows.Add(dataRow);
						}
					}
					if (numRequest != num)
					{
						goto Block_5;
					}
				}
				Helper.NlsGetText((int)num2, stringBuilder, checked((short)stringBuilder.Capacity));
				throw new Exception(stringBuilder.ToString());
				Block_5:;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00004AB4 File Offset: 0x00002EB4
		public bool FixIsFixRunning()
		{
			bool flag;
			try
			{
				int num = Helper.FixIsFixRunning();
				if (num != 1)
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
				flag = false;
			}
			return flag;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00004AF4 File Offset: 0x00002EF4
		private string funRemoveNull(string sString)
		{
			if (Strings.Asc(Strings.Mid(sString, 1, 1)) == 0)
			{
				return "";
			}
			int num = checked(Strings.InStr(1, sString, "\0", CompareMethod.Binary) - 1);
			if (num > 0)
			{
				return Strings.Left(sString, num);
			}
			return Strings.Trim(sString);
		}
	}
}
