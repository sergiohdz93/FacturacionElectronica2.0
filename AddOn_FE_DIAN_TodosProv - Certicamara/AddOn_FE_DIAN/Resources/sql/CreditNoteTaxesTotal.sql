Select 'false' as "esRete", A3."U_SEI_FETributo", A5."Total" as "Total", 
A4."Code", A4."Name", A2."BaseSum", A2."TaxRate", A2."TaxSum", 
CASE 
WHEN IsNull(A1."UomCode",'') = 'Manual' Then IsNull((Select IsNull(P0."U_DIAN_UM",'') From "@FEDIAN_HOMOL_UM" P0 Where P0."U_SAP_UM" = A1."unitMsr"),'94') 
Else IsNull((Select IsNull(P0."U_DIAN_UM",'') From "@FEDIAN_HOMOL_UM" P0 Where P0."U_SAP_UM" = A1."UomCode"),'94') End "codigoUnidad"
From ORIN A0
Inner Join RIN1 A1 On A0."DocEntry" = A1."DocEntry"
Inner Join RIN4 A2 On A0."DocEntry" = A2."DocEntry" And A1."LineNum" = A2."LineNum" And A2."ExpnsCode" = -1
Inner Join OSTC A3 On A1."TaxCode" = A3."Code"
Left Join "@FEDIAN_TRIBU" A4 On A3."U_SEI_FETributo" = A4."Code"
Left Join (
			Select B0."DocEntry", B1."U_SEI_FETributo", Sum(B0."TaxSum") as "Total" 
			From RIN4 B0 
			Inner Join OSTC B1 On B0."StcCode" = B1."Code" 
			Where B0."ExpnsCode" = -1
			Group By B0."DocEntry", B1."U_SEI_FETributo"
		  ) A5 On A0."DocEntry" = A5."DocEntry" And A3."U_SEI_FETributo" = A5."U_SEI_FETributo"
Where A0."DocEntry" = {0}

Union All

Select 'true' as "esRete", A3."U_SEI_FETributo", A5."Total" as "Total", 
A4."Code", A4."Name", A2."U_HBT_BaseRet", A3."PrctBsAmnt", A2."WTAmnt", 
'' "codigoUnidad"
From ORIN A0
Inner Join RIN5 A2 On A0."DocEntry" = A2."AbsEntry"
Inner Join OWHT A3 On A2."WTCode" = A3."WTCode"
Left Join "@FEDIAN_TRIBU" A4 On A3."U_SEI_FETributo" = A4."Code"
Left Join (
			Select B0."AbsEntry", B1."U_SEI_FETributo", Sum(B0."WTAmnt") as "Total" 
			From RIN5 B0 
			Inner Join OWHT B1 On B0."WTCode" = B1."WTCode" 
			Group By B0."AbsEntry", B1."U_SEI_FETributo"
		  ) A5 On A0."DocEntry" = A5."AbsEntry" And A3."U_SEI_FETributo" = A5."U_SEI_FETributo"
Where A0."DocEntry" = {0} And A3."U_HBT_TipRet" != 0