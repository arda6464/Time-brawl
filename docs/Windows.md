## Windows

mysql'i [buradan](https://dev.mysql.com/downloads/installer/) indirin (web-community'yi öneriyorum, ama bu sadece kurulum türü)

- hesap gerekmez! "No thanks, just start my download." (Hayır teşekkürler, sadece indirmeyi başlat) seçeneğine tıklayın

kurulum programını çalıştırın ve "full" seçeneğini seçin, ardından her zaman execute veya next'e basarak kurulumu tamamlayın

sunucuyu [buradan](https://github.com/Erder00/royale-brawl-v29/archive/refs/heads/main.zip) indirin

dotnet'i [microsoft sitesinden](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) indirin ve yükleyin

royale-brawl-v29-main.zip dosyasını açın

mysql workbench'i açın ve "Local instance MySQL80"e tıklayın

![windows'ta veritabanı içe aktarma](https://github.com/Erder00/royale-brawl-v29/blob/main/docs/screenshots/db-win.png?raw=true)

"data import/restore"a tıklayın, ardından "import self-contained file"i seçin, database.sql dosyasını seçin. Sonra "new..."e tıklayın, veritabanınıza bir isim verin ve "start import"a tıklayın

/royale-brawl-v29/src/Supercell.Laser.Server dizininde bir terminal açın

projeyi derleyin
```powershell
dotnet publish
```

/bin/Release/net8.0 dizininde config.json dosyasını düzenleyin, `mysql_password`'ü ayarladığınız şifre ile ve `mysql_database`'i oluşturduğunuz veritabanı adı ile değiştirin. Ayrıca discord botunu kullanmak istiyorsanız `BotToken` ve `ChannelId`'yi de düzenleyin

binary dizinine gidin
cd bin\Release\net8.0

sunucuyu çalıştırın
```powershell
dotnet Supercell.Laser.Server.dll
```

![windows'ta sunucu](https://github.com/Erder00/royale-brawl-v29/blob/main/docs/screenshots/server-windows.png?raw=true)
