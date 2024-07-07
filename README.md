# Planthor Identity Server README

[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=Planthor_PlanthorIdentityServer&metric=code_smells)](https://sonarcloud.io/summary/new_code?id=Planthor_PlanthorIdentityServer)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=Planthor_PlanthorIdentityServer&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=Planthor_PlanthorIdentityServer)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=Planthor_PlanthorIdentityServer&metric=bugs)](https://sonarcloud.io/summary/new_code?id=Planthor_PlanthorIdentityServer)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=Planthor_PlanthorIdentityServer&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=Planthor_PlanthorIdentityServer)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=Planthor_PlanthorIdentityServer&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=Planthor_PlanthorIdentityServer)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=Planthor_PlanthorIdentityServer&metric=coverage)](https://sonarcloud.io/summary/new_code?id=Planthor_PlanthorIdentityServer)

## Getting Started

This section will provide you with all the necessary information to get the project up and running on your local machine for development and testing purposes.

## Prerequisites

.NET 8.0 SDK
An IDE (Visual Studio, JetBrains Rider or VS Code)
Duende Identity Server v7

## Installation

- Clone the repository

```sh
git clone https://github.com/Planthor/PlanthorIdentityServer.git
```

- Install Nuget packages

```sh
dotnet restore
```

- Run the project

```sh
dotnet run
```

## For run with docker-compose

- Generate self-certificates from `./infrastructure/certificates`.
- Use Docker compose file in `./infrastructure` folder.

## Facebook authentication local

- Contact owner or administration for Planthor Sandbox Facebook apps id and secret.
- Apply bash command to store app id and secret in secret Environment Variables.

```bash
dotnet user-secrets set "Authentication:Facebook:AppId" "<app-id>"
dotnet user-secrets set "Authentication:Facebook:AppSecret" "<app-secret>"
```

- Start the identity application as usual.

## Usage

This project serves as an Identity Provider (IdP) using Duende Identity Server v7. It can be utilized to authenticate and authorize users in Planthor applications, APIs, etc.

## Troubleshooting

If you encounter any issues while installing or running the project, please check the Troubleshooting guide. If the problem persists, feel free to create an issue.

## Contributing

Contributions are what make the open-source community such an amazing place to learn, inspire, and create. Any contributions you make are greatly appreciated.

## Fork the Project

- Create your Feature Branch (git checkout -b feature/AmazingFeature)
- Commit your Changes (git commit -m 'Add some AmazingFeature')
- Push to the Branch (git push origin feature/AmazingFeature)
- Open a Pull Request

## License

Distributed under the MIT License. See LICENSE for more information.

## Contact

Pham Le Trung - <trunglepham1202@gmail.com>

Project Link: https://github.com/Planthor

Please feel free to contact us if you need any further information or have any questions or issues. Happy coding!
