using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Training.Api.Model
{
    public class LoginBindingModel : IValidatableObject
    {
        /// <summary>
        /// Authentication grant types. Supports
        /// <para>
        /// password, client_credentials and refresh_token
        /// </para>
        /// </summary>
        [JsonPropertyName("grant_type")]
        [Required(ErrorMessage = "Grant type is required")]
        public string GrantType { get; set; }

        [JsonPropertyName("user_name")]
        public string UserName { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();
            if (GrantType.ToLower() == "password" || GrantType.ToLower() == "client_credentials")
            {
                if (string.IsNullOrEmpty(UserName))
                {
                    errors.Add(new ValidationResult("User name is required", new[] { "user_name" }));
                }

                if (string.IsNullOrEmpty(Password))
                {
                    errors.Add(new ValidationResult("Password is required", new[] { "Password" }));
                }
            }
            else if (GrantType.ToLower() == "refresh_token")
            {
                if (string.IsNullOrEmpty(RefreshToken))
                {
                    errors.Add(new ValidationResult("Refresh token is required for the grant type 'refresh_token'", new[] { "RefreshToken" }));
                }
            }
            else
            {
                errors.Add(new ValidationResult("Invalid grant type, allowed only 'password' and 'refresh_token'", new[] { "GrantType" }));
            }

            return errors;
        }
    }
}
