sqlcmd -E -S (local)\sqlexpress -i Recreate_DB-UO_MES.sql
"C:\Program Files\Telerik\OpenAccess ORM\bin\VSchema.exe" -assembly:"..\UO_MES\UO_Model\bin\Debug\UO_Model.dll" -config:"..\UO_MES\UO_Model\App.config" -create -connectionId:MES_SQLExpress_DBConnection -direct+
pause