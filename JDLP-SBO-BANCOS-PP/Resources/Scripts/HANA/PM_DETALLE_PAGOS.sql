CREATE VIEW "PM_DETALLE_PAGOS" AS 
------------------------------------------------------
-- VISTA HANA PERU CON CAMPOS DE USUARIO DE PERU
------------------------------------------------------
SELECT 
"Branch",
"GLAccCode",
"MetodoPago",
"BancoCargo",
"NombreBnkCargo",
"CtaCargo",
    "SucursalCargo",
    "ObjType",
    "DocSubType",
    "Tipo",
    "InvKey",
    "DocNum",
    "TaxDate",
    "FolioNum",
    "IdNumber",
    "WizardName",
--"ModPago",
CASE
-- 1 Cuenta Vista
 WHEN "ModPago" =1 and "BancoCargo" = "BancoAbono" THEN 1
 WHEN "ModPago" =1 and "BancoCargo" != "BancoAbono" THEN 7

 --2 Cuenta Ahorro
 WHEN "ModPago" =2 and "BancoCargo" = "BancoAbono" THEN 2
 WHEN "ModPago" =2 and "BancoCargo" != "BancoAbono" THEN 3

 --3 Cuenta Corriente
 WHEN "ModPago" =3 and "BancoCargo" = "BancoAbono" THEN 3
 WHEN "ModPago" =3 and "BancoCargo" != "BancoAbono" THEN 6

 -- 4 Vale Vista o Efectivo
 WHEN "ModPago" =4 THEN 4

  -- 5 Vale Vista Correo Certif.
 WHEN "ModPago" =5  THEN 8

  -- 6 Cuenta en linea o Vale Vista Virtual
 WHEN "ModPago" =6 THEN 9
 ELSE "ModPago"
 END AS "ModPago",
