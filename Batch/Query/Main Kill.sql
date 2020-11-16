USE master;  
GO  
DECLARE @db_id INT;  
SET @db_id = DB_ID('CarStore');  
IF @db_id IS NOT NULL 
	BEGIN
	ALTER DATABASE CarStore SET SINGLE_USER WITH ROLLBACK IMMEDIATE
	ALTER DATABASE CarStore MODIFY NAME = CarStoreKilled ;
	ALTER DATABASE CarStoreKilled SET MULTI_USER
	print 'Server is killed'
	END