﻿CREATE PROCEDURE "SP_EXXIS_PLANILLA_BCP_L_CORREOUSUARIO_BY_ACCION"
(
IN vCA NVARCHAR(10),
IN vPLG NVARCHAR(10)
)
LANGUAGE SQLSCRIPT
AS
BEGIN
    SELECT * FROM "@EXX_BCP_HOST" 
    WHERE "U_HTH_PLG" = :vPLG
    --AND "U_HTH_CA" = :vCA
    AND IFNULL("U_HTH_NAR2", '') <> ''
    AND IFNULL("U_HTH_NAD", '') <> '' 
    AND IFNULL("U_HTH_COR", 'N') = 'N';
END;
