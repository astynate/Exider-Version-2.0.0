﻿using Instend.Core.Models.Account;

namespace Instend_Version_2._0._0.Server.TransferModels.Account
{
    public record UpdateAccountTranferModel 
    (
        string? name,
        string? surname,
        string? nickname,
        string? avatar,
        string? description,
        AccountLink[]? links,
        DateOnly? dateOfBirth
    );
}