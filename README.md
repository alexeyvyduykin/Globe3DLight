## Globe3DLight
 
 
## Table of contents 

- [Status](#status)
- [Quick start](#quick-start)
- [Documentation](#documentation)
 

## Status

[![CodeFactor](https://www.codefactor.io/repository/github/alexeyvyduykin/globe3dlight/badge/main)](https://www.codefactor.io/repository/github/alexeyvyduykin/globe3dlight/overview/main)
[![GitHub release](https://img.shields.io/github/release/alexeyvyduykin/globe3dlight.svg?logo=github)](https://gitHub.com/alexeyvyduykin/globe3dlight/releases/latest)
[![GitHub tag](https://img.shields.io/github/tag/alexeyvyduykin/globe3dlight.svg?logo=github)](https://GitHub.com/alexeyvyduykin/globe3dlight/tags/)
[![GPLv3 license](https://img.shields.io/badge/License-GPLv3-blue.svg)](http://perso.crans.org/besson/LICENSE.html)
[![GitHub license](https://img.shields.io/github/license/alexeyvyduykin/globe3dlight.svg?logo=github)](https://github.com/alexeyvyduykin/globe3dlight/LICENSE)


## Quick start

### Процесс установки на Linux

#### 1. Перед установкой .NET выполните приведенные ниже команды, чтобы добавить ключ подписывания пакета Майкрософт в список доверенных ключей и добавить репозиторий пакетов.

```powershell
wget -O - https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor > microsoft.asc.gpg
sudo mv microsoft.asc.gpg /etc/apt/trusted.gpg.d/
wget https://packages.microsoft.com/config/debian/9/prod.list
sudo mv prod.list /etc/apt/sources.list.d/microsoft-prod.list
sudo chown root:root /etc/apt/trusted.gpg.d/microsoft.asc.gpg
sudo chown root:root /etc/apt/sources.list.d/microsoft-prod.list
```

#### 2. Установка пакета SDK для .NET:

```powershell
sudo apt-get update; \
sudo apt-get install -y apt-transport-https && \
sudo apt-get update && \
sudo apt-get install -y dotnet-sdk-5.0
```

#### 3. Построение проекта Globe3DLight:

* Перейдите к папке где расположен проект Globe3DLight:

```powershell
cd Globe3DLight
```

* Запустите построение:

```powershell
dotnet build
```

#### 4. Запуск проекта Globe3DLight:

* Запуск пректа:

```powershell
dotnet run
```


## Documentation
 
