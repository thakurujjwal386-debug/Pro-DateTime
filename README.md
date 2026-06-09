# Pro-DateTime `v1.0.0`

A lightweight C# library that adds **overridable timezone conversion** to `DateTime` and `DateTimeOffset`, backed by strongly-typed **timezone constants**.

---

## ✨ Features

- ✅ Convert `DateTime` / `DateTimeOffset` to any timezone using typed constants
- ✅ Fluent extension methods (`dateTime.ConvertToTimezone(...)`)
- ✅ Strongly-typed constants in `TimezoneConstant` — no more magic strings
- ✅ `virtual` methods on `ProDateTimeConverter` — override for custom logic or unit tests
- ✅ `IDateTimeConverter` interface for DI / mocking
- ✅ Supports both IANA and Windows timezone identifiers

---

## 📦 Installation

```bash
dotnet add package Pro-DateTime
```

---

## 🚀 Quick Start

```csharp
using ProDateTime;
using ProDateTime.Constants;
using ProDateTime.Extensions;

var converter = new ProDateTimeConverter();

// Convert UTC now → India Standard Time
DateTime ist = converter.ConvertToTimezone(DateTime.UtcNow, TimezoneConstant.AsiaKolkata);
Console.WriteLine(ist); // e.g. 2026-06-09 23:05:00

// Convert to Japan Standard Time
DateTime jst = converter.ConvertToTimezone(DateTime.UtcNow, TimezoneConstant.AsiaTokyo);

// Get current time in any timezone
DateTime ny = converter.NowIn(TimezoneConstant.AmericaNewYork);

// Fluent extension methods
DateTime london = DateTime.UtcNow.ConvertToTimezone(TimezoneConstant.EuropeLondon);
DateTime india  = DateTime.UtcNow.ToIndiaTime();

// DateTimeOffset support
DateTimeOffset dtoIst = DateTimeOffset.UtcNow.ConvertToTimezone(TimezoneConstant.AsiaKolkata);

// Get UTC offset
TimeSpan offset = converter.GetUtcOffset(TimezoneConstant.AsiaKolkata); // +05:30

// Get display name
string name = converter.GetTimezoneName(TimezoneConstant.AsiaKolkata);
// "(UTC+05:30) Chennai, Kolkata, Mumbai, New Delhi"
```

---

## 🌐 Available Timezone Constants

| Constant | Timezone | UTC Offset |
|---|---|---|
| `TimezoneConstant.AsiaKolkata` | India Standard Time | +05:30 |
| `TimezoneConstant.AsiaTokyo` | Japan Standard Time | +09:00 |
| `TimezoneConstant.AsiaShanghai` | China Standard Time | +08:00 |
| `TimezoneConstant.AsiaSingapore` | Singapore Standard Time | +08:00 |
| `TimezoneConstant.AsiaDubai` | Arabian Standard Time | +04:00 |
| `TimezoneConstant.AsiaKarachi` | Pakistan Standard Time | +05:00 |
| `TimezoneConstant.AsiaBangkok` | Indochina Time | +07:00 |
| `TimezoneConstant.AsiaSeoul` | Korea Standard Time | +09:00 |
| `TimezoneConstant.AmericaNewYork` | Eastern Time | -05:00/-04:00 |
| `TimezoneConstant.AmericaLosAngeles` | Pacific Time | -08:00/-07:00 |
| `TimezoneConstant.AmericaChicago` | Central Time | -06:00/-05:00 |
| `TimezoneConstant.EuropeLondon` | GMT / BST | +00:00/+01:00 |
| `TimezoneConstant.EuropeParis` | Central European Time | +01:00/+02:00 |
| `TimezoneConstant.EuropeMoscow` | Moscow Standard Time | +03:00 |
| `TimezoneConstant.AustraliaSydney` | Australian Eastern Time | +10:00/+11:00 |
| `TimezoneConstant.PacificAuckland` | New Zealand Standard Time | +12:00 |
| `TimezoneConstant.Utc` | UTC | +00:00 |
| _(+ many more...)_ | | |

---

## 🔧 Overriding Conversion Logic

Subclass `ProDateTimeConverter` and override `ResolveTimeZone` or `ConvertToTimezone`:

```csharp
public class NodaTimeConverter : ProDateTimeConverter
{
    protected override TimeZoneInfo ResolveTimeZone(string timezoneId)
    {
        // plug in NodaTime or any custom zone provider here
        return base.ResolveTimeZone(timezoneId);
    }
}
```

Use with DI:

```csharp
services.AddSingleton<IDateTimeConverter, ProDateTimeConverter>();
```

---

## 📁 Project Structure

```
Pro-DateTime/
  ProDateTimeConverter.cs       ← Main class (overridable)
  Constants/
    TimezoneConstant.cs         ← Strongly-typed timezone ids
  Interfaces/
    IDateTimeConverter.cs       ← DI/mock contract
  Extensions/
    DateTimeExtensions.cs       ← Fluent extension methods
```

---

## 📄 License

MIT © Vishal Rana
