namespace WEBAPI_m1IL_1.Model
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string EmailAdress { get; set; }
        public string Role { get; set; }
        public string Surname { get; set; }
        public string GivenName { get; set; }
    }

    public class UserConstants
    {
        public static List<UserModel> Users =
        [
            new()
            {
                Id = 1,
                Username = "admin",
                Password = "admin123",
                EmailAdress = "admin@example.com",
                Role = "Administrator",
                Surname = "Admin",
                GivenName = "Super"
            }
        ];
    }
}