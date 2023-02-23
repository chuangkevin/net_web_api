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
    WHERE [SN]=@CustomerSN
END

