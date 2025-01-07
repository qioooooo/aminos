using System;
using System.Runtime.InteropServices;
using System.Text;

namespace GeFanuc.iFixToolkit.Adapter
{
	public sealed class Eda
	{
		[DllImport("eda.dll", EntryPoint = "eda_add_block")]
		public static extern short AddBlock(string sNodeName, string sTagName, short nBlkType);

		[DllImport("eda.dll", EntryPoint = "eda_alm_get_user_que_ex")]
		private static extern short AlmGetUserQueEx(string szQueName, short nWait, out int pulAlmMsgType, out int pulTypers, out int pulPriority, [Out] StringBuilder pcText, [Out] StringBuilder pcLogicalNodeName, [Out] StringBuilder pcPhysicalNodeName, out int pulNumAreas, [Out] char[,] pcAreas, out FIX_SYSTEMTIME paTimeLast, out FIX_SYSTEMTIME paTimeIn, out AlmBlockRec paAlmBlockRec, out AlmOperRec paAlmOperRec, out AlmExtensionFields paAlmExtFields, out AlmID paAlmId, out int pulReserved, StringBuilder pMsgID, int iMsgIDSize);

		[DllImport("eda.dll", EntryPoint = "eda_alm_get_user_que_text")]
		public static extern short AlmGetUserQueText(string szQueName, short nWait, [Out] StringBuilder szText);

		[DllImport("eda.dll", EntryPoint = "eda_alm_send_text")]
		private static extern short AlmSendText(string szText, uint ulTypers, int ulNumAreas, [In] char[] pcAreas);

		[DllImport("eda.dll", EntryPoint = "eda_define_group")]
		public static extern IntPtr DefineGroup([MarshalAs(6)] short nNodes, short detect_changes);

		[DllImport("eda.dll", EntryPoint = "eda_define_ntf")]
		public static extern int DefineNtf(IntPtr gnum, string node, string tag, string field, [In] ref VSP vsp);

		[DllImport("eda.dll", EntryPoint = "eda_define_ntf")]
		public static extern int DefineNtf(IntPtr gnum, string node, string tag, string field, IntPtr vsp);

		[DllImport("eda.dll", EntryPoint = "eda_delete_block")]
		public static extern short DeleteBlock(string NodeName, string TagName, byte AutoLink);

		[DllImport("eda.dll", EntryPoint = "eda_delete_group")]
		public static extern void DeleteGroup(IntPtr gnum);

		[DllImport("eda.dll", EntryPoint = "eda_delete_ntf")]
		public static extern void DeleteNtf(IntPtr gnum, int tHandle);

		[DllImport("eda.dll", EntryPoint = "eda_dump")]
		public static extern void Dump(IntPtr gnum);

		[DllImport("eda.dll", EntryPoint = "eda_enum_all_scada_nodes")]
		private static extern short EnumAllScadaNodes([Out] char[,] caller_nodes, ref short startnode, [MarshalAs(6)] short maxcount, out short return_count);

		[DllImport("eda.dll", EntryPoint = "eda_enum_scada_nodes")]
		private static extern short EnumScadaNodes([Out] char[,] caller_nodes, ref short startnode, [MarshalAs(6)] short maxcount, out short return_count);

		[DllImport("eda.dll", EntryPoint = "eda_enum_tags")]
		private static extern ushort EnumTags(string node, [Out] char[,] tags, [Out] short[] ipnlist, short type, ref short startipn, [MarshalAs(6)] short maxcount, out short count);

		[DllImport("eda.dll", EntryPoint = "eda_enum_all_tags")]
		private static extern ushort EnumAllTags(string node, short type, string sfilter, [Out] char[,] tags, [MarshalAs(6)] short NumRequest, [MarshalAs(6)] out short NumReceive, out ENUMBUF eb);

		[DllImport("eda.dll", EntryPoint = "eda_enum_all_fields")]
		private static extern ushort EnumAllFields(string node, short type, [Out] char[,] fields, [MarshalAs(6)] short NumRequest, [MarshalAs(6)] out short NumReceive, out ENUMBUF eb);

