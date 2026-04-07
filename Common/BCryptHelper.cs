namespace Common
{
    public static class BCryptHelper
    {

        public static string GenerateBcrypt(this string value)
        {
            var pwdToHash = value;
            var hash = BCrypt.Net.BCrypt.HashPassword(pwdToHash, BCrypt.Net.BCrypt.GenerateSalt());
            return hash;
        }


        public static bool isMatch(this string value, string hashedValue)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(value, hashedValue);
            }
            catch (Exception)
            {

                return false;
            }

        }
    }
}