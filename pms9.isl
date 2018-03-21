retainglobalvar
var dll_handle:N32


event signin	
	DLLLoad dll_handle, "customerdisplay.dll"	
	DLLCALL_CDECL dll_handle, cdshowdisplay () 
endevent

event init
endevent

event exit//signout
	DLLFree dll_handle
endevent


event signout
	DLLCall_CDECL dll_handle, cdsenddata (2,0,"",0,"", "","")  
endevent

event begin_check
	infomessage "new check"
endevent

event begincheck
	infomessage "new check2"
endevent

event closecheck
	infomessage "check closed"
endevent


Event MI
	var i:N3
	var messagetype:N2

 
	for i = 1 to @NUMDTLT		// @NUMDTLR = Number of Detail Entries this Service Round
		if @DTL_TYPE[i] = "M" 	// M = Menu Item; I = Check Information Detail; D = Discount Name; S = Service Charge; T = Tender Media; R = Reference Number; C = CA Detail
			
			if @Dtl_is_void[i] = 1
				infomessage @dtl_name[i] + " voided now"
			tmpstr2 = " " +  @tax[1]
			
			if @Dtl_is_cond[i] = 1
				messagetype = 1
			else
				messagetype = 0
			endif 

			//tmpstr = " "  +  @dtl_ttl[i]

			DLLCall_CDECL dll_handle, cdsenddata (messagetype, i, @DTL_NAME[i], @DTL_QTY[i],  @dtl_ttl[i], @tax[1], "")
		endif
	endfor

EndEvent 

event dsc
	@DSC
	DLLCall_CDECL dll_handle, cdsenddata (messagetype, 0, "", 0, "", "", @DSC);
endevent

event DSC_VOID
endevent
 
event mi_void
	infomessage @obj + " voided"
	infomessage @DTL_NAME[@obj] + " voided"
endevent

event mi_return
endevent


event repopen_check
endevent

event tndr
endevent

event close_check
endevent

event final_tender
endevent