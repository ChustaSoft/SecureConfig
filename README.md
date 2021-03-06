# SecureConfig
---
[![Build Status](https://dev.azure.com/chustasoft/SocialNET/_apis/build/status/OpenStack/SecureConfig/%5BRELEASE%5D%20-%20ChustaSoft%20SecureConfig%20(NuGet)?branchName=master)](https://dev.azure.com/chustasoft/SocialNET/_build/latest?definitionId=17&branchName=master) [![NuGet](https://img.shields.io/nuget/v/ChustaSoft.Tools.SecureConfig )](https://www.nuget.org/packages/ChustaSoft.Tools.SecureConfig) ![Nuget](https://img.shields.io/nuget/dt/ChustaSoft.Tools.SecureConfig) ![GitHub](https://img.shields.io/github/license/ChustaSoft/SecureConfig)

Tool to give security to the configurations of an ASPNET Core application by encrypting sensitive information and handling in a secure way

Prerequisites:
- NetCore 2.1
  - Support from version 1.0.0
- NetCore 3.1
  - Support from version 1.2.0
- Net 5.0
  - Support from version 1.3.0


· Description:

This tool allows to encrypt configuration sections on app.settings files, and decrypt the information in runtime adding those configuration as a singleton inside the application.


· Getting started:

1. Install ChustaSoft.Tools.SecureConfig package via NuGet Package manager

2. Setup a private key in a secure way (ie: as a environment variable), SecureConfig will use it for encrypt and decrypt the settings files

3. Create a Settings object inside the project, should match the section that will be encrypted

4. Add the AppSettings section in all the different environment appsettings

5. In Program, add the following line during IWebHost building (through IWebHostBuilder)
   	-   .EncryptSettings<[TSettings]>(true) 
		-   [TSettings] correponds to the settings DTO created in the step 2
		-   _true if you want to encrypt the settings_
		-   _false if you want to decrypt the files_

6. In Startup, on ConfigureServices, add the following line in order to setup the singleton and manage the encrypted/decrypted settings:
	-   services.SetUpSecureConfig<[TSettings]>(Configuration, testApikey);
		-   [TSettings] correpond to the settings DTO created in the step 2
		-   testApikey corresponds to the secret key created in step 1
	
7. Inject the settings class object in the class that the project will need, SecureConfig manage this class as a Singleton in the application lifecycle

- [Full example for .NetCore 2.1](https://github.com/ChustaSoft/SecureConfig/tree/master/ChustaSoft.Tools.SecureConfig.NetCore2.TestApi)
- [Full example for .NetCore 3.1](https://github.com/ChustaSoft/SecureConfig/tree/master/ChustaSoft.Tools.SecureConfig.NetCore3.TestApi)
- [Full example for .NET 5.0](https://github.com/ChustaSoft/SecureConfig/tree/master/ChustaSoft.Tools.SecureConfig.Net5.TestApi)
- [Configuration video tutorial](https://twitter.com/ChustaSoft/status/1198636624340488192)


For deep configurations, visit [this section](https://github.com/ChustaSoft/SecureConfig/wiki#deep-configuration) of our current wiki

That's all!

Enjoy it and do not hesitate to contacts us for suggestions or doubts.

*Thanks for using and contributing*
---
[![Twitter Follow](https://img.shields.io/twitter/follow/ChustaSoft?label=Follow%20us&style=social)](https://twitter.com/ChustaSoft)
![YouTube Video Views](https://img.shields.io/youtube/views/-7MBpqpr4ko?style=social)
