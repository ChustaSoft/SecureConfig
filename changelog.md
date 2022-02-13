# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

### [1.4.1] - 2022-02-13
### Changed
- Internal documentation references changed

### [1.4.0] - 2022-02-13
### Added
- Added support to .NET 6.0
### Changed
- Encrypt flag by default as true in all TFM's

## [1.3.0] - 2021-06-12
### Added
- Added support to .NET 5.0

## [1.2.4] - 2020-09-19
### Fixed
- Fixed critical bug on UNIX systems where the paths of the appSettings files were not taken properly

## [1.2.3] - 2020-09-11
### Removed
- Removed unnecessary appSettings development check due to previous change on .NET Core runtime folder management

## [1.2.2] - 2020-09-07
### Changed
- Allowed to use custom AppSetting's param name for appSettings sections. Until this version, was mandatory to have the section to encrypt called "AppSettings", now it is possible to specify the name.

## [1.2.1] - 2020-08-08
### Changed
- Configuration extensions now return the Settings files allowing to be used by the rest of the registrations

## [1.2.0] - 2020-06-30
### Fixed
- NetCore 3.0 and 3.1 fix for encryptation mechanism on Program

## [1.1.0] - 2020-02-27
### Added
- NetCore 3.0 and 3.1 support

## [1.0.2] - 2020-01-04
### Changed
- Allowed to use any kind of private key to encrypt/decrypt the config file

## [1.0.1] - 2019-11-07
### Fixed
- Fixed critical bug not allowing the tool to find appsettings path different than the default one

## [1.0.0] - 2019-11-06
### Added
- Encryption/Decryption of configured sections inside appsettings
- Manage of multiple environment encrypted/decrypted settings 
- Installed as NuGet package
- Singleton registration of the configured settings