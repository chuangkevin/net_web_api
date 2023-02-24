CREATE OR ALTER PROCEDURE [CreateCustomer]
(
    @Name NVARCHAR(64),
    @PhoneNumber NVARCHAR(64)
)
AS
BEGIN
        INSERT INTO [WebApiDatabase].[dbo].[CUSTOMER]
        (
            NAME,
            PHONE_NO
        )
		OUTPUT Inserted.SN
        VALUES
        (
            @Name,
            @PhoneNumber
        )
END
