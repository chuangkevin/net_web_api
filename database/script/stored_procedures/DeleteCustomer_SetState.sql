CREATE OR ALTER PROCEDURE [DeleteCustomer_SetState]
(
	@CustomerSN BIGINT
)
AS
BEGIN
    UPDATE [WebApiDatabase].[dbo].[CUSTOMER]
   SET [IS_DELETE]=1,[UPDATE_DATE]=GETDATE()
    WHERE [SN]=@CustomerSN
END

