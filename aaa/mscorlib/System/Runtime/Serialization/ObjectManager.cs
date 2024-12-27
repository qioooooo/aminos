using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Reflection.Cache;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Security;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;

namespace System.Runtime.Serialization
{
	// Token: 0x0200034E RID: 846
	[ComVisible(true)]
	public class ObjectManager
	{
		// Token: 0x060021C4 RID: 8644 RVA: 0x000551FD File Offset: 0x000541FD
		public ObjectManager(ISurrogateSelector selector, StreamingContext context)
			: this(selector, context, true, false)
		{
		}

		// Token: 0x060021C5 RID: 8645 RVA: 0x00055209 File Offset: 0x00054209
		internal ObjectManager(ISurrogateSelector selector, StreamingContext context, bool checkSecurity, bool isCrossAppDomain)
		{
			if (checkSecurity)
			{
				CodeAccessPermission.DemandInternal(PermissionType.SecuritySerialization);
			}
			this.m_objects = new ObjectHolder[16];
			this.m_selector = selector;
			this.m_context = context;
			this.m_isCrossAppDomain = isCrossAppDomain;
		}

		// Token: 0x060021C6 RID: 8646 RVA: 0x0005523D File Offset: 0x0005423D
		private bool CanCallGetType(object obj)
		{
			return !RemotingServices.IsTransparentProxy(obj);
		}

		// Token: 0x170005F0 RID: 1520
		// (get) Token: 0x060021C8 RID: 8648 RVA: 0x00055253 File Offset: 0x00054253
		// (set) Token: 0x060021C7 RID: 8647 RVA: 0x0005524A File Offset: 0x0005424A
		internal object TopObject
		{
			get
			{
				return this.m_topObject;
			}
			set
			{
				this.m_topObject = value;
			}
		}

		// Token: 0x170005F1 RID: 1521
		// (get) Token: 0x060021C9 RID: 8649 RVA: 0x0005525B File Offset: 0x0005425B
		internal ObjectHolderList SpecialFixupObjects
		{
			get
			{
				if (this.m_specialFixupObjects == null)
				{
					this.m_specialFixupObjects = new ObjectHolderList();
				}
				return this.m_specialFixupObjects;
			}
		}

		// Token: 0x060021CA RID: 8650 RVA: 0x00055278 File Offset: 0x00054278
		static ObjectManager()
		{
			ObjectManager.SIConstructorTypes[0] = typeof(SerializationInfo);
			ObjectManager.SIConstructorTypes[1] = typeof(StreamingContext);
			ObjectManager.TypeOfWindowsIdentity = typeof(WindowsIdentity);
			ObjectManager.SIWindowsIdentityConstructorTypes = new Type[1];
			ObjectManager.SIWindowsIdentityConstructorTypes[0] = typeof(SerializationInfo);
		}

		// Token: 0x060021CB RID: 8651 RVA: 0x000552E0 File Offset: 0x000542E0
		internal ObjectHolder FindObjectHolder(long objectID)
		{
			int num = (int)(objectID & 4095L);
			if (num >= this.m_objects.Length)
			{
				return null;
			}
			ObjectHolder objectHolder;
			for (objectHolder = this.m_objects[num]; objectHolder != null; objectHolder = objectHolder.m_next)
			{
				if (objectHolder.m_id == objectID)
				{
					return objectHolder;
				}
			}
			return objectHolder;
		}

		// Token: 0x060021CC RID: 8652 RVA: 0x00055328 File Offset: 0x00054328
		internal ObjectHolder FindOrCreateObjectHolder(long objectID)
		{
			ObjectHolder objectHolder = this.FindObjectHolder(objectID);
			if (objectHolder == null)
			{
				objectHolder = new ObjectHolder(objectID);
				this.AddObjectHolder(objectHolder);
			}
			return objectHolder;
		}

