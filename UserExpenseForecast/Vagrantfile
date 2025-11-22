Vagrant.configure("2") do |config|
  # -------- Перша VM: Ubuntu 20.04 --------
  config.vm.define "ubuntu" do |ubuntu|
    ubuntu.vm.box = "ubuntu/focal64"
    ubuntu.vm.hostname = "expense-ubuntu"
    ubuntu.vm.network "private_network", ip: "192.168.56.30"

    ubuntu.vm.provision "shell", inline: <<-SHELL
      echo "=== Ubuntu: оновлення пакетів ==="
      sudo apt-get update -y

      echo "=== Ubuntu: встановлення git та wget ==="
      sudo apt-get install -y git wget

      echo "=== Ubuntu: встановлення .NET SDK 8.0 ==="
      wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
      sudo dpkg -i packages-microsoft-prod.deb
      sudo apt-get update -y
      sudo apt-get install -y dotnet-sdk-8.0

      echo "=== Ubuntu: клонування репозиторію з GitHub ==="
      cd /home/vagrant
      git clone https://github.com/AnnaChekhfddddddddd/UserExpenseForecast1.git UserExpenseForecastRepo || (cd UserExpenseForecastRepo && git pull)

      echo "=== Ubuntu: збірка проєкту ==="
      cd /home/vagrant/UserExpenseForecastRepo
      dotnet build

      echo "=== Ubuntu: Збірка успішна. Запускати програму можна вручну через 'dotnet run'. ==="
    SHELL
  end

  # -------- Друга VM: Debian 11 --------
  config.vm.define "debian" do |debian|
    debian.vm.box = "debian/bullseye64"
    debian.vm.hostname = "expense-debian"
    debian.vm.network "private_network", ip: "192.168.56.31"

    debian.vm.provision "shell", inline: <<-SHELL
      echo "=== Debian: оновлення пакетів ==="
      sudo apt-get update -y

      echo "=== Debian: встановлення git та wget ==="
      sudo apt-get install -y git wget

      echo "=== Debian: встановлення .NET SDK 8.0 ==="
      wget https://packages.microsoft.com/config/debian/11/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
      sudo dpkg -i packages-microsoft-prod.deb
      sudo apt-get update -y
      sudo apt-get install -y dotnet-sdk-8.0

      echo "=== Debian: клонування репозиторію з GitHub ==="
      cd /home/vagrant
      git clone https://github.com/AnnaChekhfddddddddd/UserExpenseForecast1.git UserExpenseForecastRepo || (cd UserExpenseForecastRepo && git pull)

      echo "=== Debian: збірка проєкту ==="
      cd /home/vagrant/UserExpenseForecastRepo
      dotnet build

      echo "=== Debian: Збірка успішна. Запускати програму можна вручну через 'dotnet run'. ==="
    SHELL
  end
end
