CREATE PROCEDURE "SP_EXX_BCP_A_TRANSFERENCIA_OVPM"
(
IN vCode NVARCHAR(50),
IN vName NVARCHAR(100),
IN vDocEntry NVARCHAR(25),
IN vDocNum NVARCHAR(25),
IN vDocDate NVARCHAR(20),
IN vDocCurr NVARCHAR(10),
IN vDocTotal NVARCHAR(25),
IN vTrsfrAcct NVARCHAR(20),
IN vHTH_PL NVARCHAR(50)
)
LANGUAGE SQLSCRIPT
AS
BEGIN 
    INSERT INTO "@EXX_BCP_TRAN_OVPM"
    (
        "Code",
        "Name",
        "U_DocEntry",
        "U_DocNum",
        "U_DocDate",
        "U_DocCurr",
        "U_DocTotal",
        "U_TrsfrAcct",
        "U_HTH_PL"
    )
    VALUES
    (
        vCode,
        vName,
        vDocEntry,
        vDocNum,
        vDocDate,
        vDocCurr,
        vDocTotal,
        vTrsfrAcct,
        vHTH_PL
    );
END;
