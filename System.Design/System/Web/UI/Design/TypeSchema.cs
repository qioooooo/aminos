using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Security.Permissions;

namespace System.Web.UI.Design
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public sealed class TypeSchema : IDataSourceSchema
	{
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

		public IDataSourceViewSchema[] GetViews()
		{
			return this._schema;
		}

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

		private static IDataSourceViewSchema[] GetEnumerableSchema(Type t)
		{
			TypeEnumerableViewSchema typeEnumerableViewSchema = new TypeEnumerableViewSchema(string.Empty, t);
			return new IDataSourceViewSchema[] { typeEnumerableViewSchema };
		}

		private static IDataSourceViewSchema[] GetGenericEnumerableSchema(Type t)
		{
			TypeGenericEnumerableViewSchema typeGenericEnumerableViewSchema = new TypeGenericEnumerableViewSchema(string.Empty, t);
			return new IDataSourceViewSchema[] { typeGenericEnumerableViewSchema };
		}

		private static IDataSourceViewSchema[] GetTypeSchema(Type t)
		{
			TypeViewSchema typeViewSchema = new TypeViewSchema(string.Empty, t);
			return new IDataSourceViewSchema[] { typeViewSchema };
		}

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

		private Type _type;

		private IDataSourceViewSchema[] _schema;
	}
}
