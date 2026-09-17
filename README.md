# KURT DHYLAN MOTO SHOP INVENTORY

Android-first .NET MAUI inventory + simple POS/order app.

## Included
- Product management: Brand, Model, Name, Price, Stocks
- Service/Labor management: Labor Name, Price
- Search Products
- Search Services
- Cart / order
- Payment + change calculation
- Checkout
- Receipt screen
- Stock automatically decreases after successful checkout
- SQLite local database for offline use
- Racing/red/black UI based on the provided sketches and logo

## Important: data persistence
Products and services are stored in SQLite under the app's `FileSystem.AppDataDirectory`.
Updating/reinstalling an APK normally preserves app data when Android treats it as an update to the same application ID. Do not uninstall/clear app data if you need to keep the inventory database.

## Build locally
Install the .NET 10 SDK and MAUI Android workload:

```bash
dotnet workload install maui-android
dotnet restore
dotnet build -f net10.0-android
```

## GitHub Actions
The workflow in `.github/workflows/build-android.yml` builds an unsigned debug APK and uploads it as an artifact.

For a Play Store/release APK, add a signing setup later.
