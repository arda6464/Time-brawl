## Android
1: [Github Actions](https://github.com/Erder00/royale-brawl-v29/actions) üzerinden alın

2: [Termux](https://github.com/termux/termux-app)'u yükleyin ve depolama izni verin
```
termux-setup-storage
```

3: Paketleri güncelleyin
```
pkg update && pkg upgrade
```

4: glibc ve mariadb'yi yükleyin
```
apt install glibc-repo wget mariadb
apt install glibc-runner libicu-glibc
```

5: mariadb'yi başlatın
```
mariadbd-safe --datadir='/data/data/com.termux/files/usr/var/lib/mysql'
```

6: yeni terminal oturumları açın ve [mariadb-secure-installation](https://mariadb.com/kb/en/mariadb-secure-installation/)'ı çalıştırın
```
mariadb-secure-installation
NOT: ÜRETİM ORTAMINDA KULLANILAN TÜM MariaDB SUNUCULARI İÇİN BU BETİĞİN
      TÜM BÖLÜMLERİNİN ÇALIŞTIRILMASI ÖNERİLİR! LÜTFEN HER ADIMI DİKKATLİCE OKUYUN!

MariaDB'ye güvenli bir şekilde giriş yapabilmek için root kullanıcısının mevcut
şifresine ihtiyacımız olacak. Eğer MariaDB'yi yeni yüklediyseniz ve henüz
root şifresini ayarlamadıysanız, burada sadece enter tuşuna basmanız yeterli.

Root kullanıcısı için mevcut şifreyi girin (yoksa enter'a basın):
Tamam, şifre başarıyla kullanıldı, devam ediliyor...

Root şifresini ayarlamak veya unix_socket kullanmak, kimsenin uygun yetkilendirme
olmadan MariaDB root kullanıcısına giriş yapamamasını sağlar.

Unix_socket kimlik doğrulamasını etkinleştir? [E/h] h
 ... atlanıyor.

Root şifresini ayarla? [E/h] e
Yeni şifre:
Şifreyi tekrar girin:
Şifre başarıyla güncellendi!
Ayrıcalık tabloları yeniden yükleniyor..
 ... Başarılı!


Varsayılan olarak, bir MariaDB kurulumunda anonim bir kullanıcı bulunur ve bu,
herhangi birinin kendileri için oluşturulmuş bir kullanıcı hesabına sahip olmadan
MariaDB'ye giriş yapmasına izin verir. Bu sadece test amaçlıdır ve kurulumun
biraz daha sorunsuz ilerlemesini sağlar. Üretim ortamına geçmeden önce
bunları kaldırmalısınız.

Anonim kullanıcıları kaldır? [E/h] e
 ... Başarılı!

Normalde, root sadece 'localhost'tan bağlantıya izin verilmelidir. Bu,
birinin ağ üzerinden root şifresini tahmin edememesini sağlar.

Root uzaktan girişini engelle? [E/h] e
 ... Başarılı!

Varsayılan olarak, MariaDB herkesin erişebileceği 'test' adında bir veritabanıyla gelir.
Bu da sadece test amaçlıdır ve üretim ortamına geçmeden önce kaldırılmalıdır.

Test veritabanını ve erişimini kaldır? [E/h] e
 - Test veritabanı siliniyor...
 ... Başarılı!
 - Test veritabanındaki ayrıcalıklar kaldırılıyor...
 ... Başarılı!

Ayrıcalık tablolarını yeniden yüklemek, şu ana kadar yapılan tüm değişikliklerin
hemen etkili olmasını sağlayacaktır.

Ayrıcalık tablolarını şimdi yeniden yükle? [E/h] e
 ... Başarılı!

Temizlik yapılıyor...

Hepsi tamamlandı! Yukarıdaki adımların tümünü tamamladıysanız, MariaDB
kurulumunuz artık güvenli olmalıdır.

MariaDB'yi kullandığınız için teşekkürler!
```

7: glibc runner'ı başlatın
```
grun -s
```

8: dotnet'i indirin ve yükleyin
```
wget https://download.visualstudio.microsoft.com/download/pr/501c5677-1a80-4232-9223-2c1ad336a304/867b5afc628837835a409cf4f465211d/dotnet-runtime-8.0.11-linux-arm64.tar.gz
mkdir .dotnet
tar xvf dotnet-runtime-8.0.11-linux-arm64.tar.gz -C .dotnet
grun -f .dotnet/dotnet
grun -c .dotnet/dotnet
```

9: Sunucuyu açın
```
mkdir Server
cd Server
unzip "zip dizini"
unzip Supercell.Laser.Server.1.0.0.zip
```

10: mariadb kabuğu
```
mariadb -u root -p
Şifre girin: root şifresi
CREATE DATABASE veritabanıadı;
exit;
```

11: [database.sql](../database.sql) dosyasını içe aktarın
```
wget https://github.com/Erder00/royale-brawl-v29/raw/refs/heads/main/database.sql
```
```
mariadb -u root -p"root şifresi" veritabanıadı < database.sql
```
veya
```
mariadb -u root -p veritabanıadı
Şifre girin: root şifresi
source database.sql
exit;
```

12: config.json dosyasını değiştirin
```
nano config.json
```
mysql_password'ü ayarladığınız şifre ile ve mysql_database'i oluşturduğunuz veritabanı adı ile değiştirin.
Control + x ve y ve Enter x2

### Çalıştırma
terminal 1
```
mariadbd-safe --datadir='/data/data/com.termux/files/usr/var/lib/mysql'
```
terminal 2
```
grun -s
cd Server
~/.dotnet/dotnet Supercell.Laser.Server.dll
```
