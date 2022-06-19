Kaiayuanshe DevOps on Azure
======

[![Build and Deploy](https://github.com/kaiyuanshe/kaiyuanshe-devops-azure/actions/workflows/deploy.yml/badge.svg)](https://github.com/kaiyuanshe/kaiyuanshe-devops-azure/actions/workflows/deploy.yml)

# Introduction
A Functions project to automate Azure Resources management used by kaiyuanshe projects. 

Included operations:
- Cleanup unused container images in ACR.
- Auto Configure CDN when a certificate is issued by the [cert bot](https://github.com/shibayan/keyvault-acmebot).


# Build and Deploy

- Build: Clone the source codes and follow guidance for [Visual Code](https://docs.microsoft.com/en-us/azure/azure-functions/create-first-function-vs-code-csharp?tabs=in-process#run-the-function-locally) or [Visual Studio](https://docs.microsoft.com/en-us/azure/azure-functions/functions-create-your-first-function-visual-studio?tabs=in-process#run-the-function-locally) to run locally.
- Setup Functions on Azure: Follow [Deploy the Bicep File](https://docs.microsoft.com/en-us/azure/azure-functions/functions-create-first-function-bicep?tabs=CLI%2Cvisual-studio-code#deploy-the-bicep-file) to deploy the `deploy.bicep` file. A frequent met issue is [WorkerConfig for runtime: dotnet-isolated not found](https://github.com/Azure/azure-functions-dotnet-worker/issues/821) after deployed the bicep file. A workaround is to [set the Platform to 64 bit](https://github.com/Azure/azure-functions-dotnet-worker/issues/821#issuecomment-1131210336).
- Deploy Functions: follow [Build and Deploy using Github Actions](https://docs.microsoft.com/en-us/azure/azure-functions/functions-how-to-github-actions?tabs=dotnet) to setup Github Actions.
- More Guidance to develop C# Isolated process Functions: https://docs.microsoft.com/en-us/azure/azure-functions/dotnet-isolated-process-guide

# Contributing
Pull request is welcome and appreciated.

Questions go to [dev team](mailto:infra@kaiyuanshe.org)

# Author
The project is now maintained by Kaiyuanshe.

# License
This project is licensed under [MIT License](https://github.com/kaiyuanshe/kaiyuanshe-devops-azure/blob/main/LICENSE)