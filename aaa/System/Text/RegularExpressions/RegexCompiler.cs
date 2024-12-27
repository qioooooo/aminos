using System;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;

namespace System.Text.RegularExpressions
{
	// Token: 0x02000019 RID: 25
	internal abstract class RegexCompiler
	{
		// Token: 0x060000C7 RID: 199 RVA: 0x000067FC File Offset: 0x000057FC
		static RegexCompiler()
		{
			new ReflectionPermission(PermissionState.Unrestricted).Assert();
			try
			{
				RegexCompiler._textbegF = RegexCompiler.RegexRunnerField("runtextbeg");
				RegexCompiler._textendF = RegexCompiler.RegexRunnerField("runtextend");
				RegexCompiler._textstartF = RegexCompiler.RegexRunnerField("runtextstart");
				RegexCompiler._textposF = RegexCompiler.RegexRunnerField("runtextpos");
				RegexCompiler._textF = RegexCompiler.RegexRunnerField("runtext");
				RegexCompiler._trackposF = RegexCompiler.RegexRunnerField("runtrackpos");
				RegexCompiler._trackF = RegexCompiler.RegexRunnerField("runtrack");
				RegexCompiler._stackposF = RegexCompiler.RegexRunnerField("runstackpos");
				RegexCompiler._stackF = RegexCompiler.RegexRunnerField("runstack");
				RegexCompiler._trackcountF = RegexCompiler.RegexRunnerField("runtrackcount");
				RegexCompiler._ensurestorageM = RegexCompiler.RegexRunnerMethod("EnsureStorage");
				RegexCompiler._captureM = RegexCompiler.RegexRunnerMethod("Capture");
				RegexCompiler._transferM = RegexCompiler.RegexRunnerMethod("TransferCapture");
				RegexCompiler._uncaptureM = RegexCompiler.RegexRunnerMethod("Uncapture");
				RegexCompiler._ismatchedM = RegexCompiler.RegexRunnerMethod("IsMatched");
				RegexCompiler._matchlengthM = RegexCompiler.RegexRunnerMethod("MatchLength");
				RegexCompiler._matchindexM = RegexCompiler.RegexRunnerMethod("MatchIndex");
				RegexCompiler._isboundaryM = RegexCompiler.RegexRunnerMethod("IsBoundary");
				RegexCompiler._charInSetM = RegexCompiler.RegexRunnerMethod("CharInClass");
				RegexCompiler._isECMABoundaryM = RegexCompiler.RegexRunnerMethod("IsECMABoundary");
				RegexCompiler._crawlposM = RegexCompiler.RegexRunnerMethod("Crawlpos");
				RegexCompiler._chartolowerM = typeof(char).GetMethod("ToLower", new Type[]
				{
					typeof(char),
					typeof(CultureInfo)
				});
				RegexCompiler._getcharM = typeof(string).GetMethod("get_Chars", new Type[] { typeof(int) });
				RegexCompiler._getCurrentCulture = typeof(CultureInfo).GetMethod("get_CurrentCulture");
				RegexCompiler._getInvariantCulture = typeof(CultureInfo).GetMethod("get_InvariantCulture");
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00006A10 File Offset: 0x00005A10
		private static FieldInfo RegexRunnerField(string fieldname)
		{
			return typeof(RegexRunner).GetField(fieldname, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00006A24 File Offset: 0x00005A24
		private static MethodInfo RegexRunnerMethod(string methname)
		{
			return typeof(RegexRunner).GetMethod(methname, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00006A38 File Offset: 0x00005A38
		internal static RegexRunnerFactory Compile(RegexCode code, RegexOptions options)
		{
			RegexLWCGCompiler regexLWCGCompiler = new RegexLWCGCompiler();
			new ReflectionPermission(PermissionState.Unrestricted).Assert();
			RegexRunnerFactory regexRunnerFactory;
			try
			{
				regexRunnerFactory = regexLWCGCompiler.FactoryInstanceFromCode(code, options);
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			return regexRunnerFactory;
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00006A78 File Offset: 0x00005A78
		internal static void CompileToAssembly(RegexCompilationInfo[] regexes, AssemblyName an, CustomAttributeBuilder[] attribs, string resourceFile, Evidence evidence)
		{
			RegexTypeCompiler regexTypeCompiler = new RegexTypeCompiler(an, attribs, resourceFile, evidence);
			for (int i = 0; i < regexes.Length; i++)
			{
				string pattern = regexes[i].Pattern;
				RegexOptions options = regexes[i].Options;
				string text;
				if (regexes[i].Namespace.Length == 0)
				{
					text = regexes[i].Name;
				}
				else
				{
					text = regexes[i].Namespace + "." + regexes[i].Name;
				}
				RegexTree regexTree = RegexParser.Parse(pattern, options);
				RegexCode regexCode = RegexWriter.Write(regexTree);
				new ReflectionPermission(PermissionState.Unrestricted).Assert();
				try
				{
					Type type = regexTypeCompiler.FactoryTypeFromCode(regexCode, options, text);
					regexTypeCompiler.GenerateRegexType(pattern, options, text, regexes[i].IsPublic, regexCode, regexTree, type);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
			}
			regexTypeCompiler.Save();
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00006B50 File Offset: 0x00005B50
		internal int AddBacktrackNote(int flags, Label l, int codepos)
		{
			if (this._notes == null || this._notecount >= this._notes.Length)
			{
				RegexCompiler.BacktrackNote[] array = new RegexCompiler.BacktrackNote[(this._notes == null) ? 16 : (this._notes.Length * 2)];
				if (this._notes != null)
				{
					Array.Copy(this._notes, 0, array, 0, this._notecount);
				}
				this._notes = array;
			}
			this._notes[this._notecount] = new RegexCompiler.BacktrackNote(flags, l, codepos);
			return this._notecount++;
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00006BDA File Offset: 0x00005BDA
		internal int AddTrack()
		{
			return this.AddTrack(128);
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00006BE7 File Offset: 0x00005BE7
		internal int AddTrack(int flags)
		{
			return this.AddBacktrackNote(flags, this.DefineLabel(), this._codepos);
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00006BFC File Offset: 0x00005BFC
		internal int AddGoto(int destpos)
		{
			if (this._goto[destpos] == -1)
			{
				this._goto[destpos] = this.AddBacktrackNote(0, this._labels[destpos], destpos);
			}
			return this._goto[destpos];
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00006C32 File Offset: 0x00005C32
		internal int AddUniqueTrack(int i)
		{
			return this.AddUniqueTrack(i, 128);
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00006C40 File Offset: 0x00005C40
		internal int AddUniqueTrack(int i, int flags)
		{
			if (this._uniquenote[i] == -1)
			{
				this._uniquenote[i] = this.AddTrack(flags);
			}
			return this._uniquenote[i];
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00006C64 File Offset: 0x00005C64
		internal Label DefineLabel()
		{
			return this._ilg.DefineLabel();
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00006C71 File Offset: 0x00005C71
		internal void MarkLabel(Label l)
		{
			this._ilg.MarkLabel(l);
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00006C7F File Offset: 0x00005C7F
		internal int Operand(int i)
		{
			return this._codes[this._codepos + i + 1];
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00006C92 File Offset: 0x00005C92
		internal bool IsRtl()
		{
			return (this._regexopcode & 64) != 0;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00006CA3 File Offset: 0x00005CA3
		internal bool IsCi()
		{
			return (this._regexopcode & 512) != 0;
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00006CB7 File Offset: 0x00005CB7
		internal int Code()
		{
			return this._regexopcode & 63;
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00006CC2 File Offset: 0x00005CC2
		internal void Ldstr(string str)
		{
			this._ilg.Emit(OpCodes.Ldstr, str);
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00006CD5 File Offset: 0x00005CD5
		internal void Ldc(int i)
		{
			if (i <= 127 && i >= -128)
			{
				this._ilg.Emit(OpCodes.Ldc_I4_S, (byte)i);
				return;
			}
			this._ilg.Emit(OpCodes.Ldc_I4, i);
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00006D05 File Offset: 0x00005D05
		internal void Dup()
		{
			this._ilg.Emit(OpCodes.Dup);
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00006D17 File Offset: 0x00005D17
		internal void Ret()
		{
			this._ilg.Emit(OpCodes.Ret);
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00006D29 File Offset: 0x00005D29
		internal void Pop()
		{
			this._ilg.Emit(OpCodes.Pop);
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00006D3B File Offset: 0x00005D3B
		internal void Add()
		{
			this._ilg.Emit(OpCodes.Add);
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00006D4D File Offset: 0x00005D4D
		internal void Add(bool negate)
		{
			if (negate)
			{
				this._ilg.Emit(OpCodes.Sub);
				return;
			}
			this._ilg.Emit(OpCodes.Add);
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00006D73 File Offset: 0x00005D73
		internal void Sub()
		{
			this._ilg.Emit(OpCodes.Sub);
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00006D85 File Offset: 0x00005D85
		internal void Sub(bool negate)
		{
			if (negate)
			{
				this._ilg.Emit(OpCodes.Add);
				return;
			}
			this._ilg.Emit(OpCodes.Sub);
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00006DAB File Offset: 0x00005DAB
		internal void Ldloc(LocalBuilder lt)
		{
			this._ilg.Emit(OpCodes.Ldloc_S, lt);
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00006DBE File Offset: 0x00005DBE
		internal void Stloc(LocalBuilder lt)
		{
			this._ilg.Emit(OpCodes.Stloc_S, lt);
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00006DD1 File Offset: 0x00005DD1
		internal void Ldthis()
		{
			this._ilg.Emit(OpCodes.Ldarg_0);
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00006DE3 File Offset: 0x00005DE3
		internal void Ldthisfld(FieldInfo ft)
		{
			this.Ldthis();
			this._ilg.Emit(OpCodes.Ldfld, ft);
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00006DFC File Offset: 0x00005DFC
		internal void Mvfldloc(FieldInfo ft, LocalBuilder lt)
		{
			this.Ldthisfld(ft);
			this.Stloc(lt);
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00006E0C File Offset: 0x00005E0C
		internal void Mvlocfld(LocalBuilder lt, FieldInfo ft)
		{
			this.Ldthis();
			this.Ldloc(lt);
			this.Stfld(ft);
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00006E22 File Offset: 0x00005E22
		internal void Stfld(FieldInfo ft)
		{
			this._ilg.Emit(OpCodes.Stfld, ft);
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00006E35 File Offset: 0x00005E35
		internal void Callvirt(MethodInfo mt)
		{
			this._ilg.Emit(OpCodes.Callvirt, mt);
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00006E48 File Offset: 0x00005E48
		internal void Call(MethodInfo mt)
		{
			this._ilg.Emit(OpCodes.Call, mt);
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00006E5B File Offset: 0x00005E5B
		internal void Newobj(ConstructorInfo ct)
		{
			this._ilg.Emit(OpCodes.Newobj, ct);
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00006E6E File Offset: 0x00005E6E
		internal void BrfalseFar(Label l)
		{
			this._ilg.Emit(OpCodes.Brfalse, l);
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00006E81 File Offset: 0x00005E81
		internal void BrtrueFar(Label l)
		{
			this._ilg.Emit(OpCodes.Brtrue, l);
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00006E94 File Offset: 0x00005E94
		internal void BrFar(Label l)
		{
			this._ilg.Emit(OpCodes.Br, l);
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00006EA7 File Offset: 0x00005EA7
		internal void BleFar(Label l)
		{
			this._ilg.Emit(OpCodes.Ble, l);
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00006EBA File Offset: 0x00005EBA
		internal void BltFar(Label l)
		{
			this._ilg.Emit(OpCodes.Blt, l);
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00006ECD File Offset: 0x00005ECD
		internal void BgeFar(Label l)
		{
			this._ilg.Emit(OpCodes.Bge, l);
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00006EE0 File Offset: 0x00005EE0
		internal void BgtFar(Label l)
		{
			this._ilg.Emit(OpCodes.Bgt, l);
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00006EF3 File Offset: 0x00005EF3
		internal void BneFar(Label l)
		{
			this._ilg.Emit(OpCodes.Bne_Un, l);
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00006F06 File Offset: 0x00005F06
		internal void BeqFar(Label l)
		{
			this._ilg.Emit(OpCodes.Beq, l);
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00006F19 File Offset: 0x00005F19
		internal void Brfalse(Label l)
		{
			this._ilg.Emit(OpCodes.Brfalse_S, l);
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00006F2C File Offset: 0x00005F2C
		internal void Br(Label l)
		{
			this._ilg.Emit(OpCodes.Br_S, l);
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00006F3F File Offset: 0x00005F3F
		internal void Ble(Label l)
		{
			this._ilg.Emit(OpCodes.Ble_S, l);
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00006F52 File Offset: 0x00005F52
		internal void Blt(Label l)
		{
			this._ilg.Emit(OpCodes.Blt_S, l);
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00006F65 File Offset: 0x00005F65
		internal void Bge(Label l)
		{
			this._ilg.Emit(OpCodes.Bge_S, l);
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00006F78 File Offset: 0x00005F78
		internal void Bgt(Label l)
		{
			this._ilg.Emit(OpCodes.Bgt_S, l);
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00006F8B File Offset: 0x00005F8B
		internal void Bgtun(Label l)
		{
			this._ilg.Emit(OpCodes.Bgt_Un_S, l);
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00006F9E File Offset: 0x00005F9E
		internal void Bne(Label l)
		{
			this._ilg.Emit(OpCodes.Bne_Un_S, l);
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00006FB1 File Offset: 0x00005FB1
		internal void Beq(Label l)
		{
			this._ilg.Emit(OpCodes.Beq_S, l);
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00006FC4 File Offset: 0x00005FC4
		internal void Ldlen()
		{
			this._ilg.Emit(OpCodes.Ldlen);
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00006FD6 File Offset: 0x00005FD6
		internal void Rightchar()
		{
			this.Ldloc(this._textV);
			this.Ldloc(this._textposV);
			this.Callvirt(RegexCompiler._getcharM);
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00006FFC File Offset: 0x00005FFC
		internal void Rightcharnext()
		{
			this.Ldloc(this._textV);
			this.Ldloc(this._textposV);
			this.Dup();
			this.Ldc(1);
			this.Add();
			this.Stloc(this._textposV);
			this.Callvirt(RegexCompiler._getcharM);
		}

		// Token: 0x06000100 RID: 256 RVA: 0x0000704B File Offset: 0x0000604B
		internal void Leftchar()
		{
			this.Ldloc(this._textV);
			this.Ldloc(this._textposV);
			this.Ldc(1);
			this.Sub();
			this.Callvirt(RegexCompiler._getcharM);
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00007080 File Offset: 0x00006080
		internal void Leftcharnext()
		{
			this.Ldloc(this._textV);
			this.Ldloc(this._textposV);
			this.Ldc(1);
			this.Sub();
			this.Dup();
			this.Stloc(this._textposV);
			this.Callvirt(RegexCompiler._getcharM);
		}

		// Token: 0x06000102 RID: 258 RVA: 0x000070CF File Offset: 0x000060CF
		internal void Track()
		{
			this.ReadyPushTrack();
			this.Ldc(this.AddTrack());
			this.DoPush();
		}

		// Token: 0x06000103 RID: 259 RVA: 0x000070E9 File Offset: 0x000060E9
		internal void Trackagain()
		{
			this.ReadyPushTrack();
			this.Ldc(this._backpos);
			this.DoPush();
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00007103 File Offset: 0x00006103
		internal void PushTrack(LocalBuilder lt)
		{
			this.ReadyPushTrack();
			this.Ldloc(lt);
			this.DoPush();
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00007118 File Offset: 0x00006118
		internal void TrackUnique(int i)
		{
			this.ReadyPushTrack();
			this.Ldc(this.AddUniqueTrack(i));
			this.DoPush();
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00007133 File Offset: 0x00006133
		internal void TrackUnique2(int i)
		{
			this.ReadyPushTrack();
			this.Ldc(this.AddUniqueTrack(i, 256));
			this.DoPush();
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00007154 File Offset: 0x00006154
		internal void ReadyPushTrack()
		{
			this._ilg.Emit(OpCodes.Ldloc_S, this._trackV);
			this._ilg.Emit(OpCodes.Ldloc_S, this._trackposV);
			this._ilg.Emit(OpCodes.Ldc_I4_1);
			this._ilg.Emit(OpCodes.Sub);
			this._ilg.Emit(OpCodes.Dup);
			this._ilg.Emit(OpCodes.Stloc_S, this._trackposV);
		}

		// Token: 0x06000108 RID: 264 RVA: 0x000071D4 File Offset: 0x000061D4
		internal void PopTrack()
		{
			this._ilg.Emit(OpCodes.Ldloc_S, this._trackV);
			this._ilg.Emit(OpCodes.Ldloc_S, this._trackposV);
			this._ilg.Emit(OpCodes.Dup);
			this._ilg.Emit(OpCodes.Ldc_I4_1);
			this._ilg.Emit(OpCodes.Add);
			this._ilg.Emit(OpCodes.Stloc_S, this._trackposV);
			this._ilg.Emit(OpCodes.Ldelem_I4);
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00007263 File Offset: 0x00006263
		internal void TopTrack()
		{
			this._ilg.Emit(OpCodes.Ldloc_S, this._trackV);
			this._ilg.Emit(OpCodes.Ldloc_S, this._trackposV);
			this._ilg.Emit(OpCodes.Ldelem_I4);
		}

		// Token: 0x0600010A RID: 266 RVA: 0x000072A1 File Offset: 0x000062A1
		internal void PushStack(LocalBuilder lt)
		{
			this.ReadyPushStack();
			this._ilg.Emit(OpCodes.Ldloc_S, lt);
			this.DoPush();
		}

		// Token: 0x0600010B RID: 267 RVA: 0x000072C0 File Offset: 0x000062C0
		internal void ReadyReplaceStack(int i)
		{
			this._ilg.Emit(OpCodes.Ldloc_S, this._stackV);
			this._ilg.Emit(OpCodes.Ldloc_S, this._stackposV);
			if (i != 0)
			{
				this.Ldc(i);
				this._ilg.Emit(OpCodes.Add);
			}
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00007314 File Offset: 0x00006314
		internal void ReadyPushStack()
		{
			this._ilg.Emit(OpCodes.Ldloc_S, this._stackV);
			this._ilg.Emit(OpCodes.Ldloc_S, this._stackposV);
			this._ilg.Emit(OpCodes.Ldc_I4_1);
			this._ilg.Emit(OpCodes.Sub);
			this._ilg.Emit(OpCodes.Dup);
			this._ilg.Emit(OpCodes.Stloc_S, this._stackposV);
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00007393 File Offset: 0x00006393
		internal void TopStack()
		{
			this._ilg.Emit(OpCodes.Ldloc_S, this._stackV);
			this._ilg.Emit(OpCodes.Ldloc_S, this._stackposV);
			this._ilg.Emit(OpCodes.Ldelem_I4);
		}

		// Token: 0x0600010E RID: 270 RVA: 0x000073D4 File Offset: 0x000063D4
		internal void PopStack()
		{
			this._ilg.Emit(OpCodes.Ldloc_S, this._stackV);
			this._ilg.Emit(OpCodes.Ldloc_S, this._stackposV);
			this._ilg.Emit(OpCodes.Dup);
			this._ilg.Emit(OpCodes.Ldc_I4_1);
			this._ilg.Emit(OpCodes.Add);
			this._ilg.Emit(OpCodes.Stloc_S, this._stackposV);
			this._ilg.Emit(OpCodes.Ldelem_I4);
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00007463 File Offset: 0x00006463
		internal void PopDiscardStack()
		{
			this.PopDiscardStack(1);
		}

		// Token: 0x06000110 RID: 272 RVA: 0x0000746C File Offset: 0x0000646C
		internal void PopDiscardStack(int i)
		{
			this._ilg.Emit(OpCodes.Ldloc_S, this._stackposV);
			this.Ldc(i);
			this._ilg.Emit(OpCodes.Add);
			this._ilg.Emit(OpCodes.Stloc_S, this._stackposV);
		}

		// Token: 0x06000111 RID: 273 RVA: 0x000074BC File Offset: 0x000064BC
		internal void DoReplace()
		{
			this._ilg.Emit(OpCodes.Stelem_I4);
		}

		// Token: 0x06000112 RID: 274 RVA: 0x000074CE File Offset: 0x000064CE
		internal void DoPush()
		{
			this._ilg.Emit(OpCodes.Stelem_I4);
		}

		// Token: 0x06000113 RID: 275 RVA: 0x000074E0 File Offset: 0x000064E0
		internal void Back()
		{
			this._ilg.Emit(OpCodes.Br, this._backtrack);
		}

		// Token: 0x06000114 RID: 276 RVA: 0x000074F8 File Offset: 0x000064F8
		internal void Goto(int i)
		{
			if (i < this._codepos)
			{
				Label label = this.DefineLabel();
				this.Ldloc(this._trackposV);
				this.Ldc(this._trackcount * 4);
				this.Ble(label);
				this.Ldloc(this._stackposV);
				this.Ldc(this._trackcount * 3);
				this.BgtFar(this._labels[i]);
				this.MarkLabel(label);
				this.ReadyPushTrack();
				this.Ldc(this.AddGoto(i));
				this.DoPush();
				this.BrFar(this._backtrack);
				return;
			}
			this.BrFar(this._labels[i]);
		}

		// Token: 0x06000115 RID: 277 RVA: 0x000075AE File Offset: 0x000065AE
		internal int NextCodepos()
		{
			return this._codepos + RegexCode.OpcodeSize(this._codes[this._codepos]);
		}

		// Token: 0x06000116 RID: 278 RVA: 0x000075C9 File Offset: 0x000065C9
		internal Label AdvanceLabel()
		{
			return this._labels[this.NextCodepos()];
		}

		// Token: 0x06000117 RID: 279 RVA: 0x000075E1 File Offset: 0x000065E1
		internal void Advance()
		{
			this._ilg.Emit(OpCodes.Br, this.AdvanceLabel());
		}

		// Token: 0x06000118 RID: 280 RVA: 0x000075F9 File Offset: 0x000065F9
		internal void CallToLower()
		{
			if ((this._options & RegexOptions.CultureInvariant) != RegexOptions.None)
			{
				this.Call(RegexCompiler._getInvariantCulture);
			}
			else
			{
				this.Call(RegexCompiler._getCurrentCulture);
			}
			this.Call(RegexCompiler._chartolowerM);
		}

		// Token: 0x06000119 RID: 281 RVA: 0x0000762C File Offset: 0x0000662C
		internal void GenerateForwardSection()
		{
			this._labels = new Label[this._codes.Length];
			this._goto = new int[this._codes.Length];
			for (int i = 0; i < this._codes.Length; i += RegexCode.OpcodeSize(this._codes[i]))
			{
				this._goto[i] = -1;
				this._labels[i] = this._ilg.DefineLabel();
			}
			this._uniquenote = new int[10];
			for (int j = 0; j < 10; j++)
			{
				this._uniquenote[j] = -1;
			}
			this.Mvfldloc(RegexCompiler._textF, this._textV);
			this.Mvfldloc(RegexCompiler._textstartF, this._textstartV);
			this.Mvfldloc(RegexCompiler._textbegF, this._textbegV);
			this.Mvfldloc(RegexCompiler._textendF, this._textendV);
			this.Mvfldloc(RegexCompiler._textposF, this._textposV);
			this.Mvfldloc(RegexCompiler._trackF, this._trackV);
			this.Mvfldloc(RegexCompiler._trackposF, this._trackposV);
			this.Mvfldloc(RegexCompiler._stackF, this._stackV);
			this.Mvfldloc(RegexCompiler._stackposF, this._stackposV);
			this._backpos = -1;
			for (int i = 0; i < this._codes.Length; i += RegexCode.OpcodeSize(this._codes[i]))
			{
				this.MarkLabel(this._labels[i]);
				this._codepos = i;
				this._regexopcode = this._codes[i];
				this.GenerateOneCode();
			}
		}

		// Token: 0x0600011A RID: 282 RVA: 0x000077B8 File Offset: 0x000067B8
		internal void GenerateMiddleSection()
		{
			this.DefineLabel();
			this.MarkLabel(this._backtrack);
			this.Mvlocfld(this._trackposV, RegexCompiler._trackposF);
			this.Mvlocfld(this._stackposV, RegexCompiler._stackposF);
			this.Ldthis();
			this.Callvirt(RegexCompiler._ensurestorageM);
			this.Mvfldloc(RegexCompiler._trackposF, this._trackposV);
			this.Mvfldloc(RegexCompiler._stackposF, this._stackposV);
			this.Mvfldloc(RegexCompiler._trackF, this._trackV);
			this.Mvfldloc(RegexCompiler._stackF, this._stackV);
			this.PopTrack();
			Label[] array = new Label[this._notecount];
			for (int i = 0; i < this._notecount; i++)
			{
				array[i] = this._notes[i]._label;
			}
			this._ilg.Emit(OpCodes.Switch, array);
		}

		// Token: 0x0600011B RID: 283 RVA: 0x0000789C File Offset: 0x0000689C
		internal void GenerateBacktrackSection()
		{
			for (int i = 0; i < this._notecount; i++)
			{
				RegexCompiler.BacktrackNote backtrackNote = this._notes[i];
				if (backtrackNote._flags != 0)
				{
					this._ilg.MarkLabel(backtrackNote._label);
					this._codepos = backtrackNote._codepos;
					this._backpos = i;
					this._regexopcode = this._codes[backtrackNote._codepos] | backtrackNote._flags;
					this.GenerateOneCode();
				}
			}
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00007910 File Offset: 0x00006910
		internal void GenerateFindFirstChar()
		{
			/*
An exception occurred when decompiling this method (0600011C)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Void System.Text.RegularExpressions.RegexCompiler::GenerateFindFirstChar()

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.ConvertToAst(List`1 body) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 1045
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.ConvertToAst(List`1 body, HashSet`1 ehs) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 959
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.Build(MethodDef methodDef, Boolean optimize, DecompilerContext context) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 280
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 117
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00008455 File Offset: 0x00007455
		internal void GenerateInitTrackCount()
		{
			this.Ldthis();
			this.Ldc(this._trackcount);
			this.Stfld(RegexCompiler._trackcountF);
			this.Ret();
		}

		// Token: 0x0600011E RID: 286 RVA: 0x0000847A File Offset: 0x0000747A
		internal LocalBuilder DeclareInt()
		{
			return this._ilg.DeclareLocal(typeof(int));
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00008491 File Offset: 0x00007491
		internal LocalBuilder DeclareIntArray()
		{
			return this._ilg.DeclareLocal(typeof(int[]));
		}

		// Token: 0x06000120 RID: 288 RVA: 0x000084A8 File Offset: 0x000074A8
		internal LocalBuilder DeclareString()
		{
			return this._ilg.DeclareLocal(typeof(string));
		}

		// Token: 0x06000121 RID: 289 RVA: 0x000084C0 File Offset: 0x000074C0
		internal void GenerateGo()
		{
			this._textposV = this.DeclareInt();
			this._textV = this.DeclareString();
			this._trackposV = this.DeclareInt();
			this._trackV = this.DeclareIntArray();
			this._stackposV = this.DeclareInt();
			this._stackV = this.DeclareIntArray();
			this._tempV = this.DeclareInt();
			this._temp2V = this.DeclareInt();
			this._temp3V = this.DeclareInt();
			this._textbegV = this.DeclareInt();
			this._textendV = this.DeclareInt();
			this._textstartV = this.DeclareInt();
			this._labels = null;
			this._notes = null;
			this._notecount = 0;
			this._backtrack = this.DefineLabel();
			this.GenerateForwardSection();
			this.GenerateMiddleSection();
			this.GenerateBacktrackSection();
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00008590 File Offset: 0x00007590
		internal void GenerateOneCode()
		{
			int regexopcode = this._regexopcode;
			if (regexopcode <= 285)
			{
				if (regexopcode <= 164)
				{
					switch (regexopcode)
					{
					case 0:
					case 1:
					case 2:
					case 64:
					case 65:
					case 66:
						goto IL_1462;
					case 3:
					case 4:
					case 5:
					case 67:
					case 68:
					case 69:
						goto IL_162C;
					case 6:
					case 7:
					case 8:
					case 70:
					case 71:
					case 72:
						goto IL_1915;
					case 9:
					case 10:
					case 11:
					case 73:
					case 74:
					case 75:
						break;
					case 12:
						goto IL_104E;
					case 13:
					case 77:
						goto IL_1220;
					case 14:
					{
						Label label = this._labels[this.NextCodepos()];
						this.Ldloc(this._textposV);
						this.Ldloc(this._textbegV);
						this.Ble(label);
						this.Leftchar();
						this.Ldc(10);
						this.BneFar(this._backtrack);
						return;
					}
					case 15:
					{
						Label label2 = this._labels[this.NextCodepos()];
						this.Ldloc(this._textposV);
						this.Ldloc(this._textendV);
						this.Bge(label2);
						this.Rightchar();
						this.Ldc(10);
						this.BneFar(this._backtrack);
						return;
					}
					case 16:
					case 17:
						this.Ldthis();
						this.Ldloc(this._textposV);
						this.Ldloc(this._textbegV);
						this.Ldloc(this._textendV);
						this.Callvirt(RegexCompiler._isboundaryM);
						if (this.Code() == 16)
						{
							this.BrfalseFar(this._backtrack);
							return;
						}
						this.BrtrueFar(this._backtrack);
						return;
					case 18:
						this.Ldloc(this._textposV);
						this.Ldloc(this._textbegV);
						this.BgtFar(this._backtrack);
						return;
					case 19:
						this.Ldloc(this._textposV);
						this.Ldthisfld(RegexCompiler._textstartF);
						this.BneFar(this._backtrack);
						return;
					case 20:
						this.Ldloc(this._textposV);
						this.Ldloc(this._textendV);
						this.Ldc(1);
						this.Sub();
						this.BltFar(this._backtrack);
						this.Ldloc(this._textposV);
						this.Ldloc(this._textendV);
						this.Bge(this._labels[this.NextCodepos()]);
						this.Rightchar();
						this.Ldc(10);
						this.BneFar(this._backtrack);
						return;
					case 21:
						this.Ldloc(this._textposV);
						this.Ldloc(this._textendV);
						this.BltFar(this._backtrack);
						return;
					case 22:
						this.Back();
						return;
					case 23:
						this.PushTrack(this._textposV);
						this.Track();
						return;
					case 24:
					{
						LocalBuilder tempV = this._tempV;
						Label label3 = this.DefineLabel();
						this.PopStack();
						this.Dup();
						this.Stloc(tempV);
						this.PushTrack(tempV);
						this.Ldloc(this._textposV);
						this.Beq(label3);
						this.PushTrack(this._textposV);
						this.PushStack(this._textposV);
						this.Track();
						this.Goto(this.Operand(0));
						this.MarkLabel(label3);
						this.TrackUnique2(5);
						return;
					}
					case 25:
					{
						LocalBuilder tempV2 = this._tempV;
						Label label4 = this.DefineLabel();
						Label label5 = this.DefineLabel();
						Label label6 = this.DefineLabel();
						this.PopStack();
						this.Dup();
						this.Stloc(tempV2);
						this.Ldloc(tempV2);
						this.Ldc(-1);
						this.Beq(label5);
						this.PushTrack(tempV2);
						this.Br(label6);
						this.MarkLabel(label5);
						this.PushTrack(this._textposV);
						this.MarkLabel(label6);
						this.Ldloc(this._textposV);
						this.Beq(label4);
						this.PushTrack(this._textposV);
						this.Track();
						this.Br(this.AdvanceLabel());
						this.MarkLabel(label4);
						this.ReadyPushStack();
						this.Ldloc(tempV2);
						this.DoPush();
						this.TrackUnique2(6);
						return;
					}
					case 26:
						this.ReadyPushStack();
						this.Ldc(-1);
						this.DoPush();
						this.ReadyPushStack();
						this.Ldc(this.Operand(0));
						this.DoPush();
						this.TrackUnique(1);
						return;
					case 27:
						this.PushStack(this._textposV);
						this.ReadyPushStack();
						this.Ldc(this.Operand(0));
						this.DoPush();
						this.TrackUnique(1);
						return;
					case 28:
					{
						LocalBuilder tempV3 = this._tempV;
						LocalBuilder temp2V = this._temp2V;
						Label label7 = this.DefineLabel();
						Label label8 = this.DefineLabel();
						this.PopStack();
						this.Stloc(tempV3);
						this.PopStack();
						this.Dup();
						this.Stloc(temp2V);
						this.PushTrack(temp2V);
						this.Ldloc(this._textposV);
						this.Bne(label7);
						this.Ldloc(tempV3);
						this.Ldc(0);
						this.Bge(label8);
						this.MarkLabel(label7);
						this.Ldloc(tempV3);
						this.Ldc(this.Operand(1));
						this.Bge(label8);
						this.PushStack(this._textposV);
						this.ReadyPushStack();
						this.Ldloc(tempV3);
						this.Ldc(1);
						this.Add();
						this.DoPush();
						this.Track();
						this.Goto(this.Operand(0));
						this.MarkLabel(label8);
						this.PushTrack(tempV3);
						this.TrackUnique2(7);
						return;
					}
					case 29:
					{
						LocalBuilder tempV4 = this._tempV;
						LocalBuilder temp2V2 = this._temp2V;
						Label label9 = this.DefineLabel();
						this.DefineLabel();
						Label label10 = this._labels[this.NextCodepos()];
						this.PopStack();
						this.Stloc(tempV4);
						this.PopStack();
						this.Stloc(temp2V2);
						this.Ldloc(tempV4);
						this.Ldc(0);
						this.Bge(label9);
						this.PushTrack(temp2V2);
						this.PushStack(this._textposV);
						this.ReadyPushStack();
						this.Ldloc(tempV4);
						this.Ldc(1);
						this.Add();
						this.DoPush();
						this.TrackUnique2(8);
						this.Goto(this.Operand(0));
						this.MarkLabel(label9);
						this.PushTrack(temp2V2);
						this.PushTrack(tempV4);
						this.PushTrack(this._textposV);
						this.Track();
						return;
					}
					case 30:
						this.ReadyPushStack();
						this.Ldc(-1);
						this.DoPush();
						this.TrackUnique(0);
						return;
					case 31:
						this.PushStack(this._textposV);
						this.TrackUnique(0);
						return;
					case 32:
						if (this.Operand(1) != -1)
						{
							this.Ldthis();
							this.Ldc(this.Operand(1));
							this.Callvirt(RegexCompiler._ismatchedM);
							this.BrfalseFar(this._backtrack);
						}
						this.PopStack();
						this.Stloc(this._tempV);
						if (this.Operand(1) != -1)
						{
							this.Ldthis();
							this.Ldc(this.Operand(0));
							this.Ldc(this.Operand(1));
							this.Ldloc(this._tempV);
							this.Ldloc(this._textposV);
							this.Callvirt(RegexCompiler._transferM);
						}
						else
						{
							this.Ldthis();
							this.Ldc(this.Operand(0));
							this.Ldloc(this._tempV);
							this.Ldloc(this._textposV);
							this.Callvirt(RegexCompiler._captureM);
						}
						this.PushTrack(this._tempV);
						if (this.Operand(0) != -1 && this.Operand(1) != -1)
						{
							this.TrackUnique(4);
							return;
						}
						this.TrackUnique(3);
						return;
					case 33:
						this.ReadyPushTrack();
						this.PopStack();
						this.Dup();
						this.Stloc(this._textposV);
						this.DoPush();
						this.Track();
						return;
					case 34:
						this.ReadyPushStack();
						this.Ldthisfld(RegexCompiler._trackF);
						this.Ldlen();
						this.Ldloc(this._trackposV);
						this.Sub();
						this.DoPush();
						this.ReadyPushStack();
						this.Ldthis();
						this.Callvirt(RegexCompiler._crawlposM);
						this.DoPush();
						this.TrackUnique(1);
						return;
					case 35:
					{
						Label label11 = this.DefineLabel();
						Label label12 = this.DefineLabel();
						this.PopStack();
						this.Ldthisfld(RegexCompiler._trackF);
						this.Ldlen();
						this.PopStack();
						this.Sub();
						this.Stloc(this._trackposV);
						this.Dup();
						this.Ldthis();
						this.Callvirt(RegexCompiler._crawlposM);
						this.Beq(label12);
						this.MarkLabel(label11);
						this.Ldthis();
						this.Callvirt(RegexCompiler._uncaptureM);
						this.Dup();
						this.Ldthis();
						this.Callvirt(RegexCompiler._crawlposM);
						this.Bne(label11);
						this.MarkLabel(label12);
						this.Pop();
						this.Back();
						return;
					}
					case 36:
						this.PopStack();
						this.Stloc(this._tempV);
						this.Ldthisfld(RegexCompiler._trackF);
						this.Ldlen();
						this.PopStack();
						this.Sub();
						this.Stloc(this._trackposV);
						this.PushTrack(this._tempV);
						this.TrackUnique(9);
						return;
					case 37:
						this.Ldthis();
						this.Ldc(this.Operand(0));
						this.Callvirt(RegexCompiler._ismatchedM);
						this.BrfalseFar(this._backtrack);
						return;
					case 38:
						this.Goto(this.Operand(0));
						return;
					case 39:
					case 43:
					case 44:
					case 45:
					case 46:
					case 47:
					case 48:
					case 49:
					case 50:
					case 51:
					case 52:
					case 53:
					case 54:
					case 55:
					case 56:
					case 57:
					case 58:
					case 59:
					case 60:
					case 61:
					case 62:
					case 63:
						goto IL_1B08;
					case 40:
						this.Mvlocfld(this._textposV, RegexCompiler._textposF);
						this.Ret();
						return;
					case 41:
					case 42:
						this.Ldthis();
						this.Ldloc(this._textposV);
						this.Ldloc(this._textbegV);
						this.Ldloc(this._textendV);
						this.Callvirt(RegexCompiler._isECMABoundaryM);
						if (this.Code() == 41)
						{
							this.BrfalseFar(this._backtrack);
							return;
						}
						this.BrtrueFar(this._backtrack);
						return;
					case 76:
						goto IL_1135;
					default:
						switch (regexopcode)
						{
						case 131:
						case 132:
						case 133:
							goto IL_1875;
						case 134:
						case 135:
						case 136:
							goto IL_19FD;
						case 137:
						case 138:
						case 139:
						case 140:
						case 141:
						case 142:
						case 143:
						case 144:
						case 145:
						case 146:
						case 147:
						case 148:
						case 149:
						case 150:
						case 163:
							goto IL_1B08;
						case 151:
							this.PopTrack();
							this.Stloc(this._textposV);
							this.Goto(this.Operand(0));
							return;
						case 152:
							this.PopTrack();
							this.Stloc(this._textposV);
							this.PopStack();
							this.Pop();
							this.TrackUnique2(5);
							this.Advance();
							return;
						case 153:
							this.PopTrack();
							this.Stloc(this._textposV);
							this.PushStack(this._textposV);
							this.TrackUnique2(6);
							this.Goto(this.Operand(0));
							return;
						case 154:
						case 155:
							this.PopDiscardStack(2);
							this.Back();
							return;
						case 156:
						{
							LocalBuilder tempV5 = this._tempV;
							Label label13 = this.DefineLabel();
							this.PopStack();
							this.Ldc(1);
							this.Sub();
							this.Dup();
							this.Stloc(tempV5);
							this.Ldc(0);
							this.Blt(label13);
							this.PopStack();
							this.Stloc(this._textposV);
							this.PushTrack(tempV5);
							this.TrackUnique2(7);
							this.Advance();
							this.MarkLabel(label13);
							this.ReadyReplaceStack(0);
							this.PopTrack();
							this.DoReplace();
							this.PushStack(tempV5);
							this.Back();
							return;
						}
						case 157:
						{
							Label label14 = this.DefineLabel();
							LocalBuilder tempV6 = this._tempV;
							this.PopTrack();
							this.Stloc(this._textposV);
							this.PopTrack();
							this.Dup();
							this.Stloc(tempV6);
							this.Ldc(this.Operand(1));
							this.Bgt(label14);
							this.Ldloc(this._textposV);
							this.TopTrack();
							this.Beq(label14);
							this.PushStack(this._textposV);
							this.ReadyPushStack();
							this.Ldloc(tempV6);
							this.Ldc(1);
							this.Add();
							this.DoPush();
							this.TrackUnique2(8);
							this.Goto(this.Operand(0));
							this.MarkLabel(label14);
							this.ReadyPushStack();
							this.PopTrack();
							this.DoPush();
							this.PushStack(tempV6);
							this.Back();
							return;
						}
						case 158:
						case 159:
							this.PopDiscardStack();
							this.Back();
							return;
						case 160:
							this.ReadyPushStack();
							this.PopTrack();
							this.DoPush();
							this.Ldthis();
							this.Callvirt(RegexCompiler._uncaptureM);
							if (this.Operand(0) != -1 && this.Operand(1) != -1)
							{
								this.Ldthis();
								this.Callvirt(RegexCompiler._uncaptureM);
							}
							this.Back();
							return;
						case 161:
							this.ReadyPushStack();
							this.PopTrack();
							this.DoPush();
							this.Back();
							return;
						case 162:
							this.PopDiscardStack(2);
							this.Back();
							return;
						case 164:
						{
							Label label15 = this.DefineLabel();
							Label label16 = this.DefineLabel();
							this.PopTrack();
							this.Dup();
							this.Ldthis();
							this.Callvirt(RegexCompiler._crawlposM);
							this.Beq(label16);
							this.MarkLabel(label15);
							this.Ldthis();
							this.Callvirt(RegexCompiler._uncaptureM);
							this.Dup();
							this.Ldthis();
							this.Callvirt(RegexCompiler._crawlposM);
							this.Bne(label15);
							this.MarkLabel(label16);
							this.Pop();
							this.Back();
							return;
						}
						default:
							goto IL_1B08;
						}
						break;
					}
				}
				else
				{
					switch (regexopcode)
					{
					case 195:
					case 196:
					case 197:
						goto IL_1875;
					case 198:
					case 199:
					case 200:
						goto IL_19FD;
					default:
						switch (regexopcode)
						{
						case 280:
							this.ReadyPushStack();
							this.PopTrack();
							this.DoPush();
							this.Back();
							return;
						case 281:
							this.ReadyReplaceStack(0);
							this.PopTrack();
							this.DoReplace();
							this.Back();
							return;
						case 282:
						case 283:
							goto IL_1B08;
						case 284:
							this.PopTrack();
							this.Stloc(this._tempV);
							this.ReadyPushStack();
							this.PopTrack();
							this.DoPush();
							this.PushStack(this._tempV);
							this.Back();
							return;
						case 285:
							this.ReadyReplaceStack(1);
							this.PopTrack();
							this.DoReplace();
							this.ReadyReplaceStack(0);
							this.TopStack();
							this.Ldc(1);
							this.Sub();
							this.DoReplace();
							this.Back();
							return;
						default:
							goto IL_1B08;
						}
						break;
					}
				}
			}
			else if (regexopcode <= 589)
			{
				switch (regexopcode)
				{
				case 512:
				case 513:
				case 514:
					goto IL_1462;
				case 515:
				case 516:
				case 517:
					goto IL_162C;
				case 518:
				case 519:
				case 520:
					goto IL_1915;
				case 521:
				case 522:
				case 523:
					break;
				case 524:
					goto IL_104E;
				case 525:
					goto IL_1220;
				default:
					switch (regexopcode)
					{
					case 576:
					case 577:
					case 578:
						goto IL_1462;
					case 579:
					case 580:
					case 581:
						goto IL_162C;
					case 582:
					case 583:
					case 584:
						goto IL_1915;
					case 585:
					case 586:
					case 587:
						break;
					case 588:
						goto IL_1135;
					case 589:
						goto IL_1220;
					default:
						goto IL_1B08;
					}
					break;
				}
			}
			else
			{
				switch (regexopcode)
				{
				case 643:
				case 644:
				case 645:
					goto IL_1875;
				case 646:
				case 647:
				case 648:
					goto IL_19FD;
				default:
					switch (regexopcode)
					{
					case 707:
					case 708:
					case 709:
						goto IL_1875;
					case 710:
					case 711:
					case 712:
						goto IL_19FD;
					default:
						goto IL_1B08;
					}
					break;
				}
			}
			this.Ldloc(this._textposV);
			if (!this.IsRtl())
			{
				this.Ldloc(this._textendV);
				this.BgeFar(this._backtrack);
				this.Rightcharnext();
			}
			else
			{
				this.Ldloc(this._textbegV);
				this.BleFar(this._backtrack);
				this.Leftcharnext();
			}
			if (this.IsCi())
			{
				this.CallToLower();
			}
			if (this.Code() == 11)
			{
				this.Ldstr(this._strings[this.Operand(0)]);
				this.Call(RegexCompiler._charInSetM);
				this.BrfalseFar(this._backtrack);
				return;
			}
			this.Ldc(this.Operand(0));
			if (this.Code() == 9)
			{
				this.BneFar(this._backtrack);
				return;
			}
			this.BeqFar(this._backtrack);
			return;
			IL_104E:
			string text = this._strings[this.Operand(0)];
			this.Ldc(text.Length);
			this.Ldloc(this._textendV);
			this.Ldloc(this._textposV);
			this.Sub();
			this.BgtFar(this._backtrack);
			for (int i = 0; i < text.Length; i++)
			{
				this.Ldloc(this._textV);
				this.Ldloc(this._textposV);
				if (i != 0)
				{
					this.Ldc(i);
					this.Add();
				}
				this.Callvirt(RegexCompiler._getcharM);
				if (this.IsCi())
				{
					this.CallToLower();
				}
				this.Ldc((int)text[i]);
				this.BneFar(this._backtrack);
			}
			this.Ldloc(this._textposV);
			this.Ldc(text.Length);
			this.Add();
			this.Stloc(this._textposV);
			return;
			IL_1135:
			string text2 = this._strings[this.Operand(0)];
			this.Ldc(text2.Length);
			this.Ldloc(this._textposV);
			this.Ldloc(this._textbegV);
			this.Sub();
			this.BgtFar(this._backtrack);
			int j = text2.Length;
			while (j > 0)
			{
				j--;
				this.Ldloc(this._textV);
				this.Ldloc(this._textposV);
				this.Ldc(text2.Length - j);
				this.Sub();
				this.Callvirt(RegexCompiler._getcharM);
				if (this.IsCi())
				{
					this.CallToLower();
				}
				this.Ldc((int)text2[j]);
				this.BneFar(this._backtrack);
			}
			this.Ldloc(this._textposV);
			this.Ldc(text2.Length);
			this.Sub();
			this.Stloc(this._textposV);
			return;
			IL_1220:
			LocalBuilder tempV7 = this._tempV;
			LocalBuilder temp2V3 = this._temp2V;
			Label label17 = this.DefineLabel();
			this.Ldthis();
			this.Ldc(this.Operand(0));
			this.Callvirt(RegexCompiler._ismatchedM);
			if ((this._options & RegexOptions.ECMAScript) != RegexOptions.None)
			{
				this.Brfalse(this.AdvanceLabel());
			}
			else
			{
				this.BrfalseFar(this._backtrack);
			}
			this.Ldthis();
			this.Ldc(this.Operand(0));
			this.Callvirt(RegexCompiler._matchlengthM);
			this.Dup();
			this.Stloc(tempV7);
			if (!this.IsRtl())
			{
				this.Ldloc(this._textendV);
				this.Ldloc(this._textposV);
			}
			else
			{
				this.Ldloc(this._textposV);
				this.Ldloc(this._textbegV);
			}
			this.Sub();
			this.BgtFar(this._backtrack);
			this.Ldthis();
			this.Ldc(this.Operand(0));
			this.Callvirt(RegexCompiler._matchindexM);
			if (!this.IsRtl())
			{
				this.Ldloc(tempV7);
				this.Add(this.IsRtl());
			}
			this.Stloc(temp2V3);
			this.Ldloc(this._textposV);
			this.Ldloc(tempV7);
			this.Add(this.IsRtl());
			this.Stloc(this._textposV);
			this.MarkLabel(label17);
			this.Ldloc(tempV7);
			this.Ldc(0);
			this.Ble(this.AdvanceLabel());
			this.Ldloc(this._textV);
			this.Ldloc(temp2V3);
			this.Ldloc(tempV7);
			if (this.IsRtl())
			{
				this.Ldc(1);
				this.Sub();
				this.Dup();
				this.Stloc(tempV7);
			}
			this.Sub(this.IsRtl());
			this.Callvirt(RegexCompiler._getcharM);
			if (this.IsCi())
			{
				this.CallToLower();
			}
			this.Ldloc(this._textV);
			this.Ldloc(this._textposV);
			this.Ldloc(tempV7);
			if (!this.IsRtl())
			{
				this.Dup();
				this.Ldc(1);
				this.Sub();
				this.Stloc(tempV7);
			}
			this.Sub(this.IsRtl());
			this.Callvirt(RegexCompiler._getcharM);
			if (this.IsCi())
			{
				this.CallToLower();
			}
			this.Beq(label17);
			this.Back();
			return;
			IL_1462:
			LocalBuilder tempV8 = this._tempV;
			Label label18 = this.DefineLabel();
			int num = this.Operand(1);
			if (num == 0)
			{
				return;
			}
			this.Ldc(num);
			if (!this.IsRtl())
			{
				this.Ldloc(this._textendV);
				this.Ldloc(this._textposV);
			}
			else
			{
				this.Ldloc(this._textposV);
				this.Ldloc(this._textbegV);
			}
			this.Sub();
			this.BgtFar(this._backtrack);
			this.Ldloc(this._textposV);
			this.Ldc(num);
			this.Add(this.IsRtl());
			this.Stloc(this._textposV);
			this.Ldc(num);
			this.Stloc(tempV8);
			this.MarkLabel(label18);
			this.Ldloc(this._textV);
			this.Ldloc(this._textposV);
			this.Ldloc(tempV8);
			if (this.IsRtl())
			{
				this.Ldc(1);
				this.Sub();
				this.Dup();
				this.Stloc(tempV8);
				this.Add();
			}
			else
			{
				this.Dup();
				this.Ldc(1);
				this.Sub();
				this.Stloc(tempV8);
				this.Sub();
			}
			this.Callvirt(RegexCompiler._getcharM);
			if (this.IsCi())
			{
				this.CallToLower();
			}
			if (this.Code() == 2)
			{
				this.Ldstr(this._strings[this.Operand(0)]);
				this.Call(RegexCompiler._charInSetM);
				this.BrfalseFar(this._backtrack);
			}
			else
			{
				this.Ldc(this.Operand(0));
				if (this.Code() == 0)
				{
					this.BneFar(this._backtrack);
				}
				else
				{
					this.BeqFar(this._backtrack);
				}
			}
			this.Ldloc(tempV8);
			this.Ldc(0);
			if (this.Code() == 2)
			{
				this.BgtFar(label18);
				return;
			}
			this.Bgt(label18);
			return;
			IL_162C:
			LocalBuilder tempV9 = this._tempV;
			LocalBuilder temp2V4 = this._temp2V;
			Label label19 = this.DefineLabel();
			Label label20 = this.DefineLabel();
			int num2 = this.Operand(1);
			if (num2 == 0)
			{
				return;
			}
			if (!this.IsRtl())
			{
				this.Ldloc(this._textendV);
				this.Ldloc(this._textposV);
			}
			else
			{
				this.Ldloc(this._textposV);
				this.Ldloc(this._textbegV);
			}
			this.Sub();
			if (num2 != 2147483647)
			{
				Label label21 = this.DefineLabel();
				this.Dup();
				this.Ldc(num2);
				this.Blt(label21);
				this.Pop();
				this.Ldc(num2);
				this.MarkLabel(label21);
			}
			this.Dup();
			this.Stloc(temp2V4);
			this.Ldc(1);
			this.Add();
			this.Stloc(tempV9);
			this.MarkLabel(label19);
			this.Ldloc(tempV9);
			this.Ldc(1);
			this.Sub();
			this.Dup();
			this.Stloc(tempV9);
			this.Ldc(0);
			if (this.Code() == 5)
			{
				this.BleFar(label20);
			}
			else
			{
				this.Ble(label20);
			}
			if (this.IsRtl())
			{
				this.Leftcharnext();
			}
			else
			{
				this.Rightcharnext();
			}
			if (this.IsCi())
			{
				this.CallToLower();
			}
			if (this.Code() == 5)
			{
				this.Ldstr(this._strings[this.Operand(0)]);
				this.Call(RegexCompiler._charInSetM);
				this.BrtrueFar(label19);
			}
			else
			{
				this.Ldc(this.Operand(0));
				if (this.Code() == 3)
				{
					this.Beq(label19);
				}
				else
				{
					this.Bne(label19);
				}
			}
			this.Ldloc(this._textposV);
			this.Ldc(1);
			this.Sub(this.IsRtl());
			this.Stloc(this._textposV);
			this.MarkLabel(label20);
			this.Ldloc(temp2V4);
			this.Ldloc(tempV9);
			this.Ble(this.AdvanceLabel());
			this.ReadyPushTrack();
			this.Ldloc(temp2V4);
			this.Ldloc(tempV9);
			this.Sub();
			this.Ldc(1);
			this.Sub();
			this.DoPush();
			this.ReadyPushTrack();
			this.Ldloc(this._textposV);
			this.Ldc(1);
			this.Sub(this.IsRtl());
			this.DoPush();
			this.Track();
			return;
			IL_1875:
			this.PopTrack();
			this.Stloc(this._textposV);
			this.PopTrack();
			this.Stloc(this._tempV);
			this.Ldloc(this._tempV);
			this.Ldc(0);
			this.BleFar(this.AdvanceLabel());
			this.ReadyPushTrack();
			this.Ldloc(this._tempV);
			this.Ldc(1);
			this.Sub();
			this.DoPush();
			this.ReadyPushTrack();
			this.Ldloc(this._textposV);
			this.Ldc(1);
			this.Sub(this.IsRtl());
			this.DoPush();
			this.Trackagain();
			this.Advance();
			return;
			IL_1915:
			LocalBuilder tempV10 = this._tempV;
			int num3 = this.Operand(1);
			if (num3 == 0)
			{
				return;
			}
			if (!this.IsRtl())
			{
				this.Ldloc(this._textendV);
				this.Ldloc(this._textposV);
			}
			else
			{
				this.Ldloc(this._textposV);
				this.Ldloc(this._textbegV);
			}
			this.Sub();
			if (num3 != 2147483647)
			{
				Label label22 = this.DefineLabel();
				this.Dup();
				this.Ldc(num3);
				this.Blt(label22);
				this.Pop();
				this.Ldc(num3);
				this.MarkLabel(label22);
			}
			this.Dup();
			this.Stloc(tempV10);
			this.Ldc(0);
			this.Ble(this.AdvanceLabel());
			this.ReadyPushTrack();
			this.Ldloc(tempV10);
			this.Ldc(1);
			this.Sub();
			this.DoPush();
			this.PushTrack(this._textposV);
			this.Track();
			return;
			IL_19FD:
			this.PopTrack();
			this.Stloc(this._textposV);
			this.PopTrack();
			this.Stloc(this._temp2V);
			if (!this.IsRtl())
			{
				this.Rightcharnext();
			}
			else
			{
				this.Leftcharnext();
			}
			if (this.IsCi())
			{
				this.CallToLower();
			}
			if (this.Code() == 8)
			{
				this.Ldstr(this._strings[this.Operand(0)]);
				this.Call(RegexCompiler._charInSetM);
				this.BrfalseFar(this._backtrack);
			}
			else
			{
				this.Ldc(this.Operand(0));
				if (this.Code() == 6)
				{
					this.BneFar(this._backtrack);
				}
				else
				{
					this.BeqFar(this._backtrack);
				}
			}
			this.Ldloc(this._temp2V);
			this.Ldc(0);
			this.BleFar(this.AdvanceLabel());
			this.ReadyPushTrack();
			this.Ldloc(this._temp2V);
			this.Ldc(1);
			this.Sub();
			this.DoPush();
			this.PushTrack(this._textposV);
			this.Trackagain();
			this.Advance();
			return;
			IL_1B08:
			throw new NotImplementedException(SR.GetString("UnimplementedState"));
		}

		// Token: 0x040006C5 RID: 1733
		internal const int stackpop = 0;

		// Token: 0x040006C6 RID: 1734
		internal const int stackpop2 = 1;

		// Token: 0x040006C7 RID: 1735
		internal const int stackpop3 = 2;

		// Token: 0x040006C8 RID: 1736
		internal const int capback = 3;

		// Token: 0x040006C9 RID: 1737
		internal const int capback2 = 4;

		// Token: 0x040006CA RID: 1738
		internal const int branchmarkback2 = 5;

		// Token: 0x040006CB RID: 1739
		internal const int lazybranchmarkback2 = 6;

		// Token: 0x040006CC RID: 1740
		internal const int branchcountback2 = 7;

		// Token: 0x040006CD RID: 1741
		internal const int lazybranchcountback2 = 8;

		// Token: 0x040006CE RID: 1742
		internal const int forejumpback = 9;

		// Token: 0x040006CF RID: 1743
		internal const int uniquecount = 10;

		// Token: 0x040006D0 RID: 1744
		internal static FieldInfo _textbegF;

		// Token: 0x040006D1 RID: 1745
		internal static FieldInfo _textendF;

		// Token: 0x040006D2 RID: 1746
		internal static FieldInfo _textstartF;

		// Token: 0x040006D3 RID: 1747
		internal static FieldInfo _textposF;

		// Token: 0x040006D4 RID: 1748
		internal static FieldInfo _textF;

		// Token: 0x040006D5 RID: 1749
		internal static FieldInfo _trackposF;

		// Token: 0x040006D6 RID: 1750
		internal static FieldInfo _trackF;

		// Token: 0x040006D7 RID: 1751
		internal static FieldInfo _stackposF;

		// Token: 0x040006D8 RID: 1752
		internal static FieldInfo _stackF;

		// Token: 0x040006D9 RID: 1753
		internal static FieldInfo _trackcountF;

		// Token: 0x040006DA RID: 1754
		internal static MethodInfo _ensurestorageM;

		// Token: 0x040006DB RID: 1755
		internal static MethodInfo _captureM;

		// Token: 0x040006DC RID: 1756
		internal static MethodInfo _transferM;

		// Token: 0x040006DD RID: 1757
		internal static MethodInfo _uncaptureM;

		// Token: 0x040006DE RID: 1758
		internal static MethodInfo _ismatchedM;

		// Token: 0x040006DF RID: 1759
		internal static MethodInfo _matchlengthM;

		// Token: 0x040006E0 RID: 1760
		internal static MethodInfo _matchindexM;

		// Token: 0x040006E1 RID: 1761
		internal static MethodInfo _isboundaryM;

		// Token: 0x040006E2 RID: 1762
		internal static MethodInfo _isECMABoundaryM;

		// Token: 0x040006E3 RID: 1763
		internal static MethodInfo _chartolowerM;

		// Token: 0x040006E4 RID: 1764
		internal static MethodInfo _getcharM;

		// Token: 0x040006E5 RID: 1765
		internal static MethodInfo _crawlposM;

		// Token: 0x040006E6 RID: 1766
		internal static MethodInfo _charInSetM;

		// Token: 0x040006E7 RID: 1767
		internal static MethodInfo _getCurrentCulture;

		// Token: 0x040006E8 RID: 1768
		internal static MethodInfo _getInvariantCulture;

		// Token: 0x040006E9 RID: 1769
		internal ILGenerator _ilg;

		// Token: 0x040006EA RID: 1770
		internal LocalBuilder _textstartV;

		// Token: 0x040006EB RID: 1771
		internal LocalBuilder _textbegV;

		// Token: 0x040006EC RID: 1772
		internal LocalBuilder _textendV;

		// Token: 0x040006ED RID: 1773
		internal LocalBuilder _textposV;

		// Token: 0x040006EE RID: 1774
		internal LocalBuilder _textV;

		// Token: 0x040006EF RID: 1775
		internal LocalBuilder _trackposV;

		// Token: 0x040006F0 RID: 1776
		internal LocalBuilder _trackV;

		// Token: 0x040006F1 RID: 1777
		internal LocalBuilder _stackposV;

		// Token: 0x040006F2 RID: 1778
		internal LocalBuilder _stackV;

		// Token: 0x040006F3 RID: 1779
		internal LocalBuilder _tempV;

		// Token: 0x040006F4 RID: 1780
		internal LocalBuilder _temp2V;

		// Token: 0x040006F5 RID: 1781
		internal LocalBuilder _temp3V;

		// Token: 0x040006F6 RID: 1782
		internal RegexCode _code;

		// Token: 0x040006F7 RID: 1783
		internal int[] _codes;

		// Token: 0x040006F8 RID: 1784
		internal string[] _strings;

		// Token: 0x040006F9 RID: 1785
		internal RegexPrefix _fcPrefix;

		// Token: 0x040006FA RID: 1786
		internal RegexBoyerMoore _bmPrefix;

		// Token: 0x040006FB RID: 1787
		internal int _anchors;

		// Token: 0x040006FC RID: 1788
		internal Label[] _labels;

		// Token: 0x040006FD RID: 1789
		internal RegexCompiler.BacktrackNote[] _notes;

		// Token: 0x040006FE RID: 1790
		internal int _notecount;

		// Token: 0x040006FF RID: 1791
		internal int _trackcount;

		// Token: 0x04000700 RID: 1792
		internal Label _backtrack;

		// Token: 0x04000701 RID: 1793
		internal int _regexopcode;

		// Token: 0x04000702 RID: 1794
		internal int _codepos;

		// Token: 0x04000703 RID: 1795
		internal int _backpos;

		// Token: 0x04000704 RID: 1796
		internal RegexOptions _options;

		// Token: 0x04000705 RID: 1797
		internal int[] _uniquenote;

		// Token: 0x04000706 RID: 1798
		internal int[] _goto;

		// Token: 0x0200001A RID: 26
		internal sealed class BacktrackNote
		{
			// Token: 0x06000124 RID: 292 RVA: 0x0000A0BC File Offset: 0x000090BC
			internal BacktrackNote(int flags, Label label, int codepos)
			{
				this._codepos = codepos;
				this._flags = flags;
				this._label = label;
			}

			// Token: 0x04000707 RID: 1799
			internal int _codepos;

			// Token: 0x04000708 RID: 1800
			internal int _flags;

			// Token: 0x04000709 RID: 1801
			internal Label _label;
		}
	}
}
