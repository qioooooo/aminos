using System;
using System.Collections;
using System.Data.Common;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace System.Data.OracleClient
{
	// Token: 0x02000050 RID: 80
	internal sealed class OracleCommandSet : IDisposable
	{
		// Token: 0x060002FF RID: 767 RVA: 0x0005FF60 File Offset: 0x0005F360
		public OracleCommandSet()
			: this(null, null)
		{
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0005FF78 File Offset: 0x0005F378
		public OracleCommandSet(OracleConnection connection, OracleTransaction transaction)
		{
			this._batchCommand = new OracleCommand();
			this.Connection = connection;
			this.Transaction = transaction;
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000301 RID: 769 RVA: 0x0005FFC0 File Offset: 0x0005F3C0
		private OracleCommand BatchCommand
		{
			get
			{
				OracleCommand batchCommand = this._batchCommand;
				if (batchCommand == null)
				{
					throw ADP.ObjectDisposed(base.GetType().Name);
				}
				return batchCommand;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000302 RID: 770 RVA: 0x0005FFEC File Offset: 0x0005F3EC
		public int CommandCount
		{
			get
			{
				return this.CommandList.Count;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000303 RID: 771 RVA: 0x00060004 File Offset: 0x0005F404
		private ArrayList CommandList
		{
			get
			{
				ArrayList commandList = this._commandList;
				if (commandList == null)
				{
					throw ADP.ObjectDisposed(base.GetType().Name);
				}
				return commandList;
			}
		}

		// Token: 0x17000082 RID: 130
		// (set) Token: 0x06000304 RID: 772 RVA: 0x00060030 File Offset: 0x0005F430
		public int CommandTimeout
		{
			set
			{
				this.BatchCommand.CommandTimeout = value;
			}
		}

		// Token: 0x17000083 RID: 131
		// (set) Token: 0x06000305 RID: 773 RVA: 0x0006004C File Offset: 0x0005F44C
		public OracleConnection Connection
		{
			set
			{
				this.BatchCommand.Connection = value;
			}
		}

		// Token: 0x17000084 RID: 132
		// (set) Token: 0x06000306 RID: 774 RVA: 0x00060068 File Offset: 0x0005F468
		internal OracleTransaction Transaction
		{
			set
			{
				this.BatchCommand.Transaction = value;
			}
		}

		// Token: 0x06000307 RID: 775 RVA: 0x00060084 File Offset: 0x0005F484
		public void Append(OracleCommand command)
		{
			ADP.CheckArgumentNull(command, "command");
			if (ADP.IsEmpty(command.CommandText))
			{
				throw ADP.CommandTextRequired("Append");
			}
			ICollection parameters = command.Parameters;
			OracleParameter[] array = new OracleParameter[parameters.Count];
			parameters.CopyTo(array, 0);
			string[] array2 = new string[array.Length];
			if (0 < array.Length)
			{
				for (int i = 0; i < array.Length; i++)
				{
					array2[i] = array[i].ParameterName;
					OracleParameter oracleParameter = command.CreateParameter();
					array[i].CopyTo(oracleParameter);
					object value = oracleParameter.Value;
					if (value is byte[])
					{
						byte[] array3 = (byte[])value;
						int offset = oracleParameter.Offset;
						int size = oracleParameter.Size;
						int num = array3.Length - offset;
						if (size != 0 && size < num)
						{
							num = size;
						}
						byte[] array4 = new byte[Math.Max(num, 0)];
						Buffer.BlockCopy(array3, offset, array4, 0, array4.Length);
						oracleParameter.Offset = 0;
						oracleParameter.Value = array4;
					}
					else if (value is char[])
					{
						char[] array5 = (char[])value;
						int offset2 = oracleParameter.Offset;
						int size2 = oracleParameter.Size;
						int num2 = array5.Length - offset2;
						if (size2 != 0 && size2 < num2)
						{
							num2 = size2;
						}
						char[] array6 = new char[Math.Max(num2, 0)];
						Buffer.BlockCopy(array5, offset2, array6, 0, array6.Length * 2);
						oracleParameter.Offset = 0;
						oracleParameter.Value = array6;
					}
					else if (value is ICloneable)
					{
						oracleParameter.Value = ((ICloneable)value).Clone();
					}
					array[i] = oracleParameter;
				}
			}
			string statementText = command.StatementText;
			bool flag = false;
			OracleCommandSet.LocalParameter[] array7 = this.ParseText(command, statementText, out flag);
			OracleCommandSet.LocalCommand localCommand = new OracleCommandSet.LocalCommand(statementText, flag, array, array2, array7);
			this._dirty = true;
			this.CommandList.Add(localCommand);
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0006024C File Offset: 0x0005F64C
		public void Clear()
		{
			DbCommand batchCommand = this.BatchCommand;
			if (batchCommand != null)
			{
				batchCommand.Parameters.Clear();
				batchCommand.CommandText = null;
			}
			ArrayList commandList = this._commandList;
			if (commandList != null)
			{
				commandList.Clear();
			}
			Hashtable usedParameterNames = this._usedParameterNames;
			if (usedParameterNames != null)
			{
				usedParameterNames.Clear();
			}
		}

		// Token: 0x06000309 RID: 777 RVA: 0x00060298 File Offset: 0x0005F698
		public void Dispose()
		{
			DbCommand batchCommand = this._batchCommand;
			this._batchCommand = null;
			this._commandList = null;
			this._usedParameterNames = null;
			if (batchCommand != null)
			{
				batchCommand.Dispose();
			}
		}

		// Token: 0x0600030A RID: 778 RVA: 0x000602CC File Offset: 0x0005F6CC
		public int ExecuteNonQuery()
		{
			this.GenerateBatchCommandText();
			return this.BatchCommand.ExecuteNonQuery();
		}

		// Token: 0x0600030B RID: 779 RVA: 0x000602EC File Offset: 0x0005F6EC
		private void GenerateBatchCommandText()
		{
			if (this._dirty)
			{
				DbCommand batchCommand = this.BatchCommand;
				StringBuilder stringBuilder = new StringBuilder();
				StringBuilder stringBuilder2 = new StringBuilder();
				int num = 1;
				int num2 = 1;
				int num3 = 1;
				batchCommand.Parameters.Clear();
				stringBuilder.Append(OracleCommandSet.Declarations_Prefix);
				stringBuilder2.Append(OracleCommandSet.Body_Prefix);
				foreach (object obj in this.CommandList)
				{
					OracleCommandSet.LocalCommand localCommand = (OracleCommandSet.LocalCommand)obj;
					foreach (DbParameter dbParameter in localCommand.Parameters)
					{
						string text;
						do
						{
							text = "p" + num3.ToString(CultureInfo.InvariantCulture);
							num3++;
						}
						while (this._usedParameterNames.ContainsKey(text));
						dbParameter.ParameterName = text;
						batchCommand.Parameters.Add(dbParameter);
					}
					string text2;
					do
					{
						text2 = "r" + num.ToString(CultureInfo.InvariantCulture) + "_" + num3.ToString(CultureInfo.InvariantCulture);
						num3++;
					}
					while (this._usedParameterNames.ContainsKey(text2));
					OracleParameter oracleParameter = new OracleParameter();
					oracleParameter.CommandSetResult = num++;
					oracleParameter.Direction = ParameterDirection.Output;
					oracleParameter.ParameterName = text2;
					batchCommand.Parameters.Add(oracleParameter);
					int num4 = stringBuilder2.Length;
					if (localCommand.IsQuery)
					{
						string text3 = "c" + num2.ToString(CultureInfo.InvariantCulture);
						num2++;
						stringBuilder.Append(text3);
						stringBuilder.Append(OracleCommandSet.Declarations_CursorType);
						stringBuilder2.Append(OracleCommandSet.Command_QueryPrefix_Part1);
						stringBuilder2.Append(text3);
						stringBuilder2.Append(OracleCommandSet.Command_QueryPrefix_Part2);
						num4 = stringBuilder2.Length;
						stringBuilder2.Append(localCommand.CommandText);
						stringBuilder2.Append(OracleCommandSet.Command_Suffix_Part1);
						stringBuilder2.Append(text2);
						stringBuilder2.Append(OracleCommandSet.Command_QuerySuffix_Part2);
						stringBuilder2.Append(text3);
						stringBuilder2.Append(OracleCommandSet.Command_QuerySuffix_Part3);
						oracleParameter.OracleType = OracleType.Cursor;
					}
					else
					{
						string commandText = localCommand.CommandText;
						stringBuilder2.Append(commandText.TrimEnd(new char[] { ';' }));
						stringBuilder2.Append(OracleCommandSet.Command_Suffix_Part1);
						stringBuilder2.Append(text2);
						stringBuilder2.Append(OracleCommandSet.Command_NonQuerySuffix_Part2);
						oracleParameter.OracleType = OracleType.Int32;
						localCommand.ResultParameter = oracleParameter;
					}
					foreach (OracleCommandSet.LocalParameter localParameter in localCommand.ParameterInsertionPoints)
					{
						DbParameter dbParameter2 = localCommand.Parameters[localParameter.ParameterIndex];
						string text4 = ":" + dbParameter2.ParameterName;
						stringBuilder2.Remove(num4 + localParameter.InsertionPoint, localParameter.RemovalLength);
						stringBuilder2.Insert(num4 + localParameter.InsertionPoint, text4);
						num4 += text4.Length - localParameter.RemovalLength;
					}
				}
				stringBuilder2.Append(OracleCommandSet.Body_Suffix);
				stringBuilder.Append(stringBuilder2);
				batchCommand.CommandText = stringBuilder.ToString();
				this._dirty = false;
			}
		}

		// Token: 0x0600030C RID: 780 RVA: 0x00060644 File Offset: 0x0005FA44
		internal bool GetBatchedRecordsAffected(int commandIndex, out int recordsAffected)
		{
			OracleParameter resultParameter = ((OracleCommandSet.LocalCommand)this.CommandList[commandIndex]).ResultParameter;
			if (resultParameter == null)
			{
				recordsAffected = -1;
				return true;
			}
			if (resultParameter.Value is int)
			{
				recordsAffected = (int)resultParameter.Value;
				return true;
			}
			recordsAffected = -1;
			return false;
		}

		// Token: 0x0600030D RID: 781 RVA: 0x00060690 File Offset: 0x0005FA90
		internal DbParameter GetParameter(int commandIndex, int parameterIndex)
		{
			return ((OracleCommandSet.LocalCommand)this.CommandList[commandIndex]).Parameters[parameterIndex];
		}

		// Token: 0x0600030E RID: 782 RVA: 0x000606B8 File Offset: 0x0005FAB8
		public int GetParameterCount(int commandIndex)
		{
			return ((OracleCommandSet.LocalCommand)this.CommandList[commandIndex]).Parameters.Length;
		}

		// Token: 0x0600030F RID: 783 RVA: 0x000606E0 File Offset: 0x0005FAE0
		private static Regex GetSqlTokenParser()
		{
			Regex regex = OracleCommandSet._sqlTokenParser;
			if (regex == null)
			{
				regex = new Regex(OracleCommandSet._sqlTokenPattern, RegexOptions.ExplicitCapture);
				OracleCommandSet._commentGroup = regex.GroupNumberFromName("comment");
				OracleCommandSet._identifierGroup = regex.GroupNumberFromName("identifier");
				OracleCommandSet._parameterMarkerGroup = regex.GroupNumberFromName("parametermarker");
				OracleCommandSet._queryGroup = regex.GroupNumberFromName("query");
				OracleCommandSet._stringGroup = regex.GroupNumberFromName("string");
				OracleCommandSet._otherGroup = regex.GroupNumberFromName("other");
				OracleCommandSet._sqlTokenParser = regex;
			}
			return regex;
		}

		// Token: 0x06000310 RID: 784 RVA: 0x0006076C File Offset: 0x0005FB6C
		private OracleCommandSet.LocalParameter[] ParseText(OracleCommand command, string commandText, out bool isQuery)
		{
			OracleParameterCollection parameters = command.Parameters;
			ArrayList arrayList = new ArrayList();
			Regex sqlTokenParser = OracleCommandSet.GetSqlTokenParser();
			isQuery = false;
			bool flag = false;
			Match match = sqlTokenParser.Match(commandText);
			while (Match.Empty != match)
			{
				if (!match.Groups[OracleCommandSet._commentGroup].Success)
				{
					if (match.Groups[OracleCommandSet._identifierGroup].Success || match.Groups[OracleCommandSet._stringGroup].Success || match.Groups[OracleCommandSet._otherGroup].Success)
					{
						flag = true;
					}
					else if (match.Groups[OracleCommandSet._queryGroup].Success)
					{
						if (!flag)
						{
							isQuery = true;
						}
					}
					else if (match.Groups[OracleCommandSet._parameterMarkerGroup].Success)
					{
						string value = match.Groups[OracleCommandSet._parameterMarkerGroup].Value;
						string text = value.Substring(1);
						this._usedParameterNames[text] = null;
						int num = parameters.IndexOf(text);
						if (0 > num)
						{
							string text2 = ":" + text;
							num = parameters.IndexOf(text2);
						}
						if (0 <= num)
						{
							arrayList.Add(new OracleCommandSet.LocalParameter(num, match.Index, match.Length));
						}
					}
				}
				match = match.NextMatch();
			}
			OracleCommandSet.LocalParameter[] array = new OracleCommandSet.LocalParameter[arrayList.Count];
			arrayList.CopyTo(array, 0);
			return array;
		}

		// Token: 0x04000367 RID: 871
		private static readonly string _sqlTokenPattern = "[\\s]+|(?<string>'([^']|'')*')|(?<comment>(/\\*([^\\*]|\\*[^/])*\\*/)|(--.*))|(?<parametermarker>:[\\p{Lo}\\p{Lu}\\p{Ll}\\p{Lm}\\p{Nd}\\uff3f_#$]+)|(?<query>select)|(?<identifier>([\\p{Lo}\\p{Lu}\\p{Ll}\\p{Lm}\\p{Nd}\\uff3f_#$]+)|(\"([^\"]|\"\")*\"))|(?<other>.)";

		// Token: 0x04000368 RID: 872
		private static Regex _sqlTokenParser;

		// Token: 0x04000369 RID: 873
		private static int _commentGroup;

		// Token: 0x0400036A RID: 874
		private static int _identifierGroup;

		// Token: 0x0400036B RID: 875
		private static int _parameterMarkerGroup;

		// Token: 0x0400036C RID: 876
		private static int _queryGroup;

		// Token: 0x0400036D RID: 877
		private static int _stringGroup;

		// Token: 0x0400036E RID: 878
		private static int _otherGroup;

		// Token: 0x0400036F RID: 879
		private static readonly string Declarations_Prefix = "declare\ntype refcursortype is ref cursor;\n";

		// Token: 0x04000370 RID: 880
		private static readonly string Declarations_CursorType = " refcursortype;\n";

		// Token: 0x04000371 RID: 881
		private static readonly string Body_Prefix = "begin\n";

		// Token: 0x04000372 RID: 882
		private static readonly string Body_Suffix = "end;";

		// Token: 0x04000373 RID: 883
		private static readonly string Command_QueryPrefix_Part1 = "open ";

		// Token: 0x04000374 RID: 884
		private static readonly string Command_QueryPrefix_Part2 = " for ";

		// Token: 0x04000375 RID: 885
		private static readonly string Command_Suffix_Part1 = ";\n:";

		// Token: 0x04000376 RID: 886
		private static readonly string Command_NonQuerySuffix_Part2 = " := sql%rowcount;\n";

		// Token: 0x04000377 RID: 887
		private static readonly string Command_QuerySuffix_Part2 = " := ";

		// Token: 0x04000378 RID: 888
		private static readonly string Command_QuerySuffix_Part3 = ";\n";

		// Token: 0x04000379 RID: 889
		private Hashtable _usedParameterNames = new Hashtable(StringComparer.OrdinalIgnoreCase);

		// Token: 0x0400037A RID: 890
		private ArrayList _commandList = new ArrayList();

		// Token: 0x0400037B RID: 891
		private OracleCommand _batchCommand;

		// Token: 0x0400037C RID: 892
		private bool _dirty;

		// Token: 0x02000051 RID: 81
		private sealed class LocalCommand
		{
			// Token: 0x06000312 RID: 786 RVA: 0x00060960 File Offset: 0x0005FD60
			internal LocalCommand(string commandText, bool isQuery, DbParameter[] parameters, string[] parameterNames, OracleCommandSet.LocalParameter[] parameterInsertionPoints)
			{
				this.CommandText = commandText;
				this.IsQuery = isQuery;
				this.Parameters = parameters;
				this.ParameterNames = parameterNames;
				this.ParameterInsertionPoints = parameterInsertionPoints;
			}

			// Token: 0x0400037D RID: 893
			internal readonly bool IsQuery;

			// Token: 0x0400037E RID: 894
			internal readonly string CommandText;

			// Token: 0x0400037F RID: 895
			internal readonly DbParameter[] Parameters;

			// Token: 0x04000380 RID: 896
			internal readonly string[] ParameterNames;

			// Token: 0x04000381 RID: 897
			internal readonly OracleCommandSet.LocalParameter[] ParameterInsertionPoints;

			// Token: 0x04000382 RID: 898
			internal OracleParameter ResultParameter;
		}

		// Token: 0x02000052 RID: 82
		private struct LocalParameter
		{
			// Token: 0x06000313 RID: 787 RVA: 0x00060998 File Offset: 0x0005FD98
			internal LocalParameter(int parameterIndex, int insertionPoint, int removalLength)
			{
				this.ParameterIndex = parameterIndex;
				this.InsertionPoint = insertionPoint;
				this.RemovalLength = removalLength;
			}

			// Token: 0x04000383 RID: 899
			internal readonly int ParameterIndex;

			// Token: 0x04000384 RID: 900
			internal readonly int InsertionPoint;

			// Token: 0x04000385 RID: 901
			internal readonly int RemovalLength;
		}
	}
}
