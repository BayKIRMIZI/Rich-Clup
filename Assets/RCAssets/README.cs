/// Oyunun türü runner ve swerve mekaniği ile (sağa sola) karakteri hareket ettirerek ilerliyoruz.
/// Başlangıçta 10K$ parayla fakir kıyafetler içerisinde başlıyoruz. Yoldan topladığımız paralar
/// ile karakterimizin kafasının üzerinde yazan para miktarı artıyor. Bu miktar ile orantılı
/// olarak karakterimizin giyimi değişiyor. (Hazır meshler değişiyor) Yol üzerinde bulduğumuz
/// yeşil para ve para çantası bize para kazandırırken, kırmızı para ve TAX (vergi) kağıdı bize
/// para kaybettiriyor. Önümüze yol üzerinde farklı karakterler çıkıyor. Bu karakterlerin
/// kafalarının üzerinde sahip oldukları para miktarı yazıyor. Paramız önümüze çıkan karakterden
/// fazlaysa tokat atıyor ve durmadan devam ediyor, az ise durup tokat yiyip kaybediyoruz.
/// Tokatlarsak paramız artıyor, tokat yersek paramız azalıyor. Kıyafetimiz sürekli para miktarımıza
/// göre değişiyor, yani zenginken fakir görünüme inebiliyoruz. Yediğimiz tokat paramızı 0a eşitler
/// veya sıfırdan az bir sayıya getirir ise kaybediyoruz. Finalde 5 basamaklı bir merdivenin her
/// basamağında bizi bekleyen karakterler var, bu karakterlerin tamamını tokatlarsak en tepede
/// durup dans ediyor, sonra bir sonraki bölüme geçiyoruz. Kaybedersek baştan başlıyoruz. Yol üzerinde
/// bulduğumuz yeşil para ve para çantası bize para kazandırırken, kırmızı para ve TAX (vergi)
/// kağıdı bize para kaybettiriyor.
///
/// DİKKAT EDİLMESİ GEREKEN NOKTALAR:
///
/// -Karakterlerin üzerindeki para balonu bizim paramıza göre azsa yeşil çoksa kırmızı oluyor
/// -Karakterlere tokat atarken durmuyoruz, yürümeye devam ediyoruz
/// -Karakterimizin meshi elimizdeki para miktarına göre değişiyor