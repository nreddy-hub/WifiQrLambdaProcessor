# WifiQrLambdaProcessor

A small AWS Lambda project (C# / .NET) that processes WiFi QR payloads and publishes a `WifiQrCreatedMessage`.

## Contents
- `Function.cs` — Lambda handler implementation
- `WifiQrCreatedMessage.cs` — message model used by the function
- `aws-lambda-tools-defaults.json` — defaults for `dotnet lambda` commands
- `test/` — unit tests for the project

## Prerequisites
- .NET SDK (check `TargetFramework` in the .csproj)
- Amazon.Lambda.Tools (optional for packaging/deploying):

```powershell
dotnet tool install -g Amazon.Lambda.Tools
```

Or update if already installed:

```powershell
dotnet tool update -g Amazon.Lambda.Tools
```

## Build
From the project folder:

```powershell
cd src\WifiQrLambdaProcessor
dotnet build -c Release
```

## Test

```powershell
cd test\WifiQrLambdaProcessor.Tests
dotnet test
```

## Package
Create a deployment package (ZIP) for manual upload:

```powershell
cd src\WifiQrLambdaProcessor
dotnet lambda package -c Release -o ..\\..\\wifi-qr-lambda.zip
```

## Deploy
Option A — Use `dotnet lambda deploy-function` (uses `aws-lambda-tools-defaults.json` or prompts for values):

```powershell
cd src\WifiQrLambdaProcessor
dotnet lambda deploy-function
```

Option B — Use CI/GitHub Actions to build and deploy the ZIP artifact.

## Configuration
Edit `aws-lambda-tools-defaults.json` to set function name, runtime, memory, and other defaults used by `dotnet lambda`.

## Development notes
- Inspect `Function.cs` and `WifiQrCreatedMessage.cs` for the function's input/output shapes.
- Add `.gitignore` to exclude `bin/`, `obj/`, `*.zip`, and IDE/user files.

## Contributing
Fork, create a branch, add tests for new behavior, and open a pull request.

## License
Specify your project license here (e.g., MIT) or add a `LICENSE` file.
