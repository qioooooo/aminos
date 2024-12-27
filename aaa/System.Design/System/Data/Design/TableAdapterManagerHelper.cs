using System;
using System.Collections.Generic;

namespace System.Data.Design
{
	// Token: 0x020000BE RID: 190
	internal class TableAdapterManagerHelper
	{
		// Token: 0x06000860 RID: 2144 RVA: 0x0001576C File Offset: 0x0001476C
		internal static DataRelation[] GetSelfRefRelations(DataTable dataTable)
		{
			List<DataRelation> list = new List<DataRelation>();
			List<DataRelation> list2 = new List<DataRelation>();
			foreach (object obj in dataTable.ParentRelations)
			{
				DataRelation dataRelation = (DataRelation)obj;
				if (dataRelation.ChildTable == dataRelation.ParentTable)
				{
					list.Add(dataRelation);
					if (dataRelation.ChildKeyConstraint != null)
					{
						list2.Add(dataRelation);
					}
				}
			}
			if (list2.Count > 0)
			{
				return list2.ToArray();
			}
			return list.ToArray();
		}

		// Token: 0x06000861 RID: 2145 RVA: 0x00015808 File Offset: 0x00014808
		internal static DataTable[] GetUpdateOrder(DataSet ds)
		{
			TableAdapterManagerHelper.HierarchicalObject[] array = new TableAdapterManagerHelper.HierarchicalObject[ds.Tables.Count];
			for (int i = 0; i < ds.Tables.Count; i++)
			{
				DataTable dataTable = ds.Tables[i];
				TableAdapterManagerHelper.HierarchicalObject hierarchicalObject = new TableAdapterManagerHelper.HierarchicalObject(dataTable);
				array[i] = hierarchicalObject;
			}
			for (int j = 0; j < array.Length; j++)
			{
				DataTable dataTable2 = array[j].TheObject as DataTable;
				foreach (object obj in dataTable2.Constraints)
				{
					Constraint constraint = (Constraint)obj;
					ForeignKeyConstraint foreignKeyConstraint = constraint as ForeignKeyConstraint;
					if (foreignKeyConstraint != null && !object.ReferenceEquals(foreignKeyConstraint.RelatedTable, dataTable2))
					{
						int num = ds.Tables.IndexOf(foreignKeyConstraint.RelatedTable);
						array[j].AddUniqueParent(array[num]);
					}
				}
				foreach (object obj2 in dataTable2.ParentRelations)
				{
					DataRelation dataRelation = (DataRelation)obj2;
					if (!object.ReferenceEquals(dataRelation.ParentTable, dataTable2))
					{
						int num2 = ds.Tables.IndexOf(dataRelation.ParentTable);
						array[j].AddUniqueParent(array[num2]);
					}
				}
			}
			foreach (TableAdapterManagerHelper.HierarchicalObject hierarchicalObject2 in array)
			{
				if (hierarchicalObject2.HasParent)
				{
					hierarchicalObject2.CheckParents();
				}
			}
			DataTable[] array2 = new DataTable[array.Length];
			Array.Sort<TableAdapterManagerHelper.HierarchicalObject>(array);
			for (int l = 0; l < array.Length; l++)
			{
				TableAdapterManagerHelper.HierarchicalObject hierarchicalObject3 = array[l];
				array2[l] = (DataTable)hierarchicalObject3.TheObject;
			}
			return array2;
		}

		// Token: 0x020000BF RID: 191
		internal class HierarchicalObject : IComparable<TableAdapterManagerHelper.HierarchicalObject>
		{
			// Token: 0x17000129 RID: 297
			// (get) Token: 0x06000863 RID: 2147 RVA: 0x000159F0 File Offset: 0x000149F0
			internal List<TableAdapterManagerHelper.HierarchicalObject> Parents
			{
				get
				{
					if (this.parents == null)
					{
						this.parents = new List<TableAdapterManagerHelper.HierarchicalObject>();
					}
					return this.parents;
				}
			}

