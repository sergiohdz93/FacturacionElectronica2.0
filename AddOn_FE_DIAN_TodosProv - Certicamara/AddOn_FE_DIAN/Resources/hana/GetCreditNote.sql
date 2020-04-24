Select 
'NC' as "tipoDocumento", '1.0' as "versionDocumento", Case When IfNull(A9."U_Ambiente",'02') = '02' Then 'false' Else 'true' End "registrar", A9."U_idEmpresa" as "control", 
A8."U_DocDIAN" as "codigoTipoDocumento", A0."U_SEI_FETipOper" as "tipoOperacion", A3."BeginStr" as "prefijoDocumento", A0."DocNum" as "numeroDocumento", 
TO_NVARCHAR(A0."DocDate", 'YYYY-MM-DD') as "fechaEmision", TO_TIMESTAMP (CURRENT_TIME, 'HH24:MI:SS') as "horaEmision", A6."NLineas" as "numeroLineas", A6."SubTotal" as "subtotal", 
A7."baseimpu" as "totalBaseImponible", 
A6."SubTotal" + A0."VatSum" as "subtotalMasTributos", A0."DiscSum" as "totalDescuentos", A0."DocTotal" + IfNull(A10."TotalRet",0) as "total", 

A11."ISOCurrCod" as "codigoMoneda", TO_NVARCHAR(A12."RateDate", 'YYYY-MM-DD') as "fechaCambio", A11."ISOCurrCod" as "codigoMonedaFacturado", 'COP' as "codigoMonedaCambio", 
1.00 as "baseCambioFacturado", 1.00 as "baseCambio", A12."Rate" as "trm", 

A0."U_SEI_FEMedPago" as "codigoMedioPago", TO_NVARCHAR(A0."DocDueDate", 'YYYY-MM-DD') as "fechaVencimiento", 

A1."VisOrder" + 1 as "numeroLinea", A1."Quantity" as "cantidad", A1."LineTotal" as "valorTotal",

A1."ItemCode" as "idProducto", '01' as "codigoPrecio", A1."PriceBefDi" as "valorUnitario", A1."Quantity" as "cantidadReal", 
CASE 
WHEN IfNull(A1."UomCode",'') = 'Manual' Then IfNull((Select IfNull(P0."U_DIAN_UM",'') From "@FEDIAN_HOMOL_UM" P0 Where P0."U_SAP_UM" = A1."unitMsr"),'94') 
Else IfNull((Select IfNull(P0."U_DIAN_UM",'') From "@FEDIAN_HOMOL_UM" P0 Where P0."U_SAP_UM" = A1."UomCode"),'94') End "codigoUnidad",
'false' as "esMuestraComercial", A1."Dscription" as "item", 

'00' as "codigoDesc", 'Descuento no condicionado' as "Razon", A1."PriceBefDi" as "base", A1."DiscPrcnt" as "porcentajeDesc", 
(A1."LineTotal" * A1."DiscPrcnt")/100 as "valorDesc",

A14."Code" as "codigo", A14."Name" as "Nombre", A15."BaseSum" as "baseGravable", A15."TaxRate" as "porcentaje", A15."TaxSum" as "valor",
CASE 
WHEN IfNull(A1."UomCode",'') = 'Manual' Then IfNull((Select IfNull(P0."U_DIAN_UM",'') From "@FEDIAN_HOMOL_UM" P0 Where P0."U_SAP_UM" = A1."unitMsr"),'94') 
Else IfNull((Select IfNull(P0."U_DIAN_UM",'') From "@FEDIAN_HOMOL_UM" P0 Where P0."U_SAP_UM" = A1."UomCode"),'94') End "codigoUnidad",

'01' as "codigo", 'Descuento condicionado' as "Razon", A6."SubTotal" as "base", A0."DiscPrcnt" as "porcentaje", A0."DiscSum" as "valor",