		// Token: 0x060021CD RID: 8653 RVA: 0x00055350 File Offset: 0x00054350
		private void AddObjectHolder(ObjectHolder holder)
		{
			if (holder.m_id >= (long)this.m_objects.Length && this.m_objects.Length != 4096)
			{
				int num = 4096;
				if (holder.m_id < 2048L)
				{
					num = this.m_objects.Length * 2;
					while ((long)num <= holder.m_id && num < 4096)
					{
						num *= 2;
					}
					if (num > 4096)
					{
						num = 4096;
					}
				}
				ObjectHolder[] array = new ObjectHolder[num];
				Array.Copy(this.m_objects, array, this.m_objects.Length);
				this.m_objects = array;
			}
			int num2 = (int)(holder.m_id & 4095L);
			ObjectHolder objectHolder = this.m_objects[num2];
			holder.m_next = objectHolder;
			this.m_objects[num2] = holder;
		}

		// Token: 0x060021CE RID: 8654 RVA: 0x0005540C File Offset: 0x0005440C
		private bool GetCompletionInfo(FixupHolder fixup, out ObjectHolder holder, out object member, bool bThrowIfMissing)
		{
			member = fixup.m_fixupInfo;
			holder = this.FindObjectHolder(fixup.m_id);
			if (!holder.CompletelyFixed && holder.ObjectValue != null && holder.ObjectValue is ValueType)
			{
				this.SpecialFixupObjects.Add(holder);
				return false;
			}
			if (holder != null && !holder.CanObjectValueChange && holder.ObjectValue != null)
			{
				return true;
			}
			if (!bThrowIfMissing)
			{
				return false;
			}
			if (holder == null)
			{
				throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_NeverSeen"), new object[] { fixup.m_id }));
			}
			if (holder.IsIncompleteObjectReference)
			{
				throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_IORIncomplete"), new object[] { fixup.m_id }));
			}
			throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_ObjectNotSupplied"), new object[] { fixup.m_id }));
		}

		// Token: 0x060021CF RID: 8655 RVA: 0x00055520 File Offset: 0x00054520
		private void FixupSpecialObject(ObjectHolder holder)
		{
			ISurrogateSelector surrogateSelector = null;
			if (holder.HasSurrogate)
			{
				ISerializationSurrogate surrogate = holder.Surrogate;
				object obj = surrogate.SetObjectData(holder.ObjectValue, holder.SerializationInfo, this.m_context, surrogateSelector);
				if (obj != null)
				{
					if (!holder.CanSurrogatedObjectValueChange && obj != holder.ObjectValue)
					{
						throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_NotCyclicallyReferenceableSurrogate"), new object[] { surrogate.GetType().FullName }));
					}
					holder.SetObjectValue(obj, this);
				}
				holder.m_surrogate = null;
				holder.SetFlags();
			}
			else
			{
				this.CompleteISerializableObject(holder.ObjectValue, holder.SerializationInfo, this.m_context);
			}
			holder.SerializationInfo = null;
			holder.RequiresSerInfoFixup = false;
			if (holder.RequiresValueTypeFixup && holder.ValueTypeFixupPerformed)
			{
				this.DoValueTypeFixup(null, holder, holder.ObjectValue);
			}
			this.DoNewlyRegisteredObjectFixups(holder);
		}

		// Token: 0x060021D0 RID: 8656 RVA: 0x00055600 File Offset: 0x00054600
		private bool ResolveObjectReference(ObjectHolder holder)
		{
			int num = 0;
			try
			{
				object objectValue;
				for (;;)
				{
					objectValue = holder.ObjectValue;
					holder.SetObjectValue(((IObjectReference)holder.ObjectValue).GetRealObject(this.m_context), this);
					if (holder.ObjectValue == null)
					{
						break;
					}
					if (num++ == 100)
					{
						goto Block_3;
					}
					if (!(holder.ObjectValue is IObjectReference) || objectValue == holder.ObjectValue)
					{
						goto IL_0069;
					}
				}
				holder.SetObjectValue(objectValue, this);
				return false;
				Block_3:
				throw new SerializationException(Environment.GetResourceString("Serialization_TooManyReferences"));
				IL_0069:;
			}
			catch (NullReferenceException)
			{
				return false;
			}
			holder.IsIncompleteObjectReference = false;
			this.DoNewlyRegisteredObjectFixups(holder);
			return true;
		}

