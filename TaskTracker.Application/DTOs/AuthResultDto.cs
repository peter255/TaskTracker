﻿namespace TaskTracker.Application.DTOs
{
    public class AuthResultDto
    {
        public string Token { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

}
