using IdentityModel;
using IdentityServer4;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace Yuan.IdentityServer4
{
    public class TestUsers
    {
        public static List<TestUser> Users
        {
            get
            {
                var address = new
                {
                    street_address = "One Hacker Way",
                    locality = "Heidelberg",
                    postal_code = 69118,
                    country = "Germany"
                };

                return new List<TestUser>
                {
                    new TestUser
                    {
                        SubjectId = "1",
                        Username = "alice",
                        Password = "alice",
                        Claims =
                        {
                            new Claim(JwtClaimTypes.Name, "alice Smith"),
                            new Claim(JwtClaimTypes.GivenName, "alice"),
                            new Claim(JwtClaimTypes.FamilyName, "Smith"),
                            new Claim(JwtClaimTypes.Email, "alice@email.com"),
                            new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                            new Claim(JwtClaimTypes.WebSite, "http://baidu.com"),
                            new Claim(JwtClaimTypes.Address, JsonSerializer.Serialize(address), IdentityServerConstants.ClaimValueTypes.Json),

                            new Claim(JwtClaimTypes.Role,"admin")  //添加角色
                        },


                    },
                    new TestUser
                    {
                        SubjectId = "2",
                        Username = "Ayuan",
                        Password = "Ayuan",
                        Claims =
                        {
                            new Claim(JwtClaimTypes.Name, "Ayuan Yang"),
                            new Claim(JwtClaimTypes.GivenName, "Ayuan"),
                            new Claim(JwtClaimTypes.FamilyName, "Yang"),
                            new Claim(JwtClaimTypes.Email, "yyt1517@163.com"),
                            new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                            new Claim(JwtClaimTypes.WebSite, "http://www.cloud.com"),
                            new Claim(JwtClaimTypes.Address, JsonSerializer.Serialize(address), IdentityServerConstants.ClaimValueTypes.Json),

                            new Claim(JwtClaimTypes.Role,"admin")  //添加角色
                        },


                    }
                };
            }
        }
    }
}