		// Token: 0x060021D1 RID: 8657 RVA: 0x000556A0 File Offset: 0x000546A0
		private bool DoValueTypeFixup(FieldInfo memberToFix, ObjectHolder holder, object value)
		{
			FieldInfo[] array = new FieldInfo[4];
			int num = 0;
			int[] array2 = null;
			object obj = holder.ObjectValue;
			while (holder.RequiresValueTypeFixup)
			{
				if (num + 1 >= array.Length)
				{
					FieldInfo[] array3 = new FieldInfo[array.Length * 2];
					Array.Copy(array, array3, array.Length);
					array = array3;
				}
				ValueTypeFixupInfo valueFixup = holder.ValueFixup;
				obj = holder.ObjectValue;
				if (valueFixup.ParentField != null)
				{
					FieldInfo parentField = valueFixup.ParentField;
					ObjectHolder objectHolder = this.FindObjectHolder(valueFixup.ContainerID);
					if (objectHolder.ObjectValue == null)
					{
						break;
					}
					if (Nullable.GetUnderlyingType(parentField.FieldType) != null)
					{
						array[num] = parentField.FieldType.GetField("value", BindingFlags.Instance | BindingFlags.NonPublic);
						num++;
					}
					array[num] = parentField;
					holder = objectHolder;
					num++;
				}
				else
				{
					holder = this.FindObjectHolder(valueFixup.ContainerID);
					array2 = valueFixup.ParentIndex;
					if (holder.ObjectValue == null)
					{
						break;
					}
					break;
				}
			}
			if (!(holder.ObjectValue is Array) && holder.ObjectValue != null)
			{
				obj = holder.ObjectValue;
			}
			if (num != 0)
			{
				FieldInfo[] array4 = new FieldInfo[num];
				for (int i = 0; i < num; i++)
				{
					FieldInfo fieldInfo = array[num - 1 - i];
					SerializationFieldInfo serializationFieldInfo = fieldInfo as SerializationFieldInfo;
					array4[i] = ((serializationFieldInfo == null) ? fieldInfo : serializationFieldInfo.FieldInfo);
				}
				TypedReference typedReference = TypedReference.MakeTypedReference(obj, array4);
				if (memberToFix != null)
				{
					((RuntimeFieldInfo)memberToFix).SetValueDirect(typedReference, value);
				}
				else
				{
					TypedReference.SetTypedReference(typedReference, value);
				}
			}
			else if (memberToFix != null)
			{
				FormatterServices.SerializationSetValue(memberToFix, obj, value);
			}
			if (array2 != null && holder.ObjectValue != null)
			{
				((Array)holder.ObjectValue).SetValue(obj, array2);
			}
			return true;
		}

		// Token: 0x060021D2 RID: 8658 RVA: 0x0005583C File Offset: 0x0005483C
		[Conditional("SER_LOGGING")]
		private void DumpValueTypeFixup(object obj, FieldInfo[] intermediateFields, FieldInfo memberToFix, object value)
		{
			StringBuilder stringBuilder = new StringBuilder("  " + obj);
			if (intermediateFields != null)
			{
				for (int i = 0; i < intermediateFields.Length; i++)
				{
					stringBuilder.Append("." + intermediateFields[i].Name);
				}
			}
			stringBuilder.Append(string.Concat(new object[] { ".", memberToFix.Name, "=", value }));
		}

