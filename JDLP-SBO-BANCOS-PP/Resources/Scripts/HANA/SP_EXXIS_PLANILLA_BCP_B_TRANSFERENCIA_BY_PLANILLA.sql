CREATE PROCEDURE "SP_EXXIS_PLANILLA_BCP_B_TRANSFERENCIA_BY_PLANILLA"
(
IN vPlanilla NVARCHAR(50)
)
LANGUAGE SQLSCRIPT
AS
BEGIN
    SELECT 
        '1' AS "tipoRegistro",
        '000001' AS "secuenciaFila",
        RIGHT('000000' || TO_VARCHAR(COUNT(T0."TrsfrAcct")), 6) AS "cantidadCuentasArchivo",
        'ID' || TO_VARCHAR(CURRENT_DATE, 'YYYYMMDDHH24MISS') AS "identificadorCabecera",
        '' AS "filler"
    FROM "OVPM" T0
    INNER JOIN "OCRD" T1 ON T0."CardCode" = T1."CardCode" 
    WHERE T0."DocEntry" IN (SELECT TZ."U_DocEntry" FROM "@EXX_BCP_TRAN_OVPM" TZ WHERE TZ."U_HTH_PL" = vPlanilla );
END
