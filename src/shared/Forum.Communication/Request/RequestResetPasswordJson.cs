namespace Forum.Communication.Request
{
    public class RequestResetPasswordJson
    {
        public string Code { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
