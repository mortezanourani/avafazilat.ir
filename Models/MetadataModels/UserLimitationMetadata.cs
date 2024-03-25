using Microsoft.AspNetCore.Mvc;

namespace Fazilat.Models;

public class UserLimitationMetadata
{
}

[ModelMetadataType(typeof(UserLimitationMetadata))]
public partial class UserLimitation
{
    public virtual int ExpirationYear { get; set; }

    public virtual int ExpirationMonth { get; set; }
}
