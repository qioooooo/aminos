using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Security.Permissions;

namespace System.Web.UI.Design
{
	// Token: 0x020003A3 RID: 931
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public sealed class TypeSchema : IDataSourceSchema
	{
		// Token: 0x0600225B RID: 8795 RVA: 0x000BBE2C File Offset: 0x000BAE2C
		public TypeSchema(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			this._type = type;
			if (typeof(DataTable).IsAssignableFrom(this._type))
			{
				this._schema = TypeSchema.GetDataTableSchema(this._type);
				return;
			}
			if (typeof(DataSet).IsAssignableFrom(this._type))
			{
				this._schema = TypeSchema.GetDataSetSchema(this._type);
				return;
			}
			if (TypeSchema.IsBoundGenericEnumerable(this._type))
			{
				this._schema = TypeSchema.GetGenericEnumerableSchema(this._type);
				return;
			}
			if (typeof(IEnumerable).IsAssignableFrom(this._type))
			{
				this._schema = TypeSchema.GetEnumerableSchema(this._type);
				return;
			}
			this._schema = TypeSchema.GetTypeSchema(this._type);
		}

		// Token: 0x0600225C RID: 8796 RVA: 0x000BBEFF File Offset: 0x000BAEFF
		public IDataSourceViewSchema[] GetViews()
		{
			return this._schema;
		}

		// Token: 0x0600225D RID: 8797 RVA: 0x000BBF08 File Offset: 0x000BAF08
		private static IDataSourceViewSchema[] GetDataSetSchema(Type t)
		{
			IDataSourceViewSchema[] array;
			try
			{
				DataSet dataSet = Activator.CreateInstance(t) as DataSet;
				List<IDataSourceViewSchema> list = new List<IDataSourceViewSchema>();
				foreach (object obj in dataSet.Tables)
				{
					DataTable dataTable = (DataTable)obj;
					list.Add(new DataSetViewSchema(dataTable));
				}
				array = list.ToArray();
			}
			catch
			{
				array = null;
			}
			return array;
		}

		// Token: 0x0600225E RID: 8798 RVA: 0x000BBF9C File Offset: 0x000BAF9C
		private static IDataSourceViewSchema[] GetDataTableSchema(Type t)
		{
			IDataSourceViewSchema[] array;
			try
			{
				DataTable dataTable = Activator.CreateInstance(t) as DataTable;
				DataSetViewSchema dataSetViewSchema = new DataSetViewSchema(dataTable);
				array = new IDataSourceViewSchema[] { dataSetViewSchema };
			}
			catch
			{
				array = null;
			}
			return array;
		}

		// Token: 0x0600225F RID: 8799 RVA: 0x000BBFE4 File Offset: 0x000BAFE4
		private static IDataSourceViewSchema[] GetEnumerableSchema(Type t)
		{
			TypeEnumerableViewSchema typeEnumerableViewSchema = new TypeEnumerableViewSchema(string.Empty, t);
			return new IDataSourceViewSchema[] { typeEnumerableViewSchema };
		}

		// Token: 0x06002260 RID: 8800 RVA: 0x000BC00C File Offset: 0x000BB00C
		private static IDataSourceViewSchema[] GetGenericEnumerableSchema(Type t)
		{
			TypeGenericEnumerableViewSchema typeGenericEnumerableViewSchema = new TypeGenericEnumerableViewSchema(string.Empty, t);
			return new IDataSourceViewSchema[] { typeGenericEnumerableViewSchema };
		}

		// Token: 0x06002261 RID: 8801 RVA: 0x000BC034 File Offset: 0x000BB034
		private static IDataSourceViewSchema[] GetTypeSchema(Type t)
		{
			TypeViewSchema typeViewSchema = new TypeViewSchema(string.Empty, t);
			return new IDataSourceViewSchema[] { typeViewSchema };
		}

		// Token: 0x06002262 RID: 8802 RVA: 0x000BC05C File Offset: 0x000BB05C
		internal static bool IsBoundGenericEnumerable(Type t)
		{
			Type[] array;
			if (t.IsInterface && t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>))
			{
				array = new Type[] { t };
			}
			else
			{
				array = t.GetInterfaces();
			}
			foreach (Type type in array)
			{
				if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
				{
					Type[] genericArguments = type.GetGenericArguments();
					return !genericArguments[0].IsGenericParameter;
				}
			}
			return false;
		}

		// Token: 0x0400184F RID: 6223
		private Type _type;

		// Token: 0x04001850 RID: 6224
		private IDataSourceViewSchema[] _schema;
	}
}
