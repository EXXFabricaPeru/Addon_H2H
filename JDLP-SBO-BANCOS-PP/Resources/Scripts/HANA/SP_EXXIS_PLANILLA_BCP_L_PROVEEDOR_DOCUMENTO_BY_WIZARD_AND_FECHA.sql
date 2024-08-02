CREATE PROCEDURE "SP_EXXIS_PLANILLA_BCP_L_PROVEEDOR_DOCUMENTO_BY_WIZARD_AND_FECHA"
(
    IN planillaSAP NVARCHAR(100),
    IN vRutProveedor NVARCHAR(20)
)
LANGUAGE SQLSCRIPT
AS
BEGIN
    SELECT 
        '3' AS "3.01_TipoRegistro",
        CASE T1."Tipo"
            WHEN 'Factura' THEN 'F'
            WHEN 'NotaCredito' THEN 'N'
            WHEN 'NotaDebito' THEN 'C'
            WHEN 'Anticipo' THEN 'F'
            WHEN 'Abono' THEN ''
            WHEN 'Factura de la empresa' THEN ''
            WHEN 'Nota crédito empresa' THEN ''
            WHEN 'Nota débito empresa' THEN ''
            WHEN 'Cobranza' THEN ''
            WHEN 'Otros' THEN ''
        END AS "3.02_TipoDocumentoPagar",
        REPLACE(T1."NumAtCard", '-', '') AS "3.03_NumeroDocumentoPagar",
        LPAD(CAST(CAST(T1."InvPayAmnt" AS DECIMAL(19,2)) AS NVARCHAR(100)), 17, '0') AS "3.04_ImporteDocumentoPagar"
    FROM 
        "PM_DETALLE_PAGOS" T1
    WHERE
        TRIM(T1."RutProveedor") = TRIM(:vRutProveedor)
        AND T1."WizardName" IN (SELECT "U_WIZNAME" FROM "@EXX_BCP_PROV_PAGO" WHERE "U_HTH_PL" = :planillaSAP);
END;