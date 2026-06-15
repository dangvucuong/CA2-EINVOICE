using Microsoft.Extensions.Configuration;
using Common;
namespace Model.Static
{
    public enum eConectionStringKey
    {
        DefaultConnection,
        LogConnection
    }
    public static class AppSettings
    {
        public static Dictionary<string, string> DbConnections { get; private set; }
        // public static string DefaultConnection { get; private set; }
        // public static string LogConnection { get; private set; }
        public static JwtConfig JwtConfig { get; private set; }
        public static FixedValue FixedValue { get; private set; }
        public static AWSS3Config AWSS3Config { get; private set; }
        public static WSInterTRCA2 WSInterTRCA2Config { get; private set; }
        public static GipNcm_V2 GipNcm_V2Config { get; private set; }
        public static ApiSignHd ApiSignHd { get; private set; }
        public static RedisConfig RedisConfig { get; private set; }
        public static EmailConfig EmailConfig { get; private set; }
        public static GoogleRecaptcha GoogleRecaptcha { get; private set; }
        public static RabbitConfiguration RabbitHoaDonPhatHanhConfiguration { get; private set; }
        public static RabbitMqConfiguration RabbitMqEmail { get; private set; }
        public static RabbitMqConfiguration RabbitMqHoaDonMessageToVender { get; private set; }
        public static RabbitMqConfiguration RabbitMqRemoteSigning { get; private set; }
        public static string ContentRootPath { get; set; }

