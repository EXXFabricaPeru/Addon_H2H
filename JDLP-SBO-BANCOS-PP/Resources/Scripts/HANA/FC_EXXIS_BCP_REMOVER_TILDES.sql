﻿CREATE FUNCTION "FC_EXXIS_BCP_REMOVER_TILDES" (IN Cadena VARCHAR(254))
RETURNS result VARCHAR(254)
LANGUAGE SQLSCRIPT
AS
BEGIN
-- Replace accent marks
result= REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(UPPER(Cadena), 'Á', 'A'), 'É','E'), 'Í', 'I'), 'Ó', 'O'), 'Ú','U'),'Ñ','N');
END;