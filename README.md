# SecureConfig
---
[![Build Status](https://dev.azure.com/chustasoft/SocialNET/_apis/build/status/OpenStack/SecureConfig/%5BRELEASE%5D%20-%20ChustaSoft%20SecureConfig%20(NuGet)?branchName=main)](https://dev.azure.com/chustasoft/SocialNET/_build/latest?definitionId=17&branchName=main) [![NuGet](https://img.shields.io/nuget/v/ChustaSoft.Tools.SecureConfig )](https://www.nuget.org/packages/ChustaSoft.Tools.SecureConfig) ![Nuget](https://img.shields.io/nuget/dt/ChustaSoft.Tools.SecureConfig) ![GitHub](https://img.shields.io/github/license/ChustaSoft/SecureConfig)

Tool to give security to the configurations of an ASPNET Core application by encrypting sensitive information and handling in a secure way


### Compatibility table

| Framework     | From   | Latest  | Current support    |
|---------------|--------|---------|--------------------|
| .Net Core 2.1 | 1.0.0  | 1.0.0   | :x:                |
| .Net Core 3.1 | 1.2.0  | Current | :heavy_check_mark: |
| .NET 5.0      | 1.3.0  | Current | :heavy_check_mark: |
| .NET 6.0      | 1.4.0  | Current | :heavy_check_mark: |


## Description:

This tool allows to encrypt configuration sections on app.settings files, and decrypt the information in runtime adding those configuration as a singleton inside the application.


· Getting started:

1. Install ChustaSoft.Tools.SecureConfig package via NuGet Package manager

2. Setup a private key in a secure way (ie: as a environment variable), SecureConfig will use it for encrypt and decrypt the settings files

3. Create a Settings object inside the project, should match the section that will be encrypted

4. Add the AppSettings section in all the different environment appsettings

5. Configure the tool
    - If your project framework is .NET 6.0
      - At Program
        ```chsarp
        var settings = builder.SetUpSecureConfig<AppSettings>(testApikey);

        // [...] More stuff
        
        // After build called from WebApplicationBuilder
        // [...] More stuff

        app.EncryptSettings<AppSettings>();

        ```   	

    - If your project framework is under .NET Core 2.1, 3.1 or .NET 5.0
      -  At Program
        ```chsarp
        CreateHostBuilder(args)
                        .Build()
                        .EncryptSettings<AppSettings>(true)
                        .Run();
        ```   	
      - At Startup
        ```chsarp        
        var settings = services.SetUpSecureConfig<AppSettings>(_configuration, testApikey);
        ```   	

    - In all TFM's and examples provided, for reference:
      - _testApikey_ is referred to the securely stored key documented at step 2
      - _[TSettings]_ correponds to the settings DTO created in the step 3.
      - Settings DTO (_[TSettings]_) is injected as a singleton in the DI container.
      - By default is _true_ if you want to encrypt the settings, set as _false_ if you want to decrypt the files

6. Inject the settings class object in the class that the project will need, SecureConfig manage this class as a Singleton in the application lifecycle

- [Full example for .NET 6.0](https://github.com/ChustaSoft/SecureConfig/tree/main/ChustaSoft.Tools.SecureConfig.Net6.TestApi)
- [Full example for .NET 5.0](https://github.com/ChustaSoft/SecureConfig/tree/main/ChustaSoft.Tools.SecureConfig.Net5.TestApi)
- [Full example for .NetCore 3.1](https://github.com/ChustaSoft/SecureConfig/tree/main/ChustaSoft.Tools.SecureConfig.NetCore3.TestApi)
- [Full example for .NetCore 2.1](https://github.com/ChustaSoft/SecureConfig/tree/main/ChustaSoft.Tools.SecureConfig.NetCore2.TestApi)

- [Configuration video tutorial](https://twitter.com/ChustaSoft/status/1198636624340488192) _(For versions under 6.0)_


For deep configurations, visit [this section](https://github.com/ChustaSoft/SecureConfig/wiki#deep-configuration) of our current wiki

That's all!

Enjoy it and do not hesitate to contacts us for suggestions or doubts.

*Thanks for using and contributing*
---
[![Twitter Follow](https://img.shields.io/twitter/follow/ChustaSoft?label=Follow%20us&style=social)](https://twitter.com/ChustaSoft)
![YouTube Video Views](https://img.shields.io/youtube/views/-7MBpqpr4ko?style=social)
