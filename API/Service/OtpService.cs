namespace API.Service;

public class OtpService
{
        private readonly Dictionary<string, OtpEntry> _otpStorage = new Dictionary<string, OtpEntry>();
        private static Random _random = new Random();
        
        public OtpService()
        {
        }
        
        public string GenerateOtp(string emailOrPhone)
        {
            string otp = GenerateUniqueOtp();
            if (_otpStorage.ContainsKey(emailOrPhone))
            {
                var otpEntry = _otpStorage[emailOrPhone];
                otpEntry.Otp = otp;
                otpEntry.ExpirationTime = DateTime.UtcNow.AddMinutes(15);
            }
            else
            {
                var otpEntry = new OtpEntry
                {
                    Otp = otp,
                    ExpirationTime = DateTime.UtcNow.AddMinutes(15),
                    EmailOrPhone = emailOrPhone
                };
                _otpStorage[emailOrPhone] = otpEntry;
            }
            return otp;
        }

        public string? CheckOtp(string otp)
        {
            var otpEntry = _otpStorage.Values.FirstOrDefault(o => o.Otp == otp);

            if (otpEntry != null && otpEntry.ExpirationTime > DateTime.UtcNow)
            {
                return otpEntry.EmailOrPhone;
            }
            return null;
        }

        private string GenerateUniqueOtp()
        {
            string otp;
            do
            {
                otp = GenerateOtpDigits();
            } while (_otpStorage.Values.Any(o => o.Otp == otp));

            return otp;
        }

        private string GenerateOtpDigits()
        {
            return _random.Next(100000, 999999).ToString();
        }
        public void RemoveOtp(string key)
        {
            if (_otpStorage.ContainsKey(key))
            {
                _otpStorage.Remove(key);
                Console.WriteLine($"OTP for {key} removed.");
            }
            else
            {
                Console.WriteLine($"No OTP found for {key}.");
            }
        }

        private class OtpEntry
        {
            public string Otp { get; set; }
            public DateTime ExpirationTime { get; set; }
            public string EmailOrPhone { get; set; }
        }
}