CREATE PROCEDURE "SP_EXX_BCP_A_WIZARD"
(
IN vCode NVARCHAR(50),
IN vName NVARCHAR(100),
IN vHTH_PL NVARCHAR(254),
IN vHTH_CO NVARCHAR(254),
IN vHTH_PR CHAR(1),
IN vHTH_FC NVARCHAR(50),
IN vHTH_FP NVARCHAR(50),
IN vHTH_US NVARCHAR(50),
IN vHTH_CA NVARCHAR(1),
IN vHTH_NAE NVARCHAR(254),
IN vHTH_NAR1 NVARCHAR(254),
IN vHTH_NAR2 NVARCHAR(254),
IN vHTH_PLH NVARCHAR(254),
IN vHTH_WIZ NVARCHAR(254),
IN vHTH_WIZFI NVARCHAR(254),
IN vHTH_WIZFF NVARCHAR(254)
)
LANGUAGE SQLSCRIPT
AS
BEGIN
    INSERT INTO "@EXX_BCP_HOST"
               ("Code", "Name", "U_HTH_PL", "U_HTH_CO", "U_HTH_PR", "U_HTH_FC", "U_HTH_FP", "U_HTH_US",
                "U_HTH_CA", "U_HTH_NAE", "U_HTH_NAR1", "U_HTH_NAR2", "U_HTH_PLH", "U_HTH_WIZ", "U_HTH_WIZFI", "U_HTH_WIZFF")
         VALUES
               (vCode, vName, vHTH_PL, vHTH_CO, vHTH_PR, vHTH_FC, vHTH_FP, vHTH_US,
                vHTH_CA, vHTH_NAE, vHTH_NAR1, vHTH_NAR2, vHTH_PLH, vHTH_WIZ, vHTH_WIZFI, vHTH_WIZFF);
END;