A16."PrintHeadr" as "RazonSocial", A16."PrintHeadr" as "nombreRegistrado", A16."U_SEI_FETipDoc" as "tipoIdentificacion", 
Case When Locate(A16."TaxIdNum", '-') = 0 Then A16."TaxIdNum" Else SubString(A16."TaxIdNum", 1, Locate(A16."TaxIdNum", '-')-1) End as "Identificacion",
Case When Locate(A16."TaxIdNum", '-') = 0 Then '' Else SubString(A0."TaxIdNum", (Locate(A16."TaxIdNum", '-')||1),1) End "digitoVerificacion",
Case When IfNull(A16.U_HBT_TipEnt,'') = '1' Then '2' Else '1' End "naturaleza", A16."U_SEI_FERegFis" as "codigoRegimen", A17."U_Codigo" as "responsabilidadFiscal",
A18."U_Codigo" as "codigoImpuesto", A18."U_Desc" as "nombreImpuesto", A16.Phone1 as "telefono", A16."E_Mail" as "email",

A16."Country" as "codigoPais", A19."Name" as "nombrePais", 'es' as "codigoLenguajePais", Substring(A16."U_HBT_MunMed", 1, 2) as "codigoDepartamento",
A20."U_NomDepartamento" as "nombreDepartamento", A16."U_HBT_MunMed" as "codigoCiudad", A20."Name" as "nombreCiudad", A21."Street" as "direccionFisica",
'000000' as "codigoPostal",

A2.CardName as "razonSocial", A2.CardName as "nombreRegistrado", A2."U_HBT_TipDoc" as "tipoIdentificacion",
Case When Locate(A2."LicTradNum",'-') = 0 Then A2."LicTradNum" Else SubString(A2."LicTradNum", 1, Locate(A2."LicTradNum", '-')-1) End as "Identificacion",
Case When Locate(A2."LicTradNum",'-') = 0 Then '' Else SubString(A0."LicTradNum", (Locate(A0."LicTradNum", '-')||1),1) End "digitoVerificacion",
Case When IfNull(A2.U_HBT_TipEnt,'') = '1' Then '2' Else '1' End "naturaleza", A2."U_SEI_FERegFis" as "codigoRegimen", A22."U_Codigo" as "responsabilidadFiscal",
A23.U_Codigo as "codigoImpuesto", A23."U_Desc" as "nombreImpuesto", A2."Phone1" as "Telefono", A2."E_Mail" as "email",

A2."Country" as "codigoPais", A24."Name" as "nombrePais", 'es' as "codigoLenguajePais", Substring(A2."U_HBT_MunMed", 1, 2) as "codigoDepartamento",
A25."U_NomDepartamento" as "nombreDepartamento", A2."U_HBT_MunMed" as "codigoCiudad", A25."Name" as "nombreCiudad", A2."MailAddres" as "direccionFisica",
'000000' as "codigoPostal",

A8."U_NumResol", TO_NVARCHAR(A8."U_FechaDesde", 'YYYY-MM-DD') as "fechaInicio", TO_NVARCHAR(A8."U_FechaHasta", 'YYYY-MM-DD') as "fechaFin", A3."BeginStr" as "prefijo",
A8."U_InitialNum" as "desde", A8."U_LastNum" as "hasta", 'CVCC#ÁÉÍÓÚÜÑ&áéíóúúñ@¿¡!' as "cvcc", A26."U_SEI_FEIdent",

A8."U_posicionXCufe", A8."U_posicionYCufe", A8."U_rotacionCufe", A8."U_fuenteCufe", A8."U_posicionXQr", A8."U_posicionYQr",

IsNull(A27."U_Prefijo",'') + A27."U_Folio" as "id", 'FE' as "tipo", CONVERT(char(10), A27."U_Fecha_Envio",126) as "fecha", 'CUFE-SHA384' as "algoritmo",
A27."U_ID_Seguimiento" as "CUFE"

