ARG VARIANT=8.0
FROM mcr.microsoft.com/vscode/devcontainers/dotnet:${VARIANT}
ARG NODE_VERSION="none"
RUN if [ "${NODE_VERSION}" != "none" ]; then su vscode -c "umask 0002 && . /usr/local/share/nvm/nvm.sh && nvm install ${NODE_VERSION} 2>&1"; fi

RUN apt-get update && export DEBIAN_FRONTEND=noninteractive \
    && apt-get -y install --no-install-recommends \
    unzip \
    && apt-get clean -y && rm -rf /var/lib/apt/lists/*