		[DllImport("eda.dll", EntryPoint = "eda_get_ascii")]
		public static extern short GetAscii(IntPtr gnum, int thandle, [Out] StringBuilder buf, short buf_size);

		[DllImport("eda.dll", EntryPoint = "eda_get_binary")]
		public static extern short GetBinary(IntPtr gnum, int thandle, [Out] byte[] mydata);

		[DllImport("eda.dll", EntryPoint = "eda_get_error")]
		public static extern short GetError(IntPtr gnum, int thandle);

		[DllImport("eda.dll", EntryPoint = "eda_get_float")]
		public static extern short GetFloat(IntPtr gnum, int thandle, out float myfloat);

		[DllImport("eda.dll", EntryPoint = "eda_get_one_ascii")]
		public static extern short GetOneAscii(string node, string tag, string field, [Out] StringBuilder sBuf);

		[DllImport("eda.dll", EntryPoint = "eda_get_one_binary")]
		public static extern short GetOneBinary(string node, string tag, string field, [Out] byte[] buf);

		[DllImport("eda.dll", EntryPoint = "eda_get_one_float")]
		public static extern short GetOneFloat(string node, string tag, string field, out float fVal);

		[DllImport("eda.dll", EntryPoint = "eda_get_pdb_name")]
		public static extern short GetPdbName(string node, [Out] StringBuilder pdbname);

		[DllImport("eda.dll", EntryPoint = "eda_get_pdb_name")]
		public static extern short GetPdbName(string node, [Out] StringBuilder pdbname, short MaxSize);

		[DllImport("eda.dll", EntryPoint = "eda_lookup")]
		public static extern void Lookup(IntPtr gnum);

		[DllImport("eda.dll", EntryPoint = "eda_query_count")]
		public static extern short QueryCount(IntPtr gnum);

		[DllImport("eda.dll", EntryPoint = "eda_query_item_size")]
		public static extern short QueryItemSize(IntPtr hGroup, int hTag);

		[DllImport("eda.dll", EntryPoint = "eda_query_max_nodes")]
		public static extern short QueryMaxNodes(IntPtr gnum);

		[DllImport("eda.dll", EntryPoint = "eda_query_ntf")]
		public static extern void QueryNtf(IntPtr gnum, int thandle, [Out] StringBuilder node, [Out] StringBuilder tag, [Out] StringBuilder field, out VSP thisvsp);

		[DllImport("eda.dll", EntryPoint = "eda_query_one_pdbsn")]
		public static extern short QueryOnePdbsn(IntPtr gnum, string node, out int pdbsn);

		[DllImport("eda.dll", EntryPoint = "eda_query_pdbsn")]
		private static extern void QueryPdbsn(IntPtr gnum, [Out] int[] pdbsn, [Out] char[,] names);

		[DllImport("eda.dll", EntryPoint = "eda_query_wait")]
		public static extern short QueryWait(IntPtr gnum);

		[DllImport("eda.dll", EntryPoint = "eda_read")]
		public static extern void Read(IntPtr gnum);

		[DllImport("eda.dll", EntryPoint = "eda_reload_database")]
		public static extern short ReloadDatabase(string nodename, string db_name);

		[DllImport("eda.dll", EntryPoint = "eda_save_database")]
		public static extern short SaveDatabase(string NodeName, string DatabaseName);

		[DllImport("eda.dll", EntryPoint = "eda_send_msg")]
		public static extern void SendMsg(short atyper, short adi, short timeFlag, [In] string text);

		[DllImport("eda.dll", EntryPoint = "eda_set_ascii")]
		public static extern short SetAscii(IntPtr gnum, int thandle, [In] string mychar);

		[DllImport("eda.dll", EntryPoint = "eda_set_binary")]
		public static extern short SetBinary(IntPtr gnum, int thandle, [In] byte[] mydata);

