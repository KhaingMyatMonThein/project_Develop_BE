namespace PD.Dto
{
    public class PDdto
    {
        public class CreateUserDto
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string Role { get; set; }
        }

        public class UpdateUserDto
        {
            public string Name { get; set; }
            public string Role { get; set; }
        }
    }
}
