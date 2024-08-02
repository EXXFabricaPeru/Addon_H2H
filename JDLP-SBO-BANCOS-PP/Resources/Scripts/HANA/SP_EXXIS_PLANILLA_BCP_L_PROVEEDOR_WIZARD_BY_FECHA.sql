CREATE PROCEDURE "SP_EXXIS_PLANILLA_BCP_L_PROVEEDOR_WIZARD_BY_FECHA"
(
    IN FI NVARCHAR(100),
    IN FF NVARCHAR(100)
)
LANGUAGE SQLSCRIPT
AS
BEGIN
    SELECT
        '0' AS "Marcar",
        T0."Ejecucion" AS "Wizard",
        T0."StatusDisc" AS "Estado",
        T0."Banco" AS "Banco",
        T0."Nombre" AS "Nombre",
        T0."Fecha" AS "Fecha",
        CAST(T0."MontoPago" AS DECIMAL(19,2)) AS "Monto",
        T0."CtaMayor" AS "Codigo",
        T1."FormatCode" AS "Cuenta"
    FROM "PM_RESUMEN_PAGOS" T0
    INNER JOIN "OACT" T1 ON T0."CtaMayor" = T1."AcctCode"
    WHERE 
        TO_DATE(T0."Fecha", 'YYYY-MM-DD') >= TO_DATE(:FI, 'YYYY-MM-DD')
        AND TO_DATE(T0."Fecha", 'YYYY-MM-DD') <= TO_DATE(:FF, 'YYYY-MM-DD')
        AND T0."Ejecucion" NOT IN (SELECT "U_WIZNAME" FROM "@EXX_BCP_PROV_PAGO");
END;