		[DllImport("eda.dll", EntryPoint = "eda_set_float")]
		public static extern short SetFloat(IntPtr gnum, int thandle, ref float myfloat);

		[DllImport("eda.dll", EntryPoint = "eda_set_key")]
		public static extern void SetKey(IntPtr gnum, int key);

		[DllImport("eda.dll", EntryPoint = "eda_set_one_ascii")]
		public static extern short SetOneAscii(string node, string tag, string field, [In] string sVal, int key);

		[DllImport("eda.dll", EntryPoint = "eda_set_one_binary")]
		public static extern short SetOneBinary(string node, string tag, string field, [In] byte[] buf, int key);

		[DllImport("eda.dll", EntryPoint = "eda_set_one_float")]
		public static extern short SetOneFloat(string node, string tag, string field, ref float fVal, int key);

		[DllImport("eda.dll", EntryPoint = "eda_set_one_pdbsn")]
		public static extern short SetOnePdbsn(IntPtr gnum, string node, int sn);

		[DllImport("eda.dll", EntryPoint = "eda_set_pdbsn")]
		private static extern short SetPdbsn(IntPtr gnum, [In] int[] pdbsn, [In] char[] names, short node_count);

		[DllImport("eda.dll", EntryPoint = "eda_set_refresh")]
		public static extern short SetRefresh(IntPtr gnum, byte resolution, byte units);

		[DllImport("eda.dll", EntryPoint = "eda_system_write")]
		public static extern short SystemWrite(IntPtr group, string user, string pword);

		[DllImport("eda.dll", EntryPoint = "eda_type_to_index")]
		public static extern short TypeToIndex(string Node, string TypeName);

		[DllImport("eda.dll", EntryPoint = "eda_value_changed")]
		public static extern short ValueChanged(IntPtr gnum, int thandle);

		[DllImport("eda.dll", EntryPoint = "eda_wait")]
		public static extern void Wait(IntPtr gnum);

		[DllImport("eda.dll", EntryPoint = "eda_write")]
		public static extern void Write(IntPtr gnum);

		[DllImport("eda.dll", EntryPoint = "eda_write1")]
		public static extern short Write1(IntPtr gnum, int thandle);

		[DllImport("eda.dll", EntryPoint = "eda_write_impersonate")]
		public static extern short WriteImpersonate(IntPtr gnum, string user, string password);

		private Eda()
		{
		}

		public static short AlmSendText(string sText, long ulTypers, int inumAreas, string[] asAreas)
		{
			if (asAreas == null || sText == null)
			{
				string text = ((asAreas == null) ? "asAreas" : "sText");
				throw new ArgumentNullException(text);
			}
			if (asAreas.Length == 0)
			{
				string text = "Length of asAreas cannot be 0";
				throw new ArgumentException(text, "asAreas");
			}
			if (inumAreas <= 0)
			{
				string text = string.Format("Invalid argument value:  inumAreas = {0}", inumAreas);
				throw new ArgumentException(text, "inumAreas");
			}
			uint num = (uint)ulTypers;
			short num2 = (short)asAreas.Length;
			char[] array;
			int i;
			int num3;
			checked
			{
				array = new char[(uint)(unchecked(num2 * 30))];
				i = 0;
				num3 = 0;
			}
			while (i < (int)num2)
			{
				char[] array2 = asAreas[i].ToUpper().ToCharArray();
				array2.CopyTo(array, num3);
				i++;
				num3 += 30;
			}
			return Eda.AlmSendText(sText, num, inumAreas, array);
		}

