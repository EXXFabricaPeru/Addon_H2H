CREATE PROCEDURE "SP_EXXIS_PLANILLA_BCP_L_PROVEEDOR_ABONO_BY_WIZARD_AND_FECHA"
(
IN vPlanillaSAP NVARCHAR(100)
--IN vFecha NVARCHAR(10)
)
LANGUAGE SQLSCRIPT
AS
BEGIN
    SELECT   
        '2' AS "2.01_TipoRegistro",  
        CASE T1."SucursalAbono"  
            WHEN 'Corriente' THEN 'C'   
            WHEN 'Ahorro' THEN 'A'   
            WHEN 'Interbancario' THEN 'B'   
            WHEN 'Maestra' THEN 'M'   
            WHEN 'Cheque de gerencia' THEN ''   
        END AS "2.02_TipoCuentaAbono",  
  
        TO_NVARCHAR(REPLACE(T1."CtaAbono", '-', ''), 20) AS "2.03_NumeroCuentaAbono",  
  
        '1' AS "2.04_ModalidadPago",  
        CASE T2."U_EXX_TIPODOCU"  
            WHEN '0' THEN '' -- OTROS TIPOS DE DOCUMENTOS   
            WHEN '1' THEN '1' -- DNI  
            WHEN '4' THEN '' -- CARNET DE EXTRANJERIA  
            WHEN '6' THEN '6' --RUC  
            WHEN '7' THEN '4' -- PASAPORTE  
        END AS "2.05_TipoDocumentoProveedor",  
        TO_NVARCHAR(T1."RutProveedor", 12) AS "2.06_NumeroDocumentoProveedor",  
        TO_NVARCHAR('', 3) AS "2.07_CorrelativoDocumentoProveedor",   
  
         CASE T2."U_EXX_TIPODOCU"  
        WHEN '6' THEN LEFT(TO_NVARCHAR(UPPER("FC_EXXIS_BCP_REMOVER_TILDES"(T1."NombreProveedor")), 75), 75)  
        ELSE LEFT(TO_NVARCHAR(UPPER("FC_EXXIS_BCP_REMOVER_TILDES"(T2."U_EXX_PRIMERNO") || ' ' || "FC_EXXIS_BCP_REMOVER_TILDES"(T2."U_EXX_APELLPAT") || ' ' || "FC_EXXIS_BCP_REMOVER_TILDES"(T2."U_EXX_APELLMAT")), 75), 75)  
    	END AS "2.08_NombreProveedor" ,
  
            TO_NVARCHAR('', 40) AS "2.09_ReferenciaBeneficiario",  
    TO_NVARCHAR('', 20) AS "2.10_ReferenciaEmpresa",
        CASE T1."InvCurr"  
            WHEN 'SOL' THEN '0001'  
            WHEN 'USD' THEN '1001'  
        END AS "2.11_MonedaImporteAbonar",  
        TO_NVARCHAR(RIGHT('00000000000000000' || TO_NVARCHAR(SUM(T1."InvPayAmnt"), 100), 17), 17) AS "2.12_ImporteAbonar",  
    TO_NVARCHAR('S', 1) AS "2.13_FlagValidarIDC",  
  
        CASE LENGTH(REPLACE(T1."CtaAbono", '-', ''))  
            WHEN 13 THEN RIGHT(REPLACE(T1."CtaAbono", '-', ''), 10)  
            WHEN 14 THEN RIGHT(REPLACE(T1."CtaAbono", '-', ''), 11)  
            WHEN 20 THEN RIGHT(REPLACE(T1."CtaAbono", '-', ''), 10)  
            ELSE RIGHT(REPLACE(T1."CtaAbono", '-', ''), 10)  
        END AS "abonoChkSum",
        (SELECT P4."PymNum" FROM "PWZ4" P4 
         JOIN "OPWZ" P ON P."IdNumber"=P4."IdEntry"
         WHERE P."WizardName" IN (SELECT "U_WIZNAME" FROM "@EXX_BCP_PROV_PAGO" WHERE "U_HTH_PL"=:vPlanillaSAP) 
         AND P4."CardCode"=T1."Proveedor") AS "orderProv"
    FROM "PM_DETALLE_PAGOS" T1 
    INNER JOIN "OCRD" T2 ON TRIM(T1."Proveedor") = TRIM(T2."CardCode")
    WHERE T1."WizardName" IN (SELECT "U_WIZNAME" FROM "@EXX_BCP_PROV_PAGO" WHERE "U_HTH_PL"=:vPlanillaSAP)
    AND T2."CardType" = 'S'  
    AND LEFT(T2."CardCode", 1) ='P'   
    GROUP BY   
        T1."SucursalAbono",  
        T1."CtaAbono",  
        T2."U_EXX_TIPODOCU",  
        T1."RutProveedor",  
        T1."NombreProveedor",  
        T2."U_EXX_PRIMERNO",  
        T2."U_EXX_APELLPAT",  
        T2."U_EXX_APELLMAT",  
        T1."InvCurr",  
        T1."Proveedor" 
    ORDER BY T1."RutProveedor";
END;