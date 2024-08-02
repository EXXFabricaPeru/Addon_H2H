CREATE PROCEDURE "SP_EXXIS_PLANILLA_BCP_L_TRANSFERENCIA_WIZARD_BY_FECHA_2"
(
    IN FI NVARCHAR(100),
    IN FF NVARCHAR(100)
)
LANGUAGE SQLSCRIPT
AS
BEGIN
    SELECT    
        '0' AS "Marcar",
        T0."DocEntry" AS "Entry",
        T0."DocNum" AS "Numero",
        T0."CardCode" AS "Codigo",
        COALESCE(T0."CardName", '') AS "Proveedor",
        T0."DocDate" AS "Fecha",
        'T' AS "Movimiento",
        CASE T0."DocCurr"
            WHEN 'SOL' THEN CAST(T0."DocTotal" AS DECIMAL(19,4))
            ELSE CAST(T0."DocTotalFC" AS DECIMAL(19,4))
        END AS "Valor",
        COALESCE(T0."Comments", '') AS "Comentario",
        COALESCE(T0."JrnlMemo", '') AS "Comentario2",
        T0."DocCurr" AS "Moneda",
        T0."TrsfrAcct" AS "Cuenta",
        T0."DocType"
    FROM "OVPM" T0
    INNER JOIN "DSC1" T1 ON T0."TrsfrAcct" = T1."GLAccount"
    INNER JOIN "OCRD" T2 ON T0."CardCode" = T2."CardCode"
    INNER JOIN "OCRB" T3 ON T2."CardCode" = T3."CardCode" AND T2."BankCode" = T3."BankCode" AND T0."DocCurr" = T3."U_CurrSAP"
    WHERE   
        T0."DocDate" BETWEEN TO_DATE(:FI, 'YYYY-MM-DD') AND TO_DATE(:FF, 'YYYY-MM-DD')
        AND (
            (CASE T0."DocCurr" WHEN 'SOL' THEN T0."CashSum" ELSE T0."CashSumFC" END = 0 AND   
            CASE T0."DocCurr" WHEN 'SOL' THEN T0."CreditSum" ELSE T0."CredSumFC" END = 0 AND  
            CASE T0."DocCurr" WHEN 'SOL' THEN T0."CheckSum" ELSE T0."CheckSumFC" END = 0)
        )
        AND T0."CardName" LIKE '%EVO%'
    ORDER BY T0."CardName" ASC;
END;