		// Token: 0x060021D3 RID: 8659 RVA: 0x000558B8 File Offset: 0x000548B8
		internal void CompleteObject(ObjectHolder holder, bool bObjectFullyComplete)
		{
			FixupHolderList missingElements = holder.m_missingElements;
			object obj = null;
			ObjectHolder objectHolder = null;
			int num = 0;
			if (holder.ObjectValue == null)
			{
				throw new SerializationException(Environment.GetResourceString("Serialization_MissingObject", new object[] { holder.m_id }));
			}
			if (missingElements == null)
			{
				return;
			}
			if (holder.HasSurrogate || holder.HasISerializable)
			{
				SerializationInfo serInfo = holder.m_serInfo;
				if (serInfo == null)
				{
					throw new SerializationException(Environment.GetResourceString("Serialization_InvalidFixupDiscovered"));
				}
				if (missingElements != null)
				{
					for (int i = 0; i < missingElements.m_count; i++)
					{
						if (missingElements.m_values[i] != null && this.GetCompletionInfo(missingElements.m_values[i], out objectHolder, out obj, bObjectFullyComplete))
						{
							object objectValue = objectHolder.ObjectValue;
							if (this.CanCallGetType(objectValue))
							{
								serInfo.UpdateValue((string)obj, objectValue, objectValue.GetType());
							}
							else
							{
								serInfo.UpdateValue((string)obj, objectValue, typeof(MarshalByRefObject));
							}
							num++;
							missingElements.m_values[i] = null;
							if (!bObjectFullyComplete)
							{
								holder.DecrementFixupsRemaining(this);
								objectHolder.RemoveDependency(holder.m_id);
							}
						}
					}
				}
			}
			else
			{
				for (int j = 0; j < missingElements.m_count; j++)
				{
					FixupHolder fixupHolder = missingElements.m_values[j];
					if (fixupHolder != null && this.GetCompletionInfo(fixupHolder, out objectHolder, out obj, bObjectFullyComplete))
					{
						if (objectHolder.TypeLoadExceptionReachable)
						{
							holder.TypeLoadException = objectHolder.TypeLoadException;
							if (holder.Reachable)
							{
								throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_TypeLoadFailure"), new object[] { holder.TypeLoadException.TypeName }));
							}
						}
						if (holder.Reachable)
						{
							objectHolder.Reachable = true;
						}
						switch (fixupHolder.m_fixupType)
						{
						case 1:
							if (holder.RequiresValueTypeFixup)
							{
								throw new SerializationException(Environment.GetResourceString("Serialization_ValueTypeFixup"));
							}
							((Array)holder.ObjectValue).SetValue(objectHolder.ObjectValue, (int[])obj);
							break;
						case 2:
						{
							MemberInfo memberInfo = (MemberInfo)obj;
							if (memberInfo.MemberType != MemberTypes.Field)
							{
								throw new SerializationException(Environment.GetResourceString("Serialization_UnableToFixup"));
							}
							if (holder.RequiresValueTypeFixup && holder.ValueTypeFixupPerformed)
							{
								if (!this.DoValueTypeFixup((FieldInfo)memberInfo, holder, objectHolder.ObjectValue))
								{
									throw new SerializationException(Environment.GetResourceString("Serialization_PartialValueTypeFixup"));
								}
							}
							else
							{
								FormatterServices.SerializationSetValue(memberInfo, holder.ObjectValue, objectHolder.ObjectValue);
							}
							if (objectHolder.RequiresValueTypeFixup)
							{
								objectHolder.ValueTypeFixupPerformed = true;
							}
							break;
						}
						default:
							throw new SerializationException(Environment.GetResourceString("Serialization_UnableToFixup"));
						}
						num++;
						missingElements.m_values[j] = null;
						if (!bObjectFullyComplete)
						{
							holder.DecrementFixupsRemaining(this);
							objectHolder.RemoveDependency(holder.m_id);
						}
					}
				}
			}
			this.m_fixupCount -= (long)num;
			if (missingElements.m_count == num)
			{
				holder.m_missingElements = null;
			}
		}

		// Token: 0x060021D4 RID: 8660 RVA: 0x00055BB4 File Offset: 0x00054BB4
		private void DoNewlyRegisteredObjectFixups(ObjectHolder holder)
		{
			if (holder.CanObjectValueChange)
			{
				return;
			}
			LongList dependentObjects = holder.DependentObjects;
			if (dependentObjects == null)
			{
				return;
			}
			dependentObjects.StartEnumeration();
			while (dependentObjects.MoveNext())
			{
				long num = dependentObjects.Current;
				ObjectHolder objectHolder = this.FindObjectHolder(num);
				objectHolder.DecrementFixupsRemaining(this);
				if (objectHolder.DirectlyDependentObjects == 0)
				{
					if (objectHolder.ObjectValue != null)
					{
						this.CompleteObject(objectHolder, true);
					}
					else
					{
						objectHolder.MarkForCompletionWhenAvailable();
					}
				}
			}
		}

