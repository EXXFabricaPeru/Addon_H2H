CREATE PROCEDURE "SP_EXXIS_PLANILLA_BCP_B_TRANSFERENCIA_ABONO_BY_PLANILLA_CUENTA_MONEDA"    
(    
IN vPlanilla NVARCHAR(50),    
IN vNumeroCuenta NVARCHAR(30),    
IN vMoneda NVARCHAR(3)    
)    
LANGUAGE SQLSCRIPT    
AS    
BEGIN    
    SELECT     
        '3' AS "tipoRegistro",    
        '000003' AS "secuencialFila",    
        CASE UPPER(SUBSTRING(IFNULL(T3."Branch", '-'), 1, 1))    
            WHEN 'A' THEN 'A'    
            WHEN 'M' THEN 'M'    
            WHEN 'C' THEN 'C'    
            WHEN 'I' THEN 'B'    
            ELSE IFNULL(T3."Branch", '')   
        END AS "tipoCuenta",    
        CASE UPPER(SUBSTRING(IFNULL(T3."Branch", '-'), 1, 1))  
            WHEN 'I' THEN CAST('' AS CHAR(1))  
            ELSE 'N'   
        END AS cuentaPropia,  
        CAST(REPLACE(T3."Account", '-', '') AS CHAR(20)) AS numeroCuentaAbono,    
        CAST(REPLACE(T1."Account", '-', '') AS CHAR(20)) AS numeroCuentaCargo,  
        CASE LENGTH(REPLACE(T3."Account", '-', ''))  
            WHEN 13 THEN RIGHT(REPLACE(T3."Account", '-', ''), 10)  
            WHEN 14 THEN RIGHT(REPLACE(T3."Account", '-', ''), 11)  
            WHEN 20 THEN RIGHT(REPLACE(T3."Account", '-', ''), 10)  
            ELSE REPLACE(T3."Account", '-', '') 
        END AS abonoChkSum,  
        CASE TRIM(T2."U_EXX_TIPODOCU")    
            WHEN '1' THEN 'DNI'    
            WHEN '4' THEN 'CE'      
            WHEN '6' THEN 'RUC'    
            WHEN '7' THEN 'PAS'     
            WHEN 'A' THEN 'CE'      
            ELSE 'ABC' 
        END AS tipoDocumentoBeneficiario,    
        CAST(T2."LicTradNum" AS CHAR(12)) AS numeroDocumentoBeneficiario,    
        CAST('' AS CHAR(3)) AS correlativoDocumento,    
        CASE T2."U_EXX_TIPODOCU"    
            WHEN '6' THEN LEFT("FC_EXXIS_BCP_REMOVER_TILDES"(T2."CardName"), 75)    
            ELSE LEFT("FC_EXXIS_BCP_REMOVER_TILDES"(T2."U_EXX_PRIMERNO") || ' ' ||  
              "FC_EXXIS_BCP_REMOVER_TILDES"(T2."U_EXX_APELLPAT") || ' ' ||  
                "FC_EXXIS_BCP_REMOVER_TILDES"(T2."U_EXX_APELLMAT"), 75)    
        END AS nombreBeneficiario,    
        CASE T0."DocCurr"    
            WHEN 'SOL' THEN '0001'    
            ELSE '1001' 
        END AS monedaMontoTransferir,    
        CASE T0."DocCurr"    
            WHEN 'SOL' THEN RIGHT('00000000000000000' ||  
                TO_VARCHAR(CAST(CAST(T0."DocTotal" AS DECIMAL(19,2)) AS VARCHAR(20))), 17)    
            ELSE RIGHT('00000000000000000' ||  
                TO_VARCHAR(CAST(CAST(T0."DocTotalFC" AS DECIMAL(19,2)) AS VARCHAR(20))), 17) 
        END AS montoOperacion,    
        CAST('' AS CHAR(40)) AS referencia,    
        CAST('IDABONO' || TO_VARCHAR(CURRENT_DATE, 'YYYYMMDD') ||  
            RIGHT('0000' || TO_VARCHAR(ROW_NUMBER() OVER(ORDER BY T0."DocCurr" ASC)), 4) AS CHAR(32)) AS identificadorAbono,  
        CASE T3."UsrNumber2"
            WHEN 'S' THEN 'N'
            WHEN 'N' THEN 'S'
            ELSE 'N' 
        END AS titularCuenta, 
        CAST(IFNULL(T2."E_Mail", '') AS CHAR(100)) AS emailClienteBCP,    
        '' AS filler    
    FROM  "OVPM" T0    
    INNER JOIN "DSC1" T1 ON T0."TrsfrAcct" = T1."GLAccount"    
    INNER JOIN "OCRD" T2 ON T0."CardCode" = T2."CardCode"     
    INNER JOIN "OCRB" T3 ON T2."CardCode" = T3."CardCode"  AND T2."BankCode" = T3."BankCode"    
    WHERE     
        REPLACE(T1."Account", '-', '') = vNumeroCuenta    
        AND  T0."DocCurr" = vMoneda    
        AND T0."DocEntry" IN (SELECT Z1."U_DocEntry" FROM "@EXX_BCP_TRAN_OVPM" Z1 WHERE Z1."U_HTH_PL" = vPlanilla );    
END;