        public static void Ini(IConfiguration configuration)
        {
            DbConnections = new Dictionary<string, string>();
            // DefaultConnection = "DefaultConnection";
            // LogConnection = "LogConnection";
            DbConnections.Add(eConectionStringKey.DefaultConnection.ToString(), configuration["ConnectionStrings:DefaultConnection"]);
            DbConnections.Add(eConectionStringKey.LogConnection.ToString(), configuration["ConnectionStrings:LogConnection"]);
            JwtConfig = new JwtConfig()
            {
                Key = configuration["JWT:key"] + configuration["JWT:key"] + configuration["JWT:key"],
                Issuer = configuration["JWT:issuer"]
            };
            FixedValue = new FixedValue()
            {
                LimitOTPPerDay = configuration["FixedValue:LimitOTPPerDay"] != null ? int.Parse(configuration["FixedValue:LimitOTPPerDay"]) : 0,
                DevPassword = configuration["FixedValue:DevPassword"],
                AuthToken = configuration["FixedValue:AuthToken"],
                FileDomain = configuration["FixedValue:FileDomain"],
                MNGui = configuration["FixedValue:MNGui"],
                MNNhan = configuration["FixedValue:MNNhan"],
                RegistedDomainPasskey = configuration["FixedValue:RegistedDomainPasskey"],
                QRCode = configuration["FixedValue:QRCode"],
                EcoSystemToken = configuration["FixedValue:EcoSystemToken"],
                ShowSwaggerUI = configuration["FixedValue:ShowSwaggerUI"].ConvertToBoolean(),
                RemoteSigningDomain = configuration["FixedValue:RemoteSigningDomain"].ConvertToString(),
                RemoteSigningWaittimeSecond = configuration["FixedValue:RemoteSigningWaittimeSecond"].ConvertToInt(),
                RemoteSigningDurationSecond = configuration["FixedValue:RemoteSigningDurationSecond"].ConvertToInt(),
                DocSoWebserviceEndpoint = configuration["FixedValue:DocSoWebserviceEndpoint"].ConvertToString(),
                LogThongDiep = configuration["FixedValue:LogThongDiep"].ConvertToBoolean(),
                DisableConsummer = configuration["FixedValue:DisableConsummer"].ConvertToBoolean(),
            };
            AWSS3Config = new AWSS3Config()
            {
                AccessKey = configuration["AWS_S3:AccessKey"],
                BucketName = configuration["AWS_S3:BucketName"],
                DefaultFolder = configuration["AWS_S3:DefaultFolder"],
                SecretKey = configuration["AWS_S3:SecretKey"]
            };
            GoogleRecaptcha = new GoogleRecaptcha()
            {
                ClientID = configuration["GoogleRecaptcha:ClientID"],
                SecretKey = configuration["GoogleRecaptcha:SecretKey"],
            };
            RedisConfig = new RedisConfig()
            {
                Host = configuration["RedisConfig:Host"],
                Port = configuration["RedisConfig:Port"] != null ? int.Parse(configuration["RedisConfig:Port"]) : 0,
            };
            EmailConfig = new EmailConfig()
            {
                FromEmailAddress = configuration["Email:UserMail"].ConvertToString(),
                Host = configuration["Email:Host"].ConvertToString(),
                Port = configuration["Email:Port"].ConvertToInt(),
                UserEmail = configuration["Email:UserName"].ConvertToString(),
                PasswordEmail = configuration["Email:Password"].ConvertToString(),
                EnableSsl = configuration["Email:EnableSSL"].ConvertToBoolean(),
                DisplayName = configuration["Email:DisplayName"].ConvertToString(),
                FixedEmail = configuration["Email:FixedEmail"].ConvertToString()
            };
            WSInterTRCA2Config = new WSInterTRCA2()
            {
                Endpoint = configuration["WSInterTRCA2:Endpoint"].ConvertToString(),
                Username = configuration["WSInterTRCA2:Username"].ConvertToString(),
                Password = configuration["WSInterTRCA2:Password"].ConvertToString(),
            };

            GipNcm_V2Config = new GipNcm_V2()
            {
                Endpoint = configuration["GipNcm_V2:Endpoint"].ConvertToString(),
                Username = configuration["GipNcm_V2:Username"].ConvertToString(),
                Password = configuration["GipNcm_V2:Password"].ConvertToString(),
            };

            ApiSignHd = new ApiSignHd()
            {
                Endpoint = configuration["ApiSignHd:Endpoint"].ConvertToString(),
                MST = configuration["ApiSignHd:MST"].ConvertToString(),
                Serial = configuration["ApiSignHd:Serial"].ConvertToString(),

            };
            RabbitHoaDonPhatHanhConfiguration = new RabbitConfiguration()
            {
                ExchangeName = configuration["RabbitHoaDonPhatHanhConfiguration:ExchangeName"].ConvertToString(),
                HostName = configuration["RabbitHoaDonPhatHanhConfiguration:HostName"].ConvertToString(),
                Password = configuration["RabbitHoaDonPhatHanhConfiguration:Password"].ConvertToString(),
                Port = configuration["RabbitHoaDonPhatHanhConfiguration:Port"].ConvertToInt(),
                QueueName = configuration["RabbitHoaDonPhatHanhConfiguration:QueueName"].ConvertToString(),
                UserName = configuration["RabbitHoaDonPhatHanhConfiguration:UserName"].ConvertToString(),
                VirtualHost = configuration["RabbitHoaDonPhatHanhConfiguration:VirtualHost"].ConvertToString(),
            };
            RabbitMqEmail = new RabbitMqConfiguration()
            {
                UserName = configuration["RabbitMqEmail:UserName"].ConvertToString(),
                Password = configuration["RabbitMqEmail:Password"].ConvertToString(),
                VirtualHost = configuration["RabbitMqEmail:VirtualHost"].ConvertToString(),
                HostName = configuration["RabbitMqEmail:HostName"].ConvertToString(),
                Port = configuration["RabbitMqEmail:Port"].ConvertToInt(),
                QueueName = configuration["RabbitMqEmail:QueueName"].ConvertToString(),
                ExchangeName = configuration["RabbitMqEmail:ExchangeName"].ConvertToString(),

            };
            RabbitMqHoaDonMessageToVender = new RabbitMqConfiguration()
            {
                UserName = configuration["RabbitMqHoaDonMessageToVender:UserName"].ConvertToString(),
                Password = configuration["RabbitMqHoaDonMessageToVender:Password"].ConvertToString(),
                VirtualHost = configuration["RabbitMqHoaDonMessageToVender:VirtualHost"].ConvertToString(),
                HostName = configuration["RabbitMqHoaDonMessageToVender:HostName"].ConvertToString(),
                Port = configuration["RabbitMqHoaDonMessageToVender:Port"].ConvertToInt(),
                QueueName = configuration["RabbitMqHoaDonMessageToVender:QueueName"].ConvertToString(),
                ExchangeName = configuration["RabbitMqHoaDonMessageToVender:ExchangeName"].ConvertToString(),

            };
            RabbitMqRemoteSigning = new RabbitMqConfiguration()
            {
                UserName = configuration["RabbitRemoteSigningConfiguration:UserName"].ConvertToString(),
                Password = configuration["RabbitRemoteSigningConfiguration:Password"].ConvertToString(),
                VirtualHost = configuration["RabbitRemoteSigningConfiguration:VirtualHost"].ConvertToString(),
                HostName = configuration["RabbitRemoteSigningConfiguration:HostName"].ConvertToString(),
                Port = configuration["RabbitRemoteSigningConfiguration:Port"].ConvertToInt(),
                QueueName = configuration["RabbitRemoteSigningConfiguration:QueueName"].ConvertToString(),
                ExchangeName = configuration["RabbitRemoteSigningConfiguration:ExchangeName"].ConvertToString(),

            };
        }

    }

