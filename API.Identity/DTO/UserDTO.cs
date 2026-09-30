namespace API.Identity.DTO
{
    public record CreateUserRequest(
        string FirstName,
        string LastName,
        string EmailAddress,
        string ContactNumber,
        string Address,
        string UserName,
        string Password,
        string Role
        );

    public record UpdateUserRequest(
        string FirstName,
        string LastName,
        string EmailAddress,
        string ContactNumber,
        string Address
        );

    public record UpdateUserPasswordRequest ( string Password );
}
