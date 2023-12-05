# mon-petit-surf-back
Bienvenue dans l'API de Mon Petit Surf, une application ASP.Net Core qui permet de consulter les conditions de surf d'une soixantaine de spots de la côte française.

## Configuration requise
- .NET Core SDK
- Visual Studio

## Configuration des secrets JWT
Pour que l'application fonctionne correctement, vous devez configurer les secrets liés au JSON Web Token (JWT). Suivez ces étapes :
1. Créez une classe 'Secrets.cs' dans le dossier 'MonPetitSurf'

2. Ajoutez les informations nécessaires :
```sh
internal class Secrets
{
  public const string JWT_SECRETS = "..."
  public const string Issuer = "..."
  public const string Audience = "..."
}
```

3. Gardez ces secrets en sécurité en les ajoutant à votre .gitignore

## Dépendances
- BCrypt.Net - Version 0.1.0
- Microsoft.AspNetCore.Authentication.JwtBearer - Version 7.0.13
- Microsoft.AspNetCore.OpenApi - Version 7.0.12
- Microsoft.EntityFrameworkCore - Version 7.0.12
- Microsoft.EntityFrameworkCore.Tools - Version 7.0.12
- Microsoft.Extensions.Configuration.UserSecrets - Version 7.0.0
- Microsoft.IdentityModel.Tokens - Version 7.0.3
- MySql.Data - Version 8.1.0
- Pomelo.EntityFrameworkCore.MySql - Version 7.0.0
- Swashbuckle.AspNetCore - Version 6.5.0
- Swashbuckle.AspNetCore.Filters - Version 7.0.12
- System.IdentityModel.Tokens.Jwt - Version 7.0.3

## Installation et execution
1. Clonez le référentiel
```sh
git clone https://github.com/draosi/mon-petit-surf-back.git
```

2. Naviguez vers le dossier du projet
```sh
cd mon-petit-surf-back
```

3. Ouvrez le projet dans visual studio ou utilisez la ligne de commande
```sh
dotnet run
```