    public class DbConnection
    {
        public string ConnectionName { get; set; }
        public string ConnectionString { get; set; }
    }
    public class JwtConfig
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
    }
    public class FixedValue
    {
        public int LimitOTPPerDay { get; set; }
        public string DevPassword { get; set; }
        public string AuthToken { get; set; }
        public string FileDomain { get; set; }
        public string RegistedDomainPasskey { get; set; }
        public string MNGui { get; set; }
        public string MNNhan { get; set; }
        public string QRCode { get; set; }
        public string EcoSystemToken { get; set; }
        public string RemoteSigningDomain { get; set; }
        public int RemoteSigningWaittimeSecond { get; set; }
        public int RemoteSigningDurationSecond { get; set; }
        public bool ShowSwaggerUI { get; set; }
        public string DocSoWebserviceEndpoint { get; set; }
        public bool LogThongDiep { get; set; }
        public bool DisableConsummer { get; set; }


    }

    public class AWSS3Config
    {
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string BucketName { get; set; }
        public string DefaultFolder { get; set; }
        public string Domain
        {
            get
            {
                return $"https://{BucketName}.s3.ap-southeast-1.amazonaws.com";
            }
        }

    }
    public class RedisConfig
    {
        public string Host { get; set; }
        public int Port { get; set; }
    }
    public sealed class RabbitMqConfiguration
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string VirtualHost { get; set; }
        public string HostName { get; set; }
        public int Port { get; set; }

        public string QueueName { get; set; }
        public string ExchangeName { get; set; }
    }
    public class EmailConfig
    {
        public string FromEmailAddress { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string UserEmail { get; set; }
        public string PasswordEmail { get; set; }
        public bool EnableSsl { get; set; }
        public string DisplayName { get; set; }
        public string FixedEmail { get; set; }

    }
    public class GoogleRecaptcha
    {
        public string SecretKey { get; set; }
        public string ClientID { get; set; }
    }
    public class WSInterTRCA2
    {
        public string Endpoint { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class GipNcm_V2
    {
        public string Endpoint { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class ApiSignHd
    {
        public string Endpoint { get; set; }
        public string MST { get; set; }
        public string Serial { get; set; }

    }
    public class RabbitConfiguration
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string VirtualHost { get; set; }
        public string HostName { get; set; }
        public int Port { get; set; }

        public string QueueName { get; set; }
        public string ExchangeName { get; set; }
    }
}

