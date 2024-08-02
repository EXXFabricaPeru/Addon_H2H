CREATE PROCEDURE "SP_EXXIS_PLANILLA_BCP_L_HOST_BY_FECHA_PROCESO"      
(      
IN vFI NVARCHAR(10),      
IN vFF NVARCHAR(10),      
IN vProceso NVARCHAR(5)      
)      
LANGUAGE SQLSCRIPT      
AS      
BEGIN      
 SELECT
 '0'		AS "Marcar",
 T0."U_HTH_PL" AS "Planilla",    
 T0."U_HTH_CO" AS "Nombre",    
 CASE IFNULL(T0."U_HTH_PLG",'')    
  WHEN 'G' THEN CASE T0."U_HTH_PR"    
       WHEN 'W' THEN 'W-Enviado'    
       WHEN 'C' THEN 'C-Procesado'    
       WHEN 'P' THEN 'P-Procesodo Parcial'    
       WHEN 'E' THEN 'E-Archivo rechazado' END    
          
  ELSE CASE T0."U_HTH_PR"    
       WHEN 'W' THEN 'W-Pendiente'    
       WHEN 'C' THEN 'C-Procesado'    
       WHEN 'P' THEN 'P-Procesodo Parcial'    
       WHEN 'E' THEN 'E-Archivo rechazado' END      
  END   AS "Estado",    
     
 T0."U_HTH_US" AS "Usuario",    
 T0."U_HTH_NAE" AS "Archivo",    
    
 CASE IFNULL(T0."U_HTH_PLG",'')    
  WHEN  '' THEN 'P-Planilla no enviada'    
  WHEN 'G' THEN 'G-Planilla enviada'    
  WHEN 'E' THEN 'E-Error al enviar planilla'     
 END    AS "Generado"    
    
FROM "@EXX_BCP_HOST" T0      
 WHERE     
  T0."U_HTH_CA" = :vProceso     
  AND TO_VARCHAR(T0."U_HTH_FC", 'YYYY-MM-DD') BETWEEN :vFI AND :vFF;      
END;