		// Token: 0x060021D5 RID: 8661 RVA: 0x00055C1C File Offset: 0x00054C1C
		public virtual object GetObject(long objectID)
		{
			if (objectID <= 0L)
			{
				throw new ArgumentOutOfRangeException("objectID", Environment.GetResourceString("ArgumentOutOfRange_ObjectID"));
			}
			ObjectHolder objectHolder = this.FindObjectHolder(objectID);
			if (objectHolder == null || objectHolder.CanObjectValueChange)
			{
				return null;
			}
			return objectHolder.ObjectValue;
		}

		// Token: 0x060021D6 RID: 8662 RVA: 0x00055C5E File Offset: 0x00054C5E
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public virtual void RegisterObject(object obj, long objectID)
		{
			this.RegisterObject(obj, objectID, null, 0L, null);
		}

		// Token: 0x060021D7 RID: 8663 RVA: 0x00055C6C File Offset: 0x00054C6C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public void RegisterObject(object obj, long objectID, SerializationInfo info)
		{
			this.RegisterObject(obj, objectID, info, 0L, null);
		}

		// Token: 0x060021D8 RID: 8664 RVA: 0x00055C7A File Offset: 0x00054C7A
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public void RegisterObject(object obj, long objectID, SerializationInfo info, long idOfContainingObj, MemberInfo member)
		{
			this.RegisterObject(obj, objectID, info, idOfContainingObj, member, null);
		}

		// Token: 0x060021D9 RID: 8665 RVA: 0x00055C8C File Offset: 0x00054C8C
		internal void RegisterString(string obj, long objectID, SerializationInfo info, long idOfContainingObj, MemberInfo member)
		{
			ObjectHolder objectHolder = new ObjectHolder(obj, objectID, info, null, idOfContainingObj, (FieldInfo)member, null);
			this.AddObjectHolder(objectHolder);
		}

		// Token: 0x060021DA RID: 8666 RVA: 0x00055CB4 File Offset: 0x00054CB4
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public void RegisterObject(object obj, long objectID, SerializationInfo info, long idOfContainingObj, MemberInfo member, int[] arrayIndex)
		{
			ISerializationSurrogate serializationSurrogate = null;
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			if (objectID <= 0L)
			{
				throw new ArgumentOutOfRangeException("objectID", Environment.GetResourceString("ArgumentOutOfRange_ObjectID"));
			}
			if (member != null && !(member is RuntimeFieldInfo) && !(member is SerializationFieldInfo))
			{
				throw new SerializationException(Environment.GetResourceString("Serialization_UnknownMemberInfo"));
			}
			if (this.m_selector != null)
			{
				Type type;
				if (this.CanCallGetType(obj))
				{
					type = obj.GetType();
				}
				else
				{
					type = typeof(MarshalByRefObject);
				}
				ISurrogateSelector surrogateSelector;
				serializationSurrogate = this.m_selector.GetSurrogate(type, this.m_context, out surrogateSelector);
			}
			if (obj is IDeserializationCallback)
			{
				DeserializationEventHandler deserializationEventHandler = new DeserializationEventHandler(((IDeserializationCallback)obj).OnDeserialization);
				this.AddOnDeserialization(deserializationEventHandler);
			}
			if (arrayIndex != null)
			{
				arrayIndex = (int[])arrayIndex.Clone();
			}
			ObjectHolder objectHolder = this.FindObjectHolder(objectID);
			if (objectHolder == null)
			{
				objectHolder = new ObjectHolder(obj, objectID, info, serializationSurrogate, idOfContainingObj, (FieldInfo)member, arrayIndex);
				this.AddObjectHolder(objectHolder);
				if (objectHolder.RequiresDelayedFixup)
				{
					this.SpecialFixupObjects.Add(objectHolder);
				}
				this.AddOnDeserialized(obj);
				return;
			}
			if (objectHolder.ObjectValue != null)
			{
				throw new SerializationException(Environment.GetResourceString("Serialization_RegisterTwice"));
			}
			objectHolder.UpdateData(obj, info, serializationSurrogate, idOfContainingObj, (FieldInfo)member, arrayIndex, this);
			if (objectHolder.DirectlyDependentObjects > 0)
			{
				this.CompleteObject(objectHolder, false);
			}
			if (objectHolder.RequiresDelayedFixup)
			{
				this.SpecialFixupObjects.Add(objectHolder);
			}
			if (objectHolder.CompletelyFixed)
			{
				this.DoNewlyRegisteredObjectFixups(objectHolder);
				objectHolder.DependentObjects = null;
			}
			if (objectHolder.TotalDependentObjects > 0)
			{
				this.AddOnDeserialized(obj);
				return;
			}
			this.RaiseOnDeserializedEvent(obj);
		}

