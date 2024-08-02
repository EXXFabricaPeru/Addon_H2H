CREATE PROCEDURE "SP_EXX_BCP_A_PROVEEDOR_ABONO"
(
IN vCode NVARCHAR(50),
IN vName NVARCHAR(100),
IN vU_HTH_PL NVARCHAR(254),
IN vU_HTP_PC NVARCHAR(254),
IN vU_HTA_PA NVARCHAR(254),
IN vU_201_TR NVARCHAR(1),
IN vU_202_TCA NVARCHAR(1),
IN vU_203_NCA NVARCHAR(20),
IN vU_204_MP NVARCHAR(1),
IN vU_205_TDP NVARCHAR(1),
IN vU_206_NDP NVARCHAR(12),
IN vU_207_CDP NVARCHAR(3),
IN vU_208_NP NVARCHAR(75),
IN vU_209_RB NVARCHAR(40),
IN vU_210_RE NVARCHAR(20),
IN vU_211_MIA NVARCHAR(4),
IN vU_212_IA NVARCHAR(17),
IN vU_213_FVIDC NVARCHAR(1),
IN vACHK NVARCHAR(30)
)
LANGUAGE SQLSCRIPT
AS
BEGIN
    INSERT INTO "@EXX_BCP_PROV_ABO"
               ("Code", "Name", "U_HTH_PL", "U_HTP_PC", "U_HTA_PA", "U_201_TR", "U_202_TCA", "U_203_NCA", 
                "U_204_MP", "U_205_TDP", "U_206_NDP", "U_207_CDP", "U_208_NP", "U_209_RB", "U_210_RE", 
                "U_211_MIA", "U_212_IA", "U_213_FVIDC", "U_ACHK")
         VALUES
               (vCode, vName, vU_HTH_PL, vU_HTP_PC, vU_HTA_PA, vU_201_TR, vU_202_TCA, vU_203_NCA, 
                vU_204_MP, vU_205_TDP, vU_206_NDP, vU_207_CDP, vU_208_NP, vU_209_RB, vU_210_RE, 
                vU_211_MIA, vU_212_IA, vU_213_FVIDC, vACHK);
END;