			// Token: 0x1700012A RID: 298
			// (get) Token: 0x06000864 RID: 2148 RVA: 0x00015A0B File Offset: 0x00014A0B
			internal bool HasParent
			{
				get
				{
					return this.parents != null && this.parents.Count > 0;
				}
			}

			// Token: 0x06000865 RID: 2149 RVA: 0x00015A25 File Offset: 0x00014A25
			internal HierarchicalObject(object theObject)
			{
				this.TheObject = theObject;
			}

			// Token: 0x06000866 RID: 2150 RVA: 0x00015A34 File Offset: 0x00014A34
			internal void AddUniqueParent(TableAdapterManagerHelper.HierarchicalObject parent)
			{
				if (!this.Parents.Contains(parent))
				{
					this.Parents.Add(parent);
				}
			}

			// Token: 0x06000867 RID: 2151 RVA: 0x00015A50 File Offset: 0x00014A50
			internal void CheckParents()
			{
				if (this.HasParent)
				{
					Stack<TableAdapterManagerHelper.HierarchicalObject> stack = new Stack<TableAdapterManagerHelper.HierarchicalObject>();
					Stack<TableAdapterManagerHelper.HierarchicalObject> stack2 = new Stack<TableAdapterManagerHelper.HierarchicalObject>();
					stack2.Push(this);
					stack.Push(this);
					this.CheckParents(stack2, stack);
				}
			}

			// Token: 0x06000868 RID: 2152 RVA: 0x00015A88 File Offset: 0x00014A88
			internal void CheckParents(Stack<TableAdapterManagerHelper.HierarchicalObject> work, Stack<TableAdapterManagerHelper.HierarchicalObject> path)
			{
				if (!this.HasParent || (!object.ReferenceEquals(this, path.Peek()) && path.Contains(this)))
				{
					TableAdapterManagerHelper.HierarchicalObject hierarchicalObject = path.Pop();
					TableAdapterManagerHelper.HierarchicalObject hierarchicalObject2 = work.Pop();
					while (work.Count > 0 && path.Count > 0 && object.ReferenceEquals(hierarchicalObject, hierarchicalObject2))
					{
						hierarchicalObject = path.Pop();
						hierarchicalObject2 = work.Pop();
					}
					if (hierarchicalObject2 != hierarchicalObject)
					{
						path.Push(hierarchicalObject2);
						hierarchicalObject2.CheckParents(work, path);
					}
					return;
				}
				if (this.HasParent)
				{
					TableAdapterManagerHelper.HierarchicalObject hierarchicalObject3 = null;
					for (int i = this.Parents.Count - 1; i >= 0; i--)
					{
						TableAdapterManagerHelper.HierarchicalObject hierarchicalObject4 = this.Parents[i];
						if (!path.Contains(hierarchicalObject4) && hierarchicalObject4.Height <= this.Height)
						{
							hierarchicalObject4.Height = this.Height + 1;
							if (hierarchicalObject4.Height > 1000)
							{
								return;
							}
							work.Push(hierarchicalObject4);
							hierarchicalObject3 = hierarchicalObject4;
						}
					}
					if (hierarchicalObject3 != null)
					{
						path.Push(hierarchicalObject3);
						hierarchicalObject3.CheckParents(work, path);
					}
				}
			}

			// Token: 0x06000869 RID: 2153 RVA: 0x00015B85 File Offset: 0x00014B85
			int IComparable<TableAdapterManagerHelper.HierarchicalObject>.CompareTo(TableAdapterManagerHelper.HierarchicalObject other)
			{
				return other.Height - this.Height;
			}

			// Token: 0x04000C17 RID: 3095
			internal int Height;

			// Token: 0x04000C18 RID: 3096
			internal object TheObject;

			// Token: 0x04000C19 RID: 3097
			private List<TableAdapterManagerHelper.HierarchicalObject> parents;
		}
	}
}