		public unsafe static short EnumAllScadaNodes(out string[] asCallerNodes, ref short nstartnode, short nmaxcount, out short nreturn_count)
		{
			short num = 0;
			nreturn_count = 0;
			asCallerNodes = new string[0];
			if (nmaxcount <= 0)
			{
				string text = string.Format("Invalid argument value:  nmaxcount = {0}", nmaxcount);
				throw new ArgumentException(text);
			}
			char[,] array;
			short num2;
			checked
			{
				array = new char[(int)((uint)(unchecked((nmaxcount + 1) * 3))), 9];
				num2 = Eda.EnumAllScadaNodes(array, ref nstartnode, nmaxcount, out num);
			}
			if (num > 0)
			{
				int num3 = (int)(num * 3);
				asCallerNodes = new string[checked((uint)num3)];
				for (int i = 0; i < num3; i++)
				{
					fixed (char* ptr = &array[i, 0])
					{
						asCallerNodes[i] = new string(ptr, 0, 9);
					}
				}
			}
			nreturn_count = num;
			return num2;
		}

		public unsafe static short EnumScadaNodes(out string[] asCallerNodes, ref short nstartnode, short nmaxcount, out short nreturn_count)
		{
			short num = 0;
			nreturn_count = 0;
			asCallerNodes = new string[0];
			if (nmaxcount <= 0)
			{
				string text = string.Format("Invalid argument value: nmaxcount = {0}", nmaxcount);
				throw new ArgumentException(text);
			}
			char[,] array = new char[(int)(checked((uint)nmaxcount)), 9];
			short num2 = Eda.EnumScadaNodes(array, ref nstartnode, nmaxcount, out num);
			if (num > 0)
			{
				asCallerNodes = new string[checked((uint)num)];
				for (int i = 0; i < (int)num; i++)
				{
					fixed (char* ptr = &array[i, 0])
					{
						asCallerNodes[i] = new string(ptr, 0, 9);
					}
				}
			}
			nreturn_count = num;
			return num2;
		}

		public unsafe static short EnumTags(string sznodeName, out string[] asNodeTags, out short[] anIpnlist, short ntype, ref short nstartipn, short nmaxcount, out short ncount)
		{
			asNodeTags = new string[0];
			anIpnlist = new short[0];
			ncount = 0;
			if (nmaxcount <= 0)
			{
				string text = string.Format("Invalid argument value: nmaxcount = {0}", nmaxcount);
				throw new ArgumentException(text);
			}
			char[,] array;
			ushort num;
			checked
			{
				array = new char[(int)((uint)nmaxcount), 31];
				anIpnlist = new short[(uint)nmaxcount];
				num = Eda.EnumTags(sznodeName, array, anIpnlist, ntype, ref nstartipn, nmaxcount, out ncount);
			}
			if (ncount > 0)
			{
				asNodeTags = new string[checked((uint)ncount)];
				for (int i = 0; i < (int)ncount; i++)
				{
					fixed (char* ptr = &array[i, 0])
					{
						asNodeTags[i] = new string(ptr, 0, 31);
					}
				}
			}
			return (short)num;
		}

		public unsafe static short EnumAllTags(string sNode, short ntype, string sFilter, out string[] asTags, short numrequest, out short numreceive, out ENUMBUF eb)
		{
			asTags = new string[0];
			numreceive = 0;
			if (numrequest <= 0)
			{
				string text = string.Format("Invalid argument value: numrequest = {0}", numrequest);
				throw new ArgumentException(text);
			}
			char[,] array = new char[(int)(checked((uint)numrequest)), 31];
			ushort num = Eda.EnumAllTags(sNode, ntype, sFilter, array, numrequest, out numreceive, out eb);
			if (numreceive > 0)
			{
				asTags = new string[checked((uint)numreceive)];
				for (int i = 0; i < (int)numreceive; i++)
				{
					fixed (char* ptr = &array[i, 0])
					{
						asTags[i] = new string(ptr, 0, 31);
					}
				}
			}
			return (short)num;
		}

		public unsafe static short EnumAllFields(string sNode, short ntype, out string[] asFields, short numrequest, out short numreceive, out ENUMBUF eb)
		{
			asFields = new string[0];
			numreceive = 0;
			if (numrequest <= 0)
			{
				string text = string.Format("Invalid argument value: numrequest = {0}", numrequest);
				throw new ArgumentException(text);
			}
			char[,] array = new char[(int)(checked((uint)numrequest)), 20];
			ushort num = Eda.EnumAllFields(sNode, ntype, array, numrequest, out numreceive, out eb);
			if (numreceive > 0)
			{
				asFields = new string[checked((uint)numreceive)];
				for (int i = 0; i < (int)numreceive; i++)
				{
					fixed (char* ptr = &array[i, 0])
					{
						asFields[i] = new string(ptr, 0, 20);
					}
				}
			}
			return (short)num;
		}

