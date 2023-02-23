CREATE OR ALTER PROCEDURE [DeleteCustomer_SetState]
(
	@CustomerSN BIGINT
)
AS
BEGIN
    UPDATE [WebApiDatabase].[dbo].[CUSTOMER]
   SET [IS_DELETE]=1
    WHERE [SN]=@CustomerSN
END

