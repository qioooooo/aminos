using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace System.Data.SqlClient
{
	// Token: 0x020002C9 RID: 713
	internal sealed class SqlCommandSet
	{
		// Token: 0x06002476 RID: 9334 RVA: 0x00277A90 File Offset: 0x00276E90
		internal SqlCommandSet()
		{
			this._batchCommand = new SqlCommand();
		}

		// Token: 0x17000580 RID: 1408
		// (get) Token: 0x06002477 RID: 9335 RVA: 0x00277ACC File Offset: 0x00276ECC
		private SqlCommand BatchCommand
		{
			get
			{
				SqlCommand batchCommand = this._batchCommand;
				if (batchCommand == null)
				{
					throw ADP.ObjectDisposed(this);
				}
				return batchCommand;
			}
		}

		// Token: 0x17000581 RID: 1409
		// (get) Token: 0x06002478 RID: 9336 RVA: 0x00277AEC File Offset: 0x00276EEC
		internal int CommandCount
		{
			get
			{
				return this.CommandList.Count;
			}
		}

		// Token: 0x17000582 RID: 1410
		// (get) Token: 0x06002479 RID: 9337 RVA: 0x00277B04 File Offset: 0x00276F04
		private List<SqlCommandSet.LocalCommand> CommandList
		{
			get
			{
				List<SqlCommandSet.LocalCommand> commandList = this._commandList;
				if (commandList == null)
				{
					throw ADP.ObjectDisposed(this);
				}
				return commandList;
			}
		}

		// Token: 0x17000583 RID: 1411
		// (set) Token: 0x0600247A RID: 9338 RVA: 0x00277B24 File Offset: 0x00276F24
		internal int CommandTimeout
		{
			set
			{
				this.BatchCommand.CommandTimeout = value;
			}
		}

		// Token: 0x17000584 RID: 1412
		// (get) Token: 0x0600247B RID: 9339 RVA: 0x00277B40 File Offset: 0x00276F40
		// (set) Token: 0x0600247C RID: 9340 RVA: 0x00277B58 File Offset: 0x00276F58
		internal SqlConnection Connection
		{
			get
			{
				return this.BatchCommand.Connection;
			}
			set
			{
				this.BatchCommand.Connection = value;
			}
		}

		// Token: 0x17000585 RID: 1413
		// (set) Token: 0x0600247D RID: 9341 RVA: 0x00277B74 File Offset: 0x00276F74
		internal SqlTransaction Transaction
		{
			set
			{
				this.BatchCommand.Transaction = value;
			}
		}

		// Token: 0x17000586 RID: 1414
		// (get) Token: 0x0600247E RID: 9342 RVA: 0x00277B90 File Offset: 0x00276F90
		internal int ObjectID
		{
			get
			{
				return this._objectID;
			}
		}

		// Token: 0x0600247F RID: 9343 RVA: 0x00277BA4 File Offset: 0x00276FA4
		internal void Append(SqlCommand command)
		{
			ADP.CheckArgumentNull(command, "command");
			Bid.Trace("<sc.SqlCommandSet.Append|API> %d#, command=%d, parameterCount=%d\n", this.ObjectID, command.ObjectID, command.Parameters.Count);
			string commandText = command.CommandText;
			if (ADP.IsEmpty(commandText))
			{
				throw ADP.CommandTextRequired("Append");
			}
			CommandType commandType = command.CommandType;
			CommandType commandType2 = commandType;
			if (commandType2 == CommandType.Text || commandType2 == CommandType.StoredProcedure)
			{
				SqlParameterCollection sqlParameterCollection = null;
				SqlParameterCollection parameters = command.Parameters;
				if (0 < parameters.Count)
				{
					sqlParameterCollection = new SqlParameterCollection();
					for (int i = 0; i < parameters.Count; i++)
					{
						SqlParameter sqlParameter = new SqlParameter();
						parameters[i].CopyTo(sqlParameter);
						sqlParameterCollection.Add(sqlParameter);
						if (!SqlCommandSet.SqlIdentifierParser.IsMatch(sqlParameter.ParameterName))
						{
							throw ADP.BadParameterName(sqlParameter.ParameterName);
						}
					}
					foreach (object obj in sqlParameterCollection)
					{
						SqlParameter sqlParameter2 = (SqlParameter)obj;
						object value = sqlParameter2.Value;
						byte[] array = value as byte[];
						if (array != null)
						{
							int offset = sqlParameter2.Offset;
							int size = sqlParameter2.Size;
							int num = array.Length - offset;
							if (size != 0 && size < num)
							{
								num = size;
							}
							byte[] array2 = new byte[Math.Max(num, 0)];
							Buffer.BlockCopy(array, offset, array2, 0, array2.Length);
							sqlParameter2.Offset = 0;
							sqlParameter2.Value = array2;
						}
						else
						{
							char[] array3 = value as char[];
							if (array3 != null)
							{
								int offset2 = sqlParameter2.Offset;
								int size2 = sqlParameter2.Size;
								int num2 = array3.Length - offset2;
								if (size2 != 0 && size2 < num2)
								{
									num2 = size2;
								}
								char[] array4 = new char[Math.Max(num2, 0)];
								Buffer.BlockCopy(array3, offset2, array4, 0, array4.Length * 2);
								sqlParameter2.Offset = 0;
								sqlParameter2.Value = array4;
							}
							else
							{
								ICloneable cloneable = value as ICloneable;
								if (cloneable != null)
								{
									sqlParameter2.Value = cloneable.Clone();
								}
							}
						}
					}
				}
				int num3 = -1;
				if (sqlParameterCollection != null)
				{
					for (int j = 0; j < sqlParameterCollection.Count; j++)
					{
						if (ParameterDirection.ReturnValue == sqlParameterCollection[j].Direction)
						{
							num3 = j;
							break;
						}
					}
				}
				SqlCommandSet.LocalCommand localCommand = new SqlCommandSet.LocalCommand(commandText, sqlParameterCollection, num3, command.CommandType);
				this.CommandList.Add(localCommand);
				return;
			}
			if (commandType2 == CommandType.TableDirect)
			{
				throw SQL.NotSupportedCommandType(commandType);
			}
			throw ADP.InvalidCommandType(commandType);
		}

		// Token: 0x06002480 RID: 9344 RVA: 0x00277E28 File Offset: 0x00277228
		internal static void BuildStoredProcedureName(StringBuilder builder, string part)
		{
			if (part != null && 0 < part.Length)
			{
				if ('[' == part[0])
				{
					int num = 0;
					foreach (char c in part)
					{
						if (']' == c)
						{
							num++;
						}
					}
					if (1 == num % 2)
					{
						builder.Append(part);
						return;
					}
				}
				builder.Append("[");
				builder.Append(part.Replace("]", "]]"));
				builder.Append("]");
			}
		}

		// Token: 0x06002481 RID: 9345 RVA: 0x00277EB4 File Offset: 0x002772B4
		internal void Clear()
		{
			Bid.Trace("<sc.SqlCommandSet.Clear|API> %d#", this.ObjectID);
			DbCommand batchCommand = this.BatchCommand;
			if (batchCommand != null)
			{
				batchCommand.Parameters.Clear();
				batchCommand.CommandText = null;
			}
			List<SqlCommandSet.LocalCommand> commandList = this._commandList;
			if (commandList != null)
			{
				commandList.Clear();
			}
		}

		// Token: 0x06002482 RID: 9346 RVA: 0x00277F00 File Offset: 0x00277300
		internal void Dispose()
		{
			Bid.Trace("<sc.SqlCommandSet.Dispose|API> %d#", this.ObjectID);
			SqlCommand batchCommand = this._batchCommand;
			this._commandList = null;
			this._batchCommand = null;
			if (batchCommand != null)
			{
				batchCommand.Dispose();
			}
		}

		// Token: 0x06002483 RID: 9347 RVA: 0x00277F3C File Offset: 0x0027733C
		internal int ExecuteNonQuery()
		{
			SqlConnection.ExecutePermission.Demand();
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<sc.SqlCommandSet.ExecuteNonQuery|API> %d#", this.ObjectID);
			int num;
			try
			{
				if (this.Connection.IsContextConnection)
				{
					throw SQL.BatchedUpdatesNotAvailableOnContextConnection();
				}
				this.ValidateCommandBehavior("ExecuteNonQuery", CommandBehavior.Default);
				this.BatchCommand.BatchRPCMode = true;
				this.BatchCommand.ClearBatchCommand();
				this.BatchCommand.Parameters.Clear();
				for (int i = 0; i < this._commandList.Count; i++)
				{
					SqlCommandSet.LocalCommand localCommand = this._commandList[i];
					this.BatchCommand.AddBatchCommand(localCommand.CommandText, localCommand.Parameters, localCommand.CmdType);
				}
				num = this.BatchCommand.ExecuteBatchRPCCommand();
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return num;
		}

		// Token: 0x06002484 RID: 9348 RVA: 0x00278020 File Offset: 0x00277420
		internal SqlParameter GetParameter(int commandIndex, int parameterIndex)
		{
			return this.CommandList[commandIndex].Parameters[parameterIndex];
		}

		// Token: 0x06002485 RID: 9349 RVA: 0x00278044 File Offset: 0x00277444
		internal bool GetBatchedAffected(int commandIdentifier, out int recordsAffected, out Exception error)
		{
			error = this.BatchCommand.GetErrors(commandIdentifier);
			int? recordsAffected2 = this.BatchCommand.GetRecordsAffected(commandIdentifier);
			recordsAffected = recordsAffected2.GetValueOrDefault();
			return recordsAffected2 != null;
		}

		// Token: 0x06002486 RID: 9350 RVA: 0x0027807C File Offset: 0x0027747C
		internal int GetParameterCount(int commandIndex)
		{
			return this.CommandList[commandIndex].Parameters.Count;
		}

		// Token: 0x06002487 RID: 9351 RVA: 0x002780A0 File Offset: 0x002774A0
		private void ValidateCommandBehavior(string method, CommandBehavior behavior)
		{
			if ((behavior & ~(CommandBehavior.SequentialAccess | CommandBehavior.CloseConnection)) != CommandBehavior.Default)
			{
				ADP.ValidateCommandBehavior(behavior);
				throw ADP.NotSupportedCommandBehavior(behavior & ~(CommandBehavior.SequentialAccess | CommandBehavior.CloseConnection), method);
			}
		}

		// Token: 0x0400173E RID: 5950
		private const string SqlIdentifierPattern = "^@[\\p{Lo}\\p{Lu}\\p{Ll}\\p{Lm}_@#][\\p{Lo}\\p{Lu}\\p{Ll}\\p{Lm}\\p{Nd}\uff3f_@#\\$]*$";

		// Token: 0x0400173F RID: 5951
		private static readonly Regex SqlIdentifierParser = new Regex("^@[\\p{Lo}\\p{Lu}\\p{Ll}\\p{Lm}_@#][\\p{Lo}\\p{Lu}\\p{Ll}\\p{Lm}\\p{Nd}\uff3f_@#\\$]*$", RegexOptions.ExplicitCapture | RegexOptions.Singleline);

		// Token: 0x04001740 RID: 5952
		private List<SqlCommandSet.LocalCommand> _commandList = new List<SqlCommandSet.LocalCommand>();

		// Token: 0x04001741 RID: 5953
		private SqlCommand _batchCommand;

		// Token: 0x04001742 RID: 5954
		private static int _objectTypeCount;

		// Token: 0x04001743 RID: 5955
		internal readonly int _objectID = Interlocked.Increment(ref SqlCommandSet._objectTypeCount);

		// Token: 0x020002CA RID: 714
		private sealed class LocalCommand
		{
			// Token: 0x06002489 RID: 9353 RVA: 0x002780E4 File Offset: 0x002774E4
			internal LocalCommand(string commandText, SqlParameterCollection parameters, int returnParameterIndex, CommandType cmdType)
			{
				this.CommandText = commandText;
				this.Parameters = parameters;
				this.ReturnParameterIndex = returnParameterIndex;
				this.CmdType = cmdType;
			}

			// Token: 0x04001744 RID: 5956
			internal readonly string CommandText;

			// Token: 0x04001745 RID: 5957
			internal readonly SqlParameterCollection Parameters;

			// Token: 0x04001746 RID: 5958
			internal readonly int ReturnParameterIndex;

			// Token: 0x04001747 RID: 5959
			internal readonly CommandType CmdType;
		}
	}
}
