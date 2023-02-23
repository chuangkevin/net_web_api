CREATE OR ALTER PROCEDURE [GetCustomerBySN]
(
    @CustomerSN BIGINT
)
AS
BEGIN

    SELECT [SN],[NAME],[PHONE_NO],[IS_DELETE],[CREATE_DATE],[UPDATE_DATE]
	FROM [WebApiDatabase].[dbo].[CUSTOMER]
    WHERE [SN] = @CustomerSN

END
