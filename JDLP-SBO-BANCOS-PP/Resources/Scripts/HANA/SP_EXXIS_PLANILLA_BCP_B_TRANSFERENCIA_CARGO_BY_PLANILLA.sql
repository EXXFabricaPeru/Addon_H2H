CREATE PROCEDURE "SP_EXXIS_PLANILLA_BCP_B_TRANSFERENCIA_CARGO_BY_PLANILLA"  
(  
IN vPlanilla NVARCHAR(50)  
--IN vCuenta NVARCHAR(1),  
--IN vMoneda NVARCHAR(3)  
)  
LANGUAGE SQLSCRIPT  
AS  
BEGIN  
  
 SELECT   
  '2' AS "tipoRegistro",  
  '000002' AS "secuencialFila", -- correlativo al crear txt  
  T0."DocCurr" AS "Moneda",  
  RIGHT('0000000' ||  
  TO_VARCHAR(COUNT(T1."Account")), 6) AS "cantidadAbonosPlanilla",  
  T1."UsrNumber1" AS "tipoCuentaCargo",  
  CASE T0."DocCurr"  
   WHEN 'SOL' THEN '0001'  
   ELSE '1001' END AS "monedaCuentaCargo",  
   CAST(REPLACE(T1."Account",'-','') AS CHAR(20)) AS "numeroCuentaCargo",
   
   CASE LENGTH(REPLACE(T1."Account",'-',''))
	 WHEN 13 THEN RIGHT(REPLACE(T1."Account",'-',''),10)
	 WHEN 20 THEN RIGHT(REPLACE(T1."Account",'-',''),10)
	ELSE RIGHT(REPLACE(T1."Account",'-',''),10) END AS "cargoChkSum",

   CASE T0."DocCurr"  
   WHEN 'SOL' THEN RIGHT('00000000000000000' || 
	TO_VARCHAR(SUM(CAST(T0."DocTotal" AS DECIMAL(19,2)))), 17)  
   ELSE RIGHT('00000000000000000' || 
	TO_VARCHAR(SUM(CAST(0 AS DECIMAL(19,2)))), 17)    
   END AS "montoTotalSoles",  
   CASE T0."DocCurr"  
   WHEN 'SOL' THEN RIGHT('00000000000000000' || 
	TO_VARCHAR(SUM(CAST(0 AS DECIMAL(19,2)))), 17)    
   ELSE RIGHT('00000000000000000' || 
	TO_VARCHAR(SUM(CAST(T0."DocTotalFC" AS DECIMAL(19,2)))), 17)   
   END AS "montoTotalDolares",  
     
  RIGHT('000000000000000' || '32478546616', 15) AS "totalControl",  
  CAST('IDCARGO' || TO_VARCHAR(CURRENT_DATE, 'YYYYMMDD') ||
	RIGHT('0000' || TO_VARCHAR(ROW_NUMBER() OVER(ORDER BY T0."DocCurr" ASC)), 4) AS CHAR(32)) AS "identificadorCargo",
  '' AS "filler"  
  FROM  "OVPM" T0  
 INNER JOIN "DSC1" T1 ON T0."TrsfrAcct" = T1."GLAccount"  
 INNER JOIN "OCRD" T2 ON T0."CardCode" = T2."CardCode"   
 INNER JOIN "OCRB" T3 ON T2."CardCode" = T3."CardCode"  AND T2."BankCode" = T3."BankCode"  
 WHERE   
 --T1."UsrNumber1" = vCuenta  
 --AND T0."DocCurr" = vMoneda  
 --AND T1."Branch" = vMoneda  
  IFNULL(T1."Branch", '-99') != '99'  
  AND T0."DocEntry" IN (SELECT Z1."U_DocEntry" FROM "@EXX_BCP_TRAN_OVPM" Z1 WHERE Z1."U_HTH_PL" = vPlanilla)  
 GROUP BY T0."DocCurr",T1."UsrNumber1",T1."Account";  
END;
