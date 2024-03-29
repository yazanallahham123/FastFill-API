﻿using AutoMapper;
using FastFill_API.Web.Dto;
using FastFill_API.Web.Services;
using FastFill_API.Web.Utils.Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API.Controllers
{
    [Route("/api/[controller]")]
    public class LoginController : Controller
    {
        private readonly IMapper _mapper;
        private readonly SecurityServices _securityServices;
        private readonly UserServices _userServices;

        public LoginController(IMapper mapper, SecurityServices securityServices, UserServices userServices)
        {
            _mapper = mapper;
            _securityServices = securityServices;
            _userServices = userServices;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            bool isEmpty = true;

            if (login.mobileNumber != null)
            {
                if (login.mobileNumber.Trim() != "")
                {
                    if (login.password != null)
                    {
                        if (login.password.Trim() != "")
                        {
                            isEmpty = false;
                        }
                    }
                }
            }

            if (isEmpty)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(LoginErrorMessages.ModelStateParser(ModelState));
            }

            User user = await _userServices.GetByMobileNumber(login.mobileNumber, 0);
            
            if (user == null)
            {
                return NotFound(ResponseMessages.NOT_FOUND);
            }

            user.Language = login.language;

            await _userServices.UpdateUserLanguage(user.Id, login.language);

            if (!_securityServices.VerifyPassword(login.password, user.StoredSalt, user.Password))
            {
                ModelState.AddModelError("Password", LoginErrorMessages.INCORRECT_PASSWORD);
                return BadRequest(LoginErrorMessages.ModelStateParser(ModelState));
            }

            var jwtToken = _securityServices.GenerateJWTToken(user);

            user.Token = jwtToken;
            IActionResult response = Ok(new
            {
                token = jwtToken,                
                userDetails = _mapper.Map<User>(user),
            });

            return Ok(response);
        }

        // POST: api/user
        [HttpGet("ShowSignupInStationApp")]
        public async Task<IActionResult> ShowSignupInStationApp()
        {
            TempSetting res = await _userServices.ShowSignupInStationApp();

            return Ok(res.ShowSignupInStationApp);
        }

        // GET: api/user/ByPhone
        [HttpGet("ByPhone/{mobileNumber}")]
        public async Task<IActionResult> GetUserByMobileNumber(string mobileNumber)
        {
            if (mobileNumber == null)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            if (mobileNumber.Trim() == "")
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }
            User user = await _userServices.GetByMobileNumber(mobileNumber, 0);

            if (user == null)
            {
                return NotFound(ResponseMessages.NOT_FOUND);
            }

            return Ok(_mapper.Map<User>(user));
        }



    }
}
