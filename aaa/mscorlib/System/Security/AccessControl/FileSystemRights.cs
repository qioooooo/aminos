using System;

namespace System.Security.AccessControl
{
	// Token: 0x0200090D RID: 2317
	[Flags]
	public enum FileSystemRights
	{
		// Token: 0x04002B7D RID: 11133
		ReadData = 1,
		// Token: 0x04002B7E RID: 11134
		ListDirectory = 1,
		// Token: 0x04002B7F RID: 11135
		WriteData = 2,
		// Token: 0x04002B80 RID: 11136
		CreateFiles = 2,
		// Token: 0x04002B81 RID: 11137
		AppendData = 4,
		// Token: 0x04002B82 RID: 11138
		CreateDirectories = 4,
		// Token: 0x04002B83 RID: 11139
		ReadExtendedAttributes = 8,
		// Token: 0x04002B84 RID: 11140
		WriteExtendedAttributes = 16,
		// Token: 0x04002B85 RID: 11141
		ExecuteFile = 32,
		// Token: 0x04002B86 RID: 11142
		Traverse = 32,
		// Token: 0x04002B87 RID: 11143
		DeleteSubdirectoriesAndFiles = 64,
		// Token: 0x04002B88 RID: 11144
		ReadAttributes = 128,
		// Token: 0x04002B89 RID: 11145
		WriteAttributes = 256,
		// Token: 0x04002B8A RID: 11146
		Delete = 65536,
		// Token: 0x04002B8B RID: 11147
		ReadPermissions = 131072,
		// Token: 0x04002B8C RID: 11148
		ChangePermissions = 262144,
		// Token: 0x04002B8D RID: 11149
		TakeOwnership = 524288,
		// Token: 0x04002B8E RID: 11150
		Synchronize = 1048576,
		// Token: 0x04002B8F RID: 11151
		FullControl = 2032127,
		// Token: 0x04002B90 RID: 11152
		Read = 131209,
		// Token: 0x04002B91 RID: 11153
		ReadAndExecute = 131241,
		// Token: 0x04002B92 RID: 11154
		Write = 278,
		// Token: 0x04002B93 RID: 11155
		Modify = 197055
	}
}
