CREATE VIEW "PM_RESUMEN_PAGOS" AS 
SELECT
    "Branch" AS "Sucursal",
    "WizardName" AS "Ejecucion",
    "StatusDisc",
    "BancoCargo" AS "Banco",
    "NombreBnkCargo" AS "Nombre",
    "FechaPago" AS "Fecha",
    SUM("InvPayAmnt") AS "MontoDocs",
    SUM("InvPayAmnt") AS "MontoPago",
    "GLAccCode" AS "CtaMayor"
FROM "PM_DETALLE_PAGOS"
GROUP BY 
    "Branch",
    "WizardName",
    "StatusDisc", 
    "BancoCargo",
    "NombreBnkCargo",
    "FechaPago", 
    "GLAccCode";
