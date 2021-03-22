# Globe3DLight
 

## Процесс установки на Linux

1. Перед установкой .NET выполните приведенные ниже команды, чтобы добавить ключ подписывания пакета Майкрософт в 
список доверенных ключей и добавить репозиторий пакетов.

  wget -O - https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor > microsoft.asc.gpg
  sudo mv microsoft.asc.gpg /etc/apt/trusted.gpg.d/
  wget https://packages.microsoft.com/config/debian/9/prod.list
  sudo mv prod.list /etc/apt/sources.list.d/microsoft-prod.list
  sudo chown root:root /etc/apt/trusted.gpg.d/microsoft.asc.gpg
  sudo chown root:root /etc/apt/sources.list.d/microsoft-prod.list


2. Установка пакета SDK для .NET:

  sudo apt-get update; \
  sudo apt-get install -y apt-transport-https && \
  sudo apt-get update && \
  sudo apt-get install -y dotnet-sdk-5.0

3. Построение проекта Globe3DLight:

  * Перейдите к папке где расположен проект Globe3DLight:

  cd Globe3DLight

  * Запустите построение:

  dotnet build

4. Запуск проекта Globe3DLight:

  * Запуск пректа:

  dotnet run