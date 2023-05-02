using OPBids.Entities.Base;

namespace OPBids.Entities.View.Auth
{
    public class AuthPayload : BaseVM
    {
        public string auth_x_un { get; set; }
        public string auth_x_pwd { get; set; }
        public string auth_x_code { get; set; }
    }
}
