CREATE PROCEDURE "SP_EXX_BCP_A_PROVEEDOR_DOCUMENTO"
(
IN vCode NVARCHAR(50),
IN vName NVARCHAR(100),
IN vU_HTH_PL NVARCHAR(254),
IN vU_HTP_PC NVARCHAR(254),
IN vU_HTA_PA NVARCHAR(254),
IN vU_HTD_PD NVARCHAR(254),
IN vU_301_TR NVARCHAR(1),
IN vU_302_TDP NVARCHAR(1),
IN vU_303_NDP NVARCHAR(15),
IN vU_304_IDP NVARCHAR(17)
)
LANGUAGE SQLSCRIPT
AS
BEGIN
    INSERT INTO "@EXX_BCP_PROV_DOC"
               ("Code", "Name", "U_HTH_PL", "U_HTP_PC", "U_HTA_PA", "U_HTD_PD", "U_301_TR", "U_302_TDP", "U_303_NDP", "U_304_IDP")
         VALUES
               (vCode, vName, vU_HTH_PL, vU_HTP_PC, vU_HTA_PA, vU_HTD_PD, vU_301_TR, vU_302_TDP, vU_303_NDP, vU_304_IDP);
END;