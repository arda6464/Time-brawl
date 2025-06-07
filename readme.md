# Royale Brawl v29

29.258 sürümüne dayalı Brawl Stars özel sunucusu

## İstemciyi [buradan](https://mega.nz/file/ajRBxSLC#AQHc3MEfuEf9NHBlDMKuFfM2p7wyCT5nq-Ex_Hn5gFg) indirin
![Logo](https://github.com/arda6464/Time-brawl/blob/main/docs/screenshots/lobby.png?raw=true)


## Özellikler

- çevrimdışı savaşlar
- vip / premium sistemi
- hesap sistemi (supercell ID benzeri sistem)
- rapor sistemi
- discord entegrasyonu
- çevrimiçi oyun odaları
- arkadaşlar ve kulüpler
- brawl pass ve kupa yolu
- global, kulüp ve savaşçı sıralamaları
- rastgele etkinlikler
- içerik üretici kodları
- kulüplerde slash komutları
- anti-ddos koruması

#### discord bot komutları

- !help - tüm mevcut komutları gösterir
- !status - sunucu durumunu gösterir
- !ping - pong ile yanıt verir
- !unlockall - oyuncunun hesabındaki HER ŞEYİ açar (!unlockall [TAG])
- !givepremium - bir hesaba premium verir (!givepremium [TAG])
- !ban - bir hesabı yasaklar (!ban [TAG])
- !unban - bir hesabın yasağını kaldırır (!unban [TAG])
- !mute - bir oyuncuyu susturur (!mute [TAG])
- !unmute - bir oyuncunun susturmasını kaldırır (!unmute [TAG])
- !resetseason - sezonu sıfırlar
- !changename - bir oyuncunun adını değiştirir (!changename [TAG] [yeniAd])
- !reports - tüm raporlanan mesajların bağlantısını gönderir
- !userinfo - oyuncu bilgilerini gösterir (!userinfo [TAG])
- !changecredentials - bir hesabın kullanıcı adı/şifresini değiştirir (!changecredentials [TAG] [yeniKullanıcıAdı] [yeniŞifre])
- !settrophies - tüm savaşçılar için kupa sayısını ayarlar (!settrophies [TAG] [Kupalar])
- !addgems - bir oyuncuya elmas verir (!addgems [TAG] [Elmas])
- !givepremium - bir hesaba bir aylık premium verir (!givepremium [TAG])


#### kulüp komutları

- /help - tüm mevcut komutları listeler
- /status - sunucu durumunu gösterir

## Kurulum

gereksinimler:

[dotnet 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
[mysql](https://dev.mysql.com/downloads/)

Nasıl kurulur:

[Android](https://github.com/arda6464/Time-brawl/blob/main/docs/Android.md)
[Linux](https://github.com/arda6464/Time-brawl/blob/main/docs/Linux.md)
[Windows](https://github.com/arda6464/Time-brawl/blob/main/docs/Windows.md)

Ardından [hazır istemciyi](https://mega.nz/file/ajRBxSLC#AQHc3MEfuEf9NHBlDMKuFfM2p7wyCT5nq-Ex_Hn5gFg) kullanarak sunucuya bağlanın

apk'yı decompile edin ve `lib/armeabi-v7a/liberder.script.so` dosyasındaki ip adresini kendi ip adresinizle değiştirin

## Teşekkürler

 - [xeon](https://git.xeondev.com/xeon) ve Erder'in [royale brawl](https://github.com/Erder00/royale-brawl) projesine dayanmaktadır
 - discord işlemleri için [netcord](https://netcord.dev) kullanılmaktadır (gerçekten harika, bir göz atın)
 - [spz](https://github.com/spz2020), [santer](https://github.com/SANS3R66) ve [8Hacc](https://github.com/8-bitHacc)'a pull request'leri için teşekkürler <3

## Yapılacaklar

- daha iyi anti-hile (script içinde csv sha'larını ve apk bütünlüğünü kontrol et, çok kısa süren savaşları tespit et)
- kripto
- daha fazla kulüp komutu ekle

