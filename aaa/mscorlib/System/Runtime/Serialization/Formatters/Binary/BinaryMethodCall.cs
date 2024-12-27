using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007C6 RID: 1990
	internal sealed class BinaryMethodCall
	{
		// Token: 0x060046F5 RID: 18165 RVA: 0x000F39C8 File Offset: 0x000F29C8
		internal object[] WriteArray(string uri, string methodName, string typeName, Type[] instArgs, object[] args, object methodSignature, object callContext, object[] properties)
		{
			this.uri = uri;
			this.methodName = methodName;
			this.typeName = typeName;
			this.instArgs = instArgs;
			this.args = args;
			this.methodSignature = methodSignature;
			this.callContext = callContext;
			this.properties = properties;
			int num = 0;
			if (args == null || args.Length == 0)
			{
				this.messageEnum = MessageEnum.NoArgs;
			}
			else
			{
				this.argTypes = new Type[args.Length];
				this.bArgsPrimitive = true;
				for (int i = 0; i < args.Length; i++)
				{
					if (args[i] != null)
					{
						this.argTypes[i] = args[i].GetType();
						if ((!BinaryConverter.IsTypePrimitive(this.argTypes[i]) && this.argTypes[i] != Converter.typeofString) || args[i] is ISerializable)
						{
							this.bArgsPrimitive = false;
							break;
						}
					}
				}
				if (this.bArgsPrimitive)
				{
					this.messageEnum = MessageEnum.ArgsInline;
				}
				else
				{
					num++;
					this.messageEnum = MessageEnum.ArgsInArray;
				}
			}
			if (instArgs != null)
			{
				num++;
				this.messageEnum |= MessageEnum.GenericMethod;
			}
			if (methodSignature != null)
			{
				num++;
				this.messageEnum |= MessageEnum.MethodSignatureInArray;
			}
			if (callContext == null)
			{
				this.messageEnum |= MessageEnum.NoContext;
			}
			else if (callContext is string)
			{
				this.messageEnum |= MessageEnum.ContextInline;
			}
			else
			{
				num++;
				this.messageEnum |= MessageEnum.ContextInArray;
			}
			if (properties != null)
			{
				num++;
				this.messageEnum |= MessageEnum.PropertyInArray;
			}
			if (IOUtil.FlagTest(this.messageEnum, MessageEnum.ArgsInArray) && num == 1)
			{
				this.messageEnum ^= MessageEnum.ArgsInArray;
				this.messageEnum |= MessageEnum.ArgsIsArray;
				return args;
			}
			if (num > 0)
			{
				int num2 = 0;
				this.callA = new object[num];
				if (IOUtil.FlagTest(this.messageEnum, MessageEnum.ArgsInArray))
				{
					this.callA[num2++] = args;
				}
				if (IOUtil.FlagTest(this.messageEnum, MessageEnum.GenericMethod))
				{
					this.callA[num2++] = instArgs;
				}
				if (IOUtil.FlagTest(this.messageEnum, MessageEnum.MethodSignatureInArray))
				{
					this.callA[num2++] = methodSignature;
				}
				if (IOUtil.FlagTest(this.messageEnum, MessageEnum.ContextInArray))
				{
					this.callA[num2++] = callContext;
				}
				if (IOUtil.FlagTest(this.messageEnum, MessageEnum.PropertyInArray))
				{
					this.callA[num2] = properties;
				}
				return this.callA;
			}
			return null;
		}

		// Token: 0x060046F6 RID: 18166 RVA: 0x000F3C28 File Offset: 0x000F2C28
		internal void Write(__BinaryWriter sout)
		{
			sout.WriteByte(21);
			sout.WriteInt32((int)this.messageEnum);
			IOUtil.WriteStringWithCode(this.methodName, sout);
			IOUtil.WriteStringWithCode(this.typeName, sout);
			if (IOUtil.FlagTest(this.messageEnum, MessageEnum.ContextInline))
			{
				IOUtil.WriteStringWithCode((string)this.callContext, sout);
			}
			if (IOUtil.FlagTest(this.messageEnum, MessageEnum.ArgsInline))
			{
				sout.WriteInt32(this.args.Length);
				for (int i = 0; i < this.args.Length; i++)
				{
					IOUtil.WriteWithCode(this.argTypes[i], this.args[i], sout);
				}
			}
		}

		// Token: 0x060046F7 RID: 18167 RVA: 0x000F3CC8 File Offset: 0x000F2CC8
		internal void Read(__BinaryParser input)
		{
			this.messageEnum = (MessageEnum)input.ReadInt32();
			this.methodName = (string)IOUtil.ReadWithCode(input);
			this.typeName = (string)IOUtil.ReadWithCode(input);
			if (IOUtil.FlagTest(this.messageEnum, MessageEnum.ContextInline))
			{
				this.scallContext = (string)IOUtil.ReadWithCode(input);
				this.callContext = new LogicalCallContext
				{
					RemotingData = 
					{
						LogicalCallID = this.scallContext
					}
				};
			}
			if (IOUtil.FlagTest(this.messageEnum, MessageEnum.ArgsInline))
			{
				this.args = IOUtil.ReadArgs(input);
			}
		}

		// Token: 0x060046F8 RID: 18168 RVA: 0x000F3D5C File Offset: 0x000F2D5C
		internal IMethodCallMessage ReadArray(object[] callA, object handlerObject)
		{
			if (IOUtil.FlagTest(this.messageEnum, MessageEnum.ArgsIsArray))
			{
				this.args = callA;
			}
			else
			{
				int num = 0;
				if (IOUtil.FlagTest(this.messageEnum, MessageEnum.ArgsInArray))
				{
					if (callA.Length < num)
					{
						throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_Method"), new object[0]));
					}
					this.args = (object[])callA[num++];
				}
				if (IOUtil.FlagTest(this.messageEnum, MessageEnum.GenericMethod))
				{
					if (callA.Length < num)
					{
						throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_Method"), new object[0]));
					}
					this.instArgs = (Type[])callA[num++];
				}
				if (IOUtil.FlagTest(this.messageEnum, MessageEnum.MethodSignatureInArray))
				{
					if (callA.Length < num)
					{
						throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_Method"), new object[0]));
					}
					this.methodSignature = callA[num++];
				}
				if (IOUtil.FlagTest(this.messageEnum, MessageEnum.ContextInArray))
				{
					if (callA.Length < num)
					{
						throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_Method"), new object[0]));
					}
					this.callContext = callA[num++];
				}
				if (IOUtil.FlagTest(this.messageEnum, MessageEnum.PropertyInArray))
				{
					if (callA.Length < num)
					{
						throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_Method"), new object[0]));
					}
					this.properties = callA[num++];
				}
			}
			return new MethodCall(handlerObject, new BinaryMethodCallMessage(this.uri, this.methodName, this.typeName, this.instArgs, this.args, this.methodSignature, (LogicalCallContext)this.callContext, (object[])this.properties));
		}

		// Token: 0x060046F9 RID: 18169 RVA: 0x000F3F26 File Offset: 0x000F2F26
		internal void Dump()
		{
		}

		// Token: 0x060046FA RID: 18170 RVA: 0x000F3F28 File Offset: 0x000F2F28
		[Conditional("_LOGGING")]
		private void DumpInternal()
		{
			if (BCLDebug.CheckEnabled("BINARY"))
			{
				if (IOUtil.FlagTest(this.messageEnum, MessageEnum.ContextInline))
				{
					string text = this.callContext as string;
				}
				if (IOUtil.FlagTest(this.messageEnum, MessageEnum.ArgsInline))
				{
					for (int i = 0; i < this.args.Length; i++)
					{
					}
				}
			}
		}

		// Token: 0x040023AD RID: 9133
		private string uri;

		// Token: 0x040023AE RID: 9134
		private string methodName;

		// Token: 0x040023AF RID: 9135
		private string typeName;

		// Token: 0x040023B0 RID: 9136
		private Type[] instArgs;

		// Token: 0x040023B1 RID: 9137
		private object[] args;

		// Token: 0x040023B2 RID: 9138
		private object methodSignature;

		// Token: 0x040023B3 RID: 9139
		private object callContext;

		// Token: 0x040023B4 RID: 9140
		private string scallContext;

		// Token: 0x040023B5 RID: 9141
		private object properties;

		// Token: 0x040023B6 RID: 9142
		private Type[] argTypes;

		// Token: 0x040023B7 RID: 9143
		private bool bArgsPrimitive = true;

		// Token: 0x040023B8 RID: 9144
		private MessageEnum messageEnum;

		// Token: 0x040023B9 RID: 9145
		private object[] callA;
	}
}
