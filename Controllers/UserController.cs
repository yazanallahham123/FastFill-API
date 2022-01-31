using AutoMapper;
using FastFill_API.Data;
using FastFill_API.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastFill_API.Web.Utils;
using FastFill_API.Web.Dto;
using FastFill_API.Web.Utils.Messages;
using System.Security.Claims;
using FastFill_API.Model;

namespace FastFill_API.Controllers
{
    [Route("/api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UserServices _userServices;
        private readonly SecurityServices _securityServices;

        public UserController(IMapper mapper, UserServices userServices, SecurityServices securityServices)
        {
            _mapper = mapper;
            _userServices = userServices;
            _securityServices = securityServices;
        }


        // GET: api/user/users
        [HttpGet("users")]
        [Authorize(Policy = Policies.Admin)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUsers(int page = 1, int pageSize = 10)
        {
            PaginationInfo paginationInfo = new PaginationInfo();
            IEnumerable<User> users = await _userServices.GetUsers(page, pageSize, paginationInfo);
            IEnumerable<User> user = _mapper.Map<IEnumerable<User>>(users);

            var response = new
            {
                Users = user,
                PaginationInfo = paginationInfo
            };

            return Ok(response);
        }

        // GET: api/user/admins
        [HttpGet("admins")]
        [Authorize(Policy = Policies.Admin)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAdmins(int page = 1, int pageSize = 10)
        {
            PaginationInfo paginationInfo = new PaginationInfo();
            IEnumerable<User> users = await _userServices.GetAdmins(page, pageSize, paginationInfo);
            IEnumerable<User> user = _mapper.Map<IEnumerable<User>>(users);

            var response = new
            {
                Users = user,
                PaginationInfo = paginationInfo
            };

            return Ok(response);

        }

        // GET: api/user/supports
        [HttpGet("supports")]
        [Authorize(Policy = Policies.Admin)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSupports(int page = 1, int pageSize = 10)
        {
            PaginationInfo paginationInfo = new PaginationInfo();
            IEnumerable<User> users = await _userServices.GetSupports(page, pageSize, paginationInfo);
            IEnumerable<User> user = _mapper.Map<IEnumerable<User>>(users);

            var response = new
            {
                Users = user,
                PaginationInfo = paginationInfo
            };

            return Ok(response);

        }

        // GET: api/user
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserById(int id)
        {
            if (id == 0)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }
            User user = await _userServices.GetUserById(id);

            if (user == null)
            {
                return NotFound(ResponseMessages.NOT_FOUND);
            }

            return Ok(_mapper.Map<User>(user));
        }

        // GET: api/user/ByPhone
        [HttpGet("ByPhone/{mobileNumber}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserByMobileNumber (string mobileNumber)
        {
            if (mobileNumber == null)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            if (mobileNumber.Trim() == "")
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }
            User user = await _userServices.GetByMobileNumber(mobileNumber);

            if (user == null)
            {
                return NotFound(ResponseMessages.NOT_FOUND);
            }

            return Ok(_mapper.Map<User>(user));
        }

        // POST: api/user
        [HttpPost("User")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostUser([FromBody] UserDto user)
        {
            if (user == null)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(UserErrorMessages.ModelStateParser(ModelState));
            }

            if (_userServices.ExistsMobileNumber(user.MobileNumber))
            {
                ModelState.AddModelError("user", UserErrorMessages.MOBILE_NUMBER_EXISTS);
                return BadRequest(UserErrorMessages.ModelStateParser(ModelState));
            }

            User userEntity = _mapper.Map<User>(user);

            if (!_userServices.AddUser(userEntity, RoleType.User))
            {
                return BadRequest(ResponseMessages.ERROR_UPDATE);
            }

            //Generate JWT token
            IActionResult response = BadRequest(ResponseMessages.ERROR_UPDATE);

            if (userEntity != null)
            {
                var tokenString = _securityServices.GenerateJWTToken(userEntity);
                userEntity.Token = tokenString;
                response = Ok(new
                {
                    token = tokenString,
                    userDetails = _mapper.Map<User>(userEntity),
                });
            }

            return response;
        }

        // POST: api/user
        [HttpPost("Admin")]
        [Authorize(Policy = Policies.Admin)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostAdmin([FromBody] UserDto user)
        {
            if (user == null)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(UserErrorMessages.ModelStateParser(ModelState));
            }

            if (_userServices.ExistsMobileNumber(user.MobileNumber))
            {
                ModelState.AddModelError("user", UserErrorMessages.MOBILE_NUMBER_EXISTS);
                return BadRequest(UserErrorMessages.ModelStateParser(ModelState));
            }

            User userEntity = _mapper.Map<User>(user);

            if (!_userServices.AddUser(userEntity, RoleType.Admin))
            {
                return BadRequest(ResponseMessages.ERROR_UPDATE);
            }

            //Generate JWT token
            IActionResult response = BadRequest(ResponseMessages.ERROR_UPDATE);

            if (userEntity != null)
            {
                var tokenString = _securityServices.GenerateJWTToken(userEntity);

                response = Ok(new
                {
                    token = tokenString,
                    userDetails = _mapper.Map<UserDto>(userEntity),
                });
            }

            return response;
        }

        // POST: api/user
        [HttpPost("Support")]
        [Authorize(Policy = Policies.Admin)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostSupport([FromBody] UserDto user)
        {
            if (user == null)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(UserErrorMessages.ModelStateParser(ModelState));
            }

            if (_userServices.ExistsMobileNumber(user.MobileNumber))
            {
                ModelState.AddModelError("user", UserErrorMessages.MOBILE_NUMBER_EXISTS);
                return BadRequest(UserErrorMessages.ModelStateParser(ModelState));
            }

            User userEntity = _mapper.Map<User>(user);

            if (!_userServices.AddUser(userEntity, RoleType.Support))
            {
                return BadRequest(ResponseMessages.ERROR_UPDATE);
            }

            //Generate JWT token
            IActionResult response = BadRequest(ResponseMessages.ERROR_UPDATE);

            if (userEntity != null)
            {
                var tokenString = _securityServices.GenerateJWTToken(userEntity);

                response = Ok(new
                {
                    token = tokenString,
                    userDetails = _mapper.Map<UserDto>(userEntity),
                });
            }

            return response;
        }

        // PUT: api/user
        [HttpPut]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutUser([FromBody] UserDto user)
        {
            if (user == null)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(UserErrorMessages.ModelStateParser(ModelState));
            }

            //get old user for check if user update whatsApp phone number and to check if user exist

            //get user id from token
            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            User oldUser = await _userServices.GetByMobileNumber(user.MobileNumber);
            User currentUser = await _userServices.GetUserById(currentUserId);

            if (oldUser == null)
            {
                return NotFound(ResponseMessages.NOT_FOUND);
            }

            if ((oldUser.Id != currentUserId) && (currentUser.RoleId != (int)RoleType.Admin))
            {
                ModelState.AddModelError("user", UserErrorMessages.NOT_OWNER);
                return BadRequest(UserErrorMessages.ModelStateParser(ModelState));
            }

            if (!oldUser.MobileNumber.Equals(user.MobileNumber))
            {
                if (_userServices.ExistsMobileNumber(user.MobileNumber))
                {
                    ModelState.AddModelError("user", UserErrorMessages.MOBILE_NUMBER_EXISTS);
                    return BadRequest(UserErrorMessages.ModelStateParser(ModelState));
                }

                oldUser.MobileNumber = user.MobileNumber;
            }

            oldUser.FirstName = user.FirstName;
            oldUser.LastName = user.LastName;
            if (!_userServices.Update(oldUser))
            {
                return BadRequest(ResponseMessages.ERROR_UPDATE);
            }

            return Ok(ResponseMessages.UPDATED_SUCCESSFULLY);
        }

        [HttpPut("Update")]
        [Authorize(Policy = Policies.Admin)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutAdmin([FromBody] UserDto user)
        {
            if (user == null)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(UserErrorMessages.ModelStateParser(ModelState));
            }

            //get old user for check if user update whatsApp phone number and to check if user exist

            //get user id from token
            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            User oldUser = await _userServices.GetByMobileNumber(user.MobileNumber);
            User currentUser = await _userServices.GetUserById(currentUserId);

            if (oldUser == null)
            {
                return NotFound(ResponseMessages.NOT_FOUND);
            }

            if (!oldUser.MobileNumber.Equals(user.MobileNumber))
            {
                if (_userServices.ExistsMobileNumber(user.MobileNumber))
                {
                    ModelState.AddModelError("user", UserErrorMessages.MOBILE_NUMBER_EXISTS);
                    return BadRequest(UserErrorMessages.ModelStateParser(ModelState));
                }

                oldUser.MobileNumber = user.MobileNumber;
            }

            oldUser.FirstName = user.FirstName;
            oldUser.LastName = user.LastName;

            if (!_userServices.Update(oldUser))
            {
                return BadRequest(ResponseMessages.ERROR_UPDATE);
            }

            return Ok(ResponseMessages.UPDATED_SUCCESSFULLY);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = Policies.Admin)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteUser(int id)
        {

            User user = await _userServices.GetUserById(id);
          
            if (user == null)
            {
                return NotFound(ResponseMessages.NOT_FOUND);
            }

            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);            
            User currentUser = await _userServices.GetUserById(currentUserId);


            // Check if the person deleting the account is its owner
            if ((user.Id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value)) && ((currentUser.RoleId != (int)RoleType.Admin)))
            {
                ModelState.AddModelError("user", UserErrorMessages.NOT_OWNER);
                return BadRequest(UserErrorMessages.ModelStateParser(ModelState));
            }

            if (!_userServices.DeleteUser(user))
            {
                return BadRequest(ResponseMessages.ERROR_UPDATE);
            }

            return Ok(ResponseMessages.DELETED_SUCCESSFULLY);
        }

        // Put: api/user/ChangePassword
        [HttpPut("ChangePassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordRequest)
        {
            if (changePasswordRequest == null)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(LoginErrorMessages.ModelStateParser(ModelState));
            }

            User oldUser = await _userServices.GetByMobileNumber(changePasswordRequest.MobileNumber);

            if (oldUser == null)
            {
                return NotFound(ResponseMessages.NOT_FOUND);
            }

            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            User currentUser = await _userServices.GetUserById(currentUserId);

            if ((oldUser.Id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value)) && ((currentUser.RoleId != (int)RoleType.Admin)))
            {
                ModelState.AddModelError("user", UserErrorMessages.NOT_OWNER);
                return BadRequest(UserErrorMessages.ModelStateParser(ModelState));
            }

            if (!_securityServices.VerifyPassword(changePasswordRequest.OldPassword, oldUser.StoredSalt, oldUser.Password))
            {
                ModelState.AddModelError("Password", LoginErrorMessages.INCORRECT_OLD_PASSWORD);
                return BadRequest(LoginErrorMessages.ModelStateParser(ModelState));
            }

            if (!_userServices.ChangePassword(oldUser, changePasswordRequest.NewPassword))
            {
                return BadRequest(ResponseMessages.ERROR_UPDATE);
            }

            return Ok(ResponseMessages.UPDATED_SUCCESSFULLY);
        }

        // Put: api/user/ResetPassword
        [HttpPut("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordBody resetPasswordBody)
        {
            if (resetPasswordBody == null)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(LoginErrorMessages.ModelStateParser(ModelState));
            }

            User oldUser = await _userServices.GetByMobileNumber(resetPasswordBody.MobileNumber);

            if (oldUser == null)
            {
                return NotFound(ResponseMessages.NOT_FOUND);
            }

            //TODO
            //should check OTP VerificationId

            if (!_userServices.ChangePassword(oldUser, resetPasswordBody.NewPassword))
            {
                return BadRequest(ResponseMessages.ERROR_UPDATE);
            }

            return Ok(ResponseMessages.UPDATED_SUCCESSFULLY);
        }

    }
}
