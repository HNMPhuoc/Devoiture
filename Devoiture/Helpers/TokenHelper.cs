using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

public static class TokenHelper
{
    // Sử dụng hàm để tạo chuỗi khóa bí mật
    private static readonly string SecretKey = GenerateRandomString(32); // Đảm bảo độ dài đủ để đảm bảo tính bảo mật

    // Hàm để tạo chuỗi ngẫu nhiên có độ dài length
    private static string GenerateRandomString(int length)
    {
        const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var randomBytes = new byte[length];

        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(randomBytes);
        }

        var result = new StringBuilder(length);
        foreach (var byteValue in randomBytes)
        {
            result.Append(validChars[byteValue % validChars.Length]);
        }

        return result.ToString();
    }

    public static string GenerateToken(DateTime expiry, int maYc)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(SecretKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim("expiry", expiry.ToString()),
                new Claim("maYc", maYc.ToString()) // Thêm mã yêu cầu vào các claim
            }),
            Expires = expiry,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public static DateTime? ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(SecretKey);

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var expiryClaim = jwtToken.Claims.First(x => x.Type == "expiry").Value;

            return DateTime.Parse(expiryClaim);
        }
        catch
        {
            return null;
        }
    }

    public static int? GetMaYcFromToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            var maYcClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == "maYc");
            if (maYcClaim != null && int.TryParse(maYcClaim.Value, out int maYc))
            {
                return maYc;
            }
        }
        catch (Exception)
        {
            // Xử lý ngoại lệ nếu có lỗi trong quá trình trích xuất mã yêu cầu từ token
        }
        return null;
    }
}