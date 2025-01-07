using System;

namespace Microsoft.VisualBasic.CompilerServices
{
	internal class OperatorCaches
	{
		private OperatorCaches()
		{
		}

		internal static readonly OperatorCaches.FixedList ConversionCache = new OperatorCaches.FixedList();

		internal static readonly OperatorCaches.FixedExistanceList UnconvertibleTypeCache = new OperatorCaches.FixedExistanceList();

		internal sealed class FixedList
		{
			internal FixedList()
				: this(50)
			{
			}

			internal FixedList(int Size)
			{
				this.m_Size = Size;
				checked
				{
					this.m_List = new OperatorCaches.FixedList.Entry[this.m_Size - 1 + 1];
					int num = 0;
					int num2 = this.m_Size - 2;
					for (int i = num; i <= num2; i++)
					{
						this.m_List[i].Next = i + 1;
					}
					for (int j = this.m_Size - 1; j >= 1; j += -1)
					{
						this.m_List[j].Previous = j - 1;
					}
					this.m_List[0].Previous = this.m_Size - 1;
					this.m_Last = this.m_Size - 1;
				}
			}

			private void MoveToFront(int Item)
			{
				if (Item == this.m_First)
				{
					return;
				}
				int next = this.m_List[Item].Next;
				int previous = this.m_List[Item].Previous;
				this.m_List[previous].Next = next;
				this.m_List[next].Previous = previous;
				this.m_List[this.m_First].Previous = Item;
				this.m_List[this.m_Last].Next = Item;
				this.m_List[Item].Next = this.m_First;
				this.m_List[Item].Previous = this.m_Last;
				this.m_First = Item;
			}

			internal void Insert(Type TargetType, Type SourceType, ConversionResolution.ConversionClass Classification, Symbols.Method OperatorMethod)
			{
				checked
				{
					if (this.m_Count < this.m_Size)
					{
						this.m_Count++;
					}
					int last = this.m_Last;
					this.m_First = last;
					this.m_Last = this.m_List[this.m_Last].Previous;
					this.m_List[last].TargetType = TargetType;
					this.m_List[last].SourceType = SourceType;
					this.m_List[last].Classification = Classification;
					this.m_List[last].OperatorMethod = OperatorMethod;
				}
			}

			internal bool Lookup(Type TargetType, Type SourceType, ref ConversionResolution.ConversionClass Classification, ref Symbols.Method OperatorMethod)
			{
				int num = this.m_First;
				checked
				{
					for (int i = 0; i < this.m_Count; i++)
					{
						if (TargetType == this.m_List[num].TargetType && SourceType == this.m_List[num].SourceType)
						{
							Classification = this.m_List[num].Classification;
							OperatorMethod = this.m_List[num].OperatorMethod;
							this.MoveToFront(num);
							return true;
						}
						num = this.m_List[num].Next;
					}
					Classification = ConversionResolution.ConversionClass.Bad;
					OperatorMethod = null;
					return false;
				}
			}

			private readonly OperatorCaches.FixedList.Entry[] m_List;

			private readonly int m_Size;

			private int m_First;

			private int m_Last;

			private int m_Count;

			private const int DefaultSize = 50;

			private struct Entry
			{
				internal Type TargetType;

				internal Type SourceType;

				internal ConversionResolution.ConversionClass Classification;

				internal Symbols.Method OperatorMethod;

				internal int Next;

				internal int Previous;
			}
		}

		internal sealed class FixedExistanceList
		{
			internal FixedExistanceList()
				: this(50)
			{
			}

			internal FixedExistanceList(int Size)
			{
				this.m_Size = Size;
				checked
				{
					this.m_List = new OperatorCaches.FixedExistanceList.Entry[this.m_Size - 1 + 1];
					int num = 0;
					int num2 = this.m_Size - 2;
					for (int i = num; i <= num2; i++)
					{
						this.m_List[i].Next = i + 1;
					}
					for (int j = this.m_Size - 1; j >= 1; j += -1)
					{
						this.m_List[j].Previous = j - 1;
					}
					this.m_List[0].Previous = this.m_Size - 1;
					this.m_Last = this.m_Size - 1;
				}
			}

			private void MoveToFront(int Item)
			{
				if (Item == this.m_First)
				{
					return;
				}
				int next = this.m_List[Item].Next;
				int previous = this.m_List[Item].Previous;
				this.m_List[previous].Next = next;
				this.m_List[next].Previous = previous;
				this.m_List[this.m_First].Previous = Item;
				this.m_List[this.m_Last].Next = Item;
				this.m_List[Item].Next = this.m_First;
				this.m_List[Item].Previous = this.m_Last;
				this.m_First = Item;
			}

			internal void Insert(Type Type)
			{
				checked
				{
					if (this.m_Count < this.m_Size)
					{
						this.m_Count++;
					}
					int last = this.m_Last;
					this.m_First = last;
					this.m_Last = this.m_List[this.m_Last].Previous;
					this.m_List[last].Type = Type;
				}
			}

			internal bool Lookup(Type Type)
			{
				int num = this.m_First;
				checked
				{
					for (int i = 0; i < this.m_Count; i++)
					{
						if (Type == this.m_List[num].Type)
						{
							this.MoveToFront(num);
							return true;
						}
						num = this.m_List[num].Next;
					}
					return false;
				}
			}

			private readonly OperatorCaches.FixedExistanceList.Entry[] m_List;

			private readonly int m_Size;

			private int m_First;

			private int m_Last;

			private int m_Count;

			private const int DefaultSize = 50;

			private struct Entry
			{
				internal Type Type;

				internal int Next;

				internal int Previous;
			}
		}
	}
}
