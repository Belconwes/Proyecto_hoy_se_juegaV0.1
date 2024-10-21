﻿using System.ComponentModel.DataAnnotations;

namespace ProyectoHsj_Beta.ViewsModels
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string? Token { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }
    }
}
