## Linux

#### bu rehber ubuntu için hazırlanmıştır, farklı dağıtımlar kullanırken servis ve apt komutlarını ayarlayın. [diğer dağıtımlar için önemli notlar](https://github.com/Erder00/royale-brawl-v29/blob/main/docs/Linux.md#other-distros)

mysql ve dotnet'i paket yöneticiniz üzerinden yükleyin
```bash
sudo apt install mysql-server dotnet-sdk-8.0
```
depoyu klonlayın (gerekirse `sudo apt install git` ile git'i yükleyin)
```bash
git clone https://github.com/Erder00/royale-brawl-v29
```
doğru dizine gidin
```bash
cd royale-brawl-v29/
```
mysql'i başlatın
```bash
sudo service mysql start
```
mysql kabuğu
```bash
sudo mysql
```
mysql root şifresini ayarlayın
```bash
ALTER USER 'root'@'localhost' IDENTIFIED WITH caching_sha2_password BY 'ŞİFRENİZ';
```
yeni bir mysql veritabanı oluşturun
```bash
CREATE DATABASE veritabanıadı;
```
mysql'den çıkın
```bash
exit;
```
database.sql dosyasını içe aktarın
```bash
sudo mysql -u root -p veritabanıadı < database.sql
```
projeye gidin
```bash
cd src/Supercell.Laser.Server
```
projeyi derleyin
```bash
dotnet publish
```
şimdi derlenen dll'in bulunduğu dizine gidin
```bash
cd bin/Release/net8.0/
```
tercih ettiğiniz metin düzenleyici ile config dosyasını düzenleyin, örneğin vim:
```bash
vim config.json
```
`mysql_password`'ü ayarladığınız şifre ile ve `mysql_database`'i oluşturduğunuz veritabanı adı ile değiştirin. Ayrıca discord botunu kullanmak istiyorsanız `BotToken` ve `ChannelId`'yi de düzenleyin

son olarak sunucuyu çalıştırın
```bash
dotnet Supercell.Laser.Server.dll
```
![linux'ta sunucu](https://github.com/Erder00/royale-brawl-v29/blob/main/docs/screenshots/server-linux.png?raw=true)

# diğer dağıtımlar
- arch linux'ta (ki ben kişisel olarak bunu kullanıyorum, bu arada) mysql paketi pek iyi değil, benim için [distrobox](https://distrobox.it) üzerinden bir ubuntu konteynerinde mysql çalıştırmak en iyi çözüm oldu
- userland veya termux'ta (sanırım bu da linux sayılır, değil mi?) dotnet çalıştırmak başarısız olacaktır, `.bashrc` dosyanıza `export DOTNET_GCHeapHardLimit=1C0000000` eklemek veya terminalde çalıştırmak sorunu çözmelidir
