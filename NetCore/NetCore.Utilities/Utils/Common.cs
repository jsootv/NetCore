using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace NetCore.Utilities.Utils
{
    public static class Common
    {
        // Package 추가 : Microsoft.AspNetCore.Routing.Abstractions
        //         버전 : 2.1.1
        /// <summary>
        /// 라우트 데이터값 가져오기
        /// </summary>
        /// <param name="routeData">라우트 데이터</param>
        /// <param name="key">키 값</param>
        /// <returns></returns>
        public static string GetRouteDataVal(RouteData routeData, string key)
        {
            return routeData.Values[key] != null ? routeData.Values[key] as string : string.Empty;
        }

        /// <summary>
        /// 로그인을 요구하지 않는 링크(페이지)인지 체크하라
        /// </summary>
        /// <param name="controllerName">컨트롤러명</param>
        /// <param name="actionName">액션명</param>
        /// <returns></returns>
        public static bool CheckTheLinksNeedNotLogIn(string controllerName, string actionName)
        {
            List<string> links = new List<string>();
            links.Add("/Home/Index");
            links.Add("/Home/Privacy");
            links.Add("/Home/Error");
            links.Add("/Membership/Index");
            links.Add("/Membership/Login");
            links.Add("/Membership/Register");
            links.Add("/Data/AES");
            links.Add("/Data/Cart");

            return links.Any(lnk => string.Equals(lnk, $"/{controllerName}/{actionName}", StringComparison.OrdinalIgnoreCase));
        }
        
        /// <summary>
        /// Data Protection 지정하기
        /// </summary>
        /// <param name="services">등록할 서비스</param>
        /// <param name="keyPath">키 경로</param>
        /// <param name="applicationName">애플리케이션 이름</param>
        /// <param name="cryptoType">암호화 유형</param>
        public static void SetDataProtection(IServiceCollection services, string keyPath, string applicationName, Enum cryptoType)
        {
            var builder = services.AddDataProtection()
                    .PersistKeysToFileSystem(new DirectoryInfo(keyPath))
                    .SetDefaultKeyLifetime(TimeSpan.FromDays(7))
                    .SetApplicationName(applicationName);

            switch (cryptoType)
            {
                case Enums.CryptoType.Unmanaged:
                    //AES
                    //Advanced Encryption Standard
                    //Two-way : 암호화, 복호화
                    builder.UseCryptographicAlgorithms(
                        new AuthenticatedEncryptorConfiguration()
                        {
                            EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
                            //SHA
                            //Secure Hash Algorithm
                            //One-way : 암호화
                            ValidationAlgorithm = ValidationAlgorithm.HMACSHA512
                        });
                    break;
                case Enums.CryptoType.Managed:
                    builder.UseCustomCryptographicAlgorithms(
                        new ManagedAuthenticatedEncryptorConfiguration()
                        {
                            // A type that subclasses SymmetricAlgorithm
                            EncryptionAlgorithmType = typeof(Aes),

                            // Specified in bits
                            EncryptionAlgorithmKeySize = 256,

                            // A type that subclasses KeyedHashAlgorithm
                            ValidationAlgorithmType = typeof(HMACSHA512)
                        });
                    break;
                case Enums.CryptoType.CngGcm:
                    //Windows CNG algorithm using Galois/Counter Mode encryption
                    //Galois/Counter Mode
                    //GCM
                    builder.UseCustomCryptographicAlgorithms(
                        new CngGcmAuthenticatedEncryptorConfiguration()
                        {
                            // Passed to BCryptOpenAlgorithmProvider
                            EncryptionAlgorithm = "AES",
                            EncryptionAlgorithmProvider = null,

                            // Specified in bits
                            EncryptionAlgorithmKeySize = 256
                        });
                    break;
                case Enums.CryptoType.CngCbc:
                default:
                    //Windows CNG algorithm using CBC-mode encryption
                    //CNG algorithm
                    //Cryptography API : Next Generation
                    //CBC-mode
                    //Cipher Block Chaining
                    builder.UseCustomCryptographicAlgorithms(
                        new CngCbcAuthenticatedEncryptorConfiguration()
                        {
                            // Passed to BCryptOpenAlgorithmProvider
                            EncryptionAlgorithm = "AES",
                            EncryptionAlgorithmProvider = null,

                            // Specified in bits
                            EncryptionAlgorithmKeySize = 256,

                            // Passed to BCryptOpenAlgorithmProvider
                            HashAlgorithm = "SHA512",
                            HashAlgorithmProvider = null
                        });
                    break;
            }
        }
    }
}