		public unsafe static short QueryPdbsn(IntPtr eh, int[] aiPdbsnList, string[] asNodeNames)
		{
			if (aiPdbsnList == null || asNodeNames == null)
			{
				string text = ((aiPdbsnList == null) ? "aiPdbsnList" : "asNodeNames");
				throw new ArgumentNullException(text);
			}
			if (asNodeNames.Length == 0)
			{
				string text = "Length of asNodeNames cannot be 0";
				throw new ArgumentException(text, "asNodeNames");
			}
			char[,] array = new char[(int)(checked((uint)asNodeNames.Length)), 9];
			Eda.QueryPdbsn(eh, aiPdbsnList, array);
			int num = asNodeNames.Length;
			for (int i = 0; i < num; i++)
			{
				fixed (char* ptr = &array[i, 0])
				{
					asNodeNames[i] = new string(ptr, 0, 9);
				}
			}
			return 0;
		}

		public static short SetPdbsn(IntPtr gnum, int[] aiPdbsn, string[] asNames, short node_count)
		{
			if (aiPdbsn == null || asNames == null)
			{
				string text = ((aiPdbsn == null) ? "aiPdbsn" : "asNames");
				throw new ArgumentNullException(text);
			}
			if (asNames.Length == 0)
			{
				string text = "Length of asNames cannot be 0";
				throw new ArgumentException(text, "asNames");
			}
			short num = (short)asNames.Length;
			char[] array;
			int i;
			int num2;
			checked
			{
				array = new char[(uint)(unchecked(num * 9))];
				i = 0;
				num2 = 0;
			}
			while (i < (int)num)
			{
				string text2 = asNames[i];
				char[] array2 = text2.ToUpper().ToCharArray();
				array2.CopyTo(array, num2);
				i++;
				num2 += 9;
			}
			return Eda.SetPdbsn(gnum, aiPdbsn, array, num);
		}

		public unsafe static short AlmGetUserQueEx(string szQueName, short nWait, out int pulAlmMsgType, out int pulTypers, out int pulPriority, StringBuilder pcText, StringBuilder pcLogicalNodeName, StringBuilder pcPhysicalNodeName, out int pulNumAreas, out string[] asAreas, out FIX_SYSTEMTIME paTimeLast, out FIX_SYSTEMTIME paTimeIn, out AlmBlockRec paAlmBlockRec, out AlmOperRec paAlmOperRec, out AlmExtensionFields paAlmExtFields, out AlmID paAlmId, out int iReserved, StringBuilder sbMsgID, int iMsgIDSize)
		{
			pulAlmMsgType = 0;
			pulTypers = 0;
			pulPriority = 0;
			pulNumAreas = 0;
			asAreas = new string[0];
			iReserved = 0;
			char[,] array = new char[15, 30];
			short num = Eda.AlmGetUserQueEx(szQueName, nWait, out pulAlmMsgType, out pulTypers, out pulPriority, pcText, pcLogicalNodeName, pcPhysicalNodeName, out pulNumAreas, array, out paTimeLast, out paTimeIn, out paAlmBlockRec, out paAlmOperRec, out paAlmExtFields, out paAlmId, out iReserved, sbMsgID, iMsgIDSize);
			if (pulNumAreas > 0)
			{
				asAreas = new string[checked((uint)pulNumAreas)];
				for (int i = 0; i < pulNumAreas; i++)
				{
					fixed (char* ptr = &array[i, 0])
					{
						asAreas[i] = new string(ptr, 0, 30);
					}
				}
			}
			return num;
		}
	}
}
