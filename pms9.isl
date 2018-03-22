retainglobalvar
var dll_handle:N32


event signin	
	DLLLoad dll_handle, "customerdisplay.dll"
	DLLCALL_CDECL dll_handle, cdshowcustomerdisplay () 
	DLLCALL_CDECL dll_handle, cdsetdisplaymode (1)
endevent

event init
endevent

event exit//signout
	DLLFree dll_handle
endevent


event signout
	DLLCall_CDECL dll_handle, cdsenddata (2,0," ",0," "," "," ")  
endevent

event begin_check
	DLLCALL_CDECL dll_handle, cdsenddata (2,1," ",0, " ", " ", " ")
endevent

event close_check
	DLLCALL_CDECL dll_handle, cdsetdisplaymode (1)
endevent


Event MI
	var i:N3
	var messagetype:N2

 
	for i = 1 to @NUMDTLT		// @NUMDTLR = Number of Detail Entries this Service Round
		if @DTL_TYPE[i] = "M" 	// M = Menu Item; I = Check Information Detail; D = Discount Name; S = Service Charge; T = Tender Media; R = Reference Number; C = CA Detail
			
			if @Dtl_is_void[i] = 1
				infomessage @dtl_name[i] + " voided now"
			endif
			
			if @Dtl_is_cond[i] = 1
				messagetype = 1
			else
				messagetype = 0
			endif 


			DLLCall_CDECL dll_handle, cdsenddata (messagetype, i, @DTL_NAME[i], @DTL_QTY[i],  @dtl_ttl[i], @tax[1], "")
		endif
	endfor

EndEvent 

event dsc
	@DSC
	DLLCall_CDECL dll_handle, cdsenddata (3, 0, "", 0, "", "", @DSC);
endevent

event DSC_VOID
	DLLCALL_CDECL dll_handle, cdsenddata (3,0," ",0, " ", " ", "0")
endevent
 
event mi_void
	infomessage @obj + " voided"
	infomessage @DTL_NAME[@obj] + " voided"
	DLLCALL_CDECL dll_handle, cdsenddata (5,@obj," ",0, " ", " ", " ")
endevent

event mi_return
endevent


event repopen_check
endevent

event tndr
//figure out amount paid (or change?)
endevent

event trans_cancel
infomessage "transaction cancelled"
endevent

event void_check
info "void_check"
endevent


event final_tender
//@CHANGE (has value if ttldue == "0.00")
//@TTLDUE (how muc is still needed)
//@CHK_TTL (total of check)
	DLLCALL_CDECL dll_handle, cdsetdisplaymode (1)
endevent