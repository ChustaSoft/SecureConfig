# SecureConfig
Tool to give security to the configurations of an ASPNET Core application by encrypting sensitive information and handling in a secure way

Prerequisites:
· Versions 2.x.x
 - NET Core 2.2 and above


· Description:

This tool allows to encrypt configuration sections on app.settings files, and decrypt the information in runtime adding those configuration as a singleton inside the application.


· Getting started:

1. Install ChustaSoft.Tools.SecureConfig package via NuGet Package manager

2. Setup a 32 hexadecimal character private key. (ie: a GUID) in a secure way, ie: as a environment variable

3. Create a Settings object inside the project, should match the section that will be encrypted

4. Add the Settings in all the different environment appsettings

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

Full example:
- https://github.com/ChustaSoft/SecureConfig/tree/master/ChustaSoft.Tools.SecureConfig.TestApi

Configuration video tutorial:
- https://twitter.com/ChustaSoft/status/1198636624340488192


That's all!

Enjoy it and do not hesitate to contribute with us.



Follow us on Twitter:
- https://twitter.com/ChustaSoft