"Status",
    "StatusDisc",
    "Checked",
    "Canceled",
    "FechaPago",
    -- "pymNum",
    "Proveedor",
    "RutProveedor",
    "NombreProveedor",
    "Direccion",
    "Comuna",
    "Ciudad",
    "Fono",
    "Region",
    "E_Mail",
    "BancoAbono",
    "NombreBnkAbono",
    "CtaAbono",
    "SucursalAbono",
    SUM("InvPayAmnt") AS "InvPayAmnt",
    SUM("TotalLoc") AS "TotalLoc",
    SUM("TotalFC") AS "TotalFC",
    SUM("TotalSys") AS "TotalSys",
    "InvCurr",
    "PymCurr",
    "TipoPersona",
    "TipoDocum",
    "Paterno",
    "Materno",
    "Nombre1",
    "Nombre2",
    "NumAtCard",
    "FlagSinglePay",
    "UsrNumber4" AS "TipoCtaCargo",
    "UsrNumber3",
    "ReceiptNum",
    "Indicator"
 FROM (
select distinct 
        IFNULL(D3."BPLId", -1) AS "Branch",
        D3."PymMeth" AS "MetodoPago",
        D4."BnkDflt" AS "BancoCargo",
        -- (GBL) MT.BnkDflt as BancoCargo,
        BAP."BankName" AS "NombreBnkCargo",
        D4."BankAccou" AS "CtaCargo",
        -- (GBL) MT.DflAccount as CtaCargo,
        IFNULL(BA1."Branch",'') AS "SucursalCargo",
        -- (GBL) IFNULL(MT.Branch,'') as SucursalCargo,
        D3."ObjType",
        IFNULL(FA."DocSubType",'') AS "DocSubType",

CASE WHEN D3."ObjType" = 18 AND FA."DocSubType" = '--'  THEN 'Factura' ELSE 
CASE WHEN D3."ObjType" = 18 AND FA."DocSubType" = 'DM'  THEN 'NotaDebito' ELSE 
CASE WHEN D3."ObjType" = 19 THEN 'NotaCredito' ELSE 
CASE WHEN D3."ObjType" = 204 THEN 'Anticipo' ELSE 'Abono' END END END END as "Tipo", 
D3."InvKey",
CASE WHEN D3."ObjType" = 18 THEN IFNULL(FA."DocNum",-1) else 
CASE WHEN D3."ObjType" = 19 THEN IFNULL(NC."DocNum",-1) ELSE 
CASE WHEN D3."ObjType" = 204 THEN IFNULL(ANT."DocNum",0) ELSE 
CASE WHEN D3."ObjType" = 30 THEN IFNULL(D3."DocNum",0) ELSE 
0 END END END END AS "DocNum",
CASE WHEN D3."ObjType" = 18 THEN IFNULL(FA."TaxDate",CURRENT_DATE) else 
CASE WHEN D3."ObjType" = 19 THEN IFNULL(NC."TaxDate",CURRENT_DATE) ELSE 
CASE WHEN D3."ObjType" = 204 THEN IFNULL(ANT."TaxDate",CURRENT_DATE) ELSE CURRENT_DATE END END END AS "TaxDate",
--CASE WHEN D3."ObjType" = 18 THEN IFNULL(FA.FolioNum,-1) else 
--CASE WHEN D3."ObjType" = 19 THEN IFNULL(NC.FolioNum,-1) ELSE 
--CASE WHEN D3."ObjType" = 204 THEN IFNULL(ANT.FolioNum,-1) ELSE 
CASE WHEN D3."ObjType" = 18 THEN cast(IFNULL(FA."NumAtCard",'-1') as varchar) else 
CASE WHEN D3."ObjType" = 19 THEN cast(IFNULL(NC."NumAtCard",'-1')as varchar) ELSE 
CASE WHEN D3."ObjType" = 204 THEN cast(IFNULL(ANT."NumAtCard",'-1') as varchar)ELSE 
CASE WHEN D3."ObjType" = 30 THEN cast(IFNULL(D3."DocNum",'0') as varchar) ELSE 
'0' END END END END AS "FolioNum", 
H."IdNumber",
H."WizardName",
--IFNULL(BS.UsrNumber1,'-1') as "ModPago",
IFNULL(BP."U_ModPago",'-1') as "ModPago",
H."Status",
H."StatusDisc",
D3."Checked",
H."Canceled",
H."PmntDate" as "FechaPago",
D3."PymNum",
D3."CardCode" as "Proveedor",
BP."LicTradNum" As "RutProveedor",
D3."CardName" as "NombreProveedor",
IFNULL(BP."Address",'No Registra') as "Direccion",
IFNULL(BP."Block",IFNULL(BP."County",'No Registra')) AS "Comuna",
IFNULL(BP."City",'No Registra') AS "Ciudad",
IFNULL(BP."Phone1",' ') AS "Fono",
IFNULL(BP."State2",'13') as "Region",
IFNULL(BP."E_Mail",'No Registra') AS "E_Mail",
IFNULL(EX1."U_BANKCODE_NEW", BP."BankCode") as "BancoAbono",
BA."BankName" as "NombreBnkAbono",
BP."DflAccount" as  "CtaAbono",
IFNULL(BP."DflBranch",'') AS "SucursalAbono",
D3."GLAccCode",
IFNULL(D3."GLAccName",'') AS "GLAcctName",
--gbl 420 CASE WHEN D3."ObjType" = 19 THEN D3."InvPayAmnt" * -1 ELSE D3."InvPayAmnt" END AS "InvPayAmnt",
--CASE WHEN D3."ObjType" = 19 THEN 
--	CASE WHEN D3."LineRate" = 0 THEN D3."InvPayAmnt" * -1 ELSE D3."LineRate" * D3."InvPayAmnt" * -1 END
--	 ELSE 
--	 CASE WHEN D3."LineRate" = 0 THEN D3."InvPayAmnt" ELSE D3."LineRate" * D3."InvPayAmnt" END
--	END AS "InvPayAmnt",
CASE 
	WHEN D3."ObjType" = 19 THEN 
 
		CASE 
		WHEN D3."InvCurr" = D3."PymCurr" THEN D3."InvPayAmnt" * -1 
		ELSE 
			CASE 
			WHEN D3."PymCurr" = (select TOP 1 "MainCurncy" from OADM) THEN
				CASE
				WHEN (select TOP 1 "DirectRate" from OADM)  = 'Y' THEN D3."InvPayAmnt" * ( CASE  D3."LineRate" WHEN 0 THEN 1
				                           ELSE D3."LineRate" END ) * -1 
				ELSE D3."InvPayAmnt" / (CASE  D3."LineRate" WHEN 0 THEN 1
				                           ELSE D3."LineRate" END ) * -1 
				END
			ELSE 
				CASE
				WHEN (select TOP 1 "DirectRate" from OADM)  = 'Y' THEN D3."InvPayAmnt" / ( CASE  D3."LineRate" WHEN 0 THEN 1
				                           ELSE D3."LineRate" END ) * -1 
				ELSE D3."InvPayAmnt" * D3."LineRate" * -1 
				END
			END
		END
	 ELSE 
		CASE 
		WHEN D3."InvCurr" = D3."PymCurr" THEN D3."InvPayAmnt" 
		ELSE 
			CASE 
			WHEN D3."PymCurr" = (select TOP 1 "MainCurncy" from OADM) THEN
				CASE
				WHEN (select TOP 1 "DirectRate" from OADM)  = 'Y' THEN D3."InvPayAmnt" * D3."LineRate"
				ELSE D3."InvPayAmnt" / CASE  D3."LineRate" WHEN 0 THEN 1
				                           ELSE D3."LineRate" END
				END
			ELSE 
				CASE
				WHEN (select TOP 1 "DirectRate" from OADM)  = 'Y' THEN D3."InvPayAmnt" / CASE  D3."LineRate" WHEN 0 THEN 1
				                                                                          ELSE D3."LineRate" END
				ELSE D3."InvPayAmnt" * ( CASE  D3."LineRate" WHEN 0 THEN 1
				                           ELSE D3."LineRate" END )
				END
			END
		END
	END AS "InvPayAmnt",
CASE WHEN D3."ObjType" = 19 THEN D3."TotalLoc" * -1 ELSE D3."TotalLoc" END AS "TotalLoc",
CASE WHEN D3."ObjType" = 19 THEN IFNULL(D3."TotalFC",0) * -1 ELSE IFNULL(D3."TotalFC",0) END  as "TotalFC",
CASE WHEN D3."ObjType" = 19 THEN D3."TotalSys" * -1 ELSE D3."TotalSys" END AS "TotalSys",
D3."PayAmount",
D3."PayAmntFC",
D3."PayAmntSys",
D3."InvCurr",
D3."PymCurr",
IFNULL(BP."U_EXX_TIPOPERS",'') AS "TipoPersona",
IFNULL(BP."U_EXX_TIPODOCU",'') AS "TipoDocum",
IFNULL(BP."U_EXX_APELLPAT",'') AS "Paterno",
IFNULL(BP."U_EXX_APELLMAT",'') AS "Materno",
IFNULL(BP."U_EXX_PRIMERNO",'') AS "Nombre1",
IFNULL(BP."U_EXX_SEGUNDNO",'') AS "Nombre2" ,
FA."NumAtCard",
BS."UsrNumber2" as  "FlagSinglePay", IFNULL(BA1."UsrNumber4",'?')  AS "UsrNumber4", 
IFNULL(BA1."UsrNumber3",'?')  AS "UsrNumber3", 
CASE WHEN D3."ObjType" = 18 THEN IFNULL(FA."ReceiptNum",-1) else 
CASE WHEN D3."ObjType" = 19 THEN IFNULL(NC."ReceiptNum",-1) ELSE 
CASE WHEN D3."ObjType" = 204 THEN IFNULL(ANT."ReceiptNum",0) ELSE 
CASE WHEN D3."ObjType" = 30 THEN IFNULL(D4."RctId",0) ELSE 
0 END END END END AS "ReceiptNum",
CASE WHEN D3."ObjType" = 19 THEN NC."Indicator" 
ELSE FA."Indicator" END  as "Indicator"
from 
OPWZ H JOIN 
PWZ3 D3 ON H."IdNumber" = D3."IdEntry"  RIGHT JOIN 
PWZ4 D4 ON H."IdNumber" = D4."IdEntry" and D4."CardCode" = D3."CardCode" AND D4."PymMeth" = D3."PymMeth" JOIN 
OPYM MT ON D3."PymMeth" = MT."PayMethCod" JOIN 
OCRD BP ON D3."CardCode" = bp."CardCode" LEFT JOIN 
ODSC BA ON BP."BankCode" = ba."BankCode" LEFT JOIN
-- (GBL) ODSC BAP ON MT.BnkDflt = BAP."BankCode"  JOIN  
ODSC BAP ON D4."BnkDflt" = BAP."BankCode"  JOIN 
DSC1 BA1 ON BA1."BankCode" = D4."BnkDflt" AND BA1."Account" = D4."BankAccou" LEFT JOIN 
OPCH FA ON D3."InvKey" = FA."DocEntry" AND FA."ObjType" = 18 LEFT JOIN 
ORPC NC ON D3."InvKey" = NC."DocEntry" AND NC."ObjType" = 19 LEFT JOIN 
ODPO ANT ON D3."InvKey" = ANT."DocEntry" AND ANT."ObjType" = 204 LEFT JOIN 
OCRB BS ON BS."CardCode" =BP."CardCode" AND BS."Account" = BP."DflAccount" AND BS."BankCode" = BP."BankCode"  --JOIN -- <<--- Campos Ecuador 
LEFT JOIN "@EXPM_BANKCODES" EX1 ON BP."BankCode" = EX1."U_BANKCODE_OUT" AND D4."BnkDflt" = EX1."U_BANKCODE_IN"
WHERE D3."Checked" = 'Y' AND H."Canceled" = 'N' 
AND D3."InvKey" NOT IN (select "InvID" from "PWZ5" WHERE "IdEntry" = D3."IdEntry" and "CardCode" = D3."CardCode") ) AS a
group by
"Branch",
"GLAccCode",
"MetodoPago",
"BancoCargo",
"NombreBnkCargo",
"CtaCargo",
"SucursalCargo",
"ObjType",
"DocSubType",
"Tipo",
"InvKey",
"DocNum",
"TaxDate",
"FolioNum",
"IdNumber",
"WizardName",
"ModPago",
"Status",
"StatusDisc",
"Checked",
"Canceled",
"FechaPago",
--pymNum,
"Proveedor",
"RutProveedor",
"NombreProveedor",
"Direccion",
"Comuna",
"Ciudad",
"Fono",
"Region",
"E_Mail",
"BancoAbono",
"NombreBnkAbono",
"CtaAbono",
"SucursalAbono",
"InvCurr",
"PymCurr",
"TipoPersona",
"TipoDocum",
"Paterno",
"Materno",
"Nombre1",
"Nombre2",
"NumAtCard",
"FlagSinglePay", 
"UsrNumber4", 
"UsrNumber3", 
"ReceiptNum", 
"Indicator"



