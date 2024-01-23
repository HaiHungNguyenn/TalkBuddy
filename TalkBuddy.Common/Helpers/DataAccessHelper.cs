﻿using Microsoft.Extensions.Configuration;
using TalkBuddy.Common.Constants;
using TalkBuddy.Common.Exceptions;

namespace TalkBuddy.Common.Helpers;

public class DataAccessHelper
{
    private static IConfiguration _configuration
    {
        get => _configuration ?? throw new Exception("Configuration is not initialized");
        set => _configuration = value;
    }

    public static void InitConfiguration(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public static string GetConnectionString(string connectionName) => _configuration.GetConnectionString(connectionName) ?? throw new MissingConnectionStringException("Cannot find the specified connection string");

    public static string GetDefaultConnectionString() => GetConnectionString(DataAccessConstants.DEFAULT_CONNECTION_NAME);
}