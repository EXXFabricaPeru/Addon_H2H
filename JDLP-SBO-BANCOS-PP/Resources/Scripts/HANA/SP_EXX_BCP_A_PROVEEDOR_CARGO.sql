CREATE PROCEDURE "SP_EXX_BCP_A_PROVEEDOR_CARGO"
(
IN vCode NVARCHAR(50),
IN vName NVARCHAR(100),
IN vU_HTH_PL NVARCHAR(254),
IN vU_HTP_PC NVARCHAR(254),
IN vU_101_TR NVARCHAR(1),
IN vU_102_CA NVARCHAR(6),
IN vU_103_FP NVARCHAR(8),
IN vU_104_TC NVARCHAR(1),
IN vU_105_MC NVARCHAR(4),
IN vU_106_NC NVARCHAR(20),
IN vU_107_MT NVARCHAR(17),
IN vU_108_RP NVARCHAR(40),
IN vU_109_FE NVARCHAR(1),
IN vU_110_TC NVARCHAR(15),
IN vCCHK NVARCHAR(30)
)
LANGUAGE SQLSCRIPT
AS
BEGIN
    INSERT INTO "@EXX_BCP_PROV_CAR"
               ("Code", "Name", "U_HTH_PL", "U_HTP_PC", "U_101_TR", "U_102_CAP", "U_103_FP", "U_104_TCC", 
                "U_105_MCC", "U_106_NCC", "U_107_MTP", "U_108_RP", "U_109_FEITF", "U_110_TC", "U_CCHK")
         VALUES
               (vCode, vName, vU_HTH_PL, vU_HTP_PC, vU_101_TR, vU_102_CA, vU_103_FP, vU_104_TC, 
                vU_105_MC, vU_106_NC, vU_107_MT, vU_108_RP, vU_109_FE, vU_110_TC, vCCHK);
END;
