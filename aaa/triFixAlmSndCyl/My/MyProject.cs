using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.VisualBasic.CompilerServices;

namespace triFixAlmSndCyl.My
{
	// Token: 0x02000004 RID: 4
	[StandardModule]
	[HideModuleName]
	[GeneratedCode("MyTemplate", "8.0.0.0")]
	internal sealed class MyProject
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000006 RID: 6 RVA: 0x00006664 File Offset: 0x00004A64
		[HelpKeyword("My.Computer")]
		internal static MyComputer Computer
		{
			[DebuggerHidden]
			get
			{
				return MyProject.m_ComputerObjectProvider.GetInstance;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000007 RID: 7 RVA: 0x0000667C File Offset: 0x00004A7C
		[HelpKeyword("My.Application")]
		internal static MyApplication Application
		{
			[DebuggerHidden]
			get
			{
				return MyProject.m_AppObjectProvider.GetInstance;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000008 RID: 8 RVA: 0x00006694 File Offset: 0x00004A94
		[HelpKeyword("My.User")]
		internal static User User
		{
			[DebuggerHidden]
			get
			{
				return MyProject.m_UserObjectProvider.GetInstance;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000009 RID: 9 RVA: 0x000066AC File Offset: 0x00004AAC
		[HelpKeyword("My.Forms")]
		internal static MyProject.MyForms Forms
		{
			[DebuggerHidden]
			get
			{
				return MyProject.m_MyFormsObjectProvider.GetInstance;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000A RID: 10 RVA: 0x000066C4 File Offset: 0x00004AC4
		[HelpKeyword("My.WebServices")]
		internal static MyProject.MyWebServices WebServices
		{
			[DebuggerHidden]
			get
			{
				return MyProject.m_MyWebServicesObjectProvider.GetInstance;
			}
		}

		// Token: 0x04000001 RID: 1
		private static readonly MyProject.ThreadSafeObjectProvider<MyComputer> m_ComputerObjectProvider = new MyProject.ThreadSafeObjectProvider<MyComputer>();

		// Token: 0x04000002 RID: 2
		private static readonly MyProject.ThreadSafeObjectProvider<MyApplication> m_AppObjectProvider = new MyProject.ThreadSafeObjectProvider<MyApplication>();

		// Token: 0x04000003 RID: 3
		private static readonly MyProject.ThreadSafeObjectProvider<User> m_UserObjectProvider = new MyProject.ThreadSafeObjectProvider<User>();

		// Token: 0x04000004 RID: 4
		private static MyProject.ThreadSafeObjectProvider<MyProject.MyForms> m_MyFormsObjectProvider = new MyProject.ThreadSafeObjectProvider<MyProject.MyForms>();

		// Token: 0x04000005 RID: 5
		private static readonly MyProject.ThreadSafeObjectProvider<MyProject.MyWebServices> m_MyWebServicesObjectProvider = new MyProject.ThreadSafeObjectProvider<MyProject.MyWebServices>();

		// Token: 0x02000005 RID: 5
		[MyGroupCollection("System.Windows.Forms.Form", "Create__Instance__", "Dispose__Instance__", "My.MyProject.Forms")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal sealed class MyForms
		{
			// Token: 0x17000006 RID: 6
			// (get) Token: 0x0600000B RID: 11 RVA: 0x000066DC File Offset: 0x00004ADC
			// (set) Token: 0x06000010 RID: 16 RVA: 0x00006790 File Offset: 0x00004B90
			public frmAbout frmAbout
			{
				get
				{
					this.m_frmAbout = MyProject.MyForms.Create__Instance__<frmAbout>(this.m_frmAbout);
					return this.m_frmAbout;
				}
				set
				{
					if (value == this.m_frmAbout)
					{
						return;
					}
					if (value != null)
					{
						throw new ArgumentException("Property can only be set to Nothing");
					}
					this.Dispose__Instance__<frmAbout>(ref this.m_frmAbout);
				}
			}

			// Token: 0x17000007 RID: 7
			// (get) Token: 0x0600000C RID: 12 RVA: 0x00006700 File Offset: 0x00004B00
			// (set) Token: 0x06000011 RID: 17 RVA: 0x000067B8 File Offset: 0x00004BB8
			public frmAlarmSndCfg frmAlarmSndCfg
			{
				get
				{
					this.m_frmAlarmSndCfg = MyProject.MyForms.Create__Instance__<frmAlarmSndCfg>(this.m_frmAlarmSndCfg);
					return this.m_frmAlarmSndCfg;
				}
				set
				{
					if (value == this.m_frmAlarmSndCfg)
					{
						return;
					}
					if (value != null)
					{
						throw new ArgumentException("Property can only be set to Nothing");
					}
					this.Dispose__Instance__<frmAlarmSndCfg>(ref this.m_frmAlarmSndCfg);
				}
			}

			// Token: 0x17000008 RID: 8
			// (get) Token: 0x0600000D RID: 13 RVA: 0x00006724 File Offset: 0x00004B24
			// (set) Token: 0x06000012 RID: 18 RVA: 0x000067E0 File Offset: 0x00004BE0
			public frmAlmSndRun frmAlmSndRun
			{
				get
				{
					this.m_frmAlmSndRun = MyProject.MyForms.Create__Instance__<frmAlmSndRun>(this.m_frmAlmSndRun);
					return this.m_frmAlmSndRun;
				}
				set
				{
					if (value == this.m_frmAlmSndRun)
					{
						return;
					}
					if (value != null)
					{
						throw new ArgumentException("Property can only be set to Nothing");
					}
					this.Dispose__Instance__<frmAlmSndRun>(ref this.m_frmAlmSndRun);
				}
			}

			// Token: 0x17000009 RID: 9
			// (get) Token: 0x0600000E RID: 14 RVA: 0x00006748 File Offset: 0x00004B48
			// (set) Token: 0x06000013 RID: 19 RVA: 0x00006808 File Offset: 0x00004C08
			public frmDelCache frmDelCache
			{
				get
				{
					this.m_frmDelCache = MyProject.MyForms.Create__Instance__<frmDelCache>(this.m_frmDelCache);
					return this.m_frmDelCache;
				}
				set
				{
					if (value == this.m_frmDelCache)
					{
						return;
					}
					if (value != null)
					{
						throw new ArgumentException("Property can only be set to Nothing");
					}
					this.Dispose__Instance__<frmDelCache>(ref this.m_frmDelCache);
				}
			}

			// Token: 0x1700000A RID: 10
			// (get) Token: 0x0600000F RID: 15 RVA: 0x0000676C File Offset: 0x00004B6C
			// (set) Token: 0x06000014 RID: 20 RVA: 0x00006830 File Offset: 0x00004C30
			public frmTagModify frmTagModify
			{
				get
				{
					this.m_frmTagModify = MyProject.MyForms.Create__Instance__<frmTagModify>(this.m_frmTagModify);
					return this.m_frmTagModify;
				}
				set
				{
					if (value == this.m_frmTagModify)
					{
						return;
					}
					if (value != null)
					{
						throw new ArgumentException("Property can only be set to Nothing");
					}
					this.Dispose__Instance__<frmTagModify>(ref this.m_frmTagModify);
				}
			}

			// Token: 0x06000015 RID: 21 RVA: 0x00006858 File Offset: 0x00004C58
			[DebuggerHidden]
			private static T Create__Instance__<T>(T Instance) where T : Form, new()
			{
				if (Instance == null || Instance.IsDisposed)
				{
					if (MyProject.MyForms.m_FormBeingCreated != null)
					{
						if (MyProject.MyForms.m_FormBeingCreated.ContainsKey(typeof(T)))
						{
							throw new InvalidOperationException(Utils.GetResourceString("WinForms_RecursiveFormCreate", new string[0]));
						}
					}
					else
					{
						MyProject.MyForms.m_FormBeingCreated = new Hashtable();
					}
					MyProject.MyForms.m_FormBeingCreated.Add(typeof(T), null);
					try
					{
						return new T();
					}
					catch (TargetInvocationException ex) when (ex.InnerException != null)
					{
						string resourceString = Utils.GetResourceString("WinForms_SeeInnerException", new string[] { ex.InnerException.Message });
						throw new InvalidOperationException(resourceString, ex.InnerException);
					}
					finally
					{
						MyProject.MyForms.m_FormBeingCreated.Remove(typeof(T));
					}
					return Instance;
				}
				return Instance;
			}

			// Token: 0x06000016 RID: 22 RVA: 0x00006964 File Offset: 0x00004D64
			[DebuggerHidden]
			private void Dispose__Instance__<T>(ref T instance) where T : Form
			{
				instance.Dispose();
				instance = default(T);
			}

			// Token: 0x06000017 RID: 23 RVA: 0x0000698C File Offset: 0x00004D8C
			[DebuggerHidden]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public MyForms()
			{
			}

			// Token: 0x06000018 RID: 24 RVA: 0x00006994 File Offset: 0x00004D94
			[EditorBrowsable(EditorBrowsableState.Never)]
			public override bool Equals(object o)
			{
				return base.Equals(RuntimeHelpers.GetObjectValue(o));
			}

			// Token: 0x06000019 RID: 25 RVA: 0x000069B0 File Offset: 0x00004DB0
			[EditorBrowsable(EditorBrowsableState.Never)]
			public override int GetHashCode()
			{
				return base.GetHashCode();
			}

			// Token: 0x0600001A RID: 26 RVA: 0x000069C4 File Offset: 0x00004DC4
			[EditorBrowsable(EditorBrowsableState.Never)]
			internal new Type GetType()
			{
				return typeof(MyProject.MyForms);
			}

			// Token: 0x0600001B RID: 27 RVA: 0x000069DC File Offset: 0x00004DDC
			[EditorBrowsable(EditorBrowsableState.Never)]
			public override string ToString()
			{
				return base.ToString();
			}

			// Token: 0x04000006 RID: 6
			public frmAbout m_frmAbout;

			// Token: 0x04000007 RID: 7
			public frmAlarmSndCfg m_frmAlarmSndCfg;

			// Token: 0x04000008 RID: 8
			public frmAlmSndRun m_frmAlmSndRun;

			// Token: 0x04000009 RID: 9
			public frmDelCache m_frmDelCache;

			// Token: 0x0400000A RID: 10
			public frmTagModify m_frmTagModify;

			// Token: 0x0400000B RID: 11
			[ThreadStatic]
			private static Hashtable m_FormBeingCreated;
		}

		// Token: 0x02000006 RID: 6
		[EditorBrowsable(EditorBrowsableState.Never)]
		[MyGroupCollection("System.Web.Services.Protocols.SoapHttpClientProtocol", "Create__Instance__", "Dispose__Instance__", "")]
		internal sealed class MyWebServices
		{
			// Token: 0x0600001C RID: 28 RVA: 0x000069F0 File Offset: 0x00004DF0
			[EditorBrowsable(EditorBrowsableState.Never)]
			[DebuggerHidden]
			public override bool Equals(object o)
			{
				return base.Equals(RuntimeHelpers.GetObjectValue(o));
			}

			// Token: 0x0600001D RID: 29 RVA: 0x00006A0C File Offset: 0x00004E0C
			[DebuggerHidden]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public override int GetHashCode()
			{
				return base.GetHashCode();
			}

			// Token: 0x0600001E RID: 30 RVA: 0x00006A20 File Offset: 0x00004E20
			[EditorBrowsable(EditorBrowsableState.Never)]
			[DebuggerHidden]
			internal new Type GetType()
			{
				return typeof(MyProject.MyWebServices);
			}

			// Token: 0x0600001F RID: 31 RVA: 0x00006A38 File Offset: 0x00004E38
			[DebuggerHidden]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public override string ToString()
			{
				return base.ToString();
			}

			// Token: 0x06000020 RID: 32 RVA: 0x00006A4C File Offset: 0x00004E4C
			[DebuggerHidden]
			private static T Create__Instance__<T>(T instance) where T : new()
			{
				if (instance == null)
				{
					return new T();
				}
				return instance;
			}

			// Token: 0x06000021 RID: 33 RVA: 0x00006A68 File Offset: 0x00004E68
			[DebuggerHidden]
			private void Dispose__Instance__<T>(ref T instance)
			{
				instance = default(T);
			}

			// Token: 0x06000022 RID: 34 RVA: 0x00006A84 File Offset: 0x00004E84
			[DebuggerHidden]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public MyWebServices()
			{
			}
		}

		// Token: 0x02000007 RID: 7
		[ComVisible(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal sealed class ThreadSafeObjectProvider<T> where T : new()
		{
			// Token: 0x1700000B RID: 11
			// (get) Token: 0x06000023 RID: 35 RVA: 0x00006A8C File Offset: 0x00004E8C
			internal T GetInstance
			{
				[DebuggerHidden]
				get
				{
					if (MyProject.ThreadSafeObjectProvider<T>.m_ThreadStaticValue == null)
					{
						MyProject.ThreadSafeObjectProvider<T>.m_ThreadStaticValue = new T();
					}
					return MyProject.ThreadSafeObjectProvider<T>.m_ThreadStaticValue;
				}
			}

			// Token: 0x06000024 RID: 36 RVA: 0x00006AB4 File Offset: 0x00004EB4
			[DebuggerHidden]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public ThreadSafeObjectProvider()
			{
			}

			// Token: 0x0400000C RID: 12
			[CompilerGenerated]
			[ThreadStatic]
			private static T m_ThreadStaticValue;
		}
	}
}
