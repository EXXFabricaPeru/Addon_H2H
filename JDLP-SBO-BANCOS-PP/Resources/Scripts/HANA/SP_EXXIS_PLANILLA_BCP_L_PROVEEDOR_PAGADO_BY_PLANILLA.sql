﻿CREATE PROCEDURE "SP_EXXIS_PLANILLA_BCP_L_PROVEEDOR_PAGADO_BY_PLANILLA"
(
    IN vPlanilla NVARCHAR(254)
)
LANGUAGE SQLSCRIPT
AS
BEGIN
    SELECT * 
    FROM "@EXX_BCP_PROV_PAGO" T0 
    WHERE T0."U_HTH_PL" = :vPlanilla;
END;
