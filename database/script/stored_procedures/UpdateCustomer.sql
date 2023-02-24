CREATE OR ALTER PROCEDURE [UpdateCustomer]
(
	@CustomerSN BIGINT,
    @Name NVARCHAR(64),
    @PhoneNumber NVARCHAR(64)
)
AS
BEGIN
    UPDATE [WebApiDatabase].[dbo].[CUSTOMER]
	SET [NAME] = @Name
      ,[PHONE_NO] = @PhoneNumber
	  ,[UPDATE_DATE]=GETDATE()
	OUTPUT @CustomerSN
    WHERE [SN]=@CustomerSN
END

