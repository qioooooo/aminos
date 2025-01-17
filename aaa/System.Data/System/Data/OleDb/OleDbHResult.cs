﻿using System;

namespace System.Data.OleDb
{
	// Token: 0x0200022A RID: 554
	internal enum OleDbHResult
	{
		// Token: 0x040012E9 RID: 4841
		CO_E_CLASSSTRING = -2147221005,
		// Token: 0x040012EA RID: 4842
		REGDB_E_CLASSNOTREG = -2147221164,
		// Token: 0x040012EB RID: 4843
		CO_E_NOTINITIALIZED = -2147221008,
		// Token: 0x040012EC RID: 4844
		S_OK = 0,
		// Token: 0x040012ED RID: 4845
		S_FALSE,
		// Token: 0x040012EE RID: 4846
		E_UNEXPECTED = -2147418113,
		// Token: 0x040012EF RID: 4847
		E_NOTIMPL = -2147467263,
		// Token: 0x040012F0 RID: 4848
		E_OUTOFMEMORY = -2147024882,
		// Token: 0x040012F1 RID: 4849
		E_INVALIDARG = -2147024809,
		// Token: 0x040012F2 RID: 4850
		E_NOINTERFACE = -2147467262,
		// Token: 0x040012F3 RID: 4851
		E_POINTER,
		// Token: 0x040012F4 RID: 4852
		E_HANDLE = -2147024890,
		// Token: 0x040012F5 RID: 4853
		E_ABORT = -2147467260,
		// Token: 0x040012F6 RID: 4854
		E_FAIL,
		// Token: 0x040012F7 RID: 4855
		E_ACCESSDENIED = -2147024891,
		// Token: 0x040012F8 RID: 4856
		DB_E_BADACCESSORHANDLE = -2147217920,
		// Token: 0x040012F9 RID: 4857
		DB_E_ROWLIMITEXCEEDED,
		// Token: 0x040012FA RID: 4858
		DB_E_REOLEDBNLYACCESSOR,
		// Token: 0x040012FB RID: 4859
		DB_E_SCHEMAVIOLATION,
		// Token: 0x040012FC RID: 4860
		DB_E_BADROWHANDLE,
		// Token: 0x040012FD RID: 4861
		DB_E_OBJECTOPEN,
		// Token: 0x040012FE RID: 4862
		DB_E_BADCHAPTER,
		// Token: 0x040012FF RID: 4863
		DB_E_CANTCONVERTVALUE,
		// Token: 0x04001300 RID: 4864
		DB_E_BADBINDINFO,
		// Token: 0x04001301 RID: 4865
		DB_SEC_E_PERMISSIONDENIED,
		// Token: 0x04001302 RID: 4866
		DB_E_NOTAREFERENCECOLUMN,
		// Token: 0x04001303 RID: 4867
		DB_E_LIMITREJECTED,
		// Token: 0x04001304 RID: 4868
		DB_E_NOCOMMAND,
		// Token: 0x04001305 RID: 4869
		DB_E_COSTLIMIT,
		// Token: 0x04001306 RID: 4870
		DB_E_BADBOOKMARK,
		// Token: 0x04001307 RID: 4871
		DB_E_BADLOCKMODE,
		// Token: 0x04001308 RID: 4872
		DB_E_PARAMNOTOPTIONAL,
		// Token: 0x04001309 RID: 4873
		DB_E_BADCOLUMNID,
		// Token: 0x0400130A RID: 4874
		DB_E_BADRATIO,
		// Token: 0x0400130B RID: 4875
		DB_E_BADVALUES,
		// Token: 0x0400130C RID: 4876
		DB_E_ERRORSINCOMMAND,
		// Token: 0x0400130D RID: 4877
		DB_E_CANTCANCEL,
		// Token: 0x0400130E RID: 4878
		DB_E_DIALECTNOTSUPPORTED,
		// Token: 0x0400130F RID: 4879
		DB_E_DUPLICATEDATASOURCE,
		// Token: 0x04001310 RID: 4880
		DB_E_CANNOTRESTART,
		// Token: 0x04001311 RID: 4881
		DB_E_NOTFOUND,
		// Token: 0x04001312 RID: 4882
		DB_E_NEWLYINSERTED = -2147217893,
		// Token: 0x04001313 RID: 4883
		DB_E_CANNOTFREE = -2147217894,
		// Token: 0x04001314 RID: 4884
		DB_E_GOALREJECTED = -2147217892,
		// Token: 0x04001315 RID: 4885
		DB_E_UNSUPPORTEDCONVERSION,
		// Token: 0x04001316 RID: 4886
		DB_E_BADSTARTPOSITION,
		// Token: 0x04001317 RID: 4887
		DB_E_NOQUERY,
		// Token: 0x04001318 RID: 4888
		DB_E_NOTREENTRANT,
		// Token: 0x04001319 RID: 4889
		DB_E_ERRORSOCCURRED,
		// Token: 0x0400131A RID: 4890
		DB_E_NOAGGREGATION,
		// Token: 0x0400131B RID: 4891
		DB_E_DELETEDROW,
		// Token: 0x0400131C RID: 4892
		DB_E_CANTFETCHBACKWARDS,
		// Token: 0x0400131D RID: 4893
		DB_E_ROWSNOTRELEASED,
		// Token: 0x0400131E RID: 4894
		DB_E_BADSTORAGEFLAG,
		// Token: 0x0400131F RID: 4895
		DB_E_BADCOMPAREOP,
		// Token: 0x04001320 RID: 4896
		DB_E_BADSTATUSVALUE,
		// Token: 0x04001321 RID: 4897
		DB_E_CANTSCROLLBACKWARDS,
		// Token: 0x04001322 RID: 4898
		DB_E_BADREGIONHANDLE,
		// Token: 0x04001323 RID: 4899
		DB_E_NONCONTIGUOUSRANGE,
		// Token: 0x04001324 RID: 4900
		DB_E_INVALIDTRANSITION,
		// Token: 0x04001325 RID: 4901
		DB_E_NOTASUBREGION,
		// Token: 0x04001326 RID: 4902
		DB_E_MULTIPLESTATEMENTS,
		// Token: 0x04001327 RID: 4903
		DB_E_INTEGRITYVIOLATION,
		// Token: 0x04001328 RID: 4904
		DB_E_BADTYPENAME,
		// Token: 0x04001329 RID: 4905
		DB_E_ABORTLIMITREACHED,
		// Token: 0x0400132A RID: 4906
		DB_E_ROWSETINCOMMAND,
		// Token: 0x0400132B RID: 4907
		DB_E_CANTTRANSLATE,
		// Token: 0x0400132C RID: 4908
		DB_E_DUPLICATEINDEXID,
		// Token: 0x0400132D RID: 4909
		DB_E_NOINDEX,
		// Token: 0x0400132E RID: 4910
		DB_E_INDEXINUSE,
		// Token: 0x0400132F RID: 4911
		DB_E_NOTABLE,
		// Token: 0x04001330 RID: 4912
		DB_E_CONCURRENCYVIOLATION,
		// Token: 0x04001331 RID: 4913
		DB_E_BADCOPY,
		// Token: 0x04001332 RID: 4914
		DB_E_BADPRECISION,
		// Token: 0x04001333 RID: 4915
		DB_E_BADSCALE,
		// Token: 0x04001334 RID: 4916
		DB_E_BADTABLEID,
		// Token: 0x04001335 RID: 4917
		DB_E_BADTYPE,
		// Token: 0x04001336 RID: 4918
		DB_E_DUPLICATECOLUMNID,
		// Token: 0x04001337 RID: 4919
		DB_E_DUPLICATETABLEID,
		// Token: 0x04001338 RID: 4920
		DB_E_TABLEINUSE,
		// Token: 0x04001339 RID: 4921
		DB_E_NOLOCALE,
		// Token: 0x0400133A RID: 4922
		DB_E_BADRECORDNUM,
		// Token: 0x0400133B RID: 4923
		DB_E_BOOKMARKSKIPPED,
		// Token: 0x0400133C RID: 4924
		DB_E_BADPROPERTYVALUE,
		// Token: 0x0400133D RID: 4925
		DB_E_INVALID,
		// Token: 0x0400133E RID: 4926
		DB_E_BADACCESSORFLAGS,
		// Token: 0x0400133F RID: 4927
		DB_E_BADSTORAGEFLAGS,
		// Token: 0x04001340 RID: 4928
		DB_E_BYREFACCESSORNOTSUPPORTED,
		// Token: 0x04001341 RID: 4929
		DB_E_NULLACCESSORNOTSUPPORTED,
		// Token: 0x04001342 RID: 4930
		DB_E_NOTPREPARED,
		// Token: 0x04001343 RID: 4931
		DB_E_BADACCESSORTYPE,
		// Token: 0x04001344 RID: 4932
		DB_E_WRITEONLYACCESSOR,
		// Token: 0x04001345 RID: 4933
		DB_SEC_E_AUTH_FAILED,
		// Token: 0x04001346 RID: 4934
		DB_E_CANCELED,
		// Token: 0x04001347 RID: 4935
		DB_E_CHAPTERNOTRELEASED,
		// Token: 0x04001348 RID: 4936
		DB_E_BADSOURCEHANDLE,
		// Token: 0x04001349 RID: 4937
		DB_E_PARAMUNAVAILABLE,
		// Token: 0x0400134A RID: 4938
		DB_E_ALREADYINITIALIZED,
		// Token: 0x0400134B RID: 4939
		DB_E_NOTSUPPORTED,
		// Token: 0x0400134C RID: 4940
		DB_E_MAXPENDCHANGESEXCEEDED,
		// Token: 0x0400134D RID: 4941
		DB_E_BADORDINAL,
		// Token: 0x0400134E RID: 4942
		DB_E_PENDINGCHANGES,
		// Token: 0x0400134F RID: 4943
		DB_E_DATAOVERFLOW,
		// Token: 0x04001350 RID: 4944
		DB_E_BADHRESULT,
		// Token: 0x04001351 RID: 4945
		DB_E_BADLOOKUPID,
		// Token: 0x04001352 RID: 4946
		DB_E_BADDYNAMICERRORID,
		// Token: 0x04001353 RID: 4947
		DB_E_PENDINGINSERT,
		// Token: 0x04001354 RID: 4948
		DB_E_BADCONVERTFLAG,
		// Token: 0x04001355 RID: 4949
		DB_E_BADPARAMETERNAME,
		// Token: 0x04001356 RID: 4950
		DB_E_MULTIPLESTORAGE,
		// Token: 0x04001357 RID: 4951
		DB_E_CANTFILTER,
		// Token: 0x04001358 RID: 4952
		DB_E_CANTORDER,
		// Token: 0x04001359 RID: 4953
		MD_E_BADTUPLE,
		// Token: 0x0400135A RID: 4954
		MD_E_BADCOORDINATE,
		// Token: 0x0400135B RID: 4955
		MD_E_INVALIDAXIS,
		// Token: 0x0400135C RID: 4956
		MD_E_INVALIDCELLRANGE,
		// Token: 0x0400135D RID: 4957
		DB_E_NOCOLUMN,
		// Token: 0x0400135E RID: 4958
		DB_E_COMMANDNOTPERSISTED = -2147217817,
		// Token: 0x0400135F RID: 4959
		DB_E_DUPLICATEID,
		// Token: 0x04001360 RID: 4960
		DB_E_OBJECTCREATIONLIMITREACHED,
		// Token: 0x04001361 RID: 4961
		DB_E_BADINDEXID = -2147217806,
		// Token: 0x04001362 RID: 4962
		DB_E_BADINITSTRING,
		// Token: 0x04001363 RID: 4963
		DB_E_NOPROVIDERSREGISTERED,
		// Token: 0x04001364 RID: 4964
		DB_E_MISMATCHEDPROVIDER,
		// Token: 0x04001365 RID: 4965
		DB_E_BADCOMMANDID,
		// Token: 0x04001366 RID: 4966
		SEC_E_BADTRUSTEEID = -2147217814,
		// Token: 0x04001367 RID: 4967
		SEC_E_NOTRUSTEEID,
		// Token: 0x04001368 RID: 4968
		SEC_E_NOMEMBERSHIPSUPPORT,
		// Token: 0x04001369 RID: 4969
		SEC_E_INVALIDOBJECT,
		// Token: 0x0400136A RID: 4970
		SEC_E_NOOWNER,
		// Token: 0x0400136B RID: 4971
		SEC_E_INVALIDACCESSENTRYLIST,
		// Token: 0x0400136C RID: 4972
		SEC_E_INVALIDOWNER,
		// Token: 0x0400136D RID: 4973
		SEC_E_INVALIDACCESSENTRY,
		// Token: 0x0400136E RID: 4974
		DB_E_BADCONSTRAINTTYPE = -2147217801,
		// Token: 0x0400136F RID: 4975
		DB_E_BADCONSTRAINTFORM,
		// Token: 0x04001370 RID: 4976
		DB_E_BADDEFERRABILITY,
		// Token: 0x04001371 RID: 4977
		DB_E_BADMATCHTYPE = -2147217792,
		// Token: 0x04001372 RID: 4978
		DB_E_BADUPDATEDELETERULE = -2147217782,
		// Token: 0x04001373 RID: 4979
		DB_E_BADCONSTRAINTID,
		// Token: 0x04001374 RID: 4980
		DB_E_BADCOMMANDFLAGS,
		// Token: 0x04001375 RID: 4981
		DB_E_OBJECTMISMATCH,
		// Token: 0x04001376 RID: 4982
		DB_E_NOSOURCEOBJECT = -2147217775,
		// Token: 0x04001377 RID: 4983
		DB_E_RESOURCELOCKED,
		// Token: 0x04001378 RID: 4984
		DB_E_NOTCOLLECTION,
		// Token: 0x04001379 RID: 4985
		DB_E_REOLEDBNLY,
		// Token: 0x0400137A RID: 4986
		DB_E_ASYNCNOTSUPPORTED,
		// Token: 0x0400137B RID: 4987
		DB_E_CANNOTCONNECT,
		// Token: 0x0400137C RID: 4988
		DB_E_TIMEOUT,
		// Token: 0x0400137D RID: 4989
		DB_E_RESOURCEEXISTS,
		// Token: 0x0400137E RID: 4990
		DB_E_RESOURCEOUTOFSCOPE = -2147217778,
		// Token: 0x0400137F RID: 4991
		DB_E_DROPRESTRICTED = -2147217776,
		// Token: 0x04001380 RID: 4992
		DB_E_DUPLICATECONSTRAINTID = -2147217767,
		// Token: 0x04001381 RID: 4993
		DB_E_OUTOFSPACE,
		// Token: 0x04001382 RID: 4994
		DB_SEC_E_SAFEMODE_DENIED,
		// Token: 0x04001383 RID: 4995
		DB_S_ROWLIMITEXCEEDED = 265920,
		// Token: 0x04001384 RID: 4996
		DB_S_COLUMNTYPEMISMATCH,
		// Token: 0x04001385 RID: 4997
		DB_S_TYPEINFOOVERRIDDEN,
		// Token: 0x04001386 RID: 4998
		DB_S_BOOKMARKSKIPPED,
		// Token: 0x04001387 RID: 4999
		DB_S_NONEXTROWSET = 265925,
		// Token: 0x04001388 RID: 5000
		DB_S_ENDOFROWSET,
		// Token: 0x04001389 RID: 5001
		DB_S_COMMANDREEXECUTED,
		// Token: 0x0400138A RID: 5002
		DB_S_BUFFERFULL,
		// Token: 0x0400138B RID: 5003
		DB_S_NORESULT,
		// Token: 0x0400138C RID: 5004
		DB_S_CANTRELEASE,
		// Token: 0x0400138D RID: 5005
		DB_S_GOALCHANGED,
		// Token: 0x0400138E RID: 5006
		DB_S_UNWANTEDOPERATION,
		// Token: 0x0400138F RID: 5007
		DB_S_DIALECTIGNORED,
		// Token: 0x04001390 RID: 5008
		DB_S_UNWANTEDPHASE,
		// Token: 0x04001391 RID: 5009
		DB_S_UNWANTEDREASON,
		// Token: 0x04001392 RID: 5010
		DB_S_ASYNCHRONOUS,
		// Token: 0x04001393 RID: 5011
		DB_S_COLUMNSCHANGED,
		// Token: 0x04001394 RID: 5012
		DB_S_ERRORSRETURNED,
		// Token: 0x04001395 RID: 5013
		DB_S_BADROWHANDLE,
		// Token: 0x04001396 RID: 5014
		DB_S_DELETEDROW,
		// Token: 0x04001397 RID: 5015
		DB_S_TOOMANYCHANGES,
		// Token: 0x04001398 RID: 5016
		DB_S_STOPLIMITREACHED,
		// Token: 0x04001399 RID: 5017
		DB_S_LOCKUPGRADED = 265944,
		// Token: 0x0400139A RID: 5018
		DB_S_PROPERTIESCHANGED,
		// Token: 0x0400139B RID: 5019
		DB_S_ERRORSOCCURRED,
		// Token: 0x0400139C RID: 5020
		DB_S_PARAMUNAVAILABLE,
		// Token: 0x0400139D RID: 5021
		DB_S_MULTIPLECHANGES,
		// Token: 0x0400139E RID: 5022
		DB_S_NOTSINGLETON = 265943,
		// Token: 0x0400139F RID: 5023
		DB_S_NOROWSPECIFICCOLUMNS = 265949,
		// Token: 0x040013A0 RID: 5024
		XACT_E_FIRST = -2147168256,
		// Token: 0x040013A1 RID: 5025
		XACT_E_LAST = -2147168222,
		// Token: 0x040013A2 RID: 5026
		XACT_S_FIRST = 315392,
		// Token: 0x040013A3 RID: 5027
		XACT_S_LAST = 315401,
		// Token: 0x040013A4 RID: 5028
		XACT_E_ALREADYOTHERSINGLEPHASE = -2147168256,
		// Token: 0x040013A5 RID: 5029
		XACT_E_CANTRETAIN,
		// Token: 0x040013A6 RID: 5030
		XACT_E_COMMITFAILED,
		// Token: 0x040013A7 RID: 5031
		XACT_E_COMMITPREVENTED,
		// Token: 0x040013A8 RID: 5032
		XACT_E_HEURISTICABORT,
		// Token: 0x040013A9 RID: 5033
		XACT_E_HEURISTICCOMMIT,
		// Token: 0x040013AA RID: 5034
		XACT_E_HEURISTICDAMAGE,
		// Token: 0x040013AB RID: 5035
		XACT_E_HEURISTICDANGER,
		// Token: 0x040013AC RID: 5036
		XACT_E_ISOLATIONLEVEL,
		// Token: 0x040013AD RID: 5037
		XACT_E_NOASYNC,
		// Token: 0x040013AE RID: 5038
		XACT_E_NOENLIST,
		// Token: 0x040013AF RID: 5039
		XACT_E_NOISORETAIN,
		// Token: 0x040013B0 RID: 5040
		XACT_E_NORESOURCE,
		// Token: 0x040013B1 RID: 5041
		XACT_E_NOTCURRENT,
		// Token: 0x040013B2 RID: 5042
		XACT_E_NOTRANSACTION,
		// Token: 0x040013B3 RID: 5043
		XACT_E_NOTSUPPORTED,
		// Token: 0x040013B4 RID: 5044
		XACT_E_UNKNOWNRMGRID,
		// Token: 0x040013B5 RID: 5045
		XACT_E_WRONGSTATE,
		// Token: 0x040013B6 RID: 5046
		XACT_E_WRONGUOW,
		// Token: 0x040013B7 RID: 5047
		XACT_E_XTIONEXISTS,
		// Token: 0x040013B8 RID: 5048
		XACT_E_NOIMPORTOBJECT,
		// Token: 0x040013B9 RID: 5049
		XACT_E_INVALIDCOOKIE,
		// Token: 0x040013BA RID: 5050
		XACT_E_INDOUBT,
		// Token: 0x040013BB RID: 5051
		XACT_E_NOTIMEOUT,
		// Token: 0x040013BC RID: 5052
		XACT_E_ALREADYINPROGRESS,
		// Token: 0x040013BD RID: 5053
		XACT_E_ABORTED,
		// Token: 0x040013BE RID: 5054
		XACT_E_LOGFULL,
		// Token: 0x040013BF RID: 5055
		XACT_E_TMNOTAVAILABLE,
		// Token: 0x040013C0 RID: 5056
		XACT_E_CONNECTION_DOWN,
		// Token: 0x040013C1 RID: 5057
		XACT_E_CONNECTION_DENIED,
		// Token: 0x040013C2 RID: 5058
		XACT_E_REENLISTTIMEOUT,
		// Token: 0x040013C3 RID: 5059
		XACT_E_TIP_CONNECT_FAILED,
		// Token: 0x040013C4 RID: 5060
		XACT_E_TIP_PROTOCOL_ERROR,
		// Token: 0x040013C5 RID: 5061
		XACT_E_TIP_PULL_FAILED,
		// Token: 0x040013C6 RID: 5062
		XACT_E_DEST_TMNOTAVAILABLE,
		// Token: 0x040013C7 RID: 5063
		XACT_E_CLERKNOTFOUND = -2147168128,
		// Token: 0x040013C8 RID: 5064
		XACT_E_CLERKEXISTS,
		// Token: 0x040013C9 RID: 5065
		XACT_E_RECOVERYINPROGRESS,
		// Token: 0x040013CA RID: 5066
		XACT_E_TRANSACTIONCLOSED,
		// Token: 0x040013CB RID: 5067
		XACT_E_INVALIDLSN,
		// Token: 0x040013CC RID: 5068
		XACT_E_REPLAYREQUEST,
		// Token: 0x040013CD RID: 5069
		XACT_S_ASYNC = 315392,
		// Token: 0x040013CE RID: 5070
		XACT_S_DEFECT,
		// Token: 0x040013CF RID: 5071
		XACT_S_REOLEDBNLY,
		// Token: 0x040013D0 RID: 5072
		XACT_S_SOMENORETAIN,
		// Token: 0x040013D1 RID: 5073
		XACT_S_OKINFORM,
		// Token: 0x040013D2 RID: 5074
		XACT_S_MADECHANGESCONTENT,
		// Token: 0x040013D3 RID: 5075
		XACT_S_MADECHANGESINFORM,
		// Token: 0x040013D4 RID: 5076
		XACT_S_ALLNORETAIN,
		// Token: 0x040013D5 RID: 5077
		XACT_S_ABORTING,
		// Token: 0x040013D6 RID: 5078
		XACT_S_SINGLEPHASE,
		// Token: 0x040013D7 RID: 5079
		STG_E_INVALIDFUNCTION = -2147287039,
		// Token: 0x040013D8 RID: 5080
		STG_E_FILENOTFOUND,
		// Token: 0x040013D9 RID: 5081
		STG_E_PATHNOTFOUND,
		// Token: 0x040013DA RID: 5082
		STG_E_TOOMANYOPENFILES,
		// Token: 0x040013DB RID: 5083
		STG_E_ACCESSDENIED,
		// Token: 0x040013DC RID: 5084
		STG_E_INVALIDHANDLE,
		// Token: 0x040013DD RID: 5085
		STG_E_INSUFFICIENTMEMORY = -2147287032,
		// Token: 0x040013DE RID: 5086
		STG_E_INVALIDPOINTER,
		// Token: 0x040013DF RID: 5087
		STG_E_NOMOREFILES = -2147287022,
		// Token: 0x040013E0 RID: 5088
		STG_E_DISKISWRITEPROTECTED,
		// Token: 0x040013E1 RID: 5089
		STG_E_SEEKERROR = -2147287015,
		// Token: 0x040013E2 RID: 5090
		STG_E_WRITEFAULT = -2147287011,
		// Token: 0x040013E3 RID: 5091
		STG_E_READFAULT,
		// Token: 0x040013E4 RID: 5092
		STG_E_SHAREVIOLATION = -2147287008,
		// Token: 0x040013E5 RID: 5093
		STG_E_LOCKVIOLATION,
		// Token: 0x040013E6 RID: 5094
		STG_E_FILEALREADYEXISTS = -2147286960,
		// Token: 0x040013E7 RID: 5095
		STG_E_INVALIDPARAMETER = -2147286953,
		// Token: 0x040013E8 RID: 5096
		STG_E_MEDIUMFULL = -2147286928,
		// Token: 0x040013E9 RID: 5097
		STG_E_PROPSETMISMATCHED = -2147286800,
		// Token: 0x040013EA RID: 5098
		STG_E_ABNORMALAPIEXIT = -2147286790,
		// Token: 0x040013EB RID: 5099
		STG_E_INVALIDHEADER,
		// Token: 0x040013EC RID: 5100
		STG_E_INVALIDNAME,
		// Token: 0x040013ED RID: 5101
		STG_E_UNKNOWN,
		// Token: 0x040013EE RID: 5102
		STG_E_UNIMPLEMENTEDFUNCTION,
		// Token: 0x040013EF RID: 5103
		STG_E_INVALIDFLAG,
		// Token: 0x040013F0 RID: 5104
		STG_E_INUSE,
		// Token: 0x040013F1 RID: 5105
		STG_E_NOTCURRENT,
		// Token: 0x040013F2 RID: 5106
		STG_E_REVERTED,
		// Token: 0x040013F3 RID: 5107
		STG_E_CANTSAVE,
		// Token: 0x040013F4 RID: 5108
		STG_E_OLDFORMAT,
		// Token: 0x040013F5 RID: 5109
		STG_E_OLDDLL,
		// Token: 0x040013F6 RID: 5110
		STG_E_SHAREREQUIRED,
		// Token: 0x040013F7 RID: 5111
		STG_E_NOTFILEBASEDSTORAGE,
		// Token: 0x040013F8 RID: 5112
		STG_E_EXTANTMARSHALLINGS,
		// Token: 0x040013F9 RID: 5113
		STG_E_DOCFILECORRUPT,
		// Token: 0x040013FA RID: 5114
		STG_E_BADBASEADDRESS = -2147286768,
		// Token: 0x040013FB RID: 5115
		STG_E_INCOMPLETE = -2147286527,
		// Token: 0x040013FC RID: 5116
		STG_E_TERMINATED,
		// Token: 0x040013FD RID: 5117
		STG_S_CONVERTED = 197120,
		// Token: 0x040013FE RID: 5118
		STG_S_BLOCK,
		// Token: 0x040013FF RID: 5119
		STG_S_RETRYNOW,
		// Token: 0x04001400 RID: 5120
		STG_S_MONITORING
	}
}
