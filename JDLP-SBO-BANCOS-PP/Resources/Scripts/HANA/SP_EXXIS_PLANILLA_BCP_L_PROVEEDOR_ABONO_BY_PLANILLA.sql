CREATE PROCEDURE "SP_EXXIS_PLANILLA_BCP_L_PROVEEDOR_ABONO_BY_PLANILLA"
(
IN vPlanilla NVARCHAR(254)
)
LANGUAGE SQLSCRIPT
AS
BEGIN 
    --SELECT T0.* FROM "@EXX_BCP_PROV_ABO" T0 WHERE T0."U_HTH_PL" = :vPlanilla;
    SELECT X.*, 
           (SELECT P4."PymNum" FROM "PWZ4" P4 
            JOIN "OPWZ" P ON P."IdNumber"=P4."IdEntry"
            JOIN "OCRD" C ON C."LicTradNum"=X."U_206_NDP"
            WHERE P."WizardName" IN (SELECT "U_WIZNAME" FROM "@EXX_BCP_PROV_PAGO" WHERE "U_HTH_PL"=X."U_HTH_PL") 
            AND P4."CardCode"=C."CardCode") AS "orderProv"
    FROM (
        SELECT T0.* FROM "@EXX_BCP_PROV_ABO" T0 
        WHERE T0."U_HTH_PL" = :vPlanilla
    ) AS X
    ORDER BY "orderProv";
END;
