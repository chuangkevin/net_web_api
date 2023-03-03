using JWT.Algorithms;
using JWT.Builder;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.Security.Claims;

namespace WebAPI.Helper
{
    public class JwtHelper
    {
        private readonly JwtSettingsOptions _settings;

        public JwtHelper(IOptionsMonitor<JwtSettingsOptions> settings)
        {
            //注入appsetting的json
            _settings = settings.CurrentValue;
        }

        public string GenerateToken(string userName, int expireMinutes = 120)
        {
            //發行人
            var issuer = _settings.Issuer;
            //加密的key，拿來比對jwt-token沒有
            var signKey = _settings.SignKey;
            //建立JWT - Token
            var token = JwtBuilder.Create()
                        //所採用的雜湊演算法
                        .WithAlgorithm(new HMACSHA256Algorithm()) // symmetric
                                                                  //加密key
                        .WithSecret(signKey)
                        //角色
                        .AddClaim("roles", "admin")
                        //JWT ID
                        .AddClaim("jti", Guid.NewGuid().ToString())
                        //發行人
                        .AddClaim("iss", issuer)
                        //使用對象名稱
                        .AddClaim("sub", userName) // User.Identity.Name
                                                   //過期時間
                        .AddClaim("exp", DateTimeOffset.UtcNow.AddMinutes(expireMinutes).ToUnixTimeSeconds())
                        //此時間以前是不可以使用
                        .AddClaim("nbf", DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                        //發行時間
                        .AddClaim("iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                        //使用者全名
                        .AddClaim(ClaimTypes.Name, userName)
                        //進行編碼
                        .Encode();
            return token;
        }
    }
    //將appsetting轉為強行別所使用
    public class JwtSettingsOptions
    {
        public string Issuer { get; set; } = "";
        public string SignKey { get; set; } = "";
    }
}
