FROM mcr.microsoft.com/vscode/devcontainers/dotnet:8.0

RUN apt-get update && export DEBIAN_FRONTEND=noninteractive && \
    apt-get -y install --no-install-recommends \
    # Install any apt packages here
    unzip && \
    apt-get clean -y && rm -rf /var/lib/apt/lists/*