		// Token: 0x060021DB RID: 8667 RVA: 0x00055E48 File Offset: 0x00054E48
		internal void CompleteISerializableObject(object obj, SerializationInfo info, StreamingContext context)
		{
			RuntimeConstructorInfo runtimeConstructorInfo = null;
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			if (!(obj is ISerializable))
			{
				throw new ArgumentException(Environment.GetResourceString("Serialization_NotISer"));
			}
			Type type = obj.GetType();
			try
			{
				if (type == ObjectManager.TypeOfWindowsIdentity && this.m_isCrossAppDomain)
				{
					runtimeConstructorInfo = ObjectManager.GetConstructor(type, ObjectManager.SIWindowsIdentityConstructorTypes);
				}
				else
				{
					runtimeConstructorInfo = ObjectManager.GetConstructor(type);
				}
			}
			catch (Exception ex)
			{
				throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_ConstructorNotFound"), new object[] { type }), ex);
			}
			runtimeConstructorInfo.SerializationInvoke(obj, info, context);
		}

		// Token: 0x060021DC RID: 8668 RVA: 0x00055EEC File Offset: 0x00054EEC
		internal static RuntimeConstructorInfo GetConstructor(Type t)
		{
			return ObjectManager.GetConstructor(t, ObjectManager.SIConstructorTypes);
		}

		// Token: 0x060021DD RID: 8669 RVA: 0x00055EFC File Offset: 0x00054EFC
		internal static RuntimeConstructorInfo GetConstructor(Type t, Type[] ctorParams)
		{
			RuntimeConstructorInfo runtimeConstructorInfo;
			if ((runtimeConstructorInfo = (RuntimeConstructorInfo)t.Cache[CacheObjType.ConstructorInfo]) == null)
			{
				RuntimeType runtimeType = (RuntimeType)t;
				runtimeConstructorInfo = runtimeType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, CallingConventions.Any, ctorParams, null) as RuntimeConstructorInfo;
				if (runtimeConstructorInfo == null)
				{
					throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_ConstructorNotFound"), new object[] { t.FullName }));
				}
				t.Cache[CacheObjType.ConstructorInfo] = runtimeConstructorInfo;
			}
			return runtimeConstructorInfo;
		}

		// Token: 0x060021DE RID: 8670 RVA: 0x00055F74 File Offset: 0x00054F74
		public virtual void DoFixups()
		{
			int num = -1;
			while (num != 0)
			{
				num = 0;
				ObjectHolderListEnumerator fixupEnumerator = this.SpecialFixupObjects.GetFixupEnumerator();
				while (fixupEnumerator.MoveNext())
				{
					ObjectHolder objectHolder = fixupEnumerator.Current;
					if (objectHolder.ObjectValue == null)
					{
						throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_ObjectNotSupplied"), new object[] { objectHolder.m_id }));
					}
					if (objectHolder.TotalDependentObjects == 0)
					{
						if (objectHolder.RequiresSerInfoFixup)
						{
							this.FixupSpecialObject(objectHolder);
							num++;
						}
						else if (!objectHolder.IsIncompleteObjectReference)
						{
							this.CompleteObject(objectHolder, true);
						}
						if (objectHolder.IsIncompleteObjectReference && this.ResolveObjectReference(objectHolder))
						{
							num++;
						}
					}
				}
			}
			if (this.m_fixupCount != 0L)
			{
				for (int i = 0; i < this.m_objects.Length; i++)
				{
					for (ObjectHolder objectHolder = this.m_objects[i]; objectHolder != null; objectHolder = objectHolder.m_next)
					{
						if (objectHolder.TotalDependentObjects > 0)
						{
							this.CompleteObject(objectHolder, true);
						}
					}
					if (this.m_fixupCount == 0L)
					{
						return;
					}
				}
				throw new SerializationException(Environment.GetResourceString("Serialization_IncorrectNumberOfFixups"));
			}
			if (this.TopObject is TypeLoadExceptionHolder)
			{
				throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_TypeLoadFailure"), new object[] { ((TypeLoadExceptionHolder)this.TopObject).TypeName }));
			}
		}

