# Time Brawl v29

29.258 sürümüne dayalı Brawl Stars özel sunucusu

## İstemciyi [buradan](https://mega.nz/file/ajRBxSLC#AQHc3MEfuEf9NHBlDMKuFfM2p7wyCT5nq-Ex_Hn5gFg) indirin
![Logo](https://github.com/arda6464/Time-brawl/blob/main/docs/screenshots/lobby.jpg?raw=true)


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

- !yardım - tüm mevcut komutları gösterir
- !admin - admin komutları gösterir
- !status - sunucu durumunu gösterir
- !ping - pong ile yanıt verir
- !unlockall - oyuncunun hesabındaki HER ŞEYİ açar (!unlockall [TAG])
- !unlockskins - oyuncunun tüm kostümlerini açar (!unlockskins [TAG])
- !givepremium - bir hesaba premium verir (!givepremium [TAG])
- !removepremium - bir hesaptan premium'i kaldırır (!removepremium [TAG])
- !ban - bir hesabı yasaklar (!ban [TAG])
- !unban - bir hesabın yasağını kaldırır (!unban [TAG])
- !banip - bir IP adresini yasaklar (!banip [IP])
- !unbanip - bir IP adresinin yasağını kaldırır (!unbanip [IP])
- !mute - bir oyuncuyu susturur (!mute [TAG])
- !unmute - bir oyuncunun susturmasını kaldırır (!unmute [TAG])
- !resetseason - sezonu sıfırlar
- !isimdegistir - bir oyuncunun adını değiştirir (!isimdegistir [TAG] [yeniAd])
- !reports - tüm raporlanan mesajların bağlantısını gönderir
- !userinfo - oyuncu bilgilerini gösterir (!userinfo [TAG])
- !iddegis - bir hesabın kullanıcı adı/şifresini değiştirir (!iddegis [TAG] [yeniKullanıcıAdı] [yeniŞifre])
- !settrophies - tüm savaşçılar için kupa sayısını ayarlar (!settrophies [TAG] [Kupalar])
- !addtrophies - tüm savaşçılara kupa ekler (!addtrophies [TAG] [Kupalar])
- !addgems - bir oyuncuya elmas verir (!addgems [TAG] [Elmas])
- !removegems - bir oyuncudan elmas alır (!removegems [TAG] [Elmas])
- !gemsall - tüm oyunculara elmas verir (!gemsall [Miktar] [Mesaj])
- !liderlik - liderlik tablosunu gösterir
- !startevent - bir etkinliği başlatır (!startevent [TAG])
- !event - etkinlik durumunu gösterir (!event [TAG])
- !addteklif - yeni bir mağaza teklifi ekler (!addteklif [Başlık] [SüreGün] [ÜrünTürü] [Miktar] [Fiyat])
- !deleteclub - bir kulübü siler (!deleteclub [KulüpTAG])
- !bildirimall - tüm oyunculara bildirim gönderir (!bildirimall [Mesaj])
- !ozelmesaj - özel bir mesaj gönderir (!ozelmesaj [Mesaj])
- !popupall - tüm oyunculara popup gönderir
- !kapatmesaj - sunucu kapatma mesajı gönderir (!kapatmesaj [Mesaj])
- !kayıt - yeni bir hesap kaydeder (!kayıt [TAG])
- !hesabım - hesap bilgilerini gösterir (!hesabım [TAG])
(komutların çoğu hatalı olabilir ve yorum satırına alınmış olabilir lütfen ellemeyin.)

#### kulüp komutları

- /help - tüm mevcut komutları listeler
- /status - sunucu durumunu gösterir
- /kulüpad - kulüp adını değiştirir
- /register - kayıt olur
- /login hesaba giriş yapar

## Kurulum

gereksinimler:

[dotnet 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
[mysql](https://dev.mysql.com/downloads/)

Nasıl kurulur:

[Android](https://github.com/arda6464/Time-brawl/blob/main/docs/Android.md)
[Linux](https://github.com/arda6464/Time-brawl/blob/main/docs/Linux.md)
[Windows](https://github.com/arda6464/Time-brawl/blob/main/docs/Windows.md)

Ardından [hazır istemciyi](https://mega.nz/file/r8ZUjbSY#AdUgm2Br2_SYGZDlGhY1V7zaWih55ulhNmzedf7dg4A) kullanarak sunucuya bağlanın

apk'yı decompile edin ve `lib/armeabi-v7a/liberder.script.so` dosyasındaki ip adresini kendi ip adresinizle değiştirin

## Teşekkürler

 - [xeon](https://git.xeondev.com/xeon) ve Erder'in [royale brawl](https://github.com/Erder00/royale-brawl) projesine dayanmaktadır
 - discord işlemleri için [netcord](https://netcord.dev) kullanılmaktadır (gerçekten harika, bir göz atın)

## Yapılacaklar

- daha iyi anti-hile (script içinde csv sha'larını ve apk bütünlüğünü kontrol et, çok kısa süren savaşları tespit et)
- kripto
- daha fazla kulüp komutu ekle

