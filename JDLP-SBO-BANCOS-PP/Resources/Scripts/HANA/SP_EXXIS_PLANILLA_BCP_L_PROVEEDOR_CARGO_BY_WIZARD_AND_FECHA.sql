CREATE PROCEDURE "SP_EXXIS_PLANILLA_BCP_L_PROVEEDOR_CARGO_BY_WIZARD_AND_FECHA"  
(
    IN planillaSAP NVARCHAR(100)
    --@Fecha NVARCHAR(10)
)
LANGUAGE SQLSCRIPT
AS
BEGIN
    SELECT 
        '1' AS "1.01_TipoRegistro",
        RIGHT('000000' || TO_NVARCHAR(7, COUNT(DISTINCT T1."RutProveedor")), 6) AS "1.02_CantidadAbonosPlanilla", 
        TO_NVARCHAR(CURRENT_DATE, 'YYYYMMDD') AS "1.03_FechaProceso",
        'C' AS "1.04_TipoCuentaCargo",
        CASE T1."PymCurr"
            WHEN 'SOL' THEN '0001'
            ELSE '1001' 
        END AS "1.05_MonedaCuentaCargo",
        TO_NVARCHAR(20, REPLACE(T1."CtaCargo", '-', '')) AS "1.06_NumeroCuentaCargo", 
        RIGHT('00000000000000000' || TO_NVARCHAR(17, SUM(T1."InvPayAmnt")), 17) AS "1.07_MontoTotalPlanilla",
        LTRIM(RTRIM((SELECT TOP 1 "U_HTH_CO" FROM  "@EXX_BCP_HOST" WHERE "U_HTH_PL" = :planillaSAP))) AS "1.08_ReferenciaPlanilla",
        'N' AS "1.09_FlagExoneracionITF",
        TO_NVARCHAR(15, RIGHT('000000000000000' || TO_NVARCHAR(17, '000001100000000'), 15)) AS "1.10_TotalControl",
        CASE LENGTH(REPLACE(T1."CtaCargo", '-', ''))
            WHEN 13 THEN RIGHT(REPLACE(T1."CtaCargo", '-', ''), 10)
            WHEN 20 THEN RIGHT(REPLACE(T1."CtaCargo", '-', ''), 10)
            ELSE RIGHT(REPLACE(T1."CtaCargo", '-', ''), 10)
        END AS "cargoChkSum"
    FROM 
        /*[dbo].[PM_RESUMEN_PAGOS] T0*/
        "PM_DETALLE_PAGOS" T1 -- ON T0.Ejecucion = T1.WizardName
        /*INNER JOIN OACT T2 ON T0.CtaMayor = T2.AcctCode*/
    WHERE 
        T1."WizardName" IN (SELECT "U_WIZNAME" FROM "@EXX_BCP_PROV_PAGO" WHERE "U_HTH_PL" = :planillaSAP)
    GROUP BY 
        /*T2.ActCurr*/
        T1."CtaCargo",
        T1."PymCurr";
END;