		// Token: 0x060021DF RID: 8671 RVA: 0x000560D4 File Offset: 0x000550D4
		private void RegisterFixup(FixupHolder fixup, long objectToBeFixed, long objectRequired)
		{
			ObjectHolder objectHolder = this.FindOrCreateObjectHolder(objectToBeFixed);
			if (objectHolder.RequiresSerInfoFixup && fixup.m_fixupType == 2)
			{
				throw new SerializationException(Environment.GetResourceString("Serialization_InvalidFixupType"));
			}
			objectHolder.AddFixup(fixup, this);
			ObjectHolder objectHolder2 = this.FindOrCreateObjectHolder(objectRequired);
			objectHolder2.AddDependency(objectToBeFixed);
			this.m_fixupCount += 1L;
		}

		// Token: 0x060021E0 RID: 8672 RVA: 0x00056130 File Offset: 0x00055130
		public virtual void RecordFixup(long objectToBeFixed, MemberInfo member, long objectRequired)
		{
			if (objectToBeFixed <= 0L || objectRequired <= 0L)
			{
				throw new ArgumentOutOfRangeException((objectToBeFixed <= 0L) ? "objectToBeFixed" : "objectRequired", Environment.GetResourceString("Serialization_IdTooSmall"));
			}
			if (member == null)
			{
				throw new ArgumentNullException("member");
			}
			if (!(member is RuntimeFieldInfo) && !(member is SerializationFieldInfo))
			{
				throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_InvalidType"), new object[] { member.GetType().ToString() }));
			}
			FixupHolder fixupHolder = new FixupHolder(objectRequired, member, 2);
			this.RegisterFixup(fixupHolder, objectToBeFixed, objectRequired);
		}

		// Token: 0x060021E1 RID: 8673 RVA: 0x000561C8 File Offset: 0x000551C8
		public virtual void RecordDelayedFixup(long objectToBeFixed, string memberName, long objectRequired)
		{
			if (objectToBeFixed <= 0L || objectRequired <= 0L)
			{
				throw new ArgumentOutOfRangeException((objectToBeFixed <= 0L) ? "objectToBeFixed" : "objectRequired", Environment.GetResourceString("Serialization_IdTooSmall"));
			}
			if (memberName == null)
			{
				throw new ArgumentNullException("memberName");
			}
			FixupHolder fixupHolder = new FixupHolder(objectRequired, memberName, 4);
			this.RegisterFixup(fixupHolder, objectToBeFixed, objectRequired);
		}

		// Token: 0x060021E2 RID: 8674 RVA: 0x00056220 File Offset: 0x00055220
		public virtual void RecordArrayElementFixup(long arrayToBeFixed, int index, long objectRequired)
		{
			this.RecordArrayElementFixup(arrayToBeFixed, new int[] { index }, objectRequired);
		}

		// Token: 0x060021E3 RID: 8675 RVA: 0x00056244 File Offset: 0x00055244
		public virtual void RecordArrayElementFixup(long arrayToBeFixed, int[] indices, long objectRequired)
		{
			if (arrayToBeFixed <= 0L || objectRequired <= 0L)
			{
				throw new ArgumentOutOfRangeException((arrayToBeFixed <= 0L) ? "objectToBeFixed" : "objectRequired", Environment.GetResourceString("Serialization_IdTooSmall"));
			}
			if (indices == null)
			{
				throw new ArgumentNullException("indices");
			}
			FixupHolder fixupHolder = new FixupHolder(objectRequired, indices, 1);
			this.RegisterFixup(fixupHolder, arrayToBeFixed, objectRequired);
		}

