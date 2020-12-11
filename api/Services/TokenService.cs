using Microsoft.Extensions.Configuration; //for IConfiguration
using Microsoft.IdentityModel.Tokens; //for SymmetricSecurityKey
using System.Text; // for Encoding
using System.Collections.Generic; // for List
using System.Security.Claims; // for Claim class.
using System.IdentityModel.Tokens.Jwt; // for JwtRegisteredClaimNames

using api.Interfaces;
using api.Entities;

namespace api.Services
{
    public class TokenService:ITokenService
    {
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration config){
            /* initialize the _key.
            SymmetricSecurityKey() constructor requires the byte-array of token key. */
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));


        }
        public string CreateToken(AppUser user){
            /*
            step 1: we need to put the claims inside token hence use the CLAIM class.
            Currently we are just going to add the claims on UserName only.
            UserName would be stored inside JWT token. */
            var claims = new List<Claim>{
                new Claim(JwtRegisteredClaimNames.NameId, user.UserName)
            };
            /* step 2: we need to create the credentials for signing by using SigningCredentials class which takes the security key and algorithm.
             lets select the highest available security algorithm i..e HmacSha512Signature*/
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
            /* step 3: we need to describe the token i..e what contains in the token, expiry of token, signing credential etc.
            We can add more things in the descriptor of the token */
            var tokenDescriptor = new SecurityTokenDescriptor{
                                                                Subject = new ClaimsIdentity(claims),
                                                                Expires = System.DateTime.Now.AddDays(7),
                                                                SigningCredentials = creds
                                                            };
            /* step 4: implement the token handler for creation of actual token.*/
            var tokenHandler = new JwtSecurityTokenHandler();
            /* step 5: create the actual token by using TokenDescriptor */
            var token = tokenHandler.CreateToken(tokenDescriptor);
            /* step 6: issue the token by using WriteToken() method of JwtSecurityTokenHandler class. */
            return tokenHandler.WriteToken(token);
        }
    }
}