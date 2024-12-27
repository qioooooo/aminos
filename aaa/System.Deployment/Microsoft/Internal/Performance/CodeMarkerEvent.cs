using System;

namespace Microsoft.Internal.Performance
{
	// Token: 0x020001EC RID: 492
	internal enum CodeMarkerEvent
	{
		// Token: 0x04000867 RID: 2151
		perfBeginSession = 1,
		// Token: 0x04000868 RID: 2152
		perfSeatBeltEndSession,
		// Token: 0x04000869 RID: 2153
		perfCPUFrequency,
		// Token: 0x0400086A RID: 2154
		perfInitOverheadStart,
		// Token: 0x0400086B RID: 2155
		perfInitOverheadStop,
		// Token: 0x0400086C RID: 2156
		perfUnInitOverheadStart,
		// Token: 0x0400086D RID: 2157
		perfUnInitOverheadStop,
		// Token: 0x0400086E RID: 2158
		perfCalibrate1,
		// Token: 0x0400086F RID: 2159
		perfCalibrate2,
		// Token: 0x04000870 RID: 2160
		perfREBOOT,
		// Token: 0x04000871 RID: 2161
		perfBootStart = 500,
		// Token: 0x04000872 RID: 2162
		perfBootStop,
		// Token: 0x04000873 RID: 2163
		perfIdle,
		// Token: 0x04000874 RID: 2164
		perfOpenStart,
		// Token: 0x04000875 RID: 2165
		perfOpenEnd,
		// Token: 0x04000876 RID: 2166
		perfPrintStart,
		// Token: 0x04000877 RID: 2167
		perfPrintEnd,
		// Token: 0x04000878 RID: 2168
		perfSaveStart,
		// Token: 0x04000879 RID: 2169
		perfSaveEnd,
		// Token: 0x0400087A RID: 2170
		perfNextSlide,
		// Token: 0x0400087B RID: 2171
		perfSlideShowBegin,
		// Token: 0x0400087C RID: 2172
		perfNewFileBegin,
		// Token: 0x0400087D RID: 2173
		perfDialogBegin,
		// Token: 0x0400087E RID: 2174
		perfPrintReturnControl,
		// Token: 0x0400087F RID: 2175
		perfSlideShowBackSlide,
		// Token: 0x04000880 RID: 2176
		perfOLEInsertBegin,
		// Token: 0x04000881 RID: 2177
		perfSlideViewScrollBegin,
		// Token: 0x04000882 RID: 2178
		perfNewMessageBegin,
		// Token: 0x04000883 RID: 2179
		perfNewMessageEnd,
		// Token: 0x04000884 RID: 2180
		perfNewAddrBegin,
		// Token: 0x04000885 RID: 2181
		perfNewAddrEnd,
		// Token: 0x04000886 RID: 2182
		perfNewNoteBegin,
		// Token: 0x04000887 RID: 2183
		perfNewNoteEnd,
		// Token: 0x04000888 RID: 2184
		perfNewTaskBegin,
		// Token: 0x04000889 RID: 2185
		perfNewTaskEnd,
		// Token: 0x0400088A RID: 2186
		perfNewApptBegin,
		// Token: 0x0400088B RID: 2187
		perfNewApptEnd,
		// Token: 0x0400088C RID: 2188
		perfNewDistListBegin,
		// Token: 0x0400088D RID: 2189
		perfNewDistListEnd,
		// Token: 0x0400088E RID: 2190
		perfOnDeliverMailStart,
		// Token: 0x0400088F RID: 2191
		perfOnDeliverMailStop,
		// Token: 0x04000890 RID: 2192
		perfSelectTopItemBegin,
		// Token: 0x04000891 RID: 2193
		perfSelectTopItemEnd,
		// Token: 0x04000892 RID: 2194
		perfReplyToItemBegin,
		// Token: 0x04000893 RID: 2195
		perfReplyToItemEnd,
		// Token: 0x04000894 RID: 2196
		perfOnMailSendBegin,
		// Token: 0x04000895 RID: 2197
		perfOnMailSendEnd,
		// Token: 0x04000896 RID: 2198
		perfDeleteItemBegin,
		// Token: 0x04000897 RID: 2199
		perfDeleteItemEnd,
		// Token: 0x04000898 RID: 2200
		perfOpenAttachBegin,
		// Token: 0x04000899 RID: 2201
		perfOpenAttachEnd,
		// Token: 0x0400089A RID: 2202
		perfDeleteItemInspectorBegin,
		// Token: 0x0400089B RID: 2203
		perfDeleteItemInspectorEnd,
		// Token: 0x0400089C RID: 2204
		perfCheckNameBegin,
		// Token: 0x0400089D RID: 2205
		perfCheckNameEnd,
		// Token: 0x0400089E RID: 2206
		perfEndBootAtInit,
		// Token: 0x0400089F RID: 2207
		perfChangeFolderBegin,
		// Token: 0x040008A0 RID: 2208
		perfChangeFolderEnd,
		// Token: 0x040008A1 RID: 2209
		perfExitBegin,
		// Token: 0x040008A2 RID: 2210
		perfExitEnd,
		// Token: 0x040008A3 RID: 2211
		perfMoveItemtoFolderBegin,
		// Token: 0x040008A4 RID: 2212
		perfMoveItemtoFolderEnd,
		// Token: 0x040008A5 RID: 2213
		perfEmptyDelItemsBegin,
		// Token: 0x040008A6 RID: 2214
		perfEmptyDelItemsEnd,
		// Token: 0x040008A7 RID: 2215
		perfAcceptMRBegin,
		// Token: 0x040008A8 RID: 2216
		perfAcceptMREnd,
		// Token: 0x040008A9 RID: 2217
		perfGoToDateBegin,
		// Token: 0x040008AA RID: 2218
		perfGoToDateEnd,
		// Token: 0x040008AB RID: 2219
		perfFindOrgBegin,
		// Token: 0x040008AC RID: 2220
		perfFindOrgEnd,
		// Token: 0x040008AD RID: 2221
		perfOutlookTodayBegin,
		// Token: 0x040008AE RID: 2222
		perfOutlookTodayEnd,
		// Token: 0x040008AF RID: 2223
		perfExcelRecalcBegin,
		// Token: 0x040008B0 RID: 2224
		perfExcelRecalcEnd,
		// Token: 0x040008B1 RID: 2225
		perfCopyBegin,
		// Token: 0x040008B2 RID: 2226
		perfCopyEnd,
		// Token: 0x040008B3 RID: 2227
		perfPasteBegin,
		// Token: 0x040008B4 RID: 2228
		perfPasteEnd,
		// Token: 0x040008B5 RID: 2229
		perfExcelPivotTableWizardBegin,
		// Token: 0x040008B6 RID: 2230
		perfExcelPivotTableWizardEnd,
		// Token: 0x040008B7 RID: 2231
		perfWordRepagStart,
		// Token: 0x040008B8 RID: 2232
		perfWordRepagStop,
		// Token: 0x040008B9 RID: 2233
		perfWordScrollStart,
		// Token: 0x040008BA RID: 2234
		perfWordScrollStop,
		// Token: 0x040008BB RID: 2235
		perfCutBegin,
		// Token: 0x040008BC RID: 2236
		perfCutEnd,
		// Token: 0x040008BD RID: 2237
		perfInsertBegin,
		// Token: 0x040008BE RID: 2238
		perfInsertEnd,
		// Token: 0x040008BF RID: 2239
		perfExcelRunMacroBegin,
		// Token: 0x040008C0 RID: 2240
		perfExcelRunMacroEnd,
		// Token: 0x040008C1 RID: 2241
		perfExcelClearAllBegin,
		// Token: 0x040008C2 RID: 2242
		perfExcelClearAllEnd,
		// Token: 0x040008C3 RID: 2243
		perfGroupObjBegin,
		// Token: 0x040008C4 RID: 2244
		perfGroupObjEnd,
		// Token: 0x040008C5 RID: 2245
		perfUngroupObjBegin,
		// Token: 0x040008C6 RID: 2246
		perfUngroupObjEnd,
		// Token: 0x040008C7 RID: 2247
		perfExcelScrollPaneBegin,
		// Token: 0x040008C8 RID: 2248
		perfExcelScrollPaneEnd,
		// Token: 0x040008C9 RID: 2249
		perfExcelColBestFitBegin,
		// Token: 0x040008CA RID: 2250
		perfExcelColBestFitEnd,
		// Token: 0x040008CB RID: 2251
		perfExcelDrawPaneBegin,
		// Token: 0x040008CC RID: 2252
		perfExcelDrawPaneEnd,
		// Token: 0x040008CD RID: 2253
		perfExcelDrawingCommandBegin,
		// Token: 0x040008CE RID: 2254
		perfExcelDrawingCommandEnd,
		// Token: 0x040008CF RID: 2255
		perfShowVBEBegin,
		// Token: 0x040008D0 RID: 2256
		perfShowVBEEnd,
		// Token: 0x040008D1 RID: 2257
		perfExcelDrawChartBegin,
		// Token: 0x040008D2 RID: 2258
		perfExcelDrawChartEnd,
		// Token: 0x040008D3 RID: 2259
		perfNewDatabaseBegin,
		// Token: 0x040008D4 RID: 2260
		perfNewDatabaseEnd,
		// Token: 0x040008D5 RID: 2261
		perfOpenDatabaseBegin,
		// Token: 0x040008D6 RID: 2262
		perfOpenDatabaseEnd,
		// Token: 0x040008D7 RID: 2263
		perfOpenObjectBegin,
		// Token: 0x040008D8 RID: 2264
		perfOpenObjectEnd,
		// Token: 0x040008D9 RID: 2265
		perfWizardBegin,
		// Token: 0x040008DA RID: 2266
		perfWizardEnd,
		// Token: 0x040008DB RID: 2267
		perfWizardReady,
		// Token: 0x040008DC RID: 2268
		perfBuilderBegin,
		// Token: 0x040008DD RID: 2269
		perfBuilderEnd,
		// Token: 0x040008DE RID: 2270
		perfBuilderReady,
		// Token: 0x040008DF RID: 2271
		perfFrontPageNewWebBegin,
		// Token: 0x040008E0 RID: 2272
		perfFrontPageNewWebEnd,
		// Token: 0x040008E1 RID: 2273
		perfFrontPageOpenWebBegin,
		// Token: 0x040008E2 RID: 2274
		perfFrontPageOpenWebEnd,
		// Token: 0x040008E3 RID: 2275
		perfFrontPageCloseWebBegin,
		// Token: 0x040008E4 RID: 2276
		perfFrontPageCloseWebEnd,
		// Token: 0x040008E5 RID: 2277
		perfFrontPageReportsViewBegin,
		// Token: 0x040008E6 RID: 2278
		perfFrontPageReportsViewEnd,
		// Token: 0x040008E7 RID: 2279
		perfFrontPageReportsSummaryBegin,
		// Token: 0x040008E8 RID: 2280
		perfFrontPageReportsSummaryEnd,
		// Token: 0x040008E9 RID: 2281
		perfOutlookForwardItemBegin,
		// Token: 0x040008EA RID: 2282
		perfOutlookForwardItemEnd,
		// Token: 0x040008EB RID: 2283
		perfOutlookCloseItemBegin,
		// Token: 0x040008EC RID: 2284
		perfOutlookCloseItemEnd,
		// Token: 0x040008ED RID: 2285
		perfOwcCreateObjectBegin,
		// Token: 0x040008EE RID: 2286
		perfOwcCreateObjectEnd,
		// Token: 0x040008EF RID: 2287
		perfOwcFreezeEventsBegin,
		// Token: 0x040008F0 RID: 2288
		perfOwcFreezeEventsEnd,
		// Token: 0x040008F1 RID: 2289
		perfAtlPersistPropBagLoadBegin,
		// Token: 0x040008F2 RID: 2290
		perfAtlPersistPropBagLoadEnd,
		// Token: 0x040008F3 RID: 2291
		perfOwcLoadObjectBegin,
		// Token: 0x040008F4 RID: 2292
		perfOwcLoadObjectEnd,
		// Token: 0x040008F5 RID: 2293
		perfAtlQuickActivateBegin,
		// Token: 0x040008F6 RID: 2294
		perfAtlQuickActivateEnd,
		// Token: 0x040008F7 RID: 2295
		perfOwcGenericEventInvokeBegin,
		// Token: 0x040008F8 RID: 2296
		perfOwcGenericEventInvokeEnd,
		// Token: 0x040008F9 RID: 2297
		perfAtlPersistPropBagSaveBegin,
		// Token: 0x040008FA RID: 2298
		perfAtlPersistPropBagSaveEnd,
		// Token: 0x040008FB RID: 2299
		perfAtlViewObjectDrawBegin,
		// Token: 0x040008FC RID: 2300
		perfAtlViewObjectDrawEnd,
		// Token: 0x040008FD RID: 2301
		perfOwcFinalReleaseBegin,
		// Token: 0x040008FE RID: 2302
		perfOwcFinalReleaseEnd,
		// Token: 0x040008FF RID: 2303
		perfOutlookReplyForwardOnItemBegin,
		// Token: 0x04000900 RID: 2304
		perfOutlookReplyForwardOnItemEnd,
		// Token: 0x04000901 RID: 2305
		perfExcelMoveCopySheetBegin,
		// Token: 0x04000902 RID: 2306
		perfExcelMoveCopySheetEnd,
		// Token: 0x04000903 RID: 2307
		perfWordNewStart,
		// Token: 0x04000904 RID: 2308
		perfWordNewEnd,
		// Token: 0x04000905 RID: 2309
		perfWordCountStart,
		// Token: 0x04000906 RID: 2310
		perfWordCountEnd,
		// Token: 0x04000907 RID: 2311
		perfWordOutlineViewStart,
		// Token: 0x04000908 RID: 2312
		perfWordOutlineViewEnd,
		// Token: 0x04000909 RID: 2313
		perfWordPageViewStart,
		// Token: 0x0400090A RID: 2314
		perfWordPageViewEnd,
		// Token: 0x0400090B RID: 2315
		perfWordNormalViewStart,
		// Token: 0x0400090C RID: 2316
		perfWordNormalViewEnd,
		// Token: 0x0400090D RID: 2317
		perfWordWebViewStart,
		// Token: 0x0400090E RID: 2318
		perfWordWebViewEnd,
		// Token: 0x0400090F RID: 2319
		perfWordFindNextStart,
		// Token: 0x04000910 RID: 2320
		perfWordFindNextEnd,
		// Token: 0x04000911 RID: 2321
		perfWordFindAllStart,
		// Token: 0x04000912 RID: 2322
		perfWordFindAllEnd,
		// Token: 0x04000913 RID: 2323
		perfWordReplaceAllStart,
		// Token: 0x04000914 RID: 2324
		perfWordReplaceAllEnd,
		// Token: 0x04000915 RID: 2325
		perfWordAutoFormatStart,
		// Token: 0x04000916 RID: 2326
		perfWordAutoFormatEnd,
		// Token: 0x04000917 RID: 2327
		perfWordInsPictureStart,
		// Token: 0x04000918 RID: 2328
		perfWordInsPictureEnd,
		// Token: 0x04000919 RID: 2329
		perfWordInsBookmarkStart,
		// Token: 0x0400091A RID: 2330
		perfWordInsBookmarkEnd,
		// Token: 0x0400091B RID: 2331
		perfWordInsSymbolStart,
		// Token: 0x0400091C RID: 2332
		perfWordInsSymbolEnd,
		// Token: 0x0400091D RID: 2333
		perfWordInsObjectStart,
		// Token: 0x0400091E RID: 2334
		perfWordInsObjectEnd,
		// Token: 0x0400091F RID: 2335
		perfWordInsTocStart,
		// Token: 0x04000920 RID: 2336
		perfWordInsTocEnd,
		// Token: 0x04000921 RID: 2337
		perfWordSpellStart,
		// Token: 0x04000922 RID: 2338
		perfWordSpellEnd,
		// Token: 0x04000923 RID: 2339
		perfWordGrammarStart,
		// Token: 0x04000924 RID: 2340
		perfWordGrammarEnd,
		// Token: 0x04000925 RID: 2341
		perfWordInsCommentStart,
		// Token: 0x04000926 RID: 2342
		perfWordInsCommentEnd,
		// Token: 0x04000927 RID: 2343
		perfIOLDocUploadStart,
		// Token: 0x04000928 RID: 2344
		perfIOLDocUploadEnd,
		// Token: 0x04000929 RID: 2345
		perfIOLDocDownloadStart,
		// Token: 0x0400092A RID: 2346
		perfIOLDocDownloadEnd,
		// Token: 0x0400092B RID: 2347
		perfHlinkDownloadStart,
		// Token: 0x0400092C RID: 2348
		perfHlinkDownloadEnd,
		// Token: 0x0400092D RID: 2349
		perfPostNavigateStart,
		// Token: 0x0400092E RID: 2350
		perfPostNavigateEnd,
		// Token: 0x0400092F RID: 2351
		perfNavigateStart,
		// Token: 0x04000930 RID: 2352
		perfNavigateEnd,
		// Token: 0x04000931 RID: 2353
		perfNSEDeleteStart,
		// Token: 0x04000932 RID: 2354
		perfNSEDeleteEnd,
		// Token: 0x04000933 RID: 2355
		perfNSEDragDropStart,
		// Token: 0x04000934 RID: 2356
		perfNSEDragDropEnd,
		// Token: 0x04000935 RID: 2357
		perfNSEEnumStart,
		// Token: 0x04000936 RID: 2358
		perfNSEEnumEnd,
		// Token: 0x04000937 RID: 2359
		perfOutlookSaveCloseStart,
		// Token: 0x04000938 RID: 2360
		perfOutlookSaveCloseEnd,
		// Token: 0x04000939 RID: 2361
		perfExcelBkgndErrChkStart,
		// Token: 0x0400093A RID: 2362
		perfExcelBkgndErrChkEnd,
		// Token: 0x0400093B RID: 2363
		perfOutlookSyncOSTStart,
		// Token: 0x0400093C RID: 2364
		perfOutlookSyncOSTEnd,
		// Token: 0x0400093D RID: 2365
		perfAccessCompileBegin,
		// Token: 0x0400093E RID: 2366
		perfAccessCompileEnd,
		// Token: 0x0400093F RID: 2367
		perfAccessSaveProjectBegin,
		// Token: 0x04000940 RID: 2368
		perfAccessSaveProjectEnd,
		// Token: 0x04000941 RID: 2369
		perfPhdSolidColorFillStart,
		// Token: 0x04000942 RID: 2370
		perfPhdFadeStart,
		// Token: 0x04000943 RID: 2371
		perfPhdBlurSharpStart,
		// Token: 0x04000944 RID: 2372
		perfPhdPhotoCorrectionStart,
		// Token: 0x04000945 RID: 2373
		perfPhdDesignerEffectStart,
		// Token: 0x04000946 RID: 2374
		perfPhdDrawAutoShapeStart,
		// Token: 0x04000947 RID: 2375
		perfPhdPhotoArtisticBrushStart,
		// Token: 0x04000948 RID: 2376
		perfPhdDesignerEdgeStart,
		// Token: 0x04000949 RID: 2377
		perfPhdColorCorrectStart,
		// Token: 0x0400094A RID: 2378
		perfPhdMoveStart,
		// Token: 0x0400094B RID: 2379
		perfPhdResizeStart,
		// Token: 0x0400094C RID: 2380
		perfPhdRotateStart,
		// Token: 0x0400094D RID: 2381
		perfPhd3DStart,
		// Token: 0x0400094E RID: 2382
		perfPhdInsertTextStart,
		// Token: 0x0400094F RID: 2383
		perfPhdUpdateTextStart,
		// Token: 0x04000950 RID: 2384
		perfPhdTemplatesStart,
		// Token: 0x04000951 RID: 2385
		perfPhdDocSwitchStart,
		// Token: 0x04000952 RID: 2386
		perfPhdWorkpaneStart,
		// Token: 0x04000953 RID: 2387
		perfPhdZoomStart,
		// Token: 0x04000954 RID: 2388
		perfPhdCropStart,
		// Token: 0x04000955 RID: 2389
		perfPhdCutOutStart,
		// Token: 0x04000956 RID: 2390
		perfPPTApplyTemplateStart,
		// Token: 0x04000957 RID: 2391
		perfPPTChangeView,
		// Token: 0x04000958 RID: 2392
		perfPPTAddMaster,
		// Token: 0x04000959 RID: 2393
		perfPPTDeleteMaster,
		// Token: 0x0400095A RID: 2394
		perfFrontPageWebProvisionBegin,
		// Token: 0x0400095B RID: 2395
		perfFrontPageWebProvisionEnd,
		// Token: 0x0400095C RID: 2396
		perfFrontPageEnsureFolderBegin,
		// Token: 0x0400095D RID: 2397
		perfFrontPageEnsureFolderEnd,
		// Token: 0x0400095E RID: 2398
		perfFrontPageDownloadFileBegin,
		// Token: 0x0400095F RID: 2399
		perfFrontPageDownloadFileEnd,
		// Token: 0x04000960 RID: 2400
		perfFrontPageBrowserOpBegin,
		// Token: 0x04000961 RID: 2401
		perfFrontPageBrowserOpEnd,
		// Token: 0x04000962 RID: 2402
		perfFrontPageUploadFileBegin,
		// Token: 0x04000963 RID: 2403
		perfFrontPageUploadFileEnd,
		// Token: 0x04000964 RID: 2404
		perfOfficeHlinkDialogBootBegin,
		// Token: 0x04000965 RID: 2405
		perfOfficeHlinkDialogBootEnd,
		// Token: 0x04000966 RID: 2406
		perfOfficeHlinkDialogBegin,
		// Token: 0x04000967 RID: 2407
		perfOfficeHlinkDialogReady,
		// Token: 0x04000968 RID: 2408
		perfOwcPageInteractive,
		// Token: 0x04000969 RID: 2409
		perfOutlookContactQuickFindBegin,
		// Token: 0x0400096A RID: 2410
		perfOutlookContactQuickFindEnd,
		// Token: 0x0400096B RID: 2411
		perfOfficeFileSaveDlgBegin,
		// Token: 0x0400096C RID: 2412
		perfSCPCodeVerBegin,
		// Token: 0x0400096D RID: 2413
		perfSCPCodeVerEnd,
		// Token: 0x0400096E RID: 2414
		perfFrontPageBlockingRpcBegin,
		// Token: 0x0400096F RID: 2415
		perfFrontPageBlockingRpcEnd,
		// Token: 0x04000970 RID: 2416
		perfFrontPageListUrlsBegin,
		// Token: 0x04000971 RID: 2417
		perfFrontPageListUrlsEnd,
		// Token: 0x04000972 RID: 2418
		perfFrontPageEnsureFullListBegin,
		// Token: 0x04000973 RID: 2419
		perfFrontPageEnsureFullListEnd,
		// Token: 0x04000974 RID: 2420
		perfFrontPageFolderViewBegin,
		// Token: 0x04000975 RID: 2421
		perfFrontPageFolderViewEnd,
		// Token: 0x04000976 RID: 2422
		perfFrontPageStructureViewBegin,
		// Token: 0x04000977 RID: 2423
		perfFrontPageStructureViewEnd,
		// Token: 0x04000978 RID: 2424
		perfFrontPagePageViewBegin,
		// Token: 0x04000979 RID: 2425
		perfFrontPagePageViewEnd,
		// Token: 0x0400097A RID: 2426
		perfFrontPageTodoViewBegin,
		// Token: 0x0400097B RID: 2427
		perfFrontPageTodoViewEnd,
		// Token: 0x0400097C RID: 2428
		perfFrontPageUsageViewBegin,
		// Token: 0x0400097D RID: 2429
		perfFrontPageUsageViewEnd,
		// Token: 0x0400097E RID: 2430
		perfFrontPageHyperLinkViewBegin,
		// Token: 0x0400097F RID: 2431
		perfFrontPageHyperLinkViewEnd,
		// Token: 0x04000980 RID: 2432
		perfFrontPageSaveStructureBegin,
		// Token: 0x04000981 RID: 2433
		perfFrontPageSaveStructureEnd,
		// Token: 0x04000982 RID: 2434
		perfFrontPagePutDocMetaBegin,
		// Token: 0x04000983 RID: 2435
		perfFrontPagePutDocMetaEnd,
		// Token: 0x04000984 RID: 2436
		perfFrontPageRecalcBegin,
		// Token: 0x04000985 RID: 2437
		perfFrontPageRecalcEnd,
		// Token: 0x04000986 RID: 2438
		perfFrontPageFolderExpandBegin,
		// Token: 0x04000987 RID: 2439
		perfFrontPageFolderExpandEnd,
		// Token: 0x04000988 RID: 2440
		perfFrontPageFolderContractBegin,
		// Token: 0x04000989 RID: 2441
		perfFrontPageFolderContractEnd,
		// Token: 0x0400098A RID: 2442
		perfFrontPageCrossWebFindBegin,
		// Token: 0x0400098B RID: 2443
		perfFrontPageCrossWebFindEnd,
		// Token: 0x0400098C RID: 2444
		perfFrontPageNewPageBegin,
		// Token: 0x0400098D RID: 2445
		perfFrontPageNewPageEnd,
		// Token: 0x0400098E RID: 2446
		perfFrontPageSharedBorderBegin,
		// Token: 0x0400098F RID: 2447
		perfFrontPageSharedBorderEnd,
		// Token: 0x04000990 RID: 2448
		perfFrontPageThemeBegin,
		// Token: 0x04000991 RID: 2449
		perfFrontPageThemeEnd,
		// Token: 0x04000992 RID: 2450
		perfFrontPageStructureDeletePageBegin,
		// Token: 0x04000993 RID: 2451
		perfFrontPageStructureDeletePageEnd,
		// Token: 0x04000994 RID: 2452
		perfOLViewAllProposeBegin,
		// Token: 0x04000995 RID: 2453
		perfOLViewAllProposeEnd,
		// Token: 0x04000996 RID: 2454
		perfOfficeArtZoomBegin,
		// Token: 0x04000997 RID: 2455
		perfOfficeArtZoomEnd,
		// Token: 0x04000998 RID: 2456
		PerfOfficeArtScrollBegin,
		// Token: 0x04000999 RID: 2457
		PerfOfficeArtScrollEnd,
		// Token: 0x0400099A RID: 2458
		PerfOfficeArtPasteBegin = 799,
		// Token: 0x0400099B RID: 2459
		PerfOfficeArtPasteEnd,
		// Token: 0x0400099C RID: 2460
		PerfOfficeArtRotateSelectionBegin,
		// Token: 0x0400099D RID: 2461
		PerfOfficeArtRotateSelectionEnd,
		// Token: 0x0400099E RID: 2462
		PerfOfficeArtEditSelectionBegin,
		// Token: 0x0400099F RID: 2463
		PerfOfficeArtEditSelectionEnd,
		// Token: 0x040009A0 RID: 2464
		PerfOfficeArtNudgeBegin,
		// Token: 0x040009A1 RID: 2465
		PerfOfficeArtNudgeEnd,
		// Token: 0x040009A2 RID: 2466
		PerfOfficeArtResizeBegin,
		// Token: 0x040009A3 RID: 2467
		PerfOfficeArtResizeEnd,
		// Token: 0x040009A4 RID: 2468
		PerfOLProposeNTBegin,
		// Token: 0x040009A5 RID: 2469
		PerfOLProposeNTEnd,
		// Token: 0x040009A6 RID: 2470
		perfFrontPageEditBegin,
		// Token: 0x040009A7 RID: 2471
		perfFrontPageEditEnd,
		// Token: 0x040009A8 RID: 2472
		perfFrontPageScrollBegin,
		// Token: 0x040009A9 RID: 2473
		perfFrontPageScrollEnd,
		// Token: 0x040009AA RID: 2474
		perfFrontPageTimerBegin,
		// Token: 0x040009AB RID: 2475
		perfFrontPageTimerEnd,
		// Token: 0x040009AC RID: 2476
		perfFrontPageRenameBegin,
		// Token: 0x040009AD RID: 2477
		perfFrontPageRenameEnd,
		// Token: 0x040009AE RID: 2478
		perfFrontPagePublishBegin,
		// Token: 0x040009AF RID: 2479
		perfFrontPagePublishEnd,
		// Token: 0x040009B0 RID: 2480
		perfFrontPageResizeBegin,
		// Token: 0x040009B1 RID: 2481
		perfFrontPageResizeEnd,
		// Token: 0x040009B2 RID: 2482
		perfOutlookSyncGroupBegin,
		// Token: 0x040009B3 RID: 2483
		perfOutlookSyncGroupEnd,
		// Token: 0x040009B4 RID: 2484
		perfOutlookSyncSubmitBegin,
		// Token: 0x040009B5 RID: 2485
		perfOutlookSyncSubmitEnd,
		// Token: 0x040009B6 RID: 2486
		perfOwcPostPerfInit,
		// Token: 0x040009B7 RID: 2487
		perfFrontPageClosePageBegin,
		// Token: 0x040009B8 RID: 2488
		perfFrontPageClosePageEnd,
		// Token: 0x040009B9 RID: 2489
		perfDatapageOpenStart,
		// Token: 0x040009BA RID: 2490
		perfVisioZoomStart,
		// Token: 0x040009BB RID: 2491
		perfVisioZoomEnd,
		// Token: 0x040009BC RID: 2492
		perfVisioGroupStart,
		// Token: 0x040009BD RID: 2493
		perfVisioGroupEnd,
		// Token: 0x040009BE RID: 2494
		perfVisioPrintPreviewStart,
		// Token: 0x040009BF RID: 2495
		perfVisioPrintPreviewEnd,
		// Token: 0x040009C0 RID: 2496
		perfVisioUndo,
		// Token: 0x040009C1 RID: 2497
		perfVisioRedo,
		// Token: 0x040009C2 RID: 2498
		perfVisioSelectAllBegin,
		// Token: 0x040009C3 RID: 2499
		perfVisioSelectAllEnd,
		// Token: 0x040009C4 RID: 2500
		perfVisioBatchLayoutBegin,
		// Token: 0x040009C5 RID: 2501
		perfVisioBatchLayoutEnd,
		// Token: 0x040009C6 RID: 2502
		perfVisioAddonStart,
		// Token: 0x040009C7 RID: 2503
		perfVisioAddonEnd,
		// Token: 0x040009C8 RID: 2504
		perfVisioVDXParseINodeStart,
		// Token: 0x040009C9 RID: 2505
		perfVisioVDXParseINodeEnd,
		// Token: 0x040009CA RID: 2506
		perfVisioVDXParseDOMStart,
		// Token: 0x040009CB RID: 2507
		perfVisioVDXParseDOMEnd,
		// Token: 0x040009CC RID: 2508
		perfVisioUnionStart,
		// Token: 0x040009CD RID: 2509
		perfVisioUnionEnd,
		// Token: 0x040009CE RID: 2510
		perfVisioFragmentStart,
		// Token: 0x040009CF RID: 2511
		perfVisioFragmentEnd,
		// Token: 0x040009D0 RID: 2512
		perfVisioCombineStart,
		// Token: 0x040009D1 RID: 2513
		perfVisioCombineEnd,
		// Token: 0x040009D2 RID: 2514
		perfVisioSetTextANSIStart,
		// Token: 0x040009D3 RID: 2515
		perfVisioSetTextANSIEnd,
		// Token: 0x040009D4 RID: 2516
		perfVisioInitInsertControlDlgStart,
		// Token: 0x040009D5 RID: 2517
		perfVisioInitInsertControlDlgEnd,
		// Token: 0x040009D6 RID: 2518
		perfVisioDropOnPageStart,
		// Token: 0x040009D7 RID: 2519
		perfVisioDropOnPageEnd,
		// Token: 0x040009D8 RID: 2520
		perfVisioRefreshViewStart,
		// Token: 0x040009D9 RID: 2521
		perfVisioRefreshViewEnd,
		// Token: 0x040009DA RID: 2522
		perfVisioMoveObject,
		// Token: 0x040009DB RID: 2523
		perfVisioMoveObjectEnd,
		// Token: 0x040009DC RID: 2524
		perfVisioRefreshROMStart,
		// Token: 0x040009DD RID: 2525
		perfVisioRefreshROMEnd,
		// Token: 0x040009DE RID: 2526
		perfIERenderComplete,
		// Token: 0x040009DF RID: 2527
		perfIEDone,
		// Token: 0x040009E0 RID: 2528
		perfOutlookViewChangedStart,
		// Token: 0x040009E1 RID: 2529
		perfOutlookViewChangedEnd,
		// Token: 0x040009E2 RID: 2530
		perfDesignerNewElementStart,
		// Token: 0x040009E3 RID: 2531
		perfDesignerNewElementStop,
		// Token: 0x040009E4 RID: 2532
		perfDesignerOpenFormStart,
		// Token: 0x040009E5 RID: 2533
		perfDesignerOpenFormStop,
		// Token: 0x040009E6 RID: 2534
		perfDesignerCreateFieldStart,
		// Token: 0x040009E7 RID: 2535
		perfDesignerCreateFieldStop,
		// Token: 0x040009E8 RID: 2536
		perfDesignerOpenEditorStart,
		// Token: 0x040009E9 RID: 2537
		perfDesignerOpenEditorStop,
		// Token: 0x040009EA RID: 2538
		perfDesignerNewAppStart,
		// Token: 0x040009EB RID: 2539
		perfDesignerNewAppStop,
		// Token: 0x040009EC RID: 2540
		perfOutlookSearchFolderSearchStart,
		// Token: 0x040009ED RID: 2541
		perfOutlookSearchFolderSearchEnd,
		// Token: 0x040009EE RID: 2542
		perfWordSmartTagBkgCheckStart,
		// Token: 0x040009EF RID: 2543
		perfWordSmartTagBkgCheckEnd,
		// Token: 0x040009F0 RID: 2544
		perfWordSmartTagFrgCheckStart,
		// Token: 0x040009F1 RID: 2545
		perfWordSmartTagFrgCheckEnd,
		// Token: 0x040009F2 RID: 2546
		perfDesignerCreateProjectStart,
		// Token: 0x040009F3 RID: 2547
		perfDesignerCreateProjectEnd,
		// Token: 0x040009F4 RID: 2548
		perfDesignerOpenProjectStart,
		// Token: 0x040009F5 RID: 2549
		perfDesignerOpenProjectEnd,
		// Token: 0x040009F6 RID: 2550
		perfOutlookViewSortStart,
		// Token: 0x040009F7 RID: 2551
		perfOutlookViewSortEnd,
		// Token: 0x040009F8 RID: 2552
		perfOutlookViewScrollStart,
		// Token: 0x040009F9 RID: 2553
		perfOutlookViewScrollEnd,
		// Token: 0x040009FA RID: 2554
		perfDesignerAddFieldStart,
		// Token: 0x040009FB RID: 2555
		perfDesignerAddFieldStop,
		// Token: 0x040009FC RID: 2556
		perfDesignerBootWithProjectStop,
		// Token: 0x040009FD RID: 2557
		perfDesignerUpdateFieldStart,
		// Token: 0x040009FE RID: 2558
		perfDesignerUpdateFieldStop,
		// Token: 0x040009FF RID: 2559
		perfDesignerLoadFieldChooserStart,
		// Token: 0x04000A00 RID: 2560
		perfDesignerLoadFieldChooserStop,
		// Token: 0x04000A01 RID: 2561
		perfDesignerUpdateFormRegStart,
		// Token: 0x04000A02 RID: 2562
		perfDesignerUpdateFormRegStop,
		// Token: 0x04000A03 RID: 2563
		perfDesignerSyncProjectStart,
		// Token: 0x04000A04 RID: 2564
		perfDesignerSyncProjectStop,
		// Token: 0x04000A05 RID: 2565
		perfDesignerToggleOfflineStart,
		// Token: 0x04000A06 RID: 2566
		perfDesignerToggleOfflineStop,
		// Token: 0x04000A07 RID: 2567
		perfAccessSUINavBegin,
		// Token: 0x04000A08 RID: 2568
		perfAccessSUINavEnd,
		// Token: 0x04000A09 RID: 2569
		perfCloseObjectBegin,
		// Token: 0x04000A0A RID: 2570
		perfCloseObjectEnd,
		// Token: 0x04000A0B RID: 2571
		perfOwcPivotInsertFieldSetBegin,
		// Token: 0x04000A0C RID: 2572
		perfOwcPivotInsertFieldSetEnd,
		// Token: 0x04000A0D RID: 2573
		perfOutlookItemViewNextPrevBegin,
		// Token: 0x04000A0E RID: 2574
		perfOutlookItemViewNextPrevEnd,
		// Token: 0x04000A0F RID: 2575
		perfNewClientBegin,
		// Token: 0x04000A10 RID: 2576
		perfNewClientEnd,
		// Token: 0x04000A11 RID: 2577
		perfNewFileEnd,
		// Token: 0x04000A12 RID: 2578
		perfNewFrameBegin,
		// Token: 0x04000A13 RID: 2579
		perfNewFrameEnd,
		// Token: 0x04000A14 RID: 2580
		perfSubmitFormBegin,
		// Token: 0x04000A15 RID: 2581
		perfSubmitFormEnd,
		// Token: 0x04000A16 RID: 2582
		perfBLgcScriptLoadBegin,
		// Token: 0x04000A17 RID: 2583
		perfBLgcScriptLoadEnd,
		// Token: 0x04000A18 RID: 2584
		perfBLgcScriptRunBegin,
		// Token: 0x04000A19 RID: 2585
		perfBLgcScriptRunEnd,
		// Token: 0x04000A1A RID: 2586
		PerfBLgcNodeValidationBegin,
		// Token: 0x04000A1B RID: 2587
		PerfBLgcNodeValidationEnd,
		// Token: 0x04000A1C RID: 2588
		perfGITreeGenBegin,
		// Token: 0x04000A1D RID: 2589
		perfGITreeGenEnd,
		// Token: 0x04000A1E RID: 2590
		perfSolutionLoadBegin,
		// Token: 0x04000A1F RID: 2591
		perfSolutionLoadEnd,
		// Token: 0x04000A20 RID: 2592
		perfXMLUndoBegin,
		// Token: 0x04000A21 RID: 2593
		perfXMLUndoEnd,
		// Token: 0x04000A22 RID: 2594
		perfXMLRedoBegin,
		// Token: 0x04000A23 RID: 2595
		perfXMLRedoEnd,
		// Token: 0x04000A24 RID: 2596
		perfIncrementalUpdateBegin,
		// Token: 0x04000A25 RID: 2597
		perfIncrementalUpdateEnd,
		// Token: 0x04000A26 RID: 2598
		perfMSOXEVIconBegin,
		// Token: 0x04000A27 RID: 2599
		perfMSOXEVIconEnd,
		// Token: 0x04000A28 RID: 2600
		perfMSOXEVLaunchBegin,
		// Token: 0x04000A29 RID: 2601
		perfMSOXEVLaunchEnd,
		// Token: 0x04000A2A RID: 2602
		perfViewChangeBegin,
		// Token: 0x04000A2B RID: 2603
		perfViewChangeEnd,
		// Token: 0x04000A2C RID: 2604
		perfLoadGIIntoViewBegin,
		// Token: 0x04000A2D RID: 2605
		perfLoadGIIntoViewEnd,
		// Token: 0x04000A2E RID: 2606
		perfXSLReapplyHTMLUpdateBegin,
		// Token: 0x04000A2F RID: 2607
		perfXSLReapplyHTMLUpdateEnd,
		// Token: 0x04000A30 RID: 2608
		perfCalculateTokenCurrentHTMLBegin,
		// Token: 0x04000A31 RID: 2609
		perfCalculateTokenCurrentHTMLEnd,
		// Token: 0x04000A32 RID: 2610
		perfCalculateTokenNewHTMLBegin,
		// Token: 0x04000A33 RID: 2611
		perfCalculateTokenNewHTMLEnd,
		// Token: 0x04000A34 RID: 2612
		perfCalculateHTMLDifferenceBegin,
		// Token: 0x04000A35 RID: 2613
		perfCalculateHTMLDifferenceEnd,
		// Token: 0x04000A36 RID: 2614
		perfChangeDifferenceScopeBegin,
		// Token: 0x04000A37 RID: 2615
		perfChangeDifferenceScopeEnd,
		// Token: 0x04000A38 RID: 2616
		perfGenerateDeltaChangeLogsBegin,
		// Token: 0x04000A39 RID: 2617
		perfGenerateDeltaChangeLogsEnd,
		// Token: 0x04000A3A RID: 2618
		perfCanvasDecodeBegin,
		// Token: 0x04000A3B RID: 2619
		perfCanvasDecodeEnd,
		// Token: 0x04000A3C RID: 2620
		perfCanvasExecBegin,
		// Token: 0x04000A3D RID: 2621
		perfCanvasExecEnd,
		// Token: 0x04000A3E RID: 2622
		perfComponentInsertBegin,
		// Token: 0x04000A3F RID: 2623
		perfComponentInsertEnd,
		// Token: 0x04000A40 RID: 2624
		perfSolutionComponentLoadBegin,
		// Token: 0x04000A41 RID: 2625
		perfSolutionComponentLoadEnd,
		// Token: 0x04000A42 RID: 2626
		perfSolutionComponentUnloadBegin,
		// Token: 0x04000A43 RID: 2627
		perfSolutionComponentUnloadEnd,
		// Token: 0x04000A44 RID: 2628
		perfSolutionComponentPaneLaunchBegin,
		// Token: 0x04000A45 RID: 2629
		perfSolutionComponentPaneLaunchEnd,
		// Token: 0x04000A46 RID: 2630
		perfXMLToXSDBegin,
		// Token: 0x04000A47 RID: 2631
		perfXMLToXSDEnd,
		// Token: 0x04000A48 RID: 2632
		perfXMLToXSDBuildDataStructuresBegin,
		// Token: 0x04000A49 RID: 2633
		perfXMLToXSDBuildDataStructuresEnd,
		// Token: 0x04000A4A RID: 2634
		perfXMLToXSDGenerateXSDBegin,
		// Token: 0x04000A4B RID: 2635
		perfXMLToXSDGenerateXSDEnd,
		// Token: 0x04000A4C RID: 2636
		perfApplyXSLReapplyBegin,
		// Token: 0x04000A4D RID: 2637
		perfApplyXSLReapplyEnd,
		// Token: 0x04000A4E RID: 2638
		perfOpen_NewCtrlLoadBegin,
		// Token: 0x04000A4F RID: 2639
		perfOpen_NewCtrlLoadEnd,
		// Token: 0x04000A50 RID: 2640
		perfOpen_DocSurfaceBegin,
		// Token: 0x04000A51 RID: 2641
		perfOpen_DocSurfaceEnd,
		// Token: 0x04000A52 RID: 2642
		perfOpen_ContextWorkBegin,
		// Token: 0x04000A53 RID: 2643
		perfOpen_ContextWorkEnd,
		// Token: 0x04000A54 RID: 2644
		perfSaveAsStart,
		// Token: 0x04000A55 RID: 2645
		perfSaveAsEnd,
		// Token: 0x04000A56 RID: 2646
		perfDataObjectLoadFromURLBegin,
		// Token: 0x04000A57 RID: 2647
		perfDataObjectLoadFromURLEnd,
		// Token: 0x04000A58 RID: 2648
		perfDataObjectSaveFromURLBegin,
		// Token: 0x04000A59 RID: 2649
		perfDataObjectSaveFromURLEnd,
		// Token: 0x04000A5A RID: 2650
		perfSubmitPreCheckBegin,
		// Token: 0x04000A5B RID: 2651
		perfSubmitPreCheckEnd,
		// Token: 0x04000A5C RID: 2652
		perfDataObjectCloseBegin,
		// Token: 0x04000A5D RID: 2653
		perfDataObjectCloseEnd,
		// Token: 0x04000A5E RID: 2654
		perfDataObjectSubmitBegin,
		// Token: 0x04000A5F RID: 2655
		perfDataObjectSubmitEnd,
		// Token: 0x04000A60 RID: 2656
		perfDataObjectLoadFromDocBegin,
		// Token: 0x04000A61 RID: 2657
		perfDataObjectLoadFromDocEnd,
		// Token: 0x04000A62 RID: 2658
		perfDataObjectSaveFromDocBegin,
		// Token: 0x04000A63 RID: 2659
		perfDataObjectSaveFromDocEnd,
		// Token: 0x04000A64 RID: 2660
		perfXDocsBootBegin,
		// Token: 0x04000A65 RID: 2661
		perfXDocsBootEnd,
		// Token: 0x04000A66 RID: 2662
		perfBoldBegin,
		// Token: 0x04000A67 RID: 2663
		perfBoldEnd,
		// Token: 0x04000A68 RID: 2664
		perfItalicsBegin,
		// Token: 0x04000A69 RID: 2665
		perfItalicsEnd,
		// Token: 0x04000A6A RID: 2666
		perfHTMLUndoBegin,
		// Token: 0x04000A6B RID: 2667
		perfHTMLUndoEnd,
		// Token: 0x04000A6C RID: 2668
		perfHTMLRedoBegin,
		// Token: 0x04000A6D RID: 2669
		perfHTMLRedoEnd,
		// Token: 0x04000A6E RID: 2670
		perfInsertTableBegin,
		// Token: 0x04000A6F RID: 2671
		perfInsertTableEnd,
		// Token: 0x04000A70 RID: 2672
		perfInsertInternalTableBegin,
		// Token: 0x04000A71 RID: 2673
		perfInsertInternalTableEnd,
		// Token: 0x04000A72 RID: 2674
		perfInternalTableMoveBegin,
		// Token: 0x04000A73 RID: 2675
		perfInternalTableMoveEnd,
		// Token: 0x04000A74 RID: 2676
		perfInsertRowBegin,
		// Token: 0x04000A75 RID: 2677
		perfInsertRowEnd,
		// Token: 0x04000A76 RID: 2678
		perfInsertColBegin,
		// Token: 0x04000A77 RID: 2679
		perfInsertColEnd,
		// Token: 0x04000A78 RID: 2680
		perfInsertRowInternalBegin,
		// Token: 0x04000A79 RID: 2681
		perfInsertRowInternalEnd,
		// Token: 0x04000A7A RID: 2682
		perfInsertColInternalBegin,
		// Token: 0x04000A7B RID: 2683
		perfInsertColInternalEnd,
		// Token: 0x04000A7C RID: 2684
		perfInsertListBegin,
		// Token: 0x04000A7D RID: 2685
		perfInsertListEnd,
		// Token: 0x04000A7E RID: 2686
		perfRecalcBegin,
		// Token: 0x04000A7F RID: 2687
		perfRecalcEnd,
		// Token: 0x04000A80 RID: 2688
		perfInsertFFFBegin,
		// Token: 0x04000A81 RID: 2689
		perfInsertFFFEnd,
		// Token: 0x04000A82 RID: 2690
		perfCanvasActionBegin,
		// Token: 0x04000A83 RID: 2691
		perfCanvasActionEnd,
		// Token: 0x04000A84 RID: 2692
		perfVSShowMainWindow = 7000,
		// Token: 0x04000A85 RID: 2693
		perfVSStatusBarReady,
		// Token: 0x04000A86 RID: 2694
		perfVSLoadPropertyBrowserBegin,
		// Token: 0x04000A87 RID: 2695
		perfVSLoadPropertyBrowserEnd,
		// Token: 0x04000A88 RID: 2696
		perfVSInVStudioMain,
		// Token: 0x04000A89 RID: 2697
		perfVSStartPageCreated,
		// Token: 0x04000A8A RID: 2698
		perfVSDynamicHelpUpdate,
		// Token: 0x04000A8B RID: 2699
		perfVSLoadUIBegin,
		// Token: 0x04000A8C RID: 2700
		perfVSLoadUIEnd,
		// Token: 0x04000A8D RID: 2701
		perfVSBrowserDocumentComplete,
		// Token: 0x04000A8E RID: 2702
		perfVSInitThread,
		// Token: 0x04000A8F RID: 2703
		perfVSFindInFilesBegin,
		// Token: 0x04000A90 RID: 2704
		perfVSFindInFilesEnd,
		// Token: 0x04000A91 RID: 2705
		perfVSStatusBarBuildSucceeded,
		// Token: 0x04000A92 RID: 2706
		perfVSStatusBarRebuildSucceeded,
		// Token: 0x04000A93 RID: 2707
		perfVSDebuggerEnterBreakState,
		// Token: 0x04000A94 RID: 2708
		perfVSDebuggerSessionEnd,
		// Token: 0x04000A95 RID: 2709
		perfVSDebuggerReceivesLoadCompleteEvent,
		// Token: 0x04000A96 RID: 2710
		perfVSDebuggerReceivesEntryPointEvent,
		// Token: 0x04000A97 RID: 2711
		perfVSDebuggerReceivesGoCommand,
		// Token: 0x04000A98 RID: 2712
		perfVSDebuggerReceivesStartNoDebugCommand,
		// Token: 0x04000A99 RID: 2713
		perfVSDebuggerReceivesStepIntoCommand,
		// Token: 0x04000A9A RID: 2714
		perfVSDebuggerReceivesStepOverCommand,
		// Token: 0x04000A9B RID: 2715
		perfVSDebuggerReceivesStepOutCommand,
		// Token: 0x04000A9C RID: 2716
		perfVSDebuggerReceivesBreakCommand,
		// Token: 0x04000A9D RID: 2717
		perfVSDebuggerReceivesStopCommand,
		// Token: 0x04000A9E RID: 2718
		perfVSDebuggerReceivesRestartCommand,
		// Token: 0x04000A9F RID: 2719
		perfVSDebuggerLaunchesAllTargets,
		// Token: 0x04000AA0 RID: 2720
		perfVSDebuggerSendsStartDebuggingRequest,
		// Token: 0x04000AA1 RID: 2721
		perfVSDebuggerLaunchesSingleTarget,
		// Token: 0x04000AA2 RID: 2722
		perfVSDebuggerAutoAttachComplete,
		// Token: 0x04000AA3 RID: 2723
		perfVSDebuggerAddBreakpoint,
		// Token: 0x04000AA4 RID: 2724
		perfVSDebuggerToggleBreakpoint,
		// Token: 0x04000AA5 RID: 2725
		perfVSDebuggerInsertBreakpoint,
		// Token: 0x04000AA6 RID: 2726
		perfVSEditorNavigate,
		// Token: 0x04000AA7 RID: 2727
		perfVSEditorPasteBegin,
		// Token: 0x04000AA8 RID: 2728
		perfVSEditorPasteEnd,
		// Token: 0x04000AA9 RID: 2729
		perfVSEditorFileLoadBegin,
		// Token: 0x04000AAA RID: 2730
		perfVSEditorFileLoadEnd,
		// Token: 0x04000AAB RID: 2731
		perfVSEditorToolTipPaint,
		// Token: 0x04000AAC RID: 2732
		perfVSEditorStatementCompletionPaint,
		// Token: 0x04000AAD RID: 2733
		perfVSEditorCutBegin,
		// Token: 0x04000AAE RID: 2734
		perfVSEditorCutEnd,
		// Token: 0x04000AAF RID: 2735
		perfVSEditorWordWrapBegin,
		// Token: 0x04000AB0 RID: 2736
		perfVSEditorWordWrapEnd,
		// Token: 0x04000AB1 RID: 2737
		perfVSEditorStatementCompletionWordInsert,
		// Token: 0x04000AB2 RID: 2738
		perfVSEditorCommit,
		// Token: 0x04000AB3 RID: 2739
		perfVSProjectLoad,
		// Token: 0x04000AB4 RID: 2740
		perfVSFileOpen,
		// Token: 0x04000AB5 RID: 2741
		perfVSExternalToolComplete,
		// Token: 0x04000AB6 RID: 2742
		perfVSTaskListPopulated,
		// Token: 0x04000AB7 RID: 2743
		perfVSCVExpanded,
		// Token: 0x04000AB8 RID: 2744
		perfVSUIHierExpanded,
		// Token: 0x04000AB9 RID: 2745
		perfVSClassViewPopulated,
		// Token: 0x04000ABA RID: 2746
		perfVSEditGoToDeclaration,
		// Token: 0x04000ABB RID: 2747
		perfVSEditGoToDefinition,
		// Token: 0x04000ABC RID: 2748
		perfVSEditorDropDownDropped,
		// Token: 0x04000ABD RID: 2749
		perfVSSolutionExplorerNavigation,
		// Token: 0x04000ABE RID: 2750
		perfVSSolutionExplorerSolutionPopulated,
		// Token: 0x04000ABF RID: 2751
		perfVSUIHierCollapsed,
		// Token: 0x04000AC0 RID: 2752
		perfVSHelpFilterUpdated,
		// Token: 0x04000AC1 RID: 2753
		perfVSHelpFilterCacheRecomputed,
		// Token: 0x04000AC2 RID: 2754
		perfVSHelpFilterIndexUIUpdated,
		// Token: 0x04000AC3 RID: 2755
		perfVSHelpFilterContentsUIUpdated,
		// Token: 0x04000AC4 RID: 2756
		perfVSHelpFilterFTSResultsUIUpdated,
		// Token: 0x04000AC5 RID: 2757
		perfVSHelpSearchCompleted,
		// Token: 0x04000AC6 RID: 2758
		perfVSCloseSolution,
		// Token: 0x04000AC7 RID: 2759
		perfVSSaveAll,
		// Token: 0x04000AC8 RID: 2760
		perfVSDebuggingFinishedLoadingPackage,
		// Token: 0x04000AC9 RID: 2761
		perfVSSolutionBeginDeploy,
		// Token: 0x04000ACA RID: 2762
		perfVSSolutionEndDeploy,
		// Token: 0x04000ACB RID: 2763
		perfVSStartPageLoadDownloadService,
		// Token: 0x04000ACC RID: 2764
		perfVSMacrosExplorerShowEnd = 7080,
		// Token: 0x04000ACD RID: 2765
		perfVSMacrosIDEShowEnd,
		// Token: 0x04000ACE RID: 2766
		perfVSMacrosMacroRunEnd,
		// Token: 0x04000ACF RID: 2767
		perfVSStatusBarBuildFailed = 7090,
		// Token: 0x04000AD0 RID: 2768
		perfVSStatusBarRebuildFailed,
		// Token: 0x04000AD1 RID: 2769
		perfVSStatusBarBuildCanceled,
		// Token: 0x04000AD2 RID: 2770
		perfVSStatusBarRebuildCanceled,
		// Token: 0x04000AD3 RID: 2771
		perfVSToolboxSupportedCheckStart,
		// Token: 0x04000AD4 RID: 2772
		perfVSToolboxSupportedCheckStop,
		// Token: 0x04000AD5 RID: 2773
		perfVSToolboxResetDone,
		// Token: 0x04000AD6 RID: 2774
		perfVSPrimeCLRNotScheduled,
		// Token: 0x04000AD7 RID: 2775
		perfVSHelpIndexLoadComplete = 7100,
		// Token: 0x04000AD8 RID: 2776
		perfVSPrimeCLRBegin,
		// Token: 0x04000AD9 RID: 2777
		perfVSPrimeCLREnd,
		// Token: 0x04000ADA RID: 2778
		perfVSFinishedBooting,
		// Token: 0x04000ADB RID: 2779
		perfVSNewProjectDlgComplete,
		// Token: 0x04000ADC RID: 2780
		perfVSBrowserDocumentNavigateStart,
		// Token: 0x04000ADD RID: 2781
		perfVSNewProjectDlgOpened,
		// Token: 0x04000ADE RID: 2782
		perfVSHelpF1CommandHandler = 7110,
		// Token: 0x04000ADF RID: 2783
		perfVSHelpF1ContextPacking,
		// Token: 0x04000AE0 RID: 2784
		perfVSHelpF1RemoteF1Call,
		// Token: 0x04000AE1 RID: 2785
		perfVSHelpF1ContextUnpacking,
		// Token: 0x04000AE2 RID: 2786
		perfVSHelpF1LocalDataLookup,
		// Token: 0x04000AE3 RID: 2787
		perfVSHelpF1LocalDataFound,
		// Token: 0x04000AE4 RID: 2788
		perfVSHelpF1ShowURL,
		// Token: 0x04000AE5 RID: 2789
		perfVSHelpWBLogTopicId,
		// Token: 0x04000AE6 RID: 2790
		perfVSHelpStartLoadHxSession = 7120,
		// Token: 0x04000AE7 RID: 2791
		perfVSHelpCompleteLoadHxSession,
		// Token: 0x04000AE8 RID: 2792
		perfVSHelpStartLoadHxCollection,
		// Token: 0x04000AE9 RID: 2793
		perfVSHelpCompleteLoadHxCollection,
		// Token: 0x04000AEA RID: 2794
		perfVSHelpStartLoadHxIndex,
		// Token: 0x04000AEB RID: 2795
		perfVSHelpCompleteLoadHxIndex,
		// Token: 0x04000AEC RID: 2796
		perfVSHelpStartLoadHxTOC,
		// Token: 0x04000AED RID: 2797
		perfVSHelpCompleteLoadHxTOC,
		// Token: 0x04000AEE RID: 2798
		perfVSHelpStartLoadHxFIndex,
		// Token: 0x04000AEF RID: 2799
		perfVSHelpCompleteLoadHxFIndex,
		// Token: 0x04000AF0 RID: 2800
		perfVSHelpStartLoadHxKIndex,
		// Token: 0x04000AF1 RID: 2801
		perfVSHelpCompleteLoadHxKIndex,
		// Token: 0x04000AF2 RID: 2802
		perfVSHelpStartLoadHxAIndex,
		// Token: 0x04000AF3 RID: 2803
		perfVSHelpCompleteLoadHxAIndex,
		// Token: 0x04000AF4 RID: 2804
		perfVSHelpStartLocalSearch = 7141,
		// Token: 0x04000AF5 RID: 2805
		perfVSHelpStartHHQuery,
		// Token: 0x04000AF6 RID: 2806
		perfVSHelpCompleteHHQuery,
		// Token: 0x04000AF7 RID: 2807
		perfVSHelpCompleteLocalSearch,
		// Token: 0x04000AF8 RID: 2808
		perfHxInitializeSession = 7160,
		// Token: 0x04000AF9 RID: 2809
		perfHxCollectionCreated,
		// Token: 0x04000AFA RID: 2810
		perfHxCollectionFileLoaded,
		// Token: 0x04000AFB RID: 2811
		perfHxExCollectionLoaded,
		// Token: 0x04000AFC RID: 2812
		perfHxCollectionInitialized,
		// Token: 0x04000AFD RID: 2813
		perfHxExCollectionStartInit,
		// Token: 0x04000AFE RID: 2814
		perfHxExCollNSpaceListInit,
		// Token: 0x04000AFF RID: 2815
		perfHxExCollCtrlNSpaceInit,
		// Token: 0x04000B00 RID: 2816
		perfHxExCollNSpaceInit,
		// Token: 0x04000B01 RID: 2817
		perfHxExCollNSpaceCollLoad,
		// Token: 0x04000B02 RID: 2818
		perfHxExCollTitleListBuilt,
		// Token: 0x04000B03 RID: 2819
		perfHxExCollTopicsCounted,
		// Token: 0x04000B04 RID: 2820
		perfHxExCollGotTitleInfo,
		// Token: 0x04000B05 RID: 2821
		perfHxExCollMergeValidated,
		// Token: 0x04000B06 RID: 2822
		perfHxExCollInitFTSKeyword,
		// Token: 0x04000B07 RID: 2823
		perfHxExCollBTLStart,
		// Token: 0x04000B08 RID: 2824
		perfHxExCollBTLBuiltFileList,
		// Token: 0x04000B09 RID: 2825
		perfHxExCollBTLValidatedColl,
		// Token: 0x04000B0A RID: 2826
		perfHxExCollBTLHelpFileChanged,
		// Token: 0x04000B0B RID: 2827
		perfHxExCollBTLGotHelpFilesInfo,
		// Token: 0x04000B0C RID: 2828
		perfHxExCollBTLValidatedFastInfo,
		// Token: 0x04000B0D RID: 2829
		perfHxExCollBTLPersistedValidation,
		// Token: 0x04000B0E RID: 2830
		perfHxExCollBTLHelpFileNotChanged,
		// Token: 0x04000B0F RID: 2831
		perfHxExCollBTLPulledFastInfoData,
		// Token: 0x04000B10 RID: 2832
		perfVSProfilerAttached = 7198,
		// Token: 0x04000B11 RID: 2833
		perfVSClientRunStart,
		// Token: 0x04000B12 RID: 2834
		perfVBCompilerPrettyListBegin,
		// Token: 0x04000B13 RID: 2835
		perfVBCompilerPrettyListEnd,
		// Token: 0x04000B14 RID: 2836
		perfVBCompilerStartOutliningBegin,
		// Token: 0x04000B15 RID: 2837
		perfVBCompilerStartOutliningEnd,
		// Token: 0x04000B16 RID: 2838
		perfVBCompilerUpdateLineSeparatorsBegin,
		// Token: 0x04000B17 RID: 2839
		perfVBCompilerUpdateLineSeparatorsEnd,
		// Token: 0x04000B18 RID: 2840
		perfVBCompilerEditClassifyBegin,
		// Token: 0x04000B19 RID: 2841
		perfVBCompilerEditClassifyEnd,
		// Token: 0x04000B1A RID: 2842
		perfVBCompilerEditFilterBegin,
		// Token: 0x04000B1B RID: 2843
		perfVBCompilerEditFilterEnd,
		// Token: 0x04000B1C RID: 2844
		perfVBCompilerSymbolLocationUpdateBegin,
		// Token: 0x04000B1D RID: 2845
		perfVBCompilerSymbolLocationUpdateEnd,
		// Token: 0x04000B1E RID: 2846
		perfVBCompilerBackgroundThreadStop,
		// Token: 0x04000B1F RID: 2847
		perfVBCompilerBackgroundThreadStart,
		// Token: 0x04000B20 RID: 2848
		perfVBCompilerCodeModelLoadFileBegin,
		// Token: 0x04000B21 RID: 2849
		perfVBCompilerCodeModelLoadFileEnd,
		// Token: 0x04000B22 RID: 2850
		perfVBCompilerDropDownLoadBegin,
		// Token: 0x04000B23 RID: 2851
		perfVBCompilerDropDownLoadEnd,
		// Token: 0x04000B24 RID: 2852
		perfVBCompilerClassViewObjectRefreshBegin,
		// Token: 0x04000B25 RID: 2853
		perfVBCompilerClassViewObjectRefreshEnd,
		// Token: 0x04000B26 RID: 2854
		perfVBCompilerIntellisenseBegin,
		// Token: 0x04000B27 RID: 2855
		perfVBCompilerIntellisenseEnd,
		// Token: 0x04000B28 RID: 2856
		perfVBCompilerReachedBoundState,
		// Token: 0x04000B29 RID: 2857
		perfVBCompilerReachedCompiledState,
		// Token: 0x04000B2A RID: 2858
		perfVBCompilerCompilationAborted,
		// Token: 0x04000B2B RID: 2859
		perfVBCompilerFileChanged,
		// Token: 0x04000B2C RID: 2860
		perfVBDebuggerENCDeltaGenBegin,
		// Token: 0x04000B2D RID: 2861
		perfVBDebuggerENCDeltaGenEnd,
		// Token: 0x04000B2E RID: 2862
		perfVBDebuggerENCEnterBreak,
		// Token: 0x04000B2F RID: 2863
		perfVBDebuggerENCExitBreak,
		// Token: 0x04000B30 RID: 2864
		perfVBCompilerRegisterDesignViewAttributeBegin,
		// Token: 0x04000B31 RID: 2865
		perfVBCompilerRegisterDesignViewAttributeEnd,
		// Token: 0x04000B32 RID: 2866
		perfVBCompilerCommitBegin,
		// Token: 0x04000B33 RID: 2867
		perfVBCompilerCommitEnd,
		// Token: 0x04000B34 RID: 2868
		perfViewSwitchBegin = 7300,
		// Token: 0x04000B35 RID: 2869
		perfViewSwitchEnd,
		// Token: 0x04000B36 RID: 2870
		perfParseBegin,
		// Token: 0x04000B37 RID: 2871
		perfParseEnd,
		// Token: 0x04000B38 RID: 2872
		perfSecondaryBufferCodeGenerationBegin,
		// Token: 0x04000B39 RID: 2873
		perfSecondaryBufferCodeGenerationEnd,
		// Token: 0x04000B3A RID: 2874
		perfIntellisenseWindowPopulationBegin,
		// Token: 0x04000B3B RID: 2875
		perfIntellisenseWindowPopulationEnd,
		// Token: 0x04000B3C RID: 2876
		perfSchemaLoadBegin,
		// Token: 0x04000B3D RID: 2877
		perfSchemaLoadEnd,
		// Token: 0x04000B3E RID: 2878
		perfValidationBegin,
		// Token: 0x04000B3F RID: 2879
		perfValidationEnd,
		// Token: 0x04000B40 RID: 2880
		perfSCPBegin,
		// Token: 0x04000B41 RID: 2881
		perfSCPEnd,
		// Token: 0x04000B42 RID: 2882
		perfEditorReady,
		// Token: 0x04000B43 RID: 2883
		perfEditorStartupBegin = 7316,
		// Token: 0x04000B44 RID: 2884
		perfEditorStartupEnd,
		// Token: 0x04000B45 RID: 2885
		perfWebFormTagIntellisenseReady,
		// Token: 0x04000B46 RID: 2886
		perfWebFormCodeIntellisenseReady,
		// Token: 0x04000B47 RID: 2887
		qaTaskListReady,
		// Token: 0x04000B48 RID: 2888
		qaMarkupOutlineReady,
		// Token: 0x04000B49 RID: 2889
		perfWebFormEventNavigationBegin,
		// Token: 0x04000B4A RID: 2890
		perfWebFormEventNavigationEnd,
		// Token: 0x04000B4B RID: 2891
		perfWebFormLoadComplete,
		// Token: 0x04000B4C RID: 2892
		perfWebFormFirstIdleInView,
		// Token: 0x04000B4D RID: 2893
		perfIntellisenseParseBegin,
		// Token: 0x04000B4E RID: 2894
		perfIntellisenseParseEnd,
		// Token: 0x04000B4F RID: 2895
		perfVCDTParseOnMainThreadBegin = 7350,
		// Token: 0x04000B50 RID: 2896
		perfVCDTParseOnParserThreadBegin,
		// Token: 0x04000B51 RID: 2897
		perfVCDTParseEnd,
		// Token: 0x04000B52 RID: 2898
		perfVCDTParseAbort,
		// Token: 0x04000B53 RID: 2899
		perfVSProjShowCodeBegin = 7400,
		// Token: 0x04000B54 RID: 2900
		perfVSProjShowCodeEnd,
		// Token: 0x04000B55 RID: 2901
		perfVSProjShowDesignerBegin,
		// Token: 0x04000B56 RID: 2902
		perfVSProjShowDesignerEnd,
		// Token: 0x04000B57 RID: 2903
		perfVSProjFactoryCreateProjectBegin,
		// Token: 0x04000B58 RID: 2904
		perfVSProjFactoryCreateProjectEnd,
		// Token: 0x04000B59 RID: 2905
		perfVSProjCreateProjectBegin,
		// Token: 0x04000B5A RID: 2906
		perfVSProjCreateProjectEnd,
		// Token: 0x04000B5B RID: 2907
		perfVSProjLoadProjectFileBegin,
		// Token: 0x04000B5C RID: 2908
		perfVSProjLoadProjectFileEnd,
		// Token: 0x04000B5D RID: 2909
		perfVSProjPublishBegin,
		// Token: 0x04000B5E RID: 2910
		perfVSProjPublishEnd,
		// Token: 0x04000B5F RID: 2911
		perfVSProjLoadMSBuildProjectFileBegin,
		// Token: 0x04000B60 RID: 2912
		perfVSProjLoadMSBuildProjectFileEnd,
		// Token: 0x04000B61 RID: 2913
		perfVSProjSetCmdUIContextBegin,
		// Token: 0x04000B62 RID: 2914
		perfVSProjSetCmdUIContextEnd,
		// Token: 0x04000B63 RID: 2915
		perfVSSolutionOnAfterOpenSolutionBegin,
		// Token: 0x04000B64 RID: 2916
		perfVSSolutionOnAfterOpenSolutionEnd,
		// Token: 0x04000B65 RID: 2917
		perfVSProjPOGRefreshBegin,
		// Token: 0x04000B66 RID: 2918
		perfVSProjPOGRefreshEnd,
		// Token: 0x04000B67 RID: 2919
		perfVSProjOnAfterManagedProjectCreate,
		// Token: 0x04000B68 RID: 2920
		perfVSProjOnStartHostingProcess,
		// Token: 0x04000B69 RID: 2921
		perfVSProjOnHostingProcessNotUsed,
		// Token: 0x04000B6A RID: 2922
		perfEditorReplaceInFilesStart,
		// Token: 0x04000B6B RID: 2923
		perfEditorReplaceInFilesEnd,
		// Token: 0x04000B6C RID: 2924
		perfEditorPaintStart,
		// Token: 0x04000B6D RID: 2925
		perfEditorPaintEnd,
		// Token: 0x04000B6E RID: 2926
		perfEditorLoadTextImageFromMemoryStart,
		// Token: 0x04000B6F RID: 2927
		perfEditorLoadTextImageFromMemoryEnd,
		// Token: 0x04000B70 RID: 2928
		perfEditorSaveTextImageToMemoryStart,
		// Token: 0x04000B71 RID: 2929
		perfEditorSaveTextImageToMemoryEnd,
		// Token: 0x04000B72 RID: 2930
		perfEditorSaveTextReplaceLinesExStart,
		// Token: 0x04000B73 RID: 2931
		perfEditorSaveTextReplaceLinesExEnd,
		// Token: 0x04000B74 RID: 2932
		perfEditorSaveTextReplaceStreamExStart,
		// Token: 0x04000B75 RID: 2933
		perfEditorSaveTextReplaceStreamExEnd,
		// Token: 0x04000B76 RID: 2934
		perfEditorSaveTextVerticalScrollStart,
		// Token: 0x04000B77 RID: 2935
		perfEditorSaveTextVerticalScrollEnd,
		// Token: 0x04000B78 RID: 2936
		perfEditorCreateEditorInstance,
		// Token: 0x04000B79 RID: 2937
		perfVSWebMigrationBegin = 7450,
		// Token: 0x04000B7A RID: 2938
		perfVSWebMigrationEnd,
		// Token: 0x04000B7B RID: 2939
		perfVSWebAfterFirstIdle,
		// Token: 0x04000B7C RID: 2940
		perfVSWebOpenStarts,
		// Token: 0x04000B7D RID: 2941
		perfVSWebOpenEnds,
		// Token: 0x04000B7E RID: 2942
		perfVSWebInitialProcessingComplete,
		// Token: 0x04000B7F RID: 2943
		perfVSWebBuildWebsiteBegins,
		// Token: 0x04000B80 RID: 2944
		perfVSWebBuildWebsiteEnds,
		// Token: 0x04000B81 RID: 2945
		perfVSWebCodeMarkerControl,
		// Token: 0x04000B82 RID: 2946
		perfVSTProjectPackageSetSiteStart = 8000,
		// Token: 0x04000B83 RID: 2947
		perfVSTProjectPrecreateForOuterStart,
		// Token: 0x04000B84 RID: 2948
		perfVSTProjectSetInnerProjectEnd,
		// Token: 0x04000B85 RID: 2949
		perfVSTProjectInitializeForOuterStart,
		// Token: 0x04000B86 RID: 2950
		perfVSTProjectInitializeForOuterEnd,
		// Token: 0x04000B87 RID: 2951
		perfVSTProjectSyncWithHostStart,
		// Token: 0x04000B88 RID: 2952
		perfVSTProjectSyncWithHostEnd,
		// Token: 0x04000B89 RID: 2953
		perfVSTProjectSetProjectClientStart,
		// Token: 0x04000B8A RID: 2954
		perfVSTProjectSetProjectClientEnd,
		// Token: 0x04000B8B RID: 2955
		perfVSTProjectOnProjectCreatedStart,
		// Token: 0x04000B8C RID: 2956
		perfVSTProjectOnProjectCreatedEnd,
		// Token: 0x04000B8D RID: 2957
		perfVSTProjectAddExtensibleItemStart,
		// Token: 0x04000B8E RID: 2958
		perfVSTProjectAddExtensibleItemEnd,
		// Token: 0x04000B8F RID: 2959
		perfVSTProjectRefreshBufferContentStart,
		// Token: 0x04000B90 RID: 2960
		perfVSTProjectRefreshBufferContentEnd,
		// Token: 0x04000B91 RID: 2961
		perfVSTInteractiveProjectCreateStart,
		// Token: 0x04000B92 RID: 2962
		perfVSTInteractiveProjectCreateEnd,
		// Token: 0x04000B93 RID: 2963
		perfVSTInteractiveProjectResetAllStart,
		// Token: 0x04000B94 RID: 2964
		perfVSTInteractiveProjectResetAllEnd,
		// Token: 0x04000B95 RID: 2965
		perfVSTProjectWizard,
		// Token: 0x04000B96 RID: 2966
		perfVSTProjectWizardOnFinish,
		// Token: 0x04000B97 RID: 2967
		perfVSTProjectWizardOnBeforeCreateProjectStart,
		// Token: 0x04000B98 RID: 2968
		perfVSTProjectWizardOnBeforeCreateProjectEnd,
		// Token: 0x04000B99 RID: 2969
		perfVSTProjectWizardProjectFinishedGeneratingStart,
		// Token: 0x04000B9A RID: 2970
		perfVSTProjectWizardProjectFinishedGeneratingEnd,
		// Token: 0x04000B9B RID: 2971
		perfVSTProjectBlueprintInitStart,
		// Token: 0x04000B9C RID: 2972
		perfVSTProjectBlueprintInitEnd,
		// Token: 0x04000B9D RID: 2973
		perfVSTDesignerCreateLoaderStart = 8028,
		// Token: 0x04000B9E RID: 2974
		perfVSTDesignerCreateLoaderEnd,
		// Token: 0x04000B9F RID: 2975
		perfVSTDesignerDoVerbStart,
		// Token: 0x04000BA0 RID: 2976
		perfVSTDesignerDoVerbEnd,
		// Token: 0x04000BA1 RID: 2977
		perfVSTDesignerSetSiteStart,
		// Token: 0x04000BA2 RID: 2978
		perfVSTDesignerSetSiteEnd,
		// Token: 0x04000BA3 RID: 2979
		perfVSTDesignerInitDocDesignerStart,
		// Token: 0x04000BA4 RID: 2980
		perfVSTDesignerInitDocDesignerEnd,
		// Token: 0x04000BA5 RID: 2981
		perfVSTDesignerOnViewChangedStart,
		// Token: 0x04000BA6 RID: 2982
		perfVSTDesignerOnViewChangedEnd,
		// Token: 0x04000BA7 RID: 2983
		perfVSTDesignerCoCreateStart,
		// Token: 0x04000BA8 RID: 2984
		perfVSTDesignerCoCreateEnd,
		// Token: 0x04000BA9 RID: 2985
		perfVSTDesignerBeginLoadStart,
		// Token: 0x04000BAA RID: 2986
		perfVSTDesignerBeginLoadEnd,
		// Token: 0x04000BAB RID: 2987
		perfVSTDSWDropStart,
		// Token: 0x04000BAC RID: 2988
		perfVSTDSWDropEnd,
		// Token: 0x04000BAD RID: 2989
		perfVSTClientUpdateProjectStart,
		// Token: 0x04000BAE RID: 2990
		perfVSTClientUpdateProjectEnd,
		// Token: 0x04000BAF RID: 2991
		perfVSTClientHostSideAdaptorStart,
		// Token: 0x04000BB0 RID: 2992
		perfVSTClientHostSideAdaptorEnd,
		// Token: 0x04000BB1 RID: 2993
		perfVSTClientOleRunStart,
		// Token: 0x04000BB2 RID: 2994
		perfVSTClientOleRunEnd,
		// Token: 0x04000BB3 RID: 2995
		perfVSTClientInstantiateDocumentStart,
		// Token: 0x04000BB4 RID: 2996
		perfVSTClientInstantiateDocumentEnd,
		// Token: 0x04000BB5 RID: 2997
		perfVSTClientRefreshProgrammingModelStart,
		// Token: 0x04000BB6 RID: 2998
		perfVSTClientRefreshProgrammingModelEnd,
		// Token: 0x04000BB7 RID: 2999
		perfVSTClientShowDocumentStart,
		// Token: 0x04000BB8 RID: 3000
		perfVSTClientShowDocumentEnd,
		// Token: 0x04000BB9 RID: 3001
		perfVSTClientBindStart,
		// Token: 0x04000BBA RID: 3002
		perfVSTClientBindEnd,
		// Token: 0x04000BBB RID: 3003
		perfVSTClientCoCreateHostInstanceStart,
		// Token: 0x04000BBC RID: 3004
		perfVSTClientCoCreateHostInstanceEnd,
		// Token: 0x04000BBD RID: 3005
		perfAppInfoTaskStart,
		// Token: 0x04000BBE RID: 3006
		perfAppInfoTaskEnd,
		// Token: 0x04000BBF RID: 3007
		perfCustomizeFirstStart,
		// Token: 0x04000BC0 RID: 3008
		perfCustomizeFirstEnd,
		// Token: 0x04000BC1 RID: 3009
		perfCustomizeStartupInfoStart,
		// Token: 0x04000BC2 RID: 3010
		perfCustomizeStartupInfoEnd,
		// Token: 0x04000BC3 RID: 3011
		perfCustomizeAppAsmDependStart,
		// Token: 0x04000BC4 RID: 3012
		perfCustomizeAppAsmDependEnd,
		// Token: 0x04000BC5 RID: 3013
		perfCustomizeRefAsmDependStart,
		// Token: 0x04000BC6 RID: 3014
		perfCustomizeRefAsmDependEnd,
		// Token: 0x04000BC7 RID: 3015
		perfCustomizeEntryPointsStart,
		// Token: 0x04000BC8 RID: 3016
		perfCustomizeEntryPointsEnd,
		// Token: 0x04000BC9 RID: 3017
		perfCustomizePersistStart,
		// Token: 0x04000BCA RID: 3018
		perfCustomizePersistEnd,
		// Token: 0x04000BCB RID: 3019
		perfCustomizeLastStart,
		// Token: 0x04000BCC RID: 3020
		perfCustomizeLastEnd,
		// Token: 0x04000BCD RID: 3021
		perfPersisterGetObjectStart,
		// Token: 0x04000BCE RID: 3022
		perfPersisterGetObjectEnd,
		// Token: 0x04000BCF RID: 3023
		perfPersisterWriteStart = 8080,
		// Token: 0x04000BD0 RID: 3024
		perfPersisterWriteEnd,
		// Token: 0x04000BD1 RID: 3025
		perfPersisterFinishedStart,
		// Token: 0x04000BD2 RID: 3026
		perfPersisterFinishedEnd,
		// Token: 0x04000BD3 RID: 3027
		perfOfficePersistenceObjectIsOpenedStart,
		// Token: 0x04000BD4 RID: 3028
		perfOfficePersistenceObjectIsOpenedEnd,
		// Token: 0x04000BD5 RID: 3029
		perfOfficePersistenceObjectCoCreateStart,
		// Token: 0x04000BD6 RID: 3030
		perfOfficePersistenceObjectCoCreateEnd,
		// Token: 0x04000BD7 RID: 3031
		perfOfficePersistenceObjectDeletePropsStart,
		// Token: 0x04000BD8 RID: 3032
		perfOfficePersistenceObjectDeletePropsEnd,
		// Token: 0x04000BD9 RID: 3033
		perfOfficePersistenceObjectOpenDocStart,
		// Token: 0x04000BDA RID: 3034
		perfOfficePersistenceObjectOpenDocEnd,
		// Token: 0x04000BDB RID: 3035
		perfOfficePersistenceObjectAddCtrlStart,
		// Token: 0x04000BDC RID: 3036
		perfOfficePersistenceObjectAddCtrlEnd,
		// Token: 0x04000BDD RID: 3037
		perfOfficePersistenceObjectSetProtectionStart,
		// Token: 0x04000BDE RID: 3038
		perfOfficePersistenceObjectSetProtectionEnd,
		// Token: 0x04000BDF RID: 3039
		perfVSTSecurityTaskStart,
		// Token: 0x04000BE0 RID: 3040
		perfVSTSecurityTaskEnd,
		// Token: 0x04000BE1 RID: 3041
		perfCustomizeDataCacheStart,
		// Token: 0x04000BE2 RID: 3042
		perfCustomizeDataCacheEnd,
		// Token: 0x04000BE3 RID: 3043
		perfReadCachedDataManifestStart,
		// Token: 0x04000BE4 RID: 3044
		perfReadCachedDataManifestEnd,
		// Token: 0x04000BE5 RID: 3045
		perfOpenEventStart = 8104,
		// Token: 0x04000BE6 RID: 3046
		perfOpenEventEnd,
		// Token: 0x04000BE7 RID: 3047
		perfFindControlStart,
		// Token: 0x04000BE8 RID: 3048
		perfFindControlEnd,
		// Token: 0x04000BE9 RID: 3049
		perfCreateEvidenceStart = 8110,
		// Token: 0x04000BEA RID: 3050
		perfCreateEvidenceEnd,
		// Token: 0x04000BEB RID: 3051
		perfStartCLRStart = 8114,
		// Token: 0x04000BEC RID: 3052
		perfStartCLREnd,
		// Token: 0x04000BED RID: 3053
		perfCreateDomainStart,
		// Token: 0x04000BEE RID: 3054
		perfCreateDomainEnd,
		// Token: 0x04000BEF RID: 3055
		perfConfigDomainStart = 8120,
		// Token: 0x04000BF0 RID: 3056
		perfConfigDomainEnd,
		// Token: 0x04000BF1 RID: 3057
		perfExecManifestStart,
		// Token: 0x04000BF2 RID: 3058
		perfExecManifestEnd,
		// Token: 0x04000BF3 RID: 3059
		perfExecManifestParseManifestStart = 8126,
		// Token: 0x04000BF4 RID: 3060
		perfExecManifestParseManifestEnd,
		// Token: 0x04000BF5 RID: 3061
		perfExecManifestUpdateStart,
		// Token: 0x04000BF6 RID: 3062
		perfExecManifestUpdateEnd,
		// Token: 0x04000BF7 RID: 3063
		perfExecManifestSetPolicyStart,
		// Token: 0x04000BF8 RID: 3064
		perfExecManifestSetPolicyEnd,
		// Token: 0x04000BF9 RID: 3065
		perfExecManifestConfigStart,
		// Token: 0x04000BFA RID: 3066
		perfExecManifestConfigEnd,
		// Token: 0x04000BFB RID: 3067
		perfExecManifestConfigLoadStartupAsmStart = 8136,
		// Token: 0x04000BFC RID: 3068
		perfExecManifestConfigLoadStartupAsmEnd,
		// Token: 0x04000BFD RID: 3069
		perfExecManifestConfigCreateStartupObjAsmGetTypeStart,
		// Token: 0x04000BFE RID: 3070
		perfExecManifestConfigCreateStartupObjAsmGetTypeEnd,
		// Token: 0x04000BFF RID: 3071
		perfExecManifestConfigCreateStartupObjInvokeStart = 8142,
		// Token: 0x04000C00 RID: 3072
		perfExecManifestConfigCreateStartupObjInvokeEnd,
		// Token: 0x04000C01 RID: 3073
		perfExecManifestCompleteStartupObjectInitializationStart,
		// Token: 0x04000C02 RID: 3074
		perfExecManifestCompleteStartupObjectInitializationEnd,
		// Token: 0x04000C03 RID: 3075
		perfCreateForClientNewStart,
		// Token: 0x04000C04 RID: 3076
		perfCreateForClientNewEnd,
		// Token: 0x04000C05 RID: 3077
		perfCreateForClientStartupStart,
		// Token: 0x04000C06 RID: 3078
		perfCreateForClientStartupEnd,
		// Token: 0x04000C07 RID: 3079
		perfCreateForClientMyAppStart,
		// Token: 0x04000C08 RID: 3080
		perfCreateForClientMyAppEnd,
		// Token: 0x04000C09 RID: 3081
		perfCreateForClientInitializeViewComponentsStart,
		// Token: 0x04000C0A RID: 3082
		perfCreateForClientInitializeViewComponentsEnd,
		// Token: 0x04000C0B RID: 3083
		perfCreateForClientInitializeComponentsStart,
		// Token: 0x04000C0C RID: 3084
		perfCreateForClientInitializeComponentsEnd,
		// Token: 0x04000C0D RID: 3085
		perfCreateForClientInitializeDataComponentsStart,
		// Token: 0x04000C0E RID: 3086
		perfCreateForClientInitializeDataComponentsEnd,
		// Token: 0x04000C0F RID: 3087
		perfCreateForClientBindStart,
		// Token: 0x04000C10 RID: 3088
		perfCreateForClientBindEnd,
		// Token: 0x04000C11 RID: 3089
		perfCreateForClientInitCompleteStart,
		// Token: 0x04000C12 RID: 3090
		perfCreateForClientInitCompleteEnd,
		// Token: 0x04000C13 RID: 3091
		perfVSTOLoaderLoadStart,
		// Token: 0x04000C14 RID: 3092
		perfVSTOLoaderLoadEnd,
		// Token: 0x04000C15 RID: 3093
		perfGetDefaultDomainStart = 8168,
		// Token: 0x04000C16 RID: 3094
		perfGetDefaultDomainEnd,
		// Token: 0x04000C17 RID: 3095
		perfGetAppbaseStart,
		// Token: 0x04000C18 RID: 3096
		perfGetAppbaseEnd,
		// Token: 0x04000C19 RID: 3097
		perfCreateCustomizationDomainInteropStart,
		// Token: 0x04000C1A RID: 3098
		perfCreateCustomizationDomainInteropEnd,
		// Token: 0x04000C1B RID: 3099
		perfCreateUrisStart,
		// Token: 0x04000C1C RID: 3100
		perfCreateUrisEnd,
		// Token: 0x04000C1D RID: 3101
		perfGetConfigStringsStart,
		// Token: 0x04000C1E RID: 3102
		perfGetConfigStringsEnd,
		// Token: 0x04000C1F RID: 3103
		perfReflectOnADMStart,
		// Token: 0x04000C20 RID: 3104
		perfReflectOnADMEnd,
		// Token: 0x04000C21 RID: 3105
		perfCreateDictionariesStart,
		// Token: 0x04000C22 RID: 3106
		perfCreateDictionariesEnd,
		// Token: 0x04000C23 RID: 3107
		perfCallbackHostStart,
		// Token: 0x04000C24 RID: 3108
		perfCallbackHostEnd,
		// Token: 0x04000C25 RID: 3109
		perfCompleteInitializationStart,
		// Token: 0x04000C26 RID: 3110
		perfCompleteInitializationEnd,
		// Token: 0x04000C27 RID: 3111
		perfVSHelpExternalHelpInitializing = 9000,
		// Token: 0x04000C28 RID: 3112
		perfVSHelpExternalObjectCreated,
		// Token: 0x04000C29 RID: 3113
		perfVSHelpExternalGotInitData,
		// Token: 0x04000C2A RID: 3114
		perfVSHelpExternalPutHelpOwner,
		// Token: 0x04000C2B RID: 3115
		perfVSHelpExternalGotSettingsManager,
		// Token: 0x04000C2C RID: 3116
		perfVSHelpPutSettingsTokenStart,
		// Token: 0x04000C2D RID: 3117
		perfVSHelpPutSettingsTokenComplete,
		// Token: 0x04000C2E RID: 3118
		perfVSHelpPutSettingsTokenFireSettingsChanged,
		// Token: 0x04000C2F RID: 3119
		perfVSHelpSetCollectionBegin,
		// Token: 0x04000C30 RID: 3120
		perfVSHelpSetCollectionReInitStart,
		// Token: 0x04000C31 RID: 3121
		perfVSHelpSetCollectionReleasedObjects,
		// Token: 0x04000C32 RID: 3122
		perfVSHelpSetCollectionFiredSettingsChanged,
		// Token: 0x04000C33 RID: 3123
		perfVSHelpSetCollectionFiredCollectionChanged,
		// Token: 0x04000C34 RID: 3124
		perfVSHelpExternalHelpInitialized,
		// Token: 0x04000C35 RID: 3125
		perfVSHelpExternalCommunicatedHelpToken,
		// Token: 0x04000C36 RID: 3126
		perfVSHelpServiceF1Begin,
		// Token: 0x04000C37 RID: 3127
		perfVSHelpGotShellContextService,
		// Token: 0x04000C38 RID: 3128
		perfVSHelpGotBrowserWindow,
		// Token: 0x04000C39 RID: 3129
		perfVSHelpGetBrowserWindowFoundModalState,
		// Token: 0x04000C3A RID: 3130
		perfVSHelpGetBrowserWindowGotWBService,
		// Token: 0x04000C3B RID: 3131
		perfVSHelpGetBrowserWindowCreatedWB,
		// Token: 0x04000C3C RID: 3132
		perfVSCreateWebBrowserExStart,
		// Token: 0x04000C3D RID: 3133
		perfVSCreateWebBrowserExNeedCreate,
		// Token: 0x04000C3E RID: 3134
		perfVSCreateWebBrowserExDocDataCreated,
		// Token: 0x04000C3F RID: 3135
		perfVSCreateWebBrowserExPrepareCreateToolWin,
		// Token: 0x04000C40 RID: 3136
		perfVSCreateWebBrowserExCreatedToolWin,
		// Token: 0x04000C41 RID: 3137
		perfVSCreateWebBrowserExToolWinInitialized,
		// Token: 0x04000C42 RID: 3138
		perfVSCreateWebBrowserExDocDataInitialized,
		// Token: 0x04000C43 RID: 3139
		perfVSWebBrowserNavigateComplete2,
		// Token: 0x04000C44 RID: 3140
		perfHxInitFTSKeywordBegin = 9030,
		// Token: 0x04000C45 RID: 3141
		perfHxInitFTSKeywordCreateFTS,
		// Token: 0x04000C46 RID: 3142
		perfHxInitFTSKeywordCreateFTI,
		// Token: 0x04000C47 RID: 3143
		perfHxInitFTSKeywordInitFTI,
		// Token: 0x04000C48 RID: 3144
		perfHxInitFTSInitTitleArray,
		// Token: 0x04000C49 RID: 3145
		perfHxInitFTSInitTitleArrayInitTitlesBegin,
		// Token: 0x04000C4A RID: 3146
		perfHxInitFTSInitTitleArrayInitTitlesEnd,
		// Token: 0x04000C4B RID: 3147
		perfHxInitFTSInitTitleArrayInitFreeHxSEnd,
		// Token: 0x04000C4C RID: 3148
		perfHxInitFTSInitTitleArrayInitHxQEnd,
		// Token: 0x04000C4D RID: 3149
		perfHxInitFTSInitTitleArrayInitHxQ,
		// Token: 0x04000C4E RID: 3150
		perfHxInitFTSInitTitleArrayComputeTopicCount,
		// Token: 0x04000C4F RID: 3151
		perfHxInitFTSInitTitleArrayOutputMapInfo,
		// Token: 0x04000C50 RID: 3152
		perfHxIndexQueryBegin,
		// Token: 0x04000C51 RID: 3153
		perfHxIndexInitializeMergedFileBegin,
		// Token: 0x04000C52 RID: 3154
		perfHxIndexQueryGotOffset,
		// Token: 0x04000C53 RID: 3155
		perfHxIndexQueryFoundMatchingKeyword,
		// Token: 0x04000C54 RID: 3156
		perfHxIndexQueryAddedTopicsForKeyword,
		// Token: 0x04000C55 RID: 3157
		perfHxIndexQueryFoundAllTopics,
		// Token: 0x04000C56 RID: 3158
		perfHxIndexQueryAddedAllTopics,
		// Token: 0x04000C57 RID: 3159
		perfVSHelpIndexPutFilter = 9050,
		// Token: 0x04000C58 RID: 3160
		perfVSHelpIndexInitBegin,
		// Token: 0x04000C59 RID: 3161
		perfVSHelpIndexMerged,
		// Token: 0x04000C5A RID: 3162
		perfVSHelpIndexGotXLinkInfo,
		// Token: 0x04000C5B RID: 3163
		perfVSHelpIndexInitWithXLinkInfo,
		// Token: 0x04000C5C RID: 3164
		perfVSHelpMergeIndexBegin,
		// Token: 0x04000C5D RID: 3165
		perfVSHelpMergeIndexDoneCount,
		// Token: 0x04000C5E RID: 3166
		perfVSHelpMergeIndexParsedHxK,
		// Token: 0x04000C5F RID: 3167
		perfVSHelpMergeIndexGotLock,
		// Token: 0x04000C60 RID: 3168
		perfVSHelpMergeIndexInitializedValidator,
		// Token: 0x04000C61 RID: 3169
		perfVSHelpMergeIndexRoundZero,
		// Token: 0x04000C62 RID: 3170
		perfVSHelpMergeIndexRoundOne,
		// Token: 0x04000C63 RID: 3171
		perfVSHelpMergeIndexRoundTwo,
		// Token: 0x04000C64 RID: 3172
		perfVSHelpMergeIndexDoneRounds,
		// Token: 0x04000C65 RID: 3173
		perfVSHelpMergeIndexPersistedHxD,
		// Token: 0x04000C66 RID: 3174
		perfVSHelpMergeIndexComplete,
		// Token: 0x04000C67 RID: 3175
		perfVSHelpInitValidatorBegin,
		// Token: 0x04000C68 RID: 3176
		perfVSHelpInitValidatorGotAccess,
		// Token: 0x04000C69 RID: 3177
		perfVSHelpInitValidatorWithFileBegin,
		// Token: 0x04000C6A RID: 3178
		perfVSHelpInitValidatorOpenedFS,
		// Token: 0x04000C6B RID: 3179
		perfVSHelpInitValidatorCheckedSig,
		// Token: 0x04000C6C RID: 3180
		perfVSHelpInitValidatorOpenedContentTVD,
		// Token: 0x04000C6D RID: 3181
		perfVSHelpInitValidatorOpenedContentFN,
		// Token: 0x04000C6E RID: 3182
		perfVSHelpInitValidatorOpenedMergedTVD,
		// Token: 0x04000C6F RID: 3183
		perfVSHelpInitValidatorOpenedMergedFN,
		// Token: 0x04000C70 RID: 3184
		perfVSHelpInitValidatorLoadedMergedFileData,
		// Token: 0x04000C71 RID: 3185
		perfVSHelpInitValidatorContentsChanged,
		// Token: 0x04000C72 RID: 3186
		perfVSHelpInitValidatorCheckedMergedFileValidity,
		// Token: 0x04000C73 RID: 3187
		perfVSHelpUserSettingsLoadManagedStart = 9090,
		// Token: 0x04000C74 RID: 3188
		perfVSHelpUserSettingsLoadManagedComplete,
		// Token: 0x04000C75 RID: 3189
		perfVSWebPackageLoaded,
		// Token: 0x04000C76 RID: 3190
		perfVSContextServiceStart,
		// Token: 0x04000C77 RID: 3191
		perfVSContextServiceCreated,
		// Token: 0x04000C78 RID: 3192
		perfVSContextServiceLoaded,
		// Token: 0x04000C79 RID: 3193
		perfVSDexploreRun,
		// Token: 0x04000C7A RID: 3194
		perfVSDexploreInitializedPaths,
		// Token: 0x04000C7B RID: 3195
		perfVSDexploreCreatedAppid,
		// Token: 0x04000C7C RID: 3196
		perfVSDexploreDisplayedSplashScreen,
		// Token: 0x04000C7D RID: 3197
		perfVSDexploreStartSetSite,
		// Token: 0x04000C7E RID: 3198
		perfVSDexploreEnsuredAppName,
		// Token: 0x04000C7F RID: 3199
		perfVSDexploreInitializedUserContext,
		// Token: 0x04000C80 RID: 3200
		perfVSDexploreInitAppNameStart,
		// Token: 0x04000C81 RID: 3201
		perfVSDexploreInitAppNameGotBaseName,
		// Token: 0x04000C82 RID: 3202
		perfVSDexploreGotHxSession,
		// Token: 0x04000C83 RID: 3203
		perfVSDexploreGotHxCollection,
		// Token: 0x04000C84 RID: 3204
		perfVSDexploreGotCollectionTitle,
		// Token: 0x04000C85 RID: 3205
		perfVSHelpIndexOnSettingsRootChanged,
		// Token: 0x04000C86 RID: 3206
		perfVSHelpIndexOSRCInitializedUI,
		// Token: 0x04000C87 RID: 3207
		perfVSHelpTocOnSettingsRootChanged,
		// Token: 0x04000C88 RID: 3208
		perfVSHelpTocOSRCInitializedUI,
		// Token: 0x04000C89 RID: 3209
		perfVSHelpXLinkIndexSetFilterBegin,
		// Token: 0x04000C8A RID: 3210
		perfVSHelpXLinkIndexInitialized,
		// Token: 0x04000C8B RID: 3211
		perfVSHelpXLinkIndexFilterSet,
		// Token: 0x04000C8C RID: 3212
		perfVSHelpXLinkIndexInitializeMergedFile,
		// Token: 0x04000C8D RID: 3213
		perfVSHelpXLinkIndexInitializeMergedFileOpenedFS,
		// Token: 0x04000C8E RID: 3214
		perfVSHelpXLinkIndexInitializeMergedFileInitializedLeaves,
		// Token: 0x04000C8F RID: 3215
		perfVSHelpXLinkIndexInitializeMergedFileCachedBranches,
		// Token: 0x04000C90 RID: 3216
		perfVSHelpXLinkIndexInitializeMergedFileCachedPageSlots,
		// Token: 0x04000C91 RID: 3217
		perfVSHelpF1FoundTargetItems,
		// Token: 0x04000C92 RID: 3218
		perfVSHelpF1InitializedDisambiguationData,
		// Token: 0x04000C93 RID: 3219
		perfVSHelpKeywordLookupBegin,
		// Token: 0x04000C94 RID: 3220
		perfVSHelpKeywordLookupGotCWHService,
		// Token: 0x04000C95 RID: 3221
		perfVSHelpKeywordLookupSetIndex,
		// Token: 0x04000C96 RID: 3222
		perfVSHelpKeywordLookupGotTopics,
		// Token: 0x04000C97 RID: 3223
		perfVSHelpKeywordLookupInitializedAttrFilter,
		// Token: 0x04000C98 RID: 3224
		perfVSHelpKeywordLookupLoadedTopicsForKeyword,
		// Token: 0x04000C99 RID: 3225
		perfVSHelpKeywordLookupStartXmlTopics,
		// Token: 0x04000C9A RID: 3226
		perfVSHelpGetTopicsFromKeywordStart = 9128,
		// Token: 0x04000C9B RID: 3227
		perfVSHelpGetTopicsFromKeywordSetIndex,
		// Token: 0x04000C9C RID: 3228
		perfVSHelpGetTopicsFromKeywordGotTopics,
		// Token: 0x04000C9D RID: 3229
		perfVSHelpGetTopicsFromKeywordGotVsTopicList,
		// Token: 0x04000C9E RID: 3230
		perfVSHelpKeywordLookupFoundXmlTopics,
		// Token: 0x04000C9F RID: 3231
		perfVSHelpExternalSetCollection,
		// Token: 0x04000CA0 RID: 3232
		perfVSContextWindowInitCreateContents = 9135,
		// Token: 0x04000CA1 RID: 3233
		perfVSContextWindowInitGetMonitorService,
		// Token: 0x04000CA2 RID: 3234
		perfVSContextWindowInitGetAppContext,
		// Token: 0x04000CA3 RID: 3235
		perfVSContextWindowInitReadRegistry,
		// Token: 0x04000CA4 RID: 3236
		perfVSContextWindowInitGetShellContextService,
		// Token: 0x04000CA5 RID: 3237
		perfVSContextWindowInitCmdUIContexts,
		// Token: 0x04000CA6 RID: 3238
		perfVSContextWindowInitSelection,
		// Token: 0x04000CA7 RID: 3239
		perfVSDexploreSetMDIOption,
		// Token: 0x04000CA8 RID: 3240
		perfVSDexploreInitCheckLoadPackage,
		// Token: 0x04000CA9 RID: 3241
		perfVSHelpInitTocLoadedFilters,
		// Token: 0x04000CAA RID: 3242
		perfVSHelpInitTocCreatedControl,
		// Token: 0x04000CAB RID: 3243
		perfVSHelpInitTocInitControlProperties,
		// Token: 0x04000CAC RID: 3244
		perfVSHelpInitTocStart,
		// Token: 0x04000CAD RID: 3245
		perfVSHelpF1LookupEnsureContextInit,
		// Token: 0x04000CAE RID: 3246
		perfVSHelpF1LookupGotHelp,
		// Token: 0x04000CAF RID: 3247
		perfVSHelpF1LookupGotHelp2,
		// Token: 0x04000CB0 RID: 3248
		perfVSHelpF1LookupGotFirstTimeDlg,
		// Token: 0x04000CB1 RID: 3249
		perfVSHelpF1LookupGotContextMonitor,
		// Token: 0x04000CB2 RID: 3250
		perfVSHelpF1LookupGotAppCtx,
		// Token: 0x04000CB3 RID: 3251
		perfVSHelpF1LookupUpdatedAppCtx,
		// Token: 0x04000CB4 RID: 3252
		perfVSHelpF1LookupGotContextAsSafeArray,
		// Token: 0x04000CB5 RID: 3253
		perfVSContextWindowCreated,
		// Token: 0x04000CB6 RID: 3254
		perfVSLoadContextFilesInitStrings,
		// Token: 0x04000CB7 RID: 3255
		perfVSLoadContextFilesFoundFile,
		// Token: 0x04000CB8 RID: 3256
		perfVSLoadContextFilesParsedFile,
		// Token: 0x04000CB9 RID: 3257
		perfVSDexploreWinMain,
		// Token: 0x04000CBA RID: 3258
		perfVSHelpILocalRegistry,
		// Token: 0x04000CBB RID: 3259
		perfVSDexploreInitParams,
		// Token: 0x04000CBC RID: 3260
		perfVSDexploreInitGuids,
		// Token: 0x04000CBD RID: 3261
		perfVSDexploreInitOle,
		// Token: 0x04000CBE RID: 3262
		perfVSHelpFilterToolInitGotHelpService,
		// Token: 0x04000CBF RID: 3263
		perfVSHelpFilterToolInitGotFilters,
		// Token: 0x04000CC0 RID: 3264
		perfVSHelpFilterToolInitAddedFilters,
		// Token: 0x04000CC1 RID: 3265
		perfVSHelpFilterToolInitPutFilter,
		// Token: 0x04000CC2 RID: 3266
		perfVSHelpFilterToolInitFillComplete,
		// Token: 0x04000CC3 RID: 3267
		perfVSHelpFilterToolInitFillBegin,
		// Token: 0x04000CC4 RID: 3268
		perfVSHelpHrDoLocalF1LookupBegin,
		// Token: 0x04000CC5 RID: 3269
		perfVSHelpHrDoLocalF1LookupContextInit,
		// Token: 0x04000CC6 RID: 3270
		perfVSHelpHrDoLocalF1LookupGetContextMonitor,
		// Token: 0x04000CC7 RID: 3271
		perfVSHelpHrDoLocalF1LookupUnpackContext,
		// Token: 0x04000CC8 RID: 3272
		perfVSHelpHrDoLocalF1LookupGetAppCtx,
		// Token: 0x04000CC9 RID: 3273
		perfVSHelpItemLoadKeywordGotTopicAt,
		// Token: 0x04000CCA RID: 3274
		perfVSHelpItemLoadKeywordFilteredTopic,
		// Token: 0x04000CCB RID: 3275
		perfVSHelpGetHelpSettings,
		// Token: 0x04000CCC RID: 3276
		perfVSHelpSettingsInitialized,
		// Token: 0x04000CCD RID: 3277
		perfVSHelpGetF1Preference,
		// Token: 0x04000CCE RID: 3278
		perfVSHelpF1PreferenceFound,
		// Token: 0x04000CCF RID: 3279
		perfVSHelpHrDoLocalF1Begin,
		// Token: 0x04000CD0 RID: 3280
		perfVSHelpHrDoOnlineF1Begin,
		// Token: 0x04000CD1 RID: 3281
		perfVSHelpHrDoLocalF1Failover,
		// Token: 0x04000CD2 RID: 3282
		perfVSHelpOnlineF1Callback,
		// Token: 0x04000CD3 RID: 3283
		perfVSHelpGetAllAttrValuesBegin,
		// Token: 0x04000CD4 RID: 3284
		perfVSHelpGetAllAttrValuesComplete,
		// Token: 0x04000CD5 RID: 3285
		perfVSHelpGetAllAttrValuesGotCollection,
		// Token: 0x04000CD6 RID: 3286
		perfVSHelpGetAllAttrValuesGotAttrNames,
		// Token: 0x04000CD7 RID: 3287
		perfVSHelpGetAllAttrValuesGotAttrValues,
		// Token: 0x04000CD8 RID: 3288
		perfVSHelpF1LookupLoadKeywordBegin,
		// Token: 0x04000CD9 RID: 3289
		perfVSHelpF1LookupGetNameBegin,
		// Token: 0x04000CDA RID: 3290
		perfVSHelpF1LookupGetNameComplete,
		// Token: 0x04000CDB RID: 3291
		perfVSHelpF1LookupGetUrlBegin,
		// Token: 0x04000CDC RID: 3292
		perfVSHelpF1LookupGetUrlComplete,
		// Token: 0x04000CDD RID: 3293
		perfVSHelpF1PrepareHeaders,
		// Token: 0x04000CDE RID: 3294
		perfVSHelpF1NavigateWithDisambiguation,
		// Token: 0x04000CDF RID: 3295
		perfVSHelpF1FoundTopicURL,
		// Token: 0x04000CE0 RID: 3296
		perfVSInitFMain = 9220,
		// Token: 0x04000CE1 RID: 3297
		perfVSInitializedAppIdGlobals,
		// Token: 0x04000CE2 RID: 3298
		perfVSInitializedBase,
		// Token: 0x04000CE3 RID: 3299
		perfVSInitializedGlobal,
		// Token: 0x04000CE4 RID: 3300
		perfVSCheckedTimeBomb,
		// Token: 0x04000CE5 RID: 3301
		perfVSSitedAppid,
		// Token: 0x04000CE6 RID: 3302
		perfVSCheckedLicensing,
		// Token: 0x04000CE7 RID: 3303
		perfVSCheckedActivation,
		// Token: 0x04000CE8 RID: 3304
		perfVSInitUIThread,
		// Token: 0x04000CE9 RID: 3305
		perfVSInitUIInitializedThemes,
		// Token: 0x04000CEA RID: 3306
		perfVSInitUIInitializedBrushes,
		// Token: 0x04000CEB RID: 3307
		perfVSInitUIInitializedBubble,
		// Token: 0x04000CEC RID: 3308
		perfVSInitUIInitializedMainMenuWin,
		// Token: 0x04000CED RID: 3309
		perfVSInitUIInitializedDisasterRecovery,
		// Token: 0x04000CEE RID: 3310
		perfVSInitUIModeInitComplete,
		// Token: 0x04000CEF RID: 3311
		perfVSInitUIPreloadedPackages,
		// Token: 0x04000CF0 RID: 3312
		perfVSInitUIRegisteredCF,
		// Token: 0x04000CF1 RID: 3313
		perfVSInitUIInitializedAppId,
		// Token: 0x04000CF2 RID: 3314
		perfVSInitUILoadedInitialProject,
		// Token: 0x04000CF3 RID: 3315
		perfVSInitUIOnIDEInitialized,
		// Token: 0x04000CF4 RID: 3316
		perfVSInitUICheckedBadAddins,
		// Token: 0x04000CF5 RID: 3317
		perfVSInitUIThreadComplete,
		// Token: 0x04000CF6 RID: 3318
		perfVSPbrsUpdateFixer,
		// Token: 0x04000CF7 RID: 3319
		perfVSMainLoggedPushingMsgLoop,
		// Token: 0x04000CF8 RID: 3320
		perfVSCallVsMainFoundMsenv,
		// Token: 0x04000CF9 RID: 3321
		perfVSCallVsMainLoadedMsenv,
		// Token: 0x04000CFA RID: 3322
		perfVSCallVsMainFoundVStudioMainProc,
		// Token: 0x04000CFB RID: 3323
		perfVSInitMainMenuWindowInitCommonControls,
		// Token: 0x04000CFC RID: 3324
		perfVSInitMainMenuWindowCreatedHwnd,
		// Token: 0x04000CFD RID: 3325
		perfVSInitMainMenuWindowInitOffice,
		// Token: 0x04000CFE RID: 3326
		perfVSInitMainMenuWindowPrefInitPart2,
		// Token: 0x04000CFF RID: 3327
		perfVSInitMainMenuWindowCreateVSShellMenu,
		// Token: 0x04000D00 RID: 3328
		perfVSInitMainMenuWindowInitializeCmdbars,
		// Token: 0x04000D01 RID: 3329
		perfVSInitMainMenuWindowSetupCmdUIcontexts,
		// Token: 0x04000D02 RID: 3330
		perfVSInitMainMenuWindowInitDebugMgr,
		// Token: 0x04000D03 RID: 3331
		perfVSInitMainMenuWindowResetContextGuids,
		// Token: 0x04000D04 RID: 3332
		perfVSInitMainMenuWindowInitHierWindow,
		// Token: 0x04000D05 RID: 3333
		perfVSInitMainMenuWindowExtIDEInit,
		// Token: 0x04000D06 RID: 3334
		perfVSInitMainMenuWindowAliases,
		// Token: 0x04000D07 RID: 3335
		perfVSPreloadPackagesLoadedPackage,
		// Token: 0x04000D08 RID: 3336
		perfVSInitMainMenuWindowInitializeMSODialogs,
		// Token: 0x04000D09 RID: 3337
		perfVSLocalRegistryCreateInstanceBegin,
		// Token: 0x04000D0A RID: 3338
		perfVSLocalRegistryVsLoaderCoCreateInstance,
		// Token: 0x04000D0B RID: 3339
		perfVSLocalRegistryGetClassObjectOfClsid,
		// Token: 0x04000D0C RID: 3340
		perfVSLocalRegistryCFCreateInstance,
		// Token: 0x04000D0D RID: 3341
		perfVSInitGlobalLoadLangDLLMain = 9274,
		// Token: 0x04000D0E RID: 3342
		perfVSInitGlobalInitOle,
		// Token: 0x04000D0F RID: 3343
		perfVSInitGlobalInitRegValues,
		// Token: 0x04000D10 RID: 3344
		perfVSInitGlobalCheckDllVersions,
		// Token: 0x04000D11 RID: 3345
		perfVSInitGlobalInitShellFromRegistry,
		// Token: 0x04000D12 RID: 3346
		perfVSInitGlobalInitDirectories = 9280,
		// Token: 0x04000D13 RID: 3347
		perfVSInitGlobalInitShellFromRegistry2,
		// Token: 0x04000D14 RID: 3348
		perfVSInitGlobalCreateIdeFonts,
		// Token: 0x04000D15 RID: 3349
		perfVSInitGlobalCreateTheSolution,
		// Token: 0x04000D16 RID: 3350
		perfVSInitGlobalEnsureDDEAtoms,
		// Token: 0x04000D17 RID: 3351
		perfVSInitGlobalSVsAppid,
		// Token: 0x04000D18 RID: 3352
		perfVSInitGlobalRegisterJITDebugger,
		// Token: 0x04000D19 RID: 3353
		perfVSInitGlobalInitMRUs,
		// Token: 0x04000D1A RID: 3354
		perfVSInitGlobalMergeExternalTools,
		// Token: 0x04000D1B RID: 3355
		perfVSInitGlobalVsUIProjWinHierarchyInitClass,
		// Token: 0x04000D1C RID: 3356
		perfVSInitGlobalRestoreFileAssociations,
		// Token: 0x04000D1D RID: 3357
		perfVSMainMenuWindowShown,
		// Token: 0x04000D1E RID: 3358
		perfVSInitMainMenuWindowExtIDEWins,
		// Token: 0x04000D1F RID: 3359
		perfVSInitMainMenuWindowShowDockableWins,
		// Token: 0x04000D20 RID: 3360
		perfVSInitMainMenuWindowFixedPaneWins,
		// Token: 0x04000D21 RID: 3361
		perfVSInitMainMenuWindowResizedAllDocks,
		// Token: 0x04000D22 RID: 3362
		perfVSInitMainMenuWindowComplete,
		// Token: 0x04000D23 RID: 3363
		perfVSSettingsStartupCheckBegin = 9300,
		// Token: 0x04000D24 RID: 3364
		perfVSSettingsStartupCheckComplete,
		// Token: 0x04000D25 RID: 3365
		perfVSSettingsImportStart,
		// Token: 0x04000D26 RID: 3366
		perfVSSettingsImportComplete,
		// Token: 0x04000D27 RID: 3367
		perfVSSettingsExportStart,
		// Token: 0x04000D28 RID: 3368
		perfVSSettingsExportComplete,
		// Token: 0x04000D29 RID: 3369
		perfVSSettingsLoadBegin,
		// Token: 0x04000D2A RID: 3370
		perfVSSettingsLoadComplete,
		// Token: 0x04000D2B RID: 3371
		perfVSSettingsSaveBegin,
		// Token: 0x04000D2C RID: 3372
		perfVSSettingsSaveComplete,
		// Token: 0x04000D2D RID: 3373
		perfVSInitDelayLoadOfUIDLLsBegin = 9350,
		// Token: 0x04000D2E RID: 3374
		perfVSInitDelayLoadOfUIDLLsEndIteration,
		// Token: 0x04000D2F RID: 3375
		perfVSInitDelayLoadOfUIDLLsEnd,
		// Token: 0x04000D30 RID: 3376
		perfHxProtocolInitBegin = 9400,
		// Token: 0x04000D31 RID: 3377
		perfHxProtocolInitComplete,
		// Token: 0x04000D32 RID: 3378
		perfHxProtocolInternalStartBegin,
		// Token: 0x04000D33 RID: 3379
		perfHxProtocolInternalStartGotPhysicalUrl,
		// Token: 0x04000D34 RID: 3380
		perfHxProtocolInternalStartItsProtocolInitialized,
		// Token: 0x04000D35 RID: 3381
		perfHxProtocolInternalItsStartComplete,
		// Token: 0x04000D36 RID: 3382
		perfHxProtocolInternalIECacheCleared,
		// Token: 0x04000D37 RID: 3383
		perfHxIndexTopicId2TopicArrayBegin,
		// Token: 0x04000D38 RID: 3384
		perfHxTitleInformationInitializeBegin,
		// Token: 0x04000D39 RID: 3385
		perfHxTitleInformationInitializeOpenedTxt,
		// Token: 0x04000D3A RID: 3386
		perfHxTitleInformationInitializeVersionCorrect,
		// Token: 0x04000D3B RID: 3387
		perfHxTitleInformationInitializeComplete,
		// Token: 0x04000D3C RID: 3388
		perfHxTitleGetTopicURLBegin,
		// Token: 0x04000D3D RID: 3389
		perfHxTitleGotTopicData,
		// Token: 0x04000D3E RID: 3390
		perfHxTitleGotUrlOffset,
		// Token: 0x04000D3F RID: 3391
		perfHxInitTitleBegin,
		// Token: 0x04000D40 RID: 3392
		perfHxInitTitleOpenedSubFiles,
		// Token: 0x04000D41 RID: 3393
		perfHxInitTitleComplete,
		// Token: 0x04000D42 RID: 3394
		perfVSWebPkgCreated = 9430,
		// Token: 0x04000D43 RID: 3395
		perfVSWebPkgSetSiteBegin,
		// Token: 0x04000D44 RID: 3396
		perfVSWebPkgSetModuleSite,
		// Token: 0x04000D45 RID: 3397
		perfVSWebPkgSetSiteUILibraryLoaded,
		// Token: 0x04000D46 RID: 3398
		perfVSWebPkgSetSitePrefsLoaded,
		// Token: 0x04000D47 RID: 3399
		perfVSWebPkgSetSiteServicesProffered1,
		// Token: 0x04000D48 RID: 3400
		perfVSWebPkgSetSiteContextTrackerCreated,
		// Token: 0x04000D49 RID: 3401
		perfVSWebPkgSetSiteServicesProffered2
	}
}