		// Token: 0x060021E4 RID: 8676 RVA: 0x0005629C File Offset: 0x0005529C
		public virtual void RaiseDeserializationEvent()
		{
			if (this.m_onDeserializedHandler != null)
			{
				this.m_onDeserializedHandler(this.m_context);
			}
			if (this.m_onDeserializationHandler != null)
			{
				this.m_onDeserializationHandler(null);
			}
		}

		// Token: 0x060021E5 RID: 8677 RVA: 0x000562CB File Offset: 0x000552CB
		internal virtual void AddOnDeserialization(DeserializationEventHandler handler)
		{
			this.m_onDeserializationHandler = (DeserializationEventHandler)Delegate.Combine(this.m_onDeserializationHandler, handler);
		}

		// Token: 0x060021E6 RID: 8678 RVA: 0x000562E4 File Offset: 0x000552E4
		internal virtual void RemoveOnDeserialization(DeserializationEventHandler handler)
		{
			this.m_onDeserializationHandler = (DeserializationEventHandler)Delegate.Remove(this.m_onDeserializationHandler, handler);
		}

		// Token: 0x060021E7 RID: 8679 RVA: 0x00056300 File Offset: 0x00055300
		internal virtual void AddOnDeserialized(object obj)
		{
			SerializationEvents serializationEventsForType = SerializationEventsCache.GetSerializationEventsForType(obj.GetType());
			this.m_onDeserializedHandler = serializationEventsForType.AddOnDeserialized(obj, this.m_onDeserializedHandler);
		}

		// Token: 0x060021E8 RID: 8680 RVA: 0x0005632C File Offset: 0x0005532C
		internal virtual void RaiseOnDeserializedEvent(object obj)
		{
			SerializationEvents serializationEventsForType = SerializationEventsCache.GetSerializationEventsForType(obj.GetType());
			serializationEventsForType.InvokeOnDeserialized(obj, this.m_context);
		}

		// Token: 0x060021E9 RID: 8681 RVA: 0x00056354 File Offset: 0x00055354
		public void RaiseOnDeserializingEvent(object obj)
		{
			SerializationEvents serializationEventsForType = SerializationEventsCache.GetSerializationEventsForType(obj.GetType());
			serializationEventsForType.InvokeOnDeserializing(obj, this.m_context);
		}

		// Token: 0x04000DFC RID: 3580
		private const int DefaultInitialSize = 16;

		// Token: 0x04000DFD RID: 3581
		private const int MaxArraySize = 4096;

		// Token: 0x04000DFE RID: 3582
		private const int ArrayMask = 4095;

		// Token: 0x04000DFF RID: 3583
		private const int MaxReferenceDepth = 100;

		// Token: 0x04000E00 RID: 3584
		private DeserializationEventHandler m_onDeserializationHandler;

		// Token: 0x04000E01 RID: 3585
		private SerializationEventHandler m_onDeserializedHandler;

		// Token: 0x04000E02 RID: 3586
		private static Type[] SIConstructorTypes = new Type[2];

		// Token: 0x04000E03 RID: 3587
		private static Type TypeOfWindowsIdentity;

		// Token: 0x04000E04 RID: 3588
		private static Type[] SIWindowsIdentityConstructorTypes;

		// Token: 0x04000E05 RID: 3589
		internal ObjectHolder[] m_objects;

		// Token: 0x04000E06 RID: 3590
		internal object m_topObject;

		// Token: 0x04000E07 RID: 3591
		internal ObjectHolderList m_specialFixupObjects;

		// Token: 0x04000E08 RID: 3592
		internal long m_fixupCount;

		// Token: 0x04000E09 RID: 3593
		internal ISurrogateSelector m_selector;

		// Token: 0x04000E0A RID: 3594
		internal StreamingContext m_context;

		// Token: 0x04000E0B RID: 3595
		private bool m_isCrossAppDomain;
	}
}
