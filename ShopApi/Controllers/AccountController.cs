using EmailService;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using ShopApi.Model.Identity;
//-----------------
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace ShopAPI.Controllers
{

    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<UserIdentityX01> _userManager;
        private readonly SignInManager<UserIdentityX01> _loginManager;

        private readonly IEmailSender _emailSender;
        private readonly IConfiguration Configuration;


        public AccountController(
            IConfiguration configuration,
            UserManager<UserIdentityX01> userManager,
            SignInManager<UserIdentityX01> signInManager,

            IEmailSender emailSender
                       )
        {
            Configuration = configuration;
            _userManager = userManager;
            _loginManager = signInManager;

            _emailSender = emailSender;

        }


        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginInputModelDto login) //[FromBody] LoginInputModel login
        {

            // Console.WriteLine("PassLogInAsync ----", login.Email);

            if (login == null)
            {
                return BadRequest(" Неверный запрос клиента");
            }
            if (string.IsNullOrEmpty(login.Email) && string.IsNullOrEmpty(login.Phone))
            {
                return BadRequest(" Неверный Email клиента");
            }
            UserIdentityX01? user;
            if (!string.IsNullOrEmpty(login.Email))
            {
                user = await _userManager.FindByEmailAsync(login.Email);

            }
            else
            {
                user = _userManager.Users.FirstOrDefault(u => u.PhoneNumber == login.Phone);


            }
            // var user = await _userManager.FindByEmailAsync(model.UserName); //??? 

            if (user == null)
            {
                ModelState.AddModelError("username", "пользователь не найден!");
                return Unauthorized("пользователь не найден!");
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError("email", "Электронная почта не подтверждена!");
                return Unauthorized("Email не подтвержден");
            }

            if (string.IsNullOrEmpty(login.Password))
            {
                return BadRequest(" Неверный Password клиента");

            }

            var result = await _loginManager.PasswordSignInAsync(user, login.Password, login.RememberMe, lockoutOnFailure: true);
            if (result.Succeeded)
            {

                var accessToken = GenerateTokenAsync(user).Result;
                var refreshToken = GenerateRefreshToken();
               await SetRefreshTokenAsync(refreshToken, user);


                return Ok(new TokenModelDto { Access_token = accessToken });
            }

            // return Unauthorized();
            return Unauthorized("неверный пароль "); //неверный пароль


        }

        [HttpPost("TelegramExternalLogin")]
        [AllowAnonymous]
        public async Task<IActionResult> TelegramExternalLogin([FromBody] UserTelegramDto user)
        {


            //https://xl-01.ru/telelram-callback?id=5384361370&first_name=Igor&username=igor_01ts&auth_date=1663303742&hash=bd734108e0d695bd89a6eaf56e50b3cdc2a5adc8dc4e98a8c777bc66545bebbf
            //https://gist.github.com/anonymous/6516521b1fb3b464534fbc30ea3573c2
            // https://codex.so/telegram-auth?ysclid=l89qryxyrb334407760




            StringBuilder dataStringBuilder = new StringBuilder(256);

            dataStringBuilder.Append("auth_date");
            dataStringBuilder.Append('=');
            dataStringBuilder.Append(user.AuthDate);
            dataStringBuilder.Append('\n');

            dataStringBuilder.Append("first_name");
            dataStringBuilder.Append('=');
            dataStringBuilder.Append(user.FirstName);
            dataStringBuilder.Append('\n');

            dataStringBuilder.Append("id");
            dataStringBuilder.Append('=');
            dataStringBuilder.Append(user.Id);
            dataStringBuilder.Append('\n');





            dataStringBuilder.Append("username");
            dataStringBuilder.Append('=');
            dataStringBuilder.Append(user.UserName);
            //     dataStringBuilder.Append('\n');


            byte[]? secretKey = null;
            var token_bot = Configuration.GetSection("IdentityX01:Telegram-Tokens-Key").GetChildren().ToList();

            foreach (var token in token_bot)
            {
                if (token != null && token.Key == user.SpaId)
                {
                    secretKey = ShaHash(token.Value!);
                }
            }
            byte[]? myHash = null;
            if (secretKey != null)
                myHash = HashHmac(secretKey, Encoding.UTF8.GetBytes(dataStringBuilder.ToString()));

            //php   hash   https://php.ru/manual/function.hash.html?ysclid=l87m3tbcl2736101629
            //php   hash_hmac   https://www.php.net/manual/ru/function.hash-hmac.php
            //php    time()  https://www.php.net/manual/ru/function.time.php

            var teltime = int.Parse(user.AuthDate);
            var time = DateTimeOffset.Now.ToUnixTimeSeconds();


            if ((time - teltime) > 86400)// 24часа
            {
                return BadRequest("Data is outdated");
            }
            if (myHash == null) return BadRequest("Hash token =null");
            var myHashStr = String.Concat(myHash.Select(i => i.ToString("x2")));
            var providerKey = user.Id;
            if (myHashStr == user.Hash)
            {
                //AspNetUserLogins   table in identity BD
                //ProviderKey — это уникальный ключ Facebook, связанный с пользователем на Facebook.


                var info = new UserLoginInfo("Telegram", providerKey, "Telegram");

                var user_tel = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);


                if (user_tel == null)
                {
                    user_tel = new UserIdentityX01 { FirstName = user.FirstName, UserName = user.UserName, NormalizedUserName = user.Id };
                    await _userManager.CreateAsync(user_tel);
                    //prepare and send an email for the email confirmation
                    await _userManager.AddToRoleAsync(user_tel, Enum.GetName(Role.Shopper) ?? "Shopper");
                    await _userManager.AddLoginAsync(user_tel, info);
                }
                else
                {
                    await _userManager.AddLoginAsync(user_tel, info);
                }
                if (user_tel == null)
                    return BadRequest("Invalid External Authentication.");

                //----------------------------------------------
                var accessToken = GenerateTokenAsync(user_tel).Result;
                var refreshToken = GenerateRefreshToken();
                SetRefreshTokenAsync(refreshToken, user_tel);


                return Ok(new TokenModelDto { Access_token = accessToken });
            }


            return BadRequest("hash error");


        }

        private byte[] ShaHash(String value)
        {
            using (var hasher = SHA256.Create())

            { return hasher.ComputeHash(Encoding.UTF8.GetBytes(value)); }
        }

        private byte[] HashHmac(byte[] key, byte[] message)
        {
            var hash = new HMACSHA256(key);
            return hash.ComputeHash(message);
        }



        [HttpPost("GoogleExternalLogin")]
        [AllowAnonymous]
        public async Task<IActionResult> GoogleExternalLogin([FromBody] ExternalGoogleDto externalAuth)
        {
            var payload = await VerifyGoogleToken(externalAuth);
            if (payload == null)
                return BadRequest("Invalid External Authentication.");
            if (String.IsNullOrEmpty(externalAuth.Provider))
                return BadRequest("Invalid External Provider Name.");

            var info = new UserLoginInfo(externalAuth.Provider, payload.Subject, externalAuth.Provider);

            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(payload.Email);
                if (user == null)
                {
                    user = new UserIdentityX01
                    {
                        Email = payload.Email,
                        UserName = payload.Email,
                        FirstName = payload.FamilyName,
                        LastName = payload.Name,
                        Address = ""

                    };
                    await _userManager.CreateAsync(user);

                    //prepare and send an email for the email confirmation

                    await _userManager.AddToRoleAsync(user, Enum.GetName(Role.Shopper) ?? "Shopper");
                    await _userManager.AddLoginAsync(user, info);
                }
                else
                {
                    await _userManager.AddLoginAsync(user, info);
                }
            }

            if (user == null)
                return BadRequest("Invalid External Authentication.");

            //check for the Locked out account

            var accessToken = GenerateTokenAsync(user).Result;
            var refreshToken = GenerateRefreshToken();
           await SetRefreshTokenAsync(refreshToken, user);


            return Ok(new TokenModelDto { Access_token = accessToken });
        }





        [HttpPost("VKExternalLogin")]
        [AllowAnonymous]
        public async Task<IActionResult> VKExternalLogin()
        {
            //   VKExternalLogin([FromBody] ExternalAuthDto externalAuth) -- not work !!!! 16.03.23
            // поле photo_rec_url -нереалиизовано

            string body = String.Empty;
            using (StreamReader stream = new StreamReader(Request.Body))
            {
                body = await stream.ReadToEndAsync();
            }

            var externalAuth = JsonConvert.DeserializeObject<ExternalVkDto>(body);

            if (externalAuth == null) return BadRequest("Invalid External Authentication.");

            var payload = VerifyVKToken(externalAuth);
            if (payload == null)
                return BadRequest("Invalid External Authentication.");

            var info = new UserLoginInfo(externalAuth.Provider, payload.UserId.ToString(), externalAuth.Provider);

            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            if (user == null)
            {
                //  user = await _userManager.FindByEmailAsync(payload.Email);

                user = new UserIdentityX01 { UserName = payload.UserId.ToString(), NormalizedUserName = payload.UserId.ToString(), FirstName = payload.FirstName, LastName = payload.LastName };
                await _userManager.CreateAsync(user);

                //prepare and send an email for the email confirmation

                await _userManager.AddToRoleAsync(user, Enum.GetName(Role.Shopper) ?? "Shopper");
                await _userManager.AddLoginAsync(user, info);


            }
            else
            {
                await _userManager.AddLoginAsync(user, info);
            }

            if (user == null)
                return BadRequest("Invalid External Authentication.");

            //check for the Locked out account

            var accessToken = GenerateTokenAsync(user).Result;
            var refreshToken = GenerateRefreshToken();
            await SetRefreshTokenAsync(refreshToken, user);


            return Ok(new TokenModelDto { Access_token = accessToken });
        }



        //[Route("Register")]
        [HttpPost("Registration")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
        {
            if (userForRegistration == null || !ModelState.IsValid)
                return BadRequest("данные не валидны");

            var user = new UserIdentityX01
            {
                Address = userForRegistration.Address,
                Email = userForRegistration.Email,
                SpaId = userForRegistration.SpaId,
                FirstName = userForRegistration.FirstName,
                PhoneNumber = userForRegistration.Phone,
                LastName = userForRegistration.LastName
            };
            if (String.IsNullOrEmpty(userForRegistration.Password))
                return BadRequest("не задан Password");

            var result = await _userManager.CreateAsync(user, userForRegistration.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);

                return BadRequest(new RegistrationResponseDto { Errors = errors });
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var param = new Dictionary<string, string>
            {
                {"token", token },
                {"email", user.Email ??""}
            };

            var callback = QueryHelpers.AddQueryString(userForRegistration.ClientURI, param);

            try
            {

                var message = new Message(new string[] { user.Email }, "Токен подтверждения электронной почты,Возможно Почта недействительна", callback, null);
                await _emailSender.SendEmailAsync(message);
            }
            catch
            {

                await _userManager.DeleteAsync(user);

                return BadRequest("Токен подтверждения электронной почты неотправлен");
            }

            await _userManager.AddToRoleAsync(user, Enum.GetName(Role.Shopper) ?? "Shopper");  //'shopper'

            return StatusCode(201);
        }

        [HttpPost("ForgotPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);
            if (user == null)
                return BadRequest("Invalid Request");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var param = new Dictionary<string, string>
            {
                {"token", token },
                {"email", forgotPasswordDto.Email }
            };

            var callback = QueryHelpers.AddQueryString(forgotPasswordDto.ClientURI, param);

            var message = new Message(new string[] { forgotPasswordDto.Email }, "сбросить  пароль ", callback, null);
            await _emailSender.SendEmailAsync(message);

            return Ok();
        }


        [HttpPost("ResetPasswordMail")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPasswordMail([FromBody] ResetPasswordMailDto resetPasswordDto)
        {
            if (resetPasswordDto==null || !ModelState.IsValid)
                return BadRequest();
                if(string.IsNullOrEmpty(resetPasswordDto.Email) || 
                 string.IsNullOrEmpty(resetPasswordDto.Token)||
                 string.IsNullOrEmpty(resetPasswordDto.Password)                 
                 ) return BadRequest();

            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user == null)
                return BadRequest("Invalid Request");

            var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.Password);
            if (!resetPassResult.Succeeded)
            {
                var errors = resetPassResult.Errors.Select(e => e.Description);

                return BadRequest(new { Errors = errors });
            }

            return Ok();
        }



        [HttpGet("EmailConfirmation")]
        [AllowAnonymous]
        public async Task<IActionResult> EmailConfirmation([FromQuery] string email, [FromQuery] string token)
        {
            //https://github.com/CodeMazeBlog/angular-identity-aspnetcore-security/tree/angular-google-authentication-identity
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest("Invalid Email Confirmation Request");

            var confirmResult = await _userManager.ConfirmEmailAsync(user, token);
            if (!confirmResult.Succeeded)
                return BadRequest("Invalid Email Confirmation Request");

            return Ok();
        }


        [HttpGet("x01-token")]
        // [AllowAnonymous]   //  is test ok()
        public string X01Token()
        {

            return "This action is OK";

        }


        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> RefreshToken(TokenModelDto tokenApiModel)
        {
            var refreshToken = Request.Cookies["refreshToken"];

            var user = await getUserIdentity(tokenApiModel);
            if (user == null)
                return Unauthorized("Invalid Access Token.");

            if (!user.RefreshToken.Equals(refreshToken))
            {
                return Unauthorized("Invalid Refresh Token.");
            }
            else if (user.TokenExpires < DateTime.Now)
            {
                return Unauthorized("Token expired.");
            }

            string token = await GenerateTokenAsync(user);
            var newRefreshToken = GenerateRefreshToken();
             await     SetRefreshTokenAsync(newRefreshToken, user);

            return Ok(token);
        }

        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7),
                Created = DateTime.Now
            };

            return refreshToken;
        }

        private async Task SetRefreshTokenAsync(RefreshToken newRefreshToken, UserIdentityX01 user)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };
            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

            user.RefreshToken = newRefreshToken.Token;
            user.TokenCreated = newRefreshToken.Created;
            user.TokenExpires = newRefreshToken.Expires;

            await _userManager.UpdateAsync(user);
        }


        //////////////////////------------------Создаем Токен-----------------------------------
        private async Task<string> GenerateTokenAsync(UserIdentityX01 user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);


            var claims = new List<Claim> {

                new Claim(JwtRegisteredClaimNames.NameId, user.Id) ,


                 };
            foreach (var r in userRoles)
            {
                claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, r));

            }

            //var mySecret = Environment.GetEnvironmentVariable("ClientSecrets"); // ключ для шифрации
            var mySecurityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(Configuration.GetSection("IdentityX01:TokenX01-Key").Value!)
                );

            int LIFETIME = 240;//1; //60; // время жизни токена 
            string[]? audence = Configuration.GetSection("Authentication:Schemes:JwtBearer:Audiences").Get<string[]>();
            string joinedString = String.Empty;
            if (audence != null)
            {
               joinedString = audence.Aggregate((prev, current) => prev + "," + current);
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var time = DateTime.UtcNow;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims), //Claims

                Expires = time.Add(TimeSpan.FromMinutes(LIFETIME)),
                Issuer = Configuration.GetSection("Authentication:Schemes:JwtBearer:Issuer").Value,// издатель токена
                Audience = joinedString,// потребитель токена
               SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private async Task<GoogleJsonWebSignature.Payload?> VerifyGoogleToken(ExternalGoogleDto externalAuth)
        {

            //https://console.cloud.google.com/apis/credentials?project=x-01-mystore
            //https://github.com/CodeMazeBlog/angular-identity-aspnetcore-security/blob/angular-google-authentication-identity/Web%20API/CompanyEmployees/CompanyEmployees/Program.cs
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string>() { Configuration.GetSection("IdentityX01:Google-Token-Key").Value! }
                };
                var payload = await GoogleJsonWebSignature.ValidateAsync(externalAuth.IdToken, settings);

                //  Console.WriteLine("GoogleJsonWebSignature.ValidateAsync--"+payload);
                return payload;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //log an exception
                return null;
            }
        }

        /* ---help ---
        https://vk.com/dev/widget_auth (!!!)
        hash vk https://habr.com/ru/sandbox/26984/
        https://dev.vk.com/api/open-api/getting-started(!!!)
        https://kotoff.net/article/39-avtorizacija-na-sajte-s-pomoschju-vk-prostoj-i-ponjatnyj-sposob-na-php.html
        https://babakov.net//blog/netcore/325.html
        */
        private VkProfileDto? VerifyVKToken(ExternalVkDto vkDto)
        {
            VkProfileDto profile = new VkProfileDto();



            //Защищённый ключ--Environment.GetEnvironmentVariable("VK_Token")
            string s = vkDto.IdApp + vkDto.IdUser + Configuration.GetSection("IdentityX01:VK-Token-Key").Value;
            string hash = MD5HashGet(s);
            Console.WriteLine("--VK-- MD5HashGet--");
            Console.WriteLine(hash);
            Console.WriteLine(vkDto.Hash);

            if (String.Equals(vkDto.Hash, hash))
            {
                profile.UserId = vkDto.IdUser;
                profile.FirstName = vkDto.First_name;
                profile.LastName = vkDto.Last_name;

                return profile;

            }



            return null;

        }

        private static string MD5HashGet(string forHash)
        {
            byte[] hash = Encoding.ASCII.GetBytes(forHash);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] hashenc = md5.ComputeHash(hash);
            string result = "";
            foreach (var b in hashenc)
            {
                result += b.ToString("x2");
            }
            return result;
        }

        private async Task<UserIdentityX01?> getUserIdentity(TokenModelDto token)
        {
            if (String.IsNullOrEmpty(token.Access_token))
                return null;

            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token.Access_token);
            string userId = jwt.Claims.First(c => c.Type == JwtRegisteredClaimNames.NameId).Value;

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {

                return null;
            }
            return user;

        }

        /// <summary>
        /// Отправляем запрос на получение 
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        private string GetRequest(string host, string req)
        {
            string str = "";

            var Vk = new HttpClient();
            Vk.DefaultRequestHeaders.Add("Connection", "close");

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(req);
            request.UseDefaultCredentials = true;
            request.PreAuthenticate = true;
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Method = "GET";
            request.Host = host;
            // request.UserAgent = "RM";
            request.ContentType = "application/x-www-form-urlencoded";
            request.KeepAlive = false;

            using (HttpWebResponse responsevk = (HttpWebResponse)request.GetResponse())
            using (var stream = responsevk.GetResponseStream())
            using (var streamReader = new StreamReader(stream, Encoding.UTF8))
            {
                str = streamReader.ReadToEnd();
            }
            return str;
        }

    }
}

