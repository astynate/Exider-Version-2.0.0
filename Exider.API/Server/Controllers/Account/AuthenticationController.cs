﻿using Exider.Core;
using Exider.Core.Dependencies.Repositories.Account;
using Exider.Core.Models.Account;
using Exider.Dependencies.Services;
using Exider.Repositories.Account;
using Exider.Repositories.Repositories;
using Exider_Version_2._0._0.ServerApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace Exider_Version_2._0._0.Server.Controllers.Account
{

    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {

        private readonly ITokenService _tokenService;

        private readonly IUsersRepository _usersRepository;

        private readonly IEmailRepository _emailRepository;

        private readonly IEmailService _emailService;

        public AuthenticationController
        (
            ITokenService tokenService,
            IUsersRepository usersRepository,
            IEmailRepository emailRepository,
            IEmailService emailService
        )
        {
            _tokenService = tokenService;
            _usersRepository = usersRepository;
            _emailRepository = emailRepository;
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> Login(IEncryptionService encryptionService, ISessionsRepository sessionsRepository)
        {

            string? username = Request.Form["username"];
            string? password = Request.Form["password"];

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return BadRequest("Ivalid form data");

            UserModel? user = await _usersRepository.GetUserByEmailOrNicknameAsync(username);

            if (user is null)
                return BadRequest("User not found");

            var getEmailResult = await _emailRepository.GetByEmailAsync(user.Email);

            if (getEmailResult.IsFailure)
                return Conflict(getEmailResult.Error);

            if (getEmailResult.Value.IsConfirmed == false)
                return StatusCode(470, "Email not verified");

            if (user.Password != encryptionService.HashUsingSHA256(password))
                return StatusCode(StatusCodes.Status401Unauthorized);

            string accessToken = _tokenService
                .GenerateAccessToken(user.Id.ToString(), 30, Configuration.testEncryptionKey);

            string refreshToken = _tokenService
                .GenerateRefreshToken(user.Id.ToString());

            var sessionCreationResult = SessionModel.Create
            (
                "",
                "",
                refreshToken,
                user.Id
            );

            if (sessionCreationResult.IsFailure)
                return BadRequest(sessionCreationResult.Error);

            await sessionsRepository.AddSessionAsync(sessionCreationResult.Value);
            await _emailService.SendLoginNotificationEmail(user.Email, sessionCreationResult.Value);

            Response.Cookies.Append("system_refresh_token", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
            });

            return Ok(accessToken);

        }

    }
}
