# Test Data Generator

Bu proje, unit testler için esnek ve genişletilebilir bir test verisi oluşturma altyapısı sunar. Adapter Pattern kullanarak farklı mock kütüphanelerini (Moq, Faker vb.) tek bir interface üzerinden yönetmeyi sağlar.

## 📋 Proje Yapısı

```
TestDataGenerator/
├── src/
│   ├── TestDataGenerator.Core/           # Core katmanı
│   │   ├── Extensions/                   
│   │   │   └── TestDataAdapterExtensions.cs
│   │   ├── Interfaces/
│   │   │   ├── ITestDataAdapter.cs
│   │   │   └── IRepository.cs
│   │   └── Models/
│   │       └── Product.cs
│   │
│   ├── TestDataGenerator.Adapters/       # Adapter implementasyonları
│   │   ├── Base/
│   │   │   └── BaseTestDataAdapter.cs
│   │   ├── Faker/
│   │   │   └── FakerTestDataAdapter.cs
│   │   └── Moq/
│   │       └── MoqTestDataAdapter.cs
│   │
│   └── TestDataGenerator.Demo/           # Örnek kullanımlar
│       └── Services/
│           └── ProductService.cs
│
└── tests/                                # Test projeleri
    └── TestDataGenerator.Tests/
        ├── Adapters/
        │   ├── FakerAdapterTests.cs
        │   └── MoqAdapterTests.cs
        └── Services/
            └── ProductServiceTests.cs
```

## 🚀 Başlangıç

### Gereksinimler
- .NET 7.0+
- Moq
- Bogus (Faker)
- xUnit

### Kurulum

```bash
dotnet add package Moq
dotnet add package Bogus
dotnet add package xunit
```

### Temel Kullanım

```csharp
// Moq adapter kullanımı
ITestDataAdapter<Product> moqAdapter = new MoqTestDataAdapter<Product>();
var product = moqAdapter.Generate();

// Faker adapter kullanımı
ITestDataAdapter<Product> fakerAdapter = new FakerTestDataAdapter<Product>();
var fakeProducts = fakerAdapter.GenerateMany(5);
```

## 🎯 Özellikler

### Adapter Extensions
```csharp
// Özelleştirilmiş veri oluşturma
var customProduct = adapter.GenerateWithCustomization(p => {
    p.Price = 999.99m;
    p.IsActive = true;
});

// Asenkron veri oluşturma
var asyncProduct = await adapter.GenerateAsync();
var asyncProducts = await adapter.GenerateManyAsync(5);
```

### Faker Adapter Özellikleri
```csharp
var fakerAdapter = (FakerTestDataAdapter<Product>)adapter;
var faker = fakerAdapter.GetFaker();

// Özel kurallar ekleme
faker.RuleFor(p => p.Price, f => f.Random.Decimal(100, 200));
```

### Moq Adapter Özellikleri
```csharp
var moqAdapter = (MoqTestDataAdapter<Product>)adapter;
var repository = moqAdapter.GetMockRepository();

// Repository davranışlarını özelleştirme
repository.Setup(r => r.Get(1)).Returns(new Product { Id = 1 });
```

## 🧪 Test Örnekleri

### Faker Tests
```csharp
[Fact]
public void Generate_Should_Create_Product_With_Valid_Data()
{
    var product = _adapter.Generate();
    Assert.NotNull(product);
    Assert.NotEqual(0, product.Id);
    Assert.NotEmpty(product.Name);
}
```

### Moq Tests
```csharp
[Fact]
public void MockRepository_Should_Track_Method_Calls()
{
    var moqAdapter = (MoqTestDataAdapter<Product>)_adapter;
    var repository = moqAdapter.GetMockRepository();
    
    var product = _adapter.Generate();
    _ = repository.Object.Get(1);
    
    repository.Verify(r => r.Get(It.IsAny<int>()), Times.Once());
}
```

## 🏗️ Design Patterns

1. **Adapter Pattern**
   - `ITestDataAdapter<T>` interface'i
   - Her mock kütüphanesi için ayrı adapter implementasyonu
   - Ortak bir API üzerinden farklı kütüphanelere erişim

2. **Template Method Pattern**
   - `BaseTestDataAdapter<T>` abstract sınıfı
   - Temel davranışların tanımlanması

## 📝 SOLID Prensipleri

1. **Single Responsibility (SRP)**
   - Her adapter tek bir kütüphaneyi yönetir
   - Extension methodları ayrı bir sınıfta

2. **Open/Closed (OCP)**
   - Yeni mock kütüphaneleri için yeni adapter'lar eklenebilir
   - Mevcut kod değiştirilmeden genişletilebilir

3. **Liskov Substitution (LSP)**
   - Tüm adapter'lar ITestDataAdapter'ı tam olarak implemente eder

4. **Interface Segregation (ISP)**
   - ITestDataAdapter interface'i minimal ve odaklı
   - Her adapter kendi özel metodlarını sunabilir

5. **Dependency Inversion (DIP)**
   - Yüksek seviye modüller soyutlamalara bağlı
   - Interface üzerinden iletişim

## 🔍 Best Practices

1. **Her zaman interface üzerinden çalışın**
```csharp
ITestDataAdapter<Product> adapter = new FakerTestDataAdapter<Product>();
```

2. **Extension methodlarını kullanın**
```csharp
var customProduct = adapter.GenerateWithCustomization(p => p.Price = 100);
```

3. **Mock repository'leri test başına özelleştirin**
```csharp
var mockRepo = moqAdapter.GetMockRepository();
mockRepo.Setup(r => r.Get(1)).Returns(specificProduct);
```

4. **Faker kurallarını test senaryosuna göre ayarlayın**
```csharp
var faker = fakerAdapter.GetFaker();
faker.RuleFor(p => p.Price, f => f.Random.Decimal(100, 200));
```

## 🤝 Katkıda Bulunma

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 Lisans

MIT License - Detaylar için [LICENSE.md](LICENSE.md) dosyasına bakın.
