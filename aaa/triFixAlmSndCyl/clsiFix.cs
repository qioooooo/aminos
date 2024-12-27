using System;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Trendtek.iFix;

namespace triFixAlmSndCyl
{
	// Token: 0x02000009 RID: 9
	public class clsiFix
	{
		// Token: 0x0600002B RID: 43 RVA: 0x00004B3C File Offset: 0x00002F3C
		public clsiFix(string sPicName, Callback_Method callbackDelegate, string sSource = "", string sValue = "")
		{
			this._sPicname = sPicName;
			this._sSource = sSource;
			this._sValue = sValue;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00004B5C File Offset: 0x00002F5C
		public void OpenPicture()
		{
			FixHelper fixHelper = new FixHelper();
			try
			{
				object obj;
				bool flag;
				fixHelper.GetWorkspaceObject(ref obj, ref flag);
				if (!Information.IsNothing(RuntimeHelpers.GetObjectValue(obj)))
				{
					object obj2 = NewLateBinding.LateGet(obj, null, "Documents", new object[0], null, null, null);
					Type type = null;
					string text = "Open";
					object[] array = new object[] { this._sPicname, 3 };
					object[] array2 = array;
					string[] array3 = null;
					Type[] array4 = null;
					bool[] array5 = new bool[] { true, false };
					NewLateBinding.LateCall(obj2, type, text, array2, array3, array4, array5, true);
					if (array5[0])
					{
						this._sPicname = (string)Conversions.ChangeType(RuntimeHelpers.GetObjectValue(array[0]), typeof(string));
					}
				}
			}
			catch (Exception ex)
			{
			}
			finally
			{
				object obj;
				if (Information.IsNothing(RuntimeHelpers.GetObjectValue(obj)))
				{
					obj = null;
				}
			}
		}

		// Token: 0x0400000D RID: 13
		private string _sPicname;

		// Token: 0x0400000E RID: 14
		private string _sSource;

		// Token: 0x0400000F RID: 15
		private string _sValue;

		// Token: 0x04000010 RID: 16
		private Callback_Method Callback;
	}
}
