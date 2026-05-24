using Microsoft.IdentityModel.Tokens;

namespace AxisMart.Application.Shared.Authentication;

public interface IRsaKeyProvider
{
    RsaSecurityKey GetPrivateKey();
    RsaSecurityKey GetPublicKey();
}