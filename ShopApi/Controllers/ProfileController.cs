
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Model.Identity;

using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShopAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly UserManager<UserIdentityX01> _userManager;
        private readonly SignInManager<UserIdentityX01> _loginManager;


        public ProfileController(
            UserManager<UserIdentityX01> userManager,
            SignInManager<UserIdentityX01> signInManager

                       // IEmailSender emailSender
                       )
        {
            _userManager = userManager;
            _loginManager = signInManager;

            // _emailSender = emailSender;

        }



        [HttpGet("GetUser")]
         [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetUser()
        {

            Console.WriteLine("test Profile" +HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Name)
                .Select(x => x.Value)
                .FirstOrDefault());



            var user = await _userManager.GetUserAsync(HttpContext.User);

            Console.WriteLine("test Profile" + user);

            // Console.WriteLine(""+this.HttpContext.ToString());
            // id  должен быть первым в cla
            //FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            if (user != null)
            {
                //   var idUser = Guid.Parse(idUserClaim.Value);
                // var user = await this._userManager.FindByIdAsync(userId.Claims[]);                                     //_repository.GetUserId(idUser);
                UserProfileDto userSerialize = new UserProfileDto();
                userSerialize.FirstName = user.FirstName;
                userSerialize.LastName = user.LastName;
                userSerialize.Address = user.Address;
                userSerialize.Email = user.Email;
                userSerialize.Phone = user.Phone;


                return Ok(userSerialize);
            }
            else
            {
                ModelState.AddModelError("User", "Данный Пользоватль Несуществует - обратитесь к Администратору ресурса");
                Console.WriteLine("User Profile get httpGet BadRequst");
                return BadRequest(ModelState);
            }

        }

        //---------------------------------------------------------------------------------



        [HttpPost("EditUser")]
         [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UpdateUser(UserProfileDto userSerialize)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user != null)
            {
                user.LastName = userSerialize.LastName;
                user.FirstName = userSerialize.FirstName;
                user.PhoneNumber = userSerialize.Phone;
                user.Email = userSerialize.Email;
                user.Address = userSerialize.Address;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    ModelState.AddModelError("", "Что-то пошло не так");
                    return BadRequest(ModelState);
                }


            }
            else
            {
                ModelState.AddModelError("User", "Данный Пользоватль Несуществует - обратитесь к Администратору ресурса");
                Console.WriteLine("User Profile get httpGet BadRequst");
                return BadRequest(ModelState);
            }



        }

        [HttpPost("ResetPasswordProfile")]
         [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> ResetPasswordProfile([FromBody] ResetPasswordProfileDto resetPasswordDto)
        {
            //Console.Write(resetPasswordDto);
            if (!ModelState.IsValid)
                return BadRequest("Model is not valid");

            if (String.IsNullOrEmpty(resetPasswordDto.Email) && String.IsNullOrEmpty(resetPasswordDto.Phone))
                return BadRequest("Email or Phone is null or empty");//7.10.23

            UserIdentityX01? user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null)
                return BadRequest("Invalid Request");

            var resetPassResult = await _userManager.ChangePasswordAsync(user, resetPasswordDto.OldPassword!, resetPasswordDto.NewPassword!);
            if (!resetPassResult.Succeeded)
            {
              //  var errors = resetPassResult.Errors.Select(e => e.Description);

              var err=String.Empty;

                foreach (var error in  resetPassResult.Errors)
                {
                   // ModelState.AddModelError(string.Empty, error.Description);
                   err+=error.Description;
                }

                return BadRequest(err);
            }

            return Ok();
        }

        //----------------------------------

        [HttpDelete("Delete/{id}")]
         [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeleteUser(string id)
        {

            var user = await _userManager.GetUserAsync(HttpContext.User);


            if (user == null)
                return BadRequest("User Not Found");


            if (!user.Email!.Equals(id) && !user.PhoneNumber!.Equals(id))
                return BadRequest("this mail or phone not for user account");
            var resetPassResult = await _userManager.DeleteAsync(user);

            if (!resetPassResult.Succeeded)
            {
                var errors = resetPassResult.Errors.Select(e => e.Description);

                return BadRequest(new { Errors = errors });
            }

            return Ok();




        }
    }
}