From ORIN A0
Inner Join RIN1 A1 On A0."DocEntry" = A1."DocEntry"
Inner Join OCRD A2 On A0."CardCode" = A2."CardCode"
Inner Join NNM1 A3 On A0."Series" = A3."Series"
Left Join "@HBT_MUNICIPIO" A4 On A2."U_HBT_MunMed" = A4."Code"
Inner Join OCTG A5 On A0."GroupNum" = A5."GroupNum"
Left Join (Select "DocEntry",Count(*) as "NLineas", Sum("LineTotal") as "SubTotal" From "RIN1" Group By "DocEntry") A6 On A0."DocEntry" = A6."DocEntry"
Left Join (Select "DocEntry", Sum("BaseSum") as "baseimpu" From "RIN4" Where "ExpnsCode" = -1 Group By "DocEntry") A7 On A0."DocEntry" = A7."DocEntry"
Left Join "@FEDIAN_NUMAUTORI" A8 On A0."Series" = A8."Code"
Left Join "@FEDIAN_PARAMG" A9 On 1 = 1
Left Join (
			Select B0."AbsEntry" , Sum("WTAmnt") as "TotalRet"
			From RIN5 B0 
			Inner Join OWHT B1 On B0."WTCode" = B1."WTCode" And B1."U_HBT_TipRet" != 0
			Group By B0."AbsEntry"
		  ) A10 On A0."DocEntry" = A10."AbsEntry"
Left Join OCRN A11 On A0."DocCur" = A11."CurrCode"
Left Join ORTT A12 On A0."DocCur" = A12."Currency" And A0."DocDate" = A12."RateDate"
Left Join OSTC A13 On A1."TaxCode" = A13."Code"
Left Join "@FEDIAN_TRIBU" A14 On A13."U_SEI_FETributo" = A14."Code"
Left Join RIN4 A15 On A1."DocEntry" = A15."DocEntry" And A1."LineNum" = A15."LineNum" And A15."ExpnsCode" = -1
Left Join OADM A16 On 1 = 1
Left Join (Select "Code", STRING_AGG("U_Codigo",';') As "U_Codigo" From "@FEDIAN_SNRES" Group By "Code") A17 On A16."TaxIdNum" = A17."Code"
Left Join (	Select Top 1 A0."TaxIdNum", A2.U_Codigo, A2.U_Desc
			From OADM A0
			Inner Join "@FEDIAN_SN" A1 On A0."TaxIdNum" = A1."Code"
			Inner Join "@FEDIAN_SNTRI" A2 On A1.Code = A2.Code
			) A18 On A16."TaxIdNum" = A18."TaxIdNum"
Left Join OCRY A19 On A16."Country" = A19."Code"
Left Join "@HBT_MUNICIPIO" A20 On A16."U_HBT_MunMed" = A20."Code"
Left Join ADM1 A21 ON A16.Code= A21.Code
Left Join (Select "Code", STRING_AGG("U_Codigo",';') As "U_Codigo" From "@FEDIAN_SNRES" Group By "Code") A22 On A2."CardCode" = A22."Code"
Left Join (
			Select Top 1 A0."CardCode", A2.U_Codigo, A2.U_Desc
			From OCRD A0
			Inner Join "@FEDIAN_SN" A1 On A0."CardCode" = A1."Code"
			Inner Join "@FEDIAN_SNTRI" A2 On A1.Code = A2.Code
		  ) A23 On A2."CardCode" = A23."CardCode"
Left Join OCRY A24 On A2."Country" = A24."Code"
Left Join "@HBT_MUNICIPIO" A25 On A2."U_HBT_MunMed" = A25."Code"
Left Join OITM A26 On A1."ItemCode" = A26."ItemCode"
Left Join "@FEDIAN_MONITORLOG" A27 On A1."BaseType" = A27."U_ObjType" And A1."BaseEntry" = A27."U_DocNum"
Where A0."DocEntry" = {0}